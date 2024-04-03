using System.Windows;

namespace scribbles {
   /// <summary>Interaction logic for ColorPick.xaml</summary>
   public partial class ColorPick : Window {

      #region Constructor------
      public ColorPick () {
         InitializeComponent ();
      }
      #endregion

      #region Properties-------
      public MyCanvas OwnerCanvas => ((MainWindow)Owner).Canvas;
      #endregion

      #region Methods----------
      private void Red_Click (object sender, RoutedEventArgs e) { OwnerCanvas.CurrentShapeColor = "#FF0000"; Close (); }

      private void Black_Click (object sender, RoutedEventArgs e) { OwnerCanvas.CurrentShapeColor = "#000000"; Close (); }

      private void Blue_Click (object sender, RoutedEventArgs e) { OwnerCanvas.CurrentShapeColor = "#0000FF"; Close (); }

      private void Brown_Click (object sender, RoutedEventArgs e) { OwnerCanvas.CurrentShapeColor = "#A52A2A"; Close (); }

      private void Green_Click (object sender, RoutedEventArgs e) { OwnerCanvas.CurrentShapeColor = "#00FF00"; Close (); }

      private void Yellow_Click (object sender, RoutedEventArgs e) { OwnerCanvas.CurrentShapeColor = "#FFFF00"; Close (); }
      #endregion
   }

}
