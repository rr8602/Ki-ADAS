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

            // 중복 등록 방지
            CleanupInvalidReferences();
            
            foreach (var weakRef in _registeredForms)
            {
                if (weakRef.TryGetTarget(out Form existingForm) && existingForm == form)
                {
                    return;
                }
            }

            // 새 폼 등록
            _registeredForms.Add(new WeakReference<Form>(form));
            
            // 폼이 닫힐 때 목록에서 제거
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
            // 이벤트 발생
            LanguageChanged?.Invoke(null, new LanguageChangedEventArgs(newLanguage));
            
            // 등록된 모든 폼 업데이트
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
                        // 폼이 표시 중인 경우에만 업데이트 (성능 최적화)
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