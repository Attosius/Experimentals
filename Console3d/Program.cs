namespace Console3d
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using static Console3d.ConsoleScreenBuffer;

    public class ConsoleScreenBuffer
    {
        // Import the CreateConsoleScreenBuffer function from kernel32.dll
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr CreateConsoleScreenBuffer(
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwFlags,
            IntPtr lpScreenBufferData
        );

        // Import the SetConsoleActiveScreenBuffer function from kernel32.dll
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool SetConsoleActiveScreenBuffer(
            IntPtr hConsoleOutput
        );

        // Import the CloseHandle function from kernel32.dll
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        // Import the WriteConsoleOutputCharacter function from kernel32.dll
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool WriteConsoleOutputCharacter(
            IntPtr hConsoleOutput,
            string lpCharacter,
        uint nLength,
            COORD dwWriteCoord,
            out uint lpNumberOfCharsWritten
        );

        // Structures for COORD
        [StructLayout(LayoutKind.Sequential)]
        public struct COORD
        {
            public short X;
            public short Y;
        }

        // Constants for dwDesiredAccess (access rights)
        const uint GENERIC_READ = 0x80000000;
        const uint GENERIC_WRITE = 0x40000000;

        // Constants for dwShareMode (share mode)
        const uint FILE_SHARE_READ = 0x00000001;
        const uint FILE_SHARE_WRITE = 0x00000002;

        // Constants for dwFlags (screen buffer flags)
        const uint CONSOLE_TEXTMODE_BUFFER = 1;

        static int nScreenWidth = 120; // Ширина консольного окна
        static int nScreenHeight = 40; // Высота консольного окна
    
        static float fPlayerX = 1.0f; // Координата игрока по оси X
        static float fPlayerY = 1.0f; // Координата игрока по оси Y
        static float fPlayerA = 0.0f; // Направление игрока
      
        static int nMapHeight = 16; // Высота игрового поля
        static int nMapWidth = 16;  // Ширина игрового поля
       
        static double fFOV = 3.14159 / 3; // Угол обзора (поле видимости)
        static float fDepth = 30.0f;     // Максимальная дистанция обзора

        public static void Main(string[] args)
        {
            // Create a new console screen buffer
            IntPtr hConsole = CreateConsoleScreenBuffer(
                GENERIC_READ | GENERIC_WRITE, // Desired access
                FILE_SHARE_READ | FILE_SHARE_WRITE, // Share mode
                IntPtr.Zero, // Security attributes (can be null)
                CONSOLE_TEXTMODE_BUFFER, // Screen buffer flags
                IntPtr.Zero // Screen buffer data (must be null)
            );

            // Create a new console screen buffer
            //IntPtr hConsole = CreateConsoleScreenBuffer(
            //    GENERIC_READ | GENERIC_WRITE, 0, IntPtr.Zero, CONSOLE_TEXTMODE_BUFFER, IntPtr.Zero // Буфер экрана
            //);

            if (hConsole == IntPtr.Zero)
            {
                Console.WriteLine("Failed to create console screen buffer. Error code: " + Marshal.GetLastWin32Error());
                return;
            }

            // Set the new screen buffer as the active one
            if (!SetConsoleActiveScreenBuffer(hConsole))
            {
                Console.WriteLine("Failed to set active screen buffer. Error code: " + Marshal.GetLastWin32Error());
                CloseHandle(hConsole);
                return;
            }
            Console.WriteLine("New console screen buffer created and activated!");

            // Write something to the new buffer using WriteConsoleOutputCharacter
            COORD coord = new COORD { X = 0, Y = 0 }; // Write at column 10, row 5
            string textToWrite = "Hello from WriteConsoleOutputCharacter!";
            uint charsWritten;

            //bool writeSuccess = WriteConsoleOutputCharacter(
            //    hConsole,
            //    textToWrite,
            //    (uint)textToWrite.Length,
            //    coord,
            //    out charsWritten
            //);

            //if (!writeSuccess)
            //{
            //    Console.WriteLine("Failed to write to console buffer. Error code: " + Marshal.GetLastWin32Error());
            //}
            //else
            //{
            //    Console.WriteLine($"Successfully wrote {charsWritten} characters to the console buffer.");
            //}

            var screen = new char[nScreenWidth * nScreenHeight + 1]; // Массив для записи в буфер
            uint dwBytesWritten = 0; // Для дебага
            screen[nScreenWidth * nScreenHeight] = '\0';
            //screen[0] = '1';
            //screen[1] = '2';
            //screen[2] = '3';


            //var data = new string(screen);
            //bool writeSuccess = WriteConsoleOutputCharacter(
            //    hConsole,
            //    data.ToString(),
            //    (uint)(nScreenWidth * nScreenHeight),
            //    coord,
            //    out dwBytesWritten
            //);

            //if (!writeSuccess)
            //{
            //    Console.WriteLine("Failed to write to console buffer. Error code: " + Marshal.GetLastWin32Error());
            //}
            //else
            //{
            //    Console.WriteLine($"Successfully wrote {writeSuccess} characters to the console buffer.");
            //}
            //Debug.WriteLine($"Successfully wrote {writeSuccess} characters to the console buffer.");




            string map = string.Empty; // Строковый массив
            map += "################";
            map += "#..............#";
            map += "#..............#";
            map += "#..............#";
            map += "#..............#";
            map += "#..............#";
            map += "#..............#";
            map += "#..............#";
            map += "#..............#";
            map += "#..............#";
            map += "#..............#";
            map += "#..............#";
            map += "#..............#";
            map += "#..............#";
            map += "#..............#";
            map += "################";

            //for (int nx = 0; nx < nMapWidth; nx++)
            //    for (int ny = 0; ny < nMapWidth; ny++)
            //    {
            //        screen[(ny + 1) * nScreenWidth + nx] = map[ny * nMapWidth + nx];
            //    }

            screen[((int)fPlayerX + 1) * nScreenWidth + (int)fPlayerY] = 'P';

            WriteConsoleOutputCharacter(
                hConsole,
                new string(screen),
                (uint)(nScreenWidth * nScreenHeight),
                coord,
                out dwBytesWritten
            );
            //while (true) // Игровой цикл
            {
                // Повторяющиеся действия
            }
            Console.ReadKey(); // Keep the window open

            // Clean up:  Revert to original buffer (optional but recommended)
            // You'd need to store the handle of the original buffer to revert.

            //CloseHandle(hConsole);  // Close the new buffer's handle when you're done.
            //Otherwise you get a resource leak
        }
    }

}
