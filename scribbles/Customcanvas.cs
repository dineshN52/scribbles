using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.Linq;
using Scribbles;
using System.IO;
using System.Text;
using System;

namespace scribbles;

public partial class MyCanvas : Canvas {
   Scribble mScr = new ();
   CustomRectangle mRect = new ();
   CustomLine mLine = new ();
   ConnectedLine mConnectedLine = new ();
   public List<Shapes> mShapes = new ();
   Stack<Shapes> mUndoShapes = new ();

   bool IsErase = false;
   bool IsRect = false;
   bool IsScribble = true;
   bool IsLine = false;
   bool IsConnectedLine = false;

   public List<Shapes> mAllShapes => mShapes;

   public void ScribbleOn () {
      IsScribble = true; IsErase = false; IsRect = false; IsLine = false; IsConnectedLine = false;
   }
   public void LineOn () {
      IsLine = true; IsErase = false; IsScribble = false; IsRect = false; IsConnectedLine = false;
   }

   public void RectOn () {
      IsRect = true; IsErase = false; IsLine = false; IsScribble = false; IsConnectedLine = false;
   }

   public void ConnectedLineOn () {
      IsConnectedLine = true; IsErase = false; IsRect = false; IsScribble = false; IsLine = false;
   }

   public void EraseOn () {
      IsErase = true; IsScribble = true; IsRect = false; IsLine = false; IsConnectedLine = false;
   }

   protected override void OnRender (DrawingContext dc) {
      base.OnRender (dc);
      foreach (var shape in mShapes) {
         switch (shape) {
            case Scribble:
               Scribble? scr = shape as Scribble;
               for (int i = 0; i < scr.mPoints.Count - 1; i++)
                  dc.DrawLine (scr.mPen, scr.mPoints[i], scr.mPoints[i + 1]);
               break;
            case CustomLine:
               CustomLine? line = shape as CustomLine;
               dc.DrawLine (line.mPen, line.X, line.Y);
               break;
            case CustomRectangle:
               CustomRectangle? rectangle = shape as CustomRectangle;
               Rect r = new(rectangle.mStartpoint, rectangle.mEndPoint);
               dc.DrawRectangle (Background, rectangle.mPen, r);
               break;
              //case ConnectedLine:
              //   ConnectedLine? Cline = shape as ConnectedLine;
              //   for (int i = 0; i < Cline.mPoints.Count - 1; i++)
              //      dc.DrawLine (Cline.mPen, Cline.mPoints[i].X , Cline.mPoints[i].Y);
              //   break;
         }
      }
   }

   protected override void OnMouseDown (MouseButtonEventArgs e) {
      base.OnMouseDown (e);
      if (e.LeftButton == MouseButtonState.Pressed) {
         if (IsScribble) {
            mShapes.Add (mScr);
            mScr.mPen = IsErase ? new Pen (Brushes.White, 10) : new (Brushes.Black, 1);
            mScr.mPoints.Add (e.GetPosition (this));
         }
         if (IsRect) {
            mShapes.Add (mRect);
            mRect.mPen = new Pen (Brushes.Black, 1);
            mRect.mStartpoint = e.GetPosition (this);
         }
         if (IsLine) {
            mShapes.Add (mLine);
            mLine.mPen = new Pen (Brushes.Black, 1);
            mLine.X = e.GetPosition (this);
         }
         if (IsConnectedLine) {
            //mShapes.Add (mConnectedLine);
            //mConnectedLine.mPen = new Pen (Brushes.Black, 1);
            //mConnectedLine.mLine.mStartpoint = e.GetPosition (this);
         }
      }
   }

