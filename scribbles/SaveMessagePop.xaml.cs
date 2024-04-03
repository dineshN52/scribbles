﻿using BackEnd;
using System.ComponentModel;
using System.Windows;

namespace scribbles;

/// <summary>Interaction logic for Save.xaml</summary>
public partial class Save : Window {

   #region Constructor---------
   public Save (CancelEventArgs eCancel, FileManager File, bool IsNewFile) {
      InitializeComponent ();
      Text.Text = IsNewFile ? "Do you want to Save the changes to" + "\nuntitled?" : "Do you want to Save the changes to" + $"\n{File.mCurrentFileName} ?";
      mMainCloseEvent = eCancel;
      mFile = File;
      Closed += Save_Closed;
   }
   #endregion

   #region Properties----------
   private CancelEventArgs MainCancelEvent => mMainCloseEvent;

   public MyCanvas OwnerCanvas => ((MainWindow)Owner).Canvas;
   #endregion

   #region Methods-------------
   private void Yes_Click (object sender, RoutedEventArgs e) {
      IsClickedYesOrNo = true;
      if (OwnerCanvas.IsNewFile) {
         if (!mFile.SaveUntitled (OwnerCanvas.AllShapes))
            mMainCloseEvent.Cancel = true;
      } else
         mFile.Save (OwnerCanvas.AllShapes);
      SystemCommands.CloseWindow (this);
   }

   private void No_Click (object sender, RoutedEventArgs e) {
      IsClickedYesOrNo = true;
      SystemCommands.CloseWindow (this);
   }

   private void Cancel_Click (object sender, RoutedEventArgs e) { SystemCommands.CloseWindow (this); MainCancelEvent.Cancel = true; }

   private void Save_Closed (object? sender, System.EventArgs e) {
      if (OwnerCanvas.IsModified && !IsClickedYesOrNo)
         MainCancelEvent.Cancel = true;
   }
   #endregion

   #region Private-------------
   private FileManager mFile = new ();
   private CancelEventArgs mMainCloseEvent;
   private bool IsClickedYesOrNo;
   #endregion
}
