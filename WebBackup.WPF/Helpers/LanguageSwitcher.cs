using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WebBackup.WPF
{
    public static class LanguageSwitcher
    {
        private const string DEFAULT_LANGUAGE_NAME = "en-US";

        /// <summary>
        /// Set the default language from config settings or current culture.
        /// </summary>
        /// <param name="config"></param>
        public static void SetDefaultLanguage(IConfiguration config)
        {
            string languageName;
            var langSection = config.GetSection("Language");
            // If config section exists.
            if (langSection.Exists())
            {
                string languageSectionValue = langSection.Value;
                // If Language is set to default no merge needed.
                if (languageSectionValue == DEFAULT_LANGUAGE_NAME)
                {
                    return;
                }
                languageName = languageSectionValue;
            }
            else
            {
                // Get current culture name.
                CultureInfo cultureInfo = Thread.CurrentThread.CurrentUICulture;
                languageName = cultureInfo.Name;
            }

            // Make new Resource Dictionary
            ResourceDictionary dict = new ResourceDictionary();
            try
            {
                dict.Source = GetLanguageUri(languageName);
            }
            catch (Exception)
            {
                // If no file found for culture or appsetting use the default language
                dict.Source = GetLanguageUri(DEFAULT_LANGUAGE_NAME);
            }

            // Add Resource Dictionary to application
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }

        /// <summary>
        /// Get the language resource dictionary Uri.
        /// </summary>
        /// <param name="languageName">language name (en-US)</param>
        private static Uri GetLanguageUri(string languageName)
        {
            return new Uri($@"\Resources\Languages\Strings.{languageName}.xaml", UriKind.Relative);
        }
    }
}
