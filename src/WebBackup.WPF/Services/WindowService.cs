using System.Linq;
using System.Windows;

namespace WebBackup.WPF.Services
{
    public interface IWindowService
    {
        void ShowDialog<T, P>(object? viewModel = null) where T : Window, new() where P : Window;
        void Close(object window);
        bool ConfirmDelete(string? title, string? caption);
        
    }

    public class WindowService : IWindowService
    {
        public void ShowDialog<T, P>(object? viewModel = null) where T : Window, new() where P : Window
        {
            Window window = new T();
            if (viewModel != null)
            {
                window.DataContext = viewModel;
            }
            window.Owner = Application.Current.Windows.OfType<P>().SingleOrDefault(x => x.IsActive);
            window.ShowDialog();
        }

        public void Close(object window)
        {
            if (window is Window closeWindow)
            {
                closeWindow.Close();
            }
        }

        public bool ConfirmDelete(string? title, string? caption)
        {
            MessageBoxResult mBoxResult = MessageBox.Show(caption, title, MessageBoxButton.YesNo, MessageBoxImage.Warning);
            bool result = false;
            if (mBoxResult == MessageBoxResult.Yes)
            {
                result = true;
            }
            return result;
        }
    }
}
