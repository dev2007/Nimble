using Newtonsoft.Json;
using Nimble.Model;
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
    /// ConfigWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigWindow : Window
    {
        private IList<ModuleModel> moduleList = new List<ModuleModel>();

        public ConfigWindow()
        {
            InitializeComponent();
            this.Closed += ConfigWindow_Closed;
            InitList();
        }

        private void ConfigWindow_Closed(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                BindInvoker();
            });
        }

        private void BindInvoker()
        {
            Plugin.RemoveAllInvoker();
            var toWriteList = new List<string>();
            foreach (var data in moduleList)
            {
                if (data.IsCheck)
                {
                    toWriteList.Add(data.Invoker.GUID);
                    Plugin.EnableInvoker(data.Invoker);
                }
            }

            WriteFile(toWriteList);
        }

        private void WriteFile(IList<string> list)
        {
            string json = JsonConvert.SerializeObject(list);

            FileStream file = null;
            StreamWriter sw = null;
            try
            {
                file = new FileStream(Define.ConfigPath, FileMode.Create);
                sw = new StreamWriter(file);
                sw.Write(json);
                sw.Flush();
            }
            catch(Exception e)
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


        private void InitList()
        {
            Plugin.LoadDll();
            moduleList.Clear();
            foreach (var data in Plugin.wrapperList)
            {
                var invoker = data.Key;
                var check = data.Value;
                moduleList.Add(new ModuleModel() { IsCheck = check, Name = invoker.AppName, Version = invoker.Version, Invoker = invoker });
            }
            listBox.ItemsSource = moduleList;
        }


        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ListBoxItem listBoxItem = GetVisualAncestor<ListBoxItem>((DependencyObject)sender);
            listBox.SelectedValue = listBox.ItemContainerGenerator.ItemFromContainer(listBoxItem);
        }

        private static T GetVisualAncestor<T>(DependencyObject o) where T : DependencyObject
        {
            do
            {
                o = VisualTreeHelper.GetParent(o);
            } while (o != null && !typeof(T).IsAssignableFrom(o.GetType()));

            return (T)o;
        }
    }
}
