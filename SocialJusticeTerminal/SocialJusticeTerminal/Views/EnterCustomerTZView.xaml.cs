using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SocialJusticeTerminal.Views
{
    /// <summary>
    /// Interaction logic for EnterCustomerTzView.xaml
    /// </summary>
    public partial class EnterCustomerTzView : Window
    {
        public EnterCustomerTzView()
        {
            InitializeComponent();
        }

        public void FocusOnTextbox()
        {
            txtTz.Focus();
        }
    }
}
