using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ForgeBot
{

    public class NoFlickerPanel : Panel
    {
        public NoFlickerPanel()
        {
            //Enable these styles to reduce flicker
            //1. Enable user paint.
            this.SetStyle(ControlStyles.UserPaint, true);
            //2. Enable double buffer.
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //3. Ignore a windows erase message to reduce flicker.
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }
    }

}
