using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyDownload.DownLoad
{
    public enum TaskResult
    {
        start,
        loading,
        end,
        error,
        hang
    }

    public class DownloadFlag
    {
        string remoteURL;
        string fileName;
        string saveDirectory;

        public DownloadFlag(string remoteURL, string saveDirectory, string fileName)
        {
            this.remoteURL = remoteURL;
            this.fileName = fileName;
            this.saveDirectory = saveDirectory;
        }

        TaskResult result;
        float process;
        long downloadLen = 0;
        long maxSize=0;
        string taskMessages;

        public TaskResult Result { get => result;internal set => result = value; }
        public string TaskMessages { get => taskMessages;internal set => taskMessages = value; }
        public float Process { get => process; set => process = value; }
        public string RemoteURL { get => remoteURL; }
        public string FileName { get => fileName; }
        public long DownloadLen { get => downloadLen; set => downloadLen = value; }
        public string SaveDirectory { get => saveDirectory; }
        public long MaxSize { get => maxSize; set => maxSize = value; }
    }

    internal class FileDownTask
    {
        string remoteURL;
        string saveDirectory;
        string fileName;
        long fileSize;
        long downloaded;

        DownloadFlag downloadFlag;

        List<DownloadWorker> workers = new List<DownloadWorker>();

        public event Action<DownloadFlag> OnTaskStatusChange;
        public event Action<long> OnDownLoadChange;

        public string RemoteURL { get => remoteURL;  }
        public string SaveDirectory { get => saveDirectory; }
        public string FileName { get => fileName;  }
        public long FileSize { get => fileSize; }
        public long Downloaded { get => downloaded;  }
        public TaskResult Result { get => downloadFlag.Result;  }

        static object _processLock=new object();

        public FileDownTask(string remoteURL, string saveDirectory, string fileName)
        {
            this.remoteURL = remoteURL;
            this.saveDirectory = saveDirectory;
            this.fileName = fileName;
            this.fileSize =-1;
            this.downloaded = 0;
            downloadFlag = new DownloadFlag(remoteURL,saveDirectory,fileName);
        }
        public FileDownTask(DownloadFlag flag)
        {
            downloadFlag = flag;
            this.remoteURL = flag.RemoteURL;
            this.saveDirectory = flag.SaveDirectory;
            this.fileName = flag.FileName;
            this.fileSize =flag.MaxSize;
            this.downloaded = 0;

            flag.DownloadLen = 0;
            flag.Result = TaskResult.start;
           

        }
        public void AddWorker(DownloadWorker worker)
        {
            if(workers.Contains(worker)==false)
                workers.Add(worker);
        }
        /// <summary>
        /// 由于错误被中断
        /// </summary>
        public void BreakTask()
        {
            foreach(var worker in workers)
            {
                worker.Break();
            }
            Tool.ShowMessage($"[{FileName}]" + "中断任务");
            SetTaskResult(TaskResult.error, "线程出错");

        }
        ~FileDownTask()
        {
            OnTaskStatusChange = null;
            OnDownLoadChange = null;
        }
        public void Dispose()
        {
            OnTaskStatusChange = null;
            OnDownLoadChange = null;
        }
        void CheckSavePath()
        {
            var savePath=Path.Combine(saveDirectory, fileName);
            if (!File.Exists(savePath))File.Create(savePath).Dispose();
        }
        public async Task<bool> InitFileSize()
        {

            CheckSavePath();

            try
            {
                string remoteUrl = RemoteURL;
                Tool.ShowMessage($"{remoteUrl}");
                var webRequest = WebRequest.Create(remoteUrl) as HttpWebRequest;
                webRequest.Timeout = 30 * 1000;//设置超时，若webRequest.SendWebRequest()连接超时会返回，且isNetworkError为true
                string show = Tool.IsStuticMode() ? remoteURL : fileName;
                //Tool.ShowMessage($"{show}---开始下载任务");
                webRequest.CookieContainer = new CookieContainer();
                webRequest.Headers.Add("Accept-Language", "zh-cn,zh;q=0.8,en-us;q=0.5,en;q=0.3");
                webRequest.UserAgent = "Code Sample Web Client";
                webRequest.Method = "GET";
                webRequest.ServicePoint.Expect100Continue = false;

                WebResponse response = null;
                try
                {
                    response = await webRequest.GetResponseAsync();
                }
                catch (Exception e)
                {
                    Tool.ShowMessage($"WebResponse引发异常" + e.Message); return false;
                }

                if (response.ContentLength <= 0)
                {
                    Tool.ShowMessage("消息长度错误,可能是对方网站回应错误，请重试");
                    response.Dispose();
                    return false;
                }
                else
                {
                    this.fileSize = response.ContentLength;
                    downloadFlag.MaxSize= this.fileSize;
                    response.Dispose();
                    return true;
                }
            }
            catch(Exception e) 
            {
                Tool.ShowMessage($"WebRequest引发异常" + e.Message); return false;
            }


        }
        public void GrowthProgress(long size)
        {
            lock (_processLock)
            {
                downloaded += size;
                OnDownLoadChange(size);
                downloadFlag.DownloadLen = downloaded;
                downloadFlag.Process=(float)downloaded/ (float)fileSize;
                if (downloaded == FileSize)
                {
                    SetTaskResult(TaskResult.end);
                    Dispose();
                    //Tool.ShowMessage($"{remoteURL}->任务下载完成");
                }
                else if(downloaded < FileSize)
                    SetTaskResult(TaskResult.loading);
                else
                    SetTaskResult(TaskResult.error);
            }
        }
        public void SetTaskResult(TaskResult taskResult,string message="")
        {
            if (taskResult == downloadFlag.Result) return;
            downloadFlag.Result = taskResult;
            downloadFlag.TaskMessages = message;
            //通知任务状态变更
            OnTaskStatusChange?.Invoke(downloadFlag);
        }
    }
}
