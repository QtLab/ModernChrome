using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ModernChrome.Sample
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            ThemeManager.ThemeChanged += (sender, args) =>
            {
                if (args.Theme != "Light")
                {
                    ThemeText.Foreground = Brushes.White;
                    AccentText.Foreground = Brushes.White;
                    ActionText.Foreground = Brushes.White;
                }
                else
                {
                    ThemeText.Foreground = Brushes.Black;
                    AccentText.Foreground = Brushes.Black;
                    ActionText.Foreground = Brushes.Black;
                }
            };
        }

        private void ChangeAccent(object sender, RoutedEventArgs e)
        {
            var color = (sender as Button)?.Tag.ToString();
            if (!string.IsNullOrEmpty(color))
                BorderBrush = Application.Current.FindResource($"StatusBar{color}BrushKey") as SolidColorBrush;
        }

        private void ChangeTheme(object sender, RoutedEventArgs e)
        {
            var theme = (sender as Button)?.Tag.ToString();
            if (!string.IsNullOrEmpty(theme))
                ThemeManager.ChangeTheme(Application.Current, theme);
        }

        private void ShowDialog(object sender, RoutedEventArgs e)
        {
            new SampleDialog {Owner = this}.ShowDialog();
        }
    }
}