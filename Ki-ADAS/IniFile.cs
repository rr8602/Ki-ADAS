using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ki_ADAS
{
    public class IniFile
    {
        private readonly string _path;
        private readonly Dictionary<string, Dictionary<string, string>> _data = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

        public IniFile(string iniPath)
        {
            try
            {
                _path = iniPath;

                if (!File.Exists(_path))
                {
                    File.Create(_path).Dispose();
                }
                else
                {
                    ParseFile();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("IniFileLoadError", "Error", _path, ex.Message);
            }
        }

        private void ParseFile()
        {
            try
            {
                _data.Clear();
                string currentSection = null;
                string[] lines = File.ReadAllLines(_path, Encoding.UTF8);

                foreach (string line in lines)
                {
                    string trimmedLine = line.Trim();

                    if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine.StartsWith(";") || trimmedLine.StartsWith("#"))
                        continue;

                    if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
                    {
                        currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2).Trim();

                        if (!_data.ContainsKey(currentSection))
                        {
                            _data[currentSection] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                        }
                    }
                    else if (currentSection != null)
                    {
                        int separatorIndex = trimmedLine.IndexOf('=');
                        
                        if (separatorIndex > 0)
                        {
                            string key = trimmedLine.Substring(0, separatorIndex).Trim();
                            string value = trimmedLine.Substring(separatorIndex + 1).Trim();
                            _data[currentSection][key] = value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("IniFileParseError", "Error", _path, ex.Message);
            }
        }

        public string ReadValue(string section, string key, string defaultValue = "")
        {
            if (_data.TryGetValue(section, out var sectionDict) && sectionDict.TryGetValue(key, out var value))
            {
                return value;
            }

            return defaultValue;
        }

        public void WriteValue(string section, string key, string value)
        {
            if (!_data.ContainsKey(section))
            {
                _data[section] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }

            _data[section][key] = value;
            SaveChanges();
        }

        private void SaveChanges()
        {
            try
            {
                var sb = new StringBuilder();

                foreach (var sectionPair in _data)
                {
                    sb.AppendLine($"[{sectionPair.Key}]");

                    foreach (var keyPair in sectionPair.Value)
                    {
                        sb.AppendLine($"{keyPair.Key}={keyPair.Value}");
                    }

                    sb.AppendLine();
                }

                File.WriteAllText(_path, sb.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("IniFileSaveError", "Error", _path, ex.Message);
            }
        }

        public List<string> GetKeys(string section)
        {
            if (_data.TryGetValue(section, out var sectionDict))
            {
                return sectionDict.Keys.ToList();
            }

            return new List<string>();
        }

        public int ReadInteger(string section, string key, int defaultValue = 0)
        {
            string value = ReadValue(section, key, defaultValue.ToString());
            if (int.TryParse(value, out int result)) return result;
            return defaultValue;
        }

        public void WriteInteger(string section, string key, int value)
        {
            WriteValue(section, key, value.ToString());
        }

        public double ReadDouble(string section, string key, double defaultValue = 0.0)
        {
            string value = ReadValue(section, key, defaultValue.ToString());
            if (double.TryParse(value, out double result)) return result;
            return defaultValue;
        }

        public void WriteDouble(string section, string key, double value)
        {
            WriteValue(section, key, value.ToString());
        }

        public string ReadString(string section, string key, string defaultValue = "")
        {
            return ReadValue(section, key, defaultValue);
        }

        public void WriteString(string section, string key, string value)
        {
            WriteValue(section, key, value);
        }
    }
}