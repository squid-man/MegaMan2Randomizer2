using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace RandomizerHost.Views;

// The browser does not deliver notifications that dialogs have been cancelled. This causes the function which launched the dialog to hang indefinitely. To solve this problem, detect when the main view is active again and cancel the dialog operation, causing an OperationCanceledException to be generated in the dialog function. However, there is a race condition where view notifications like PointerMoved can get through before the dialog is displayed; to solve this, wait a couple seconds before processing view notifications.
internal class DialogMonitor : IDisposable
{
    public CancellationToken Token => _cancelToken.Token;

    public DialogMonitor(Visual view, int delaySeconds = 2)
    {
        _top = TopLevel.GetTopLevel(view)!;

        _isDisposed = false;
        _timerExpired = false;

        _timer = new()
        {
            Interval = TimeSpan.FromSeconds(delaySeconds),
        };
        _timer.Tick += OnDialogTimerTick;

        _timerExpired = false;
        _cancelToken = new();

        _top.GotFocus += OnViewEvent;
        _top.PointerMoved += OnViewEvent;

        _timer.Start();
    }

    public void Dispose()
    {
        if (_isDisposed)
            return;

        _isDisposed = true;

        _timer.Stop();
        _cancelToken.Cancel();

        _top.GotFocus -= OnViewEvent;
        _top.PointerMoved -= OnViewEvent;
    }

    TopLevel _top;

    DispatcherTimer _timer;
    bool _timerExpired;

    CancellationTokenSource _cancelToken;

    bool _isDisposed;

    void OnDialogTimerTick(object? sender, EventArgs e)
    {
        _timerExpired = true;

        _timer.Stop();
    }

    void OnViewEvent(object? sender, EventArgs e)
    {
        if (_timerExpired)
            Dispose();
    }
}
