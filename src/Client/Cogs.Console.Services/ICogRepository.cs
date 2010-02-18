using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Cogs.Console.Services
{
    public interface ICogRepository
    {
        Stream RetrieveCogFileStream(string name, string version);
    }
}
