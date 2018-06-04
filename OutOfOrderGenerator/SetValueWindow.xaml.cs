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

namespace OutOfOrderGenerator
{
    /// <summary>
    /// SetValueWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SetValueWindow : Window
    {
        public string gcname { get; set; }
        public SetValueWindow()
        {
            InitializeComponent();
            txtName.Focus();
        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            gcname = txtName.Text.Trim();
            this.Close();
        }

        public void SetValue(string s)
        {
            txtName.Text = s;
        }
    }
}
