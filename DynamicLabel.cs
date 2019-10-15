using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
namespace SpaghettiCode 
{
    class DynamicLabel : Control
    {
        public bool disposed = false;
        private System.Drawing.ContentAlignment textAlign = ContentAlignment.TopLeft;
        public System.Drawing.ContentAlignment TextAlign {
            get
            {
                return textAlign;
            }
            set
            {
                textAlign = value;
                this.Invalidate();
            }
        }
        bool textChanged = false;
        public bool wysrodkujY = false;
        public bool ResizeUp = false;
        public bool DrawMidOnce = true;
        public bool DrawMidPermamently = false;
        protected override void OnTextChanged(EventArgs e)
        {
            
            base.OnTextChanged(e);
            textChanged = true;
            this.Refresh();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            var Size = e.Graphics.MeasureString(this.Text, this.Font);
            float FontSize = this.Font.Size; 
            //if (!textChanged)
            //{
            //    return;
            //}
            if (string.IsNullOrEmpty(this.Text))
                goto koniec;
            //check if the string is too big for the label and needs scaling down
            if (Size.Height * Size.Width > this.Height * this.Width)
            //    || (Size.Height+5 > this.Height || Size.Width+5 >this.Width) )
            {
                while ((Size.Height * Size.Width > this.Height * this.Width)
                     || (Math.Ceiling(Size.Width/this.Width) * Size.Height>this.Height))
                    // || (Size.Height+5 > this.Height || Size.Width+5 >this.Width) )
                {
                    if (Math.Ceiling(Size.Width / this.Width) > 2)
                        FontSize -= FontSize / (float)Math.Ceiling(Size.Width / this.Width);
                    FontSize -= 0.5f;
                    Size = e.Graphics.MeasureString(this.Text,
                        new System.Drawing.Font(this.Font.Name, FontSize, this.Font.Style));
                    if (Size.IsEmpty || Size.Height == 0 || Size.Width == 0)
                        break;
                }
               // FontSize -= 5.0f;
                this.Font = new System.Drawing.Font(this.Font.Name, FontSize, this.Font.Style);
                textChanged = false ;
            }
            else if (ResizeUp)
            {
                
                FontSize += 0.5f;
                Size = e.Graphics.MeasureString(this.Text,
                    new System.Drawing.Font(this.Font.Name, FontSize, this.Font.Style));
                if ((Size.Height * Size.Width < this.Height * this.Width) && (Size.Height+5 < this.Height && Size.Width+5 <this.Width))
                    while ((Size.Height * Size.Width < this.Height * this.Width) 
                        && this.Height> Size.Height+5
                        && this.Width > Size.Width+5)
                    {
                        FontSize += 0.5f;
                        Size = e.Graphics.MeasureString(this.Text,
                            new System.Drawing.Font(this.Font.Name, FontSize, this.Font.Style));
                        if (Size.IsEmpty || Size.Height == 0 || Size.Width == 0)
                            break;
                        //this is so we dont go over the scale
                         if ((Size.Height * Size.Width < this.Height * this.Width) && this.Height > Size.Height + 5    && this.Width > Size.Width+5)
                            continue;
                        else
                        {
                            FontSize -= 0.5f;
                            this.Font = new System.Drawing.Font(this.Font.Name, FontSize, this.Font.Style);
                            textChanged = false;
                            break;
                        }
                    }
                
            }
            koniec:
            Size = e.Graphics.MeasureString(this.Text,
                          new System.Drawing.Font(this.Font.Name, this.Font.Size , this.Font.Style));
            if (this.TextAlign == ContentAlignment.TopCenter && !this.wysrodkujY)
                e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), //this.Bounds.Left +
                        (this.Bounds.Width / 2 - Size.Width / 2), 0 /*this.Bounds.Y*/);
            else if (this.TextAlign == ContentAlignment.TopCenter && this.wysrodkujY)
                e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), //this.Bounds.Left +
                        (this.Bounds.Width / 2 - Size.Width / 2), this.Bounds.Height / 2 - Size.Height / 2);
            else if (DrawMidOnce)
                e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), //this.Bounds.Left +
                    (this.Bounds.Width / 2 - Size.Width / 2), this.Bounds.Height / 2 - Size.Height / 2);
            else
                e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), ClientRectangle);
            textChanged = false;
            DrawMidOnce = false;
        }
       // ~DynamicLabel() { Dispose(false); }
        protected override void Dispose(bool disposing)
        {
            if (this.disposed)
                return;
            this.disposed = true;
            base.Dispose(disposing);
        }
        public DynamicLabel()
        {
            this.BackColor = Color.AliceBlue;
            this.TextAlign = ContentAlignment.TopLeft;
        }
    }
}
