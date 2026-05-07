using System;
using System.Threading.Tasks;

namespace MM2RandoLib.Utilities;

public interface IRomSaver : IDisposable
{
    bool IsDisposed { get; }

    void AddFile(string filename, byte[] data);

    Task Commit();
}
