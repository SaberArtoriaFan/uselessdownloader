using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MyDownload.Setting
{
    [DataContractAttribute]
    internal class Record
    {
        [DataMember]
        List<string> infos;

        public Record(List<string> files)
        {
            this.infos = files;
        }

        public List<string> Infos { get => infos; }
    }
}
