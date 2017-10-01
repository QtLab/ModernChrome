using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Interop;
using System.Windows.Threading;
using MahApps.Metro.Controls;

namespace ModernChrome.Behaviors
{
    /// <inheritdoc />
    /// <summary>
    ///     Extends a window with four glow windows that display a colored shadow.
    /// </summary>
    internal sealed class GlowWindowBehavior : Behavior<Window>
    {
        private static readonly TimeSpan GlowTimerDelay = TimeSpan.FromMilliseconds(200);

        private IntPtr _handle;

        private HwndSource _hWndSource;

        private GlowWindow _left, _right, _top, _bottom;

        private DispatcherTimer _makeGlowVisibleTimer;

        private bool IsGlowDisabled =>
            AssociatedObject is ModernWindow borderlessWindow && borderlessWindow.GlowBrush == null;

        private void AssociatedObjectOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (IsGlowDisabled)
                return;

            AssociatedObject.StateChanged -= AssociatedObjectStateChanged;
            AssociatedObject.StateChanged += AssociatedObjectStateChanged;

            if (_makeGlowVisibleTimer == null)
            {
                _makeGlowVisibleTimer = new DispatcherTimer {Interval = GlowTimerDelay};
                _makeGlowVisibleTimer.Tick += GlowVisibleTimerOnTick;
            }

            _left = new GlowWindow(AssociatedObject, GlowDirection.Left);
            _right = new GlowWindow(AssociatedObject, GlowDirection.Right);
            _top = new GlowWindow(AssociatedObject, GlowDirection.Top);
            _bottom = new GlowWindow(AssociatedObject, GlowDirection.Bottom);

            Show();
            Update();

            AssociatedObject.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() => SetOpacityTo(1)));
        }

        private void AssociatedObjectStateChanged(object sender, EventArgs e)
        {
            _makeGlowVisibleTimer?.Stop();
            if (AssociatedObject.WindowState == WindowState.Normal)
                if (_makeGlowVisibleTimer != null)
                    _makeGlowVisibleTimer.Start();
                else
                    RestoreGlow();
            else
                HideGlow();
        }

        private void AssociatedObjectUnloaded(object sender, RoutedEventArgs e)
        {
            if (_makeGlowVisibleTimer == null)
                return;
            _makeGlowVisibleTimer.Stop();
            _makeGlowVisibleTimer.Tick -= GlowVisibleTimerOnTick;
            _makeGlowVisibleTimer = null;
        }

        private void GlowVisibleTimerOnTick(object sender, EventArgs e)
        {
            _makeGlowVisibleTimer?.Stop();
            RestoreGlow();
        }

        private void HideGlow()
        {
            if (_left != null) _left.IsGlowing = false;
            if (_top != null) _top.IsGlowing = false;
            if (_right != null) _right.IsGlowing = false;
            if (_bottom != null) _bottom.IsGlowing = false;
            Update();
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.SourceInitialized += (o, args) =>
            {
                _handle = new WindowInteropHelper(AssociatedObject).Handle;
                _hWndSource = HwndSource.FromHwnd(_handle);
                _hWndSource?.AddHook(AssociatedObjectWindowProc);
            };
            AssociatedObject.Loaded += AssociatedObjectOnLoaded;
            AssociatedObject.Unloaded += AssociatedObjectUnloaded;
        }

        private void RestoreGlow()
        {
            if (_left != null) _left.IsGlowing = true;
            if (_top != null) _top.IsGlowing = true;
            if (_right != null) _right.IsGlowing = true;
            if (_bottom != null) _bottom.IsGlowing = true;
            Update();
        }

        private void SetOpacityTo(double newOpacity)
        {
            if (_left == null || _right == null || _top == null || _bottom == null) return;
            _left.Opacity = newOpacity;
            _right.Opacity = newOpacity;
            _top.Opacity = newOpacity;
            _bottom.Opacity = newOpacity;
        }

        private void Show()
        {
            _left?.Show();
            _right?.Show();
            _top?.Show();
            _bottom?.Show();
        }

        private void Update()
        {
            if (!(_left != null && _right != null && _top != null && _bottom != null)) return;
            _left.Update();
            _right.Update();
            _top.Update();
            _bottom.Update();
        }

#pragma warning disable 618
        private void UpdateCore()
        {
            if (!(_left != null && _right != null && _top != null && _bottom != null) || _handle == IntPtr.Zero) return;
            _left.Update();
            _right.Update();
            _top.Update();
            _bottom.Update();
        }
#pragma warning restore 618

        /// <devdoc>https://msdn.microsoft.com/en-us/library/windows/desktop/ms632612(v=vs.85).aspx</devdoc>
        [StructLayout(LayoutKind.Sequential)]
        internal struct WindowPosition
        {
            public IntPtr hWnd;

            public IntPtr hWndInsertAfter;

            public int x;

            public int y;

            public int cx;

            public int cy;

            public int flags;
        }

#pragma warning disable 618
        private WindowPosition _prevWindowPos;

        private IntPtr AssociatedObjectWindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (_hWndSource?.RootVisual == null)
                return IntPtr.Zero;

            switch (msg)
            {
                case 0x0047:
                case 0x0046:
                    var wp = (WindowPosition) Marshal.PtrToStructure(lParam, typeof(WindowPosition));
                    if (!wp.Equals(_prevWindowPos))
                        UpdateCore();
                    _prevWindowPos = wp;
                    break;
                case 0x0005:
                case 0x0214:
                    UpdateCore();
                    break;
                default:
                    return IntPtr.Zero;
            }
            return IntPtr.Zero;
        }
#pragma warning restore 618
    }
}