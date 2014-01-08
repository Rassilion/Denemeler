using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ForgeBot
{
    internal static class Helper
    {

        internal static void ApplyDimensions(this Form form,Rectangle dimensions)
        {
            form.Top = dimensions.Top;
            form.Left = dimensions.Left;
            form.Width = dimensions.Width;
            form.Height = dimensions.Height;
        }
    }
}
