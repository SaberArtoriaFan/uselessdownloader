using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyDownload.DownLoad
{
    internal class DownloadWorker
    {
        FileDownTask downTask;
        Task myTask;
        long start;
        long block;
        int maxRun = 10;
        bool isDispose = false;
        public DownloadWorker(FileDownTask downTask, long start, long block)
        {
            this.downTask = downTask;
            this.start = start;
            this.block = block;
            this.isDispose = false;
            //Tool.ShowMessage($"开始{start},结束{block}");
        }
        public void Break()
        {
            if(isDispose==false)
                Form1.Instance.DownloadManager.ChangeWorkTaskNum(1);
            isDispose = true;
            downTask = null;
        }
        public void Run()
        {
            if (downTask == null) return;
            myTask = Task.Run(() =>
            {
                try
                {
                    string remoteUrl = downTask.RemoteURL;
                    string savePatnc = Path.Combine(downTask.SaveDirectory, downTask.FileName);
                    //发送请求
                    var webRequest = WebRequest.Create(remoteUrl) as HttpWebRequest;
                    webRequest.AddRange(start, block);
                    webRequest.CookieContainer = new CookieContainer();

                    webRequest.Headers.Add("Accept-Language", "zh-cn,zh;q=0.8,en-us;q=0.5,en;q=0.3");
                    webRequest.UserAgent = "Code Sample Web Client";
                    webRequest.Method = "GET";
                    webRequest.Timeout = 30 * 1000;//设置超时，若webRequest.SendWebRequest()连接超时会返回，且isNetworkError为true
                    webRequest.ServicePoint.Expect100Continue = false;
                    var response =webRequest.GetResponse();
                    if (response.ContentLength <= 0)
                    {
                        //response.
                        Tool.ShowMessage("Download Error:" + "线程请求长度丢失");
                        goto ERROR;
                    }
                    else
                    {
                        try
                        {
                            //获取二进制数据
                            using (var stream = response.GetResponseStream())
                            {
                                var buffer = new byte[1024 * 50];
                                int offest = stream.Read(buffer, 0, buffer.Length);

                                if (Form1.Instance.DownloadManager.stiuckLog)
                                    Tool.ShowMessage($"线程任务{remoteUrl}开始->S{start},E{block}");

                                FileStream nFile = new FileStream(savePatnc, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                                //写入数据
                                nFile.Seek(start, SeekOrigin.Begin);
                                while (offest > 0)
                                {
                                    nFile.Write(buffer, 0, offest);
                                    //报告进度
                                    downTask.GrowthProgress(offest);
                                    offest = stream.Read(buffer, 0, buffer.Length);
                                }
                                nFile.Dispose();
                                nFile.Close();

                            }
                            isDispose =true;
                            Form1.Instance.DownloadManager.ChangeWorkTaskNum(1);
                        }
                        catch (Exception ex)
                        {
                            Tool.ShowMessage("线程出错了2" + ex + remoteUrl);
                            goto ERROR;

                        }
                        finally
                        {
                            response.Close();
                            response.Dispose();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Tool.ShowMessage("线程出错了1" + ex + downTask.FileName);
                    goto ERROR;

                }

                return;
            ERROR:
                if (maxRun-- > 0)
                {
                    Thread.Sleep(500);
                    Tool.ShowMessage($"[{downTask.FileName}]" + "线程重新尝试获取信息");
                    Run();
                }
                else
                {

                    downTask?.BreakTask();
                }
            });
 

        }

    }
}
