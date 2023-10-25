using HML;
using Microsoft.Win32;
using MyDownload.DownLoad;
using MyDownload.Setting;
using System;
using System.IO;
using System.Web;
using System.Windows.Forms;

namespace MyDownload
{
    public partial class Form1 : Form
    {
        string configSavePath;
        string recordSavePath;

        string lastconfigName = "LastDownloadConfig.json";
        ProjectSetting currSetting;
        int selectModel = 0;
        DownloadManager downloadManager = new DownloadManager();
        List<string> records = new List<string>();

        public static Form1 Instance { get; private set; }
        internal DownloadManager DownloadManager { get => downloadManager; }

        public Form1()
        {
            Instance = this;
            configSavePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            recordSavePath = configSavePath;
            configSavePath += @"Configs";
            recordSavePath += @"Records";
            //Console.WriteLine(configSavePath);
            if (!Directory.Exists(configSavePath)) Directory.CreateDirectory(configSavePath);
            if (!Directory.Exists(recordSavePath)) Directory.CreateDirectory(recordSavePath);

            InitializeComponent();

        }
        #region �߼�����
        public void UpdateDownloadSpeed(long speed)
        {
            if (downloadSpeed.IsDisposed) return;
            if (downloadSpeed.InvokeRequired)
                downloadSpeed.BeginInvoke(UpdateDownloadSpeed, speed);
            else
                downloadSpeed.Text = Tool.To(speed) + "S";
        }
        public void UpdateDowloadLabel(long all, long now)
        {
            if (downloadProcess.IsDisposed) return;
            if (downloadProcess.InvokeRequired)
            {
                downloadProcess.BeginInvoke(UpdateDowloadLabel, all, now);
            }
            else
            {
                downloadProcess.Text = $"{Tool.To(now)}/{Tool.To(all)}";
            }
        }
        public void UpdateProcess(float per)
        {
            if (progressBar1.IsDisposed) return;

            if (progressBar1.InvokeRequired)
                progressBar1.BeginInvoke(UpdateProcess, per);
            else
                progressBar1.Value = (int)(per * 100);
        }
        internal void ShowMessage(string message)
        {
            //Console.WriteLine(message);
            records.Add(message);
            if (MessageBoxText.IsDisposed) return;
            if (MessageBoxText.InvokeRequired)
            {
                MessageBoxText.BeginInvoke(_InvokeShowMessage, message);
            }
            else
                MessageBoxText.AppendText(message + "\r\n");
        }
        void _InvokeShowMessage(string message)
        {
            if (MessageBoxText.IsDisposed) return;
            MessageBoxText.AppendText(message + "\r\n");
        }
        internal void AddMessage(string message)
        {
            //Console.WriteLine(message);
            if (records.Count > 0)
                records[records.Count - 1] += message;
            else
                records.Add(message);
            if (MessageBoxText.IsDisposed) return;
            if (MessageBoxText.InvokeRequired)
            {
                MessageBoxText.BeginInvoke(_InvokeAddMessage, message);
            }
            else
                MessageBoxText.AppendText(message);
        }
        void _InvokeAddMessage(string message)
        {
            if (MessageBoxText.IsDisposed) return;
            MessageBoxText.AppendText(message);

        }
        void InitConfigSelect()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(configSavePath);
            List<string> res = new List<string>();
            foreach (var file in directoryInfo.GetFiles())
            {
                if (file.Name.EndsWith(".json"))
                {
                    res.Add(file.Name);
                }
            }
            ConfigSelectBox.Items.Clear();
            ConfigSelectBox.Items.AddRange(res.ToArray());

        }
        void SwitchConfig(ProjectSetting setting)
        {
            if (setting == null)
            {
                RemoteURLTextBox.Text = string.Empty;
                ConfigURLTextbox.Text = string.Empty;
                savePathTextbox.Text = string.Empty;
                ConfigSelectBox.SelectedItem = string.Empty;
                currSetting = null;
                return;
            }

            if (!ConfigSelectBox.Items.Contains(setting.SelfName))
                ConfigSelectBox.Items.Add(setting.SelfName);
            RemoteURLTextBox.Text = setting.RemoteUrl;
            ConfigURLTextbox.Text = setting.ConfigUrl;
            savePathTextbox.Text = setting.SaveDirectory;
            ConfigSelectBox.SelectedItem = setting.SelfName;
            SwitchDownloadMode(setting.DownloadMode);
        }
        /// <summary>
        /// 0����һģʽ
        /// 1,���������ļ�ģʽ
        /// </summary>
        /// <param name="mode"></param>
        void SwitchDownloadMode(int mode)
        {
            selectModel = mode;
            switch (mode)
            {
                case 0:
                    remoteURLLabel.Hide();
                    RemoteURLTextBox.Hide();
                    remoteButton.Hide();
                    startDownloadButton.Hide();
                    configURLlabel.Text = "URL";
                    ReadConfigButton.Text = "����";
                    downloadModeButton.Text = "��һ����";
                    break;
                case 1:
                    configURLlabel.Text = "��Դ����URL";
                    ReadConfigButton.Text = "��ȡ����";
                    downloadModeButton.Text = "��������";

                    remoteURLLabel.Show();
                    RemoteURLTextBox.Show();
                    remoteButton.Show();
                    startDownloadButton.Show();
                    break;
            }
        }
        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {
            InitConfigSelect();
            var setting = JsonUtility.GetJsonFileToObject<ProjectSetting>(Path.Combine(configSavePath, lastconfigName));
            if (setting == null)
            {
                SwitchDownloadMode(0);
                return;
            }

            if (!string.IsNullOrEmpty(setting.LastConfigName) && setting.LastConfigName != lastconfigName)
            {
                var last = JsonUtility.GetJsonFileToObject<ProjectSetting>(Path.Combine(configSavePath, setting.LastConfigName));
                if (last != null)
                    setting = last;
            }
            currSetting = setting;
            SwitchConfig(setting);
        }



