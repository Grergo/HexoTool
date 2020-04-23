using MaterialDesignThemes.Wpf;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace HexoTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Setting s;
        private string Editor="";
        public MainWindow()
        {
            InitializeComponent();
            InitConfig();
            Loadscaffolds();
        }
        private void Window_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }
        private async void Setting_Click(object sender, RoutedEventArgs e)
        {
            s = new Setting();
            _ = await DialogHost.Show(s, "RootDialogHost");
            Editor = s.select.Content.ToString();
        }
        private void InitConfig()
        {
            
            if(ConfigurationManager.AppSettings["MarkDownEditer"] != null)
            {
                Editor = ConfigurationManager.AppSettings["MarkDownEditer"].ToString();
            }
                
        }
        private string ExcuteCMD(string Command)
        {
            var processInfo = new ProcessStartInfo("cmd.exe", "/S /C " + Command)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true
            };

            var process = new Process { StartInfo = processInfo };
            process.Start();
            string output= process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output.ToString();
        }

        private void UpLoad_Click(object sender, RoutedEventArgs e)
        {
            sbar.IsActive = false;
            sbarmessage.Content = "正在发布,请稍后";
            sbar.IsActive = true;
            try
            {
                ExcuteCMD("hexo clean");
                ExcuteCMD("hexo g");
                ExcuteCMD("hexo d");
            }
            catch
            {
                sbar.IsActive = false;
                sbarmessage.Content = "找不到hexo";
                sbar.IsActive = true;
            }
            
            string output=ExcuteCMD("python ./libs/ReloadGitee.py");
                sbar.IsActive = false;
                sbarmessage.Content = "执行完毕,返回信息如下:  "+output;
                sbar.IsActive = true;
            
        }

        private void Newartical_Click(object sender, RoutedEventArgs e)
        {
            if (Editor == "")
            {
                sbarmessage.Content = "请先选择MarkDown编辑器";
                sbar.IsActive = true;
                return;
            }
            else
            {
                ExcuteCMD("hexo new " + sc.SelectedItem.ToString() + " " + Filenamebox.Text);
                ExcuteCMD(Editor + " " + "./source/_posts/" + Filenamebox.Text+".md");
            }
        }
        private void Loadscaffolds()
        {
            
            if(Directory.Exists(Directory.GetCurrentDirectory().Replace("\\", "/") + "/scaffolds"))
            {
                DirectoryInfo TheFolder = new DirectoryInfo(Directory.GetCurrentDirectory().Replace("\\", "/") + "/scaffolds");
                foreach (FileInfo NextFolder in TheFolder.GetFiles())
                {
                    sc.Items.Add(NextFolder.Name.Split('.')[0]);
                }
            }
            else
            {
                sbarmessage.Content = "请将本工具放置于Hexo目录下";
                sbar.IsActive = true;
            }
            
        }

        private void sbarmessage_ActionClick(object sender, RoutedEventArgs e)
        {
            sbar.IsActive = false;
        }
    }
}
