using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Awesomium.Core;
using Awesomium.Windows.Forms;
using System.Reflection;
using Timer = System.Windows.Forms.Timer;

namespace ForgeBot
{

    public partial class MainForm :Form
    {
        private enum LogType
        {
            JustMessage,
            ScanFound,
            ScanProductionFound,
            ScanStart,
            ScanProductionStart,
            ClickProductionStart,
            ClickProductionSelect,
            ClickProductionDone,
            ScanStop,
            ScanProductionStop,
            ClickDone,
            ClickStart,
            ClickStop,
            BotIdle,
            PlungedScanFound,
            RottedSuppliesScanFound,
            ZoomingIn,
            CheckForZoom,
            BotInPause
        }

        enum BotStatus
        {
            None,
            Idle,
            Scan,
            ScanProd,
            ClickProd,
            Click,
            Pause
        }

        #region delegetes

        private delegate void BotIdleCallback();
        private delegate Screenshot GetNewBrowserImageCallback();
        private delegate void InjectClickAtCallback(int x, int y);
        private delegate void AddMessageCallback(string info);
        private delegate void AddMessageWExceptionCallback(string info,Exception exception);
        private delegate void UpdateStatusCallback(LogType status, string info);
        private delegate void ToggleFormResizeCallback(bool allowResize);
        internal delegate void InitializeViewDelegate(WebView webView);
        private delegate void RestartApplicationCallback();
        #endregion

        #region Fields
        private TimeSpan _countdownTimer;
        private TimeSpan _pauseTimer;

        private bool _needsResize;
        private bool _isViewUserFriendly = true;
        private bool _allowedInteraction = true;
        private BotStatus _botStatus = BotStatus.None;
        private bool _formIsResizing;
        private int _windowNotRecognizedCounter = 0;
        private bool _windowNotRecognized=false;
        private int _autoRefreshTicksCounter = 0;
        private bool _performInitialAutoLogin;
        private bool _performInitialStartBot;

        public readonly ApplicationSettings Settings;

        private Task _botThread;
        private readonly CancellationTokenSource _cancelationTokenBotThread = new CancellationTokenSource();

        private readonly MessagesForm _messagesForm;

        private WebView _aweWebView;

        private WebSession _aweWebSession;
        private string _lastUrl;
        private bool _documentLoadedcompletly;
        private bool _blueScreenDetected;
        private readonly Bitmap _internalScreenBitmap;

        private readonly CachedBitmapReader _cachedBitmap=new CachedBitmapReader();

        #endregion

        #region Properties
        internal static Rectangle DefaultFormPositionAndSize
        {
            get { return new Rectangle(16, 16, 766, 558); }
        }

        public bool PerformApplicationRestart { get; set; }

        public bool KeepAutoReloadButton { get; set; }

        private static string GetVersion()
        {
            if (ApplicationDeployment.IsNetworkDeployed) // clickonce
            {
                return ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }
#if DEBUG
            return " -DEBUG-";
#else
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            return string.Format("{0}.{1}.{2} Build {3}", version.Major, version.Minor,version.Build,version.Revision);
#endif
        }

        private static string BotVersion
        {
            get { return GetVersion(); }
        }

        private Control PictureContainer
        {
            get { return panelPictureContainer; }
        }

        internal Form MessageLogForm
        {
            get { return _messagesForm; }
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private int BrowserHeight
        {
            get { return (splitContainer1.Panel1.Height - BrowserTop); }
        }

        private int BrowserWidth
        {
            get { return (splitContainer1.Panel1.Width - BrowserLeft); }
        }

        private int BrowserTop
        {
            get { return 0; }
        }

        private int BrowserLeft
        {
            get { return 0; }
        }

        private bool WindowNotRecognized
        {
            get { return _windowNotRecognized; }
            set
            {
                _windowNotRecognized = value;
                if (_windowNotRecognized==false)
                    _windowNotRecognizedCounter = 0;
            }
        }

        #endregion

        #region MainForm constructor
        public MainForm(bool startupAutoLogin, bool autoStartBot, bool buttonAutoReloadWasPressed)
        {
            Settings = ApplicationSettings.LoadSettingsFromFile();
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.ApplicationLanguage);

            _internalScreenBitmap = new Bitmap(Settings.BotPictureWidth, Settings.BotPictureHeight,
                                               PixelFormat.Format32bppRgb);
            _messagesForm = new MessagesForm(Settings)
                                {
                                    ShowInTaskbar = false,
                                    FormBorderStyle = FormBorderStyle.SizableToolWindow,
                                    TopMost = true
                                };

            _performInitialAutoLogin = startupAutoLogin;
            _performInitialStartBot = autoStartBot;

            ReloadWebCore();

            InitializeComponent();

            // kada se ulazi u design mode, ovo se samo pomera. ovako cemo ga fixirati
            splitContainer1.SplitterDistance = 475;

            buttonAutoReload.Checked = buttonAutoReloadWasPressed;

            // interval that we will check if WebCore has updated content. When using BitmapSurface 
            // WebCore does not advise us automatically through Updated event
            // we drive @ 66 fps
            webCoreTimer.Interval = 15;
            webCoreTimer.Start();

            #region Fix key press controls

            foreach (var control in panelButtonControls.Controls)
            {
                var ctrl = control as Button;
                if (ctrl != null)
                {
                    //ctrl.KeyDown += OnKeyDownSuppress;
                    //ctrl.KeyUp += OnKeyUp;
                    //ctrl.KeyPress += OnKeyPress;
                    ctrl.Click += OnClickWithChangeFocus;
                    ctrl.GotFocus += OnUnwantedControlGotFocus;
                }
                var ctrl2 = control as CheckBox;
                if (ctrl2 != null)
                {
                    //ctrl.KeyDown += OnKeyDownSuppress;
                    //ctrl.KeyUp += OnKeyUp;
                    //ctrl.KeyPress += OnKeyPress;
                    ctrl2.Click += OnClickWithChangeFocus;
                    ctrl2.GotFocus += OnUnwantedControlGotFocus;
                }
            }

            // koristimo textBox1 da uhvatimo key. jer kad button dobije focus, ne mozemo da ga vratimo nazad na form.
            textBox1.Focus();
            textBox1.KeyPress += TextBox1KeyPress;
            textBox1.KeyDown += TextBox1KeyDown;
            textBox1.KeyUp += TextBox1KeyUp;
            PictureContainer.KeyPress += TextBox1KeyPress;
            PictureContainer.KeyDown += TextBox1KeyDown;
            PictureContainer.KeyUp += TextBox1KeyUp;
            PictureContainer.MouseWheel += OnMouseWheel;
            PictureContainer.MouseDown += OnMouseDown;
            PictureContainer.MouseUp += OnMouseUp;
            PictureContainer.MouseMove += OnMouseMove;
            PictureContainer.SizeChanged += PictureBoxContainerSizeChanged;
            PictureContainer.Paint += PictureContainerPaint;

            #endregion

            _messagesForm.VisibleChanged += MessagesFormVisibleChanged;

            // Initialize the view.
            InitializeView(WebCore.CreateWebView(BrowserWidth, BrowserHeight, _aweWebSession));

            var activeCoordsForm = new CoordsForm(Settings);
            activeCoordsForm.loadDataFromSettings();

            Logger.LogInfo(string.Format("ivanJo BOT start. AutoLogin={0},BotAutoStart={1}",startupAutoLogin,autoStartBot));
        }
        #endregion

        #region Awesomium initializers
        private void ReloadWebCore()
        {
            //if (WebCore.IsRunning)
            //    WebCore.Shutdown();

            if (_aweWebView != null && _aweWebView.Surface != null && !((BitmapSurface)_aweWebView.Surface).IsDisposed)
            {
                ((BitmapSurface)_aweWebView.Surface).Dispose();
            }

            if (_aweWebView != null && !_aweWebView.IsDisposed)
            {
                _aweWebView.Dispose();
                _aweWebView = null;
            }

            if (!WebCore.IsRunning)
            {
                // webcore moze da se iniciajlizuje samo jednom u okviru procesa. cak ni shutdown ne pomaze.

                // awesomium setup
                var cnfg = new WebConfig();
                cnfg.AdditionalOptions = new string[]
                                             {
                                                 "--enable-accelerated-filters"
                                                 , "--enable-accelerated-painting"
                                                 , "--enable-accelerated-plugins"
                                                 , "--enable-threaded-compositing"
                                                 , "--gpu-startup-dialog"
                                             };
                cnfg.UserAgent = Settings.BrowserUserAgent;
                //cnfg.AutoUpdatePeriod = 50;

                WebCore.Initialize(cnfg);

                WebPreferences webPreferences = WebPreferences.Default;
                webPreferences.ProxyConfig = "auto";
                _aweWebSession = WebCore.CreateWebSession(
                    String.Format("{0}{1}Cache", Path.GetDirectoryName(Application.ExecutablePath),
                                  Path.DirectorySeparatorChar),
                    webPreferences);
            }
        }

        private void InitializeView(WebView view)
        {
            if (view == null)
                return;

            _aweWebView = view;

            _aweWebView.CursorChanged += OnCursorChanged;
            _aweWebView.AddressChanged += OnAddressChanged;
            
            _aweWebView.ShowCreatedWebView += OnShowNewView;
            _aweWebView.Crashed += OnCrashed;
            _aweWebView.DocumentReady += OnDocumentReady;

            // Load a URL, if this is not a child view.
            if (_aweWebView.ParentView == null)
            {
                if(Settings.FoEServer=="Default")
                    _aweWebView.Source = new Uri("http://www.forgeofempires.com");
                else
                    _aweWebView.Source = new Uri("http://" + Settings.FoEServer);
            }

            if(WebCore.IsRunning)
                WebCore.ResourceInterceptor = new FoEResourceInterceptor();


            // Give focus to the view.
            _aweWebView.FocusView();

            _needsResize = true;
            ResizeView();
        }

        

        #endregion

        #region Awesomium events

