using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WebBackup.WPF.Services
{
    public interface IWindowService
    {
        void ShowDialog<T>(object? viewModel = null) where T : Window, new();
    }

    public class WindowService : IWindowService
    {
        public void ShowDialog<T>(object? viewModel = null) where T : Window, new()
        {
            Window window = new T();
            if (viewModel != null)
            {
                window.DataContext = viewModel;
            }
            window.ShowDialog();
        }
    }
}
