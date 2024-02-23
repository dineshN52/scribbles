using System.Windows;
using System.IO;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Text;
using Scribbles;
using System.Windows.Media;

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

      private void OpenFile_Click (object sender, RoutedEventArgs e) {
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
         
      }

      private void Save_Click (object sender, RoutedEventArgs e) {

      }
     

      private void EraseButton_Click (object sender, RoutedEventArgs e) => paintCanvas.EraseOn ();

      private void Undo_Click (object sender, RoutedEventArgs e) => paintCanvas.Undo ();

      private void Redo_Click (object sender, RoutedEventArgs e) => paintCanvas.Redo ();

      private void Rectangle_Click (object sender, RoutedEventArgs e) => paintCanvas.RectOn ();

      private void scribble_Click (object sender, RoutedEventArgs e) => paintCanvas.ScribbleOn ();

      private void Line_Click (object sender, RoutedEventArgs e) => paintCanvas.LineOn ();
   }
}