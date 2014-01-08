using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Awesomium.Core;

namespace ForgeBot
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelPictureContainer = new ForgeBot.NoFlickerPanel();
            this.panelButtonControls = new System.Windows.Forms.Panel();
            this.chk_ShowLog = new System.Windows.Forms.CheckBox();
            this.buttonCoords = new System.Windows.Forms.Button();
            this.buttonAutoReload = new System.Windows.Forms.CheckBox();
            this.buttonChangeView = new System.Windows.Forms.CheckBox();
            this.buttonStartBot = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonPauseResume = new System.Windows.Forms.Button();
            this.labelTimer = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonSavePicture = new System.Windows.Forms.Button();
            this.pictureBoxDonate = new System.Windows.Forms.PictureBox();
            this.labelAuthor = new System.Windows.Forms.Label();
            this.btnReload = new System.Windows.Forms.Button();
            this.buttonSettings = new System.Windows.Forms.Button();
            this.webControl1 = new Awesomium.Windows.Forms.WebControl();
            this.timerBotCountdown = new System.Windows.Forms.Timer(this.components);
            this.webCoreTimer = new System.Windows.Forms.Timer(this.components);
            this.timerPauseCountdown = new System.Windows.Forms.Timer(this.components);
            this.timerAutoRefresh = new System.Windows.Forms.Timer(this.components);
            this.timerAutoStartBot = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelButtonControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDonate)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
            this.splitContainer1.Panel1.Controls.Add(this.panelPictureContainer);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panelButtonControls);
            this.splitContainer1.TabStop = false;
            // 
            // panelPictureContainer
            // 
            resources.ApplyResources(this.panelPictureContainer, "panelPictureContainer");
            this.panelPictureContainer.Name = "panelPictureContainer";
            // 
            // panelButtonControls
            // 
            this.panelButtonControls.Controls.Add(this.chk_ShowLog);
            this.panelButtonControls.Controls.Add(this.buttonCoords);
            this.panelButtonControls.Controls.Add(this.buttonAutoReload);
            this.panelButtonControls.Controls.Add(this.buttonChangeView);
            this.panelButtonControls.Controls.Add(this.buttonStartBot);
            this.panelButtonControls.Controls.Add(this.button1);
            this.panelButtonControls.Controls.Add(this.textBox1);
            this.panelButtonControls.Controls.Add(this.buttonPauseResume);
            this.panelButtonControls.Controls.Add(this.labelTimer);
            this.panelButtonControls.Controls.Add(this.labelStatus);
            this.panelButtonControls.Controls.Add(this.label1);
            this.panelButtonControls.Controls.Add(this.buttonSavePicture);
            this.panelButtonControls.Controls.Add(this.pictureBoxDonate);
            this.panelButtonControls.Controls.Add(this.labelAuthor);
            this.panelButtonControls.Controls.Add(this.btnReload);
            this.panelButtonControls.Controls.Add(this.buttonSettings);
            resources.ApplyResources(this.panelButtonControls, "panelButtonControls");
            this.panelButtonControls.Name = "panelButtonControls";
            // 
            // chk_ShowLog
            // 
            resources.ApplyResources(this.chk_ShowLog, "chk_ShowLog");
            this.chk_ShowLog.Name = "chk_ShowLog";
            this.chk_ShowLog.UseVisualStyleBackColor = true;
            this.chk_ShowLog.CheckedChanged += new System.EventHandler(this.BtnShowLogClick);
            // 
            // buttonCoords
            // 
            resources.ApplyResources(this.buttonCoords, "buttonCoords");
            this.buttonCoords.Name = "buttonCoords";
            this.buttonCoords.UseVisualStyleBackColor = true;
            this.buttonCoords.Click += new System.EventHandler(this.buttonCoords_Click);
            // 
            // buttonAutoReload
            // 
            resources.ApplyResources(this.buttonAutoReload, "buttonAutoReload");
            this.buttonAutoReload.Name = "buttonAutoReload";
            this.buttonAutoReload.UseVisualStyleBackColor = true;
            this.buttonAutoReload.CheckedChanged += new System.EventHandler(this.buttonAutoReload_CheckedChanged);
            // 
            // buttonChangeView
            // 
            resources.ApplyResources(this.buttonChangeView, "buttonChangeView");
            this.buttonChangeView.Name = "buttonChangeView";
            this.buttonChangeView.UseVisualStyleBackColor = true;
            this.buttonChangeView.CheckedChanged += new System.EventHandler(this.buttonChangeView_CheckedChanged);
            // 
            // buttonStartBot
            // 
            resources.ApplyResources(this.buttonStartBot, "buttonStartBot");
            this.buttonStartBot.Name = "buttonStartBot";
            this.buttonStartBot.TabStop = false;
            this.buttonStartBot.UseVisualStyleBackColor = true;
            this.buttonStartBot.Click += new System.EventHandler(this.buttonStartBot_Click);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            // 
            // buttonPauseResume
            // 
            resources.ApplyResources(this.buttonPauseResume, "buttonPauseResume");
            this.buttonPauseResume.Name = "buttonPauseResume";
            this.buttonPauseResume.TabStop = false;
            this.buttonPauseResume.UseVisualStyleBackColor = true;
            this.buttonPauseResume.Click += new System.EventHandler(this.ButtonPauseResumeClick);
            // 
            // labelTimer
            // 
            resources.ApplyResources(this.labelTimer, "labelTimer");
            this.labelTimer.BackColor = System.Drawing.Color.Transparent;
            this.labelTimer.Name = "labelTimer";
            // 
            // labelStatus
            // 
            resources.ApplyResources(this.labelStatus, "labelStatus");
            this.labelStatus.BackColor = System.Drawing.Color.Transparent;
            this.labelStatus.Name = "labelStatus";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Name = "label1";
            // 
            // buttonSavePicture
            // 
            resources.ApplyResources(this.buttonSavePicture, "buttonSavePicture");
            this.buttonSavePicture.Name = "buttonSavePicture";
            this.buttonSavePicture.TabStop = false;
            this.buttonSavePicture.UseVisualStyleBackColor = true;
            this.buttonSavePicture.Click += new System.EventHandler(this.ButtonSavePictureClick);
            // 
            // pictureBoxDonate
            // 
            this.pictureBoxDonate.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxDonate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxDonate.Image = global::ForgeBot.Resource.DonateButton;
            resources.ApplyResources(this.pictureBoxDonate, "pictureBoxDonate");
            this.pictureBoxDonate.Name = "pictureBoxDonate";
            this.pictureBoxDonate.TabStop = false;
            this.pictureBoxDonate.Click += new System.EventHandler(this.PictureBox2Click);
            // 
            // labelAuthor
            // 
            resources.ApplyResources(this.labelAuthor, "labelAuthor");
            this.labelAuthor.BackColor = System.Drawing.Color.Transparent;
            this.labelAuthor.Name = "labelAuthor";
            // 
            // btnReload
            // 
            resources.ApplyResources(this.btnReload, "btnReload");
            this.btnReload.Name = "btnReload";
            this.btnReload.TabStop = false;
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.BtnReloadClick);
            // 
            // buttonSettings
            // 
            resources.ApplyResources(this.buttonSettings, "buttonSettings");
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.TabStop = false;
            this.buttonSettings.UseVisualStyleBackColor = true;
            this.buttonSettings.Click += new System.EventHandler(this.ButtonSettingsClick);
            // 
            // webControl1
            // 
            resources.ApplyResources(this.webControl1, "webControl1");
            this.webControl1.Name = "webControl1";
