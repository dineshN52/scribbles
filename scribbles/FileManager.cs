using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace scribbles;

/// <summary>Class to manage opening and saving of the files(Text and Binary files)</summary>
public class FileManager {

   public FileManager () { }

   public static List<Shapes> OpenFile () {
      OpenFileDialog open = new () {
         Filter = "Text Files(*.txt)|*.txt|BIN Files(*.bin)|*.bin"
      };
      List<Shapes> shapes = new ();
      if (open.ShowDialog () == true) {
         char[] l = open.FileName.TakeLast (3).ToArray ();
         string s = string.Join ("", l);
         if (s.Equals ("txt")) {
            string[] allShapes = File.ReadAllLines (open.FileName);
            shapes = Open (allShapes);
         } else {
            byte[] allBytes = File.ReadAllBytes (open.FileName);
            shapes = Open (allBytes);
         }
      }
      return shapes;
   }

   private static List<Shapes> Open (string[] allShapes) {
      List<Shapes> all = new ();
      int limits = 0;
      for (int i = 0; i < allShapes.Length; i = limits) {
         string s = allShapes[i];
         switch (s) {
            case "Scribble":
               Scribble sr = new () {
                  color = allShapes[i + 1], Thickness = int.Parse (allShapes[i + 2])
               };
               limits = (limits + 4) + int.Parse (allShapes[i + 3]);
               for (int j = i + 4; j < limits; j++)
                  sr.Points.Add (CustomPoint.Parse (allShapes[j]));
               all.Add (sr);
               break;
            case "CustomLine":
               CustomLine line = new () {
                  color = allShapes[i + 1], Thickness = int.Parse (allShapes[i + 2])
               };
               limits += 5;
               line.X = CustomPoint.Parse (allShapes[i + 3]);
               line.Y = CustomPoint.Parse (allShapes[i + 4]);
               all.Add (line);
               break;
            case "CustomRectangle":
               CustomRectangle rect = new () {
                  color = allShapes[i + 1], Thickness = int.Parse (allShapes[i + 2])
               };
               limits += 5;
               rect.mStartpoint = CustomPoint.Parse (allShapes[i + 3]);
               rect.mEndPoint = CustomPoint.Parse (allShapes[i + 4]);
               all.Add (rect);
               break;
            case "CustomCircle":
               CustomCircle circle = new () {
                  color = allShapes[i + 1], Thickness = int.Parse (allShapes[i + 2])
               };
               limits += 5;
               circle.centre = CustomPoint.Parse (allShapes[i + 3]);
               circle.radius = double.Parse (allShapes[i + 4]);
               all.Add (circle);
               break;
         }
      }
      return all;
   }

   private static List<Shapes> Open (byte[] allBytes) {
      List<Shapes> all = new ();
      int limits = 0;
      for (int i = 0; i < allBytes.Length; i = limits) {
         byte[] shape = new byte[4]; byte[] color = new byte[7]; byte[] thickness = new byte[4];
         Array.Copy (allBytes, i, shape, 0, 4);
         Array.Copy (allBytes, i + 4, color, 0, 7);
         Array.Copy (allBytes, i + 11, thickness, 0, 4);
         int s = BitConverter.ToInt32 (shape, 0);
         switch (s) {
            case 1:
               byte[] count = new byte[4];
               Array.Copy (allBytes, i + 15, count, 0, 4);
               Scribble scr = new ();
               string ss = System.Text.Encoding.Default.GetString (color);
               scr.color = ss;
               scr.Thickness = BitConverter.ToInt32 (thickness, 0);
               limits = (limits + 19) + (BitConverter.ToInt32 (count, 0) * 16);
               for (int j = i + 19; j < limits; j += 16) {
                  byte[] x = new byte[8]; byte[] y = new byte[8];
                  Array.Copy (allBytes, j, x, 0, 8);
                  Array.Copy (allBytes, j + 8, y, 0, 8);
                  CustomPoint p = new (BitConverter.ToDouble (x), BitConverter.ToDouble (y));
                  scr.Points.Add (p);
               }
               all.Add (scr);
               break;
            case 2:
               CustomLine line = new ();
               string ls = System.Text.Encoding.Default.GetString (color);
               line.color = ls;
               limits += 47;
               line.Thickness = BitConverter.ToInt32 (thickness, 0);
               for (int j = i + 15; j < limits; j += 32) {
                  byte[] x1 = new byte[8]; byte[] y1 = new byte[8]; byte[] x2 = new byte[8]; byte[] y2 = new byte[8];
                  Array.Copy (allBytes, j, x1, 0, 8);
                  Array.Copy (allBytes, j + 8, y1, 0, 8);
                  Array.Copy (allBytes, j + 16, x2, 0, 8);
                  Array.Copy (allBytes, j + 24, y2, 0, 8);
                  CustomPoint X = new (BitConverter.ToDouble (x1), BitConverter.ToDouble (y1));
                  CustomPoint Y = new (BitConverter.ToDouble (x2), BitConverter.ToDouble (y2));
                  line.X = X; line.Y = Y;
               }
               all.Add (line);
               break;
            case 3:
               CustomRectangle rect = new ();
               string rs = System.Text.Encoding.Default.GetString (color);
               rect.color = rs;
               limits += 47;
               for (int j = i + 15; j < limits; j += 32) {
                  rect.Thickness = BitConverter.ToInt32 (thickness, 0);
                  byte[] startX = new byte[8]; byte[] startY = new byte[8]; byte[] endX = new byte[8]; byte[] endY = new byte[8];
                  Array.Copy (allBytes, j, startX, 0, 8);
                  Array.Copy (allBytes, j + 8, startY, 0, 8);
                  Array.Copy (allBytes, j + 16, endX, 0, 8);
                  Array.Copy (allBytes, j + 24, endY, 0, 8);
                  CustomPoint X = new (BitConverter.ToDouble (startX), BitConverter.ToDouble (startY));
                  CustomPoint Y = new (BitConverter.ToDouble (endX), BitConverter.ToDouble (endY));
                  rect.mStartpoint = X; rect.mEndPoint = Y;
               }
               all.Add (rect);
               break;
            case 4:
               CustomCircle circle = new ();
               string cs = System.Text.Encoding.Default.GetString (color);
               circle.color = cs;
               limits += 47;
               for (int j = i + 15; j < limits; j += 32) {
                  circle.Thickness = BitConverter.ToInt32 (thickness, 0);
                  byte[] centreX = new byte[8]; byte[] centreY = new byte[8]; byte[] radius = new byte[8];
                  Array.Copy (allBytes, j, centreX, 0, 8);
                  Array.Copy (allBytes, j + 8, centreY, 0, 8);
                  Array.Copy (allBytes, j + 16, radius, 0, 8);
                  CustomPoint centre = new (BitConverter.ToDouble (centreX), BitConverter.ToDouble (centreY));
                  circle.radius = BitConverter.ToDouble (radius, 0);
                  circle.centre = centre;
               }
               all.Add (circle);
               break;
         }
      }
      return all;
   }

