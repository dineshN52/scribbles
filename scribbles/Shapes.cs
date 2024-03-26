using System.Collections.Generic;
using System.Text;
using System;


namespace scribbles;

/// <summary>Class which creates custom shapes</summary>
public class Shapes {
   public virtual string? color { get; set; }

   public virtual int Thickness { get; set; }
}
public class Scribble : Shapes {

   public List<CustomPoint> Points = new ();

   public override string ToString () {
      StringBuilder s = new ();
      s.AppendLine ("Scribble");
      s.AppendLine (color);
      s.AppendLine (Thickness.ToString ());
      s.AppendLine (Points.Count.ToString ());
      foreach (var p in Points)
         s.AppendLine (p.ToString ());
      return s.ToString ();
   }

}
public class CustomLine : Shapes {
   public CustomPoint X { get; set; }
   public CustomPoint Y { get; set; }
   public override string ToString () {
      StringBuilder s = new ();
      s.AppendLine ("CustomLine");
      s.AppendLine (color);
      s.AppendLine (Thickness.ToString ());
      s.AppendLine (X.ToString ());
      s.AppendLine (Y.ToString ());
      return s.ToString ();
   }
}
public class CustomRectangle : Shapes {
   public CustomPoint mStartpoint { get; set; }
   public CustomPoint mEndPoint { get; set; }
   public override string ToString () {
      StringBuilder s = new ();
      s.AppendLine ("CustomRectangle");
      s.AppendLine (color);
      s.AppendLine (Thickness.ToString ());
      s.AppendLine (mStartpoint.ToString ());
      s.AppendLine (mEndPoint.ToString ());
      return s.ToString ();
   }
}

public class CustomCircle : Shapes {
   public CustomPoint centre { get; set; }
   public double radius { get; set; }
   public override string ToString () {
      StringBuilder s = new ();
      s.AppendLine ("CustomCircle");
      s.AppendLine (color);
      s.AppendLine (Thickness.ToString ());
      s.AppendLine (centre.ToString ());
      s.AppendLine (radius.ToString ());
      return s.ToString ();
   }
}

public struct CustomPoint {
   public CustomPoint (double x, double y) {
      X = x; Y = y;
   }
   public double X { get; set; }

   public double Y { get; set; }

   public static CustomPoint Parse (string input) {
      CustomPoint C = new ();
      if (C.TryParse (input, out CustomPoint f)) return f;
      throw new ArgumentException ("Input is not in correct format");
   }

   private bool TryParse (string input, out CustomPoint f) {
      f = new CustomPoint ();
      if (input.Length <= 0) return false;
      string[] points = input.Split (',');
      if (points.Length == 2) {
         f = new (double.Parse (points[0]), double.Parse (points[1]));
         return true;
      }
      return false;
   }

   public override string ToString () => ($"{X},{Y}");
}

public static class Colors {
   public static string? ShapeColor = default;
}
