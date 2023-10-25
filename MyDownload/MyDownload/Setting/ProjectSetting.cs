using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyDownload.Setting
{
    [DataContractAttribute]
    internal class ProjectSetting
    {
        [DataMemberAttribute]
        int downloadMode = 0;
        [DataMemberAttribute]
        string selfName="";
        [DataMemberAttribute]
        string remoteUrl;
        [DataMemberAttribute]
        string saveDirectory;
        [DataMemberAttribute]
        string configUrl;
        [DataMemberAttribute]
        string lastConfigName;
        public ProjectSetting(string selfName, string remoteUrl, string saveDirectory, string configUrl, string lastConfigName, int downloadMode = 0)
        {
            this.selfName = selfName;
            this.remoteUrl = remoteUrl;
            this.saveDirectory = saveDirectory;
            this.configUrl = configUrl;
            this.lastConfigName = lastConfigName;
            this.downloadMode = downloadMode;
        }

        public string RemoteUrl { get => remoteUrl;  }
        public string SaveDirectory { get => saveDirectory;  }
        public string ConfigUrl { get => configUrl; }
        public string LastConfigName { get => lastConfigName;}
        public string SelfName { get => selfName;  }
        public int DownloadMode { get => downloadMode; set => downloadMode = value; }
    }
}
