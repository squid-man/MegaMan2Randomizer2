using System;
using System.Threading.Tasks;

namespace MM2RandoLib.Utilities;

public interface IRomSaver : IDisposable
{
    bool IsDisposed { get; }

    Task AddFile(string filename, byte[] data);

    Task Commit();
}
