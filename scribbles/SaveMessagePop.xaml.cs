using System.Windows;

namespace scribbles;

/// <summary>Interaction logic for SaveMessagePop.xaml</summary>
public partial class SaveMessagePop : Window {
   #region Constructor----------
   public SaveMessagePop () {
      InitializeComponent ();
   }
   #endregion

   #region Methods-------------
   private void Yes_Click (object sender, RoutedEventArgs e) {
      if (Currentcanvas.canvas.IsNewfile)
         mFile.SaveUntitled (Currentcanvas.canvas.AllShapes);
      else
         mFile.Save (Currentcanvas.canvas.AllShapes);
      SystemCommands.CloseWindow (Owner);
      SystemCommands.CloseWindow (this);
   }

   private void No_Click (object sender, RoutedEventArgs e) {
      SystemCommands.CloseWindow (Owner);
      SystemCommands.CloseWindow (this);
   }

   private void Cancel_Click (object sender, RoutedEventArgs e) => SystemCommands.CloseWindow (this);
   #endregion

   #region Data--------
   FileManager mFile = new ();
   #endregion
}
