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
            // Инициализация NotifyIcon
            notifyIcon = new NotifyIcon
            {
                Icon = isOn ? SystemIcons.WinLogo : SystemIcons.Error, // Установите иконку, можете заменить на свою
                Visible = true,
                Text = "Включатель" // Текст подсказки
            };
            UpdateIcon(isOn);

            // Инициализация контекстного меню
            contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Развернуть", null, onRestore_Click);
            contextMenu.Items.Add("Закрыть", null, onExit_Click);
            contextMenu.Items.Add("Включить", null, btnOn_Click);
            contextMenu.Items.Add("Выключить", null, btnOff_Click);

            // Привязка контекстного меню к NotifyIcon
            notifyIcon.ContextMenuStrip = contextMenu;

            // Подписка на события
            notifyIcon.DoubleClick += onNotifyIcon_DoubleClick;
        }
        // Обработчик двойного клика на иконке
        private void onNotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            Restore(); // Разворачивает окно
        }

        // Обработчик клика "Развернуть" в контекстном меню
        private void onRestore_Click(object sender, EventArgs e)
        {
            Restore(); // Разворачивает окно
        }

        // Обработчик клика "Закрыть" в контекстном меню
        private void onExit_Click(object sender, EventArgs e)
        {
            notifyIcon.Visible = false; // Убираем значок из трея перед закрытием
            Application.Exit(); // Закрываем приложение
        }

        // Метод для восстановления окна
        private void Restore()
        {
            this.Show(); // Показываем окно
            this.WindowState = FormWindowState.Normal; // Устанавливаем нормальное состояние
            this.Activate(); // Активируем окно
        }

        // Обработка события "Свернуть" окна
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide(); // Скрываем окно, когда оно свернуто
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Предотвращаем закрытие приложения, если оно свёрнуто
            if (e.CloseReason == CloseReason.UserClosing && this.WindowState == FormWindowState.Minimized)
            {
                e.Cancel = true; // Отменяем событие закрытия
                this.Hide(); // Скрываем окно
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

            // Список для хранения названий окон
            //List<string> windowTitles = new List<string>();
            var cmdPtr = GetCmdPtr();
            this.lblCmdStatus.Text = cmdPtr == IntPtr.Zero ? "OFF" : "ON";
            return process != null && cmdPtr != IntPtr.Zero;
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

            // Список для хранения названий окон
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

            //var timer = new System.Windows.Forms.Timer(1000); // 1000 миллисекунд = 1 секунда
            //timer.Elapsed += Timer_Elapsed;
            //timer.AutoReset = false; // Устанавливаем так, чтобы таймер не перезапускался автоматически
            //timer.Enabled = true; // Запускаем таймер
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
