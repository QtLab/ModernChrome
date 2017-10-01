using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ModernChrome.Converters
{
    /// <inheritdoc />
    /// <summary>
    ///     Represents the converter that converts ResizeMode enumeration values to boolean Enabled values.
    /// </summary>
    internal sealed class ResizeModeToEnabledConverter : IValueConverter
    {
        private static ResizeModeToEnabledConverter _instance;

        /// <summary>
        ///     Gets a singleton instance of this converter.
        /// </summary>
        public static ResizeModeToEnabledConverter Instance =>
            _instance ?? (_instance = new ResizeModeToEnabledConverter());

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ResizeMode resizeMode) || !(parameter is string param)) return false;
            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (param == "MIN")
                return resizeMode != ResizeMode.NoResize;
            if (param == "MAX")
                return resizeMode == ResizeMode.CanResize || resizeMode == ResizeMode.CanResizeWithGrip;
            return false;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}