   protected override void OnMouseMove (MouseEventArgs e) {
      base.OnMouseMove (e);
      if (e.LeftButton != MouseButtonState.Pressed) {
         //if (IsConnectedLine && mConnectedLine.mLines != null) {
         //   mConnectedLine.mLines.Y = e.GetPosition (this);
         //   InvalidateVisual ();
         //}
         return;
      }
      if (IsScribble) {
         mScr.mPoints.Add (e.GetPosition (this));
         InvalidateVisual ();
      }
      if (IsRect) {
         mRect.mEndPoint = e.GetPosition (this);
         InvalidateVisual ();
      }
      if (IsLine) {
         mLine.Y = e.GetPosition (this);
         InvalidateVisual ();
      }
      if (IsConnectedLine) {
         //mConnectedLine.mPoints.Add (mConnectedLine.mLines);
         //mConnectedLine.mLines = new ();
         //InvalidateVisual ();
      }
   }

   protected override void OnMouseLeftButtonUp (MouseButtonEventArgs e) {
      base.OnMouseLeftButtonUp (e);
      if (e.LeftButton != MouseButtonState.Released) return;
      if (IsScribble) mScr = new ();
      if (IsLine) mLine = new ();
      if (IsRect) mRect = new ();
   }

   public void Undo () {
      if (mShapes.Count == 0) return;
      mUndoShapes.Push (mShapes.Last ());
      mShapes.RemoveAt (mShapes.Count - 1);
      InvalidateVisual ();
   }

   public void Redo () {
      if (mUndoShapes.Count == 0) return;
      mShapes.Add (mUndoShapes.Pop ());
      InvalidateVisual ();
   }

   public BinaryWriter BianryWriter (ref BinaryWriter bw) {
      foreach (var file in mShapes) {
         switch (file) {
            case Scribble scr:
               bw.Write (1);
               string b = scr.mPen.Brush.ToString ();
               bw.Write (b);
               bw.Write (scr.mPoints.Count);
               foreach (var point in scr.mPoints) {
                  bw.Write (point.X);
                  bw.Write (point.Y);
               }
               break;
            case CustomLine line:
               bw.Write (2);
               string c = line.mPen.Brush.ToString ();
               bw.Write (c);
               bw.Write (line.X.X);
               bw.Write (line.X.Y);
               bw.Write (line.Y.X);
               bw.Write (line.Y.Y);
               break;
            case CustomRectangle rect:
               bw.Write (3);
               string d = rect.mPen.Brush.ToString ();
               bw.Write (d);
               bw.Write (rect.mStartpoint.X);
               bw.Write (rect.mStartpoint.Y);
               bw.Write (rect.mEndPoint.X);
               bw.Write (rect.mEndPoint.Y);
               break;
         }
      }
      return bw;
   }

   public void TextOpen (string[] allShapes) {
      int limits = 0; mShapes.Clear ();
      for (int i = 0; i < allShapes.Length; i = limits) {
         string s = allShapes[i];
         switch (s) {
            case "Scribble":
               Scribble scr = new ();
               scr.mPen = new Pen ((Brush)new BrushConverter ().ConvertFrom (allShapes[i + 1]), int.Parse (allShapes[i + 2]));
               limits = (limits + 4) + int.Parse (allShapes[i + 3]);
               for (int j = i + 4; j < limits; j++)
                  scr.mPoints.Add (Point.Parse (allShapes[j]));
               mShapes.Add (scr);
               InvalidateVisual ();
               break;
            case "CustomLine":
               CustomLine l = new ();
               l.mPen = new Pen ((Brush)new BrushConverter ().ConvertFrom (allShapes[i + 1]), int.Parse (allShapes[i + 2]));
               limits += 5;
               l.X = Point.Parse (allShapes[i + 3]);
               l.Y = Point.Parse (allShapes[i + 4]);
               mShapes.Add (l);
               InvalidateVisual ();
               break;
            case "CustomRectangle":
               CustomRectangle rect = new ();
               rect.mPen = new Pen ((Brush)new BrushConverter ().ConvertFrom (allShapes[i + 1]), int.Parse (allShapes[i + 2]));
               limits += 5;
               rect.mStartpoint = Point.Parse (allShapes[i + 3]);
               rect.mEndPoint = Point.Parse (allShapes[i + 4]);
               mShapes.Add (rect);
               InvalidateVisual ();
               break;
         }
      }
   }

