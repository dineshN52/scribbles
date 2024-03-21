using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.Linq;
using System;

namespace scribbles;

public partial class MyCanvas : Canvas {

   public List<Shapes> AllShapes => mShapes;

   public void ScribbleOn () => mCurrentShape = new Scribble ();

   public void LineOn () => mCurrentShape = new CustomLine ();

   public void RectOn () => mCurrentShape = new CustomRectangle ();

   public void CircleOn () => mCurrentShape = new CustomCircle ();

   protected override void OnRender (DrawingContext dc) {
      base.OnRender (dc);
      foreach (var shape in mShapes) {
         switch (shape) {
            case Scribble scr:
               Color sc = (Color)ColorConverter.ConvertFromString (scr.color);
               Brush sb = new SolidColorBrush (sc);
               Pen sPen = new (sb, scr.Thickness);
               for (int i = 0; i < scr.Points.Count - 1; i++)
                  dc.DrawLine (sPen, new Point (scr.Points[i].X, scr.Points[i].Y), new Point (scr.Points[i + 1].X, scr.Points[i + 1].Y));
               break;
            case CustomLine line:
               Color lc = (Color)ColorConverter.ConvertFromString (line.color);
               Brush lb = new SolidColorBrush (lc);
               Pen lPen = new (lb, line.Thickness);
               dc.DrawLine (lPen, new Point (line.X.X, line.X.Y), new Point (line.Y.X, line.Y.Y));
               break;
            case CustomRectangle rectangle:
               Color rc = (Color)ColorConverter.ConvertFromString (rectangle.color);
               Brush rb = new SolidColorBrush (rc);
               Pen rPen = new (rb, rectangle.Thickness);
               CustomPoint A = rectangle.mStartpoint; CustomPoint B = rectangle.mEndPoint;
               Rect r = new (new Point (A.X, A.Y), new Point (B.X, B.Y));
               dc.DrawRectangle (Background, rPen, r);
               break;
            case CustomCircle circle:
               Color cc = (Color)ColorConverter.ConvertFromString (circle.color);
               Brush cb = new SolidColorBrush (cc);
               Pen cPen = new (cb, circle.Thickness);
               CustomPoint C = circle.centre; double D = circle.radius;
               dc.DrawEllipse (Background, cPen, new Point (C.X, C.Y), D, D);
               break;
         }
      }
   }

   protected override void OnMouseDown (MouseButtonEventArgs e) {
      base.OnMouseDown (e);
      if (e.LeftButton == MouseButtonState.Pressed) {
         switch (mCurrentShape) {
            case Scribble sr:
               mShapes.Add (sr);
               if (Colors.ShapeColor != null) sr.color = Colors.ShapeColor;
               else sr.color = "#000000";
               sr.Thickness = 1;
               sr.Points.Add (new CustomPoint (e.GetPosition (this).X, e.GetPosition (this).Y));
               break;
            case CustomRectangle rect:
               mShapes.Add (rect);
               if (Colors.ShapeColor != null) rect.color = Colors.ShapeColor;
               else rect.color = "#000000";
               rect.Thickness = 1;
               rect.mStartpoint = new CustomPoint (e.GetPosition (this).X, e.GetPosition (this).Y);
               break;
            case CustomLine line:
               mShapes.Add (line);
               if (Colors.ShapeColor != null) line.color = Colors.ShapeColor;
               else line.color = "#000000";
               line.Thickness = 1;
               line.X = new CustomPoint (e.GetPosition (this).X, e.GetPosition (this).Y);
               break;
            case CustomCircle circle:
               mShapes.Add (circle);
               if (Colors.ShapeColor != null) circle.color = Colors.ShapeColor;
               else circle.color = "#000000";
               circle.Thickness = 1;
               circle.centre = new CustomPoint (e.GetPosition (this).X, e.GetPosition (this).Y);
               break;
         }
      }

   }

   protected override void OnMouseMove (MouseEventArgs e) {
      base.OnMouseMove (e);
      if (e.LeftButton != MouseButtonState.Pressed) return;
      switch (mCurrentShape) {
         case Scribble sr:
            sr.Points.Add (new CustomPoint (e.GetPosition (this).X, e.GetPosition (this).Y));
            InvalidateVisual ();
            break;
         case CustomRectangle rect:
            rect.mEndPoint = new CustomPoint (e.GetPosition (this).X, e.GetPosition (this).Y);
            InvalidateVisual ();
            break;
         case CustomLine line:
            line.Y = new CustomPoint (e.GetPosition (this).X, e.GetPosition (this).Y);
            InvalidateVisual ();
            break;
         case CustomCircle circle:
            circle.radius = Math.Sqrt ((circle.centre.X - e.GetPosition (this).X) * (circle.centre.X - e.GetPosition (this).X) +
               (circle.centre.Y - e.GetPosition (this).Y) * (circle.centre.Y - e.GetPosition (this).Y));
            InvalidateVisual ();
            break;
      }
   }

   protected override void OnMouseLeftButtonUp (MouseButtonEventArgs e) {
      base.OnMouseLeftButtonUp (e);
      if (e.LeftButton != MouseButtonState.Released) return;
      switch (mCurrentShape) {
         case Scribble:
            mCurrentShape = new Scribble (); break;
         case CustomRectangle:
            mCurrentShape = new CustomRectangle (); break;
         case CustomLine:
            mCurrentShape = new CustomLine (); break;
         case CustomCircle:
            mCurrentShape = new CustomCircle (); break;
      }
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

   public void OpenDocument () {
      mShapes.Clear ();
      mShapes = FileManager.OpenFile ();
      InvalidateVisual ();
   }

   #region Private data--------
   public Shapes mCurrentShape = new Scribble ();
   private List<Shapes> mShapes = new ();
   private Stack<Shapes> mUndoShapes = new ();
   public string? mShapeColor = Colors.ShapeColor;
   #endregion 
}
