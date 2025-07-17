using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Ki_ADAS
{
    public static class LanguageManager
    {
        public static event EventHandler<LanguageChangedEventArgs> LanguageChanged;
        
        private static readonly List<WeakReference<Form>> _registeredForms = new List<WeakReference<Form>>();

        public static Language CurrentLanguage
        {
            get { return LanguageResource.CurrentLanguage; }
            set 
            { 
                if (LanguageResource.CurrentLanguage != value)
                {
                    LanguageResource.CurrentLanguage = value;
                    OnLanguageChanged(value);
                }
            }
        }

        public static void RegisterForm(Form form)
        {
            if (form == null) return;

            // �ߺ� ��� ����
            CleanupInvalidReferences();
            
            foreach (var weakRef in _registeredForms)
            {
                if (weakRef.TryGetTarget(out Form existingForm) && existingForm == form)
                {
                    return;
                }
            }

            // �� �� ���
            _registeredForms.Add(new WeakReference<Form>(form));
            
            // ���� ���� �� ��Ͽ��� ����
            form.FormClosed += (sender, e) => {
                CleanupInvalidReferences();
            };
        }

        public static void ChangeLanguage(Language language)
        {
            CurrentLanguage = language;
        }

        private static void OnLanguageChanged(Language newLanguage)
        {
            // �̺�Ʈ �߻�
            LanguageChanged?.Invoke(null, new LanguageChangedEventArgs(newLanguage));
            
            // ��ϵ� ��� �� ������Ʈ
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
                        // ���� ǥ�� ���� ��쿡�� ������Ʈ (���� ����ȭ)
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