using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;
using System.Windows.Ink;

namespace scribbles;

internal partial class MyCanvas : Canvas {
   System.Windows.Point mCurrentpoint = new ();
   readonly List<System.Windows.Point> mPoints = new ();
   List<int> mIndices = new ();
   List<(System.Windows.Point,bool)> mDStrokes = new ();
   Stack<List<(System.Windows.Point,bool)>> undoStrokes = new ();
   public bool IsErase { get; set; }
   public Stack<List<System.Windows.Point>> Strokes {
      get {
         Stack<List<System.Windows.Point>> newStack = new ();
         //Stack<List<(System.Windows.Point,bool)>> strokes = new ();
         //foreach (var s in mDStrokes)
         //   strokes.Push (s);
         //int c = strokes.Count;
         //for (int i = 0; i < c; i++) {
         //   List<(System.Windows.Point,bool)> points = strokes.Pop ();
         //   List<System.Windows.Point> l = new ();
         //   for (int j = 0; j < points.Count - 1; j++) {
         //      l.Add (points[j].Item1);
         //   }
         //   newStack.Push (l);
         //}
         return newStack;
      }  
   }
  
   protected override void OnRender (DrawingContext dc) {
      base.OnRender (dc);
      Pen pen = new ();
      if (mPoints.Count > 1) {
         pen = IsErase ? new (Brushes.White, 1) : new (Brushes.Black, 1);
         for (int i = 0; i < mPoints.Count - 1; i++)
            dc.DrawLine (pen, mPoints[i], mPoints[i + 1]);
      }
      if (mDStrokes.Count >1) {
         pen = new (Brushes.Black, 1);
         List<(System.Windows.Point,bool)> strokes = new ();
         foreach (var s in mDStrokes)
            strokes.Add (s);
         int c = mIndices.Count;
         if (c > 0) {
            DrawStreak (0, mIndices[0]);
            for (int i = 1; i < c; i++)
               DrawStreak (mIndices[i-1]+1, mIndices[i]);
         }    
      }
      void DrawStreak(int a,int b) {
         for (int j = a; j < b ; j++) {
            if (mDStrokes[j].Item2) pen = new (Brushes.White, 1);
            dc.DrawLine (pen, mDStrokes[j].Item1, mDStrokes[j + 1].Item1);
         }
      }
   }


   protected override void OnMouseDown (MouseButtonEventArgs e) {
      if (e.LeftButton == MouseButtonState.Pressed) {
         mCurrentpoint = e.GetPosition (this);
         if (IsErase) {
            if (mDStrokes.Contains ((mCurrentpoint, false))) {
               int a = mDStrokes.IndexOf ((mCurrentpoint, false));
               mDStrokes[a] = (mCurrentpoint, true);
            }
         } else {
            mDStrokes.Add ((mCurrentpoint, IsErase));
            mPoints.Add (mCurrentpoint);
         }
      }
   }

   protected override void OnMouseMove (MouseEventArgs e) {
      if (e.LeftButton != MouseButtonState.Pressed) return;
      System.Windows.Point p = e.GetPosition (this);
      if (IsErase) {
         if (mDStrokes.Contains ((p, false))) {
            int a = mDStrokes.IndexOf ((p, false));
            mDStrokes[a] = (p, true);
         }
      } else {
         mDStrokes.Add ((p, IsErase));
         mPoints.Add (p);
      }
      InvalidateVisual ();
   }

   protected override void OnMouseLeftButtonUp (MouseButtonEventArgs e) {
      if (e.ButtonState == MouseButtonState.Released) {
         mPoints.Clear ();
         InvalidateVisual ();
         mIndices.Add (mDStrokes.Count - 1);
         InvalidateVisual ();
      }
     
   }

   //public void Undo () {
   //   if (mDStrokes.Count == 0) return;
   //   undoStrokes.Push (mDStrokes.Pop ());
   //   InvalidateVisual ();
   //}
   //public void Redo () {
   //   if (undoStrokes.Count == 0) return;
   //   mDStrokes.Push (undoStrokes.Pop ());
   //   InvalidateVisual ();
   //}
}


