using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.IO;
using log4net;
using log4net.Config;

namespace WebApplication1.Models
{
    /// <summary>
    /// 泛型日志服务类，封装Log4Net
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LogService<T>
    {
        #region 单例实现

        private static LogService<T> _instance = null;
        private ILog log;

        private LogService()
        {
            XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
            log = LogManager.GetLogger(typeof(T));
        }

        public static LogService<T> Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LogService<T>();
                }
                return _instance;
            }
        }

        #endregion

        #region 封装log4net的ILog方法

        public void Debug(object message)
        {
            log.Debug(message);
        }

        public void DebugFormatted(string format, params object[] args)
        {
            log.DebugFormat(CultureInfo.InvariantCulture, format, args);
        }

        public void Info(object message)
        {
            log.Info(message);
        }

        public void InfoFormatted(string format, params object[] args)
        {
            log.InfoFormat(CultureInfo.InvariantCulture, format, args);
        }

        public void Warn(object message)
        {
            log.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            log.Warn(message, exception);
        }

        public void WarnFormatted(string format, params object[] args)
        {
            log.WarnFormat(CultureInfo.InvariantCulture, format, args);
        }

        public void Error(object message)
        {
            log.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            log.Error(message, exception);
        }

        public void ErrorFormatted(string format, params object[] args)
        {
            log.ErrorFormat(CultureInfo.InvariantCulture, format, args);
        }

        public void Fatal(object message)
        {
            log.Fatal(message);
        }

        public void Fatal(object message, Exception exception)
        {
            log.Fatal(message, exception);
        }

        public void FatalFormatted(string format, params object[] args)
        {
            log.FatalFormat(CultureInfo.InvariantCulture, format, args);
        }

        public bool IsDebugEnabled
        {
            get
            {
                return log.IsDebugEnabled;
            }
        }

        public bool IsInfoEnabled
        {
            get
            {
                return log.IsInfoEnabled;
            }
        }

        public bool IsWarnEnabled
        {
            get
            {
                return log.IsWarnEnabled;
            }
        }

        public bool IsErrorEnabled
        {
            get
            {
                return log.IsErrorEnabled;
            }
        }

        public bool IsFatalEnabled
        {
            get
            {
                return log.IsFatalEnabled;
            }
        }

        #endregion

        #region 删除日志文件
        /// <summary>
        /// 删除日志
        /// </summary>
        public void DeleteLogFile()
        {
            try
            {
                int maxSize = 90;       //默认保留90天的日志

                if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogSaveDays"]))
                {
                    string strMaxSize = System.Configuration.ConfigurationManager.AppSettings["LogSaveDays"];
                    int.TryParse(strMaxSize, out maxSize);
                }

                string logFilePath = String.Empty;      //定义日志文件目录变量
                log4net.Appender.IAppender[] appenders = this.log.Logger.Repository.GetAppenders();
                foreach (log4net.Appender.IAppender appender in appenders)
                {
                    if (appender.Name == "FileAppender" && appender.GetType().Name == "RollingFileAppender")
                    {
                        log4net.Appender.RollingFileAppender rollingFileAppender = appender as log4net.Appender.RollingFileAppender;
                        logFilePath = Path.GetDirectoryName(rollingFileAppender.File);     //获取日志文件目录
                        break;
                    }
                }
                if (!String.IsNullOrEmpty(logFilePath))
                {
                    //logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logFilePath);
                    if (Directory.Exists(logFilePath))
                    {
                        DirectoryInfo dir = new DirectoryInfo(logFilePath);
                        FileInfo[] logFiles = dir.GetFiles();
                        if (logFiles != null && logFiles.Length > maxSize)
                        {
                            Dictionary<string, FileInfo> dicFiles = new Dictionary<string, FileInfo>();
                            List<string> lstFileNames = new List<string>();
                            foreach (FileInfo fi in logFiles)
                            {
                                dicFiles.Add(fi.FullName, fi);
                                lstFileNames.Add(fi.FullName);
                            }
                            lstFileNames.Sort();
                            for (int i = 0; i < lstFileNames.Count - maxSize; i++)
                            {
                                dicFiles[lstFileNames[i]].Delete();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Error("删除日志文件发生错误:" + ex.Message);
            }
        }

        #endregion
    }

    /// <summary>
    /// 日志服务类，封装Log4Net
    /// </summary>
    public sealed class LogService
    {
        #region 单例实现

        private static LogService _instance = null;
        private ILog log;

        private LogService()
        {
            XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
            log = LogManager.GetLogger(typeof(LogService));
        }

        public static LogService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LogService();
                }
                return _instance;
            }
        }

        #endregion

        #region 封装log4net的ILog方法

        public void Debug(object message)
        {
            log.Debug(message);
        }

        public void DebugFormatted(string format, params object[] args)
        {
            log.DebugFormat(CultureInfo.InvariantCulture, format, args);
        }

        public void Info(object message)
        {
            log.Info(message);
        }

        public void InfoFormatted(string format, params object[] args)
        {
            log.InfoFormat(CultureInfo.InvariantCulture, format, args);
        }

        public void Warn(object message)
        {
            log.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            log.Warn(message, exception);
        }

        public void WarnFormatted(string format, params object[] args)
        {
            log.WarnFormat(CultureInfo.InvariantCulture, format, args);
        }

        public void Error(object message)
        {
            log.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            log.Error(message, exception);
        }

        public void ErrorFormatted(string format, params object[] args)
        {
            log.ErrorFormat(CultureInfo.InvariantCulture, format, args);
        }

        public void Fatal(object message)
        {
            log.Fatal(message);
        }

        public void Fatal(object message, Exception exception)
        {
            log.Fatal(message, exception);
        }

        public void FatalFormatted(string format, params object[] args)
        {
            log.FatalFormat(CultureInfo.InvariantCulture, format, args);
        }

        public bool IsDebugEnabled
        {
            get
            {
                return log.IsDebugEnabled;
            }
        }

        public bool IsInfoEnabled
        {
            get
            {
                return log.IsInfoEnabled;
            }
        }

        public bool IsWarnEnabled
        {
            get
            {
                return log.IsWarnEnabled;
            }
        }

        public bool IsErrorEnabled
        {
            get
            {
                return log.IsErrorEnabled;
            }
        }

        public bool IsFatalEnabled
        {
            get
            {
                return log.IsFatalEnabled;
            }
        }

        #endregion

        #region 删除日志文件
        /// <summary>
        /// 删除日志
        /// </summary>
        public void DeleteLogFile()
        {
            try
            {
                int maxSize = 90;       //默认保留90天的日志

                if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["LogSaveDays"]))
                {
                    string strMaxSize = System.Configuration.ConfigurationManager.AppSettings["LogSaveDays"];
                    int.TryParse(strMaxSize, out maxSize);
                }

                string logFilePath = String.Empty;      //定义日志文件目录变量
                log4net.Appender.IAppender[] appenders = this.log.Logger.Repository.GetAppenders();
                foreach (log4net.Appender.IAppender appender in appenders)
                {
                    if (appender.Name == "FileAppender" && appender.GetType().Name == "RollingFileAppender")
                    {
                        log4net.Appender.RollingFileAppender rollingFileAppender = appender as log4net.Appender.RollingFileAppender;
                        logFilePath = Path.GetDirectoryName(rollingFileAppender.File);     //获取日志文件目录
                        break;
                    }
                }
                if (!String.IsNullOrEmpty(logFilePath))
                {
                    //logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logFilePath);
                    if (Directory.Exists(logFilePath))
                    {
                        DirectoryInfo dir = new DirectoryInfo(logFilePath);
                        FileInfo[] logFiles = dir.GetFiles();
                        if (logFiles != null && logFiles.Length > maxSize)
                        {
                            Dictionary<string, FileInfo> dicFiles = new Dictionary<string, FileInfo>();
                            List<string> lstFileNames = new List<string>();
                            foreach (FileInfo fi in logFiles)
                            {
                                dicFiles.Add(fi.FullName, fi);
                                lstFileNames.Add(fi.FullName);
                            }
                            lstFileNames.Sort();
                            for (int i = 0; i < lstFileNames.Count - maxSize; i++)
                            {
                                dicFiles[lstFileNames[i]].Delete();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Error("删除日志文件发生错误:" + ex.Message);
            }
        }

        #endregion
    }
}
