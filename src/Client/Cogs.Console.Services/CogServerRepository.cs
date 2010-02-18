using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Cogs.Console.Services
{
    public class CogServerRepository : ICogRepository
    {
        public Stream RetrieveCogFileStream(string name, string version)
        {
            WebRequest req = HttpWebRequest.Create("http://ninject.org/assets/dist/Ninject-1.0-release-net-2.0.zip");

            WebResponse response = req.GetResponse();
                     
            return response.GetResponseStream();
        }
    }
}