   public void BinaryOpen (byte[] allBytes) {
      int limits = 0; mShapes.Clear ();
      for (int i = 0; i < allBytes.Length; i = limits) {
         byte[] shape = new byte[4]; byte[] color = new byte[10];
         Array.Copy (allBytes, i, shape, 0, 4);
         Array.Copy (allBytes, i + 4, color, 0, 10);
         int s = BitConverter.ToInt32 (shape, 0);
         switch (s) {
            case 1:
               byte[] count = new byte[4];
               Array.Copy (allBytes, i + 14, count, 0, 4);
               Scribble scr = new ();
               string ss = System.Text.Encoding.Default.GetString (color);
               Color sc = (Color)ColorConverter.ConvertFromString (ss);
               Brush sb = new SolidColorBrush (sc);
               int thickness = sc == Color.FromRgb (255, 255, 255) ? 10 : 1;
               scr.mPen = new Pen (sb, thickness);
               limits = (limits + 18) + (BitConverter.ToInt32 (count, 0) * 16);
               for (int j = i + 18; j < limits; j += 16) {
                  byte[] x = new byte[8]; byte[] y = new byte[8];
                  Array.Copy (allBytes, j, x, 0, 8);
                  Array.Copy (allBytes, j + 8, y, 0, 8);
                  Point p = new (BitConverter.ToDouble (x), BitConverter.ToDouble (y));
                  scr.mPoints.Add (p);
               }
               mShapes.Add (scr);
               InvalidateVisual ();
               break;
            case 2:
               CustomLine cL = new ();
               string ls = System.Text.Encoding.Default.GetString (color);
               Color lc = (Color)ColorConverter.ConvertFromString (ls);
               Brush lb = new SolidColorBrush (lc);
               cL.mPen = new Pen (lb, 1);
               limits += 46;
               for (int j = i + 14; j < limits; j += 32) {
                  byte[] x1 = new byte[8]; byte[] y1 = new byte[8]; byte[] x2 = new byte[8]; byte[] y2 = new byte[8];
                  Array.Copy (allBytes, j, x1, 0, 8);
                  Array.Copy (allBytes, j + 8, y1, 0, 8);
                  Array.Copy (allBytes, j + 16, x2, 0, 8);
                  Array.Copy (allBytes, j + 24, y2, 0, 8);
                  Point X = new (BitConverter.ToDouble (x1), BitConverter.ToDouble (y1));
                  Point Y = new (BitConverter.ToDouble (x2), BitConverter.ToDouble (y2));
                  cL.X = X; cL.Y = Y;
               }
               mShapes.Add (cL);
               InvalidateVisual ();
               break;
            case 3:
               CustomRectangle r = new ();
               string rs = System.Text.Encoding.Default.GetString (color);
               Color rc = (Color)ColorConverter.ConvertFromString (rs);
               Brush rb = new SolidColorBrush (rc);
               r.mPen = new Pen (rb, 1);
               limits += 46;
               for (int j = i + 14; j < limits; j += 32) {
                  byte[] startX = new byte[8]; byte[] startY = new byte[8]; byte[] endX = new byte[8]; byte[] endY = new byte[8];
                  Array.Copy (allBytes, j, startX, 0, 8);
                  Array.Copy (allBytes, j + 8, startY, 0, 8);
                  Array.Copy (allBytes, j + 16, endX, 0, 8);
                  Array.Copy (allBytes, j + 24, endY, 0, 8);
                  Point X = new (BitConverter.ToDouble (startX), BitConverter.ToDouble (startY));
                  Point Y = new (BitConverter.ToDouble (endX), BitConverter.ToDouble (endY));
                  r.mStartpoint = X; r.mEndPoint = Y;
               }
               mShapes.Add (r);
               InvalidateVisual ();
               break;
         }
      }
   }
}
