using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Cogs.Console.Services
{
    public class LocalCogRepository : ICogRepository
    {
        public Stream RetrieveCogFileStream(string name, string version)
        {
            return File.OpenRead("../../SampleCogs/ninject-1.0.cog");
        }

    }
}
