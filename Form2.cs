using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static RLM.Form1;

namespace RLM
{
    public partial class Form2 : Form
    {
        public Form2(string str1)
        {
            str = str1;
            InitializeComponent();
        }
        // 设置此窗体为活动窗体：
        // 将创建指定窗口的线程带到前台并激活该窗口。键盘输入直接指向窗口，并为用户更改各种视觉提示。
        // 系统为创建前台窗口的线程分配的优先级略高于其他线程。
        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        // 设置此窗体为活动窗体：
        // 激活窗口。窗口必须附加到调用线程的消息队列。
        [DllImport("user32.dll", EntryPoint = "SetActiveWindow")]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        // 设置窗体位置
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int Width, int Height, int flags);



        public event childclose closefather;
        public string str;
        public int X, Y;    
        private void Form2_Load(object sender, EventArgs e)
        {

           
            trackBar1.Maximum = 100;
            SetWindowPos(this.Handle, -1, 0, 0, 0, 0, 0x0001 | 0x0002 | 0x0010 | 0x0080);

            // 设置本窗体为活动窗体
            SetActiveWindow(this.Handle);
            SetForegroundWindow(this.Handle);

            // 设置窗体置顶
            this.TopMost = true;
          
          string[] str2 = str.Split(',');
            
            trackBar1.Value = int.Parse((double.Parse(str2[1])*100).ToString());
            this.Opacity = double.Parse(str2[1]);
            f2s = double.Parse(str2[1]);
            trackBar2.Maximum = 100;
            trackBar2.Value = int.Parse((double.Parse(str2[2]) * 100).ToString());
             
            f2xx = double.Parse(str2[2]);
            if (str2.Length  >4)
            {
                this.Top = int.Parse(str2[3]);
                this.Left = int.Parse(str2[4]);
            }
            string cpath = System.IO.Directory.GetCurrentDirectory()+@"\images\";
           
            string[] files = Directory.GetFiles(cpath, str2[0]+ "_Seed_"+"*.jpg", System.IO.SearchOption.AllDirectories);
           // pictureBox1.Image = Image.FromFile(files[0]);
            
            textBox1.Text =files.Length.ToString();
            if (files.Length >0)
            {

           
            for (int i = 0; i < files.Length; i++)
            {

           
                PictureBox pb = new PictureBox();
                pb.Image = Image.FromFile(files[i]);
                pb.Size = new Size(int.Parse((200*(1+f2xx)).ToString()), int.Parse((200 * (1 + f2xx)).ToString()));
                pb.SizeMode = PictureBoxSizeMode.Zoom;
                pb.Location = new Point(10+int.Parse((200 * (1 + f2xx)).ToString()) * i,-40);
                this.Controls.Add(pb);

            }
            this.Height = int.Parse((200 * (1 + f2xx)).ToString())-40;
            this.Width = files.Length * int.Parse((200 * (1 + f2xx)).ToString()) + 160;
            }
            label3.Text = f2s.ToString();
        }
         

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
             
            
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            closefather(f2s+","+ f2xx + ","+Top.ToString() + "," + Left.ToString());
        }
        public double f2s;
        public double f2xx=0;
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            f2s = double.Parse(trackBar1.Value.ToString()) /100;
            this.Opacity  =  f2s ;
            label3.Text = f2s.ToString();
        }
        private void f2xxdo()
        {
            foreach (var f in this.Controls)
            {
                if (f is PictureBox pb)
                {


                    pb.Size = new Size(int.Parse((200 * (1 + f2xx)).ToString()), int.Parse((200 * (1 + f2xx)).ToString()));
                    pb.SizeMode = PictureBoxSizeMode.Zoom;
                   // pb.Location = new Point(10 + int.Parse((200 * (1 + f2xx)).ToString()) * int.Parse(textBox1.Text), 0);
                }
            }
            this.Height = int.Parse((200 * (1 + f2xx)).ToString()) + 20;
            this.Width = int.Parse(textBox1.Text) * int.Parse((200 * (1 + f2xx)).ToString()) + 200;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            f2xx = double.Parse(trackBar2.Value.ToString()) / 100;
            label4.Text = f2xx.ToString();
            //f2xxdo();
        }

        private void setTag(Control cons)

        {

            //遍历窗体中的控件

            foreach (Control con in cons.Controls)

            {

                con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size + ":" + con.Name;

                if (con.Controls.Count > 0)

                    setTag(con);

            }

        }
        private void setControls(float newx, float newy, Control cons)

        {
            foreach (Control con in cons.Controls)

            {
                con.Visible = false;

            }
            //遍历窗体中的控件，重新设置控件的值

            foreach (Control con in cons.Controls)

            {

                string[] mytag = con.Tag.ToString().Split(new char[] { ':' });//获取控件的Tag属性值，并分割后存储字符串数组

                float a = Convert.ToSingle(mytag[0]) * newx;//根据窗体缩放比例确定控件的值，宽度

                con.Width = (int)a;//宽度

                a = Convert.ToSingle(mytag[1]) * newy;//高度

                con.Height = (int)(a);

                a = Convert.ToSingle(mytag[2]) * newx;//左边距离

                con.Left = (int)(a);

                a = Convert.ToSingle(mytag[3]) * newy;//上边缘距离

                con.Top = (int)(a);

                Single currentSize = Convert.ToSingle(mytag[4]) * newx;//字体大小

                con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);

                if (con.Controls.Count > 0)

                {

                    setControls(newx, newy, con);

                }

            }
            foreach (Control con in cons.Controls)

            {
                con.Visible = true;

            }
        }

        void modular_calEchoPhaseFromSignal1_Resize(object sender, EventArgs e)

        {

            float newx = (this.Width) / X; //窗体宽度缩放比例

            float newy = this.Height / Y;//窗体高度缩放比例

            setControls(newx, newy, this);//随窗体改变控件大小

            // this.Text = this.Width.ToString() + " " + this.Height.ToString();//窗体标题栏文本

        } 
    }
}
