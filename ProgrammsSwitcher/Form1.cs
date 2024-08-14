using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

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

        // Делегат для обратного вызова
        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        // Импортируем необходимые функции из user32.dll
        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        // Константы для сообщений
        private const uint WM_CLOSE = 0x0010;

        public Form1()
        {
            InitializeComponent();
            Refresh();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Refresh();
        }

        private void Refresh()
        {
            Process? process = GetWinProcess();
            this.lblWinStatus.Text = process == null ? "OFF" : "ON";
            //process.Close();

            // Список для хранения названий окон
            //List<string> windowTitles = new List<string>();
            var cmdPtr = GetCmdPtr();
            this.lblCmdStatus.Text = cmdPtr == IntPtr.Zero ? "OFF" : "ON";
        }

        private static IntPtr GetCmdPtr()
        {
            IntPtr cmdProcessPtr = IntPtr.Zero;
            // Вызываем EnumWindows с нашим обратным вызовом
            EnumWindows((hWnd, lParam) =>
            {
                // Получаем длину названия окна
                int length = GetWindowTextLength(hWnd);
                if (length == 0)
                {
                    return true; // Перейти к следующему окну
                }

                var windowText = new StringBuilder(length + 1);
                // Создаем StringBuilder для получения названия
                GetWindowText(hWnd, windowText, windowText.Capacity);
                if (windowText.ToString().Contains("goodbyedpi"))
                {
                    cmdProcessPtr = hWnd;
                    return false;
                    //IntPtr hWnd2 = FindWindow(null, windowText.ToString());

                    //// Проверяем, найдено ли окно
                    //if (hWnd2 != IntPtr.Zero)
                    //{
                    //    // Отправляем сообщение о закрытии
                    //    bool result2 = PostMessage(hWnd2, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                    //}
                    // Замените на hWnd окна, которое вы хотите закрыть
                    //IntPtr hWndToClose = new IntPtr(hWnd);

                    // Используем PostMessage для отправки сообщения закрытия
                    //bool result = PostMessage(hWndToClose, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                    //IntPtr result = SendMessage(hWndToClose, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                }
                // Добавляем название в список
                return true; // Перейти к следующему окну
            }, IntPtr.Zero);
            return cmdProcessPtr;
        }

        private static Process? GetWinProcess()
        {
            return Process.GetProcesses().FirstOrDefault(o => o.ProcessName == "goodbyedpi");
        }

        private async void btnOn_Click(object sender, EventArgs e)
        {
            string appPath = "e:\\Files+\\goodbyedpi-0.2.3rc1-2\\goodbyedpi-0.2.3rc1\\x86_64\\goodbyedpi.exe"; // Например, Блокнот

            // Инициализируем новый процесс
            Process process = new Process();

            // Устанавливаем параметры запуска
            process.StartInfo.FileName = appPath;

            // Открываем новое окно
            process.StartInfo.UseShellExecute = true;

            try
            {
                // Запускаем процесс
                process.Start();
                Console.WriteLine($"Запущено приложение: {appPath}");
            }
            catch (Exception ex)
            {
                // Обрабатываем исключения, если запуск не удался
                Console.WriteLine($"Не удалось запустить приложение: {ex.Message}");
            }
            // Имитация длительной операции
            await Task.Delay(500); // Задержка в 5 секунд
            Refresh();
        }

        private async void btnOff_Click(object sender, EventArgs e)
        {
            Process? process = GetWinProcess();
            if (process != null)
            {
                process.Close();
                process = GetWinProcess();
                this.lblWinStatus.Text = process == null ? "Closed" : "Tring to closing";
            }else
            {
                this.lblWinStatus.Text = "Not found";
            }

            // Список для хранения названий окон
            //List<string> windowTitles = new List<string>();
            var cmdPtr = GetCmdPtr();
            if (cmdPtr != IntPtr.Zero)
            {
                bool result = PostMessage(cmdPtr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                if (result)
                {
                    this.lblCmdStatus.Text = "Closed";
                }else
                {
                    this.lblCmdStatus.Text = "Tring to closing";
                }
            }
            await Task.Delay(500);
            Refresh();

            //var timer = new System.Windows.Forms.Timer(1000); // 1000 миллисекунд = 1 секунда
            //timer.Elapsed += Timer_Elapsed;
            //timer.AutoReset = false; // Устанавливаем так, чтобы таймер не перезапускался автоматически
            //timer.Enabled = true; // Запускаем таймер
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            Refresh();
        }
    }
}
