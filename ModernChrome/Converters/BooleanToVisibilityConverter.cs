namespace ModernChrome.Converters
{
    /// <summary>
    ///     Represents the converter that converts Boolean values to and from Visibility enumeration values.
    /// </summary>
    internal sealed class BooleanToVisibilityConverter
    {
        private static System.Windows.Controls.BooleanToVisibilityConverter _instance;

        /// <summary>
        ///     Gets a singleton instance of this converter.
        /// </summary>
        public static System.Windows.Controls.BooleanToVisibilityConverter Instance =>
            _instance ?? (_instance = new System.Windows.Controls.BooleanToVisibilityConverter());
    }
}