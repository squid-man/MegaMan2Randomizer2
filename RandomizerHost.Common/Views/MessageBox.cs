using Avalonia;
using Avalonia.Controls;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace RandomizerHost.Views;

static class MessageBox
{
    public static Task<ButtonResult> ShowAsync(
        Visual parent, 
        string text, 
        string title, 
        ButtonEnum buttons = ButtonEnum.Ok,
        Icon icon = Icon.None)
    {
        TopLevel? top = TopLevel.GetTopLevel(parent);

        Debug.Assert(top != null);

        var box = MessageBoxManager.GetMessageBoxStandard(
            title, text, buttons, icon);
        if (top is Window wnd)
            return box.ShowWindowDialogAsync(wnd);
        else
            /// TODO: Implement title bar as a popup
            return box.ShowAsPopupAsync(top);
    }
}
