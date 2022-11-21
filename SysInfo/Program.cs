using System;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SysInfo
{
    internal class Program
    {
        private const int MF_BYCOMMAND = 0x00000000;

        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_SIZE = 0xF000;

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        static void Main(string[] args)
        {
            Console.SetWindowSize(33, 26);

            Console.BufferWidth = Console.WindowWidth = 60;
            Console.BufferHeight = Console.WindowHeight;

            IntPtr handle = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(handle, false);

            if (handle != IntPtr.Zero)
            {
                DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);
            }

            Console.Title = "System Info";

            Console.WriteLine($"Machine name: {Environment.MachineName}");
            Console.WriteLine($"Computer name: {SystemInformation.ComputerName}");
            Console.WriteLine($"Username: {SystemInformation.UserName}");
            Console.WriteLine($"User domain name: {SystemInformation.UserDomainName}\n");

            Console.WriteLine($"OS: {Environment.OSVersion}");
            Console.WriteLine($"64 bit: {Environment.Is64BitOperatingSystem}\n");

            using (var mos = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor"))
                foreach (ManagementObject mo in mos.Get())
                    Console.WriteLine($"Procesor: {mo["Name"]}");

            Console.WriteLine($"Processor count (Cores): {Environment.ProcessorCount}\n");

            Console.WriteLine($"Mouse wheel present: {SystemInformation.MouseWheelPresent}");
            Console.WriteLine($"Mouse buttons: {SystemInformation.MouseButtons}\n");

            Console.WriteLine($"Network: {SystemInformation.Network}\n");

            Console.WriteLine($"Working area:  Width: {SystemInformation.WorkingArea.Width} Height: {SystemInformation.WorkingArea.Height}");
            Console.WriteLine($"Monitor count: {SystemInformation.MonitorCount}");
            Console.WriteLine($"Primary monitor size: {SystemInformation.PrimaryMonitorSize.Width} x {SystemInformation.PrimaryMonitorSize.Height}\n");

            for (int i = 0; i < Screen.AllScreens.Length; i++)
            {
                var screen = Screen.AllScreens[i];

                Console.WriteLine($"Is primary Screen: {screen.Primary}");
                Console.WriteLine($"Device Name: {screen.DeviceName}");

                var mpixel = (decimal)(screen.Bounds.Width * screen.Bounds.Height) / 1000000;

                Console.WriteLine($"Resolution: {screen.Bounds.Width} x {screen.Bounds.Height} {mpixel} Megapixels");
                Console.WriteLine($"Bits per pixel: {screen.BitsPerPixel}");
            }

            Console.ReadKey();
        }
    }
}