        private void StartDownload(object sender, EventArgs e)
        {
            Console.WriteLine("��ʼ�������ļ�������");
            try
            {
                //��ȡ�����ļ�
                Uri uri = new Uri(ConfigURLTextbox.Text);
                //���� 123.xlsx
                var fileName = HttpUtility.UrlDecode(uri.Segments.Last());
                var config = JsonUtility.GetJsonFileToObject<FileConfig>(Path.Combine(savePathTextbox.Text, fileName));

                if (config == null || config.Files == null)
                {
                    Tool.ShowMessage($"�����ļ���ʧ->{Path.Combine(savePathTextbox.Text, fileName)}");
                    return;
                }

                foreach (var url in config.Files)
                {
                    string savePath = Path.Combine(savePathTextbox.Text, url);
                    string fileN = Path.GetFileName(savePath);
                    savePath = Path.GetDirectoryName(savePath);
                    downloadManager.AddTask(Path.Combine(RemoteURLTextBox.Text, url), savePath, fileN);
                }
                downloadManager.StartDownload();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void ReadConfigFromURL(object sender, EventArgs e)
        {

            downloadManager.AddTask(ConfigURLTextbox.Text, savePathTextbox.Text);
            downloadManager.StartDownload();
        }

        private void OnResourceConfigURLButton(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", ConfigURLTextbox.Text);

        }

        private void OnOpenSavePathButton(object sender, EventArgs e)
        {
            Tool.OpenDire(savePathTextbox);

        }

        private void OnRemoteURLButton(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", RemoteURLTextBox.Text);
        }

        private void SavePathTextChanged(object sender, EventArgs e)
        {
            if (savePathTextbox.Text == string.Empty) return;
            if (!Directory.Exists(savePathTextbox.Text))
            {
                savePathTextbox.Text = string.Empty;
                Console.WriteLine("����ı���·�����Ϸ���");
            }
        }

        private void ConfigURLTextbox_TextChanged(object sender, EventArgs e)
        {

        }



        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            //Console.WriteLine("ssa");
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            //string str = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            JsonUtility.WriteJsonFile(Path.Combine(configSavePath, lastconfigName), JsonUtility.ToJson(new ProjectSetting(lastconfigName, RemoteURLTextBox.Text, savePathTextbox.Text, ConfigURLTextbox.Text, ConfigSelectBox.Text, selectModel)));

            base.OnFormClosing(e);

        }

        private void OpenPathButton_Click(object sender, EventArgs e)
        {
            Tool.OpenInFrontDir(savePathTextbox.Text);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void ConfigSelectBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ConfigSelectBox.SelectedItem == null || string.IsNullOrEmpty(ConfigSelectBox.SelectedItem as string)) return;
            if (currSetting == null || (string)ConfigSelectBox.SelectedItem == currSetting.SelfName) return;
            var setting = JsonUtility.GetJsonFileToObject<ProjectSetting>(Path.Combine(configSavePath, (string)ConfigSelectBox.SelectedItem));



            if (setting != null)
            {
                currSetting = setting;
                SwitchConfig(setting);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void SaveNewConfigButton_Click(object sender, EventArgs e)
        {
            var origin = ConfigSelectBox.SelectedItem as string;
            if (string.IsNullOrEmpty(origin)) origin = "������";
            else origin = origin.Replace(".json", "");
            string s = Microsoft.VisualBasic.Interaction.InputBox("���������Ϊ���ļ���(�벻Ҫ�Ӻ�׺)", "���Ϊ", origin);

            if (string.IsNullOrEmpty(s))
            {

                return;
            }
            if (s.Contains('.'))
            {
                Console.WriteLine("�ַ���ʧЧ����������ȷ���ļ�����" + s);
                return;
            }
            s += ".json";
            string path = Path.Combine(configSavePath, s);
            if (string.IsNullOrEmpty(Path.GetFileNameWithoutExtension(path)))
            {
                Console.WriteLine("�ַ���ʧЧ����������ȷ���ļ�����" + path);
                return;
            }

            var setting = new ProjectSetting(s, RemoteURLTextBox.Text, savePathTextbox.Text, ConfigURLTextbox.Text, s, selectModel);
            JsonUtility.WriteJsonFile(path, JsonUtility.ToJson(setting));
            SwitchConfig(setting);
            Console.WriteLine("����ɹ�Ϊ->" + $"{s}");
        }

        private void OutputRecord_Click(object sender, EventArgs e)
        {
            var path = Path.Combine(recordSavePath, "Record.json");
            JsonUtility.WriteJsonFile(path, JsonUtility.ToJson(new Record(records)));
            Tool.OpenInFrontFile(path);

        }

        private void ClearRecordButton_Click(object sender, EventArgs e)
        {
            MessageBoxText.Text = string.Empty;
        }

        private void DelectConfigButton_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(configSavePath)||ConfigSelectBox.SelectedItem == null) { Tool.ShowMessage("��Ч����"); return; }
            var configName = ConfigSelectBox.SelectedItem as string;
            var path = Path.Combine(configSavePath, configName);
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                    Console.WriteLine($"�ɹ�ɾ��->{path}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ɾ��ʧ��->{path}��ԭ��->{ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"ɾ��ʧ��->{path}��ԭ��->·��������");
            }

            InitConfigSelect();
            if (ConfigSelectBox.Items.Count > 0)
                ConfigSelectBox.SelectedItem = ConfigSelectBox.Items[0];
            else
                SwitchConfig(null);
        }

        private void downloadProcess_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click_1(object sender, EventArgs e)
        {

        }

        private void modelButton_CheckedChanged(object sender, EventArgs e)
        {


        }

        private void downloadModeButton_Click(object sender, EventArgs e)
        {
            var mode = 0;
            if (selectModel == 0)
                mode = 1;
            else
                mode = 0;
            //selectModel = mode;
            SwitchDownloadMode(mode);
        }
    }
}