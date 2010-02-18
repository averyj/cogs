using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using System.IO; 

namespace Cogs.Console.Services
{
   
    public class CogInstallationService
    {
        public const string COGSPATH = "c:\\program files\\cogs\\cogs\\";
    
        public void InstallCog(ICogRepository cogRepository, string name, string version)
        {
            Stream stream = cogRepository.RetrieveCogFileStream(name, version);

            UnZipFile(stream);
        }

        private void UnZipFile(Stream stream)
        {
            using (ZipInputStream s = new ZipInputStream(stream))
            {
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    ProcessZipEntry(theEntry, s);                    
                }
            }
        }

        private void ProcessZipEntry(ZipEntry theEntry, ZipInputStream s)
        {
            string directoryName = Path.GetDirectoryName(theEntry.Name);
            string fileName = Path.GetFileName(theEntry.Name);

            if (directoryName.Length > 0)
            {
                Directory.CreateDirectory(COGSPATH + directoryName);
            }

            if (fileName != String.Empty)
            {
                using (FileStream streamWriter = File.Create(COGSPATH + theEntry.Name))
                {

                    int size = 2048;
                    byte[] data = new byte[2048];
                    while (true)
                    {
                        size = s.Read(data, 0, data.Length);
                        if (size > 0)
                        {
                            streamWriter.Write(data, 0, size);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }

        public string[] ParseCogName(string p)
        {
            if(p.Contains("-"))
            {
                return p.Split('-');   
            }
            else
            {
                return new string[2]{p, string.Empty};
            }
        }
    }
}
