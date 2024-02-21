using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace scribbles;

internal partial class MyCanvas : Canvas {
    System.Windows.Point mCurrentpoint = new ();
    readonly List<System.Windows.Point> mPoints = new ();
    Stack<List<System.Windows.Point>> mDStrokes = new ();
    Stack<List<System.Windows.Point>> undoStrokes = new ();
    public Stack<List<System.Windows.Point>> Strokes => mDStrokes;
    public bool IsErase { get; set; }
    protected override void OnRender (DrawingContext dc) {
        base.OnRender (dc);
        Pen pen = new (Brushes.Black, 1);
        if (mDStrokes != null) {
            Stack<List<System.Windows.Point>> strokes = new ();
            foreach (var s in mDStrokes)
                strokes.Push (s);
            int c = strokes.Count;
            for (int i = 0; i < c; i++) {
                List<System.Windows.Point> points = strokes.Pop ();
                for (int j = 0; j < points.Count - 1; j++)
                    dc.DrawLine (pen, points[j], points[j + 1]);
            }
        }
        if (mPoints.Count > 1) {
            for (int i = 0; i < mPoints.Count - 1; i++)
                dc.DrawLine (pen, mPoints[i], mPoints[i + 1]);
        }
    }

    protected override void OnMouseDown (MouseButtonEventArgs e) {
        if (e.LeftButton == MouseButtonState.Pressed) {
            mCurrentpoint = e.GetPosition (this);
            mPoints.Add (mCurrentpoint);
        }
    }

    protected override void OnMouseMove (MouseEventArgs e) {
        if (e.LeftButton != MouseButtonState.Pressed) return;
        System.Windows.Point p = e.GetPosition (this);
        mPoints.Add (p);
        InvalidateVisual ();
    }

    protected override void OnMouseLeftButtonUp (MouseButtonEventArgs e) {
        if (e.ButtonState == MouseButtonState.Released) {
            List<System.Windows.Point> p = new ();
            foreach (var element in mPoints)
                p.Add (element);
            mDStrokes.Push (p);
            mPoints.Clear ();
            InvalidateVisual ();
        }
    }

    public void Undo() {
        if (mDStrokes.Count == 0) return;
        undoStrokes.Push (mDStrokes.Pop ());
        InvalidateVisual ();
    }
    public void Redo() {
        if (undoStrokes.Count == 0) return;
        mDStrokes.Push (undoStrokes.Pop ());
        InvalidateVisual ();
    }
}


