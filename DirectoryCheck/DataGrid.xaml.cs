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
using System.Windows.Shapes;
using static DirectoryCheck.DataGrid;

namespace DirectoryCheck
{
    /// <summary>
    /// Interaction logic for DataGrid.xaml
    /// </summary>
    public partial class DataGrid : Window
    {
        public DataGrid()
        {
            InitializeComponent();
        }

        public class DllInfo
        {
            public static List<DllInfo> ItemsSource { get; internal set; }
            public string FileName { get; set; }
            public Version Version { get; set; }
        }

        public void PopulateDllDataGrid(List<DllInfo> dllInfoList)
        {
            DllInfo.ItemsSource = dllInfoList;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
