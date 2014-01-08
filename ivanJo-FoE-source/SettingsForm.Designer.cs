using System;
using System.Drawing;
using System.Windows.Forms;

namespace ForgeBot
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private Button buttonClose;
        private Button buttonSave;
        private CheckBox checkBoxSaveScanResults;
        private CheckBox checkBoxSupply;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private NumericUpDown numericUpDown1;
        private NumericUpDown numericUpDown_WaitBeforeProduction;
        private NumericUpDown numericUpDownCollectDistance;
        private NumericUpDown numericUpDownSypplyDistance;
        private RadioButton radioButton15m;
        private RadioButton radioButton1d;
        private RadioButton radioButton1h;
        private RadioButton radioButton4h;
        private RadioButton radioButton5m;
        private RadioButton radioButton8h;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.checkBoxSupply = new System.Windows.Forms.CheckBox();
            this.numericUpDown_WaitBeforeProduction = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton1d = new System.Windows.Forms.RadioButton();
            this.radioButton8h = new System.Windows.Forms.RadioButton();
            this.radioButton4h = new System.Windows.Forms.RadioButton();
            this.radioButton1h = new System.Windows.Forms.RadioButton();
            this.radioButton15m = new System.Windows.Forms.RadioButton();
            this.radioButton5m = new System.Windows.Forms.RadioButton();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.numericUpDownPauseCountdown = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDownBotScreenHeight = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownBotScreenWidth = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownSypplyDistanceX = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.numericUpDownSypplyDistance = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownCollectDistance = new System.Windows.Forms.NumericUpDown();
            this.checkBoxSaveScanResults = new System.Windows.Forms.CheckBox();
            this.checkBoxMilitary = new System.Windows.Forms.CheckBox();
            this.checkBoxPlungedItemsClick = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.numericUpDown_PrcRndClick = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.numericUpDown_PrcRndCheck = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.numericUpDown_MaxItemsClicked = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.checkBoxGoods = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.radioButtonGoods2d = new System.Windows.Forms.RadioButton();
            this.radioButtonGoods1d = new System.Windows.Forms.RadioButton();
            this.radioButtonGoods8h = new System.Windows.Forms.RadioButton();
            this.radioButtonGoods4h = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnResetWindowPositions = new System.Windows.Forms.Button();
            this.checkBoxShowLogOnStartup = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.numericUpDownAutoRefresh = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownAutoLogin = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.comboBoxFoEServer = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.tb_UserAgent = new System.Windows.Forms.TextBox();
            this.comboBoxLanguage = new ImageComboBox.ImageComboBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_WaitBeforeProduction)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPauseCountdown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBotScreenHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBotScreenWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSypplyDistanceX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSypplyDistance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCollectDistance)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_PrcRndClick)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_PrcRndCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_MaxItemsClicked)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAutoRefresh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAutoLogin)).BeginInit();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Name = "label1";
            // 
            // numericUpDown1
            // 
            resources.ApplyResources(this.numericUpDown1, "numericUpDown1");
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // checkBoxSupply
            // 
            resources.ApplyResources(this.checkBoxSupply, "checkBoxSupply");
            this.checkBoxSupply.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxSupply.Name = "checkBoxSupply";
            this.checkBoxSupply.UseVisualStyleBackColor = false;
            // 
            // numericUpDown_WaitBeforeProduction
            // 
            resources.ApplyResources(this.numericUpDown_WaitBeforeProduction, "numericUpDown_WaitBeforeProduction");
            this.numericUpDown_WaitBeforeProduction.Name = "numericUpDown_WaitBeforeProduction";
            this.numericUpDown_WaitBeforeProduction.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Name = "label2";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.radioButton1d);
            this.groupBox1.Controls.Add(this.radioButton8h);
            this.groupBox1.Controls.Add(this.radioButton4h);
            this.groupBox1.Controls.Add(this.radioButton1h);
            this.groupBox1.Controls.Add(this.radioButton15m);
            this.groupBox1.Controls.Add(this.radioButton5m);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // radioButton1d
            // 
            resources.ApplyResources(this.radioButton1d, "radioButton1d");
            this.radioButton1d.Name = "radioButton1d";
            this.radioButton1d.UseVisualStyleBackColor = true;
            this.radioButton1d.CheckedChanged += new System.EventHandler(this.radioButton1d_CheckedChanged);
            // 
            // radioButton8h
            // 
            resources.ApplyResources(this.radioButton8h, "radioButton8h");
            this.radioButton8h.Name = "radioButton8h";
            this.radioButton8h.UseVisualStyleBackColor = true;
            this.radioButton8h.CheckedChanged += new System.EventHandler(this.radioButton8h_CheckedChanged);
            // 
            // radioButton4h
            // 
            resources.ApplyResources(this.radioButton4h, "radioButton4h");
            this.radioButton4h.Name = "radioButton4h";
            this.radioButton4h.UseVisualStyleBackColor = true;
            this.radioButton4h.CheckedChanged += new System.EventHandler(this.radioButton4h_CheckedChanged);
            // 
            // radioButton1h
            // 
            resources.ApplyResources(this.radioButton1h, "radioButton1h");
            this.radioButton1h.Name = "radioButton1h";
            this.radioButton1h.UseVisualStyleBackColor = true;
            this.radioButton1h.CheckedChanged += new System.EventHandler(this.radioButton1h_CheckedChanged);
            // 
            // radioButton15m
            // 
            resources.ApplyResources(this.radioButton15m, "radioButton15m");
            this.radioButton15m.Name = "radioButton15m";
            this.radioButton15m.UseVisualStyleBackColor = true;
            this.radioButton15m.CheckedChanged += new System.EventHandler(this.radioButton15m_CheckedChanged);
            // 
            // radioButton5m
            // 
            resources.ApplyResources(this.radioButton5m, "radioButton5m");
            this.radioButton5m.Name = "radioButton5m";
            this.radioButton5m.UseVisualStyleBackColor = true;
            this.radioButton5m.CheckedChanged += new System.EventHandler(this.radioButton5m_CheckedChanged);
            // 
            // buttonClose
            // 
            resources.ApplyResources(this.buttonClose, "buttonClose");
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonSave
            // 
            resources.ApplyResources(this.buttonSave, "buttonSave");
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.numericUpDownPauseCountdown);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.numericUpDownBotScreenHeight);
            this.groupBox2.Controls.Add(this.numericUpDownBotScreenWidth);
            this.groupBox2.Controls.Add(this.numericUpDownSypplyDistanceX);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.numericUpDownSypplyDistance);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.numericUpDownCollectDistance);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // numericUpDownPauseCountdown
            // 
            resources.ApplyResources(this.numericUpDownPauseCountdown, "numericUpDownPauseCountdown");
            this.numericUpDownPauseCountdown.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numericUpDownPauseCountdown.Name = "numericUpDownPauseCountdown";
            this.numericUpDownPauseCountdown.Value = new decimal(new int[] {
            130,
            0,
            0,
            0});
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // numericUpDownBotScreenHeight
            // 
            resources.ApplyResources(this.numericUpDownBotScreenHeight, "numericUpDownBotScreenHeight");
            this.numericUpDownBotScreenHeight.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownBotScreenHeight.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDownBotScreenHeight.Minimum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numericUpDownBotScreenHeight.Name = "numericUpDownBotScreenHeight";
            this.numericUpDownBotScreenHeight.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // 
            // numericUpDownBotScreenWidth
            // 
            resources.ApplyResources(this.numericUpDownBotScreenWidth, "numericUpDownBotScreenWidth");
            this.numericUpDownBotScreenWidth.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownBotScreenWidth.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.numericUpDownBotScreenWidth.Minimum = new decimal(new int[] {
            550,
            0,
            0,
            0});
            this.numericUpDownBotScreenWidth.Name = "numericUpDownBotScreenWidth";
            this.numericUpDownBotScreenWidth.Value = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            // 
            // numericUpDownSypplyDistanceX
            // 
            resources.ApplyResources(this.numericUpDownSypplyDistanceX, "numericUpDownSypplyDistanceX");
            this.numericUpDownSypplyDistanceX.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDownSypplyDistanceX.Name = "numericUpDownSypplyDistanceX";
            this.numericUpDownSypplyDistanceX.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // numericUpDownSypplyDistance
            // 
            resources.ApplyResources(this.numericUpDownSypplyDistance, "numericUpDownSypplyDistance");
            this.numericUpDownSypplyDistance.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.numericUpDownSypplyDistance.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDownSypplyDistance.Name = "numericUpDownSypplyDistance";
            this.numericUpDownSypplyDistance.Value = new decimal(new int[] {
            130,
            0,
            0,
            0});
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // numericUpDownCollectDistance
            // 
            resources.ApplyResources(this.numericUpDownCollectDistance, "numericUpDownCollectDistance");
            this.numericUpDownCollectDistance.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.numericUpDownCollectDistance.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDownCollectDistance.Name = "numericUpDownCollectDistance";
            this.numericUpDownCollectDistance.Value = new decimal(new int[] {
            130,
            0,
            0,
            0});
            // 
            // checkBoxSaveScanResults
            // 
            resources.ApplyResources(this.checkBoxSaveScanResults, "checkBoxSaveScanResults");
            this.checkBoxSaveScanResults.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxSaveScanResults.Name = "checkBoxSaveScanResults";
            this.checkBoxSaveScanResults.UseVisualStyleBackColor = false;
            // 
            // checkBoxMilitary
            // 
            resources.ApplyResources(this.checkBoxMilitary, "checkBoxMilitary");
            this.checkBoxMilitary.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxMilitary.Name = "checkBoxMilitary";
            this.checkBoxMilitary.UseVisualStyleBackColor = false;
            // 
            // checkBoxPlungedItemsClick
            // 
            resources.ApplyResources(this.checkBoxPlungedItemsClick, "checkBoxPlungedItemsClick");
            this.checkBoxPlungedItemsClick.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxPlungedItemsClick.Name = "checkBoxPlungedItemsClick";
            this.checkBoxPlungedItemsClick.UseVisualStyleBackColor = false;
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.numericUpDown_PrcRndClick);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.numericUpDown_PrcRndCheck);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.numericUpDown_MaxItemsClicked);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // numericUpDown_PrcRndClick
            // 
            resources.ApplyResources(this.numericUpDown_PrcRndClick, "numericUpDown_PrcRndClick");
            this.numericUpDown_PrcRndClick.Name = "numericUpDown_PrcRndClick";
            this.numericUpDown_PrcRndClick.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // numericUpDown_PrcRndCheck
            // 
            resources.ApplyResources(this.numericUpDown_PrcRndCheck, "numericUpDown_PrcRndCheck");
            this.numericUpDown_PrcRndCheck.Name = "numericUpDown_PrcRndCheck";
            this.numericUpDown_PrcRndCheck.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // numericUpDown_MaxItemsClicked
            // 
            resources.ApplyResources(this.numericUpDown_MaxItemsClicked, "numericUpDown_MaxItemsClicked");
            this.numericUpDown_MaxItemsClicked.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_MaxItemsClicked.Name = "numericUpDown_MaxItemsClicked";
            this.numericUpDown_MaxItemsClicked.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.Transparent;
            this.groupBox4.Controls.Add(this.checkBoxGoods);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.numericUpDown1);
            this.groupBox4.Controls.Add(this.checkBoxSaveScanResults);
            this.groupBox4.Controls.Add(this.checkBoxPlungedItemsClick);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.checkBoxMilitary);
            this.groupBox4.Controls.Add(this.numericUpDown_WaitBeforeProduction);
            this.groupBox4.Controls.Add(this.checkBoxSupply);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // checkBoxGoods
            // 
            resources.ApplyResources(this.checkBoxGoods, "checkBoxGoods");
            this.checkBoxGoods.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxGoods.Name = "checkBoxGoods";
            this.checkBoxGoods.UseVisualStyleBackColor = false;
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.Transparent;
            this.groupBox5.Controls.Add(this.radioButtonGoods2d);
            this.groupBox5.Controls.Add(this.radioButtonGoods1d);
            this.groupBox5.Controls.Add(this.radioButtonGoods8h);
            this.groupBox5.Controls.Add(this.radioButtonGoods4h);
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // radioButtonGoods2d
            // 
            resources.ApplyResources(this.radioButtonGoods2d, "radioButtonGoods2d");
            this.radioButtonGoods2d.Name = "radioButtonGoods2d";
            this.radioButtonGoods2d.UseVisualStyleBackColor = true;
            this.radioButtonGoods2d.CheckedChanged += new System.EventHandler(this.radioButtonGoods2d_CheckedChanged);
            // 
            // radioButtonGoods1d
            // 
            resources.ApplyResources(this.radioButtonGoods1d, "radioButtonGoods1d");
            this.radioButtonGoods1d.Name = "radioButtonGoods1d";
            this.radioButtonGoods1d.UseVisualStyleBackColor = true;
            this.radioButtonGoods1d.CheckedChanged += new System.EventHandler(this.radioButtonGoods1d_CheckedChanged);
            // 
            // radioButtonGoods8h
            // 
            resources.ApplyResources(this.radioButtonGoods8h, "radioButtonGoods8h");
            this.radioButtonGoods8h.Name = "radioButtonGoods8h";
            this.radioButtonGoods8h.UseVisualStyleBackColor = true;
            this.radioButtonGoods8h.CheckedChanged += new System.EventHandler(this.radioButtonGoods8h_CheckedChanged);
            // 
            // radioButtonGoods4h
            // 
            resources.ApplyResources(this.radioButtonGoods4h, "radioButtonGoods4h");
            this.radioButtonGoods4h.Name = "radioButtonGoods4h";
            this.radioButtonGoods4h.UseVisualStyleBackColor = true;
            this.radioButtonGoods4h.CheckedChanged += new System.EventHandler(this.radioButtonGoods4h_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnResetWindowPositions);
            this.panel1.Controls.Add(this.buttonSave);
            this.panel1.Controls.Add(this.buttonClose);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // btnResetWindowPositions
            // 
            resources.ApplyResources(this.btnResetWindowPositions, "btnResetWindowPositions");
            this.btnResetWindowPositions.Name = "btnResetWindowPositions";
            this.btnResetWindowPositions.UseVisualStyleBackColor = true;
            this.btnResetWindowPositions.Click += new System.EventHandler(this.btnResetWindowPositions_Click);
            // 
            // checkBoxShowLogOnStartup
            // 
            resources.ApplyResources(this.checkBoxShowLogOnStartup, "checkBoxShowLogOnStartup");
            this.checkBoxShowLogOnStartup.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxShowLogOnStartup.Name = "checkBoxShowLogOnStartup";
            this.checkBoxShowLogOnStartup.UseVisualStyleBackColor = false;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.numericUpDownAutoRefresh);
            this.groupBox6.Controls.Add(this.numericUpDownAutoLogin);
            this.groupBox6.Controls.Add(this.label14);
            this.groupBox6.Controls.Add(this.checkBoxShowLogOnStartup);
            this.groupBox6.Controls.Add(this.label12);
            resources.ApplyResources(this.groupBox6, "groupBox6");
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.TabStop = false;
            // 
            // numericUpDownAutoRefresh
            // 
            resources.ApplyResources(this.numericUpDownAutoRefresh, "numericUpDownAutoRefresh");
            this.numericUpDownAutoRefresh.Maximum = new decimal(new int[] {
            1440,
            0,
            0,
            0});
            this.numericUpDownAutoRefresh.Name = "numericUpDownAutoRefresh";
            this.numericUpDownAutoRefresh.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // numericUpDownAutoLogin
            // 
            resources.ApplyResources(this.numericUpDownAutoLogin, "numericUpDownAutoLogin");
            this.numericUpDownAutoLogin.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDownAutoLogin.Name = "numericUpDownAutoLogin";
            this.numericUpDownAutoLogin.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // comboBoxFoEServer
            // 
            this.comboBoxFoEServer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFoEServer.FormattingEnabled = true;
            resources.ApplyResources(this.comboBoxFoEServer, "comboBoxFoEServer");
            this.comboBoxFoEServer.Items.AddRange(new object[] {
            resources.GetString("comboBoxFoEServer.Items"),
            resources.GetString("comboBoxFoEServer.Items1"),
            resources.GetString("comboBoxFoEServer.Items2"),
            resources.GetString("comboBoxFoEServer.Items3"),
            resources.GetString("comboBoxFoEServer.Items4"),
            resources.GetString("comboBoxFoEServer.Items5"),
            resources.GetString("comboBoxFoEServer.Items6"),
            resources.GetString("comboBoxFoEServer.Items7"),
            resources.GetString("comboBoxFoEServer.Items8"),
            resources.GetString("comboBoxFoEServer.Items9"),
            resources.GetString("comboBoxFoEServer.Items10"),
            resources.GetString("comboBoxFoEServer.Items11"),
            resources.GetString("comboBoxFoEServer.Items12"),
            resources.GetString("comboBoxFoEServer.Items13"),
            resources.GetString("comboBoxFoEServer.Items14"),
            resources.GetString("comboBoxFoEServer.Items15"),
            resources.GetString("comboBoxFoEServer.Items16"),
            resources.GetString("comboBoxFoEServer.Items17"),
            resources.GetString("comboBoxFoEServer.Items18"),
            resources.GetString("comboBoxFoEServer.Items19"),
            resources.GetString("comboBoxFoEServer.Items20"),
            resources.GetString("comboBoxFoEServer.Items21"),
            resources.GetString("comboBoxFoEServer.Items22"),
            resources.GetString("comboBoxFoEServer.Items23")});
            this.comboBoxFoEServer.Name = "comboBoxFoEServer";
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Name = "label15";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.tb_UserAgent);
            this.groupBox7.Controls.Add(this.comboBoxLanguage);
            this.groupBox7.Controls.Add(this.comboBoxFoEServer);
            this.groupBox7.Controls.Add(this.label5);
            this.groupBox7.Controls.Add(this.label15);
            resources.ApplyResources(this.groupBox7, "groupBox7");
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.TabStop = false;
            // 
            // tb_UserAgent
            // 
            resources.ApplyResources(this.tb_UserAgent, "tb_UserAgent");
            this.tb_UserAgent.Name = "tb_UserAgent";
            // 
            // comboBoxLanguage
            // 
            this.comboBoxLanguage.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLanguage.ImageList = this.imageList1;
            this.comboBoxLanguage.Indent = 0;
            resources.ApplyResources(this.comboBoxLanguage, "comboBoxLanguage");
            this.comboBoxLanguage.Items.AddRange(new ImageComboBox.ImageComboBoxItem[] {
            ((ImageComboBox.ImageComboBoxItem)(resources.GetObject("comboBoxLanguage.Items"))),
            ((ImageComboBox.ImageComboBoxItem)(resources.GetObject("comboBoxLanguage.Items1"))),
            ((ImageComboBox.ImageComboBoxItem)(resources.GetObject("comboBoxLanguage.Items2"))),
            ((ImageComboBox.ImageComboBoxItem)(resources.GetObject("comboBoxLanguage.Items3"))),
            ((ImageComboBox.ImageComboBoxItem)(resources.GetObject("comboBoxLanguage.Items4")))});
            this.comboBoxLanguage.Name = "comboBoxLanguage";
            this.comboBoxLanguage.SelectedIndexChanged += new System.EventHandler(this.comboBoxLanguage_SelectedIndexChanged);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "de.png");
            this.imageList1.Images.SetKeyName(1, "en.png");
            this.imageList1.Images.SetKeyName(2, "es.png");
            this.imageList1.Images.SetKeyName(3, "fr.png");
            this.imageList1.Images.SetKeyName(4, "it.png");
            this.imageList1.Images.SetKeyName(5, "ru.png");
            this.imageList1.Images.SetKeyName(6, "us.png");
            this.imageList1.Images.SetKeyName(7, "pl.png");
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Name = "label5";
            // 
            // SettingsForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SettingsForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_WaitBeforeProduction)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPauseCountdown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBotScreenHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBotScreenWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSypplyDistanceX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSypplyDistance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCollectDistance)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_PrcRndClick)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_PrcRndCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_MaxItemsClicked)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAutoRefresh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAutoLogin)).EndInit();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private CheckBox checkBoxMilitary;
        private CheckBox checkBoxPlungedItemsClick;
        private NumericUpDown numericUpDownSypplyDistanceX;
        private Label label7;
        private Label label8;
        private Label label6;
        private NumericUpDown numericUpDownBotScreenHeight;
        private NumericUpDown numericUpDownBotScreenWidth;
        private GroupBox groupBox3;
        private NumericUpDown numericUpDown_PrcRndCheck;
        private Label label10;
        private NumericUpDown numericUpDown_MaxItemsClicked;
        private Label label9;
        private NumericUpDown numericUpDown_PrcRndClick;
        private Label label11;
        private GroupBox groupBox4;
        private GroupBox groupBox5;
        private RadioButton radioButtonGoods1d;
        private RadioButton radioButtonGoods8h;
        private RadioButton radioButtonGoods4h;
        private RadioButton radioButtonGoods2d;
        private Panel panel1;
        private CheckBox checkBoxShowLogOnStartup;
        private GroupBox groupBox6;
        private Button btnResetWindowPositions;
        private NumericUpDown numericUpDownAutoLogin;
        private Label label12;
        private Label label13;
        private NumericUpDown numericUpDownPauseCountdown;
        private NumericUpDown numericUpDownAutoRefresh;
        private Label label14;
        private ComboBox comboBoxFoEServer;
        private Label label15;
        private CheckBox checkBoxGoods;
        private GroupBox groupBox7;
        private ImageList imageList1;
        private ImageComboBox.ImageComboBox comboBoxLanguage;
        private TextBox tb_UserAgent;
        private Label label5;
    }
}