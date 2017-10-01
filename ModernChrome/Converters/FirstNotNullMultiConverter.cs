using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace ModernChrome.Converters
{
    /// <inheritdoc />
    /// <summary>
    ///     Represents the converter that converts multiple values to the first value that is not null.
    /// </summary>
    internal sealed class FirstNotNullMultiConverter : IMultiValueConverter
    {
        private static FirstNotNullMultiConverter _instance;

        /// <summary>
        ///     Gets a singleton instance of this converter.
        /// </summary>
        public static FirstNotNullMultiConverter Instance =>
            _instance ?? (_instance = new FirstNotNullMultiConverter());

        /// <inheritdoc />
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) =>
            values.FirstOrDefault(value => value != null);

        /// <inheritdoc />
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}