// TODO: Code generation for '' failed because of Exception 'Invalid Primitive Type: System.IntPtr. Consider using CodeObjectCreateExpression.'.
            // 
            // timerBotCountdown
            // 
            this.timerBotCountdown.Interval = 1000;
            this.timerBotCountdown.Tick += new System.EventHandler(this.TimerBotCountdownTick);
            // 
            // webCoreTimer
            // 
            this.webCoreTimer.Interval = 1000;
            this.webCoreTimer.Tick += new System.EventHandler(this.webCoreTimer_Tick);
            // 
            // timerPauseCountdown
            // 
            this.timerPauseCountdown.Interval = 1000;
            this.timerPauseCountdown.Tick += new System.EventHandler(this.TimerPauseCountdownTick);
            // 
            // timerAutoRefresh
            // 
            this.timerAutoRefresh.Interval = 60000;
            // 
            // timerAutoStartBot
            // 
            this.timerAutoStartBot.Interval = 10000;
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.webControl1);
            this.Name = "MainForm";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panelButtonControls.ResumeLayout(false);
            this.panelButtonControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDonate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Timer timerBotCountdown;
        private Button buttonPauseResume;
        private Button buttonSavePicture;
        private Button buttonSettings;
        private Button buttonStartBot;
        private Label labelAuthor;
        private Label labelStatus;
        private Label labelTimer;
        private Awesomium.Windows.Forms.WebControl webControl1;
        private Button btnReload;
        private PictureBox pictureBoxDonate;
        private Label label1;
        private Panel panelButtonControls;
        private TextBox textBox1;
        private SplitContainer splitContainer1;
        private Button button1;
        private NoFlickerPanel panelPictureContainer;
        private Timer webCoreTimer;
        private Timer timerPauseCountdown;
        private Timer timerAutoRefresh;
        private CheckBox buttonChangeView;
        private CheckBox buttonAutoReload;
        private CheckBox chk_ShowLog;
        private Timer timerAutoStartBot;
        private Button buttonCoords;

    }
}