using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Security.Principal;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS
{
    static class Program
    {
        public static Timer IdleTimer = new Timer();
        static MDIParent main = null;
        static int MinuteMicroseconds = 0;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            //try
            //{
                if (new Utility.DBService().AllowToStart && !new Utility.DBService().Running && !IsRunningAsAdministrator())
                {
                    ProcessStartInfo processStartInfo = new ProcessStartInfo(Assembly.GetEntryAssembly().CodeBase);
                    processStartInfo.UseShellExecute = true;
                    processStartInfo.Verb = "runas";
                    Process.Start(processStartInfo);
                    Application.Exit();
                }
                else
                {
                    MinuteMicroseconds = SettingController.IdleTime * 60000;
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    if (SettingController.DetectIdle)
                    {

                        LeaveIdleMessageFilter limf = new LeaveIdleMessageFilter();
                        Application.AddMessageFilter(limf);
                        Application.Idle += new EventHandler(Application_Idle);
                        IdleTimer.Interval = MinuteMicroseconds;    // One minute; change as needed
                        IdleTimer.Tick += TimeDone;
                        IdleTimer.Start();
                    }

                    main = new MDIParent();
                    Application.Run(main);
                    Application.Idle -= new EventHandler(Application_Idle);
                }
            //}
            //catch (Exception ex)
            //{
                
            //    MessageBox.Show(ex.Message);
            //}

            
        }
        #region IdleDetection
        static private void Application_Idle(Object sender, EventArgs e)
        {
            if (!IdleTimer.Enabled)     // not yet idling?
                IdleTimer.Start();
        }

        static private void TimeDone(object sender, EventArgs e)
        {
            IdleTimer.Stop();   // not really necessary
            string min = SettingController.IdleTime <= 1 ? " minute" : " minutes";
            MessageBox.Show("Auto logoff after "+SettingController.IdleTime.ToString()+ min + " Idle.","mPOS-Idle Detection",MessageBoxButtons.OK,MessageBoxIcon.Information);
            List<Form> openForms = new List<Form>();
            foreach (Form of in Application.OpenForms)
            {
                openForms.Add(of);
            }
            foreach (Form of in openForms.ToList())
            {
                if (of.Name != "MDIParent" && of.Name != "Login")
                {
                    of.Close();
                }
            }
            bool isAlreadyHave = false;

            foreach (Form child in main.MdiChildren)
            {
                if (child.Text == "LogIn")
                {
                    child.Activate();
                    isAlreadyHave = true;
                }
                else
                {
                    child.Close();
                }
            }
            MemberShip.UserId = 0;
            MemberShip.UserName = "";
            MemberShip.UserRole = null;

            if (!isAlreadyHave)
            {
                Login f = new Login();
                f.MdiParent = main;
                f.WindowState = FormWindowState.Maximized;
                f.Show();

                //DisableControls();
            }

            main.menuStrip.Enabled = false;
            main.toolStripStatusLabel.Text = string.Empty;
            main.toolStripStatusLabel1.Text = string.Empty;
            main.tSSBOOrPOS.Text = string.Empty;
            main.statusToolStripMenuItem.Text = string.Empty;
            main.logOutToolStripMenuItem.Visible = false;
            main.logInToolStripMenuItem1.Visible = true;
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public class LeaveIdleMessageFilter : IMessageFilter
        {
            const int WM_NCLBUTTONDOWN = 0x00A1;
            const int WM_NCLBUTTONUP = 0x00A2;
            const int WM_NCRBUTTONDOWN = 0x00A4;
            const int WM_NCRBUTTONUP = 0x00A5;
            const int WM_NCMBUTTONDOWN = 0x00A7;
            const int WM_NCMBUTTONUP = 0x00A8;
            const int WM_NCXBUTTONDOWN = 0x00AB;
            const int WM_NCXBUTTONUP = 0x00AC;
            const int WM_KEYDOWN = 0x0100;
            const int WM_KEYUP = 0x0101;
            const int WM_MOUSEMOVE = 0x0200;
            const int WM_LBUTTONDOWN = 0x0201;
            const int WM_LBUTTONUP = 0x0202;
            const int WM_RBUTTONDOWN = 0x0204;
            const int WM_RBUTTONUP = 0x0205;
            const int WM_MBUTTONDOWN = 0x0207;
            const int WM_MBUTTONUP = 0x0208;
            const int WM_XBUTTONDOWN = 0x020B;
            const int WM_XBUTTONUP = 0x020C;

            // The Messages array must be sorted due to use of Array.BinarySearch
            static int[] Messages = new int[] {WM_NCLBUTTONDOWN,
            WM_NCLBUTTONUP, WM_NCRBUTTONDOWN, WM_NCRBUTTONUP, WM_NCMBUTTONDOWN,
            WM_NCMBUTTONUP, WM_NCXBUTTONDOWN, WM_NCXBUTTONUP, WM_KEYDOWN, WM_KEYUP,
            WM_LBUTTONDOWN, WM_LBUTTONUP, WM_RBUTTONDOWN, WM_RBUTTONUP,
            WM_MBUTTONDOWN, WM_MBUTTONUP, WM_XBUTTONDOWN, WM_XBUTTONUP};

            public bool PreFilterMessage(ref Message m)
            {
                if (m.Msg == WM_MOUSEMOVE)  // mouse move is high volume
                    return false;
                if (!Program.IdleTimer.Enabled)     // idling?
                    return false;           // No
                if (Array.BinarySearch(Messages, m.Msg) >= 0)
                    Program.IdleTimer.Stop();
                return false;
            }
        }
        #endregion
        public static bool IsRunningAsAdministrator()
        {
            WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();
            WindowsPrincipal windowsPrincipal = new WindowsPrincipal(windowsIdentity);
            return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
    
}
