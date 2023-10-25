using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace MyDownload.DownLoad
{
    internal class DownloadManager
    {
        int maxInitTime = 5;
        List<string> recordList=new List<string>();
        Queue<FileDownTask> taskPool=new Queue<FileDownTask>();
        Queue<FileDownTask> getLenTaskPool = new Queue<FileDownTask>();

        HashSet<Task> workerPool=new HashSet<Task>();

        static object locker = new object();
        static object applyLocker = new object();

        static object record_locker = new object();
        static object getLenLocker = new object();
        static object downloadSizeLocker = new object();
        static object workerlocker = new object();

        int currFreeTask =10;

        bool isCalcuLoadLengthOver = false;
        bool isWork => allDownloadLength > 0 && allDownloadLength > downloadLength;

        long allDownloadLength=0;
        long downloadLength=0;

        public bool stiuckLog=false;


        Task tempTask=null;

        void CTime()
        {
            long last = 0;
            while(downloadLength<=allDownloadLength)
            {
                last = downloadLength - last;
                Form1.Instance.UpdateDownloadSpeed(last);
                Form1.Instance.UpdateDowloadLabel(allDownloadLength, downloadLength);
                Form1.Instance.UpdateProcess((float)downloadLength / (float)allDownloadLength);
                last = downloadLength;
                Thread.Sleep(1000);
            }
            allDownloadLength = 0;
            downloadLength = 0;
            tempTask = null;
        }

        public DownloadManager()
        {

        }
        public void SetMaxWorkTask(int num)
        {
            if (!isWork)
                currFreeTask = num;
        }
        public void ChangeWorkTaskNum(int num)
        {
            lock (applyLocker)
                currFreeTask += num;
        }
         void RemoveWorker(Task task)
        {
            lock (workerlocker)
            {
                if (workerPool.Contains(task))
                    workerPool.Remove(task);
            }
        }
        void AddWorker(Task task)
        {
            lock (workerlocker)
            {
                if (!workerPool.Contains(task))
                    workerPool.Add(task);
            }
        }
        public void Clear()
        {
            taskPool.Clear();
            allDownloadLength = 0;
            downloadLength = 0;
        }
        public void AddTask(string remoteURL, string saveDirectory, string fileName="")
        {
            try
            {
                isCalcuLoadLengthOver = false;
                if(!CheckUrlIsValid(remoteURL)) 
                {
                    Tool.ShowMessage($"{remoteURL}->是一个无效URL地址");
                    return; 
                }
                if (string.IsNullOrEmpty(fileName))
                {
                    Uri uri = new Uri(remoteURL);
                    //返回 123.xlsx
                    fileName = HttpUtility.UrlDecode(uri.Segments.Last());
                    Tool.ShowMessage("自动设置名字" + fileName);
                }
                if (!CheckSaveDirectorVaile(saveDirectory)) return;
                var task = new FileDownTask(remoteURL, saveDirectory, fileName);
                task.OnTaskStatusChange += TaskReport;
                task.OnDownLoadChange += DownloadReport;
                getLenTaskPool.Enqueue(task);
                Tool.ShowMessage($"加入下载任务->{remoteURL},文件名{fileName}");
            }catch(Exception ex)
            {
                Tool.ShowMessage("新建下载任务出错->" + ex.Message);
            }


        }
        void ReaddTask(DownloadFlag flag)
        {
            var task = new FileDownTask(flag);
            task.OnTaskStatusChange += TaskReport;
            task.OnDownLoadChange += DownloadReport;
            taskPool.Enqueue(task);
        }
        /// <summary>
        /// 检测链接是否为合法的网址格式
        /// </summary>
        /// <param name="uri">待检测的链接</param>
        /// <returns></returns>
        public bool CheckUrlIsValid(string uri)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(uri))
                    return false;

                var regex = @"(http://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
                Regex re = new Regex(regex);
                return re.IsMatch(uri);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }

        private bool CheckSaveDirectorVaile(string saveDirectory)
        {
            if (string.IsNullOrEmpty(saveDirectory))
            {
                Tool.ShowMessage("新建下载任务出错->" + $"[{saveDirectory}]路径不存在");
                return false;
            }
            else if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
                if (!Directory.Exists(saveDirectory))
                {
                    Tool.ShowMessage("新建下载任务出错->" + $"[{saveDirectory}]路径不存在");
                    return false;
                }
            }
            return true;
        }
        void DownloadReport(long len)
        {
            lock (downloadSizeLocker)
                downloadLength += len;
        }
        private void TaskReport(DownloadFlag flag)
        {
            if (flag.Result == TaskResult.loading)
            {
                string show = Tool.IsStuticMode() ? flag.RemoteURL : flag.FileName;
                Tool.ShowMessage($"{show}正在下载{flag.Process}");
            }else if (flag.Result == TaskResult.end)
            {
                string show = Tool.IsStuticMode() ? flag.RemoteURL : flag.FileName;
                Tool.ShowMessage($"{show}下载完成");
                StartTask();
            }else if (flag.Result == TaskResult.error)
            {
                string show = Tool.IsStuticMode() ? flag.RemoteURL : flag.FileName;
                Tool.ShowMessage($"{show}出现错误，msg:{flag.TaskMessages}");
                ReaddTask(flag);
            }
        }

        public static void ShowMessage()
        {
            List<string> recordList = null;
            lock(record_locker)
            {
                recordList = Form1.Instance.DownloadManager.recordList;
                Form1.Instance.DownloadManager.recordList.Clear();
            }

        }
        public void StartDownload()
        {
            int max = getLenTaskPool.Count >= 10 ? 10 : getLenTaskPool.Count;
            while(max > 0)
            {
                max--;
                GetDataLength();
            }
        }
       void GetDataLength()
        {
            FileDownTask fileDownTask = null;
            lock (getLenLocker)
            {
                if (getLenTaskPool.Count == 0)
                {

                    if (isCalcuLoadLengthOver == false && workerPool.Count == 0)
                    {
                        lock (downloadSizeLocker)
                        {
                            if (isCalcuLoadLengthOver) return;
                            isCalcuLoadLengthOver = true;
                            Tool.ShowMessage($"计算总下载长度完成->{Tool.To(allDownloadLength)}");

                            StartTask();
                        }
                    }

                    return;
                }
                fileDownTask = getLenTaskPool.Dequeue();
            }
            if (fileDownTask == null) return;
            Task t = null;
            t=new Task(() =>
            {
                //Tool.ShowMessage("DDD");
                var fileTask = fileDownTask;
                //长度判断在一开始就判断
                bool res = false;
                int initTIME = 0;
                while (res == false && initTIME < maxInitTime)
                {
                    initTIME++;
                    var taskRes = fileTask.InitFileSize();
                    while (taskRes.IsCompleted == false)
                        Thread.Sleep(100);
                    res = taskRes.Result;
                    if (res == false)
                        Thread.Sleep(1000);
                }
                if (initTIME > maxInitTime || res == false)
                {
                    lock (getLenTaskPool)
                    {
                        getLenTaskPool.Enqueue(fileDownTask);
                    }
                    Tool.ShowMessage($"任务->{fileDownTask.FileName}->超出最大重传,重新加入队伍末尾");

                    return;
                }
                lock (locker)
                    taskPool.Enqueue(fileTask);
                lock (downloadSizeLocker)
                    allDownloadLength += fileTask.FileSize;
                RemoveWorker(t);
                GetDataLength();
            });
            AddWorker(t);
            t.Start();
            }
        
        void StartTask()
        {
            FileDownTask fileDownTask = null;
            lock (locker)
            {
                if (tempTask == null)
                    tempTask = Task.Run(CTime);
                if(taskPool.Count == 0)
                {
                    Tool.ShowMessage("没有下载任务了");
                    return;
                }
                lock (applyLocker)
                {
                    if (currFreeTask <= 0)
                    {
                        Tool.ShowMessage("没有可用线程");
                        return;
                    }
                }
                fileDownTask = taskPool.Dequeue();           
            }
            if (fileDownTask == null) return;

            int needWorker = (int)(fileDownTask.FileSize / (1024 * 1024 * 100))+1;
            int applyWorker = 0;
            lock (applyLocker)
            {
                applyWorker= needWorker > currFreeTask ? currFreeTask : needWorker;
                ChangeWorkTaskNum(applyWorker * -1);
            }

            Tool.ShowMessage($"开始下载任务->{fileDownTask.FileName},被分配了[{applyWorker}]个worker");

            //Tool.ShowMessage("DDD");
            long perSize = (fileDownTask.FileSize - fileDownTask.FileSize % applyWorker) / applyWorker;
            long start = -perSize;
            long block = - 1;
            for (int i = 0; i < applyWorker; i++)
            {
                start += perSize;
                block += perSize;
                if (i + 1 == applyWorker)
                    block = fileDownTask.FileSize;
                //Tool.ShowMessage($"wdw开始{start},结束{block}");
                var worker = new DownloadWorker(fileDownTask, start, block);
                worker.Run();
            }

            lock (applyLocker)
            {
                if (currFreeTask > 0)
                    StartTask();
            }
        }

       
    }
}
