using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Isogeo.AddIn.ViewsModels.Search.Results
{
    /// <summary>
    /// Enables to set ListBox's scroll while respecting MVVM Pattern by setting listBox's selectedItem
    /// https://stackoverflow.com/a/54345445/12319802
    /// </summary>
    public class ScrollIntoViewBehavior : Behavior<ListBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
            base.OnDetaching();
        }

        private static void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;

            if (listBox?.SelectedItem == null)
                return;

            var action = () =>
            {
                listBox.UpdateLayout();

                if (listBox.SelectedItem != null)
                {
                    listBox.ScrollIntoView(listBox.SelectedItem);
                    listBox.SelectedItem = null; // By setting selectedItem to null, don't show a selection on listbox
                }
            };

            listBox.Dispatcher.BeginInvoke(action, DispatcherPriority.ContextIdle);
        }
    }
}
