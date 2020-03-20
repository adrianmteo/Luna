using Luna.Utils.Logger;
using System;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;

namespace Luna.Utils
{
    public class AutoFileSaver<T> where T : INotifyPropertyChanged, new()
    {
        private static readonly ILogger Logger = AppLogger.GetLoggerForCurrentClass();

        public string Path { get; private set; }

        public bool ReadOnly { get; private set; }

        public bool FoundOnDisk { get; private set; }

        private T _model;

        public T Model
        {
            get
            {
                return _model;
            }
            private set
            {
                if (_model != null)
                {
                    _model.PropertyChanged -= Model_PropertyChanged;
                }

                _model = value;
                _model.PropertyChanged += Model_PropertyChanged;
            }
        }

        public AutoFileSaver(string path, bool readOnly = false)
        {
            Path = path;
            ReadOnly = readOnly;

            ReloadFromDisk();
        }

        public void ReloadFromDisk()
        {
            FoundOnDisk = File.Exists(Path);

            try
            {
                Logger.Debug("Reading XML for '{0}' from file '{1}'", typeof(T).Name, Path);

                Model = ObjectSerializer.DeserializeObjectFromFile<T>(Path);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);

                Model = new T();
            }
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ReadOnly)
            {
                return;
            }

            object[] attributes = typeof(T).GetProperty(e.PropertyName).GetCustomAttributes(typeof(XmlIgnoreAttribute), false);

            if (attributes.Length > 0)
            {
                return;
            }

            Logger.Debug("Writing XML for '{0}' to file '{1}'", typeof(T).Name, Path);

            if (sender is T model)
            {
                try
                {
                    ObjectSerializer.SerializeObjectToFile(Path, model);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                }
            }
        }
    }
}
