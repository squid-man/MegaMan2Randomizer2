using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace RandomizerHost.Views
{
    public partial class MessageBox : Window
    {
        public enum MessageBoxButtons
        {
            Ok,
            OkCancel,
            YesNo,
            YesNoCancel
        }

        public enum MessageBoxResult
        {
            Ok,
            Cancel,
            Yes,
            No
        }

        public MessageBox()
        {
            InitializeComponent();
        }

        public static Task<MessageBoxResult> Show(Window parent, String text, String title, MessageBoxButtons buttons)
        {
            MessageBox msgbox = new MessageBox()
            {
                Title = title
            };

            msgbox.Text.Text = text;

            MessageBoxResult res = MessageBoxResult.Ok;

            void AddButton(String caption, MessageBoxResult r, Boolean def = false)
            {
                Button btn = new Button { Content = caption };

                btn.Click += (_, __) => {
                    res = r;
                    msgbox.Close();
                };

                msgbox.Buttons.Children.Add(btn);

                if (def)
                {
                    res = r;
                }
            }

            if (buttons == MessageBoxButtons.Ok || buttons == MessageBoxButtons.OkCancel)
            {
                AddButton("Ok", MessageBoxResult.Ok, true);
            }

            if (buttons == MessageBoxButtons.YesNo || buttons == MessageBoxButtons.YesNoCancel)
            {
                AddButton("Yes", MessageBoxResult.Yes);
                AddButton("No", MessageBoxResult.No, true);
            }

            if (buttons == MessageBoxButtons.OkCancel || buttons == MessageBoxButtons.YesNoCancel)
            {
                AddButton("Cancel", MessageBoxResult.Cancel, true);
            }


            TaskCompletionSource<MessageBoxResult> tcs = new TaskCompletionSource<MessageBoxResult>();
            msgbox.Closed += delegate { tcs.TrySetResult(res); };

            if (parent != null)
            {
                msgbox.ShowDialog(parent);
            }
            else
            {
                msgbox.Show();
            }

            return tcs.Task;
        }
    }
}