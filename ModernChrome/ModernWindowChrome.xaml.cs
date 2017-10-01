using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ModernChrome
{
    /// <inheritdoc cref="UserControl" />
    internal sealed partial class ModernWindowChrome
    {
        /// <summary>
        ///     Identifies the <see cref="Close" /> property.
        /// </summary>
        public static readonly DependencyProperty CloseProperty =
            DependencyProperty.Register("Close", typeof(string), typeof(ModernWindowChrome),
                new PropertyMetadata(string.Empty));

        /// <summary>
        ///     Identifies the <see cref="Maximize" /> property.
        /// </summary>
        public static readonly DependencyProperty MaximizeProperty =
            DependencyProperty.Register("Maximize", typeof(string), typeof(ModernWindowChrome),
                new PropertyMetadata(string.Empty));

        /// <summary>
        ///     Identifies the <see cref="Minimize" /> property.
        /// </summary>
        public static readonly DependencyProperty MinimizeProperty =
            DependencyProperty.Register("Minimize", typeof(string), typeof(ModernWindowChrome),
                new PropertyMetadata(string.Empty));

        /// <summary>
        ///     Identifies the <see cref="RestoreDown" /> property.
        /// </summary>
        public static readonly DependencyProperty RestoreDownProperty =
            DependencyProperty.Register("RestoreDown", typeof(string), typeof(ModernWindowChrome),
                new PropertyMetadata(string.Empty));

        /// <inheritdoc />
        public ModernWindowChrome()
        {
            InitializeComponent();
            Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
            {
                if (string.IsNullOrWhiteSpace(Minimize))
                    Minimize = GetCaption(900);
                if (string.IsNullOrWhiteSpace(Maximize))
                    Maximize = GetCaption(901);
                if (string.IsNullOrWhiteSpace(Close))
                    Close = GetCaption(905);
                if (string.IsNullOrWhiteSpace(RestoreDown))
                    RestoreDown = GetCaption(903);
            }));
        }

        /// <summary>
        ///     Gets or sets the tool-tip for the close button.
        /// </summary>
        public string Close
        {
            get => (string) GetValue(CloseProperty);
            set => SetValue(CloseProperty, value);
        }

        /// <summary>
        ///     Gets or sets the tool-tip for the maximize button.
        /// </summary>
        public string Maximize
        {
            get => (string) GetValue(MaximizeProperty);
            set => SetValue(MaximizeProperty, value);
        }

        /// <summary>
        ///     Gets or sets the tool-tip for the minimize button.
        /// </summary>
        public string Minimize
        {
            get => (string) GetValue(MinimizeProperty);
            set => SetValue(MinimizeProperty, value);
        }

        /// <summary>
        ///     Gets or sets the tool-tip for the restore button.
        /// </summary>
        public string RestoreDown
        {
            get => (string) GetValue(RestoreDownProperty);
            set => SetValue(RestoreDownProperty, value);
        }

        private static string GetCaption(int uId)
        {
            var hInstance = LoadLibrary(Environment.SystemDirectory + @"\User32.dll");
            var lpBuffer = new StringBuilder(256);
            LoadString(hInstance, uId, lpBuffer, lpBuffer.Capacity);
            FreeLibrary(hInstance);
            return lpBuffer.ToString().Replace("&", "");
        }

        #region Native Methods

        /// <devdoc>https://msdn.microsoft.com/en-us/library/windows/desktop/ms684175(v=vs.85).aspx</devdoc>
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);

        /// <devdoc>https://msdn.microsoft.com/en-us/library/windows/desktop/ms647486(v=vs.85).aspx</devdoc>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int LoadString(IntPtr hInstance, int uId, StringBuilder lpBuffer, int nBufferMax);

        /// <devdoc>https://msdn.microsoft.com/en-us/library/windows/desktop/ms683152(v=vs.85).aspx</devdoc>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FreeLibrary(IntPtr hModule);

        #endregion
    }
}