        private void OnAddressChanged(object sender, UrlEventArgs e)
        {
            _lastUrl = _aweWebView.Source.AbsoluteUri;
            _documentLoadedcompletly = false;
            AddMessage(string.Format(Resource.MainForm_OnAddressChanged_PageLoaded, _aweWebView.Source.AbsoluteUri));
            if (_aweWebView.Source.AbsoluteUri.Contains("game/index?login=1&ref="))
            {
                buttonStartBot.Enabled = true;
                btnReload.Enabled = true;
                
                // save username/password
                var interceptor = WebCore.ResourceInterceptor as FoEResourceInterceptor;
                if(interceptor!=null && interceptor.LastLoadedUserData!=null)
                {
                    Settings.Username = interceptor.LastLoadedUserData.Username;
                    Settings.Password = interceptor.LastLoadedUserData.PasswordHash;
                    Settings.LastLoadedWorld = interceptor.LastLoadedUserData.WorldId;
                    Settings.SaveSettings();
                }

                if (_performInitialStartBot)
                {
                    _performInitialStartBot = false;
                    AutostartBot();
                }
            }
            else
            {
                buttonStartBot.Enabled = false;
                btnReload.Enabled = false;
            }

            //textBox1.Focus();
        }

        private void OnDocumentReady(object sender, UrlEventArgs e)
        {
            _documentLoadedcompletly = true;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
        internal static extern IntPtr GetFocus();

        private Control GetFocusedControl()
        {
            Control focusedControl = null;
            // To get hold of the focused control:
            IntPtr focusedHandle = GetFocus();
            if (focusedHandle != IntPtr.Zero)
                // Note that if the focused Control is not a .Net control, then this will return null.
                focusedControl = Control.FromHandle(focusedHandle);
            return focusedControl;
        }

        private void OnCursorChanged(object sender, CursorChangedEventArgs e)
        {
            // Update the cursor.
            Cursor = e.CursorType.GetCursor();
        }

        private void OnCrashed(object sender, CrashedEventArgs e)
        {
            AddMessage(string.Format(Resource.MainForm_AwesomiumCrashed + e.Status));
            RestartApplication();
        }

        private void OnShowNewView(object sender, ShowCreatedWebViewEventArgs e)
        {
            if ((_aweWebView == null) || !_aweWebView.IsLive)
                return;

            if (e.IsPopup)
            {
                // Create a WebView wrapping the view created by Awesomium.
                var view = new WebView(e.NewViewInstance);
                // ShowCreatedWebViewEventArgs.InitialPos indicates screen coordinates.
                var screenRect = e.InitialPos.ToRectangle();
                // Create a new WebForm to render the new view and size it.
                var childForm = new ChildForm(view, screenRect.Width, screenRect.Height)
                                    {
                                        ShowInTaskbar = false,
                                        FormBorderStyle = FormBorderStyle.SizableToolWindow,
                                        ClientSize = screenRect.Size
                                    };

                // Show the form.
                childForm.Show(this);
                // Move it to the specified coordinates.
                childForm.DesktopLocation = screenRect.Location;
            }
            else
            {
                // Let the new view be destroyed. It is important to set Cancel to true 
                // if you are not wrapping the new view, to avoid keeping it alive along
                // with a reference to its parent.
                e.Cancel = true;

                // Load the url to the existing view.
                _aweWebView.Source = e.TargetURL;
            }
        }

        #endregion

        #region Awesomium Click injectors
        private void InjectClickAt(int x, int y)
        {
            if (InvokeRequired)
            {
                InjectClickAtCallback method = InjectClickAt;
                Invoke(method, new object[] { x, y });
            }
            else
            {
                _aweWebView.InjectMouseMove(x, y);
                _aweWebView.InjectMouseDown(0);
                _aweWebView.InjectMouseUp(0);
            }
        }

        private void InjectClickAtInvokeForced(int x, int y)
        {
            InjectClickAtCallback method = InjectClickAt;
            Invoke(method, new object[] { x, y });
        }
        #endregion

        #region BOT Start/Stop/Runner functions
        private void BotIdle()
        {
            _allowedInteraction = true;
            ResizeBrowser(true);
            buttonChangeView.Enabled = true;
            StartTimer();
            UpdateStatus(LogType.BotIdle, null);
            buttonPauseResume.Enabled = true;

            FixFreezingIssue();
        }

        /// <summary>
        /// Sometimes, after we exit from scanning, screen freezes. 
        /// Int that case Awesomium.WebCore doesn't send events until we resize a window
        /// </summary>
        private void FixFreezingIssue()
        {
            _aweWebView.Resize(BrowserWidth, BrowserHeight + 1);
            _aweWebView.Resize(BrowserWidth, BrowserHeight);
        }

        private void AutostartBot()
        {
            timerAutoStartBot.Tick += AutoStartBot_Tick;
            timerAutoStartBot.Start();
        }

        private void AutoStartBot_Tick(object sender, EventArgs eventArgs)
        {
            timerAutoStartBot.Tick -= AutoStartBot_Tick;
            buttonStartBot.PerformClick();
            timerAutoStartBot.Dispose();
        }

        private void BotIdleInvokeForced()
        {
            BotIdleCallback method = BotIdle;
            Invoke(method, new object[0]);
        }

        private void BotStart()
        {
            _allowedInteraction = false;

            buttonChangeView.Enabled = false;
            buttonStartBot.Enabled = false;
            buttonAutoReload.Enabled = true;
            if (_isViewUserFriendly)
            {
                ResizeBrowser(false);
            }
            //buttonPauseResume.Enabled = false;
            //buttonPauseResume.Text = Resource.MainForm_botStart_Pause_Timer;
            _botThread = new Task(BotDoActionsThreaded, _cancelationTokenBotThread.Token);
            _botThread.ContinueWith(ThreadedTaskExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            _botThread.Start();

            timerPauseCountdown.Stop();
        }
        private void BotDoActionsThreaded()
        {
            _cancelationTokenBotThread.Token.ThrowIfCancellationRequested();

            //ToggleFormResize(false);

            Sleeping(200);
            BlockUntilPopupIs(false, Settings.WaitOnPopupInMillis);

            AssureZoomedIn();

            var crdClickList = ScanPicture4Collect();

            DoCollectClicks(crdClickList);
            if (!Settings.DoSupplyClicks)
            {
                BotIdleInvokeForced();
            }
            else
            {
                if (crdClickList.Count > 0)
                {
                    UpdateStatus(LogType.JustMessage, string.Format(Resource.MainForm_BotDoActionsThreaded_BotIdleBeforeProduction,Settings.WaitBeforeProductionInMillis / 1000));
                    Sleeping(Settings.WaitBeforeProductionInMillis);
                }
                var crdSupplyList = ScanPicture4Production();
                DoSupplyClicks(crdSupplyList);
                BotIdleInvokeForced();
            }

            // saves new click coordinates data
            Settings.CoordinateData = ItemCollection.BuildCoordinateData();

            //ToggleFormResize(true);

            // check if we reached game window not recognized 
            var limitWindowNotRecognized = Settings.ReloadAfterXTimesNotRecognized == 0
                                               ? int.MaxValue
                                               : Settings.ReloadAfterXTimesNotRecognized;

            // if the window was not recognized during the last scan, increment counter
            if (WindowNotRecognized)
                _windowNotRecognizedCounter++;

            if (_blueScreenDetected || (_windowNotRecognizedCounter >= limitWindowNotRecognized))
            {
                UpdateStatus(LogType.JustMessage, Resource.MainForm_BotDoActionsThreaded_RestartApplication);
                
                RestartApplicationInvokeForced();
            }

        }

        

        private void StartTimer()
        {
            int seconds = Randomizer(Settings.CheckEveryXMinutes * 60, Settings.RndPercentCheckEveryXMinutes, true);
            _countdownTimer = new TimeSpan(0, 0, 0, seconds);
            timerBotCountdown.Interval = 1000;
            timerBotCountdown.Start();
            //buttonPauseResume.Enabled = true;
            timerPauseCountdown.Stop();
        }

        private void TimerBotCountdownTick(object sender, EventArgs e)
        {
            TimeTick();
        }

        private void TimeTick()
        {
            _countdownTimer = _countdownTimer.Subtract(new TimeSpan(0, 0, 1));
            labelTimer.Text = string.Format("Timer: {0:00}:{1:00}:{2:00}", _countdownTimer.Hours, _countdownTimer.Minutes, _countdownTimer.Seconds);
            if (_countdownTimer.TotalSeconds <= 0)
            {
                timerBotCountdown.Stop();
                BotStart();
            }
        }

        #endregion

        #region Form events

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (_aweWebView == null)
                return;

            // Before using a windowed WebView, we need
            // to assign a parent window.    
            _aweWebView.ParentWindow = Handle;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            UpdateWindowsPosition();
            
            Settings.SaveSettings();

            if ((_botThread != null) && _botThread.Status == TaskStatus.Running)
            {
                AddMessage(Resource.MainForm_OnFormClosing_Stopping_running_BOT_thread);
                _cancelationTokenBotThread.Cancel(true);
                AddMessage(Resource.MainForm_OnFormClosing_BOT_thread_stopped);
            }

            // Get if this is form hosting a child view.
            bool isChild = _aweWebView.ParentView != null;

            _cachedBitmap.Dispose();

            timerAutoRefresh.Dispose();
            timerBotCountdown.Dispose();
            timerPauseCountdown.Dispose();

            // Destroy the WebView.
            if (_aweWebView != null)
                _aweWebView.Dispose();

            // The surface that is currently assigned to the view,
            // does not need to be disposed. It will be disposed 
            // internally.

            webControl1.Dispose();


            // Shut down the WebCore last.
            if (!isChild)
                WebCore.Shutdown();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

#if DEBUG
            button1.Visible = true;
#else
            button1.Visible = false;
#endif
            UpdateFormSize();
            if (Settings.ShowLogWindowOnStartup)
                _messagesForm.Show();

            panelButtonControls.BackgroundImage = Resource.panelBckgr;

            textBox1.Left = -100;

            Text = string.Format("ivanJo FoE Bot v{0}", BotVersion);
            splitContainer1.SplitterWidth = 1;
            splitContainer1.IsSplitterFixed = true;

            //buttonPauseResume.Enabled = false;

            AddMessage(Resource.MainForm_OnLoad_Bot_loaded_succesfully);
            ResizeBrowser(true);

            ApplyAutoRefreshSetting();

            textBox1.Focus();

            if(_performInitialAutoLogin)
            {
                _performInitialAutoLogin = false;
                RefreshWebPage(true);
            }
        }

        private void UpdateFormSize()
        {
            if (!Settings.MainFormInfo.IsEmpty)
            {
                Top = Settings.MainFormInfo.Top;
                Left = Settings.MainFormInfo.Left;
                Width = Settings.MainFormInfo.Width;
                Height = Settings.MainFormInfo.Height;
            }
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            _formIsResizing = false;

            if ((_aweWebView == null) || !_aweWebView.IsLive)
                return;

            if (BrowserWidth > 0 && BrowserHeight > 0)
                _needsResize = true;

            // Request resize, if needed.
            if (_needsResize)
            {
                ResizeView();
                // window position/size changed, so we save them
            }
        }

        protected override void OnResizeBegin(EventArgs e)
        {
            _formIsResizing = true;
        }


        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            Opacity = 1.0D;

            if ((_aweWebView == null) || !_aweWebView.IsLive)
                return;

            _aweWebView.FocusView();
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);

