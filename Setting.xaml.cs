using Microsoft.Win32;
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
using System.Configuration;

namespace HexoTool
{
    /// <summary>
    /// Setting.xaml 的交互逻辑
    /// </summary>
    public partial class Setting : UserControl
    {
        private string filename;
        public Setting()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;//该值确定是否可以选择多个文件
            dialog.Title = "请选择编辑器";
            dialog.Filter = "配置文件(*.exe)|*.exe";
            if (dialog.ShowDialog() == true)
            {
                filename = dialog.FileName.Replace("\\", "/");
                select.Content = filename;
                if (ConfigurationManager.AppSettings["MarkDownEditer"] != null)
                {
                    ConfigurationManager.AppSettings.Remove("MarkDownEditer");
                }
                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings.Add("MarkDownEditer", filename);
                config.Save();
            }
        }
    }
}
