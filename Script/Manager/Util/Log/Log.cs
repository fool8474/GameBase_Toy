using UnityEngine;

namespace Script.Manager.Util.Log
{
    public static class Log
    {
        public static LogContent.DebugMessage D(string category)
        {
            return LogContent.DebugMessage.Init(category);
        }

        public static LogContent.InfoMessage I(string category)
        {
            return LogContent.InfoMessage.Init(category);
        }

        public static LogContent.WarnMessage W(string category)
        {
            return LogContent.WarnMessage.Init(category);
        }

        public static LogContent.ErrorMessage E(string category)
        {
            return LogContent.ErrorMessage.Init(category);
        }

        public static LogContent.FatalMessage F(string category)
        {
            return LogContent.FatalMessage.Init(category);
        }

        public static void DF(string category, string format, params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                LogContent.DebugMessage.Init(category)
                    .AppendFormat(format, args)
                    .Print();
            }
            else
            {
                LogContent.DebugMessage.Init(category)
                    .Append(format)
                    .Print();
            }
        }

        public static void IF(string category, string format, params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                LogContent.InfoMessage.Init(category)
                    .AppendFormat(format, args)
                    .Print();
            }
            else
            {
                LogContent.InfoMessage.Init(category)
                    .Append(format)
                    .Print();
            }
        }

        public static void WF(string category, string format, params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                LogContent.WarnMessage.Init(category)
                    .AppendFormat(format, args)
                    .Print();
            }
            else
            {
                LogContent.WarnMessage.Init(category)
                    .Append(format)
                    .Print();
            }
        }

        public static void EF(string category, string format, params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                LogContent.ErrorMessage.Init(category)
                    .AppendFormat(format, args)
                    .Print();
            }
            else
            {
                LogContent.ErrorMessage.Init(category)
                    .Append(format)
                    .Print();
            }
        }

        public static void FF(string category, string format, params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                LogContent.FatalMessage.Init(category)
                    .AppendFormat(format, args)
                    .Print();
            }
            else
            {
                LogContent.FatalMessage.Init(category)
                    .Append(format)
                    .Print();
            }
        }

        public static void Print(LogContent.ILogMessage builder)
        {
            if (builder == null)
            {
                return;
            }

            var message = builder.ToString();
            var logLevel = builder.LogLevel;
            switch (logLevel)
            {
                case LogLevel.Debug:
                    Debug.Log(message);
                    break;
                case LogLevel.Info:
                    Debug.Log(message);
                    break;
                case LogLevel.Warn:
                    Debug.LogWarning(message);
                    break;
                case LogLevel.Error:
                    Debug.LogError(message);
                    break;
                case LogLevel.Fatal:
                    Debug.LogError(message);
                    break;
            }
        }
    }
}
