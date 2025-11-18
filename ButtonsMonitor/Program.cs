using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;
using System.Runtime.InteropServices;

namespace ButtonsMonitor
{
    class Program
    {
        private const int CHECK_INTERVAL = 30000; // 30 секунд
        private static bool isMonitoring = true;

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

        [Flags]
        public enum MouseEventFlags : uint
        {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
        }

        static async Task Main(string[] args)
        {
            Console.WriteLine("Azure DevOps Monitor запущен");
            Console.WriteLine("Мониторинг экрана каждые 30 секунд...");
            Console.WriteLine("Для остановки нажмите Ctrl+C\n");

            // Обработка Ctrl+C
            Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true;
                isMonitoring = false;
                Console.WriteLine("\nОстановка монитора...");
            };

            while (isMonitoring)
            {
                try
                {
                    await CheckScreenAndClick();
                    await Task.Delay(CHECK_INTERVAL);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                    await Task.Delay(CHECK_INTERVAL);
                }
            }

            Console.WriteLine("Монитор остановлен");
        }

        static async Task CheckScreenAndClick()
        {
            Console.WriteLine($"{DateTime.Now:HH:mm:ss} - Проверка экрана...");
            Thread.Sleep(2000);
            // Создаем скриншот
            using (Bitmap screenshot = TakeScreenshot())
            {
                // Сохраняем скриншот для отладки (опционально)
                string debugPath = Path.Combine(Directory.GetCurrentDirectory(), "debug_screenshot.png");
                screenshot.Save(debugPath, System.Drawing.Imaging.ImageFormat.Png); // Явное указание namespace

                // Ищем текст BuildFE и красный кружок
                var buildFeLocation = await FindBuildFEWithRedCircle(screenshot);

                if (buildFeLocation.HasValue)
                {
                    Console.WriteLine($"Найден BuildFE с красным кружком {buildFeLocation}!");

                    // Ищем кнопку "Rerun failed jobs" рядом
                    var rerunButtonLocation = FindRerunButton(screenshot, buildFeLocation.Value);

                    if (rerunButtonLocation.HasValue)
                    {
                        Console.WriteLine($"Найдена кнопка Rerun failed jobs {rerunButtonLocation.Value} , кликаем...");
                        ClickAtPosition(rerunButtonLocation.Value);
                    }
                    else
                    {
                        Console.WriteLine("Кнопка Rerun failed jobs не найдена");
                    }
                }
                else
                {
                    Console.WriteLine("BuildFE с красным кружком не найден");
                }
            }
        }

        static Bitmap TakeScreenshot()
        {
            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            Bitmap screenshot = new Bitmap(bounds.Width, bounds.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using (Graphics graphics = Graphics.FromImage(screenshot))
            {
                graphics.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size, CopyPixelOperation.SourceCopy);
            }

            return screenshot;
        }

