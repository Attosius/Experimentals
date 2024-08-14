using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ProgrammsSwitcher
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        // ������� ��� ��������� ������
        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        // ����������� ����������� ������� �� user32.dll
        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        // ��������� ��� ���������
        private const uint WM_CLOSE = 0x0010;
        private NotifyIcon notifyIcon;
        private ContextMenuStrip contextMenu;

        public Form1()
        {
            InitializeComponent();
            InitializeTrayIcon();
            Refresh();
        }
        private void InitializeTrayIcon()
        {
            var isOn = Refresh();
            // ������������� NotifyIcon
            notifyIcon = new NotifyIcon
            {
                Icon = isOn ? SystemIcons.WinLogo : SystemIcons.Error, // ���������� ������, ������ �������� �� ����
                Visible = true,
                Text = "����������" // ����� ���������
            };
            UpdateIcon(isOn);

            // ������������� ������������ ����
            contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("����������", null, onRestore_Click);
            contextMenu.Items.Add("�������", null, onExit_Click);
            contextMenu.Items.Add("��������", null, btnOn_Click);
            contextMenu.Items.Add("���������", null, btnOff_Click);

            // �������� ������������ ���� � NotifyIcon
            notifyIcon.ContextMenuStrip = contextMenu;

            // �������� �� �������
            notifyIcon.DoubleClick += onNotifyIcon_DoubleClick;
        }
        // ���������� �������� ����� �� ������
        private void onNotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            Restore(); // ������������� ����
        }

        // ���������� ����� "����������" � ����������� ����
        private void onRestore_Click(object sender, EventArgs e)
        {
            Restore(); // ������������� ����
        }

        // ���������� ����� "�������" � ����������� ����
        private void onExit_Click(object sender, EventArgs e)
        {
            notifyIcon.Visible = false; // ������� ������ �� ���� ����� ���������
            Application.Exit(); // ��������� ����������
        }

        // ����� ��� �������������� ����
        private void Restore()
        {
            this.Show(); // ���������� ����
            this.WindowState = FormWindowState.Normal; // ������������� ���������� ���������
            this.Activate(); // ���������� ����
        }

        // ��������� ������� "��������" ����
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide(); // �������� ����, ����� ��� ��������
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // ������������� �������� ����������, ���� ��� �������
            if (e.CloseReason == CloseReason.UserClosing && this.WindowState == FormWindowState.Minimized)
            {
                e.Cancel = true; // �������� ������� ��������
                this.Hide(); // �������� ����
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Refresh();
        }

        private bool Refresh()
        {
            Process? process = GetWinProcess();
            this.lblWinStatus.Text = process == null ? "OFF" : "ON";
            //process.Close();

            // ������ ��� �������� �������� ����
            //List<string> windowTitles = new List<string>();
            var cmdPtr = GetCmdPtr();
            this.lblCmdStatus.Text = cmdPtr == IntPtr.Zero ? "OFF" : "ON";
            return process != null && cmdPtr != IntPtr.Zero;
        }

        private static IntPtr GetCmdPtr()
        {
            IntPtr cmdProcessPtr = IntPtr.Zero;
            // �������� EnumWindows � ����� �������� �������
            EnumWindows((hWnd, lParam) =>
            {
                // �������� ����� �������� ����
                int length = GetWindowTextLength(hWnd);
                if (length == 0)
                {
                    return true; // ������� � ���������� ����
                }

                var windowText = new StringBuilder(length + 1);
                // ������� StringBuilder ��� ��������� ��������
                GetWindowText(hWnd, windowText, windowText.Capacity);
                if (windowText.ToString().Contains("goodbyedpi"))
                {
                    cmdProcessPtr = hWnd;
                    return false;
                    //IntPtr hWnd2 = FindWindow(null, windowText.ToString());

                    //// ���������, ������� �� ����
                    //if (hWnd2 != IntPtr.Zero)
                    //{
                    //    // ���������� ��������� � ��������
                    //    bool result2 = PostMessage(hWnd2, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                    //}
                    // �������� �� hWnd ����, ������� �� ������ �������
                    //IntPtr hWndToClose = new IntPtr(hWnd);

                    // ���������� PostMessage ��� �������� ��������� ��������
                    //bool result = PostMessage(hWndToClose, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                    //IntPtr result = SendMessage(hWndToClose, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                }
                // ��������� �������� � ������
                return true; // ������� � ���������� ����
            }, IntPtr.Zero);
            return cmdProcessPtr;
        }

        private static Process? GetWinProcess()
        {
            return Process.GetProcesses().FirstOrDefault(o => o.ProcessName == "goodbyedpi");
        }

        private async void btnOn_Click(object sender, EventArgs e)
        {
            string appPath = "e:\\Files+\\goodbyedpi-0.2.3rc1-2\\goodbyedpi-0.2.3rc1\\x86_64\\goodbyedpi.exe"; // ��������, �������

            // �������������� ����� �������
            Process process = new Process();

            // ������������� ��������� �������
            process.StartInfo.FileName = appPath;

            // ��������� ����� ����
            process.StartInfo.UseShellExecute = true;

            try
            {
                // ��������� �������
                process.Start();
                Console.WriteLine($"�������� ����������: {appPath}");
            }
            catch (Exception ex)
            {
                // ������������ ����������, ���� ������ �� ������
                Console.WriteLine($"�� ������� ��������� ����������: {ex.Message}");
            }
            // �������� ���������� ��������
            await Task.Delay(500); // �������� � 5 ������
            var isOn = Refresh();
            UpdateIcon(isOn);
        }

        private async void btnOff_Click(object sender, EventArgs e)
        {
            Process? process = GetWinProcess();
            if (process != null)
            {
                process.Close();
                process = GetWinProcess();
                this.lblWinStatus.Text = process == null ? "Closed" : "Tring to closing";
            }
            else
            {
                this.lblWinStatus.Text = "Not found";
            }

            // ������ ��� �������� �������� ����
            //List<string> windowTitles = new List<string>();
            var cmdPtr = GetCmdPtr();
            if (cmdPtr != IntPtr.Zero)
            {
                bool result = PostMessage(cmdPtr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                if (result)
                {
                    this.lblCmdStatus.Text = "Closed";
                }
                else
                {
                    this.lblCmdStatus.Text = "Tring to closing";
                }
            }
            await Task.Delay(500);
            var isOn = Refresh();
            UpdateIcon(isOn);

            //var timer = new System.Windows.Forms.Timer(1000); // 1000 ����������� = 1 �������
            //timer.Elapsed += Timer_Elapsed;
            //timer.AutoReset = false; // ������������� ���, ����� ������ �� �������������� �������������
            //timer.Enabled = true; // ��������� ������
        }

        private void UpdateIcon(bool isOn)
        {
            notifyIcon.Icon = isOn ? SystemIcons.Information : SystemIcons.Error;
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            Refresh();
        }
    }
}
