using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Ki_ADAS
{
    public class MultiLanguageForm : Form
    {
        protected Dictionary<Control, string> _languageResources = new Dictionary<Control, string>();
        public virtual void UpdateLanguage()
        {
            ApplyLanguageToControlsRecursive(this);
        }

        protected void ApplyLanguageToControlsRecursive(Control container)
        {
            foreach (Control control in container.Controls)
            {
                string resourceKey = null;

                if (_languageResources.TryGetValue(control, out resourceKey))
                {
                    ApplyTranslation(control, resourceKey);
                }
                else if (control.Tag is string tagKey && !string.IsNullOrEmpty(tagKey))
                {
                    ApplyTranslation(control, tagKey);
                }
                else if (!string.IsNullOrEmpty(control.Text) && control.Text != " ")
                {
                    if (control is Label || control is Button || control is CheckBox ||
                        control is RadioButton || control is GroupBox)
                    {
                        string autoKey = GenerateResourceKeyFromControl(control);
                        ApplyTranslation(control, autoKey, control.Text);
                    }
                }

                if (control.HasChildren)
                {
                    ApplyLanguageToControlsRecursive(control);
                }
            }
        }

        private string GenerateResourceKeyFromControl(Control control)
        {
            string name = control.Name;

            if (name.Contains("_") && name.Length > 4)
            {
                name = name.Substring(name.IndexOf('_') + 1);
            }

            if (!string.IsNullOrEmpty(name) && name.Length > 0)
            {
                name = char.ToUpper(name[0]) + (name.Length > 1 ? name.Substring(1) : "");
            }

            if (control is Label)
                return name + "Label";
            else if (control is Button)
                return name + "Button";
            else if (control is CheckBox)
                return name + "CheckBox";
            else if (control is RadioButton)
                return name + "Radio";
            else if (control is GroupBox)
                return name + "Group";

            return name;
        }

        private void ApplyTranslation(Control control, string key, string defaultText = null)
        {
            string translatedText = LanguageManager.GetString(key);

            if (!string.IsNullOrEmpty(translatedText))
            {
                control.Text = translatedText;
            }
            else if (!string.IsNullOrEmpty(defaultText))
            {
                control.Text = defaultText;
            }
        }

        protected void RegisterLanguageResource(Control control, string resourceKey)
        {
            if (control != null && !string.IsNullOrEmpty(resourceKey))
            {
                _languageResources[control] = resourceKey;
            }
        }

        protected void RegisterLanguageResourcesFromTags(Control containerControl)
        {
            foreach (Control control in containerControl.Controls)
            {
                if (control.Tag is string resourceKey && !string.IsNullOrEmpty(resourceKey))
                {
                    RegisterLanguageResource(control, resourceKey);
                }

                if (control.HasChildren)
                {
                    RegisterLanguageResourcesFromTags(control);
                }
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LanguageManager.RegisterForm(this);
            RegisterLanguageResourcesFromTags(this);
            UpdateLanguage();
        }
    }
}