using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WorldEditor.Views
{
    public partial class TextInputDialog : Window
    {
        public TextInputDialog(string title, string label, string defaultValue = "")
        {
            InitializeComponent();
            TitleText = title;
            Label = label;
            Text = defaultValue;
            DataContext = this;
        }

        public string TitleText { get; set; }
        public string Label { get; set; }
        public string Text { get => ResponseTextBox.Text; set => ResponseTextBox.Text = value; }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
