using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Windows;
using WebBackup.Core;
using WebBackup.Core.Repositories;
using WebBackup.Infrastructure.Data;
using WebBackup.WPF.Repositories;
using WebBackup.WPF.Services;
using WebBackup.WPF.ViewModels;

namespace WebBackup.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IConfiguration? Config { get; private set; }

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
            services.AddDbContext<WBContext>(options => 
                options.UseSqlite(connectionString, x => x.MigrationsAssembly("WebBackup.Infrastructure")));
            // Scoped - Repositories
            services.AddScoped<IGenericRepository<Website>, GenericRepository<Website, WBContext>>();
            // Singleton - Services
            services.AddSingleton<IWindowService, WindowService>();
            // Transienvt - ViewModels
            services.AddTransient<WebsitesViewModel>();
            //services.AddTransient<WebsiteFormViewModel>();

            return services.BuildServiceProvider();
        }

        #region Single Instance
        private static Mutex? _mutex = null;
        private static void CheckMutex()
        {
            const string appName = "Website Backup";
            _mutex = new Mutex(true, appName, out bool createdNew);
            if (!createdNew)
            {
                Current.Shutdown();
            }
        }
        #endregion

        #region XAML skins
        
        private string _activeSkin = string.Empty;
        /// <summary>
        /// Application Active skin name from ResourceDictionary 'SkinName'
        /// </summary>
        public string ActiveSkin
        {
            get => _activeSkin;
            set
            {
                if (_activeSkin.Equals(value))
                {
                    return;
                }
                _activeSkin = value;
                SkinResourceDictionary.ChangeSkin(_activeSkin);
            }
        }

        #endregion

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Check if program is running
            CheckMutex();

            // Create database file if not exist
            var context = Ioc.Default.GetRequiredService<WBContext>();
            context.Database.EnsureCreatedAsync();

            // Set default skin
            var skinConfig = Config.GetSection("Skin");
            ActiveSkin = skinConfig.Exists() ? skinConfig.Value : "Default";

            // Set default UI language
            LanguageSwitcher.SetDefaultLanguage(Config);
        }
    }
}