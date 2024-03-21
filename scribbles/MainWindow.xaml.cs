using System.Windows;

namespace scribbles {

   /// <summary>Interaction logic for MainWindow.xaml</summary>
   public partial class MainWindow : Window {

      public MainWindow () {
         InitializeComponent ();


      }

      /// <summary>Method to create a new window every new file</summary>
      private void NewWindow_Click (object sender, RoutedEventArgs e) {
         MainWindow w = new ();
         w.Show ();
      }

      private void Open_Click (object sender, RoutedEventArgs e) => paintCanvas.OpenDocument ();

      private void Save_Click (object sender, RoutedEventArgs e) {

      }

      private void SaveAsTxt_Click (object sender, RoutedEventArgs e) => FileManager.SaveAs (paintCanvas.AllShapes, true);

      private void SaveAsBin_Click (object sender, RoutedEventArgs e) => FileManager.SaveAs (paintCanvas.AllShapes, false);

      private void Exit_Click (object sender, RoutedEventArgs e) => Close ();

      private void Undo_Click (object sender, RoutedEventArgs e) => paintCanvas.Undo ();

      private void Redo_Click (object sender, RoutedEventArgs e) => paintCanvas.Redo ();

      private void scribble_Click (object sender, RoutedEventArgs e) => paintCanvas.ScribbleOn ();

      private void Line_Click (object sender, RoutedEventArgs e) => paintCanvas.LineOn ();

      private void Rectangle_Click (object sender, RoutedEventArgs e) => paintCanvas.RectOn ();

      private void Circle_Click (object sender, RoutedEventArgs e) => paintCanvas.CircleOn ();

      private void Color_Click (object sender, RoutedEventArgs e) {
         Sub S = new ();    //Creating new subwindow to display color picker
         S.Show ();
      }
   }
}