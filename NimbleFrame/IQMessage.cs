namespace NimbleFrame
{
    public interface IQMessage
    {
        /// <summary>
        /// 展示应用名称作为尾巴
        /// </summary>
        bool ShowTail { get; }
        /// <summary>
        /// 应用名称
        /// </summary>
        string AppName { get; }
        /// <summary>
        /// 应用GUID
        /// </summary>
        string GUID { get; }
        /// <summary>
        /// 应用优先级
        /// </summary>
        Priority Priority { get; }
        /// <summary>
        /// 应用版本
        /// </summary>
        string Version { get; }
        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        string Process(string message);
    }
}