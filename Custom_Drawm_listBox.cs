using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using Microsoft.WindowsCE.Forms;
using Symbol.Barcode2;
using OpenNETCF.Windows.Forms;
using OpenNETCF.WindowsCE;
using System.IO;

namespace SpaghettiCode
{
    //using CDLB = Custom_Drawn_listBox
    
    public class Custom_Drawn_listBox : ListBox2
    {
        private bool disposed = false;
        //with this constructor remmber to add items
        public Custom_Drawn_listBox()
        {
            int height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
            int width = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;

            this.ItemHeight = 70 > (int)Math.Floor(height / 6) ? 70 : (int)Math.Floor(height / 6) ;  //at least 6 per screen, usually more
            this.Height = 70 > (int)Math.Floor(height / 6) ? 70 : (int)Math.Floor(height / 6);  //at least 6 per screen, usually more
            this.Width = (int)Math.Floor(width / 2) - 15;
            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.WrapText = true;
            this.DrawItem += new DrawItemEventHandler(Custom_Drawn_listBox_DrawItem);
            this.LostFocus += new EventHandler(Custom_Drawn_listBox_LostFocus);
            this.GotFocus += new EventHandler(Custom_Drawn_listBox_GotFocus);
            this.Font = new Font("Helvetica", 8, FontStyle.Regular);
            //this.TextChanged += new EventHandler(Custom_Drawn_listBox_TextChanged);
            //this.Height
            this.BackgroundImage = null;
        }

        public bool SuppressDrawing = false;
        void Custom_Drawn_listBox_TextChanged(object sender, EventArgs e)
        {
            using (Graphics GC = this.CreateGraphics())
            {
                SizeF text_size = GC.MeasureString(this.Text, this.Font);
                if (this.ItemHeight < text_size.Height)
                    this.ItemHeight =(int) text_size.Height + (int) (10 - text_size.Height % 10);
            }
        }
        protected new virtual void Dispose(bool disposing)
        {
            if (disposed) return;

            // Release any managed resources here.
            if (disposing)
            {
                this.DrawItem -= new DrawItemEventHandler(Custom_Drawn_listBox_DrawItem);
                this.LostFocus -= new EventHandler(Custom_Drawn_listBox_LostFocus);
                this.GotFocus -= new EventHandler(Custom_Drawn_listBox_GotFocus);
            }

            // Release any unmanaged resources not wrapped by safe handles here.
            
            // Call the base class implementation.
            base.Dispose(true);
        }
        ///<summary>adapts to items.text lenghts and widths, takes width as 'fixed'</summary>
        public void Recalibrate_height()   
        {
            using (Graphics GC = this.CreateGraphics())
            {
                string concat = null;
                for (int i = 0; i < this.Items.Count; i++)
                {
                    concat += this.Items[i].Text + " ";
                }
                string[] split = concat.Split('\n' );
                float avrg_height = 0, exceeded_width=0;
                for (int i = 0; i < split.Length; i++)
                {
                    SizeF qwe = GC.MeasureString(split[i], this.Font);
                    avrg_height += qwe.Height;
                    if (this.Width < qwe.Width)
                        exceeded_width++;
                }
                float ss = avrg_height / split.Length;
                this.Height += (int) (exceeded_width * ss);
                
                /*
                SizeF text_size = GC.MeasureString(concat, this.Font);
                if (this.ItemHeight < text_size.Height)
                    this.ItemHeight = (int)text_size.Height + (int)(10 - text_size.Height % 10);*/
            }
        }
        void Custom_Drawn_listBox_GotFocus(object sender, EventArgs e)
        {
            ListBox2 pp = sender as ListBox2;
            if (pp != null) pp.Refresh();
        }

        void Custom_Drawn_listBox_LostFocus(object sender, EventArgs e)
        {
            ListBox2 pp = sender as ListBox2;
            if(pp != null) pp.Refresh();
        }
        //dla dodawania itemzuff
        public void AddItem(string obj)
        {
            this.Items.Add(new ListItem(obj));
        }
        public void AddItem(object obj)
        {
            this.Items.Add(new ListItem(obj.ToString()));
        }
        public void AddItem(ListItem obj)
        {
            this.Items.Add(obj);
        }
        public static Font CreateRotatedFont(string fontname, int height, int angleInDegrees, Graphics g)
        {
            LogFont logf = new LogFont();
            logf.Height = -1 * height;
            logf.FaceName = fontname;
            logf.Escapement = angleInDegrees * 10;
            logf.Orientation = logf.Escapement;
            logf.CharSet = LogFontCharSet.Default;
            logf.OutPrecision = LogFontPrecision.Default;
            logf.ClipPrecision = LogFontClipPrecision.Default;
            logf.Quality = LogFontQuality.ClearType;
            logf.PitchAndFamily = LogFontPitchAndFamily.Default;
            
            return Font.FromLogFont(logf);
        }
        public virtual void Custom_Drawn_listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            //default drawing, it is recommended to override
            if (SuppressDrawing)
                return;
            ListBox2 sender_obj = sender as ListBox2;
            string concat = null;
            for (int i = 0; i < sender_obj.Items.Count; i++)
            {
                concat += sender_obj.Items[i].Text +" ";
            }
            bool focused = this.Focused;
            SizeF size = e.Graphics.MeasureString(concat, sender_obj.Font);
            bool state_selected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected || e.State == DrawItemState.Focus);
            
            RectangleF rect = new RectangleF(e.Bounds.Left, e.Bounds.Top, e.Bounds.Width, e.Bounds.Height);

            if (focused)
                e.Graphics.FillRectangle(new SolidBrush(Color.Silver), e.Bounds);
            else
                e.Graphics.FillRectangle(new SolidBrush(Color.Bisque), e.Bounds);


            if (sender_obj.Size.Width < size.Width)
            {
                e.Graphics.DrawString(concat, e.Font, new SolidBrush(Color.Black), rect, new StringFormat(StringFormatFlags.NoClip));
            }
            else
                e.Graphics.DrawString(concat, e.Font, new SolidBrush(Color.Black), e.Bounds.Left +
                    (e.Bounds.Width / 2 - size.Width / 2), e.Bounds.Y);
        }
    }
}
