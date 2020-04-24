using Luna.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace Luna.Models
{
    public enum SettingsChangeType
    {
        [Description("Based on individual settings")]
        Custom,
        [Description("Based on a .theme file")]
        Theme
    }

    [XmlRoot("Settings")]
    public class SettingsModel : NotifyPropertyChanged
    {
        private static IEnumerable<KeyValuePair<T, string>> GetKeyValuePairFromEnum<T>()
        {
            Type type = typeof(T);

            IEnumerable<T> values = Enum.GetValues(type).Cast<T>();

            return values.Select((value) =>
            {
                DescriptionAttribute[] attributes = (DescriptionAttribute[])type.GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);

                string description = "";

                if (attributes.Length > 0)
                {
                    description = attributes.First().Description;
                }

                return new KeyValuePair<T, string>(value, description);
            });
        }

        private bool _enabled;

        [XmlElement("Enabled")]
        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                if (CanChangeProperty())
                {
                    _enabled = value;
                    RaisePropertyChanged();
                }
            }
        }

        private DateTime _lightThemeTime = DateTime.Parse("0001-01-01T07:00:00");

        [XmlElement("LightThemeTime")]
        public DateTime LightThemeTime
        {
            get
            {
                return _lightThemeTime;
            }
            set
            {
                if (CanChangeProperty())
                {
                    _lightThemeTime = value;
                    RaisePropertyChanged();
                }
            }
        }

        private DateTime _darkThemeTime = DateTime.Parse("0001-01-01T19:00:00");

        [XmlElement("DarkThemeTime")]
        public DateTime DarkThemeTime
        {
            get
            {
                return _darkThemeTime;
            }
            set
            {
                if (CanChangeProperty())
                {
                    _darkThemeTime = value;
                    RaisePropertyChanged();
                }
            }
        }

        private SettingsChangeType _changeType = SettingsChangeType.Custom;

        [XmlElement("ChangeType")]
        public SettingsChangeType ChangeType
        {
            get
            {
                return _changeType;
            }
            set
            {
                _changeType = value;
                RaisePropertyChanged();
            }
        }

        [XmlIgnore]
        public static IEnumerable<KeyValuePair<SettingsChangeType, string>> ChangeTypeValues = GetKeyValuePairFromEnum<SettingsChangeType>();

        private bool _changeSystemTheme;

        [XmlElement("ChangeSystemTheme")]
        public bool ChangeSystemTheme
        {
            get
            {
                return _changeSystemTheme;
            }
            set
            {
                _changeSystemTheme = value;
                RaisePropertyChanged();
            }
        }

        private bool _changeAppTheme;

        [XmlElement("ChangeAppTheme")]
        public bool ChangeAppTheme
        {
            get
            {
                return _changeAppTheme;
            }
            set
            {
                _changeAppTheme = value;
                RaisePropertyChanged();
            }
        }

        private bool _changeWallpaper;

        [XmlElement("ChangeWallpaper")]
        public bool ChangeWallpaper
        {
            get
            {
                return _changeWallpaper;
            }
            set
            {
                _changeWallpaper = value;
                RaisePropertyChanged();
            }
        }

        private string _lightWallpaperPath;

        [XmlElement("LightWallpaperPath")]
        public string LightWallpaperPath
        {
            get
            {
                return _lightWallpaperPath;
            }
            set
            {
                _lightWallpaperPath = value;
                RaisePropertyChanged();
            }
        }

        private string _darkWallpaperPath;

        [XmlElement("DarkWallpaperPath")]
        public string DarkWallpaperPath
        {
            get
            {
                return _darkWallpaperPath;
            }
            set
            {
                _darkWallpaperPath = value;
                RaisePropertyChanged();
            }
        }

        private string _lightThemePath;

        [XmlElement("LightThemePath")]
        public string LightThemePath
        {
            get
            {
                return _lightThemePath;
            }
            set
            {
                _lightThemePath = value;
                RaisePropertyChanged();
            }
        }

        private string _darkThemePath;

        [XmlElement("DarkThemePath")]
        public string DarkThemePath
        {
            get
            {
                return _darkThemePath;
            }
            set
            {
                _darkThemePath = value;
                RaisePropertyChanged();
            }
        }
    }
}
