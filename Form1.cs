using Newtonsoft.Json;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace RLM
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            
        }
        public delegate void childclose(string str);
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 10)
            {


                path = textBox2.Text;
                if (timer1.Enabled)
                {
                    timer1.Enabled = false;
                    textBox2.ReadOnly = false;
                    button1.Text = "停止";

                }
                else
                {
                    timer1.Enabled = true;
                    textBox2.ReadOnly = true;
                    button1.Text = "启动";
                }
            
             }
            else
            {
                MessageBox.Show("输入 client.txt地址 ");
            }
             

    }
        public void closethis(string str)
        {
            textBox1.Text = str;
            
            string[] str2 = str.Split(',');
            f2s =double.Parse(str2[0]);
            FTL = str2[2]+","+ str2[3];

            trackBar1.Value = int.Parse((double.Parse(str2[0]) * 100).ToString());
            f2xx = double.Parse(str2[1]);
            trackBar2.Value = int.Parse((double.Parse(str2[1]) * 100).ToString());
        }
        public Form2 f2;
        public Form3 f3;
        public string LastLine, path;
        public string mapcode,FTL;
        public double f2s;
        public double f2xx =0;
        private void getint(object opath)
        {
            string path = opath.ToString();
            //while (true)
            //{
                var reg = new Regex(@"(Generating level)(\s[0-9]{0,}\sarea\s"")([0-9_]+)(""\swith seed\s)(.*)");
                var fstream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                string str = GetLastLine(fstream);
                if (str != null && str!= LastLine)
                {
                      
                    LastLine = str;
                    Match _match = reg.Match(str);

                if (reg.Match(str).Success)
                {
                    string s1 = reg.Match(str).Result("$3");
                    if (mapcode != s1)
                    {
                        mapcode = s1;
                       
                        if (f2 != null)
                        {
                           f2.Close();
                        }
                        f2 = new Form2(s1+","+f2s+"," + f2xx + "," + FTL); // 开一个子窗口
                        textBox3.Text = s1;
                        f2.closefather += new childclose(closethis);
                        f2.Show(); // 用show可以显示无模式窗口
                    }
                }
                     

                }
               
           // }
           
        }
        private void getstr(object opath)
        {
            string path = opath.ToString();
            
            var reg = new Regex(@"(.*)(@来自 )(.*)( Hi, I'd like to buy your )([0-9]+)(.*)( for my )(.*)( in )(.*)");
            var reg2 = new Regex(@"(.*)(@来自 )(.*)(Hi, I would like to buy your )(.*)( listed for )(.*)( in )(.*)");
            var fstream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            string str = GetLastLine(fstream);
            if (str != null && str != LastLine)
            { 
                LastLine = str;
                Match _match = reg.Match(str);
                Match _match2 = reg2.Match(str);

                if (reg.Match(str).Success)
                {
                    string s1 = reg.Match(str).Result("$6");
                    string s4 = reg.Match(str).Result("$5");

                    
                    DataRow[] dr = ds.DataTable1.Select("text='" + s1.Trim() + "'"); 
                    if (dr.Length == 1)
                    {

                        string s2 = dr[0]["cntext"].ToString();
                        string s3 = dr[0]["image"].ToString();
                        f3 = new Form3(s1 + "," + s2 + "," + s3 + "," + s4); // 开一个子窗口


                        f3.Show(); // 用show可以显示无模式窗口

                    }
                   
                     
                    
                }
                if (reg2.Match(str).Success)
                {
                    string s1 = reg2.Match(str).Result("$5");
                    string s4 = "1";
                    DataRow[] dr = ds.DataTable1.Select("text='" + s1.Trim() + "'");
                    if (dr.Length == 1)
                    {

                        string s2 = dr[0]["cntext"].ToString();
                        string s3 = dr[0]["image"].ToString();
                        f3 = new Form3(s1 + "," + s2 + "," + s3 + "," + s4); // 开一个子窗口


                        f3.Show(); // 用show可以显示无模式窗口

                    }



                }


            }
             

        }
        private string GetLastLine(FileStream fs)
        {
            int seekLength = (int)(fs.Length < 1024 ? fs.Length : 1024);  // 这里需要根据自己的数据长度进行调整，也可写成动态获取，可自己实现
            byte[] buffer = new byte[seekLength];
            fs.Seek(-buffer.Length, SeekOrigin.End);
            fs.Read(buffer, 0, buffer.Length);
            string multLine = System.Text.Encoding.UTF8.GetString(buffer);
            string[] lines = multLine.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            string line = lines[lines.Length - 1];

            return line;
        }
      
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            f2s =double.Parse(trackBar1.Value.ToString()) /100 ;
            textBox1.Text = f2s.ToString();
        }

        

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            f2xx = double.Parse(trackBar2.Value.ToString()) / 100;
            textBox1.Text = f2xx.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (textBox2.Text.Length > 10)
            {


                path = textBox2.Text;
                if (timer2.Enabled)
                {
                    timer2.Enabled = false;
                    textBox2.ReadOnly = false;
                    button2.Text = "启动";
                }
                else
                {
                    timer2.Enabled = true;
                    textBox2.ReadOnly = true;
                    button2.Text = "停止";
                }
               

            }
            else
            {
                MessageBox.Show("输入 client.txt地址 ");
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            getstr(path);
        }
        DataSet1 ds = new DataSet1();
        private void Form1_Load(object sender, EventArgs e)
        {
            trackBar1.Maximum = 100;
            trackBar1.Value = 50;
            trackBar2.Maximum = 100;
            trackBar2.Value = 0;
            f2s = double.Parse(trackBar1.Value.ToString()) / 100;
            f2xx = double.Parse(trackBar2.Value.ToString()) / 100;

          textBox2.Text = @"D:\Games\Path of Exile\logs\Client.txt";
            string staticpath = System.IO.Directory.GetCurrentDirectory() + @"\static\";
            string file = File.ReadAllText(staticpath+ "enstatic"); 

            JsonSerializerSettings jsSetting = new JsonSerializerSettings();

            jsSetting.NullValueHandling = NullValueHandling.Ignore;
            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(file);
            foreach (var item in myDeserializedClass.result)
            {


                foreach (var item2 in item.entries)
                {
                    if (item2.image != null)
                    {
                        ds.DataTable1.Rows.Add(new object[] { item2.id.ToString(), item2.image.ToString(), item2.text.ToString() });
                    }
                    else
                    {
                        ds.DataTable1.Rows.Add(new object[] { item2.id.ToString(), "", item2.text.ToString() });
                    }
                      
                } 
            }
            
            file = File.ReadAllText(staticpath + "cnstatic");

            
            Root myDeserializedClass2 = JsonConvert.DeserializeObject<Root>(file);
            foreach (var item in myDeserializedClass2.result)
            {

                foreach (var item2 in item.entries)
                {
                    DataRow[] dr = ds.DataTable1.Select("id='" + item2.id + "'");
                    if (dr.Length ==1)
                    {
                        dr[0]["cntext"] = item2.text;

                    }


                }
            } 
        }

         
        private void timer1_Tick(object sender, EventArgs e)
        {
            getint(path);
        }

        public class Entries
        {
            /// <summary>
            /// alt
            /// </summary>
            public string id { get; set; }
            /// <summary>
            /// Orb of Alteration
            /// </summary>
            public string text { get; set; }
            /// <summary>
            /// /gen/image/WzI1LDE0LHsiZiI6IjJESXRlbXMvQ3VycmVuY3kvQ3VycmVuY3lSZXJvbGxNYWdpYyIsInNjYWxlIjoxfV0/6308fc8ca2/CurrencyRerollMagic.png
            /// </summary>
            public string image { get; set; }
        }

        public class Result
        {
            /// <summary>
            /// Currency
            /// </summary>
            public string id { get; set; }
            /// <summary>
            /// Currency
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// Entries
            /// </summary>
            public List<Entries> entries { get; set; }
        }

        public class Root
        {
            /// <summary>
            /// Result
            /// </summary>
            public List<Result> result { get; set; }
        }

    }
}