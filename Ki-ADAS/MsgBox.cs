using System.Windows.Forms;
using static Ki_ADAS.Language;

namespace Ki_ADAS
{
    public static class MsgBox
    {
        /// <summary>
        /// 정보 메시지 박스를 표시합니다.
        /// </summary>
        /// <param name="messageKey">언어 파일의 메시지 키</param>
        /// <param name="titleKey">언어 파일의 제목 키 (기본값: "Information")</param>
        public static void Info(string messageKey, string titleKey = "Information")
        {
            MessageBox.Show(LanguageManager.GetString(messageKey),
                            LanguageManager.GetString(titleKey),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }

        /// <summary>
        /// 포맷팅된 정보 메시지 박스를 표시합니다.
        /// </summary>
        public static void InfoWithFormat(string messageKey, string titleKey = "Information", params object[] args)
        {
            MessageBox.Show(LanguageManager.GetFormattedString(messageKey, args),
                            LanguageManager.GetString(titleKey),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }

        /// <summary>
        /// 질문 (Yes/No) 메시지 박스를 표시합니다.
        /// </summary>
        public static DialogResult Question(string messageKey, string titleKey = "Question")
        {
            return MessageBox.Show(LanguageManager.GetString(messageKey),
                                   LanguageManager.GetString(titleKey),
                                   MessageBoxButtons.YesNo,
                                   MessageBoxIcon.Question);
        }

        /// <summary>
        /// 포맷팅된 질문 (Yes/No) 메시지 박스를 표시합니다.
        /// </summary>
        public static DialogResult QuestionWithFormat(string messageKey, string titleKey = "Question", params object[] args)
        {
            return MessageBox.Show(LanguageManager.GetFormattedString(messageKey, args),
                                   LanguageManager.GetString(titleKey),
                                   MessageBoxButtons.YesNo,
                                   MessageBoxIcon.Question);
        }

        /// <summary>
        /// 질문 (OK/Cancel) 메시지 박스를 표시합니다.
        /// </summary>
        public static DialogResult QuestionOkCancel(string messageKey, string titleKey = "Question")
        {
            return MessageBox.Show(LanguageManager.GetString(messageKey),
                                   LanguageManager.GetString(titleKey),
                                   MessageBoxButtons.OKCancel,
                                   MessageBoxIcon.Question);
        }

        /// <summary>
        /// 포맷팅된 질문 (OK/Cancel) 메시지 박스를 표시합니다.
        /// </summary>
        public static DialogResult QuestionOkCancelWithFormat(string messageKey, string titleKey = "Question", params object[] args)
        {
            return MessageBox.Show(LanguageManager.GetFormattedString(messageKey, args),
                                   LanguageManager.GetString(titleKey),
                                   MessageBoxButtons.OKCancel,
                                   MessageBoxIcon.Question);
        }

        /// <summary>
        /// 경고 메시지 박스를 표시합니다.
        /// </summary>
        public static void Warn(string messageKey, string titleKey = "Warning")
        {
            MessageBox.Show(LanguageManager.GetString(messageKey),
                            LanguageManager.GetString(titleKey),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 포맷팅된 경고 메시지 박스를 표시합니다.
        /// </summary>
        public static void WarnWithFormat(string messageKey, string titleKey = "Warning", params object[] args)
        {
            MessageBox.Show(LanguageManager.GetFormattedString(messageKey, args),
                            LanguageManager.GetString(titleKey),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 오류 메시지 박스를 표시합니다.
        /// </summary>
        public static void Error(string messageKey, string titleKey = "Error")
        {
            MessageBox.Show(LanguageManager.GetString(messageKey),
                            LanguageManager.GetString(titleKey),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
        }

        /// <summary>
        /// 포맷팅된 오류 메시지 박스를 표시합니다.
        /// </summary>
        public static void ErrorWithFormat(string messageKey, string titleKey = "Error", params object[] args)
        {
            MessageBox.Show(LanguageManager.GetFormattedString(messageKey, args),
                            LanguageManager.GetString(titleKey),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
        }
    }
}
