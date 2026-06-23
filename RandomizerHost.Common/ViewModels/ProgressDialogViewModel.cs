using ReactiveUI.SourceGenerators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace RandomizerHost.ViewModels;

public partial class ProgressDialogViewModel : ViewModelBase
{
    public ObservableCollection<string?> MessageLines { get; } = new([null, null, null]);

    public IProgress<string?> GetProgressFromMessageLine(int lineIndex)
        => new LineProgress(this, lineIndex); 

    class LineProgress : IProgress<string?>
    {
        ProgressDialogViewModel _model;
        int _index;

        public LineProgress(ProgressDialogViewModel model, int index)
        {
            _model = model;
            _index = index;
        }

        public void Report(string? value)
            => _model.MessageLines[_index] = value;
    }
}