            if ((_aweWebView == null) || !_aweWebView.IsLive)
                return;

            // Let popup windows be semi-transparent,
            // when they are not active.
            if (_aweWebView.ParentView != null)
                Opacity = 0.8D;

            _aweWebView.UnfocusView();
        }

        #endregion

        #region Button click events

        private void buttonChangeView_CheckedChanged(object sender, EventArgs e)
        {
            if (_allowedInteraction)
            {
                if (_botStatus == BotStatus.Idle)
                {
                    ResizeBrowser(!_isViewUserFriendly);
                }
                else
                {
                    AddMessage(
                        Resource.MainForm_buttonChangeView_Click_You_can_only_change_the_view_mode_when_the_bot_is_idle_);
                }
            }
        }


        private void ButtonPauseResumeClick(object sender, EventArgs e)
        {
            if (_botStatus!=BotStatus.Pause)
            {
                timerPauseCountdown.Stop();
                timerBotCountdown.Stop();
                buttonPauseResume.Text = Resource.MainForm_buttonStartBot_Resume_Timer;
                buttonStartBot.Enabled = true;
                buttonStartBot.Text = Resource.MainForm_buttonStartBot_Restart_Bot;
                _botStatus = BotStatus.Pause;
            }
            else
            {
                timerPauseCountdown.Stop();
                timerBotCountdown.Start();
                buttonStartBot.Enabled = false;
                buttonStartBot.Text = Resource.MainForm_buttonStartBot_Running;
                buttonPauseResume.Text = Resource.MainForm_botStart_Pause_Timer;
                _botStatus = BotStatus.Idle;
                UpdateStatus(LogType.BotIdle,null);
            }
        }

        private void ButtonSavePictureClick(object sender, EventArgs e)
        {
            if (_aweWebView.Surface != null)
            {
                ((BitmapSurface)_aweWebView.Surface).SaveToPNG("userPicture.png");
                AddMessage(Resource.MainForm_buttonSavePicture_Click_Picture_saved);
            }
        }

        private void ButtonSettingsClick(object sender, EventArgs e)
        {
            var activeSettingsForm = new SettingsForm(this, Settings);
            UpdateWindowsPosition();
            activeSettingsForm.ShowDialog();

            ApplyAutoRefreshSetting();
        }

        private void buttonStartBot_Click(object sender, EventArgs e)
        {
            BotStart();
        }

        private void buttonCoords_Click(object sender, EventArgs e)
        {
            var activeCoordsForm = new CoordsForm(Settings);
            activeCoordsForm.loadData();
            activeCoordsForm.ShowDialog();
        }

        private void buttonAutoReload_CheckedChanged(object sender, EventArgs e)
        {
            var cb = (sender as CheckBox);
            if (cb.Checked)
            {
                timerAutoRefresh.Start();
                cb.Text = Resource.MainForm_buttonAutoReload_Auto_reload_ON;
            }
            else
            {
                timerAutoRefresh.Stop();
                cb.Text = Resource.MainForm_buttonAutoReload_Auto_reload_OFF;
            }
        }

        #endregion

        #region Keyboard/mouse Events, picture box events

        private void OnKeyDownSuppress(object sender, KeyEventArgs e)
        {
            //OnKeyDown(textBox1,e);
            //textBox1.Focus();
            e.SuppressKeyPress = true;
        }

        private void TextBox1KeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;

            if (_allowedInteraction)
            {
                if ((_aweWebView == null) || !_aweWebView.IsLive)
                    return;

                _aweWebView.InjectKeyboardEvent(e.GetKeyboardEvent(WebKeyboardEventType.KeyUp));
            }
        }

        private void TextBox1KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;

            if (_allowedInteraction)
            {
                if ((_aweWebView == null) || !_aweWebView.IsLive)
                    return;

                _aweWebView.InjectKeyboardEvent(e.GetKeyboardEvent(WebKeyboardEventType.KeyDown));
            }
        }

        private void TextBox1KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            RestartPauseTimer();
            if (_allowedInteraction)
            {
                if ((_aweWebView == null) || !_aweWebView.IsLive)
                    return;

                _aweWebView.InjectKeyboardEvent(e.GetKeyboardEvent());
            }

        }

        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Right || keyData == Keys.Left || keyData == Keys.Up || keyData == Keys.Down)
            {
                return true;
            }
            return base.IsInputKey(keyData);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_allowedInteraction)
            {
                if ((_aweWebView == null) || !_aweWebView.IsLive)
                    return;

                _aweWebView.InjectMouseMove(e.X, e.Y);
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (_allowedInteraction && (e.Button == MouseButtons.Left))
            {
                if ((_aweWebView == null) || !_aweWebView.IsLive)
                    return;

                _aweWebView.InjectMouseUp(e.Button.GetMouseButton());
            }
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (_allowedInteraction && (e.Button == MouseButtons.Left))
            {
                RestartPauseTimer();
                if ((_aweWebView == null) || !_aweWebView.IsLive)
                    return;

                _aweWebView.InjectMouseDown(e.Button.GetMouseButton());
            }
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            if (_allowedInteraction)
            {
                if ((_aweWebView == null) || !_aweWebView.IsLive)
                    return;

                _aweWebView.InjectMouseWheel(e.Delta, 0);
            }
        }

        private void OnClickWithChangeFocus(object sender, EventArgs e)
        {
            base.OnClick(e);
            textBox1.Focus();
        }

        /// <summary>
        /// When using BitmapSurface, WebCore does not raises any events nor is doing auto-update every X milliseconds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webCoreTimer_Tick(object sender, EventArgs e)
        {
            if (WebCore.IsRunning)
            {
                WebCore.Update();
                if (_aweWebView != null && _aweWebView.Surface != null)
                {
                    var surface = (BitmapSurface)_aweWebView.Surface;
                    if (surface.IsDirty)
                    {
                        var rect = new Rectangle(0, 0, surface.Width,
                                                 surface.Height);
                        
                        PictureContainer.Invalidate(rect);
                    }
                }
            }
        }

        private void PictureContainerPaint(object sender, PaintEventArgs e)
        {
            if (_aweWebView != null && _aweWebView.IsLive && _aweWebView.Surface != null )
            {
                var surface = (BitmapSurface)_aweWebView.Surface;

#if DEBUG
                var sw = new Stopwatch();
                sw.Start();

                const int runs = 1;
                for (int i = 0; i < runs; i++)
                {

#endif

                    IntPtr destinationBuffer;
                    //if (_cachedBitmap.IsDisposed)
                    //  throw new RestartApplicationNeededException();

                    _cachedBitmap.GetCachedBitmapBuffer(surface.Width, surface.Height, out destinationBuffer);

                    if (_isViewUserFriendly)
                        Win32.memcpy(destinationBuffer, surface.Buffer,
                                new UIntPtr((uint)(surface.Width * 4 * surface.Height)));
                    else
                        FastBitmapSurface.CopyMemoryRectangular(surface.Buffer, destinationBuffer,
                                                                surface.Width*4,
                                                                surface.Width*4,
                                                                new AweRect(e.ClipRectangle.X, e.ClipRectangle.Y,
                                                                            e.ClipRectangle.Width,
                                                                            e.ClipRectangle.Height),
                                                                new AweRect(e.ClipRectangle.X, e.ClipRectangle.Y,
                                                                            e.ClipRectangle.Width,
                                                                            e.ClipRectangle.Height), 4);

                    surface.IsDirty = false;
                    GraphicsHelper.GdiDrawImage(e.Graphics, _cachedBitmap.GetSourceImageHBitmap, e.ClipRectangle,
                                                e.ClipRectangle.X, e.ClipRectangle.Y);

#if DEBUG
                }

                sw.Stop();
                var avg = CalculateAverage(sw.ElapsedTicks/runs)/10000;
                double ppms = (e.ClipRectangle.Height*e.ClipRectangle.Width)/avg;
                //AddMessage(string.Format("exec: {0:0.00000} msec,W={2},H={1},Size={4}KB,Mpix/s={3:0.00}",
                //                         avg, e.ClipRectangle.Height, e.ClipRectangle.Width, ppms,
                //                         (e.ClipRectangle.Height * e.ClipRectangle.Width * 4) / 1024));

#endif
            }
        }

#if DEBUG
        private const int Max4Average = 50;
        readonly long[] avgX=new long[Max4Average];
        

        private double CalculateAverage(long elapsedTicks)
        {
            for (int i = Max4Average - 1; i > 0; i--)
            {
                avgX[i] = avgX[i - 1];
            }
            avgX[0] = elapsedTicks;

            return ((double)avgX.Sum()) / Max4Average;

        }
