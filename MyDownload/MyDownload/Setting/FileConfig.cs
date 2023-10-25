using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MyDownload.Setting
{
    [DataContractAttribute]
    internal class FileConfig
    {
        [DataMember]
        List<string> allFilesName;

        public FileConfig(List<string> files)
        {
            this.allFilesName = files;
        }

        public List<string> Files { get => allFilesName;}
    }
}
