using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ForgeBot
{
    [Serializable]
    public class ApplicationSettings
    {
        #region Delegates

        public delegate void LogWriter(string logMessage);

        #endregion

        private const string SettingsFileFilename = "settings.xml";


        public static readonly Coordinate SUPPLY_5m = new Coordinate(1810, 990, "5m");
        public static readonly Coordinate SUPPLY_15m = new Coordinate(2030, 990, "15m");
        public static readonly Coordinate SUPPLY_1h = new Coordinate(2250, 990, "1h");
        public static readonly Coordinate SUPPLY_4h = new Coordinate(1810, 1170, "4h");
        public static readonly Coordinate SUPPLY_8h = new Coordinate(2030, 1170, "8h");
        public static readonly Coordinate SUPPLY_1d = new Coordinate(2250, 1170, "1d");

        public static readonly Coordinate GOODS_4h = new Coordinate(1660, 1120, "4h");
        public static readonly Coordinate GOODS_8h = new Coordinate(1880, 1120, "8h");
        public static readonly Coordinate GOODS_1d = new Coordinate(2100, 1120, "1d");
        public static readonly Coordinate GOODS_2d = new Coordinate(2330, 1120, "2d");
        private int _botPictureHeight = 2000;
        private int _botPictureWidth = 4000;
        private int _checkEveryXMinutes = 5;
        private int _collectDistanceY = 130;
        private int _collectDistanceX = 0;
        private bool _doMilitaryUnitsClicks;
        private bool _doMilitaryUnitsUnblock;
        private bool _doPlungedItemsClick=true;
        private bool _doSupplyClicks=true;
        private bool _doGoodsClicks=true;
        private Coordinate _goodsItemCoords = GOODS_4h;
        private int _maxItemsClickedInARow = 10;
        private int _rndPercentDontCollectInARow = 20;
        private bool _saveScanResults;
        private int _supplyDistanceX;
        private int _supplyDistanceY = 130;
        private Coordinate _supplyItemCoords = SUPPLY_15m;
        private int _waitBeforeProductionInMillis = 12000;
        private int _waitOnPopupInMillis = 5000;
        private int _rndPercentCheckEveryXMinutes = 10;
        private bool _versionCheck;
        private bool _showLogWindow;
        private string _coordinateData = "";
        private int _maxLogLinesShowed = 500;
        private int _reloadAfterXTimesNotRecognized = 5;
        private int _pauseCountdownForXSeconds = 5;
        private int _autoRefreshAfterXMinutes = 0;
        private string _foEServer="Default";
        private string _browserUserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Win64; x64; Trident/4.0; .NET CLR 2.0.50727; SLCC2; .NET CLR 3.5.30729; .NET CLR 3.0.30729; .NET4.0C; .NET4.0E)";
        private string _username;
        private string _password;
        private string _lastLoadedWorld;
        private string _applicationLanguage="en-US";

        public static readonly int AutoSearchYStepping = 5;


        public int CheckEveryXMinutes
        {
            get { return _checkEveryXMinutes; }
            set { _checkEveryXMinutes = value; }
        }

        public int CollectDistanceY
        {
            get { return _collectDistanceY; }
            set { _collectDistanceY = value; }
        }

        public int CollectDistanceX
        {
            get { return _collectDistanceX; }
            set { _collectDistanceX = value; }
        }

        public bool DoSupplyClicks
        {
            get { return _doSupplyClicks; }
            set { _doSupplyClicks = value; }
        }

        public bool DoMilitaryUnitsClicks
        {
            get { return _doMilitaryUnitsClicks; }
            set { _doMilitaryUnitsClicks = value; }
        }

        public bool DoMilitaryUnitsUnblock
        {
            get { return _doMilitaryUnitsUnblock; }
            set { _doMilitaryUnitsUnblock = value; }
        }

        public bool DoPlungedItemsClick
        {
            get { return _doPlungedItemsClick; }
            set { _doPlungedItemsClick = value; }
        }

        public bool SaveScanResults
        {
            get { return _saveScanResults; }
            set { _saveScanResults = value; }
        }

        public int SupplyDistanceY
        {
            get { return _supplyDistanceY; }
            set { _supplyDistanceY = value; }
        }

        public int SupplyDistanceX
        {
            get { return _supplyDistanceX; }
            set { _supplyDistanceX = value; }
        }

        public int WaitBeforeProductionInMillis
        {
            get { return _waitBeforeProductionInMillis; }
            set { _waitBeforeProductionInMillis = value; }
        }

        public int WaitOnPopupInMillis
        {
            get { return _waitOnPopupInMillis; }
            set { _waitOnPopupInMillis = value; }
        }

        public Coordinate SupplyItemCoords
        {
            get { return _supplyItemCoords; }
            set { _supplyItemCoords = value; }
        }

        public Coordinate GoodsItemCoords
        {
            get { return _goodsItemCoords; }
            set { _goodsItemCoords = value; }
        }

        public int BotPictureWidth
        {
            get { return _botPictureWidth; }
            set { _botPictureWidth = 4000; }
        }

        public int BotPictureHeight
        {
            get { return _botPictureHeight; }
            set { _botPictureHeight = 2000; }
        }

        public int MaxItemsClickedInARow
        {
            get { return _maxItemsClickedInARow; }
            set { _maxItemsClickedInARow = value; }
        }

        public int RndPercentDontCollectInARow
        {
            get { return _rndPercentDontCollectInARow; }
            set { _rndPercentDontCollectInARow = value; }
        }

        public int RndPercentCheckEveryXMinutes
        {
            get { return _rndPercentCheckEveryXMinutes; }
            set { _rndPercentCheckEveryXMinutes = value; }
        }

        public bool VersionCheck
        {
            get { return _versionCheck; }
            set { _versionCheck = value; }
        }

        public bool ShowLogWindowOnStartup
        {
            get { return _showLogWindow; }
            set { _showLogWindow=value; }
        }

        public int MaxLogLinesDisplayed
        {
            get { return _maxLogLinesShowed; }
            set { _maxLogLinesShowed = value; }
        }

        public Rectangle MainFormInfo { get; set; }
        public Rectangle MessageLogFormInfo { get; set; }

        public int ReloadAfterXTimesNotRecognized
        {
            get { return _reloadAfterXTimesNotRecognized; }
            set { _reloadAfterXTimesNotRecognized = value; }
        }

        public int PauseCountdownForXSeconds
        {
            get { return _pauseCountdownForXSeconds; }
            set { _pauseCountdownForXSeconds=Math.Abs(value); }
        }

        public int AutoRefreshAfterXMinutes
        {
            get { return _autoRefreshAfterXMinutes; }
            set { _autoRefreshAfterXMinutes = value; }
        }

        public string FoEServer
        {
            get { return _foEServer; }
            set { _foEServer = value; }
        }

        public string BrowserUserAgent
        {
            get { return _browserUserAgent; }
            set { _browserUserAgent = value; }
        }

        public bool DoGoodsClicks
        {
            get { return _doGoodsClicks; }
            set { _doGoodsClicks = value; }
        }

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password=value; }
        }

        public string LastLoadedWorld
        {
            get { return _lastLoadedWorld; }
            set { _lastLoadedWorld = value; }
        }

        public string ApplicationLanguage
        {
            get { return _applicationLanguage; }
            set { _applicationLanguage = value; }
        }

        private void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return;
            _password = Encrypt(password);
        }
        private string GetPassword()
        {
            if (string.IsNullOrWhiteSpace(_password))
                return string.Empty;
            return Decrypt(_password);
        }

        private string Encrypt(string password)
        {
            var alg = new TripleDESStringEncryptor();
            return alg.EncryptString(password);
        }

        private string Decrypt(string password)
        {
            var alg = new TripleDESStringEncryptor();
            return alg.DecryptString(password);
        }

        public string CoordinateData
        {
            get { return _coordinateData; }
            set
            {
                _coordinateData = value;
                if (!string.IsNullOrWhiteSpace(_coordinateData))
                {
                    ItemCollection.LoadInstanceFromCoordinateData(_coordinateData);
                }
            }
        }

        public static ApplicationSettings LoadSettingsFromFile()
        {
            try
            {
                using (var reader = new FileStream(SettingsFileFilename,FileMode.Open))
                {
                    var sett=new XmlSerializerHelper<ApplicationSettings>();
                    return sett.Read(reader);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("No settings present. Please visit \"Settings\" page to configure bot.");
                return new ApplicationSettings();
            }
        }

        public void SaveSettings()
        {
            var sett = new XmlSerializerHelper<ApplicationSettings>();
            using (var writer = new StreamWriter(SettingsFileFilename))
            {
                writer.Write(sett.Save(this));
            }
            
        }

    }
}