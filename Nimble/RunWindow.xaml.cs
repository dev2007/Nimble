using Newtonsoft.Json;
using Nimble.Module;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Nimble
{
    /// <summary>
    /// RunWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RunWindow : Window
    {
        private static MainWindow mainWindow;
         
        public RunWindow()
        {
            InitializeComponent();
            Plugin.EnableAllInvoker();
        }

        public static void BindMainWindow(MainWindow instance)
        {
            mainWindow = instance;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            ConfigWindow configWindow = new ConfigWindow();
            configWindow.ShowDialog();
        }

        private void WriteFile(RobotConfig config)
        {
            string json = JsonConvert.SerializeObject(config);

            FileStream file = null;
            StreamWriter sw = null;
            try
            {
                file = new FileStream(Define.SettingConfigPath, FileMode.Create);
                sw = new StreamWriter(file);
                sw.Write(json);
                sw.Flush();
            }
            catch (Exception e)
            {

            }
            finally
            {
                if (sw != null)
                    sw.Close();
                if (file != null)
                    file.Close();
            }
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.Setting();
        }
    }
}
