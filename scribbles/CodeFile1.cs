using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

namespace scribbles;

public partial class MyCanvas : Canvas {
   Pen mPen = new (Brushes.Black, 10);
   Scribble mScr = new ();
   Rectangle mRect = new ();
   Line mLine = new ();
   //ConnectedLine mConnectedLine = new ();
   Stack<Shapes> mShapes = new ();
   Stack<Shapes> mUndoShapes = new ();
   //public Stack<List<System.Windows.Point>> Strokes {
   //   get {
   //      Stack<List<System.Windows.Point>> s = new ();
   //      return s;
   //   }
   //}
   public bool IsErase { get; set; }
   bool IsRect = false;
   bool IsScribble = true;

   public void ScribbleOn () {
      IsScribble = true; IsRect = false;

   }
   public void LineOn () {

   }
   public void RectOn () {

   }

   public void ConnectedLineOn () {

   }
   protected override void OnRender (DrawingContext dc) {
      base.OnRender (dc);

      foreach(var shape in mShapes) {
         if (shape is Scribble scr) {
            for (int i = 0; i < scr.mPoints.Count-1; i++) {
               dc.DrawLine (new Pen(Brushes.Black,10), scr.mPoints[i], scr.mPoints[i + 1]);
            }
         }
      }

      //if (IsScribble) {
      //   if (mScr.mPoints.Count > 1) {
      //      Pen pen = new (Brushes.Black, 1);
      //      if (IsErase)
      //         pen = new (Brushes.White, 1);
      //      for (int i = 0; i < mScr.mPoints.Count - 1; i++)
      //         dc.DrawLine (pen, mScr.mPoints[i], mScr.mPoints[i + 1]);
      //   }
      //}
      //foreach (var shape in mShapes)
      //   switch (shape) {
      //      case Scribble:
      //         Pen pen = new ();
      //         //if (mScr.mDStrokes != null) {
      //         //   Stack<(List<System.Windows.Point>, bool)> strokes = new ();
      //         //   foreach (var s in mScr.mDStrokes)
      //         //      strokes.Push (s);
      //         //   int c = strokes.Count;
      //         //   for (int i = 0; i < c; i++) {
      //         //      (List<System.Windows.Point>, bool) points = strokes.Pop ();
      //         //      pen = points.Item2 ? new (Brushes.White, 1) : new (Brushes.Black, 1);
      //         //      for (int j = 0; j < points.Item1.Count - 1; j++)
      //         //         dc.DrawLine (pen, points.Item1[j], points.Item1[j + 1]);
      //         //   }
      //         //}
               
      //         break;
      //   }
   }

   protected override void OnMouseDown (MouseButtonEventArgs e) {
      base.OnMouseDown (e);
      if (IsScribble && e.LeftButton == MouseButtonState.Pressed) {
         //mScr.mCurrentpoint = e.GetPosition (this);
         mScr.mPoints.Add (e.GetPosition (this));
      }
   }
   protected override void OnMouseMove (MouseEventArgs e) {
      base.OnMouseMove (e);
      if (IsScribble) {
         if (e.LeftButton != MouseButtonState.Pressed) return;
         System.Windows.Point p = e.GetPosition (this);
         mScr.mPoints.Add (p);
         InvalidateVisual ();
      }
   }
   protected override void OnMouseLeftButtonUp (MouseButtonEventArgs e) {
      base.OnMouseLeftButtonUp (e);
      if (IsScribble) {
         if (e.ButtonState == MouseButtonState.Released) {
            //List<System.Windows.Point> p = new ();
            //foreach (var element in mScr.mPoints)
            //   p.Add (element);
            //mScr.mDStrokes.Add ((p, IsErase));
            //mScr.mPoints.Clear ();
            mShapes.Push (mScr);
            InvalidateVisual ();
            mScr = new ();
         }
      }
   }

   public void Undo () {
      if (mShapes.Count == 0) return;
      mUndoShapes.Push (mShapes.Pop ());
      InvalidateVisual ();
   }
   public void Redo () {
      if (mUndoShapes.Count == 0) return;
      mShapes.Push (mUndoShapes.Pop ());
      InvalidateVisual ();
   }
}
public class Shapes {
  
}
public class Scribble : Shapes {
   
   //public System.Windows.Point mCurrentpoint { get; set; }
   public List<Point> mPoints = new ();
   //public List<(List<System.Windows.Point>, bool)> mDStrokes = new ();
}
public class Line : Shapes {

}
public class Rectangle : Shapes {

}
public class ConnectedLine: Shapes {

}