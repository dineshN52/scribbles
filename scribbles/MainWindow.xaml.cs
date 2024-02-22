using System.Windows;
using System.IO;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Text;

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
         mOpen.DefaultExt = ".png";
         mOpen.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

      }

      private void SaveAsTxt_Click (object sender, RoutedEventArgs e) {
         //StringBuilder newFile = new ();
         //foreach (var file in paintCanvas.Strokes) {
         //   foreach (var points in file)
         //      newFile.AppendLine (points.ToString ());
         //   _ = newFile.Append ('\n');
         //}
         //string fileText = newFile.ToString ();
         //SaveFileDialog dialog = new () {
         //   Filter = "Text Files(*.txt)|*.txt|All(*.*)|*"
         //};
         //if (dialog.ShowDialog () == true)
         //   File.WriteAllText (dialog.FileName, fileText);
      }

      private void SaveAsBin_Click (object sender, RoutedEventArgs e) {
         //SaveFileDialog dialog = new () {
         //   Filter = "Binary Files(*.bin)|*.bin|All(*.*)|*"
         //};
         //if (dialog.ShowDialog () == true) {
         //   using var s = File.OpenWrite (dialog.FileName);
         //   var bw = new BinaryWriter (s);
         //   foreach (var file in paintCanvas.Strokes) {
         //      foreach (var streak in file) {
         //         bw.Write (streak.X);
         //         bw.Write (streak.Y);
         //         bw.Write (default (byte));
         //      }
         //      bw.Write (Encoding.ASCII.GetBytes ("n"));
         //   }
         //}
      }

      private void Save_Click (object sender, RoutedEventArgs e) {
         
      }
      private void MenuItem_Click (object sender, RoutedEventArgs e) {
         OpenFileDialog open = new ();
         if (open.ShowDialog () == true) {
            byte[] fileBytes = File.ReadAllBytes (open.FileName);
            List<List<byte>> stride = new ();
            List<byte> str = new ();
            foreach (var b in fileBytes) {
               byte n = Encoding.ASCII.GetBytes ("n")[0];
               if (b != n) {
                  str.Add (b); continue;
               }
               stride.Add (new List<byte> (str));
               str.Clear ();
            }
         }
      }

      private void EraseButton_Click (object sender, RoutedEventArgs e) => paintCanvas.IsErase = paintCanvas.IsErase != true;

      private void Undo_Click (object sender, RoutedEventArgs e) => paintCanvas.Undo ();

      private void Redo_Click (object sender, RoutedEventArgs e) => paintCanvas.Redo ();

      private void Rectangle_Click (object sender, RoutedEventArgs e) => paintCanvas.RectOn ();

      private void scribble_Click (object sender, RoutedEventArgs e) => paintCanvas.ScribbleOn ();
   }
}