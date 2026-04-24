using System.Windows;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace laba5
{
    public partial class InputWindow : Window
    {
        public string Answer { get; private set; }

        public InputWindow(string prompt)
        {
            InitializeComponent();
            label.Content = prompt;
            inputBox.Focus();
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            Answer = inputBox.Text;
            DialogResult = true;
            Close();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void inputBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9.,]+$");
            e.Handled = !regex.IsMatch(e.Text);
        }
    }
}