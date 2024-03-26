using System.Windows;

namespace scribbles {
   /// <summary>Interaction logic for Sub.xaml</summary>
   public partial class Sub : Window {

      #region Constructor-------
      public Sub () {
         InitializeComponent ();
      }
      #endregion

      #region Methods---------
      private void Red_Click (object sender, RoutedEventArgs e) { Colors.ShapeColor = "#FF0000"; Close (); }

      private void Black_Click (object sender, RoutedEventArgs e) { Colors.ShapeColor = "#000000"; Close (); }

      private void Blue_Click (object sender, RoutedEventArgs e) { Colors.ShapeColor = "#0000FF"; Close (); }

      private void Brown_Click (object sender, RoutedEventArgs e) { Colors.ShapeColor = "#A52A2A"; Close (); }

      private void Green_Click (object sender, RoutedEventArgs e) { Colors.ShapeColor = "#00FF00"; Close (); }

      private void Yellow_Click (object sender, RoutedEventArgs e) { Colors.ShapeColor = "#FFFF00"; Close (); }
      #endregion
   }
}
