using System.Windows;
using System.IO;
using Microsoft.Win32;
using System.Text;


namespace scribbles {
   /// <summary>Interaction logic for MainWindow.xaml</summary>
   public partial class MainWindow : Window {

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
            byte[] allBytes = File.ReadAllBytes (open.FileName);
            paintCanvas.BinaryOpen (allBytes);
         }
      }

      private void TextOpen_Click (object sender, RoutedEventArgs e) {
         OpenFileDialog open = new ();
         if (open.ShowDialog () == true) {
            string[] allShapes = File.ReadAllLines (open.FileName);
            paintCanvas.TextOpen (allShapes);
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
         if (dialog.ShowDialog () == true) {
            using (FileStream fs = new (dialog.FileName, FileMode.Create)) {
               BinaryWriter bw = new (fs);
               bw = paintCanvas.BianryWriter (ref bw);
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