using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
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
        public Form1()
        {
            InitializeComponent();
            this.Location = new Point(Screen.PrimaryScreen.Bounds.X, //should be (0,0)
                             Screen.PrimaryScreen.Bounds.Y);
            this.TopMost = true;
            this.StartPosition = FormStartPosition.Manual;
        }

        // Pinvoke declaration for ShowWindow
        private const int SW_SHOWMAXIMIZED = 3;
        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hwc, IntPtr hwp);
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        public void GetPosition()
        {
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            Console.WriteLine($"X: {X}, Y:{Y}");
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Process p = Process.Start("mspaint.exe");
                Thread.Sleep(2000);
                p.WaitForInputIdle(1000);

                SetParent(p.MainWindowHandle, panel1.Handle);
                ShowWindow(p.MainWindowHandle, SW_SHOWMAXIMIZED);
                Thread.Sleep(1000);
                panel1.Focus();

                //Click on zoom
                MyCursor zoomBtn = new MyCursor("move mouse to zoom icon and press enter");
                Console.WriteLine($"ZoomBtn = X: {zoomBtn.X}, Y:{zoomBtn.Y}");



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
    }
    public class MyCursor
    {
        public uint X { get; }
        public uint Y { get; }
        public MyCursor(string message)
        {
            MessageBox.Show(message);
            X = (uint)Cursor.Position.X;
            Y = (uint)Cursor.Position.Y;
        }
      
    }
}
