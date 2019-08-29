using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NexChip.SignMessage.Utils
{
    /// <summary>
    ///     log4net日志专用
    /// </summary>
    public static class LogHelper
    {
        private static Type _t = typeof(LogHelper);
        private static readonly ILog Instance;
        static LogHelper()
        {
            var repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(repository, new FileInfo("Log4Net.xml"));

            Instance = LogManager.GetLogger(repository.Name, "Log:");
        }


        public static void setType(Type t)
        {
            _t = t;
        }

        //public static void SetConfig()
        //{

        //    XmlConfigurator.Configure();
        //}

        //public static void SetConfig(string filePath)
        //{
        //    var configFile = new FileInfo(filePath);
        //    XmlConfigurator.Configure(configFile);
        //}

        //public static void SetConfig(FileInfo configFile)
        //{
        //    XmlConfigurator.Configure(configFile);
        //}

        /// <summary>
        ///     记录普通文件记录
        /// </summary>
        /// <param name="info"></param>
        public static void Info(string info)
        {
            if (Instance.IsInfoEnabled)
            {
                Instance.Info(info);
            }
        }

        /// <summary>
        ///     记录调试信息
        /// </summary>
        /// <param name="info"></param>
        public static void Debug(string info)
        {
            if (Instance.IsErrorEnabled)
            {
                Instance.Debug(info);
            }
        }

        /// <summary>
        ///     记录警告信息
        /// </summary>
        /// <param name="info"></param>
        public static void Warn(string info)
        {
            if (Instance.IsWarnEnabled)
            {
                Instance.Warn(info);
            }
        }

        /// <summary>
        ///     记录错误日志
        /// </summary>
        /// <param name="info"></param>
        /// <param name="se"></param>
        public static void Error(string info, Exception se)
        {
            if (Instance.IsErrorEnabled)
            {
                Instance.Error(info, se);
            }
        }

        /// <summary>
        ///     记录严重错误
        /// </summary>
        /// <param name="info"></param>
        /// <param name="se"></param>
        public static void Fatal(string info, Exception se)
        {
            if (Instance.IsFatalEnabled)
            {
                Instance.Fatal(info, se);
            }
        }
    }

}
