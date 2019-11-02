using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MS_Paint_Code_Writter
{
    public partial class Form1 : Form
    {

        private static readonly string _workdir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MSPaintCodeWritter");
        private static readonly string _confdir = Path.Combine(_workdir, "config");
        private static readonly string _savedConf = Path.Combine(_confdir, "configuration.txt");

        public MyConfig CurrentConfig { get; set; } = new MyConfig();
        private bool loadedConfig = false;

        public Form1()
        {
            InitializeComponent();
        }

        // Pinvoke declaration for ShowWindow
        private const int SW_SHOWMAXIMIZED = 3;
        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        //Keyboard actions
        private const int VK_CONTROL = 0x11;
        private const int VK_E = 0x45;


        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hwc, IntPtr hwp);
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        public void ClickLeft(uint x, uint y)
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, x, y, 0, 0);
        }
        public void ClickLeft(uint x, uint y, int times, int sleep = 70)
        {
            for (int i = 0; i < times; i++)
            {
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, x, y, 0, 0);
                Thread.Sleep(sleep);
            }
        }
        public void CtrlE()
        {
            mouse_event( | MOUSEEVENTF_LEFTUP, x, y, 0, 0);
            Thread.Sleep(sleep);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (richTextBox1.TextLength == 0)
            {
                MessageBox.Show("Please type some text to convert and try again");
                return;
            }
            button1.Enabled = false;
            try
            {
                Process p = Process.Start("mspaint.exe");
                Thread.Sleep(2000);
                p.WaitForInputIdle(1000);

                SetParent(p.MainWindowHandle, panel1.Handle);
                ShowWindow(p.MainWindowHandle, SW_SHOWMAXIMIZED);
                Thread.Sleep(1000); panel1.Focus();

                if (loadedConfig == false)
                {
                    CurrentConfig.zoomBtn = new MyCursor("move mouse to zoom icon and press enter", "ZoomButton");
                    ClickLeft(CurrentConfig.zoomBtn.X, CurrentConfig.zoomBtn.Y, 12);
                    CurrentConfig.penBtn = new MyCursor("move mouse to pen icon and press enter", "PenButton");
                    ClickLeft(CurrentConfig.penBtn.X, CurrentConfig.penBtn.Y);
                    CurrentConfig.colorPaleteBtn = new MyCursor("move mouse to custom color palete icon and press enter", "ColorPicker");
                    ClickLeft(CurrentConfig.colorPaleteBtn.X, CurrentConfig.colorPaleteBtn.Y, 3);
                    CurrentConfig.RText = new MyCursor("move mouse to red color input box and press enter", "Red color");
                    CurrentConfig.GText = new MyCursor("move mouse to green color input box and press enter", "Green color");
                    CurrentConfig.BText = new MyCursor("move mouse to blue color input box and press enter", "Blue color");
                    CurrentConfig.colorPaleteOKBtn = new MyCursor("move mouse to Ok icon and press enter", "OkButon");
                    ClickLeft(CurrentConfig.colorPaleteBtn.X, CurrentConfig.colorPaleteBtn.Y);
                }
                SetForegroundWindow(p.MainWindowHandle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            var coordinates = panel1.PointToClient(Cursor.Position);
            uint X = (uint)coordinates.X;
            uint Y = (uint)coordinates.Y;
            Console.WriteLine($"X: {X}, Y:{Y}");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Directory.CreateDirectory(_confdir);

            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (StreamWriter sw = new StreamWriter(_savedConf))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, CurrentConfig);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Directory.CreateDirectory(_confdir);
            if (File.Exists(_savedConf))
            {
                using (StreamReader streamReader = new StreamReader(_savedConf))
                {
                    string json = streamReader.ReadToEnd();
                    CurrentConfig = JsonConvert.DeserializeObject<MyConfig>(json);
                    loadedConfig = true;
                }
            }
        }
    }
    public class MyCursor
    {
        public uint X { get; }
        public uint Y { get; }
        public MyCursor()
        {
            //for serializatio puproses only
        }
        public MyCursor(string message, string log)
        {
            new MyPopUp(message).ShowDialog();
            X = (uint)Cursor.Position.X;
            Y = (uint)Cursor.Position.Y;
            if (string.IsNullOrEmpty(log) == false)
            {
                Console.WriteLine($"{log.PadLeft(20)} = X: {X}, Y:{Y}");
            }
        }
    }
    public class MyConfig
    {
        public MyCursor zoomBtn { get; set; }
        public MyCursor penBtn { get; set; }
        public MyCursor colorPaleteBtn { get; set; }
        public MyCursor RText { get; set; }
        public MyCursor GText { get; set; }
        public MyCursor BText { get; set; }
        public MyCursor colorPaleteOKBtn { get; set; }
    }
}
