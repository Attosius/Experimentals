namespace Console3d
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.CompilerServices;
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

        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(int vKey);


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

        static int nScreenWidth = 120; // Ширина консольного окна X
        static int nScreenHeight = 40; // Высота консольного окна Y
    
        static double fPlayerX = 5.0f; // Координата игрока по оси X
        static double fPlayerY = 7.0f; // Координата игрока по оси Y
        static double fPlayerA = 0.0f; // Направление игрока

        static double dirX = 5.0f; // Координата игрока по оси X
        static double dirY = 7.0f; // Координата игрока по оси Y


        static int nMapHeight = 16; // Высота игрового поля
        static int nMapWidth = 16;  // Ширина игрового поля
       
        static double fFOV = 3.14159 / 3; // Угол обзора (поле видимости)
        static float fDepth = 30.0f;     // Максимальная дистанция обзора
        static double fTurn = 5.0f;			// Walking Speed
        static double fSpeed = 5.0f;			// Walking Speed

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
            map += "#..##...###....#";
            map += "#..............#";
            map += "#........#.....#";
            map += "#........#.....#";
            map += "#..............#";
            map += "#..............#";
            map += "#...###...##...#";
            map += "#..............#";
            map += "#..............#";
            map += "################";

            for (int ny = 0; ny < screen.Length; ny++)
            {
                // screen[ny] = '+';
            }

            dwBytesWritten = WriteInConsole(hConsole, coord, screen, map);
            var sw = Stopwatch.StartNew();
            while (true) // Игровой цикл
            {
                var elapsed = sw.ElapsedMilliseconds / (double)1000;
                Debug.Write(elapsed.ToString());
                sw.Restart();
                //Thread.Sleep(10);
                short keyState = GetAsyncKeyState('A');

                if ((GetAsyncKeyState('A') & 0x8000) != 0)
                {
                    fPlayerA -= fTurn * (0.75f) * elapsed;
                }
                if ((GetAsyncKeyState('D') & 0x8000) != 0)
                {
                    fPlayerA += fTurn * (0.75f) * elapsed;
                }

                Debug.WriteLine($" fPlayerA: {fPlayerA:0.000}, sin: {Math.Sin(fPlayerA):0.000}, cos: {Math.Cos(fPlayerA):0.000}");
                Debug.WriteLine($" X {fPlayerX:0.000} => {Math.Sin(fPlayerA) * 5f * elapsed:0.000}");
                Debug.WriteLine($" Y {fPlayerY:0.000} => {Math.Cos(fPlayerA) * 5f * elapsed:0.000}");
                dirX = fPlayerX + Math.Sin(fPlayerA) * 2;
                dirY = fPlayerY + Math.Cos(fPlayerA) * 2;
                Debug.WriteLine($" DirX {dirX:0.000}");
                Debug.WriteLine($" DirY {dirY:0.000}");
                Debug.WriteLine($"-------------------");

                if ((GetAsyncKeyState('W') & 0x8000) != 0)
                {
                    fPlayerX += Math.Sin(fPlayerA) * 5f * elapsed;
                    fPlayerY += Math.Cos(fPlayerA) * 5f * elapsed;

                    int indexInMap = ((int)fPlayerX * nMapWidth) + (int)(fPlayerY);
                    if (map.Length > indexInMap && map[indexInMap] == '#')
                    {
                        fPlayerX -= Math.Sin(fPlayerA) * 5f * elapsed;
                        fPlayerY -= Math.Cos(fPlayerA) * 5f * elapsed;
                    }
                }


                if ((GetAsyncKeyState('S') & 0x8000) != 0)
                {
                    fPlayerX -= Math.Sin(fPlayerA) * 5f * elapsed;
                    fPlayerY -= Math.Cos(fPlayerA) * 5f * elapsed;

                    int indexInMap = ((int)fPlayerX * nMapWidth) + (int)(fPlayerY);
                    if (map.Length > indexInMap && map[indexInMap] == '#')
                    {
                        fPlayerX += Math.Sin(fPlayerA) * 5f * elapsed;
                        fPlayerY += Math.Cos(fPlayerA) * 5f * elapsed;
                    }
                }
                if (fPlayerX > nScreenWidth)
                {
                    fPlayerX = nScreenWidth;
                }
                if (fPlayerY > nScreenHeight)
                {
                    fPlayerY = nScreenHeight;
                }
                for (int x = 0; x < nScreenWidth; x++)  // Проходим по всем X
                {
                    double fRayAngle = (fPlayerA - fFOV / 2.0d) + (fFOV / nScreenWidth) * x; // short part of FOV angle //((double)x / nScreenWidth) * fFOV;
                    //Debug.WriteLine($" fRayAngle {fRayAngle:0.000}, fPlayerA: {fPlayerA:0.000}");

                    double fDistanceToWall = 0.0d; // Расстояние до препятствия в направлении fRayAngle
                    bool bHitWall = false; // Достигнул ли луч стенку

                    double fEyeX = Math.Sin(fRayAngle); // Координаты единичного вектора fRayAngle
                    double fEyeY = Math.Cos(fRayAngle);

                    while(!bHitWall && fDistanceToWall < fDepth)
                    {
                        fDistanceToWall += 0.1d;
                        int testX = Convert.ToInt32(fPlayerX + fEyeX * fDistanceToWall);
                        int testY = Convert.ToInt32(fPlayerY + fEyeY * fDistanceToWall);
                        if (testX < 0 || testX >= nMapWidth || testY < 0 || testY >= nMapHeight)
                        {
                            bHitWall = true;
                            fDistanceToWall = fDepth;
                        } else if (map[testY * nMapWidth + testX] == '#')
                        {
                            bHitWall = true;
                            //
                        }
                    }
                    // has distance to wall in short part of FOV
                    // coord sky and floor
                    int nCeiling = Convert.ToInt32((nScreenHeight / 2) - (nScreenHeight / fDistanceToWall));
                    int nFloor = nScreenHeight - nCeiling;

                    short nShade = 0;

                    if (fDistanceToWall <= fDepth / 3d)
                    {
                        nShade = (short)'1';
                    }
                    else if (fDistanceToWall < fDepth / 2d)
                    {
                        nShade = (short)'2';
                    }
                    else if (fDistanceToWall < fDepth / 1.5d)
                    {
                        nShade = (short)'3';
                    }
                    else if (fDistanceToWall < fDepth)
                    {
                        nShade = (short)'4';
                    }
                    else
                    {
                        nShade = (short)'5';
                    }

                    for (int y = 0; y < nScreenHeight; y++)
                    {
                        if (y < nCeiling)
                        {
                            screen[y * nScreenWidth + x] = '6';
                        }else if (y > nCeiling && y < nFloor)
                        {
                            screen[y * nScreenWidth + x] = (char)nShade;
                        }else
                        {
                            double b = 1d - (y - nScreenHeight / 2.0) / (nScreenHeight / 2.0);
                            if (b < 0.25)
                            {
                                nShade = (short)'#';
                            }
                            else if (b < 0.5)
                            {
                                nShade = (short)'x';
                            }
                            else if (b < 0.75)
                            {
                                nShade = (short)'~';
                            }
                            else if (b < 0.9)
                            {
                                nShade = (short)'-';
                            }else
                            {
                                nShade = (short)'7';
                            }



                            screen[y * nScreenWidth + x] = (char)nShade; ;
                        }
                    }


                }
                WriteInConsole(hConsole, coord, screen, map);
            }


            CloseHandle(hConsole);  // Close the new buffer's handle when you're done.
        }

        private static uint WriteInConsole(nint hConsole, COORD coord, char[] screen, string map)
        {
            uint dwBytesWritten;
            for (int nx = 0; nx < nMapWidth; nx++)
            {
                for (int ny = 0; ny < nMapWidth; ny++)
                {
                    screen[(ny * nScreenWidth) + (nx)] = map[(ny * nMapWidth) + (nx)];
                }
            }

            screen[((int)fPlayerY * nScreenWidth) + ((int)fPlayerX)] = 'P';
            //screen[((int)fPlayerX * nScreenWidth) + ((int)fPlayerY)] = 'P';
            screen[((int)(dirY) * nScreenWidth) + ((int)dirX)] = 'X';


            //for (int nx = 0; nx < nMapWidth; nx++)
            //    for (int ny = 0; ny < nMapWidth; ny++)
            //    {
            //        screen[(ny + 1) * nScreenWidth + nx] = map[ny * nMapWidth + nx];
            //    }

            //screen[((int)fPlayerX + 1) * nScreenWidth + (int)fPlayerY] = 'P';

            WriteConsoleOutputCharacter(
                hConsole,
                new string(screen),
                (uint)(nScreenWidth * nScreenHeight),
                coord,
                out dwBytesWritten
            );
            return dwBytesWritten;
        }
    }

}
