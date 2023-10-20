using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class AdaScrollRichTextBox : RichTextBox
    {
        Panel panel1;
        public AdaScrollRichTextBox()
        {
            InitializeLineId();
        }

        private void InitializeLineId()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
            this.panel1 = new Panel();
            this.panel1.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel1.Width = 35;
            this.panel1.Dock = DockStyle.Left;
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(5)))), ((int)(((byte)(5)))));
            this.SelectionIndent = 35;
            this.Controls.Add(this.panel1); 
        }
        public void showLineNo()
        {
            try
            {
                this.SelectionIndent = 35;
                Point p = this.Location;
                int crntFirstIndex = this.GetCharIndexFromPosition(p);

                int crntFirstLine = this.GetLineFromCharIndex(crntFirstIndex);

                Point crntFirstPos = this.GetPositionFromCharIndex(crntFirstIndex);

                p.Y += this.Height;

                int crntLastIndex = this.GetCharIndexFromPosition(p);

                int crntLastLine = this.GetLineFromCharIndex(crntLastIndex);
                Point crntLastPos = this.GetPositionFromCharIndex(crntLastIndex);

                //准备画图
                Graphics g = this.panel1.CreateGraphics();
                //新建图，之后的绘制在图上进行，绘制完毕之后绘制到g上
                Bitmap bufferimage = new Bitmap(this.panel1.Width, this.panel1.Height);
                Graphics g1 = Graphics.FromImage(bufferimage);
                g1.Clear(this.BackColor);
                g1.SmoothingMode = SmoothingMode.HighQuality; //高质量
                g1.PixelOffsetMode = PixelOffsetMode.HighQuality; //高像素偏移质量

                Font font = new Font(this.Font, this.Font.Style);

                SolidBrush brush = new SolidBrush(Color.Green);

                //画图开始



                Rectangle rect = this.panel1.ClientRectangle;
                brush.Color = this.panel1.BackColor;

                g1.FillRectangle(brush, 0, 0, this.panel1.ClientRectangle.Width, this.panel1.ClientRectangle.Height);

                brush.Color = Color.Green;

                //绘制行号

                int lineSpace = 0;

                if (crntFirstLine != crntLastLine)
                {
                    lineSpace = (crntLastPos.Y - crntFirstPos.Y) / (crntLastLine - crntFirstLine);

                }

                else
                {
                    lineSpace = Convert.ToInt32(this.Font.Size);

                }

                //这里2.6参数可以按代码实际修改
                int brushX = this.panel1.ClientRectangle.Width - Convert.ToInt32(font.Size * 2.6);
                //这里0.1参数可以按代码实际修改
                int brushY = crntLastPos.Y + Convert.ToInt32(font.Size * 0.1f);
                for (int i = crntLastLine; i >= crntFirstLine; i--)
                {

                    g1.DrawString((i + 1).ToString(), font, brush, brushX, brushY);

                    brushY -= lineSpace;
                }
                g.DrawImage(bufferimage, 0, 0);

                g1.Dispose();
                g.Dispose();
                font.Dispose();
                brush.Dispose();
            }
            catch
            { }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            showLineNo();
            base.OnTextChanged(e);
        }
        protected override void OnVScroll(EventArgs e)
        {
            showLineNo();
            base.OnVScroll(e);
        }

    }

}
