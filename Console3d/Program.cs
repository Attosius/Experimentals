namespace Console3d
{
    using System;
    using System.Runtime.InteropServices;

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


        // Constants for dwDesiredAccess (access rights)
        const uint GENERIC_READ = 0x80000000;
        const uint GENERIC_WRITE = 0x40000000;

        // Constants for dwShareMode (share mode)
        const uint FILE_SHARE_READ = 0x00000001;
        const uint FILE_SHARE_WRITE = 0x00000002;

        // Constants for dwFlags (screen buffer flags)
        const uint CONSOLE_TEXTMODE_BUFFER = 1;


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

            // Write something to the new buffer
            Console.WriteLine("This is written to the new screen buffer.");

            Console.ReadKey(); // Keep the window open
            Console.ReadKey(); // Keep the window open

            // Clean up:  Revert to original buffer (optional but recommended)
            // You'd need to store the handle of the original buffer to revert.

            //CloseHandle(hConsole);  // Close the new buffer's handle when you're done.
            //Otherwise you get a resource leak
        }
    }

}
