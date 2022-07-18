using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
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
using WebBackup.Core;
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
            Type selectedType = e.NewValue.GetType();
            if (selectedType == typeof(Website))
            {
                Website webItem = (Website)e.NewValue;
                // Notify Main Window menu bar
                WeakReferenceMessenger.Default.Send(new WebItemChangedMessage(new WebItemMessage(webItem, Event.Select)));
            }
            else if (selectedType == typeof(FTPConnection))
            {
                // TODO: ftp selection
                //vm.SelectedWebItem = (FTPConnection)e.NewValue;
            }
            else if (selectedType == typeof(SQLConnection))
            {
                //vm.SelectedWebItem = (SQLConnection)e.NewValue;
            }
        }
    }
}
