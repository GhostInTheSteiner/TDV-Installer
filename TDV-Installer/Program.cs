using System;
using System.IO;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Globalization;
using System.Windows.Forms;

namespace TDV_Installer
{
    class Program
    {
        [DllImport("gdi32", EntryPoint = "AddFontResource")]
        public static extern int AddFontResourceA(string lpFileName);
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern int AddFontResource(string lpszFilename);
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern int CreateScalableFontResource(uint fdwHidden, string
            lpszFontRes, string lpszFontFile, string lpszCurrentPath);




        static void Main(string[] args)
        {
            RegisterFont("Book_Antiqua.ttf");
            RegisterFont("DroidSansFallback.ttf");
            RegisterFont("times_new_yorker.ttf");
            RegisterFont("Zenzai_Itacha.ttf");

            Console.WriteLine("Succesfully registered fonts!");




            File.Copy(
                Environment.CurrentDirectory + "\\Dont look at me\\The Distant Valhalla.exe",
                Environment.CurrentDirectory + "\\The Distant Valhalla.exe"
            );

            Console.WriteLine("Succesfully copied 'The Distant Valhalla.exe'!");




            SetWin7Compatibility(Environment.CurrentDirectory + "\\The Distant Valhalla.exe");

            Console.WriteLine("Succesfully set to Windows 7 compatibility!");




            if (System.Text.Encoding.Default.HeaderName.Equals("iso-2022-jp"))
            {
                MessageBox.Show("\"The Distant Valhalla - Valkyrie's Rebirth\" was successfully installed on your machine.");
            }
            else
            {
                MessageBox.Show("\"The Distant Valhalla - Valkyrie's Rebirth\" was successfully installed on your machine. However, your system doesn't have japanese locale yet. This will cause a few images to not be displayed." + Environment.NewLine + Environment.NewLine + "Please follow the instrucions on the download page to switch to japanese locale before reading.");
            }
        }




        static void SetWin7Compatibility(string exeFilePath)
        {
            var key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers", true);
            if (key == null)
                throw new InvalidOperationException(@"Cannot open registry key HKCU\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers.");
            using (key)
                key.SetValue(exeFilePath, "~ WIN7RTM");
        }




        /// <summary>
        /// Installs font on the user's system and adds it to the registry so it's available on the next session
        /// Your font must be included in your project with its build path set to 'Content' and its Copy property
        /// set to 'Copy Always'
        /// </summary>
        /// <param name="contentFontName">Your font to be passed as a resource (i.e. "myfont.tff")</param>
        private static void RegisterFont(string contentFontName)
        {
            // Creates the full path where your font will be installed
            var fontDestination = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Fonts), contentFontName);

            if (!File.Exists(fontDestination))
            {
                // Copies font to destination
                System.IO.File.Copy(Path.Combine(System.IO.Directory.GetCurrentDirectory(), "fonts\\" + contentFontName), fontDestination);

                // Retrieves font name
                // Makes sure you reference System.Drawing
                PrivateFontCollection fontCol = new PrivateFontCollection();
                fontCol.AddFontFile(fontDestination);
                var actualFontName = fontCol.Families[0].Name;

                //Add font
                AddFontResource(fontDestination);
                //Add registry entry   
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts", actualFontName, contentFontName, RegistryValueKind.String);
            }
        }
    }
}
