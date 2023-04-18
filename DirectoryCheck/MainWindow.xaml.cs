using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Path = System.IO.Path;
using System.Diagnostics;

namespace DirectoryCheck
{

    public static class DllVersionChecker
    {
        public static List<string> GetDllFiles(string directoryPath)
        {
            List<string> dllFiles = new List<string>();
            string[] files = Directory.GetFiles(directoryPath);
            foreach (string file in files)
            {
                if (Path.GetExtension(file).ToLower() == ".dll")
                {
                    dllFiles.Add(file);
                }
            }
            return dllFiles;
        }
    }



    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string GetDllVersion(string filePath)
        {
            string version = "unknown";
            if (File.Exists(filePath))
            {
                try
                {
                    FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(filePath);
                    version = fileVersionInfo.FileVersion;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting version for file {filePath}: {ex.Message}");
                }
            }
            return version;
        }


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }


    }
}
