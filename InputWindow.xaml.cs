using System.Windows;

namespace laba5
{
    public partial class InputWindow : Window
    {
        public string Answer { get; private set; }
        public InputWindow(string prompt)
        {
            InitializeComponent();
            label.Content = prompt;
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
    }
}
