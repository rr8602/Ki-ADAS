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

                // 1. 명시적으로 등록된 키 확인
                if (_languageResources.TryGetValue(control, out resourceKey))
                {
                    // 등록된 키로 번역 적용
                    ApplyTranslation(control, resourceKey);
                }
                // 2. Tag에 지정된 키 확인
                else if (control.Tag is string tagKey && !string.IsNullOrEmpty(tagKey))
                {
                    ApplyTranslation(control, tagKey);
                }
                // 3. 컨트롤 이름 기반 자동 키 생성
                else if (!string.IsNullOrEmpty(control.Text) && control.Text != " ")
                {
                    // 라벨, 버튼 등 Text 속성을 가진 컨트롤만 처리
                    if (control is Label || control is Button || control is CheckBox ||
                        control is RadioButton || control is GroupBox)
                    {
                        // lbl_wheelbase -> WheelbaseLabel 형태로 키 생성
                        string autoKey = GenerateResourceKeyFromControl(control);
                        ApplyTranslation(control, autoKey, control.Text); // 기본값 유지
                    }
                }

                if (control.HasChildren)
                {
                    ApplyLanguageToControlsRecursive(control);
                }
            }
        }

        // 컨트롤 이름에서 리소스 키 생성
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

        // 번역 적용 (기본값 폴백 지원)
        private void ApplyTranslation(Control control, string key, string defaultText = null)
        {
            string translatedText = LanguageResource.GetMessageOrDefault(key, defaultText);

            if (!string.IsNullOrEmpty(translatedText))
            {
                control.Text = translatedText;
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
                // Tag 속성이 리소스 키인 경우 등록
                if (control.Tag is string resourceKey && !string.IsNullOrEmpty(resourceKey))
                {
                    RegisterLanguageResource(control, resourceKey);
                }

                // 그룹 컨트롤(Panel, GroupBox 등)인 경우 자식 컨트롤에 대해 재귀적으로 처리
                if (control.HasChildren)
                {
                    RegisterLanguageResourcesFromTags(control);
                }
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            RegisterLanguageResourcesFromTags(this);
            UpdateLanguage();
        }
    }
}