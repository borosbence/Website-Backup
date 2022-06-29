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
using WebBackup.WPF.Models;
using WebBackup.WPF.ViewModels;

namespace WebBackup.WPF.Views
{
    /// <summary>
    /// Interaction logic for WebsitesTreeControl.xaml
    /// </summary>
    public partial class WebsitesTreeControl : UserControl
    {
        public WebsitesTreeControl()
        {
            DataContext = Ioc.Default.GetRequiredService<WebsitesViewModel>();
            InitializeComponent();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var website = e.NewValue as Website;
            var vm = this.DataContext as WebsitesViewModel;
            if (website != null)
            {
                vm.SelectedWebsite = website;
            }
        }
    }
}
