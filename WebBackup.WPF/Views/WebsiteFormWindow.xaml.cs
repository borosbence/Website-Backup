using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WebBackup.WPF.ViewModels;

namespace WebBackup.WPF.Views
{
    /// <summary>
    /// Interaction logic for WebsiteFormWindow.xaml
    /// </summary>
    public partial class WebsiteFormWindow : Window
    {
        public WebsiteFormWindow()
        {
            DataContext = Ioc.Default.GetRequiredService<WebsiteFormViewModel>();
            InitializeComponent();
        }
    }
}
