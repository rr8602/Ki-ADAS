using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace Ki_ADAS
{
    public static class LanguageManager
    {
        public static event EventHandler<LanguageChangedEventArgs> LanguageChanged;
        
        private static readonly List<WeakReference<Form>> _registeredForms = new List<WeakReference<Form>>();

        private static Dictionary<string, string> _currentStrings = new Dictionary<string, string>();
        private static Language _currentLanguage = Language.English;

        private static IniFile _iniFile = null;

        public static Language CurrentLanguageSetting
        {
            get { return _currentLanguage; }
            private set 
            {
                if (_currentLanguage != value)
                {
                    _currentLanguage = value;
                    LoadLanguageStrings(value);
                    OnLanguageChanged(value);
                }
            }
        }

        public static void RegisterForm(Form form)
        {
            if (form == null) return;

            CleanupInvalidReferences();
            
            foreach (var weakRef in _registeredForms)
            {
                if (weakRef.TryGetTarget(out Form existingForm) && existingForm == form)
                {
                    return;
                }
            }

            _registeredForms.Add(new WeakReference<Form>(form));
            
            form.FormClosed += (sender, e) => {
                CleanupInvalidReferences();
            };
        }

        public static void ChangeLanguage(Language language)
        {
            CurrentLanguageSetting = language;
        }

        private static void LoadLanguageStrings(Language language)
        {
            string languageFileName = language.ToString().ToLower() + ".ini";
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Language", languageFileName);
            string directoryPath = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            _iniFile = new IniFile(filePath);

            _currentStrings.Clear();

            try
            {
                foreach (string key in _iniFile.GetKeys("Messages"))
                {
                    _currentStrings[key] = _iniFile.ReadValue("Messages", key);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading language file {filePath}: {ex.Message}");
            }
        }

        private static void OnLanguageChanged(Language newLanguage)
        {
            LanguageChanged?.Invoke(null, new LanguageChangedEventArgs(newLanguage));
            UpdateAllForms();
        }

        private static void UpdateAllForms()
        {
            CleanupInvalidReferences();
            
            foreach (var weakRef in _registeredForms)
            {
                if (weakRef.TryGetTarget(out Form form))
                {
                    if (form is MultiLanguageForm mlForm)
                    {
                        if (form.IsHandleCreated && !form.IsDisposed)
                        {
                            if (form.InvokeRequired)
                            {
                                form.BeginInvoke(new MethodInvoker(() => mlForm.UpdateLanguage()));
                            }
                            else
                            {
                                mlForm.UpdateLanguage();
                            }
                        }
                    }
                }
            }
        }

        private static void CleanupInvalidReferences()
        {
            _registeredForms.RemoveAll(weakRef => !weakRef.TryGetTarget(out _));
        }

        public static string GetString(string key)
        {
            if (_currentStrings.ContainsKey(key))
            {
                return _currentStrings[key];
            }

            return key;
        }

        public static string GetFormattedString(string key, params object[] args)
        {
            string message = GetString(key);

            try
            {
                return string.Format(message, args);
            }
            catch (FormatException)
            {
                return message; 
            }
        }

        public static void Initialize()
        {
            LoadLanguageStrings(_currentLanguage);
        }
    }

    public class LanguageChangedEventArgs : EventArgs
    {
        public Language NewLanguage { get; } 

        public LanguageChangedEventArgs(Language newLanguage)
        {
            NewLanguage = newLanguage;
        }
    }
}