   public static void SaveAs (List<Shapes> allShapes, bool IsText) {
      SaveFileDialog dialog = new () {
         Filter = IsText ? "Text Files(*.txt)|*.txt|All(*.*)|*" : "BIN Files(*.bin)|*.bin|All(*.*)|*"
      };
      if (dialog.ShowDialog () == true) {
         if (IsText) {
            StringBuilder newFile = new ();
            foreach (var file in allShapes)
               newFile.Append (file.ToString ());
            File.WriteAllText (dialog.FileName, newFile.ToString ());
         } else {
            using (FileStream fs = new (dialog.FileName, FileMode.Create)) {
               BinaryWriter bw = new (fs);
               bw = BinaryWrite (ref bw, allShapes);
            }
         }
      }
   }

   private static BinaryWriter BinaryWrite (ref BinaryWriter bw, List<Shapes> allShapes) {
      foreach (var file in allShapes) {
         switch (file) {
            case Scribble scr:
               bw.Write (1);
               if (scr.color != null)
                  foreach (char c in scr.color)
                     bw.Write (c);
               bw.Write (scr.Thickness);
               bw.Write (scr.Points.Count);
               foreach (var point in scr.Points) {
                  bw.Write (point.X);
                  bw.Write (point.Y);
               }
               break;
            case CustomLine line:
               bw.Write (2);
               if (line.color != null)
                  foreach (char c in line.color)
                     bw.Write (c);
               bw.Write (line.Thickness);
               bw.Write (line.X.X);
               bw.Write (line.X.Y);
               bw.Write (line.Y.X);
               bw.Write (line.Y.Y);
               break;
            case CustomRectangle rect:
               bw.Write (3);
               if (rect.color != null)
                  foreach (char c in rect.color)
                     bw.Write (c);
               bw.Write (rect.Thickness);
               bw.Write (rect.mStartpoint.X);
               bw.Write (rect.mStartpoint.Y);
               bw.Write (rect.mEndPoint.X);
               bw.Write (rect.mEndPoint.Y);
               break;
            case CustomCircle circle:
               bw.Write (4);
               if (circle.color != null)
                  foreach (char c in circle.color)
                     bw.Write (c);
               bw.Write (circle.Thickness);
               bw.Write (circle.centre.X);
               bw.Write (circle.centre.Y);
               bw.Write (circle.radius);
               break;
         }
      }
      return bw;
   }

   //public void Save(List<Shapes> allShapes) {
   //   bool IsText = false;
   //   if (open != null) {
   //      string ext = open.DefaultExt;
   //      if (ext == "txt") {

   //         StringBuilder newFile = new ();
   //         foreach (var file in allShapes)
   //            newFile.Append (file.ToString ());
   //         File.WriteAllText (open.FileName, newFile.ToString ());
   //      } else {
   //      }
   //   } else
   //      SaveAs (allShapes, IsText);
   //}

}