        static async Task<Point?> FindBuildFEWithRedCircle(Bitmap screenshot)
        {
            // Используем Tesseract для распознавания текста
            using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
            {
                // Конвертируем Bitmap в Pix для Tesseract
                using (var pix = BitmapToPix(screenshot))
                using (var page = engine.Process(pix))
                {
                    string text = page.GetText();
                    Console.WriteLine("Распознанный текст: " + (string.IsNullOrEmpty(text) ? "не распознан" : text.Substring(0, Math.Min(100, text.Length)) + "..."));

                    // Ищем текст BuildFE
                    if (text.Contains("Buildre", StringComparison.OrdinalIgnoreCase))
                    {
                        // Получаем расположение текста BuildFE
                        using (var iterator = page.GetIterator())
                        {
                            iterator.Begin();

                            do
                            {
                                string wordText = iterator.GetText(PageIteratorLevel.Word);
                                if (!string.IsNullOrEmpty(wordText) && wordText.Contains("Buildre", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (iterator.TryGetBoundingBox(PageIteratorLevel.Word, out var rect))
                                    {
                                        // Проверяем наличие красного кружка рядом с текстом
                                        if (true)
                                        {
                                            return new Point(rect.X1 + rect.Width / 2, rect.Y1 + rect.Height / 2);
                                        }
                                    }
                                }
                            } while (iterator.Next(PageIteratorLevel.Word));
                        }
                    }
                }
            }

            return null;
        }

        // Метод для конвертации Bitmap в Pix
        static Pix BitmapToPix(Bitmap bitmap)
        {
            using (var memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return Pix.LoadFromMemory(memoryStream.ToArray());
            }
        }

        static bool CheckForRedCircleNearby(Bitmap screenshot, Rect rect)
        {
            // Область поиска красного кружка (справа от текста)
            int searchRadius = 50;
            int startX = rect.X1 + rect.Width;
            int startY = Math.Max(0, rect.Y1 - 10);
            int endX = Math.Min(screenshot.Width, startX + searchRadius);
            int endY = Math.Min(screenshot.Height, rect.Y2 + 10);

            // Проверяем пиксели в области поиска
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    Color pixel = screenshot.GetPixel(x, y);

                    // Проверяем красный цвет (R > 200, G < 100, B < 100)
                    if (pixel.R > 200 && pixel.G < 100 && pixel.B < 100)
                    {
                        // Проверяем, что это не одиночный пиксель, а область
                        if (IsRedCircleArea(screenshot, x, y))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        static bool IsRedCircleArea(Bitmap screenshot, int centerX, int centerY)
        {
            int redPixels = 0;
            int checkRadius = 10;

            // Проверяем небольшую область вокруг найденного красного пикселя
            for (int x = centerX - checkRadius; x <= centerX + checkRadius; x++)
            {
                for (int y = centerY - checkRadius; y <= centerY + checkRadius; y++)
                {
                    if (x >= 0 && x < screenshot.Width && y >= 0 && y < screenshot.Height)
                    {
                        Color pixel = screenshot.GetPixel(x, y);
                        if (pixel.R > 200 && pixel.G < 100 && pixel.B < 100)
                        {
                            redPixels++;
                        }
                    }
                }
            }

            // Если найдено достаточно красных пикселей, считаем что это кружок
            return redPixels > 5;
        }

        static Point? FindRerunButton(Bitmap screenshot, Point buildFeLocation)
        {
            // Ищем текст "Rerun failed jobs" или похожий в области ниже BuildFE
            using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
            {
                // Обрезаем изображение для поиска кнопки (область ниже BuildFE)
                int searchWidth = 400;
                int searchHeight = 200;
                int startX = Math.Max(0, buildFeLocation.X - 200);
                int startY = buildFeLocation.Y + 20;

                if (startX + searchWidth > screenshot.Width) startX = screenshot.Width - searchWidth;
                if (startY + searchHeight > screenshot.Height) searchHeight = screenshot.Height - startY - 1;

                if (searchWidth <= 0 || searchHeight <= 0) return null;

                using (Bitmap searchArea = new Bitmap(searchWidth, searchHeight))
                using (Graphics g = Graphics.FromImage(searchArea))
                {
                    g.DrawImage(screenshot, new Rectangle(0, 0, searchWidth, searchHeight),
                               new Rectangle(startX, startY, searchWidth, searchHeight), GraphicsUnit.Pixel);

                    // Конвертируем в Pix для Tesseract
                    using (var pix = BitmapToPix(searchArea))
                    using (var page = engine.Process(pix))
                    {
                        string text = page.GetText();

                        if (text.Contains("Rerun", StringComparison.OrdinalIgnoreCase) ||
                            text.Contains("failed", StringComparison.OrdinalIgnoreCase) ||
                            text.Contains("jobs", StringComparison.OrdinalIgnoreCase))
                        {
                            // Находим центр кнопки
                            return new Point(startX + searchWidth / 2, startY + searchHeight / 2);
                        }
                    }
                }
            }

            return null;
        }

        static void ClickAtPosition(Point position)
        {
            // Устанавливаем позицию курсора
            SetCursorPos(position.X, position.Y);

            // Ждем немного перед кликом
            Thread.Sleep(100);

            // Выполняем клик левой кнопкой мыши
            mouse_event((uint)MouseEventFlags.LEFTDOWN, 0, 0, 0, 0);
            Thread.Sleep(50);
            mouse_event((uint)MouseEventFlags.LEFTUP, 0, 0, 0, 0);

            Console.WriteLine($"Клик выполнен в позиции ({position.X}, {position.Y})");
        }
    }
}