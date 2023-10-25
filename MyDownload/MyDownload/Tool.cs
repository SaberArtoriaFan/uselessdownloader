using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace MyDownload
{
    internal static class Tool
    {
        public static void OpenDire(TextBox textBox)
        {
           
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog { ShowNewFolderButton = true };    //实例化一个文件夹选择对象，并且有新建文件夹按钮
            folderBrowserDialog.SelectedPath = textBox.Text;
            folderBrowserDialog.Description = "请选择文件路径";
            
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                textBox.Text = folderBrowserDialog.SelectedPath;
            }
        }
        public static void OpenInFrontDir(string filePath)
        {
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
            psi.Arguments = "/ e,/ select," +filePath;
            System.Diagnostics.Process.Start(psi);
        }
        public static void OpenInFrontFile(string filePath)
        {
            System.Diagnostics.Process.Start("Explorer.exe", $@"/select,{filePath}");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">后缀名，比如json或者txt</param>
        public static void SaveFile(string name,string type,string oepnDire)
        {
            // 创建保存对话框
            SaveFileDialog saveDataSend = new SaveFileDialog();
            // Environment.SpecialFolder.MyDocuments 表示在我的文档中
            saveDataSend.InitialDirectory = oepnDire;   // 获取文件路径
            saveDataSend.Filter = $"*.{type}|{type} file";   // 设置文件类型为文本文件
            saveDataSend.DefaultExt = $".{type}";   // 默认文件的拓展名
            saveDataSend.FileName = $"{name}.{type}";   // 文件默认名
            if (saveDataSend.ShowDialog() == DialogResult.OK)   // 显示文件框，并且选择文件
            {

                //saveDataSend.dire
                string fName = saveDataSend.FileName;   // 获取文件名
                                                        // 参数1：写入文件的文件名；参数2：写入文件的内容
                                                        // 字符串"Hello"是文件保存的内容，可以根据需求进行修改
                System.IO.File.WriteAllText(fName, "Hello");   // 向文件中写入内容
            }
        }
        public static void ShowMessage(string message)
        {
            message = $"[{DateTime.Now.ToLongTimeString().ToString()}]:{message}";
            Form1.Instance.ShowMessage(message);
        }
        public static bool IsStuticMode()
        {
            return Form1.Instance.DownloadManager.stiuckLog;
        }
        public static string To(long size)
        {
            long tobe = size;
            List<int> list = new List<int>();
            while(tobe >0)
            {
                int yu = (int)(tobe % 1024);
                list.Add(yu);
                tobe = (tobe - yu) / 1024;
            }
            float tb = (float)size / (MathF.Pow(1024, list.Count - 1));
            string hz = string.Empty;
            switch (list.Count)
            {
                case 0:
                    return "0B";
                case 1:
                    hz = "B";
                    break;
                case 2:
                    hz = "KB";
                    break;
                case 3:
                    hz = "MB";
                    break;
                case 4:
                    hz = "MB";
                    break;
                case 5:
                    hz = "GB";
                    break;
                case 6:
                    hz = "TB";
                    break;
            }
            tb = MathF.Round(tb,2);
            return $"{tb}{hz}";
        }
    }
}
