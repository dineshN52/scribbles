using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.Linq;
using Scribbles;

namespace scribbles;

public partial class MyCanvas : Canvas {
   Scribble mScr = new ();
   CustomRectangle mRect = new ();
   CustomLine mLine = new ();
   public List<Shapes> mShapes = new ();
   Stack<Shapes> mUndoShapes = new ();
   ConnectedLine mConnectedLine = new ();
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
      if(e.LeftButton == MouseButtonState.Pressed) {
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
}
