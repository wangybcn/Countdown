using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace Countdown
{

    
    public partial class Form1 : Form
    {
        static public string mpath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        static public string maininiFile = mpath + "Countdown.ini";
        int cur_s = 0;
        int max_s = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var cgdtp = dTP.Value;
            int h = cgdtp.Hour;
            int m = cgdtp.Minute;
            int s = cgdtp.Second;
            max_s = h * 60 * 60 + m * 60 + s;
            cur_s = 0;
            timer1.Enabled = true;
            OperIni.INIWrite(maininiFile, "main", "SelectedIndex", CB_wave_OK.SelectedIndex);
            OperIni.INIWrite(maininiFile, "main", "h", dTP.Value.Hour);
            OperIni.INIWrite(maininiFile, "main", "m", dTP.Value.Minute);

            OperIni.INIWrite(maininiFile, "main", "s", dTP.Value.Second);


            OperIni.INIWrite(maininiFile, "main", "Left", Left);
            OperIni.INIWrite(maininiFile, "main", "Top", Top);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            cur_s++;
            int lost=(max_s-cur_s);
            string showtxt = "";
            if (lost / 60 / 60 == 0)
            {
                showtxt = string.Format("{0:00}", (lost / 60) % 60).ToString() + ":" + string.Format("{0:00}", (lost % 60 % 60)).ToString();
            }
            else
            {
                showtxt = (lost / 60 / 60).ToString() + ":" + string.Format("{0:00}", (lost / 60) % 60).ToString() + ":" + string.Format("{0:00}", (lost % 60 )).ToString();
            }

             
            label1.Text = "还有" + showtxt;
            Text = "" + showtxt;

           // notifyIcon1.Text = showtxt;
            notifyIcon1.ShowBalloonTip(2000, showtxt, showtxt, ToolTipIcon.Info );
            

            if (cur_s >= max_s) 
            {
                //timer1.Stop();
                timer1.Enabled = false;
                label1.Text = "END";
                Text = "倒计时";
               // notifyIcon1.Text = "倒计时";
                if(CB_wave_OK.SelectedIndex>-1)
                    playsound(mpath+"/"+CB_wave_OK.Text);
            }
        }
        [DllImport("winmm.dll", EntryPoint = "sndPlaySoundA", CallingConvention = CallingConvention.Cdecl)]
        private static extern long sndPlaySoundA(string lpszSoundName, long uFlags);
        public  void playsound(String filename)
        {
            try
            {
                if (File.Exists(filename))
                {
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer();
                    player.SoundLocation = filename;// "音乐文件名";
                    player.Load();
                    player.Play();
                    //sndPlaySoundA(filename, (long)0x0001);
                }
            }
            catch (Exception ex)
            {
               // Form_main.log.Error("sndPlaySound 错误:" + ex.Message.ToString());
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //MaximizeBox = false;
            DirectoryInfo di = new DirectoryInfo(mpath);
            foreach (FileInfo fi in di.GetFiles("*.wav"))
            {
                CB_wave_OK.Items.Add(fi.Name);
            }
            if (CB_wave_OK.Items.Count > 0)
            {
                CB_wave_OK.SelectedIndex = OperIni.INIRead(maininiFile, "main", "SelectedIndex", 0);
            }

           // imgpath = OperIni.INIRead(maininiFile, "main", "SelectedIndex", 0);
           // file_fname = OperIni.INIRead(maininiFile, "main", "file_fname", "img");
           //dTP.Value.Hour
            //dTP.Value.Year = 2026;
           // int h = OperIni.INIRead(maininiFile, "main", "h", 0);

            DateTime dt3 = new DateTime(2015, 12, 31, OperIni.INIRead(maininiFile, "main", "h", 0), OperIni.INIRead(maininiFile, "main", "m", 25), OperIni.INIRead(maininiFile, "main", "s", 0));
            dTP.Value = dt3;
            //dTP.Value.AddHours(OperIni.INIRead(maininiFile, "main", "h", 0));
            //dTP.Value.AddMinutes(OperIni.INIRead(maininiFile, "main", "m", 0));

            Left = OperIni.INIRead(maininiFile, "main", "Left", Left);
            Top = OperIni.INIRead(maininiFile, "main", "Top", Top);


        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            OperIni.INIWrite(maininiFile, "main", "Left", Left);
            OperIni.INIWrite(maininiFile, "main", "Top", Top);

           //窗口销毁时，获取控件属性可能不正确。
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();                                //窗体显示
            this.WindowState = FormWindowState.Normal;  //窗体状态默认大小
            this.Activate(); 
        }
    }
}
