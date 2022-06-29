using System;
using System.Collections.Generic;
using System.Windows;
using System.ComponentModel;

namespace WebBackup.WPF
{
    public class SkinResourceDictionary : ResourceDictionary, ISupportInitialize
    {
        private static readonly Dictionary<string, Uri> skinUris = new Dictionary<string, Uri>();

        public static void ChangeSkin(string skinName)
        {
            foreach (ResourceDictionary dict in Application.Current.Resources.MergedDictionaries)
            {
                if (dict is SkinResourceDictionary skinDict)
                {
                    skinDict.SkinName = skinName;
                }
                else
                {
                    dict.Source = dict.Source;
                }
            }
        }

        bool IsInitializing = false;

        public string SkinName
        {
            get => skinName;
            set
            {
                skinName = value.Trim().ToUpperInvariant();
                if (!(IsInitializing || !skinUris.ContainsKey(skinName)))
                {
                    Source = skinUris[skinName];
                }
            }
        }
        private string skinName = string.Empty;

        public new void BeginInit()
        {
            IsInitializing = true;
            base.BeginInit();
        }

        public new void EndInit()
        {
            if (string.IsNullOrWhiteSpace(SkinName))
            {
                throw new Exception($"The property \"{nameof(SkinName)}\" is missing");
            }

            base.EndInit();
            if (skinUris.ContainsKey(SkinName))
            {
                skinUris.Remove(SkinName);
            }

            skinUris.Add(SkinName, Source);
            IsInitializing = false;
        }
    }
}
