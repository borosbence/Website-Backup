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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebBackup.WPF.ViewModels;

namespace WebBackup.WPF.Views
{
    /// <summary>
    /// Interaction logic for WebsitesControl.xaml
    /// </summary>
    public partial class WebsitesControl : UserControl
    {
        public WebsitesControl()
        {
            DataContext = Ioc.Default.GetRequiredService<WebsitesViewModel>();
            InitializeComponent();
        }
    }
}
