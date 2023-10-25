using HML;
namespace MyDownload
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            startDownloadButton = new Button();
            progressBar1 = new ProgressBar();
            RemoteURLTextBox = new TextBox();
            remoteURLLabel = new Label();
            configURLlabel = new Label();
            ConfigURLTextbox = new TextBox();
            savePathTextbox = new TextBox();
            localURLLabel = new Label();
            angleLabel = new AngleLabel();
            toolTip1 = new ToolTip(components);
            ReadConfigButton = new Button();
            OpenConfigFileButton = new Button();
            remoteButton = new Button();
            SelectPathButton = new Button();
            MessageBoxText = new TextBox();
            OpenPathButton = new Button();
            ConfigSelectBox = new ComboBox();
            label4 = new Label();
            SaveNewConfigButton = new Button();
            OutputRecord = new Button();
            ClearRecordButton = new Button();
            DelectConfigButton = new Button();
            downloadProcess = new Label();
            downloadSpeed = new Label();
            downloadModeButton = new Button();
            SuspendLayout();
            // 
            // startDownloadButton
            // 
            startDownloadButton.BackColor = SystemColors.ActiveCaption;
            startDownloadButton.ForeColor = Color.Crimson;
            startDownloadButton.Location = new Point(784, 56);
            startDownloadButton.Name = "startDownloadButton";
            startDownloadButton.Size = new Size(135, 75);
            startDownloadButton.TabIndex = 1;
            startDownloadButton.Text = "开始下载";
            startDownloadButton.UseVisualStyleBackColor = false;
            startDownloadButton.Click += StartDownload;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(719, 188);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(200, 23);
            progressBar1.TabIndex = 3;
            progressBar1.Value = 1;
            progressBar1.Click += progressBar1_Click;
            // 
            // RemoteURLTextBox
            // 
            RemoteURLTextBox.Location = new Point(136, 66);
            RemoteURLTextBox.Name = "RemoteURLTextBox";
            RemoteURLTextBox.Size = new Size(347, 23);
            RemoteURLTextBox.TabIndex = 5;
            RemoteURLTextBox.TextChanged += textBox1_TextChanged;
            // 
            // remoteURLLabel
            // 
            remoteURLLabel.AutoSize = true;
            remoteURLLabel.Location = new Point(27, 69);
            remoteURLLabel.Name = "remoteURLLabel";
            remoteURLLabel.Size = new Size(83, 17);
            remoteURLLabel.TabIndex = 7;
            remoteURLLabel.Text = "资源获取网址:";
            remoteURLLabel.Click += label2_Click;
            // 
            // configURLlabel
            // 
            configURLlabel.AutoSize = true;
            configURLlabel.Location = new Point(27, 114);
            configURLlabel.Name = "configURLlabel";
            configURLlabel.Size = new Size(115, 17);
            configURLlabel.TabIndex = 8;
            configURLlabel.Text = "资源配置文件URL：";
            configURLlabel.Click += label1_Click;
            // 
            // ConfigURLTextbox
            // 
            ConfigURLTextbox.Location = new Point(136, 111);
            ConfigURLTextbox.Name = "ConfigURLTextbox";
            ConfigURLTextbox.Size = new Size(347, 23);
            ConfigURLTextbox.TabIndex = 9;
            ConfigURLTextbox.TextChanged += ConfigURLTextbox_TextChanged;
            // 
            // savePathTextbox
            // 
            savePathTextbox.AutoCompleteMode = AutoCompleteMode.Append;
            savePathTextbox.AutoCompleteSource = AutoCompleteSource.FileSystem;
            savePathTextbox.Location = new Point(136, 152);
            savePathTextbox.Name = "savePathTextbox";
            savePathTextbox.Size = new Size(347, 23);
            savePathTextbox.TabIndex = 11;
            savePathTextbox.TextChanged += SavePathTextChanged;
            // 
            // localURLLabel
            // 
            localURLLabel.AutoSize = true;
            localURLLabel.Location = new Point(27, 155);
            localURLLabel.Name = "localURLLabel";
            localURLLabel.Size = new Size(91, 17);
            localURLLabel.TabIndex = 10;
            localURLLabel.Text = "本地存储URL：";
            localURLLabel.Click += label3_Click;
            // 
            // angleLabel
            // 
            angleLabel.Location = new Point(0, 0);
            angleLabel.Name = "angleLabel";
            angleLabel.Size = new Size(0, 0);
            angleLabel.TabIndex = 0;
            // 
            // toolTip1
            // 
            toolTip1.Popup += toolTip1_Popup;
            // 
            // ReadConfigButton
            // 
            ReadConfigButton.Location = new Point(613, 111);
            ReadConfigButton.Name = "ReadConfigButton";
            ReadConfigButton.Size = new Size(75, 23);
            ReadConfigButton.TabIndex = 12;
            ReadConfigButton.Text = "读取配置";
            ReadConfigButton.UseVisualStyleBackColor = true;
            ReadConfigButton.Click += ReadConfigFromURL;
            // 
            // OpenConfigFileButton
            // 
            OpenConfigFileButton.Location = new Point(517, 111);
            OpenConfigFileButton.Name = "OpenConfigFileButton";
            OpenConfigFileButton.Size = new Size(86, 23);
            OpenConfigFileButton.TabIndex = 13;
            OpenConfigFileButton.Text = "打开";
            OpenConfigFileButton.UseVisualStyleBackColor = true;
            OpenConfigFileButton.Click += OnResourceConfigURLButton;
            // 
            // remoteButton
            // 
            remoteButton.Location = new Point(517, 66);
            remoteButton.Name = "remoteButton";
            remoteButton.Size = new Size(86, 23);
            remoteButton.TabIndex = 14;
            remoteButton.Text = "打开";
            remoteButton.UseVisualStyleBackColor = true;
            remoteButton.Click += OnRemoteURLButton;
            // 
            // SelectPathButton
            // 
            SelectPathButton.Location = new Point(517, 152);
            SelectPathButton.Name = "SelectPathButton";
            SelectPathButton.Size = new Size(86, 23);
            SelectPathButton.TabIndex = 15;
            SelectPathButton.Text = "选择文件夹";
            SelectPathButton.UseVisualStyleBackColor = true;
            SelectPathButton.Click += OnOpenSavePathButton;
            // 
            // MessageBoxText
            // 
            MessageBoxText.Location = new Point(27, 222);
            MessageBoxText.Multiline = true;
            MessageBoxText.Name = "MessageBoxText";
            MessageBoxText.ScrollBars = ScrollBars.Both;
            MessageBoxText.Size = new Size(892, 299);
            MessageBoxText.TabIndex = 16;
            MessageBoxText.TextChanged += textBox1_TextChanged_1;
            // 
            // OpenPathButton
            // 
            OpenPathButton.Location = new Point(613, 152);
            OpenPathButton.Name = "OpenPathButton";
            OpenPathButton.Size = new Size(75, 23);
            OpenPathButton.TabIndex = 17;
            OpenPathButton.Text = "打开路径";
            OpenPathButton.UseVisualStyleBackColor = true;
            OpenPathButton.Click += OpenPathButton_Click;
            // 
            // ConfigSelectBox
            // 
            ConfigSelectBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ConfigSelectBox.FormattingEnabled = true;
            ConfigSelectBox.Location = new Point(149, 191);
            ConfigSelectBox.Name = "ConfigSelectBox";
            ConfigSelectBox.Size = new Size(334, 25);
            ConfigSelectBox.TabIndex = 19;
            ConfigSelectBox.SelectedIndexChanged += ConfigSelectBox_SelectedIndexChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(26, 194);
            label4.Name = "label4";
            label4.Size = new Size(116, 17);
            label4.TabIndex = 20;
            label4.Text = "当前选择的配置文件";
            label4.Click += label4_Click;
            // 
            // SaveNewConfigButton
            // 
            SaveNewConfigButton.Location = new Point(517, 191);
            SaveNewConfigButton.Name = "SaveNewConfigButton";
            SaveNewConfigButton.Size = new Size(75, 23);
            SaveNewConfigButton.TabIndex = 21;
            SaveNewConfigButton.Text = "另存为";
            SaveNewConfigButton.UseVisualStyleBackColor = true;
            SaveNewConfigButton.Click += SaveNewConfigButton_Click;
            // 
            // OutputRecord
            // 
            OutputRecord.Location = new Point(446, 528);
            OutputRecord.Name = "OutputRecord";
            OutputRecord.Size = new Size(109, 23);
            OutputRecord.TabIndex = 22;
            OutputRecord.Text = "输出日志并打开";
            OutputRecord.UseVisualStyleBackColor = true;
            OutputRecord.Click += OutputRecord_Click;
            // 
            // ClearRecordButton
            // 
            ClearRecordButton.Location = new Point(342, 528);
            ClearRecordButton.Name = "ClearRecordButton";
            ClearRecordButton.Size = new Size(75, 23);
            ClearRecordButton.TabIndex = 23;
            ClearRecordButton.Text = "清除日志";
            ClearRecordButton.UseVisualStyleBackColor = true;
            ClearRecordButton.Click += ClearRecordButton_Click;
            // 
            // DelectConfigButton
            // 
            DelectConfigButton.Location = new Point(613, 191);
            DelectConfigButton.Name = "DelectConfigButton";
            DelectConfigButton.Size = new Size(81, 23);
            DelectConfigButton.TabIndex = 24;
            DelectConfigButton.Text = "删除该配置";
            DelectConfigButton.UseVisualStyleBackColor = true;
            DelectConfigButton.Click += DelectConfigButton_Click;
            // 
            // downloadProcess
            // 
            downloadProcess.AutoSize = true;
            downloadProcess.Location = new Point(825, 168);
            downloadProcess.Name = "downloadProcess";
            downloadProcess.Size = new Size(15, 17);
            downloadProcess.TabIndex = 25;
            downloadProcess.Text = "0";
            downloadProcess.Click += downloadProcess_Click;
            // 
            // downloadSpeed
            // 
            downloadSpeed.AutoSize = true;
            downloadSpeed.Location = new Point(719, 168);
            downloadSpeed.Name = "downloadSpeed";
            downloadSpeed.Size = new Size(15, 17);
            downloadSpeed.TabIndex = 26;
            downloadSpeed.Text = "0";
            downloadSpeed.Click += label5_Click;
            // 
            // downloadModeButton
            // 
            downloadModeButton.Location = new Point(613, 66);
            downloadModeButton.Name = "downloadModeButton";
            downloadModeButton.Size = new Size(75, 23);
            downloadModeButton.TabIndex = 28;
            downloadModeButton.Text = "button1";
            downloadModeButton.UseVisualStyleBackColor = true;
            downloadModeButton.Click += downloadModeButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(957, 563);
            Controls.Add(downloadModeButton);
            Controls.Add(downloadSpeed);
            Controls.Add(downloadProcess);
            Controls.Add(DelectConfigButton);
            Controls.Add(ClearRecordButton);
            Controls.Add(OutputRecord);
            Controls.Add(SaveNewConfigButton);
            Controls.Add(label4);
            Controls.Add(ConfigSelectBox);
            Controls.Add(OpenPathButton);
            Controls.Add(MessageBoxText);
            Controls.Add(SelectPathButton);
            Controls.Add(remoteButton);
            Controls.Add(OpenConfigFileButton);
            Controls.Add(ReadConfigButton);
            Controls.Add(savePathTextbox);
            Controls.Add(localURLLabel);
            Controls.Add(ConfigURLTextbox);
            Controls.Add(configURLlabel);
            Controls.Add(remoteURLLabel);
            Controls.Add(RemoteURLTextBox);
            Controls.Add(progressBar1);
            Controls.Add(startDownloadButton);
            Name = "Form1";
            Text = "没什么用的下载器";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button startDownloadButton;
        private ProgressBar progressBar1;
        private TextBox RemoteURLTextBox;
        private Label remoteURLLabel;
        private Label configURLlabel;
        private TextBox ConfigURLTextbox;
        private TextBox savePathTextbox;
        private Label localURLLabel;
        AngleLabel angleLabel;
        private ToolTip toolTip1;
        private Button ReadConfigButton;
        private Button OpenConfigFileButton;
        private Button remoteButton;
        private Button SelectPathButton;
        private TextBox MessageBoxText;
        private Button OpenPathButton;
        private ComboBox ConfigSelectBox;
        private Label label4;
        private Button SaveNewConfigButton;
        private Button OutputRecord;
        private Button ClearRecordButton;
        private Button DelectConfigButton;
        private Label downloadProcess;
        private Label downloadSpeed;
        private Button downloadModeButton;
    }
}