using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;


namespace InsPlanIcdEditApi.Filters
{
    public abstract class LoggableClass
    {
        /// <summary>
        /// Constructor for the LoggableClass.
        /// </summary>
        /// <param name="loggerName">The name of the logger to use.</param>
        /// <param name="className">The name of the class that will be writing logs.</param>
        public LoggableClass(string loggerName, string className)
        {
            Logger = LogManager.GetLogger(loggerName);
            LoggerName = loggerName;
            ClassName = className;
        }
        private Logger Logger = null;
        private string LoggerName = "";
        public string ClassName = string.Empty;        

        /// <summary>
        /// Is DebugLog enabled?
        /// </summary>
        public bool DebugLogIsEnabled
        {
            get
            {
                return Logger.IsDebugEnabled;
            }
        }
        /// <summary>
        /// Is InfoLog enabled?
        /// </summary>
        public bool InfoLogIsEnabled
        {
            get
            {
                return Logger.IsInfoEnabled;
            }
        }
        /// <summary>
        /// Is ErrorLog enabled?
        /// </summary>
        public bool ErrorLogIsEnabled
        {
            get
            {
                return Logger.IsErrorEnabled;
            }
        }

        #region InfoLog Overloads
        /// <summary>
        /// Write a log message with a log level of INFO.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        public void InfoLog(string message, string methodName)
        {
            if (Logger.IsInfoEnabled)
                Log(LogLevel.Info, message, methodName, null);
        }
        /// <summary>
        /// Write a log message with a log level of INFO.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void InfoLog(string message, string methodName, string propertyName1, object propertyValue1)
        {
            if (Logger.IsInfoEnabled)
                Log(LogLevel.Info, message, methodName, null, propertyName1, propertyValue1);
        }
        /// <summary>
        /// Write a log message with a log level of INFO.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void InfoLog(string message, string methodName, string propertyName1, object propertyValue1, string propertyName2, object propertyValue2)
        {
            if (Logger.IsInfoEnabled)
                Log(LogLevel.Info, message, methodName, null, propertyName1, propertyValue1, propertyName2, propertyValue2);
        }
        /// <summary>
        /// Write a log message with a log level of INFO.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void InfoLog(string message, string methodName, string propertyName1, object propertyValue1, string propertyName2, object propertyValue2, string propertyName3, object propertyValue3)
        {
            if (Logger.IsInfoEnabled)
                Log(LogLevel.Info, message, methodName, null, propertyName1, propertyValue1, propertyName2, propertyValue2, propertyName3, propertyValue3);
        }
        /// <summary>
        /// Write a log message with a log level of INFO.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void InfoLog(string message, string methodName, string propertyName1, object propertyValue1, string propertyName2, object propertyValue2, string propertyName3, object propertyValue3,
            string propertyName4, object propertyValue4)
        {
            if (Logger.IsInfoEnabled)
                Log(LogLevel.Info, message, methodName, null, propertyName1, propertyValue1, propertyName2, propertyValue2, propertyName3, propertyValue3, propertyName4, propertyValue4);
        }
        /// <summary>
        /// Write a log message with a log level of INFO.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void InfoLog(string message, string methodName, string propertyName1, object propertyValue1, string propertyName2, object propertyValue2, string propertyName3, object propertyValue3,
            string propertyName4, object propertyValue4, string propertyName5, object propertyValue5)
        {
            if (Logger.IsInfoEnabled)
                Log(LogLevel.Info, message, methodName, null, propertyName1, propertyValue1, propertyName2, propertyValue2, propertyName3, propertyValue3, propertyName4, propertyValue4,
                    propertyName5, propertyValue5);
        }
        /// <summary>
        /// Write a log message with a log level of INFO.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void InfoLog(string message, string methodName, string propertyName1, object propertyValue1, string propertyName2, object propertyValue2, string propertyName3, object propertyValue3,
            string propertyName4, object propertyValue4, string propertyName5, object propertyValue5, string propertyName6, object propertyValue6)
        {
            if (Logger.IsInfoEnabled)
                Log(LogLevel.Info, message, methodName, null, propertyName1, propertyValue1, propertyName2, propertyValue2, propertyName3, propertyValue3, propertyName4, propertyValue4,
                    propertyName5, propertyValue5, propertyName6, propertyValue6);
        }
        /// <summary>
        /// Write a log message with a log level of INFO.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void InfoLog(string message, string methodName, string propertyName1, object propertyValue1, string propertyName2, object propertyValue2, string propertyName3, object propertyValue3,
            string propertyName4, object propertyValue4, string propertyName5, object propertyValue5, string propertyName6, object propertyValue6,
            string propertyName7, object propertyValue7)
        {
            if (Logger.IsInfoEnabled)
                Log(LogLevel.Info, message, methodName, null, propertyName1, propertyValue1, propertyName2, propertyValue2, propertyName3, propertyValue3, propertyName4, propertyValue4,
                    propertyName5, propertyValue5, propertyName6, propertyValue6, propertyName7, propertyValue7);
        }
        /// <summary>
        /// Write a log message with a log level of INFO.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void InfoLog(string message, string methodName, string propertyName1, object propertyValue1, string propertyName2, object propertyValue2, string propertyName3, object propertyValue3,
            string propertyName4, object propertyValue4, string propertyName5, object propertyValue5, string propertyName6, object propertyValue6,
            string propertyName7, object propertyValue7, string propertyName8, object propertyValue8)
        {
            if (Logger.IsInfoEnabled)
                Log(LogLevel.Info, message, methodName, null, propertyName1, propertyValue1, propertyName2, propertyValue2, propertyName3, propertyValue3, propertyName4, propertyValue4,
                    propertyName5, propertyValue5, propertyName6, propertyValue6, propertyName7, propertyValue7, propertyName8, propertyValue8);
        }
        /// <summary>
        /// Write a log message with a log level of INFO.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void InfoLog(string message, string methodName, string propertyName1, object propertyValue1, string propertyName2, object propertyValue2, string propertyName3, object propertyValue3,
            string propertyName4, object propertyValue4, string propertyName5, object propertyValue5, string propertyName6, object propertyValue6,
            string propertyName7, object propertyValue7, string propertyName8, object propertyValue8, string propertyName9, object propertyValue9)
        {
            if (Logger.IsInfoEnabled)
                Log(LogLevel.Info, message, methodName, null, propertyName1, propertyValue1, propertyName2, propertyValue2, propertyName3, propertyValue3, propertyName4, propertyValue4,
                    propertyName5, propertyValue5, propertyName6, propertyValue6, propertyName7, propertyValue7, propertyName8, propertyValue8, propertyName9, propertyValue9);
        }
        /// <summary>
        /// Write a log message with a log level of INFO.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void InfoLog(string message, string methodName, string propertyName1, object propertyValue1, string propertyName2, object propertyValue2, string propertyName3, object propertyValue3,
            string propertyName4, object propertyValue4, string propertyName5, object propertyValue5, string propertyName6, object propertyValue6,
            string propertyName7, object propertyValue7, string propertyName8, object propertyValue8, string propertyName9, object propertyValue9,
            string propertyName10, object propertyValue10)
        {
            if (Logger.IsInfoEnabled)
                Log(LogLevel.Info, message, methodName, null, propertyName1, propertyValue1, propertyName2, propertyValue2, propertyName3, propertyValue3, propertyName4, propertyValue4,
                    propertyName5, propertyValue5, propertyName6, propertyValue6, propertyName7, propertyValue7, propertyName8, propertyValue8, propertyName9, propertyValue9,
                    propertyName10, propertyValue10);
        }
        /// <summary>
        /// Write a log message with a log level of INFO.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void InfoLog(string message, string methodName, string propertyName1, object propertyValue1, string propertyName2, object propertyValue2, string propertyName3, object propertyValue3,
            string propertyName4, object propertyValue4, string propertyName5, object propertyValue5, string propertyName6, object propertyValue6,
            string propertyName7, object propertyValue7, string propertyName8, object propertyValue8, string propertyName9, object propertyValue9,
            string propertyName10, object propertyValue10, string propertyName11, object propertyValue11)
        {
            if (Logger.IsInfoEnabled)
                Log(LogLevel.Info, message, methodName, null, propertyName1, propertyValue1, propertyName2, propertyValue2, propertyName3, propertyValue3, propertyName4, propertyValue4,
                    propertyName5, propertyValue5, propertyName6, propertyValue6, propertyName7, propertyValue7, propertyName8, propertyValue8, propertyName9, propertyValue9,
                    propertyName10, propertyValue10, propertyName11, propertyValue11);
        }
        /// <summary>
        /// Write a log message with a log level of INFO.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void InfoLog(string message, string methodName, string propertyName1, object propertyValue1, string propertyName2, object propertyValue2, string propertyName3, object propertyValue3,
            string propertyName4, object propertyValue4, string propertyName5, object propertyValue5, string propertyName6, object propertyValue6,
            string propertyName7, object propertyValue7, string propertyName8, object propertyValue8, string propertyName9, object propertyValue9,
            string propertyName10, object propertyValue10, string propertyName11, object propertyValue11, string propertyName12, object propertyValue12)
        {
            if (Logger.IsInfoEnabled)
                Log(LogLevel.Info, message, methodName, null, propertyName1, propertyValue1, propertyName2, propertyValue2, propertyName3, propertyValue3, propertyName4, propertyValue4,
                    propertyName5, propertyValue5, propertyName6, propertyValue6, propertyName7, propertyValue7, propertyName8, propertyValue8, propertyName9, propertyValue9,
                    propertyName10, propertyValue10, propertyName11, propertyValue11, propertyName12, propertyValue12);
        }

        #endregion

        #region DebugLog Overloads
        /// <summary>
        /// Write a log message with a log level of DEBUG.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        public void DebugLog(string message, string methodName)
        {
            if (Logger.IsDebugEnabled)
                Log(LogLevel.Debug, message, methodName, null);
        }
        /// <summary>
        /// Write a log message with a log level of DEBUG.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void DebugLog(string message, string methodName, string propertyName1, object propertyValue1)
        {
            if (Logger.IsDebugEnabled)
                Log(LogLevel.Debug, message, methodName, null, propertyName1, propertyValue1);
        }
        /// <summary>
        /// Write a log message with a log level of DEBUG.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void DebugLog(string message, string methodName, string propertyName1, object propertyValue1, string propertyName2, object propertyValue2)
        {
            if (Logger.IsDebugEnabled)
                Log(LogLevel.Debug, message, methodName, null, propertyName1, propertyValue1, propertyName2, propertyValue2);
        }
        /// <summary>
        /// Write a log message with a log level of DEBUG.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void DebugLog(string message, string methodName, string propertyName1, object propertyValue1, string propertyName2, object propertyValue2, string propertyName3, object propertyValue3)
        {
            if (Logger.IsDebugEnabled)
                Log(LogLevel.Debug, message, methodName, null, propertyName1, propertyValue1, propertyName2, propertyValue2, propertyName3, propertyValue3);
        }
        /// <summary>
        /// Write a log message with a log level of DEBUG.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void DebugLog(string message, string methodName, string propertyName1, object propertyValue1, string propertyName2, object propertyValue2, string propertyName3, object propertyValue3,
            string propertyName4, object propertyValue4)
        {
            if (Logger.IsDebugEnabled)
                Log(LogLevel.Debug, message, methodName, null, propertyName1, propertyValue1, propertyName2, propertyValue2, propertyName3, propertyValue3, propertyName4, propertyValue4);
        }
        /// <summary>
        /// Write a log message with a log level of DEBUG.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void DebugLog(string message, string methodName, string propertyName1, object propertyValue1, string propertyName2, object propertyValue2, string propertyName3, object propertyValue3,
            string propertyName4, object propertyValue4, string propertyName5, object propertyValue5)
        {
            if (Logger.IsDebugEnabled)
                Log(LogLevel.Debug, message, methodName, null, propertyName1, propertyValue1, propertyName2, propertyValue2, propertyName3, propertyValue3, propertyName4, propertyValue4,
                    propertyName5, propertyValue5);
        }
        /// <summary>
        /// Write a log message with a log level of DEBUG.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void DebugLog(string message, string methodName, string propertyName1, object propertyValue1, string propertyName2, object propertyValue2, string propertyName3, object propertyValue3,
            string propertyName4, object propertyValue4, string propertyName5, object propertyValue5, string propertyName6, object propertyValue6)
        {
            if (Logger.IsDebugEnabled)
                Log(LogLevel.Debug, message, methodName, null, propertyName1, propertyValue1, propertyName2, propertyValue2, propertyName3, propertyValue3, propertyName4, propertyValue4,
                    propertyName5, propertyValue5, propertyName6, propertyValue6);
        }
        /// <summary>
        /// Write a log message with a log level of DEBUG.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void DebugLog(string message, string methodName, string propertyName1, object propertyValue1, string propertyName2, object propertyValue2, string propertyName3, object propertyValue3,
            string propertyName4, object propertyValue4, string propertyName5, object propertyValue5, string propertyName6, object propertyValue6,
            string propertyName7, object propertyValue7)
        {
            if (Logger.IsDebugEnabled)
                Log(LogLevel.Debug, message, methodName, null, propertyName1, propertyValue1, propertyName2, propertyValue2, propertyName3, propertyValue3, propertyName4, propertyValue4,
                    propertyName5, propertyValue5, propertyName6, propertyValue6, propertyName7, propertyValue7);
        }
        /// <summary>
        /// Write a log message with a log level of DEBUG.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void DebugLog(string message, string methodName, string propertyName1, object propertyValue1, string propertyName2, object propertyValue2, string propertyName3, object propertyValue3,
            string propertyName4, object propertyValue4, string propertyName5, object propertyValue5, string propertyName6, object propertyValue6,
            string propertyName7, object propertyValue7, string propertyName8, object propertyValue8)
        {
            if (Logger.IsDebugEnabled)
                Log(LogLevel.Debug, message, methodName, null, propertyName1, propertyValue1, propertyName2, propertyValue2, propertyName3, propertyValue3, propertyName4, propertyValue4,
                    propertyName5, propertyValue5, propertyName6, propertyValue6, propertyName7, propertyValue7, propertyName8, propertyValue8);
        }
        /// <summary>
        /// Write a log message with a log level of DEBUG.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void DebugLog(string message, string methodName, string propertyName1, object propertyValue1, string propertyName2, object propertyValue2, string propertyName3, object propertyValue3,
            string propertyName4, object propertyValue4, string propertyName5, object propertyValue5, string propertyName6, object propertyValue6,
            string propertyName7, object propertyValue7, string propertyName8, object propertyValue8, string propertyName9, object propertyValue9)
        {
            if (Logger.IsDebugEnabled)
                Log(LogLevel.Debug, message, methodName, null, propertyName1, propertyValue1, propertyName2, propertyValue2, propertyName3, propertyValue3, propertyName4, propertyValue4,
                    propertyName5, propertyValue5, propertyName6, propertyValue6, propertyName7, propertyValue7, propertyName8, propertyValue8, propertyName9, propertyValue9);
        }
        /// <summary>
        /// Write a log message with a log level of DEBUG.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void DebugLog(string message, string methodName, string propertyName1, object propertyValue1, string propertyName2, object propertyValue2, string propertyName3, object propertyValue3,
            string propertyName4, object propertyValue4, string propertyName5, object propertyValue5, string propertyName6, object propertyValue6,
            string propertyName7, object propertyValue7, string propertyName8, object propertyValue8, string propertyName9, object propertyValue9,
            string propertyName10, object propertyValue10)
        {
            if (Logger.IsDebugEnabled)
                Log(LogLevel.Debug, message, methodName, null, propertyName1, propertyValue1, propertyName2, propertyValue2, propertyName3, propertyValue3, propertyName4, propertyValue4,
                    propertyName5, propertyValue5, propertyName6, propertyValue6, propertyName7, propertyValue7, propertyName8, propertyValue8, propertyName9, propertyValue9,
                    propertyName10, propertyValue10);
        }
        /// <summary>
        /// Write a log message with a log level of DEBUG.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void DebugLog(string message, string methodName, string propertyName1, object propertyValue1, string propertyName2, object propertyValue2, string propertyName3, object propertyValue3,
            string propertyName4, object propertyValue4, string propertyName5, object propertyValue5, string propertyName6, object propertyValue6,
            string propertyName7, object propertyValue7, string propertyName8, object propertyValue8, string propertyName9, object propertyValue9,
            string propertyName10, object propertyValue10, string propertyName11, object propertyValue11)
        {
            if (Logger.IsDebugEnabled)
                Log(LogLevel.Debug, message, methodName, null, propertyName1, propertyValue1, propertyName2, propertyValue2, propertyName3, propertyValue3, propertyName4, propertyValue4,
                    propertyName5, propertyValue5, propertyName6, propertyValue6, propertyName7, propertyValue7, propertyName8, propertyValue8, propertyName9, propertyValue9,
                    propertyName10, propertyValue10, propertyName11, propertyValue11);
        }
        /// <summary>
        /// Write a log message with a log level of DEBUG.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void DebugLog(string message, string methodName, string propertyName1, object propertyValue1, string propertyName2, object propertyValue2, string propertyName3, object propertyValue3,
            string propertyName4, object propertyValue4, string propertyName5, object propertyValue5, string propertyName6, object propertyValue6,
            string propertyName7, object propertyValue7, string propertyName8, object propertyValue8, string propertyName9, object propertyValue9,
            string propertyName10, object propertyValue10, string propertyName11, object propertyValue11, string propertyName12, object propertyValue12)
        {
            if (Logger.IsDebugEnabled)
                Log(LogLevel.Debug, message, methodName, null, propertyName1, propertyValue1, propertyName2, propertyValue2, propertyName3, propertyValue3, propertyName4, propertyValue4,
                    propertyName5, propertyValue5, propertyName6, propertyValue6, propertyName7, propertyValue7, propertyName8, propertyValue8, propertyName9, propertyValue9,
                    propertyName10, propertyValue10, propertyName11, propertyValue11, propertyName12, propertyValue12);
        }

        #endregion

        #region ErrorLog Overloads
        /// <summary>
        /// Write a log message with a log level of ERROR.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="exc">An exception to write to the logs.</param>
        public void ErrorLog(string message, string methodName, Exception exc)
        {
            if (Logger.IsErrorEnabled)
                Log(LogLevel.Error, message, methodName, exc);
        }
        /// <summary>
        /// Write a log message with a log level of ERROR.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="exc">An exception to write to the logs.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void ErrorLog(string message, string methodName, Exception exc, string propertyName1, object propertyValue1)
        {
            if (Logger.IsErrorEnabled)
                Log(LogLevel.Error, message, methodName, exc, propertyName1, propertyValue1);
        }
        /// <summary>
        /// Write a log message with a log level of ERROR.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="exc">An exception to write to the logs.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void ErrorLog(string message, string methodName, Exception exc, string propertyName1, object propertyValue1, string propertyName2, object propertyValue2)
        {
            if (Logger.IsErrorEnabled)
                Log(LogLevel.Error, message, methodName, exc, propertyName1, propertyValue1, propertyName2, propertyValue2);
        }
        /// <summary>
        /// Write a log message with a log level of ERROR.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="exc">An exception to write to the logs.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void ErrorLog(string message, string methodName, Exception exc, string propertyName1, object propertyValue1, string propertyName2, object propertyValue2, string propertyName3, object propertyValue3)
        {
            if (Logger.IsErrorEnabled)
                Log(LogLevel.Error, message, methodName, exc, propertyName1, propertyValue1, propertyName2, propertyValue2, propertyName3, propertyValue3);
        }
        /// <summary>
        /// Write a log message with a log level of ERROR.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="exc">An exception to write to the logs.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void ErrorLog(string message, string methodName, Exception exc, string propertyName1, object propertyValue1, string propertyName2, object propertyValue2, string propertyName3, object propertyValue3,
            string propertyName4, object propertyValue4)
        {
            if (Logger.IsErrorEnabled)
                Log(LogLevel.Error, message, methodName, exc, propertyName1, propertyValue1, propertyName2, propertyValue2, propertyName3, propertyValue3, propertyName4, propertyValue4);
        }
        /// <summary>
        /// Write a log message with a log level of ERROR.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="exc">An exception to write to the logs.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void ErrorLog(string message, string methodName, Exception exc, string propertyName1, object propertyValue1, string propertyName2, object propertyValue2, string propertyName3, object propertyValue3,
            string propertyName4, object propertyValue4, string propertyName5, object propertyValue5)
        {
            if (Logger.IsErrorEnabled)
                Log(LogLevel.Error, message, methodName, exc, propertyName1, propertyValue1, propertyName2, propertyValue2, propertyName3, propertyValue3, propertyName4, propertyValue4,
                    propertyName5, propertyValue5);
        }
        /// <summary>
        /// Write a log message with a log level of ERROR.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="exc">An exception to write to the logs.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void ErrorLog(string message, string methodName, Exception exc, string propertyName1, object propertyValue1, string propertyName2, object propertyValue2, string propertyName3, object propertyValue3,
            string propertyName4, object propertyValue4, string propertyName5, object propertyValue5, string propertyName6, object propertyValue6)
        {
            if (Logger.IsErrorEnabled)
                Log(LogLevel.Error, message, methodName, exc, propertyName1, propertyValue1, propertyName2, propertyValue2, propertyName3, propertyValue3, propertyName4, propertyValue4,
                    propertyName5, propertyValue5, propertyName6, propertyValue6);
        }
        /// <summary>
        /// Write a log message with a log level of ERROR.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="exc">An exception to write to the logs.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void ErrorLog(string message, string methodName, Exception exc, string propertyName1, object propertyValue1, string propertyName2, object propertyValue2, string propertyName3, object propertyValue3,
            string propertyName4, object propertyValue4, string propertyName5, object propertyValue5, string propertyName6, object propertyValue6,
            string propertyName7, object propertyValue7)
        {
            if (Logger.IsErrorEnabled)
                Log(LogLevel.Error, message, methodName, exc, propertyName1, propertyValue1, propertyName2, propertyValue2, propertyName3, propertyValue3, propertyName4, propertyValue4,
                    propertyName5, propertyValue5, propertyName6, propertyValue6, propertyName7, propertyValue7);
        }
        /// <summary>
        /// Write a log message with a log level of ERROR.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="exc">An exception to write to the logs.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void ErrorLog(string message, string methodName, Exception exc, string propertyName1, object propertyValue1, string propertyName2, object propertyValue2, string propertyName3, object propertyValue3,
            string propertyName4, object propertyValue4, string propertyName5, object propertyValue5, string propertyName6, object propertyValue6,
            string propertyName7, object propertyValue7, string propertyName8, object propertyValue8)
        {
            if (Logger.IsErrorEnabled)
                Log(LogLevel.Error, message, methodName, exc, propertyName1, propertyValue1, propertyName2, propertyValue2, propertyName3, propertyValue3, propertyName4, propertyValue4,
                    propertyName5, propertyValue5, propertyName6, propertyValue6, propertyName7, propertyValue7, propertyName8, propertyValue8);
        }
        /// <summary>
        /// Write a log message with a log level of ERROR.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="exc">An exception to write to the logs.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void ErrorLog(string message, string methodName, Exception exc, string propertyName1, object propertyValue1, string propertyName2, object propertyValue2, string propertyName3, object propertyValue3,
            string propertyName4, object propertyValue4, string propertyName5, object propertyValue5, string propertyName6, object propertyValue6,
            string propertyName7, object propertyValue7, string propertyName8, object propertyValue8, string propertyName9, object propertyValue9)
        {
            if (Logger.IsErrorEnabled)
                Log(LogLevel.Error, message, methodName, exc, propertyName1, propertyValue1, propertyName2, propertyValue2, propertyName3, propertyValue3, propertyName4, propertyValue4,
                    propertyName5, propertyValue5, propertyName6, propertyValue6, propertyName7, propertyValue7, propertyName8, propertyValue8, propertyName9, propertyValue9);
        }
        /// <summary>
        /// Write a log message with a log level of ERROR.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="exc">An exception to write to the logs.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void ErrorLog(string message, string methodName, Exception exc, string propertyName1, object propertyValue1, string propertyName2, object propertyValue2, string propertyName3, object propertyValue3,
            string propertyName4, object propertyValue4, string propertyName5, object propertyValue5, string propertyName6, object propertyValue6,
            string propertyName7, object propertyValue7, string propertyName8, object propertyValue8, string propertyName9, object propertyValue9,
            string propertyName10, object propertyValue10)
        {
            if (Logger.IsErrorEnabled)
                Log(LogLevel.Error, message, methodName, exc, propertyName1, propertyValue1, propertyName2, propertyValue2, propertyName3, propertyValue3, propertyName4, propertyValue4,
                    propertyName5, propertyValue5, propertyName6, propertyValue6, propertyName7, propertyValue7, propertyName8, propertyValue8, propertyName9, propertyValue9,
                    propertyName10, propertyValue10);
        }
        /// <summary>
        /// Write a log message with a log level of ERROR.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="exc">An exception to write to the logs.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void ErrorLog(string message, string methodName, Exception exc, string propertyName1, object propertyValue1, string propertyName2, object propertyValue2, string propertyName3, object propertyValue3,
            string propertyName4, object propertyValue4, string propertyName5, object propertyValue5, string propertyName6, object propertyValue6,
            string propertyName7, object propertyValue7, string propertyName8, object propertyValue8, string propertyName9, object propertyValue9,
            string propertyName10, object propertyValue10, string propertyName11, object propertyValue11)
        {
            if (Logger.IsErrorEnabled)
                Log(LogLevel.Error, message, methodName, exc, propertyName1, propertyValue1, propertyName2, propertyValue2, propertyName3, propertyValue3, propertyName4, propertyValue4,
                    propertyName5, propertyValue5, propertyName6, propertyValue6, propertyName7, propertyValue7, propertyName8, propertyValue8, propertyName9, propertyValue9,
                    propertyName10, propertyValue10, propertyName11, propertyValue11);
        }
        /// <summary>
        /// Write a log message with a log level of ERROR.
        /// </summary>
        /// <param name="message">A message to write to the logs.</param>
        /// <param name="methodName">The name of the calling method.</param>
        /// <param name="exc">An exception to write to the logs.</param>
        /// <param name="propertyName1">A property name to write to the logs.</param>
        /// <param name="propertyValue1">A property value to write to the logs.</param>
        public void ErrorLog(string message, string methodName, Exception exc, string propertyName1, object propertyValue1, string propertyName2, object propertyValue2, string propertyName3, object propertyValue3,
            string propertyName4, object propertyValue4, string propertyName5, object propertyValue5, string propertyName6, object propertyValue6,
            string propertyName7, object propertyValue7, string propertyName8, object propertyValue8, string propertyName9, object propertyValue9,
            string propertyName10, object propertyValue10, string propertyName11, object propertyValue11, string propertyName12, object propertyValue12)
        {
            if (Logger.IsErrorEnabled)
                Log(LogLevel.Error, message, methodName, exc, propertyName1, propertyValue1, propertyName2, propertyValue2, propertyName3, propertyValue3, propertyName4, propertyValue4,
                    propertyName5, propertyValue5, propertyName6, propertyValue6, propertyName7, propertyValue7, propertyName8, propertyValue8, propertyName9, propertyValue9,
                    propertyName10, propertyValue10, propertyName11, propertyValue11, propertyName12, propertyValue12);
        }

        #endregion

        private void Log(LogLevel level, string message, string methodName, Exception exc,
            string propertyName1 = null, object propertyValue1 = null, string propertyName2 = null, object propertyValue2 = null, string propertyName3 = null, object propertyValue3 = null,
            string propertyName4 = null, object propertyValue4 = null, string propertyName5 = null, object propertyValue5 = null, string propertyName6 = null, object propertyValue6 = null,
            string propertyName7 = null, object propertyValue7 = null, string propertyName8 = null, object propertyValue8 = null, string propertyName9 = null, object propertyValue9 = null,
            string propertyName10 = null, object propertyValue10 = null, string propertyName11 = null, object propertyValue11 = null, string propertyName12 = null, object propertyValue12 = null)
        {
            List<object> props = new List<object>();

            props.Add(new { property = propertyName1, value = propertyValue1 });
            props.Add(new { property = propertyName2, value = propertyValue2 });
            props.Add(new { property = propertyName3, value = propertyValue3 });
            props.Add(new { property = propertyName4, value = propertyValue4 });
            props.Add(new { property = propertyName5, value = propertyValue5 });
            props.Add(new { property = propertyName6, value = propertyValue6 });
            props.Add(new { property = propertyName7, value = propertyValue7 });
            props.Add(new { property = propertyName8, value = propertyValue8 });
            props.Add(new { property = propertyName9, value = propertyValue8 });
            props.Add(new { property = propertyName10, value = propertyValue10 });
            props.Add(new { property = propertyName11, value = propertyValue11 });
            props.Add(new { property = propertyName12, value = propertyValue12 });

            LogEventInfo logEv = new LogEventInfo(level, LoggerName, CultureInfo.CurrentCulture, message, props.ToArray());

            Logger.Log(logEv);
        }
    }
}