using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace MyDownload
{
    internal static class JsonUtility
    {
        /// <summary>
        /// 将序列化的json字符串内容写入Json文件，并且保存
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="jsonConents">Json内容</param>
        public static void WriteJsonFile(string path, string jsonConents)
        {
            //string directory=path.Substring(0,path.LastIndexOf(PathSlicer));
            if(!File.Exists(path)) { File.Create(path).Dispose(); }
            File.WriteAllText(path, jsonConents, System.Text.Encoding.UTF8);
            Tool.ShowMessage($"Succ OutPut Json To {path}");
        }

        /// <summary>
        /// 获取到本地的Json文件并且解析返回对应的json字符串
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        private static string GetJsonFile(string filepath)
        {
            string json = string.Empty;
            using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    json = sr.ReadToEnd().ToString();
                }
            }
            return json;
        }
        public static T GetJsonFileToObject<T>(string filepath)
        {
            string s = GetJsonFile(filepath);
            return JsonConvert.DeserializeObject<T>(s);

        }
        /// <summary>
        /// 对象 转换为Json字符串
        /// </summary>
        /// <param name="tablelList"></param>
        /// <returns></returns>
        public static string ToJson(object tablelList)
        {
            DataContractJsonSerializer json = new DataContractJsonSerializer(tablelList.GetType());
            string finJson = "";
            //序列化
            using (MemoryStream stream = new MemoryStream())
            {
                json.WriteObject(stream, tablelList);
                finJson = Encoding.UTF8.GetString(stream.ToArray());

            }
            //(tablelList + "JSON数据为:" + finJson);
            return finJson;
        }
    }
}
