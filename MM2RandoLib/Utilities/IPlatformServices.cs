using js65;
using System;
using System.Collections.Generic;
using System.Text;

namespace MM2RandoLib.Utilities;

public interface IPlatformServices
{
    Assembler CreateAssembler(Js65Options? options, bool debugJavascript);
}
