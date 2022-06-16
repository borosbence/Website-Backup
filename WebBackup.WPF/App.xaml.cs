using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WebBackup.WPF.Data;
using WebBackup.WPF.Models;
using WebBackup.WPF.Repositories;
using WebBackup.WPF.ViewModels;

namespace WebBackup.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IConfiguration Config { get; private set; }

        public App()
        {
            Config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var serviceprovider = ConfigureServices();
            Ioc.Default.ConfigureServices(serviceprovider);
            InitializeComponent();
        }

        /// <summary>
        /// Configures the services for the application.
        /// </summary>
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            string connectionString = Config.GetConnectionString("WebBackupDB");
            services.AddDbContext<WBContext>(options => options.UseSqlite(connectionString));
            services.AddScoped<IGenericRepository<Website>, GenericRepository<Website, WBContext>>();
            services.AddTransient<WebsitesViewModel>();

            return services.BuildServiceProvider();
        }

        #region Single Instance
        private static Mutex _mutex;
        protected override void OnStartup(StartupEventArgs e)
        {
            const string appName = "Website Backup";
            _mutex = new Mutex(true, appName, out bool createdNew);
            if (!createdNew)
            {
                Current.Shutdown();
            }
            base.OnStartup(e);
        }
        #endregion
    }
}
