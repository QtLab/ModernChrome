using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ModernChrome
{
    /// <inheritdoc />
    internal sealed class DesignTimeModernWindow : ModernWindow
    {
        /// <inheritdoc />
        public DesignTimeModernWindow()
        {
            CaptionIcon = new Ellipse {Width = 20, Height = 20};
            BindingOperations.SetBinding((Ellipse) CaptionIcon, Shape.FillProperty,
                new Binding
                {
                    Path = new PropertyPath("(TextBlock.Foreground)"),
                    RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(ContentPresenter), 1),
                    FallbackValue = Brushes.Black
                });
            Title = "Modern Chrome Window";
        }
    }
}