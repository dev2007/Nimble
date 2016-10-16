using NimbleFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimble.Module
{
    /// <summary>
    /// 模块对象包装类
    /// 用于包装反射得到的类
    /// </summary>
    public class QWrapper : IQMessage
    {
        private Type assemblyModule;
        private object classInstance;

        public QWrapper(Type assemblyModule, object classInstance)
        {
            if (assemblyModule == null)
            {
                throw new Exception("反射程序集不能为空！");
            }
            if (classInstance == null)
            {
                throw new Exception("反射类对象不对为空！");
            }
            this.assemblyModule = assemblyModule;
            this.classInstance = classInstance;
        }

        public string AppName
        {
            get
            {
                return (string)assemblyModule.GetProperty("AppName").GetValue(classInstance);
            }
        }

        public string GUID
        {
            get
            {
                return (string)assemblyModule.GetProperty("GUID").GetValue(classInstance);
            }
        }

        public Priority Priority
        {
            get
            {
                return (Priority)assemblyModule.GetProperty("Priority").GetValue(classInstance);
            }
        }

        public bool ShowTail
        {
            get
            {
                return (bool)assemblyModule.GetProperty("ShowTail").GetValue(classInstance);
            }
        }

        public string Version
        {
            get
            {
                return (string)assemblyModule.GetProperty("Version").GetValue(classInstance);
            }
        }

        public string Process(string message)
        {
            Type[] params_type = new Type[1];
            params_type[0] = Type.GetType("System.String");
            Object[] params_obj = new Object[1];
            params_obj[0] = message;

            object value = assemblyModule.GetMethod("Process", params_type).Invoke(classInstance, params_obj);
            return (string)value;
        }
    }
}
