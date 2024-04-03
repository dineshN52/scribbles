using System;
using System.Data;
using System.Windows;
using System.Windows.Media;

namespace scribbles;

/// <summary>Interaction logic for MainWindow.xaml</summary>
public partial class MainWindow : Window {

   #region  Constructor--------
   public MainWindow () {
      InitializeComponent ();
      Closing += MainWindow_Closing;
   }
   #endregion

   #region Properties----------
   public MyCanvas Canvas => paintCanvas;
   #endregion

   #region Overrides-----------
   protected override void OnRender (DrawingContext drawingContext) {
      base.OnRender (drawingContext);
      undo.IsEnabled = paintCanvas.IsModified;
      redo.IsEnabled = paintCanvas.UndoShapeCount > 0 && paintCanvas.IsModified;
   }
   #endregion 

   #region Methods-------------
   /// <summary>Method to create a new window every new file</summary>
   private void NewWindow_Click (object sender, RoutedEventArgs e) {
      MainWindow w = new ();
      w.Show ();
   }

   private void Open_Click (object sender, RoutedEventArgs e) {
      try {
         paintCanvas.Open ();
      } catch (Exception ex) {
         MessageBox.Show (ex.Message);
      }
   }

   public void Save_Click (object sender, RoutedEventArgs e) => paintCanvas.Save ();

   private void SaveAsTxt_Click (object sender, RoutedEventArgs e) => paintCanvas.SaveAs (true);

   private void SaveAsBin_Click (object sender, RoutedEventArgs e) => paintCanvas.SaveAs (false);

   private void Exit_Click (object sender, RoutedEventArgs e) => Close ();

   private void Undo_Click (object sender, RoutedEventArgs e) { paintCanvas.Undo (); redo.IsEnabled = true; }

   private void Redo_Click (object sender, RoutedEventArgs e) { paintCanvas.Redo (); undo.IsEnabled = true; }

   private void scribble_Click (object sender, RoutedEventArgs e) => paintCanvas.ScribbleOn ();

   private void Rectangle_Click (object sender, RoutedEventArgs e) => paintCanvas.RectOn ();

   private void Line_Click (object sender, RoutedEventArgs e) => paintCanvas.LineOn ();

   private void Circle_Click (object sender, RoutedEventArgs e) => paintCanvas.CircleOn ();

   private void Color_Click (object sender, RoutedEventArgs e) {
      ColorPick cP = new () {
         Owner = this,
      };    //Creating new subwindow to display Color picker
      cP.Show ();
   }

   private void ThciknessVal_TextChanged (object sender, System.Windows.Controls.TextChangedEventArgs e) {
      if (paintCanvas != null)
         paintCanvas.CurrentShapeThickness = (int)double.Parse (ThciknessVal.Text);
   }

   private void MainWindow_Closing (object? sender, System.ComponentModel.CancelEventArgs e) {
      Save mPop = new (e, paintCanvas.canvasFileManager, paintCanvas.IsNewFile) {
         Owner = this,
      };
      if (paintCanvas.IsModified) mPop.ShowDialog ();
      else SystemCommands.CloseWindow (this);
   }
   #endregion
}
