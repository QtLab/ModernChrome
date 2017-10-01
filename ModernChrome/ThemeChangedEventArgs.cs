using System;

namespace ModernChrome
{
    /// <inheritdoc />
    /// <summary>
    ///     Class which is used as argument for an event to signal theme changes.
    /// </summary>
    public sealed class ThemeChangedEventArgs : EventArgs
    {
        /// <summary>
        ///     Creates a new instance of this class with the specified theme argument.
        /// </summary>
        /// <param name="theme">The new theme.</param>
        // ReSharper disable once InheritdocConsiderUsage
        public ThemeChangedEventArgs(string theme) => Theme = theme;

        /// <summary>
        ///     The new theme.
        /// </summary>
        public string Theme { get; set; }
    }
}