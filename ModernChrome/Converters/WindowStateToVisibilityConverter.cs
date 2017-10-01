using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ModernChrome.Converters
{
    /// <inheritdoc />
    /// <summary>
    ///     Represents the converter that converts WindowState enumeration values to Visibility enumeration values.
    /// </summary>
    internal sealed class WindowStateToVisibilityConverter : IValueConverter
    {
        private static WindowStateToVisibilityConverter _instance;

        /// <summary>
        ///     Gets a singleton instance of this converter.
        /// </summary>
        public static WindowStateToVisibilityConverter Instance =>
            _instance ?? (_instance = new WindowStateToVisibilityConverter());

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is WindowState state
                ? (state == WindowState.Maximized ? Visibility.Collapsed : Visibility.Visible)
                : Visibility.Collapsed;

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}