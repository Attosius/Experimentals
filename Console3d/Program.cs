namespace Console3d
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    public class Program
    {
        static int nScreenWidth = 120; // Ширина консольного окна X
        static int nScreenHeight = 40; // Высота консольного окна Y
    
        static double dPlayerX = 2.5d; // Координата игрока по оси X
        static double dPlayerY = 3.0d; // Координата игрока по оси Y
        static double dPlayerA = 0.0d; // Направление игрока

        static double dirX = 5.0d; // Координата по оси X
        static double dirY = 7.0d; // Координата по оси Y


        static int nMapHeight = 16; // Высота игрового поля
        static int nMapWidth = 16;  // Ширина игрового поля
       
        static double dFOV = 3.14159 / 3;   // Угол обзора (поле видимости)
        static float fDepth = 30.0f;        // Максимальная дистанция обзора
        static double dTurn = 18d;		    // Walking Speed
        static double dSpeed = 30d;		    // Walking Speed

        public static void Main(string[] args)
        {
            CreateConsole(out nint hConsole);

            var coord = new COORD { X = 0, Y = 0 }; // Write at column 0, row 0

            var screen = new char[nScreenWidth * nScreenHeight + 1]; // Массив для записи в буфер
            screen[nScreenWidth * nScreenHeight] = '\0';


            string map = string.Empty; // Строковый массив
            map += "################"; // 1
            map += "#..............#"; // 2
            map += "#........#.....#"; // 3
            map += "#........#.....#"; // 4
            map += "#........###...#"; // 5
            map += "#..............#"; // 6
            map += "#..#...........#"; // 7
            map += "#..............#";
            map += "#............#.#";
            map += "#............#.#";
            map += "#............#.#";
            map += "#..##........#.#";
            map += "#..##........#.#";
            map += "#..........###.#";
            map += "#..............#";
            map += "################";


            WriteInConsole(hConsole, coord, screen, map);
            var sw = Stopwatch.StartNew();
            while (true) // Игровой цикл
            {
                Run(hConsole, coord, screen, map, sw);
            }

        }

        private static void Run(nint hConsole, COORD coord, char[] screen, string map, Stopwatch sw)
        {
            var elapsed = sw.ElapsedMilliseconds / (double)1000;
            sw.Restart();

            if ((GetAsyncKeyState('A') & 0x8000) != 0)
            {
                dPlayerA -= dTurn * elapsed;
            }
            if ((GetAsyncKeyState('D') & 0x8000) != 0)
            {
                dPlayerA += dTurn * elapsed;
            }

            dirX = dPlayerX + Math.Sin(dPlayerA) * 2;
            dirY = dPlayerY + Math.Cos(dPlayerA) * 2;

            //Debug.WriteLine($" fPlayerA: {dPlayerA:0.000}, sin: {Math.Sin(dPlayerA):0.000}, cos: {Math.Cos(dPlayerA):0.000}");
            //Debug.WriteLine($" X {dPlayerX:0.000} => {Math.Sin(dPlayerA) * 5f * elapsed:0.000}");
            //Debug.WriteLine($" Y {dPlayerY:0.000} => {Math.Cos(dPlayerA) * 5f * elapsed:0.000}");
            //Debug.WriteLine($" DirX {dirX:0.000}");
            //Debug.WriteLine($" DirY {dirY:0.000}");
            //Debug.WriteLine($"-------------------");

            if ((GetAsyncKeyState('W') & 0x8000) != 0)
            {
                dPlayerX += Math.Sin(dPlayerA) * dSpeed * elapsed;
                dPlayerY += Math.Cos(dPlayerA) * dSpeed * elapsed;

                int indexInMap = ((int)dPlayerY * nMapWidth) + (int)(dPlayerX);
                if (map.Length > indexInMap && map[indexInMap] == '#')
                {
                    Debug.WriteLine($"Stop");
                    dPlayerX -= Math.Sin(dPlayerA) * dSpeed * elapsed;
                    dPlayerY -= Math.Cos(dPlayerA) * dSpeed * elapsed;
                }
            }


            if ((GetAsyncKeyState('S') & 0x8000) != 0)
            {
                dPlayerX -= Math.Sin(dPlayerA) * dSpeed * elapsed;
                dPlayerY -= Math.Cos(dPlayerA) * dSpeed * elapsed;

                int indexInMap = ((int)dPlayerY * nMapWidth) + (int)(dPlayerX);
                if (map.Length > indexInMap && map[indexInMap] == '#')
                {
                    dPlayerX += Math.Sin(dPlayerA) * dSpeed * elapsed;
                    dPlayerY += Math.Cos(dPlayerA) * dSpeed * elapsed;
                }
            }

            for (int x = 0; x < nScreenWidth; x++)  // all x in our FOV
            {
                DrawColumn(screen, map, x);
            }
            WriteInConsole(hConsole, coord, screen, map);
        }

        private static void DrawColumn(char[] screen, string map, int x)
        {
            // short part of FOV angle //((double)x / nScreenWidth) * fFOV;                                                                                                    
            double dRayAngle = (dPlayerA - dFOV / 2.0d) + ((double)x / (double)nScreenWidth) * dFOV; 

            double dDistanceToWall = 0.0d;  // distance in current fRayAngle
            bool bHitWall = false;          // is wall checked
            bool bBoundary = false;         // Set when ray hits boundary between two wall blocks

            double fEyeX = Math.Sin(dRayAngle); // Координаты единичного вектора fRayAngle
            double fEyeY = Math.Cos(dRayAngle);

            while (!bHitWall && dDistanceToWall < fDepth)
            {
                dDistanceToWall += 0.1d;

                int testX = (int)(dPlayerX + fEyeX * dDistanceToWall);
                int testY = (int)(dPlayerY + fEyeY * dDistanceToWall);

                if (testX < 0 || testX >= nMapWidth || testY < 0 || testY >= nMapHeight)
                {
                    bHitWall = true;
                    dDistanceToWall = fDepth;
                }
                else if (map[testY * nMapWidth + testX] == '#')
                {
                    bHitWall = true;

                    // Set when ray hits boundary between two wall blocks
                    var list = new List<(double magn, double dot, double acos)>();
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            double edgeX = (double)testX + (double)i - dPlayerX;
                            double edgeY = (double)testY + (double)j - dPlayerY;
                            var magnitude = Math.Sqrt((edgeX * edgeX) + (edgeY * edgeY));
                            var mEdgeX = edgeX / magnitude;
                            var mEdgeY = edgeY / magnitude;
                            var dotProd = fEyeX * mEdgeX + fEyeY * mEdgeY;
                            list.Add((magnitude, dotProd, Math.Acos(dotProd)));
                        }
                    }
                    list = list.OrderBy(x => x.magn).ToList();

                    double dBoundary = 0.005d;
                    if (list[0].acos < dBoundary)
                    {
                        bBoundary = true;
                    }
                    if (list[1].acos < dBoundary)
                    {
                        bBoundary = true;
                    }
                    //if (list[2].dot < dBoundary)
                    //{
                    //    bBoundary = true;
                    //}

                }
            }
            // has distance to wall in short part of FOV
            // coord sky and floor
            int nCeiling = Convert.ToInt32((nScreenHeight / 2) - (nScreenHeight / dDistanceToWall));
            int nFloor = nScreenHeight - nCeiling;

            char nShade = Colors.Black;
            if (dDistanceToWall <= fDepth / 4d)
            {
                nShade = Colors.White;
            }
            else if (dDistanceToWall < fDepth / 3d)
            {
                nShade = Colors.Light;
            }
            else if (dDistanceToWall < fDepth / 2d)
            {
                nShade = Colors.Gray;
            }
            else if (dDistanceToWall < fDepth)
            {
                nShade = Colors.Dark;
            }
            else
            {
                nShade = Colors.Black;
            }

            if (bBoundary)
            {
                nShade = '|'; // edge
            }
            // draw all y in one x. from 0 (ceiling) -> wall -> floor
            for (int y = 0; y < nScreenHeight; y++)
            {
                if (y <= nCeiling)
                {
                    screen[y * nScreenWidth + x] = Colors.Black;
                }
                else if (y > nCeiling && y <= nFloor)
                {
                    screen[y * nScreenWidth + x] = nShade;
                }
                else
                {
                    double b = 1d - (y - nScreenHeight / 2.0) / (nScreenHeight / 2.0);
                    if (b < 0.25)
                    {
                        nShade = '#';
                    }
                    else if (b < 0.5)
                    {
                        nShade = 'x';
                    }
                    else if (b < 0.75)
                    {
                        nShade = '~';
                    }
                    else if (b < 0.9)
                    {
                        nShade = '-';
                    }
                    else
                    {
                        nShade = Colors.Black;
                    }

                    // set symbol in output screen
                    screen[y * nScreenWidth + x] = nShade;
                }
            }
        }

        private static void CreateConsole(out nint hConsole)
        {
            // Create a new console screen buffer
            hConsole = CreateConsoleScreenBuffer(
                GENERIC_READ | GENERIC_WRITE,           // Desired access
                FILE_SHARE_READ | FILE_SHARE_WRITE,     // Share mode
                IntPtr.Zero,                            // Security attributes (can be null)
                CONSOLE_TEXTMODE_BUFFER,                // Screen buffer flags
                IntPtr.Zero                             // Screen buffer data (must be null)
            );
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

            // Set the buffer size
            var bufferSize = new COORD { X = (short)nScreenWidth, Y = (short)nScreenHeight };
            if (!SetConsoleScreenBufferSize(hConsole, bufferSize))
            {
                Console.WriteLine("Failed to set console screen buffer size. Error code: " + Marshal.GetLastWin32Error());
                CloseHandle(hConsole);
                return;
            }

            // Set the window size
            var windowSize = new SMALL_RECT { Left = 0, Top = 0, Right = (short)(nScreenWidth - 1), Bottom = (short)(nScreenHeight - 1) }; //Important to subtract 1 from width and height
            if (!SetConsoleWindowInfo(hConsole, true, ref windowSize))
            {
                Console.WriteLine("Failed to set console window info. Error code: " + Marshal.GetLastWin32Error());
                CloseHandle(hConsole);
                return;
            }

            // Set the new screen buffer as the active one
            if (!SetConsoleActiveScreenBuffer(hConsole))
            {
                Console.WriteLine("Failed to set active screen buffer. Error code: " + Marshal.GetLastWin32Error());
                CloseHandle(hConsole);
                return;
            }
        }

        private static uint WriteInConsole(nint hConsole, COORD coord, char[] screen, string map)
        {
            uint dwBytesWritten;
            for (int nx = 0; nx < nMapWidth; nx++)
            {
                for (int ny = 0; ny < nMapWidth; ny++)
                {
                    screen[((ny+2) * nScreenWidth) + (nx+2)] = map[((ny) * nMapWidth) + (nx)];
                }
            }

            screen[((int)(dPlayerY + 2) * nScreenWidth) + ((int)dPlayerX + 2)] = 'P';
            screen[((int)(dirY + 2) * nScreenWidth) + ((int)dirX + 2)] = 'X';

            //for (int nx = 0; nx < 100; nx++)
            //{
            //    var n1 = (nx / 10);
            //    var n2 = (nx % 10);
            //    for (int ny = 0; ny < 2; ny++)
            //    {
            //        screen[ny * nScreenWidth + nx] = ny % 2 == 0 ? Convert.ToChar(n1.ToString()) : Convert.ToChar(n2.ToString());
            //    }
            //}


            WriteConsoleOutputCharacter(
                hConsole,
                new string(screen),
                (uint)(nScreenWidth * nScreenHeight),
                coord,
                out dwBytesWritten
            );
            return dwBytesWritten;
        }

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
        static extern bool SetConsoleActiveScreenBuffer(IntPtr hConsoleOutput);

        // Import the CloseHandle function from kernel32.dll
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        // Import the WriteConsoleOutputCharacter function from kernel32.dll
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool WriteConsoleOutputCharacter(IntPtr hConsoleOutput, string lpCharacter, uint nLength,
            COORD dwWriteCoord, out uint lpNumberOfCharsWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetConsoleScreenBufferSize(IntPtr hConsoleOutput, COORD dwSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetConsoleWindowInfo(IntPtr hConsoleOutput, bool bAbsolute, ref SMALL_RECT lpConsoleWindow);

        [StructLayout(LayoutKind.Sequential)]
        public struct SMALL_RECT
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }
        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(int vKey);


        // Structures for COORD
        [StructLayout(LayoutKind.Sequential)]
        public struct COORD
        {
            public short X;
            public short Y;
        }

        public static class Colors
        {
            public static char None = ' ';
            public static char White = '\u2588';
            public static char Light = '\u2593';
            public static char Gray = '\u2592';
            public static char Dark = '\u2591';
            public static char Black = ' ';
            public static char WhiteHalf = '\u2590';
        }

        // Constants for dwDesiredAccess (access rights)
        const uint GENERIC_READ = 0x80000000;
        const uint GENERIC_WRITE = 0x40000000;

        // Constants for dwShareMode (share mode)
        const uint FILE_SHARE_READ = 0x00000001;
        const uint FILE_SHARE_WRITE = 0x00000002;

        // Constants for dwFlags (screen buffer flags)
        const uint CONSOLE_TEXTMODE_BUFFER = 1;
    }

}
