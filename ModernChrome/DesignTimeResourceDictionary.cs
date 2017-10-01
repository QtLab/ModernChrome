using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows;

namespace ModernChrome
{
    /// <inheritdoc />
    internal sealed class DesignTimeResourceDictionary : ResourceDictionary
    {
        private readonly ObservableCollection<ResourceDictionary> _noopMergedDictionaries =
            new NoopObservableCollection<ResourceDictionary>();

        /// <inheritdoc />
        public DesignTimeResourceDictionary()
        {
            var fieldInfo = typeof(ResourceDictionary).GetField("_mergedDictionaries",
                BindingFlags.Instance | BindingFlags.NonPublic);
            if (fieldInfo != null)
                fieldInfo.SetValue(this, _noopMergedDictionaries);
        }

        private static Lazy<bool> IsInDesignMode { get; } =
            new Lazy<bool>(() => DesignerProperties.GetIsInDesignMode(new DependencyObject()));

        private sealed class NoopObservableCollection<T> : ObservableCollection<T>
        {
            protected override void InsertItem(int index, T item)
            {
                if (IsInDesignMode.Value)
                    base.InsertItem(index, item);
            }
        }
    }
}