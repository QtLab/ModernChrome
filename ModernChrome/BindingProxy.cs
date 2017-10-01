using System.Windows;

namespace ModernChrome
{
    /// <inheritdoc />
    /// <summary>
    ///     Used to store DynamicResource objects which can be referenced in StaticResource contexts.
    /// </summary>
    internal sealed class BindingProxy : Freezable
    {
        /// <summary>
        ///     Identifies the <see cref="Data" /> property.
        /// </summary>
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));

        /// <summary>
        ///     Gets or sets the data stored in this <see cref="BindingProxy" />.
        /// </summary>
        public object Data
        {
            get => GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        protected override Freezable CreateInstanceCore() => new BindingProxy();
    }
}