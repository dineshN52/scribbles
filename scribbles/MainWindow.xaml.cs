using System.Windows;

namespace scribbles;

/// <summary>Interaction logic for MainWindow.xaml</summary>
public partial class MainWindow : Window {

   #region  Constructor----------
   public MainWindow () {
      InitializeComponent ();
      CloseButton.Click += Close_Click;
   }
   #endregion

   #region Methods--------------
   /// <summary>Method to create a new window every new file</summary>
   private void NewWindow_Click (object sender, RoutedEventArgs e) {
      MainWindow w = new ();
      w.Show ();
   }
   #endregion

   private void Open_Click (object sender, RoutedEventArgs e) => paintCanvas.OpenDocument (mFile);

   public void Save_Click (object sender, RoutedEventArgs e) {
      if (paintCanvas.IsNewfile)
         mFile.SaveAs (paintCanvas.AllShapes, true);
      else
         mFile.Save (paintCanvas.AllShapes);
      paintCanvas.IsModified = false;
   }

   private void SaveAsTxt_Click (object sender, RoutedEventArgs e) { if (mFile.SaveAs (paintCanvas.AllShapes, true)) paintCanvas.IsModified = false; }

   private void SaveAsBin_Click (object sender, RoutedEventArgs e) { if (mFile.SaveAs (paintCanvas.AllShapes, false)) paintCanvas.IsModified = false; }

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

   private void Minimize_Click (object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

   private void Maximize_Click (object sender, RoutedEventArgs e) {
      if (IsinOriginalSize) {
         IsinOriginalSize = false;
         WindowState = WindowState.Maximized;
      } else {
         IsinOriginalSize = true;
         WindowState = WindowState.Normal;
      }
   }

   private void Close_Click (object sender, RoutedEventArgs e) {
      mPop = new SaveMessagePop {
         Owner = this,
      };
      if (paintCanvas.IsModified == true)
         mPop.Show ();
      else
         SystemCommands.CloseWindow (this);
   }

   #region Data-------
   public static bool IsinOriginalSize = true;
   SaveMessagePop? mPop;
   FileManager mFile = new ();
   #endregion
}

public class Currentcanvas {
   public static MyCanvas canvas = new ();
}