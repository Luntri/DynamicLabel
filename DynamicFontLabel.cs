using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using OpenNETCF.Windows.Forms;
using System.Windows.Forms;
namespace SpaghettiCode
{
    public class DynamicFontLabel : System.Windows.Forms.Label
    {
        private bool disposed = false;
             protected override void Dispose(bool disposing)
        {
            if (this.disposed)
                return;
            this.disposed = true;
            base.Dispose(disposing);
        }
        ~DynamicFontLabel() { Dispose(false); }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            this.Refresh();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (System.Diagnostics.Debugger.IsAttached)
                ;
            var Size = e.Graphics.MeasureString(this.Text, this.Font);
            //check if the string is too big for the label and needs scaling down
            if (Size.Height > this.Height || Size.Width > this.Width)
            {
                float FontSize = this.Font.Size;
                while (Size.Height*Size.Width > this.Height* this.Width)
                {
                    FontSize -= 0.5f;
                    Size = e.Graphics.MeasureString(this.Text,
                        new System.Drawing.Font(this.Font.Name, FontSize, this.Font.Style));
                }
                this.Font = new System.Drawing.Font(this.Font.Name, FontSize, this.Font.Style);
            }
            else
            {
                float FontSize = this.Font.Size;
                FontSize += 0.5f;
                Size = e.Graphics.MeasureString(this.Text,
                    new System.Drawing.Font(this.Font.Name, FontSize, this.Font.Style));
                if (Size.Height < this.Height || Size.Width < this.Width)
                    while (Size.Height < this.Height || Size.Width < this.Width)
                    {
                        FontSize -= 0.5f;
                        Size = e.Graphics.MeasureString(this.Text,
                            new System.Drawing.Font(this.Font.Name, FontSize, this.Font.Style));
                    }
                this.Font = new System.Drawing.Font(this.Font.Name, FontSize, this.Font.Style);
            }
            
        }
        public DynamicFontLabel()
        {
            
        }
    }
}
