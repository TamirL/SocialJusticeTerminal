﻿using System.Security.AccessControl;
using System.Windows;
using System.Windows.Forms;

namespace SocialJusticeTerminal.Views
{
    /// <summary>
    /// Interaction logic for NewPurchaseView.xaml
    /// </summary>
    public partial class NewPurchaseView : Window
    {
        public NewPurchaseView()
        {
            InitializeComponent();
        }

        public void FocusOnTextbox()
        {
            this.txtPrice.Focus();
        }
    }
}
