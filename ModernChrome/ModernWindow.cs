using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using MahApps.Metro.Behaviours;
using GlowWindowBehavior = ModernChrome.Behaviors.GlowWindowBehavior;

namespace ModernChrome
{
    /// <inheritdoc />
    public class ModernWindow : Window
    {
        /// <summary>
        ///     Identifies the <see cref="CaptionIcon" /> property.
        /// </summary>
        public static readonly DependencyProperty CaptionIconProperty =
            DependencyProperty.Register("CaptionIcon", typeof(object), typeof(ModernWindow),
                new PropertyMetadata(null));

        /// <summary>
        ///     Identifies the <see cref="GlowBrush" /> property.
        /// </summary>
        public static readonly DependencyProperty GlowBrushProperty = DependencyProperty.Register("GlowBrush",
            typeof(Brush), typeof(ModernWindow), new PropertyMetadata(null));

        /// <summary>
        ///     Identifies the <see cref="NonActiveGlowBrush" /> property.
        /// </summary>
        public static readonly DependencyProperty NonActiveGlowBrushProperty =
            DependencyProperty.Register("NonActiveGlowBrush", typeof(Brush), typeof(ModernWindow),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(153, 153, 153))));

        /// <summary>
        ///     Identiefies the <see cref="ShowCaptionIcon" /> property.
        /// </summary>
        public static readonly DependencyProperty ShowCaptionIconProperty =
            DependencyProperty.Register("ShowCaptionIcon", typeof(bool), typeof(ModernWindow),
                new PropertyMetadata(true));

        /// <summary>
        ///     Identifies the <see cref="ShowCloseButton" /> property.
        /// </summary>
        public static readonly DependencyProperty ShowCloseButtonProperty =
            DependencyProperty.Register("ShowCloseButton", typeof(bool), typeof(ModernWindow),
                new PropertyMetadata(true));

        /// <summary>
        ///     Identifies the <see cref="ShowStatusBar" /> property.
        /// </summary>
        public static readonly DependencyProperty ShowStatusBarProperty =
            DependencyProperty.Register("ShowStatusBar", typeof(bool), typeof(ModernWindow),
                new PropertyMetadata(true));

        /// <summary>
        ///     Identifies the <see cref="StatusBar" /> property.
        /// </summary>
        public static readonly DependencyProperty StatusBarProperty =
            DependencyProperty.Register("StatusBar", typeof(object), typeof(ModernWindow),
                new PropertyMetadata(null));

        private bool _shouldRestoreForDragMove;

        /// <inheritdoc />
        static ModernWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModernWindow),
                new FrameworkPropertyMetadata(typeof(ModernWindow)));
        }

        /// <inheritdoc />
        public ModernWindow()
        {
            BorderBrush = new SolidColorBrush(Color.FromRgb(0x00, 0x7a, 0xcc));
            // Enforce the visibility of the title bar and status bar.
            MinHeight = 35 + (ShowStatusBar ? 23 : 0) + 2;
            // Enforce the visibility of the icon (if any) and the window command buttons.
            MinWidth = 148;

            StylizedBehaviors.SetBehaviors(this, new StylizedBehaviorCollection
            {
                new BorderlessWindowBehavior(),
                new GlowWindowBehavior()
            });

            BindingOperations.SetBinding(this, GlowBrushProperty, new Binding
            {
                Source = this,
                Path = new PropertyPath("BorderBrush")
            });
            SetCurrentValue(BorderThicknessProperty, new Thickness(0));
        }

        /// <summary>
        ///     Gets or sets the caption icon displayed in the title bar.
        /// </summary>
        public object CaptionIcon
        {
            get => GetValue(CaptionIconProperty);
            set => SetValue(CaptionIconProperty, value);
        }

        /// <summary>
        ///     Gets or sets the brush used for the Window's glow.
        /// </summary>
        public Brush GlowBrush
        {
            get => (Brush) GetValue(GlowBrushProperty);
            set => SetValue(GlowBrushProperty, value);
        }

        /// <summary>
        ///     Gets or sets the brush used for the Window's non-active glow.
        /// </summary>
        public Brush NonActiveGlowBrush
        {
            get => (Brush) GetValue(NonActiveGlowBrushProperty);
            set => SetValue(NonActiveGlowBrushProperty, value);
        }

        /// <summary>
        ///     Gets or sets a boolean value indicating whether the icon should be displayed in the title.
        /// </summary>
        public bool ShowCaptionIcon
        {
            // ReSharper disable once PossibleNullReferenceException
            get => (bool) GetValue(ShowCaptionIconProperty);
            set => SetValue(ShowCaptionIconProperty, value);
        }

        /// <summary>
        ///     Gets or sets a boolen value indicating whether the close button should be displayed in the window command button
        ///     line.
        /// </summary>
        public bool ShowCloseButton
        {
            // ReSharper disable once PossibleNullReferenceException
            get => (bool) GetValue(ShowCloseButtonProperty);
            set => SetValue(ShowCloseButtonProperty, value);
        }

        /// <summary>
        ///     Gets or sets a boolean value indicating whether the default status bar should be displayed.
        /// </summary>
        public bool ShowStatusBar
        {
            // ReSharper disable once PossibleNullReferenceException
            get => (bool) GetValue(ShowStatusBarProperty);
            set => SetValue(ShowStatusBarProperty, value);
        }

        /// <summary>
        ///     Gets or sets the contents of the status bar.
        /// </summary>
        public object StatusBar
        {
            get => GetValue(StatusBarProperty);
            set => SetValue(StatusBarProperty, value);
        }

        /// <inheritdoc />
        public override void OnApplyTemplate()
        {
            if (GetTemplateChild("WindowChrome") is ModernWindowChrome windowChrome)
            {
                windowChrome.DataContext = this;

                windowChrome.WindowCaptionBar.MouseRightButtonDown += WindowCaptionBar_OnMouseRightButtonDown;

                windowChrome.WindowTitleBar.MouseLeftButtonDown += WindowTitleBar_OnMouseLeftButtonDown;
                windowChrome.WindowTitleBar.MouseLeftButtonUp += WindowTitleBar_OnMouseLeftButtonUp;
                windowChrome.WindowTitleBar.MouseMove += WindowTitleBar_OnMouseMove;

                windowChrome.CloseButton.Click += (sender, args) => Close();
                windowChrome.RestoreButton.Click += (sender, args) =>
                    WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                windowChrome.MinimizeButton.Click += (sender, args) => WindowState = WindowState.Minimized;
            }

            base.OnApplyTemplate();
        }

        #region Event Handlers

        private void WindowCaptionBar_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var hWnd = new WindowInteropHelper(this).Handle;
            var point = PointToScreen(e.MouseDevice.GetPosition(this));
            var cmd = TrackPopupMenu(GetSystemMenu(hWnd, false), 0x100, (int) point.X, (int) point.Y, 0, hWnd,
                IntPtr.Zero);
            if (cmd > 0) SendMessage(hWnd, 0x112, (IntPtr) cmd, IntPtr.Zero);
        }

        private void WindowTitleBar_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (ResizeMode == ResizeMode.CanResize || ResizeMode == ResizeMode.CanResizeWithGrip)
                    WindowState = WindowState == WindowState.Maximized
                        ? WindowState.Normal
                        : WindowState.Maximized;
            }
            else
            {
                _shouldRestoreForDragMove = WindowState == WindowState.Maximized;
                DragMove();
            }
        }

        private void WindowTitleBar_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _shouldRestoreForDragMove = false;
        }

        private void WindowTitleBar_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!_shouldRestoreForDragMove) return;
            _shouldRestoreForDragMove = false;
            var pointToWindow = e.MouseDevice.GetPosition(sender as IInputElement);
            var pointToScreen = PointToScreen(pointToWindow);
            Left = pointToScreen.X - RestoreBounds.Width * 0.5;
            Top = Math.Max(0, pointToScreen.Y - pointToWindow.Y);
            WindowState = WindowState.Normal;
            DragMove();
        }

        #endregion

        #region Native Methods

        /// <devdoc>https://msdn.microsoft.com/en-us/library/windows/desktop/ms647985(v=vs.85).aspx</devdoc>
        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        /// <devdoc>https://msdn.microsoft.com/en-us/library/windows/desktop/ms644950(v=vs.85).aspx</devdoc>
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        /// <devdoc>https://msdn.microsoft.com/en-us/library/windows/desktop/ms648002(v=vs.85).aspx</devdoc>
        [DllImport("user32.dll")]
        private static extern int TrackPopupMenu(IntPtr hMenu, uint uFlags, int x, int y, int nReserved, IntPtr hWnd,
            IntPtr prcRect);

        #endregion
    }
}