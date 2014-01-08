using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Awesomium.Core;
using Awesomium.Windows.Forms;

namespace ForgeBot
{
    public partial class ChildForm : Form
    {
        #region Fields
        private WebView webView;
        private ImageSurface surface;
        private bool needsResize;
        private WebSession session;
        private BindingSource bindingSource;
        #endregion
        
        public ChildForm()
        {
            InitializeComponent();
        }

        // Used to create child (popup) windows.
        internal ChildForm(WebView view, int width, int height)
        {
            this.Width = width;
            this.Height = height;

            InitializeComponent();

            // Initialize the view.
            InitializeView(view);

            // We should immediately call a resize,
            // after wrapping child views.
            if ( view != null )
                view.Resize( width, height );
        }

        private void InitializeView(WebView view)
        {
            if (view == null)
                return;

            // Create an image surface to render the
            // WebView's pixel buffer.
            surface = new ImageSurface();
            surface.Updated += OnSurfaceUpdated;

            webView = view;

            // Assign our surface.
            webView.Surface = surface;

            // Handle some important events.
            webView.CursorChanged += OnCursorChanged;
            //webView.TitleChanged += OnTitleChanged;

            bindingSource = new BindingSource() { DataSource = webView };
            this.DataBindings.Add(new Binding("Text", bindingSource, "Title", true));

            //webView.AddressChanged += OnAddressChanged;
            //webView.ShowCreatedWebView += OnShowNewView;
            //webView.Crashed += OnCrashed;

            // Load a URL, if this is not a child view.
            if (webView.ParentView == null)
                // Tip: /ncr = No Country Redirect ;-)
                webView.Source = new Uri("http://www.forgeofempires.com/");

            // Give focus to the view.
            webView.FocusView();
        }

        private void ResizeView()
        {
            if ((webView == null) || !webView.IsLive)
                return;

            if (needsResize)
            {
                // Request a resize.
                webView.Resize(this.ClientSize.Width, this.ClientSize.Height);
                needsResize = false;
            }
        }
        private void OnCursorChanged(object sender, CursorChangedEventArgs e)
        {
            // Update the cursor.
            this.Cursor = Awesomium.Windows.Forms.Utilities.GetCursor(e.CursorType);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            if ((surface != null) && (surface.Image != null))
                e.Graphics.DrawImageUnscaled(surface.Image, 0, 0);
            else
                base.OnPaint(e);
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            this.Opacity = 1.0D;

            if ((webView == null) || !webView.IsLive)
                return;

            Debug.Print(webView.Source.AbsoluteUri);

            webView.FocusView();
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);

            if ((webView == null) || !webView.IsLive)
                return;

            // Let popup windows be semi-transparent,
            // when they are not active.
            if (webView.ParentView != null)
                this.Opacity = 0.8D;

            webView.UnfocusView();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // Get if this is form hosting a child view.
            bool isChild = webView.ParentView != null;

            // Destroy the WebView.
            if (webView != null)
                webView.Dispose();

            // The surface that is currently assigned to the view,
            // does not need to be disposed. It will be disposed 
            // internally.

            base.OnFormClosed(e);

            // Shut down the WebCore last.
            if (!isChild)
                WebCore.Shutdown();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if ((webView == null) || !webView.IsLive)
                return;

            if (this.ClientSize.Width > 0 && this.ClientSize.Height > 0)
                needsResize = true;

            // Request resize, if needed.
            this.ResizeView();
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            if ((webView == null) || !webView.IsLive)
                return;

            webView.InjectKeyboardEvent(e.GetKeyboardEvent());
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if ((webView == null) || !webView.IsLive)
                return;

            webView.InjectKeyboardEvent(e.GetKeyboardEvent(WebKeyboardEventType.KeyDown));
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if ((webView == null) || !webView.IsLive)
                return;

            webView.InjectKeyboardEvent(e.GetKeyboardEvent(WebKeyboardEventType.KeyUp));
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if ((webView == null) || !webView.IsLive)
                return;

            webView.InjectMouseDown(e.Button.GetMouseButton());
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if ((webView == null) || !webView.IsLive)
                return;

            webView.InjectMouseUp(e.Button.GetMouseButton());
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if ((webView == null) || !webView.IsLive)
                return;

            webView.InjectMouseMove(e.X, e.Y);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if ((webView == null) || !webView.IsLive)
                return;

            webView.InjectMouseWheel(e.Delta, 0);
        }
        private void OnSurfaceUpdated(object sender, SurfaceUpdatedEventArgs e)
        {
            // When the surface is updated, invalidate the 'dirty' region.
            // This will force the form to repaint that region.
            Invalidate(e.DirtyRegion.ToRectangle(), false);
        }
    }
}
