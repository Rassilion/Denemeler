using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using log4net.Config;

namespace ForgeBot
{
    public partial class MessagesForm : Form
    {
        private delegate void HideWindowCallback();

        private readonly ApplicationSettings _applicationSettings;
        private readonly string[] _linesToShow;
        private const string TitleFormat = "ivanJo FoE BOT. Time: {0}";
        private const string EmptyMessage = "Empty message";

        internal static Rectangle DefaultFormPositionAndSize
        {
            get { return new Rectangle(32, 32, 248, 431); }
        }

        public MessagesForm(ApplicationSettings appSettings)
        {
            InitializeComponent();
            _applicationSettings = appSettings;
            _linesToShow=new string[_applicationSettings.MaxLogLinesDisplayed];

            FormClosing += MessagesForm_FormClosing;
            timer1.Start();
        }

        public void AddMessage(string message)
        {
            var line = string.Format("[{0:HH:mm:ss}] {1}{2}", DateTime.Now, message ?? EmptyMessage, Environment.NewLine);
            
            textBox1.Text = Rollover(line);

            Logger.LogInfo(message);

            //textBox1.Select(textBox1.Text.Length, 0);
            //textBox1.ScrollToCaret();
        }
        public void AddMessage(string message,Exception ex)
        {
            var line = string.Format("[{0:HH:mm:ss}] Exception occured: {1}{2}", DateTime.Now, ex.Message, Environment.NewLine);

            textBox1.Text = Rollover(line);

            Logger.LogError(message,ex);
            //textBox1.Select(textBox1.Text.Length, 0);
            //textBox1.ScrollToCaret();
        }

        /// <summary>
        /// Does roll-over log
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private string Rollover(string line)
        {
            for (int i = _applicationSettings.MaxLogLinesDisplayed - 1; i > 0; i--)
            {
                _linesToShow[i] = _linesToShow[i - 1];
            }
            _linesToShow[0] = line;
            return string.Join(string.Empty,_linesToShow);
        }

        // Use this event handler for the FormClosing event.
        private void MessagesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            HideWindow();
            e.Cancel = true; // this cancels the close event.
        }

        private void HideWindow()
        {
            if (InvokeRequired)
            {
                HideWindowCallback method = HideWindow;
                Invoke(method, new object[] { });
            }
            else
            {
                Hide();
            }
        }
        protected override void OnShown(EventArgs e)
        {
            ApplyFormPositionAndSize();
            base.OnShown(e);

        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            SaveFormPosition();
        }

        private void SaveFormPosition()
        {
            _applicationSettings.MessageLogFormInfo = new Rectangle(Top, Left, Width, Height);
        }

        internal void ApplyFormPositionAndSize()
        {
            this.ApplyDimensions(_applicationSettings.MessageLogFormInfo.IsEmpty
                                     ? DefaultFormPositionAndSize
                                     : _applicationSettings.MessageLogFormInfo);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Text = string.Format(TitleFormat, DateTime.Now.ToString("HH:mm:ss"));
        }
    }
}