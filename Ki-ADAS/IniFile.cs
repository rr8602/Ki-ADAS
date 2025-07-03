using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ki_ADAS
{
    public class IniFile
    {
        private string _path;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public IniFile(string iniPath)
        {
            _path = iniPath;

            if (!File.Exists(_path))
            {
                using (FileStream fs = File.Create(_path)) { }
            }
        }

        public string ReadValue(string section, string key, string defaultValue = "")
        {
            StringBuilder sb = new StringBuilder(255);
            GetPrivateProfileString(section, key, defaultValue, sb, sb.Capacity, _path);

            return sb.ToString();
        }

        public void WriteValue(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, _path);
        }

        public int ReadInteger(string section, string key, int defaultValue = 0)
        {
            string value = ReadValue(section, key, defaultValue.ToString());

            if (int.TryParse(value, out int result)) 
                return result;

            return defaultValue;
        }

        public void WriteInteger(string section, string key, int value)
        {
            WriteValue(section, key, value.ToString());
        }

        public double ReadDouble(string section, string key, double defaultValue = 0.0)
        {
            string value = ReadValue(section, key, defaultValue.ToString());

            if (double.TryParse(value, out double result)) 
                return result;

            return defaultValue;
        }

        public void WriteDouble(string section, string key, double value)
        {
            WriteValue(section, key, value.ToString());
        }
    }
}
