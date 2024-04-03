using System.Windows;
using BackEnd;

namespace scribbles {
   /// <summary>Interaction logic for ColorPick.xaml</summary>
   public partial class ColorPick : Window {

      #region Constructor------
      public ColorPick () {
         InitializeComponent ();
      }
      #endregion

      #region Methods----------
      private void Red_Click (object sender, RoutedEventArgs e) { Colors.ShapeColor = "#FF0000"; Close (); }

      private void Black_Click (object sender, RoutedEventArgs e) { Colors.ShapeColor = "#000000"; Close (); }

      private void Blue_Click (object sender, RoutedEventArgs e) { Colors.ShapeColor = "#0000FF"; Close (); }

      private void Brown_Click (object sender, RoutedEventArgs e) { Colors.ShapeColor = "#A52A2A"; Close (); }

      private void Green_Click (object sender, RoutedEventArgs e) { Colors.ShapeColor = "#00FF00"; Close (); }

      private void Yellow_Click (object sender, RoutedEventArgs e) { Colors.ShapeColor = "#FFFF00"; Close (); }
      #endregion
   }

}