#endif

        /// <summary>
        /// maximize button
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSizeChanged(EventArgs e)
        {
            _needsResize = true;
            ResizeView();
            ResizeMainFormIfNecessary();
            base.OnSizeChanged(e);
        }

        private void PictureBoxContainerSizeChanged(object sender, EventArgs e)
        {
            _needsResize = true;
            ResizeView();
        }

        private void BtnReloadClick(object sender, EventArgs e)
        {
            RefreshWebPage(true);
        }

        private void PictureBox2Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=VK294XPC2FDVN");
            }
            catch
            {
            }
        }

        private void BtnShowLogClick(object sender, EventArgs e)
        {
            bool show = !_messagesForm.Visible;

            _messagesForm.VisibleChanged -= MessagesFormVisibleChanged;
            // Show the form.
            if (show)
            {
                _messagesForm.Show();
            }
            else
            {
                _messagesForm.Hide();
            }
            _messagesForm.VisibleChanged += MessagesFormVisibleChanged;
        }

        private void MessagesFormVisibleChanged(object sender, EventArgs e)
        {
            chk_ShowLog.Checked = _messagesForm.Visible;
        }

        private void OnUnwantedControlGotFocus(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void TimerPauseCountdownTick(object sender, EventArgs e)
        {
            _pauseTimer = _pauseTimer.Subtract(new TimeSpan(0, 0, 1));
            labelTimer.Text = string.Format("Pause: {0:00}:{1:00}:{2:00}", _pauseTimer.Hours, _pauseTimer.Minutes, _pauseTimer.Seconds);
            if (_pauseTimer.TotalSeconds <= 0)
            {
                timerPauseCountdown.Stop();
                timerBotCountdown.Start();
                UpdateStatus(LogType.BotIdle,null);
            }
        }

        #endregion

        #region Game: Clicker functions
        private void DoCollectClicks(List<Coordinate> crdList)
        {
            ItemCollection.Item l_Item = new ItemCollection.Item();

            UpdateStatus(LogType.ClickStart, null);
            
            // maximum number of items to click
            var maxToClick = crdList.Count > Settings.MaxItemsClickedInARow
                                 ? Settings.MaxItemsClickedInARow
                                 : crdList.Count;

            int maxClickedItems = Randomizer(maxToClick, Settings.RndPercentDontCollectInARow, false);
            if (maxClickedItems <= 0)
                maxClickedItems = 1;
            AddMessage(string.Format(Resource.MainForm_DoCollectClicks_Stealth_ClickingFirstXItems, maxClickedItems));

            // hvatamo boje za proveru da li su kliknute kutije
            var image = GetNewBrowserImageInvokeForced();
            var colors=new List<int>();

            int num = 0;
            foreach (Coordinate coordinate in crdList)
            {
                colors.Add(image.GetPixelRgb(coordinate.X, coordinate.Y));
                string l_CoordinateString = ItemCollection.MakePoint(coordinate.X,coordinate.Y);
                if (ItemCollection.Instance.Items.TryGetValue(l_CoordinateString, out l_Item))
                {
                    l_Item.CalculateNewTrialCoordinates();
                    InjectClickAtInvokeForced(coordinate.X + l_Item.m_OffsetX, coordinate.Y + l_Item.m_OffsetY);

                    num++;
                    UpdateStatus(LogType.ClickDone, num.ToString());
                    if (num >= maxClickedItems)
                        break;

                    Sleeping(200);
                }
            }
            image.Dispose();
            
            Sleeping(500);
            
            // nova slika za proveru
            image = GetNewBrowserImageInvokeForced();
            num = 0;
            foreach (Coordinate coordinate in crdList)
            {
                string l_CoordinateString = ItemCollection.MakePoint(coordinate.X, coordinate.Y);
                if (ItemCollection.Instance.Items.TryGetValue(l_CoordinateString, out l_Item))
                {
                    var colorNew = image.GetPixelRgb(coordinate.X, coordinate.Y);
                    var colorOld = colors[num++];
                    // ako su boje iste onda click nije uspeo
                    l_Item.ClickSuccessful(colorOld != colorNew);

                    ItemCollection.Instance.Items[l_CoordinateString] = l_Item;
                }
            }
            image.Dispose();

            UpdateStatus(LogType.ClickStop, null);
            
        }

        private void DoSupplyClicks(IEnumerable<Coordinate> crdList)
        {
            var l_Item = new ItemCollection.Item();
            UpdateStatus(LogType.ClickProductionStart, null);
            int num = 0;

            var lastGoodY = 0;
            var lastGoodX = 0;

            foreach (var coordinate in crdList)
            {
                var l_CoordinateString = ItemCollection.MakePoint(coordinate.X,coordinate.Y);
                if (ItemCollection.Instance.Items.TryGetValue(l_CoordinateString, out l_Item))
                {
                    l_Item.CalculateNewTrialCoordinates();
                    // we try to impose better trial coordinates for the item only if it has to tryout for the first time
                    if(lastGoodY!=0 && l_Item.ReadyForTheFirstAdjustment)
                    {
                        l_Item.m_OffsetX = lastGoodX;
                        l_Item.m_OffsetY = lastGoodY;
                    }
                    InjectClickAtInvokeForced(coordinate.X + l_Item.m_OffsetX, coordinate.Y + l_Item.m_OffsetY);
                    num++;
                    UpdateStatus(LogType.ClickProductionSelect, num.ToString());
                    BlockUntilPopupIs(true, Settings.WaitOnPopupInMillis);
                    var image = GetNewBrowserImageInvokeForced();
                    try
                    {
                        l_Item.ClickSuccessful(false);
                        if (IsPopupPresent(image))
                        {
                            Coordinate cordClose;
                            if (IsSupplyWinPresent(image, out cordClose))
                            {
                                l_Item.ClickSuccessful(true);
                                InjectClickAtInvokeForced(Settings.SupplyItemCoords.X, Settings.SupplyItemCoords.Y);
                                UpdateStatus(LogType.ClickProductionDone, num.ToString());
                                BlockUntilPopupIs(false, Settings.WaitOnPopupInMillis);
                                image.Dispose();
                                image = GetNewBrowserImageInvokeForced();
                                if (!IsPopupPresent(image))
                                {
                                    continue;
                                }
                                UpdateStatus(LogType.JustMessage,
                                             Resource.MainForm_DoSupplyClicks_Error_PopupRemained_SelectingSupplies);
                                image.Image.Save("suppliesWindow.png", ImageFormat.Png);
                                // try to close popup
                                if (TryClosePopup())
                                    continue;
                                break;
                            }
                            if (IsGoodsWinPresent(image, out cordClose))
                            {
                                l_Item.ClickSuccessful(true);
                                //saveScanResultToFile(image, crdList, 0, collectDistanceY, "goodswin-ok.png");

                                bool isMilitary = false;
                                //military units usually goes under this
                                if (IsMilitaryWinPresent(image, out cordClose))
                                {
                                    isMilitary = true;
                                    if (Settings.DoMilitaryUnitsClicks)
                                    {
                                        ClickMilitaryItems(image);
                                    }
                                    else
                                    {
                                        UpdateStatus(LogType.JustMessage,
                                                     Resource.MainForm_DoSupplyClicks_MilitaryClick_Disabled);
                                        TryClosePopup();
                                    }
                                }
                                else
                                {
                                    if (Settings.DoGoodsClicks)
                                    {
                                        InjectClickAtInvokeForced(Settings.GoodsItemCoords.X, Settings.GoodsItemCoords.Y);
                                        UpdateStatus(LogType.ClickProductionDone, num.ToString());
                                    }
                                    else
                                    {
                                        UpdateStatus(LogType.JustMessage,
                                                     Resource.MainForm_DoSupplyClicks_GoodsClick_Disabled);
                                        TryClosePopup();
                                    }
                                }
                                BlockUntilPopupIs(false, Settings.WaitOnPopupInMillis);
                                image.Dispose();
                                image = GetNewBrowserImageInvokeForced();
                                if (!IsPopupPresent(image))
                                {
                                    continue;
                                }
                                if (!isMilitary)
                                    UpdateStatus(LogType.JustMessage,
                                                 Resource.MainForm_DoSupplyClicks_Error_PopupRemained_SelectingGoods);
                                else
                                    UpdateStatus(LogType.JustMessage,
                                                 Resource.MainForm_DoSupplyClicks_Error_PopupRemained_SelectingMilitary);

                                image.Image.Save("goodsWindow.png", ImageFormat.Png);
                                // try to close popup
                                if (TryClosePopup())
                                    continue;
                                break;
                            }
                            UpdateStatus(LogType.JustMessage,
                                         string.Format(Resource.MainForm_DoSupplyClicks_NonProductionBuilding, num));

                            if (!TryClosePopup())
                            {
                                return;
                            }
                        }
                    }
                    finally
                    {
                        // Save last good click for next one
                        if (l_Item.LastClickWasSuccessful)
                        {
                            lastGoodX=l_Item.m_OffsetX;
                            lastGoodY=l_Item.m_OffsetY;
                        }
                        ItemCollection.Instance.Items[l_CoordinateString] = l_Item;
                        image.Dispose();
                    }
                }
            }
            UpdateStatus(LogType.ClickStop, null);
        }

        private void ClickMilitaryItems(Screenshot image)
        {
            var closeButtonCoords = FindCloseButton(image, 2410, 835, 40, 40);
            var startingMilitaryItemButton = new Coordinate(closeButtonCoords.X - 864, 1142);
            const int distButtons = 178;
            var borderColor = Color.FromArgb(0, 0, 0).ToArgb();
            var activeBorderColor = Color.FromArgb(0, 0, 2).ToArgb();
            var readyColor = Color.FromArgb(177, 106, 49).ToArgb();
            var activeReadyColor = Color.FromArgb(219, 141, 63).ToArgb();
            var unlockColor = Color.FromArgb(117, 116, 115).ToArgb();
            var unlockWithDiamondColor = Color.FromArgb(112, 111, 110).ToArgb();
            for (int i = 0; i < 5; i++)
            {
                //updateStatus(LogType.JustMessage, string.Format("x={0},y={1}", startingMilitaryItemButton.X, startingMilitaryItemButton.Y));
                var pixel = image.GetPixelRgb(startingMilitaryItemButton.X, startingMilitaryItemButton.Y);

                if (pixel == borderColor || pixel == activeBorderColor)
                {
                    //updateStatus(LogType.JustMessage, string.Format("black color found.", i + 1));
                    var unlocked = false;
                    // unlock not working since there's no "moon sign" above the military barrack
                    if (false && Settings.DoMilitaryUnitsUnblock && pixel == unlockColor)
                    {
                        InjectClickAtInvokeForced(startingMilitaryItemButton.X + 50, startingMilitaryItemButton.Y);
                        unlocked = true;
                        UpdateStatus(LogType.JustMessage, string.Format("Unlocking military item #{0}", i + 1));
                        Sleeping(1000);
                    }

                    var pixelPlusOne = image.GetPixelRgb(startingMilitaryItemButton.X + 1, startingMilitaryItemButton.Y);
                    if (unlocked || pixelPlusOne == readyColor || pixelPlusOne == activeReadyColor)
                    {
                        InjectClickAtInvokeForced(startingMilitaryItemButton.X + 50, startingMilitaryItemButton.Y);
                        UpdateStatus(LogType.JustMessage, string.Format(Resource.MainForm_ClickMilitaryItems_MilitaryItemSelected, i + 1));
                        break;
                    }
                }

                startingMilitaryItemButton.X += distButtons;
            }
        }


        #endregion

        #region Game: Window/Close button search functions
        private bool TryClosePopup()
        {
            using (var image = GetNewBrowserImageInvokeForced())
            {
                bool recognized = true;
                try
                {
                    Coordinate coordOpenMenuClose;
                    if (IsPopupPresent(image))
                    {
                        if (IsEventWinPresent(image, out coordOpenMenuClose))
                        {
                        }
                        else if (IsGoodsWinPresent(image, out coordOpenMenuClose))
                        {
                        }
                        else if (IsUnitWinPresent(image, out coordOpenMenuClose))
                        {
                        }
                        else if (IsEventSmallPresent(image, out coordOpenMenuClose))
                        {
                        }
                        else if (IsSupplyWinPresent(image, out coordOpenMenuClose))
                        {
                        }
                        else if (IsOngoingProductionWinPresent(image, out coordOpenMenuClose))
                        {
                        }

                        if (coordOpenMenuClose == null)
                            TryFindCloseButton(image, out coordOpenMenuClose);

                        if (coordOpenMenuClose != null)
                        {
                            InjectClickAtInvokeForced(coordOpenMenuClose.X + 9, coordOpenMenuClose.Y + 8);
                            if (BlockUntilPopupIs(false, Settings.WaitOnPopupInMillis))
                            {
                                return true;
                            }
                            image.Image.Save("closePopupError.png", ImageFormat.Png);
                            SaveErrorToFile("closePopupEorror.txt",
                                            string.Concat(new object[]
                                                              {
                                                                  Resource.
                                                                      MainForm_closePopup_Error_Popup_still_there_after_close_click
                                                                  ,
                                                                  Environment.NewLine, "X=", coordOpenMenuClose.X,
                                                                  "+9  Y=",
                                                                  coordOpenMenuClose.Y, "+8"
                                                              }));
                            UpdateStatus(LogType.JustMessage,
                                         Resource.MainForm_closePopup_Error_Popup_still_there_after_close_click);
                        }
                        else
                        {
                            image.Image.Save("closePopupError.png", ImageFormat.Png);
                            UpdateStatus(LogType.JustMessage,
                                         Resource.MainForm_closePopup_Error_Game_window_not_recognized);
                            recognized = false;
                            _blueScreenDetected = CheckForBlueScreen(image);
                            if (_blueScreenDetected)
                            {
                                UpdateStatus(LogType.JustMessage,
                                             Resource.MainForm_closePopup_Error_Blue_Screen_Of_Death);
                            }
                            return false;
                        }
                    }
                }
                finally
                {
                    // game window was recognized?
                    WindowNotRecognized = !recognized;
                }
            }
            return false;
        }

        private static bool IsCloseBtnPresent(Screenshot image, int x, int y)
        {
            var color = Color.FromArgb(105, 119, 142).ToArgb();
            if (image.GetPixelRgb(x, y) == color)
            {
                color = Color.FromArgb(90, 106, 131).ToArgb();
                if (image.GetPixelRgb(x + 17, y) == color)
                {
                    color = Color.FromArgb(178, 188, 204).ToArgb();
                    if (image.GetPixelRgb(x + 9, y + 8) == color)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool IsEventSmallPresent(Screenshot image, out Coordinate closeButton)
        {
            closeButton = null;
            if (IsCloseBtnPresent(image, 2205, 889))
            {
                closeButton = new Coordinate(2205, 889);
                return true;
            }
            return false;
        }

        private bool IsEventWinPresent(Screenshot image, out Coordinate closeButton)
        {
            Coordinate btnClose = null;

            //Parallel.For(500, 1400 + 1, (i, s) =>
            for(int i=500;i<=Settings.BotPictureHeight-500;i++)
                                            {
                                                var coords = FindCloseButton(image, 2466, i, 10, 0);
                                                if (coords != null)
                                                {
                                                    btnClose = coords;
                                                    break;
                                                }
                                            };

            closeButton = btnClose;
            return false;
        }

        private static bool IsGoodsWinPresent(Screenshot image, out Coordinate closeButton)
        {
            closeButton = null;
            var coords = FindCloseButton(image, 2424, 828, 60, 60);
            if (coords != null)
            {
                closeButton = coords;
                return true;
            }
            return false;
        }

        private static bool IsMilitaryWinPresent(Screenshot image, out Coordinate closeButton)
        {
            closeButton = null;
            var coords = FindCloseButton(image, 2412, 835, 40, 0);
            //var coords = FindCloseButton(Image, 2412, 835, 0,0);
            //if (coords == null)
            //    coords = FindCloseButton(Image, 2427, 835, 0,0);
            //if (coords == null)
            //    coords = FindCloseButton(Image, 2411, 835, 0,0);

            if (coords != null)
            {
                closeButton = coords;
                return true;
            }
            return false;
        }

        private static bool IsOngoingProductionWinPresent(Screenshot image, out Coordinate closeButton)
        {
            closeButton = null;
            var coords = FindCloseButton(image, 2204, 885, 60, 60);
            if (coords != null)
            {
                closeButton = coords;
                return true;
            }
            return false;
        }

        private static bool IsPopupPresent(Screenshot image)
        {
            var color = Color.FromArgb(171, 103, 45).ToArgb();
            if (image.GetPixelRgb(1, 1) == color)
            {
                return false;
            }
            return true;
        }

        private static bool IsSupplyWinPresent(Screenshot image, out Coordinate closeButton)
        {
            closeButton = null;
            var coords = FindCloseButton(image, 2312, 808, 60, 60);
            if (coords != null)
            {
                closeButton = coords;
                return true;
            }
            return false;
        }

        private static bool IsUnitWinPresent(Screenshot image, out Coordinate closeButton)
        {
            closeButton = null;
            var coords = FindCloseButton(image, 2427, 835, 60, 60);
            if (coords != null)
            {
                closeButton = coords;
                return true;
            }
            return false;
        }

        private bool CheckForBlueScreen(Screenshot image)
        {
            var blueScreenColor = image.GetPixelRgb(25, 25);
            var surface = ((BitmapSurface)_aweWebView.Surface);
            int min = Math.Min(surface.Height, surface.Width);
            for (int i = 0; i < min;i++ )
                if(image.GetPixelRgb(i, i)!=blueScreenColor)
                    return false;

            return true;
        }

        private static Coordinate FindCloseButton(Screenshot image, int x, int y, int xPerimeter, int yPerimeter)
        {
            Coordinate coords = null;
            //Parallel.For(-xPerimeter / 2, xPerimeter / 2 + 1, (i, s) =>
            for (int i = -xPerimeter / 2; i <= xPerimeter / 2; i++)
            {
                for (int j = -yPerimeter / 2; j <= yPerimeter / 2; j++)
                {
                    if (IsCloseBtnPresent(image, x + i, y + j))
                    {
                        //updateStatus(LogType.JustMessage, string.Format("Close button @ {0},{1}",x+i,y+j));
                        coords = new Coordinate(x + i, y + j);
                        break;
                    }
                }
            };
            return coords;
        }

        private void TryFindCloseButton(Screenshot image, out Coordinate closeButton)
        {
            closeButton = null;
            var coords = FindCloseButton(image, Settings.BotPictureWidth / 2, Settings.BotPictureHeight / 2,
                                         Settings.BotPictureWidth / 2, Settings.BotPictureHeight / 2);
            if (coords != null)
                closeButton = coords;
        }

        private bool BlockUntilPopupIs(bool present, int maxTimeInMilliseconds)
        {
            var color = Color.FromArgb(171, 103, 45).ToArgb();
            if (present)
                color = Color.FromArgb(102, 61, 27).ToArgb();

            while (maxTimeInMilliseconds > 0)
            {
                using (var image = GetNewBrowserImageInvokeForced())
                {
                    if (image.GetPixelRgb(1, 1) == color)
                    {
                        return true;
                    }
                }
                Sleeping(250);
                maxTimeInMilliseconds -= 250;
            }
            return false;
        }


        #endregion

        #region Game: Scanning collect/production
        /// <summary>
        /// we need to make sure that we're zoomed in
        /// </summary>
        private void AssureZoomedIn()
        {
            UpdateStatus(LogType.CheckForZoom, null);
            Sleeping(500);
            InjectClickAtInvokeForced(150, 50); 
            var image = GetNewBrowserImageInvokeForced();
            try
            {
                if (IsPopupPresent(image))
                {
                    TryClosePopup();
                    BlockUntilPopupIs(false, Settings.WaitOnPopupInMillis);
                    image.Dispose();
                    image = GetNewBrowserImageInvokeForced();
                }

                var coord = ScanPicture4ZoomInButton(image);
                if (coord != null)
                {
                    UpdateStatus(LogType.ZoomingIn, null);
                    InjectClickAtInvokeForced(coord.X, coord.Y);
                }
            }
            finally
            {
                image.Dispose();
            }
        }

        private Coordinate ScanPicture4ZoomInButton(Screenshot image)
        {
            var colors = new List<int>();
            colors.Add(Color.FromArgb(42, 47, 54).ToArgb()); // @ (0,0)
            colors.Add(Color.FromArgb(25, 30, 37).ToArgb()); // @ (+17,0)
            colors.Add(Color.FromArgb(172, 183, 199).ToArgb()); // @ (+14,+15)
            colors.Add(Color.FromArgb(174, 185, 201).ToArgb()); // @ (+14,+15)
            colors.Add(Color.FromArgb(172, 181, 198).ToArgb()); // @ (+7,+5)
            colors.Add(Color.FromArgb(174, 183, 200).ToArgb()); // @ (+7,+5)
            colors.Add(Color.FromArgb(127, 136, 153).ToArgb()); // @ (+8,+10)
            colors.Add(Color.FromArgb(151, 162, 181).ToArgb()); // @ (+8,+10)

            Coordinate coordinate = null;

            //image.Image.Save("scanZoom.png",ImageFormat.Png);

            // searching in bottom left corner 4 magnifier glass
            //Parallel.For(0, 300, (x,s) =>
            for(int x=0;x<300;x++)
            {
                for (int y = Settings.BotPictureHeight-50; y < Settings.BotPictureHeight; y++)
                {
                    {
                        if (image.GetPixelRgb(x, y) == colors[0] &&
                            image.GetPixelRgb(x+17, y) == colors[1] &&
                            (image.GetPixelRgb(x + 14, y + 15) == colors[2] || image.GetPixelRgb(x + 14, y + 15) == colors[3] )&&
                            (image.GetPixelRgb(x + 7, y + 5) == colors[4] || image.GetPixelRgb(x + 7, y + 5) == colors[5]) &&
                            (image.GetPixelRgb(x + 8, y + 10) == colors[6] || image.GetPixelRgb(x + 8, y + 10) == colors[7])
                            )
                        {
                            coordinate = new Coordinate(x+7, y+7);
                            break;
                        }
                        
                    }
                }
            }; 
            return coordinate;
        }

        private List<Coordinate> ScanPicture4Collect()
        {
            ItemCollection l_ItemCollection = ItemCollection.Instance;
            string l_CoordinateString;//Format xxxxyyyy
            Sleeping(500);
            InjectClickAtInvokeForced(150, 50);
            UpdateStatus(LogType.ScanStart, null);
            var sw = new Stopwatch();
            sw.Start();

            var crdList = new List<Coordinate>();
            var image = GetNewBrowserImageInvokeForced();
            try
            {
                if (IsPopupPresent(image))
                {
                    TryClosePopup();
                    BlockUntilPopupIs(false, Settings.WaitOnPopupInMillis);
                    image.Dispose();
                    image = GetNewBrowserImageInvokeForced();
                }
                var itemColor = Color.FromArgb(191, 169, 135).ToArgb();
                var goldBoxColor = Color.FromArgb(221, 189, 118).ToArgb();
                var plungedColor1 = Color.FromArgb(248, 238, 214).ToArgb();
                var plungedColor2 = Color.FromArgb(160, 144, 124).ToArgb();
                var plungedColor3 = Color.FromArgb(197, 183, 149).ToArgb();

                var wentBadColor1 = Color.FromArgb(255, 248, 229).ToArgb();
                var wentBadColor2 = Color.FromArgb(255, 247, 217).ToArgb();
                var wentBadColor3 = Color.FromArgb(43, 25, 4).ToArgb();
                uint num = 0;

                // Using picture scanner so we can use Parallel. Graphics in C# is not thread-safe
                //Parallel.For(0, Settings.BotPictureWidth, i =>
                for (int i = 0; i < Settings.BotPictureWidth; i++)
                {
                    for (int j = 50; j < Settings.BotPictureHeight-150; j++)
                    {

                        {
                            if (image.GetPixelRgb(i, j) == itemColor || image.GetPixelRgb(i, j) == goldBoxColor)
                            {
                                UpdateStatus(LogType.ScanFound,
                                             (++num).ToString());
                                //updateStatus(LogType.JustMessage, string.Format("x={0},y={1}", i, j));
                                crdList.Add(new Coordinate(i, j));
                                
                                var l_Item = new ItemCollection.Item();
                                l_CoordinateString = ItemCollection.MakePoint(i,j);
                                if (l_ItemCollection.Items.TryGetValue(l_CoordinateString, out l_Item))
                                {
                                    var existingItem = l_ItemCollection.Items[l_CoordinateString];
                                    //Item found in ItemCollection, update timestamp.
                                    l_ItemCollection.Items[l_CoordinateString] = new ItemCollection.Item(existingItem.m_OffsetX, existingItem.m_OffsetY, System.DateTime.Now.Ticks.ToString(),existingItem.YStepping,existingItem.LastClickWasSuccessful);
                                }
                                else
                                {
                                    //Item not found in ItemCollection, add new one.
                                    l_ItemCollection.Items[l_CoordinateString] = new ItemCollection.Item(Settings.CollectDistanceX, Settings.CollectDistanceY, System.DateTime.Now.Ticks.ToString(),0,true);
                                }
                                continue;
                            }
                            if (Settings.DoPlungedItemsClick)
                            {
                                // scan for plunged items
                                if (image.GetPixelRgb(i, j) == plungedColor1
                                    &&
                                    image.GetPixelRgb(i - 3, j + 12) ==
                                    plungedColor2
                                    &&
                                    image.GetPixelRgb(i + 9, j + 12) ==
                                    plungedColor3)
                                {
                                    UpdateStatus(LogType.PlungedScanFound,
                                                 (++num).ToString());
                                    crdList.Add(new Coordinate(i, j));

                                    var l_Item = new ItemCollection.Item();
                                    l_CoordinateString = ItemCollection.MakePoint(i, j);
                                    if (l_ItemCollection.Items.TryGetValue(l_CoordinateString, out l_Item))
                                    {
                                        //Item found in ItemCollection, update timestamp.
                                        var existingItem = l_ItemCollection.Items[l_CoordinateString];
                                        l_ItemCollection.Items[l_CoordinateString] = new ItemCollection.Item(existingItem.m_OffsetX, existingItem.m_OffsetY, System.DateTime.Now.Ticks.ToString(),existingItem.YStepping,existingItem.LastClickWasSuccessful);
                                    }
                                    else
                                    {
                                        //Item not found in ItemCollection, add new one.
                                        l_ItemCollection.Items[l_CoordinateString] = new ItemCollection.Item(Settings.CollectDistanceX, Settings.CollectDistanceY, System.DateTime.Now.Ticks.ToString(), 0, true);
                                    }
                                    continue;
                                }

                                // scan for "rotted supplies" items - dead fish
                                if (image.GetPixelRgb(i - 6, j + 18) ==
                                    wentBadColor1
                                    &&
                                    image.GetPixelRgb(i - 3, j + 18) ==
                                    wentBadColor2
                                    &&
                                    image.GetPixelRgb(i + 28, j + 10) ==
                                    wentBadColor3)
                                {
                                    UpdateStatus(LogType.RottedSuppliesScanFound,
                                                 (++num).ToString());
                                    crdList.Add(new Coordinate(i, j));

                                    var l_Item = new ItemCollection.Item();
                                    l_CoordinateString = ItemCollection.MakePoint(i, j);
                                    if (l_ItemCollection.Items.TryGetValue(l_CoordinateString, out l_Item))
                                    {
                                        //Item found in ItemCollection, update timestamp.
                                        var existingItem = l_ItemCollection.Items[l_CoordinateString];
                                        l_ItemCollection.Items[l_CoordinateString] = new ItemCollection.Item(existingItem.m_OffsetX, existingItem.m_OffsetY, System.DateTime.Now.Ticks.ToString(), existingItem.YStepping, existingItem.LastClickWasSuccessful);
                                    }
                                    else
                                    {
                                        //Item not found in ItemCollection, add new one.
                                        l_ItemCollection.Items[l_CoordinateString] = new ItemCollection.Item(Settings.CollectDistanceX, Settings.CollectDistanceY, System.DateTime.Now.Ticks.ToString(), 0, true);
                                    }
                                    continue;
                                }
                            }
                        }
                    }
                }

                sw.Stop();
                UpdateStatus(LogType.ScanStop, ((double) sw.ElapsedMilliseconds/1000).ToString("0.00"));
                if (Settings.SaveScanResults)
                {
                    SaveScanResultToFile(image, crdList, 0, Settings.CollectDistanceY, "scanresults.png");
                }
            }
            finally
            {
                // dispose potentially heavy resource
                image.Dispose();
            }

            return crdList;
        }

        private IEnumerable<Coordinate> ScanPicture4Production()
        {
            ItemCollection l_ItemCollection = ItemCollection.Instance;
            string l_CoordinateString;//Format xxxxyyyy

            UpdateStatus(LogType.ScanProductionStart, null);
            
            var sw = new Stopwatch();
            sw.Start();

            var crdResultList = new List<Coordinate>();
            var image = GetNewBrowserImageInvokeForced();
            try
            {
                if (IsPopupPresent(image))
                {
                    TryClosePopup();
                    BlockUntilPopupIs(false, Settings.WaitOnPopupInMillis);
                    image.Dispose();
                    image = GetNewBrowserImageInvokeForced();
                }
                var color = Color.FromArgb(240, 210, 174).ToArgb();
                var color2 = Color.FromArgb(131, 100, 67).ToArgb();
                uint num = 0;

                // Using picture scanner so we can use Parallel. Graphics in C# is not thread-safe
                //Parallel.For(0, Settings.BotPictureWidth, i =>
                for (int i = 0; i < Settings.BotPictureWidth; i++)
                {
                    for (int j = 50; j < Settings.BotPictureHeight-150; j++)
                    {
                        if ((image.GetPixelRgb(i, j) == color) &&
                            (image.GetPixelRgb(i + 1, j) == color2))
                        {
                            UpdateStatus(LogType.ScanProductionFound,
                                         (++num).ToString());
                            crdResultList.Add(new Coordinate(i, j));

                            var l_Item = new ItemCollection.Item();
                            l_CoordinateString = ItemCollection.MakePoint(i, j);
                            if (l_ItemCollection.Items.TryGetValue(l_CoordinateString, out l_Item))
                            {
                                //Item found in ItemCollection, update timestamp.
                                var existingItem = l_ItemCollection.Items[l_CoordinateString];
                                l_ItemCollection.Items[l_CoordinateString] = new ItemCollection.Item(existingItem.m_OffsetX, existingItem.m_OffsetY, System.DateTime.Now.Ticks.ToString(), existingItem.YStepping, existingItem.LastClickWasSuccessful);
                            }
                            else
                            {
                                //Item not found in ItemCollection, add new one.
                                l_ItemCollection.Items[l_CoordinateString] = new ItemCollection.Item(Settings.SupplyDistanceX, Settings.SupplyDistanceY, System.DateTime.Now.Ticks.ToString(), 0, true);
                            }
                        }
                    }
                }

                sw.Stop();
                UpdateStatus(LogType.ScanProductionStop, ((double) sw.ElapsedMilliseconds/1000).ToString("0.00"));
                if (Settings.SaveScanResults)
                {
                    SaveScanResultToFile(image, crdResultList, Settings.SupplyDistanceX, Settings.SupplyDistanceY,
                                         "supplyresults.png");
                }
            }
            finally
            {
                image.Dispose();
            }
            return crdResultList;
        }

        private Screenshot GetNewBrowserImage()
        {
            // 10 puta brzi od DrawImage ili GraphicsHelper.GdiDrawImage
            // render copy of image into already made one. this will avoid OutOfMemory problems

            var surface = ((BitmapSurface)_aweWebView.Surface);
            if(surface.Width==_internalScreenBitmap.Width && surface.Height==_internalScreenBitmap.Height)
                return PictureScanner.MakeScreenshot(surface.Buffer,surface.Width,surface.Height);

            return PictureScanner.MakeScreenshot(IntPtr.Zero, 0, 0);

        }

        private Screenshot GetNewBrowserImageInvokeForced()
        {
            GetNewBrowserImageCallback method = GetNewBrowserImage;
            return (Screenshot)Invoke(method, new object[0]);
        }
        #endregion

        #region Logging
        public void AddMessage(string message)
        {
            if (InvokeRequired)
            {
                AddMessageCallback method = AddMessage;
                Invoke(method, new object[] {message});
            }
            else
                _messagesForm.AddMessage(message);
        }

        public void AddMessage(string message,Exception ex)
        {
            if (InvokeRequired)
            {
                AddMessageWExceptionCallback method = AddMessage;
                Invoke(method, new object[] { message,ex });
            }
            else
                _messagesForm.AddMessage(message);
        }

        private void UpdateStatus(LogType status, string info)
        {
            if (InvokeRequired)
            {
                UpdateStatusCallback method = UpdateStatus;
                Invoke(method, new object[] {status, info});
            }
            else
            {
                switch (status)
                {
                    case LogType.JustMessage:
                        AddMessage(info);
                        return;

                    case LogType.ScanFound:
                        AddMessage(string.Format(Resource.MainForm_updateStatus_Item_X_found, info));
                        return;

                    case LogType.PlungedScanFound:
                        AddMessage(string.Format(Resource.MainForm_updateStatus_Plunged_item_X_found, info));
                        return;

                    case LogType.RottedSuppliesScanFound:
                        AddMessage(string.Format(Resource.MainForm_updateStatus_RottedSupply_item_X_found, info));
                        return;

                    case LogType.ScanProductionFound:
                        AddMessage(string.Format(Resource.MainForm_updateStatus_Production_item_X_found, info));
                        return;

                    case LogType.ScanStart:
                        _botStatus = BotStatus.Scan;
                        labelStatus.Text = Resource.MainForm_updateStatus_Bot_Scanning;
                        AddMessage(Resource.MainForm_updateStatus_Started_scanning);
                        return;

                    case LogType.ScanProductionStart:
                        _botStatus = BotStatus.ScanProd;
                        labelStatus.Text = Resource.MainForm_updateStatus_Bot_Scanning_production;
                        AddMessage(Resource.MainForm_updateStatus_Started_scanning_for_production);
                        return;

                    case LogType.ClickProductionStart:
                        _botStatus = BotStatus.ClickProd;
                        labelStatus.Text = Resource.MainForm_updateStatus_Bot_Clicking_production;
                        AddMessage(Resource.MainForm_updateStatus_Started_clicking_production_items);
                        return;

                    case LogType.ClickProductionSelect:
                        AddMessage(string.Format(Resource.MainForm_updateStatus_Selected_production_item_X, info));
                        return;

                    case LogType.ClickProductionDone:
                        AddMessage(Resource.MainForm_updateStatus_Selected_production_type);
                        return;

                    case LogType.ScanStop:
                        _botStatus = BotStatus.Idle;
                        labelStatus.Text = string.Format(Resource.MainForm_updateStatus_Bot_Scanning_done,info);
                        AddMessage(string.Format(Resource.MainForm_updateStatus_Scanning_done,info));
                        return;

                    case LogType.ScanProductionStop:
                        _botStatus = BotStatus.Idle;
                        labelStatus.Text = string.Format(Resource.MainForm_updateStatus_Bot_Production_scanning_done,info);
                        AddMessage(string.Format(Resource.MainForm_updateStatus_Production_scanning_done, info));
                        return;

                    case LogType.ClickDone:
                        AddMessage(string.Format(Resource.MainForm_updateStatus_Clicked_item_X, info));
                        return;

                    case LogType.ClickStart:
                        _botStatus = BotStatus.Click;
                        labelStatus.Text = Resource.MainForm_updateStatus_Bot_Clicking;
                        AddMessage(Resource.MainForm_updateStatus_Started_clicking);
                        return;

                    case LogType.ClickStop:
                        _botStatus = BotStatus.Idle;
                        labelStatus.Text = Resource.MainForm_updateStatus_Bot_Done_clicking;
                        AddMessage(Resource.MainForm_updateStatus_Bot_Done_clicking);
                        return;

                    case LogType.BotIdle:
                        _botStatus = BotStatus.Idle;
                        labelStatus.Text = Resource.MainForm_updateStatus_Bot_Idle;
                        AddMessage(string.Format(Resource.MainForm_updateStatus_Bot_Idle_for_X_min,
                                                 Settings.CheckEveryXMinutes));
                        return;

                    case LogType.ZoomingIn:
                        labelStatus.Text = Resource.MainForm_updateStatus_Bot_Zooming;
                        AddMessage(string.Format(Resource.MainForm_updateStatus_Trying_to_zoom_in));
                        return;

                    case LogType.CheckForZoom:
                        labelStatus.Text = Resource.MainForm_updateStatus_Check_for_zoom;
                        AddMessage(string.Format(Resource.MainForm_updateStatus_Check_for_zoom));
                        return;

                    case LogType.BotInPause:
                        labelStatus.Text = string.Format(Resource.MainForm_updateStatus_Bot_Paused,Settings.PauseCountdownForXSeconds);
                        AddMessage(string.Format(Resource.MainForm_updateStatus_Bot_Paused_for_X_min,
                                                 Settings.PauseCountdownForXSeconds));
                        return;
                }
            }
        }

        private static void SaveErrorToFile(string fileName, string errorInfo)
        {
            using (var writer = new StreamWriter(fileName,false))
            {
                writer.Write(errorInfo);
                writer.Close();
            }
        }

        private void SaveScanResultToFile(Screenshot image, IEnumerable<Coordinate> crdResultList, int deltaX, int deltaY,
                                          string fileName)
        {
            var setPixelFunc = new CSetPixel(image.Image);
            UpdateStatus(LogType.JustMessage, string.Format(Resource.MainForm_saveScanResultToFile_Saving_scan_results_to_file,fileName));
            try
            {
                foreach (Coordinate coordinate2 in crdResultList)
                {
                    image.Image.SetPixel(coordinate2.X, coordinate2.Y, Color.Black);
                    image.Image.SetPixel(coordinate2.X - 1, coordinate2.Y - 1, Color.White);
                    image.Image.SetPixel(coordinate2.X, coordinate2.Y - 1, Color.White);
                    image.Image.SetPixel(coordinate2.X + 1, coordinate2.Y - 1, Color.White);
                    image.Image.SetPixel(coordinate2.X - 1, coordinate2.Y, Color.White);
                    image.Image.SetPixel(coordinate2.X + 1, coordinate2.Y, Color.White);
                    image.Image.SetPixel(coordinate2.X - 1, coordinate2.Y + 1, Color.White);
                    image.Image.SetPixel(coordinate2.X, coordinate2.Y + 1, Color.White);
                    image.Image.SetPixel(coordinate2.X + 1, coordinate2.Y + 1, Color.White);

                    var fromCoordinate = new Coordinate(coordinate2.X, coordinate2.Y);
                    var toCoordinate = new Coordinate(coordinate2.X + deltaX, coordinate2.Y + deltaY);
                    Algorithms2D.Line(fromCoordinate, toCoordinate, setPixelFunc);

                    image.Image.SetPixel(toCoordinate.X - 1, toCoordinate.Y - 1, Color.White);
                    image.Image.SetPixel(toCoordinate.X - 2, toCoordinate.Y - 2, Color.White);
                    image.Image.SetPixel(toCoordinate.X + 1, toCoordinate.Y - 1, Color.White);
                    image.Image.SetPixel(toCoordinate.X + 2, toCoordinate.Y - 2, Color.White);
                }
                image.Image.Save(fileName);
                UpdateStatus(LogType.JustMessage, string.Format(Resource.MainForm_SaveScanResultToFile_ScanResultsSaved,fileName));
            }
            catch (Exception exception)
            {
                UpdateStatus(LogType.JustMessage, string.Format("Save scan results exception. Error: {0}", exception.Message));
                Logger.LogError("Save scan results exception.", exception);
            }
        }

        #endregion

        #region Private helper methods

        /// <summary>
        /// randomizuje za odredjeni procenat pocetnu vrednost u + i u -
        /// </summary>
        /// <param name="startingValue"></param>
        /// <param name="percentage"></param>
        /// <param name="allowNegative">da li sme da vrati negativnu vrednost</param>
        /// <returns></returns>
        private static int Randomizer(int startingValue, int percentage, bool allowNegative)
        {
            int min = -(startingValue * percentage) / 100;
            int max = (startingValue * percentage) / 100;
            if (!allowNegative)
            {
                min = 0;
            }

            var rnd = new Random(DateTime.Now.Millisecond);

            return startingValue - rnd.Next(min, max + 1);
        }

        private void ThreadedTaskExceptionHandler(Task task)
        {
            var exception = task.Exception;
            UpdateStatus(LogType.JustMessage, string.Format("BotThread exception occured: {0}", exception.Message));

            Logger.LogError("ThreadedTaskExceptionHandler exception occured. Trying to restart ivanJo BOT thread only.", exception);
            BotIdleInvokeForced();
        }

        /// <summary>
        /// Refresh awesomium
        /// </summary>
        /// <param name="performAutoLogin">Try to auto login with last used world</param>
        private void RefreshWebPage(bool performAutoLogin)
        {
            try
            {
                if (!string.IsNullOrEmpty(Settings.LastLoadedWorld))
                {
                    ReloadWebCore();
                    InitializeView(WebCore.CreateWebView(BrowserWidth, BrowserHeight, _aweWebSession));
                    var interceptor = WebCore.ResourceInterceptor as FoEResourceInterceptor;
                    if ((performAutoLogin || _blueScreenDetected) 
                            && !string.IsNullOrWhiteSpace(Settings.LastLoadedWorld))
                        AutoLogin();
                }
            }
            finally
            {
                _blueScreenDetected = false;
            }
        }

        /// <summary>
        /// Restarts application
        /// </summary>
        private void RestartApplication()
        {
            //Program.RestartApplication();
            PerformApplicationRestart = true;
            KeepAutoReloadButton = buttonAutoReload.Checked;
            Close();
        }

        /// <summary>
        /// Restart app due BSOD or Game window not recognized for X times
        /// </summary>
        private void RestartApplicationInvokeForced()
        {
            RestartApplicationCallback method = RestartApplication;
            Invoke(method, new object[] { });
        }

        private void AutoLogin()
        {
            _allowedInteraction = false;
            try
            {
                AddMessage(Resource.MainForm_AutoLogin_LoadingLastUsedWorld);
                const int sleepInMillis = 50;
                const int maxTimeoutCount = 10000 / sleepInMillis;
                int tries = 0;
                while (!_documentLoadedcompletly && tries < maxTimeoutCount)
                {
                    Sleeping(sleepInMillis);
                    tries++;
                }
                if (tries >= maxTimeoutCount)
                {
                    AddMessage(
                        string.Format(Resource.MainForm_AutoLogin_PageLoadTooSlow,
                                      maxTimeoutCount / 1000));
                    return;
                }
                AddMessage(Resource.MainForm_AutoLogin_WaitPageToCompleteLoading);

                const int maxTimeForDocumentToBecomeReady = 10000 / sleepInMillis;
                tries = 0;
                while (tries<maxTimeForDocumentToBecomeReady)
                {
                    Sleeping(sleepInMillis);
                    if(((JSObject)_aweWebView.ExecuteJavascriptWithResult("document.forms[\"landing_login_form\"][\"login_world_id\"]"))!=null)
                    {
                        tries = maxTimeForDocumentToBecomeReady;
                    }
                }
                dynamic formWorldId =
                    (JSObject)
                    _aweWebView.ExecuteJavascriptWithResult("document.forms[\"landing_login_form\"][\"login_world_id\"]");
                dynamic formSubmit =
                    (JSObject)
                    _aweWebView.ExecuteJavascriptWithResult("document.forms[\"landing_login_form\"][\"submit_login\"]");
                // Make sure we have the object.
                if (formWorldId == null)
                {
                    AddMessage(Resource.MainForm_AutoLogin_PageNotReady);
                    return;
                }
                using (formWorldId)
                {
                    // Invoke 'form.click' just as you would in JS!
                    formWorldId.value = Settings.LastLoadedWorld;
                    try
                    {
                        formSubmit.click();
                    }
                    catch{}
                }
            }
            finally
            {
                _allowedInteraction = true;
            }
        }

        private void ResizeMainFormIfNecessary()
        {
            if (Settings.BotPictureWidth < Width)
                Width = Settings.BotPictureWidth + 8;
            if (Settings.BotPictureHeight < Height)
                Height = Settings.BotPictureHeight + panelButtonControls.Height + 27;
        }

        private void ResizeBrowser(bool userFriendly)
        {
            if (userFriendly)
            {
                _isViewUserFriendly = true;
                PictureContainer.Dock = DockStyle.Fill;

                _aweWebView.Resize(BrowserWidth, BrowserHeight);
                AddMessage(Resource.MainForm_resizeBrowser_Entered_user_friendly_view_mode);
                buttonChangeView.Text = Resource.MainForm_resizeBrowser_ViewMode_User_friendly;
            }
            else
            {
                _isViewUserFriendly = false;
                PictureContainer.Dock = DockStyle.None;
                PictureContainer.Height = Settings.BotPictureHeight;
                PictureContainer.Width = Settings.BotPictureWidth;

                _aweWebView.Resize(PictureContainer.Width, PictureContainer.Height);
                AddMessage(Resource.MainForm_resizeBrowser_Entered_bot_friendly_view_mode);
                buttonChangeView.Text = Resource.MainForm_resizeBrowser_ViewMode_Bot_friendly;

                ResizeMainFormIfNecessary();
            }
        }

        private void ResizeView()
        {
            if ((_aweWebView == null) || !_aweWebView.IsLive || _formIsResizing || !_isViewUserFriendly)
                return;

            if (_needsResize)
            {
                // Request a resize.
                _aweWebView.Resize(BrowserWidth, BrowserHeight);

                _needsResize = false;
            }
        }

        private void ToggleFormResize(bool allowResize)
        {
            if (InvokeRequired)
            {
                var d = new ToggleFormResizeCallback(ToggleFormResize);
                Invoke(d, new object[] { allowResize });
            }
            else
            {
                MinimizeBox = allowResize;
                MaximizeBox = allowResize;
                FormBorderStyle = allowResize ? FormBorderStyle.Sizable : FormBorderStyle.FixedSingle;
            }
        }

        /// <summary>
        /// Stores MainForm and MessageLog form window positions in application settings
        /// </summary>
        internal void UpdateWindowsPosition()
        {
            if(Left+Width>0)
                Settings.MainFormInfo = new Rectangle(Left, Top, Width, Height);
            if (MessageLogForm.Left + MessageLogForm.Width > 0)
                Settings.MessageLogFormInfo = new Rectangle(MessageLogForm.Left, MessageLogForm.Top, MessageLogForm.Width, MessageLogForm.Height);
        }

        /// <summary>
        /// Applies window position stored in settings
        /// </summary>
        internal void ApplyNewWindowsPosition()
        {
            ApplyFormPositionAndSize();

            _messagesForm.ApplyFormPositionAndSize();
        }

        private void ApplyFormPositionAndSize()
        {
            this.ApplyDimensions(Settings.MainFormInfo.IsEmpty ? DefaultFormPositionAndSize : Settings.MainFormInfo);
        }

        private void Sleeping(int sleepTimeInMilliseconds)
        {
            int count = sleepTimeInMilliseconds/10;
            int i = 0;
            while(i<count)
            {
                Thread.Sleep(10);
                Application.DoEvents();
                i++;
            }

        }

        private void RestartPauseTimer()
        {
            if (_botStatus != BotStatus.Idle || Settings.PauseCountdownForXSeconds==0)
                return;

            _pauseTimer=new TimeSpan(0,0,Settings.PauseCountdownForXSeconds);
            UpdateStatus(LogType.BotInPause, Settings.PauseCountdownForXSeconds.ToString());

            timerBotCountdown.Stop();
            timerPauseCountdown.Start();
        }

        private void ApplyAutoRefreshSetting()
        {
            timerAutoRefresh.Tick -= AutoRefreshTick;
            if (Settings.AutoRefreshAfterXMinutes <= 0)
            {
                timerAutoRefresh.Stop();
                _autoRefreshTicksCounter = 0;
            }
            else
            {
                timerAutoRefresh.Tick += AutoRefreshTick;
                if(buttonAutoReload.Enabled && buttonAutoReload.Checked)
                    timerAutoRefresh.Start();
                _autoRefreshTicksCounter = 0;
            }
        }

        private void AutoRefreshTick(object sender, EventArgs e)
        {
            _autoRefreshTicksCounter++;
            if(_autoRefreshTicksCounter>=Settings.AutoRefreshAfterXMinutes)
            {
                // if we're in the middle od scanning/clicking we don't autorefresh since it will
                // create exception
                if(_allowedInteraction)
                {
                    _autoRefreshTicksCounter--;
                    return;
                }
                _autoRefreshTicksCounter = 0;
                AddMessage(Resource.MainForm_AutoRefreshTick_Reload);
                RefreshWebPage(true);
            }
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            var sw = new Stopwatch();
            sw.Start();
            
            RefreshWebPage(true);

            sw.Stop();
            AddMessage(string.Format("duration: {0} msec", sw.ElapsedMilliseconds));
        }



        public ResourceResponse OnRequest(ResourceRequest request)
        {
            var req = request;
            return null;
        }
        
    }
}