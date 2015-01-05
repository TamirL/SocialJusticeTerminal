using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace SocialJusticeTerminal.Helpers
{
    public static class TerminalMessageBox
    {
        private const string DEFAULT_CATION = "מועדון צדק חברתי";

        public static void ShowInfo(string text, string caption = null)
        {
            ShowOKMessageBox(MessageBoxImage.Information, text, caption);            
        }
        public static void ShowWarning(string text, string caption = null)
        {
            ShowOKMessageBox(MessageBoxImage.Warning, text, caption);            
        }

        public static void ShowError(string text, string caption = null)
        {
            ShowOKMessageBox(MessageBoxImage.Error, text, caption);
        }

        public static bool ShowQuestion(string questionText, string caption = null)
        {
            if (string.IsNullOrEmpty(caption))
            {
                caption = DEFAULT_CATION;
            }

            var selection = MessageBox.Show(questionText, caption, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes, MessageBoxOptions.RtlReading);
            return selection == MessageBoxResult.Yes;
        }

        private static void ShowOKMessageBox(MessageBoxImage type, string text, string caption = null)
        {
            if (string.IsNullOrEmpty(caption))
            {
                caption = DEFAULT_CATION;
            }

            MessageBox.Show(text, caption, MessageBoxButton.OK, type, MessageBoxResult.OK, MessageBoxOptions.RtlReading);
        }
    }
}
