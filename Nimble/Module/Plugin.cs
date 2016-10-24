using Newtonsoft.Json;
using NimbleFrame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nimble.Module
{
    public class Plugin
    {
        public static IDictionary<QWrapper, bool> wrapperList = new Dictionary<QWrapper, bool>();

        public static Contact.Message MessageManager = new Contact.Message();

        private static IList<string> checkList = new List<string>();

        /// <summary>
        /// 启用插件
        /// </summary>
        /// <param name="invoker"></param>
        /// <returns></returns>
        public static bool EnableInvoker(IQMessage invoker)
        {
            return MessageManager.BindInvoker(invoker);
        }

        /// <summary>
        /// 移除所有插件
        /// </summary>
        /// <returns></returns>
        public static bool RemoveAllInvoker()
        {
            return MessageManager.RemoveAllInvoker();
        }

        /// <summary>
        /// 停用插件
        /// </summary>
        /// <param name="invoker"></param>
        /// <returns></returns>
        public static bool DisableInvoker(IQMessage invoker)
        {
            return MessageManager.UnbindInvoker(invoker);
        }

        /// <summary>
        /// 加载插件目录下的Dll
        /// </summary>
        public static void LoadDll()
        {
            checkList = ReadFile();
            Plugin.Clear();
            DirectoryInfo folder = new DirectoryInfo(System.Environment.CurrentDirectory + @"\modules");
            foreach (FileInfo file in folder.GetFiles())
            {
                var result = Plugin.Load(file.FullName, file.Name.Substring(0, file.Name.Length - 4), file.Extension);
            }
        }

        /// <summary>
        /// 启用所有模块
        /// </summary>
        public static void EnableAllInvoker()
        {
            foreach (var data in wrapperList)
            {
                if (data.Value)
                    EnableInvoker(data.Key);
            }
        }


        /// <summary>
        /// 清理所有读取的插件
        /// </summary>
        private static void Clear()
        {
            wrapperList.Clear();
        }

        /// <summary>
        /// 加载插件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        private static bool Load(string path, string name, string extension)
        {
            if (extension != ".dll")
                return true;

            if (!File.Exists(path))
            {
                return false;
            }
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
            Assembly assembly = Assembly.LoadFile(path);
            Type moduleType = assembly.GetType(name);
            object instance = assembly.CreateInstance(name);
            QWrapper wrapper = new QWrapper(moduleType, instance);
            wrapperList.Add(wrapper, checkList.Contains(wrapper.GUID));
            return true;
        }

        private static IList<string> ReadFile()
        {
            StreamReader sr = null;
            IList<string> data = null;
            try
            {
                sr = new StreamReader(Define.ModuleConfigPath, Encoding.Default);
                string line;
                string content = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    content += line;
                }
                data = (IList<string>)JsonConvert.DeserializeObject(content, typeof(IList<string>));
                return data;
            }
            catch
            {

            }
            finally
            {
                if (sr != null)
                    sr.Close();
            }
            return data;
        }
    }
}
