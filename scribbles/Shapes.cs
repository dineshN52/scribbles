using System.Collections.Generic;
using System.Windows.Media;
using System.Windows;
using System.Text;

namespace Scribbles;

public class Shapes {
   public virtual Pen? mPen { get; set; }
}
public class Scribble : Shapes {

   public List<Point> mPoints = new ();

   public override string ToString () {
      StringBuilder s = new ();
      s.AppendLine ("Scribble");
      s.AppendLine (mPen.Brush.ToString());
      s.AppendLine (mPen.Thickness.ToString());
      s.AppendLine (mPoints.Count.ToString ());
      foreach (var p in mPoints)
         s.AppendLine (p.ToString ());
      return s.ToString ();
   }

}
public class CustomLine : Shapes {
   public Point X { get; set; }
   public Point Y { get; set; }

   public override string ToString () {
      StringBuilder s = new ();
      s.AppendLine ("CustomLine");
      s.AppendLine (mPen.Brush.ToString ());
      s.AppendLine (mPen.Thickness.ToString ());
      s.AppendLine (X.ToString ());
      s.AppendLine (Y.ToString ());
      return s.ToString ();
   }
}
public class CustomRectangle : Shapes {
   public Point mStartpoint { get; set; }
   public Point mEndPoint { get; set; }

   public override string ToString () {
      StringBuilder s = new ();
      s.AppendLine ("CustomRectangle");
      s.AppendLine (mPen.Brush.ToString ());
      s.AppendLine (mPen.Thickness.ToString ());
      s.AppendLine (mStartpoint.ToString ());
      s.AppendLine (mEndPoint.ToString ());
      return s.ToString ();
   }
}
public class ConnectedLine : Shapes {
   public List<(Point,Point )> mPoints { get; set; }

}