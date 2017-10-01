using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ModernChrome.Converters
{
    /// <inheritdoc />
    /// <summary>
    ///     Represents the converter that converts WindowState enumeration values to Thickness structs.
    /// </summary>
    internal sealed class WindowStateToThicknessConverter : IValueConverter
    {
        private static WindowStateToThicknessConverter _instance;

        /// <summary>
        ///     Gets a singleton instance of this converter.
        /// </summary>
        public static WindowStateToThicknessConverter Instance =>
            _instance ?? (_instance = new WindowStateToThicknessConverter());

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is WindowState state
                ? (state == WindowState.Maximized ? new Thickness(0) : new Thickness(1))
                : new Thickness(0);

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}