using System.Windows;

namespace ModernChrome.Sample
{
    public partial class SampleDialog
    {
        public SampleDialog()
        {
            InitializeComponent();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}