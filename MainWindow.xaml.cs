using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TPConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void PasteButton_Click(object sender, RoutedEventArgs e)
        {
            PasteFromClipboard();
        }

        private void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckTextBoxes()) return;

            Convert();
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            CopyToClipboard();
        }

        private void TransfortAllButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckTextBoxes()) return;

            PasteFromClipboard();
            Convert();
            CopyToClipboard();
        }

        private void PasteFromClipboard()
        {
            SetText(Clipboard.GetText());
        }

        private void CopyToClipboard()
        {
            Clipboard.SetText(GetText());
        }

        private bool CheckTextBoxes()
        {
            if (string.IsNullOrWhiteSpace(SeparatorTextBox.Text))
            {
                MessageBox.Show("Empty separator!");
                return false;
            }
            if (string.IsNullOrWhiteSpace(IDTextBox.Text))
            {
                MessageBox.Show("Empty ID!");
                return false;
            }
            return true;
        }

        private string GetText()
        {
            var textRange = new TextRange(
                TransformTextBox.Document.ContentStart,
                TransformTextBox.Document.ContentEnd
            );
            return textRange.Text;
        }

        private void SetText(string text)
        {
            TransformTextBox.Document.Blocks.Clear();
            TransformTextBox.Document.Blocks.Add(new Paragraph(new Run(text)));
        }

        private void Convert()
        {
            var converted = GetText();
            var sep = SeparatorTextBox.Text;

            converted = Regex.Replace(converted, @"(\d\d:\d\d:\d\d)\s*(.*)", IDTextBox.Text + sep + "$1" + sep + "$2" + sep, RegexOptions.Multiline);
            
            converted = Regex.Replace(converted, @"[\r\n]*", string.Empty, RegexOptions.Multiline);        

            converted = converted.Replace(IDTextBox.Text, Environment.NewLine + IDTextBox.Text);

            var rgx = new Regex(@"[\r\n]*", RegexOptions.Multiline);
            converted = rgx.Replace(converted, string.Empty, 1);

            SetText(converted);
        }
    }
}
