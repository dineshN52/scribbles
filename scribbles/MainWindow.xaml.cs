using System.Windows;
using System.IO;
using Microsoft.Win32;
using System.Text;
using Scribbles;
using System.Windows.Media;
using System;

namespace scribbles {


   /// <summary>Interaction logic for MainWindow.xaml</summary>
   public partial class MainWindow : Window {
      readonly Microsoft.Win32.OpenFileDialog mOpen = new ();

      public MainWindow () {
         InitializeComponent ();
      }

      /// <summary>Method to create a new window every new file</summary>
      /// <param name="e"></param>
      private void NewWindow_Click (object sender, RoutedEventArgs e) {
         MainWindow w = new ();
         w.Show ();
      }
      private void Exit_Click (object sender, RoutedEventArgs e) => Close ();

      private void BinaryOpen_Click (object sender, RoutedEventArgs e) {
         OpenFileDialog open = new ();
         if (open.ShowDialog () == true) {
            byte[] allbytes = File.ReadAllBytes (open.FileName);
            int limits = 0;
            for(int i=0; i< allbytes.Length; i=limits) {
               byte[] shape = new byte[4];
               byte[] color = new byte[8];
               byte[] count = new byte[4];
               Array.Copy (allbytes, i, shape, 0, 4);
               Array.Copy (allbytes, i + 5, color, 0, 8);
               Array.Copy (allbytes, i + 15, count, 0, 4);
               if(BitConverter.ToInt32 (shape, 0) == 1) {
                  Scribble scr = new ();
                  string str = System.Text.Encoding.Default.GetString (color);
                  //Brush b = new SolidColorBrush (Color.FromArgb (color[0] , color[1], color[2], color[4]));
                  Brush brush = (Brush)new BrushConverter ().ConvertFrom (str);
                  scr.mPen = new Pen (brush, 1);
                  limits = (limits +19) + (BitConverter.ToInt32 (count, 0)*16);
                  for (int j = i + 19; j < limits; j += 16) {
                     byte[] shapeC = new byte[4];
                     Array.Copy (allbytes, j, shapeC, 0, 4);
                     if (BitConverter.ToInt32 (shapeC, 0) == 1) return;
                     byte[] x = new byte[8];
                     byte[] y = new byte[8];
                     Array.Copy (allbytes, j, x, 0, 8);
                     Array.Copy (allbytes, j + 8, y, 0, 8);
                     Point p = new Point (BitConverter.ToDouble (x), BitConverter.ToDouble (y));
                     scr.mPoints.Add (p); 
                  }
                  paintCanvas.mAllShapes.Add (scr);
                  paintCanvas.InvalidateVisual ();
               }
            }
         }
      }

      private void TextOpen_Click (object sender, RoutedEventArgs e) {
         OpenFileDialog open = new ();
         if (open.ShowDialog () == true) {   
            string[] allShapes = File.ReadAllLines (open.FileName);
            int limits = 0;
            for (int i = 0; i < allShapes.Length; i = limits) {
               string s = allShapes[i];
               switch (s) {
                  case "Scribble":
                     Scribble scr = new ();
                     scr.mPen = new Pen ((Brush)new BrushConverter ().ConvertFrom (allShapes[i + 1]), int.Parse (allShapes[i + 2]));
                     limits = (limits + 4) + int.Parse (allShapes[i + 3]);
                     for (int j = i + 4; j < limits; j++)
                        scr.mPoints.Add (Point.Parse (allShapes[j]));
                     paintCanvas.mShapes.Add (scr);
                     paintCanvas.InvalidateVisual ();
                     break;
                  case "CustomLine":
                     CustomLine l = new ();
                     l.mPen = new Pen ((Brush)new BrushConverter ().ConvertFrom (allShapes[i + 1]), int.Parse (allShapes[i + 2]));
                     limits += 5;
                     l.X = Point.Parse (allShapes[i + 3]);
                     l.Y = Point.Parse (allShapes[i + 4]);
                     paintCanvas.mShapes.Add (l);
                     paintCanvas.InvalidateVisual ();
                     break;
                  case "CustomRectangle":
                     CustomRectangle rect = new ();
                     rect.mPen = new Pen ((Brush)new BrushConverter ().ConvertFrom (allShapes[i + 1]), int.Parse (allShapes[i + 2]));
                     limits += 5;
                     rect.mStartpoint = Point.Parse (allShapes[i + 3]);
                     rect.mEndPoint = Point.Parse (allShapes[i + 4]);
                     paintCanvas.mShapes.Add (rect);
                     paintCanvas.InvalidateVisual ();
                     break;
               }
            }
         }
      }

      private void SaveAsTxt_Click (object sender, RoutedEventArgs e) {
         StringBuilder newFile = new ();
         foreach (var file in paintCanvas.mAllShapes)
            newFile.Append (file.ToString ());
         SaveFileDialog dialog = new () {
            Filter = "Text Files(*.txt)|*.txt|All(*.*)|*"
         };
         if (dialog.ShowDialog () == true)
            File.WriteAllText (dialog.FileName, newFile.ToString ());
      }

      private void SaveAsBin_Click (object sender, RoutedEventArgs e) {
        
         SaveFileDialog dialog = new () {
            Filter = "BIN Files(*.bin)|*.bin|All(*.*)|*"
         };
         if (dialog.ShowDialog () == true)
            using (FileStream fs = new (dialog.FileName, FileMode.Create)) {
               BinaryWriter bw = new BinaryWriter (fs);
               foreach (var file in paintCanvas.mAllShapes) {
                  switch (file) {
                     case Scribble scr:
                        bw.Write (1);
                        bw.Write (scr.mPen.Brush+"\n");
                        bw.Write (scr.mPoints.Count);
                        foreach (var point in scr.mPoints) {
                           bw.Write (point.X);
                           bw.Write (point.Y);
                        }
                        break;
                  }
               }
            }
      }

      private void Save_Click (object sender, RoutedEventArgs e) {

      }
     

      private void EraseButton_Click (object sender, RoutedEventArgs e) => paintCanvas.EraseOn ();

      private void Undo_Click (object sender, RoutedEventArgs e) => paintCanvas.Undo ();

      private void Redo_Click (object sender, RoutedEventArgs e) => paintCanvas.Redo ();

      private void Rectangle_Click (object sender, RoutedEventArgs e) => paintCanvas.RectOn ();

      private void scribble_Click (object sender, RoutedEventArgs e) => paintCanvas.ScribbleOn ();

      private void Line_Click (object sender, RoutedEventArgs e) => paintCanvas.LineOn ();

      private void ConnectedLine_Click (object sender, RoutedEventArgs e) => paintCanvas.ConnectedLineOn ();
   }
}