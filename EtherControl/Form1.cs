using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EtherControl
{
    public partial class Form1 : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
     (
         int nLeftRect,     // x-coordinate of upper-left corner
         int nTopRect,      // y-coordinate of upper-left corner
         int nRightRect,    // x-coordinate of lower-right corner
         int nBottomRect,   // y-coordinate of lower-right corner
         int nWidthEllipse, // width of ellipse
         int nHeightEllipse // height of ellipse
     );


        private bool isAdapterEnabled;
        public Form1()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }
        private readonly Timer timer1 = new Timer();
        private bool drag = false;
        private Point start_point = new Point(0, 0);

        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            drag = true;
            start_point = new Point(e.X, e.Y);
        }

        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag)
            {

                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - start_point.X, p.Y - start_point.Y);
            }
        }

        private void Panel1_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            isAdapterEnabled = IsEthernetAdapterEnabled();
            rjToggleButton1.Checked = isAdapterEnabled;
            UpdateLabel();
            int monitorIndex = 1; // Index of the third monitor (zero-based)
            Screen screen = Screen.AllScreens[monitorIndex];
            int x = screen.WorkingArea.Left;
            int y = screen.WorkingArea.Top;
            Location = new Point(x, y);
        }

        private void UpdateSwitchAndLabel()
        {
            bool isAdapterEnabled = IsEthernetAdapterEnabled();
            rjToggleButton1.Checked = isAdapterEnabled;
            UpdateLabel();
        }

        private bool IsEthernetAdapterEnabled()
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                    adapter.OperationalStatus == OperationalStatus.Up)
                {
                    return true;
                }
            }
            return false;
        }

        private void UpdateLabel()
        {
            label1.Text = rjToggleButton1.Checked ? "Enabled" : "Disabled";
        }

        private void EnableOrDisableEthernetAdapter(bool enable)
        {
            // You can implement code to enable or disable the Ethernet adapter here.
            // Keep in mind that enabling/disabling network interfaces typically requires
            // administrative privileges, so you need to handle that accordingly.
        }

        private void Button1_Click(object sender, EventArgs e)
        {

            // Apply the changes when the Apply button is clicked
            if (rjToggleButton1.Checked == true)
            {
                try
                {

                    Process prfo = Process.Start(@"enable.bat");
                    int id = prfo.Id;
                    Process temp = Process.GetProcessById(id);
                    Visible = true;
                    temp.WaitForExit();
                    Visible = true;
                    UpdateLabel();

                }
                catch (Exception)
                {
                    MessageBox.Show("NOT ENABLED", "Error");
                }
            }
        }

        private void RjToggleButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (rjToggleButton1.Checked == false)
            {

                try
                {

                    Process prfo = Process.Start(@"disable.bat");
                    int id = prfo.Id;
                    Process temp = Process.GetProcessById(id);
                    Visible = false;
                    temp.WaitForExit();
                    Visible = true;
                    UpdateLabel();

                }
                catch (Exception)
                {
                    MessageBox.Show("NOT DISABLED", "Error");
                }
            }

            


             

            

        }
       
    }
}
