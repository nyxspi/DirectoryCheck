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
using static DirectoryCheck.DataGrid;
using System.Reflection;


namespace DirectoryCheck
{
    public class DllInfo
{
    public string FilePath { get; set; }
    public string Version { get; set; }
    public string Status { get; set; }
}

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

    public static bool IsDllExcluded(string filePath, List<string> excludedDlls)
    {
        foreach (string excludedDll in excludedDlls)
        {
            if (filePath.EndsWith(excludedDll, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }

        public static List<string> GetOutliers(List<string> dllFiles)
        {
            List<string> outliers = new List<string>();
            List<Version> versions = new List<Version>();
            foreach (string file in dllFiles)
            {
                try
                {
                    Version version = AssemblyName.GetAssemblyName(file).Version;
                    versions.Add(version);
                }
                catch (Exception ex)
                {
                    // Ignore the exception and continue with the next file
                    Console.WriteLine($"Error getting version for file {file}: {ex.Message}");
                }
            }

            if (versions.Count > 0)
            {
                Version averageVersion = new Version((int)Math.Round(versions.Average(v => v.Major)), (int)Math.Round(versions.Average(v => v.Minor)), (int)Math.Round(versions.Average(v => v.Build)), (int)Math.Round(versions.Average(v => v.Revision)));


                foreach (string file in dllFiles)
                {
                    try
                    {
                        Version version = AssemblyName.GetAssemblyName(file).Version;
                        if (version < averageVersion)
                        {
                            outliers.Add(file);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Ignore the exception and continue with the next file
                        Console.WriteLine($"Error getting version for file {file}: {ex.Message}");
                    }
                }
            }

            return outliers;
        }
    

    public static string GetAverageVersion(List<string> versionList)
    {
        int totalVersions = 0;
        int totalMajor = 0;
        int totalMinor = 0;
        int totalBuild = 0;
        int totalRevision = 0;
        foreach (string versionString in versionList)
        {
            Version version = new Version(versionString);
            totalVersions++;
            totalMajor += version.Major;
            totalMinor += version.Minor;
            totalBuild += version.Build;
            totalRevision += version.Revision;
        }
        int averageMajor = totalMajor / totalVersions;
        int averageMinor = totalMinor / totalVersions;
        int averageBuild = totalBuild / totalVersions;
        int averageRevision = totalRevision / totalVersions;
        return $"{averageMajor}.{averageMinor}.{averageBuild}.{averageRevision}";
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
            string directoryPath = @"C:\MyProject\Release";
            List<string> excludedDlls = new List<string> { "ThirdParty.dll", "AnotherThirdParty.dll" };

            List<string> dllFiles = DllVersionChecker.GetDllFiles(directoryPath);
            List<DllInfo> dllInfoList = new List<DllInfo>();
            foreach (string dllFile in dllFiles)
            {
                if (DllVersionChecker.IsDllExcluded(dllFile, excludedDlls))
                {
                    continue;
                }

                string version = FileVersionInfo.GetVersionInfo(dllFile).FileVersion;
                if (version == null)
                {
                    version = "N/A";
                }
                else
                {
                    version = new Version(version).ToString();
                }

                string averageVersion = DllVersionChecker.GetAverageVersion(dllInfoList.Select(x => x.Version).ToList());
                string status = version == averageVersion ? "Up to date" : "Outdated";
                dllInfoList.Add(new DllInfo { FilePath = dllFile, Version = version, Status = status });
            }

            /*DllInfoWindow dllInfoWindow = new DllInfoWindow();
            dllInfoWindow.PopulateDllDataGrid(dllInfoList);
            dllInfoWindow.ShowDialog();*/
        }


    }
}
