using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ForgeBot
{
    public partial class SettingsForm : Form
    {
        private Coordinate crdGoodsItem;
        private Coordinate crdSupplyItem;

        private MainForm _mainForm;
        private ApplicationSettings _settings;

        private int _oldSupplyDistanceX, _oldSupplyDistanceY, _oldCollectDistanceY;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            panel1.BackgroundImage = Resource.panelBckgr;
        }

        public SettingsForm(MainForm mainForm, ApplicationSettings settings)
        {
            InitializeComponent();

            _mainForm = mainForm;
            _settings = settings;

            numericUpDown1.Value = _settings.CheckEveryXMinutes;
            checkBoxSupply.Checked = _settings.DoSupplyClicks;
            checkBoxGoods.Checked = _settings.DoGoodsClicks;
            checkBoxMilitary.Checked = _settings.DoMilitaryUnitsClicks;
            //this.checkBoxMilitaryUnblock.Checked = _settings.DoMilitaryUnitsUnblock;
            checkBoxPlungedItemsClick.Checked = _settings.DoPlungedItemsClick;
            numericUpDown_WaitBeforeProduction.Value = _settings.WaitBeforeProductionInMillis/1000;
            //numericUpDown_Popup.Value = _settings.WaitOnPopupInMillis/1000;
            numericUpDownBotScreenWidth.Value = _settings.BotPictureWidth;
            numericUpDownBotScreenHeight.Value = _settings.BotPictureHeight;
            numericUpDown_MaxItemsClicked.Value = _settings.MaxItemsClickedInARow;
            numericUpDown_PrcRndCheck.Value = _settings.RndPercentCheckEveryXMinutes;
            numericUpDown_PrcRndClick.Value = _settings.RndPercentDontCollectInARow;
            //checkBoxVersionCheck.Checked = _settings.VersionCheck;
            checkBoxShowLogOnStartup.Checked = _settings.ShowLogWindowOnStartup;
            numericUpDownAutoLogin.Value = _settings.ReloadAfterXTimesNotRecognized;
            numericUpDownPauseCountdown.Value = _settings.PauseCountdownForXSeconds;
            numericUpDownAutoRefresh.Value = _settings.AutoRefreshAfterXMinutes;
            tb_UserAgent.Text = _settings.BrowserUserAgent;

            string name = _settings.SupplyItemCoords.Name;
            if (name == "5m")
            {
                radioButton5m.Checked = true;
            }
            else if (name == "15m")
            {
                radioButton15m.Checked = true;
            }
            else if (name == "1h")
            {
                radioButton1h.Checked = true;
            }
            else if (name == "4h")
            {
                radioButton4h.Checked = true;
            }
            else if (name == "8h")
            {
                radioButton8h.Checked = true;
            }
            else if (name == "1d")
            {
                radioButton1d.Checked = true;
            }
            else
            {
                //MessageBox.Show("error initializing settings: supply item type");
            }
            name = _settings.GoodsItemCoords.Name;
            if (name == "4h")
            {
                radioButtonGoods4h.Checked = true;
            }
            else if (name == "8h")
            {
                radioButtonGoods8h.Checked = true;
            }
            else if (name == "1d")
            {
                radioButtonGoods1d.Checked = true;
            }
            else if (name == "2d")
            {
                radioButtonGoods2d.Checked = true;
            }
            else
            {
                //MessageBox.Show("error initializing settings: goods item type");
            }

            var language = _settings.ApplicationLanguage;
            switch (language)
            {
                case "it-IT":
                    comboBoxLanguage.SelectedIndex = 2;
                    break;
                case "en-US":
                    comboBoxLanguage.SelectedIndex = 0;
                    break;
                case "de-DE":
                    comboBoxLanguage.SelectedIndex = 1;
                    break;
                case "ru-RU":
                    comboBoxLanguage.SelectedIndex = 3;
                    break;
                case "pl-PL":
                    comboBoxLanguage.SelectedIndex = 4;
                    break;
                default:
                    comboBoxLanguage.SelectedIndex = 0;
                    //MessageBox.Show("error initializing settings: application language");
                    break;
            }

            crdSupplyItem = _settings.SupplyItemCoords;
            numericUpDownCollectDistance.Value = _settings.CollectDistanceY;
            _oldCollectDistanceY = _settings.CollectDistanceY;
            numericUpDownSypplyDistance.Value = _settings.SupplyDistanceY;
            _oldSupplyDistanceY = _settings.SupplyDistanceY;
            numericUpDownSypplyDistanceX.Value = _settings.SupplyDistanceX;
            _oldSupplyDistanceX = _settings.SupplyDistanceX;
            checkBoxSaveScanResults.Checked = _settings.SaveScanResults;
            int index = comboBoxFoEServer.FindStringExact(_settings.FoEServer);
            comboBoxFoEServer.SelectedIndex = index < 0 ? 0 : index;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            _settings.CheckEveryXMinutes = (int) numericUpDown1.Value;
            _settings.DoSupplyClicks = checkBoxSupply.Checked;
            _settings.DoGoodsClicks = checkBoxGoods.Checked;
            _settings.DoMilitaryUnitsClicks = checkBoxMilitary.Checked;
            //_settings.DoMilitaryUnitsUnblock = this.checkBoxMilitaryUnblock.Checked;
            //_settings.VersionCheck = checkBoxVersionCheck.Checked;
            _settings.ShowLogWindowOnStartup = checkBoxShowLogOnStartup.Checked; 
            _settings.DoPlungedItemsClick = checkBoxPlungedItemsClick.Checked;
            _settings.WaitBeforeProductionInMillis = ((int) numericUpDown_WaitBeforeProduction.Value)*1000;
            //_settings.WaitOnPopupInMillis = ((int) numericUpDown_Popup.Value)*1000;
            _settings.SupplyItemCoords = crdSupplyItem;
            _settings.GoodsItemCoords = crdGoodsItem;
            _settings.CollectDistanceY = (int) numericUpDownCollectDistance.Value;
            _settings.SupplyDistanceY = (int) numericUpDownSypplyDistance.Value;
            _settings.SupplyDistanceX = (int) numericUpDownSypplyDistanceX.Value;
            _settings.SaveScanResults = checkBoxSaveScanResults.Checked;
            _settings.BotPictureWidth = (int) numericUpDownBotScreenWidth.Value;
            _settings.BotPictureHeight = (int) numericUpDownBotScreenHeight.Value;
            _settings.MaxItemsClickedInARow = (int) numericUpDown_MaxItemsClicked.Value;
            _settings.RndPercentCheckEveryXMinutes = (int) numericUpDown_PrcRndCheck.Value;
            _settings.RndPercentDontCollectInARow = (int) numericUpDown_PrcRndClick.Value;
            _settings.ReloadAfterXTimesNotRecognized = (int) numericUpDownAutoLogin.Value;
            _settings.PauseCountdownForXSeconds = (int) numericUpDownPauseCountdown.Value;
            _settings.AutoRefreshAfterXMinutes = (int)numericUpDownAutoRefresh.Value;
            _settings.FoEServer = comboBoxFoEServer.SelectedItem.ToString();
            _settings.ApplicationLanguage = DetectCultureFromCombo();
            _settings.BrowserUserAgent = tb_UserAgent.Text;

            if(_oldSupplyDistanceY!=_settings.SupplyDistanceY || _oldSupplyDistanceX!=_settings.SupplyDistanceX ||
                _oldCollectDistanceY!=_settings.CollectDistanceY)
                ItemCollection.EraseAllItems();
            _settings.CoordinateData = ItemCollection.BuildCoordinateData();

            _settings.SaveSettings();
            base.Close();
        }

        private void radioButton15m_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                crdSupplyItem = ApplicationSettings.SUPPLY_15m;
            }
        }

        private void radioButton1d_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
                if (radioButton1d.Checked)
                {
                    crdSupplyItem = ApplicationSettings.SUPPLY_1d;
                }
        }

        private void radioButton1h_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                crdSupplyItem = ApplicationSettings.SUPPLY_1h;
            }
        }

        private void radioButton4h_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                crdSupplyItem = ApplicationSettings.SUPPLY_4h;
            }
        }

        private void radioButton5m_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                crdSupplyItem = ApplicationSettings.SUPPLY_5m;
            }
        }

        private void radioButton8h_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton) sender).Checked)
            {
                crdSupplyItem = ApplicationSettings.SUPPLY_8h;
            }
        }

        private void radioButtonGoods4h_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
                crdGoodsItem = ApplicationSettings.GOODS_4h;
        }

        private void radioButtonGoods8h_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
                crdGoodsItem = ApplicationSettings.GOODS_8h;
        }

        private void radioButtonGoods1d_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
                crdGoodsItem = ApplicationSettings.GOODS_1d;
        }

        private void radioButtonGoods2d_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
                crdGoodsItem = ApplicationSettings.GOODS_2d;
        }

        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Dispose();
        }

        private void btnResetWindowPositions_Click(object sender, EventArgs e)
        {
            _settings.MainFormInfo = MainForm.DefaultFormPositionAndSize;
            _settings.MessageLogFormInfo = MessagesForm.DefaultFormPositionAndSize;
            _mainForm.ApplyNewWindowsPosition();
        }

        private void comboBoxLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            var culture = new CultureInfo(DetectCultureFromCombo());
            Thread.CurrentThread.CurrentUICulture=culture;

            ApplyCulture2Form(_mainForm, typeof(MainForm));
            ApplyCulture2Form(this, typeof(SettingsForm));
        }

        private void ApplyCulture2Form(Form formInstance, Type formType)
        {
            var crm = new ComponentResourceManager(formType);
            foreach (var control in GetAll(formInstance))
            {
                if(control is Button)
                {
                    var ctrl = control as Button;
                    var enabled = ctrl.Enabled;
                    crm.ApplyResources(ctrl, ctrl.Name);
                    ctrl.Enabled=enabled;
                }
                else if (control is Label)
                {
                    var ctrl = control as Label;
                    crm.ApplyResources(ctrl, ctrl.Name);
                }
                else if (control is CheckBox)
                {
                    var ctrl = control as CheckBox;
                    var enabled = ctrl.Enabled;
                    var chck = ctrl.Checked;
                    crm.ApplyResources(ctrl, ctrl.Name);
                    ctrl.Enabled = enabled;
                    ctrl.Checked = chck;
                }
                else if (control is GroupBox)
                {
                    var ctrl = control as GroupBox;
                    crm.ApplyResources(ctrl, ctrl.Name);
                }
                else if (control is Form)
                {
                    var ctrl = control as Form;
                    crm.ApplyResources(ctrl, ctrl.Name);
                }
                else if (control is RadioButton)
                {
                    var ctrl = control as RadioButton;
                    var enabled = ctrl.Enabled;
                    var chck = ctrl.Checked;
                    crm.ApplyResources(ctrl, ctrl.Name);
                    ctrl.Enabled = enabled;
                    ctrl.Checked = chck;
                }
            }
        }

        public IEnumerable<Control> GetAll(Control parent)
        {
            var controls = new List<Control>();

            foreach (Control child in parent.Controls)
            {
                controls.AddRange(GetAll(child));
            }

            controls.Add(parent);

            return controls;
        }



        private string DetectCultureFromCombo()
        {
            var language = comboBoxLanguage.SelectedIndex;
            switch (language)
            {
                case 2:
                    return "it-IT";
                case 0:
                    return "en-US";
                case 1:
                    return "de-DE";
                case 3:
                    return "ru-RU";
                case 4:
                    return "pl-PL";
                default:
                    return "en-US";
            }

        }
    }
}