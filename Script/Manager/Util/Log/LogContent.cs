using System;
using System.Text;

namespace Script.Manager.Util.Log
{
    public static class LogContent
    {
        public interface ILogMessage
        {
            LogLevel LogLevel { get; }
        }

        public abstract class LogMessage<T> : ILogMessage where T : LogMessage<T>, new()
        {
            private const char ValueDeliminator = '.';

            [ThreadStatic] private static T _instance;

            private readonly StringBuilder _builder = new StringBuilder();

            public abstract LogLevel LogLevel { get; }

            internal static T Init(string category)
            {
                if (_instance == null)
                {
                    _instance = new T();
                }

                _instance.SetLogLevel();
                _instance.SetCategory(category);
                return _instance;
            }

            private void SetCategory(string category)
            {                
                if (string.IsNullOrEmpty(category))
                {
                    return;
                }

                _builder.Append("[");
                _builder.Append(category);
                _builder.Append("]");
            }
            
            private void SetLogLevel()
            {
                if (_builder.Length > 0)
                {
                    Clear();
                }

                switch (LogLevel)
                {
                    case LogLevel.Debug:
                        _builder.Append("[D]");
                        break;
                    case LogLevel.Info:
                        _builder.Append("[I]");
                        break;
                    case LogLevel.Warn:
                        _builder.Append("[W]");
                        break;
                    case LogLevel.Error:
                        _builder.Append("[E]");
                        break;
                    case LogLevel.Fatal:
                        _builder.Append("[F]");
                        break;
                }
            }

            private void Clear()
            {
                _builder.Clear();
            }

            public T Append<TValue>(TValue value)
            {
                if (_builder.Length > 0)
                {
                    _builder.Append(" ");
                }

                _builder.Append(value);
                _builder.Append(ValueDeliminator);
                return _instance;
            }

            public T Append<TValue>(string key, TValue value, bool appendDeliminator = true)
            {
                if (_builder.Length > 0)
                {
                    _builder.Append(" ");
                }

                _builder.Append(key);
                _builder.Append("=");
                _builder.Append(value);

                if (appendDeliminator)
                {
                    _builder.Append(ValueDeliminator);
                }

                return _instance;
            }

            public T AppendFormat(string format, params object[] args)
            {
                if (_builder.Length > 0)
                {
                    _builder.Append(" ");
                }

                _builder.AppendFormat(format, args);
                _builder.Append(ValueDeliminator);
                return _instance;
            }

            public T AppendLine()
            {
                _builder.AppendLine();
                return _instance;
            }

            public override string ToString()
            {
                return _builder.ToString();
            }

            protected void PrintInternal()
            {
                Log.Print(this);
                Clear();
            }
        }

        public class DebugMessage : LogMessage<DebugMessage>
        {
            public override LogLevel LogLevel => LogLevel.Debug;

            public void Print()
            {
                PrintInternal();
            }
        }

        public class InfoMessage : LogMessage<InfoMessage>
        {
            public override LogLevel LogLevel => LogLevel.Info;

            public void Print()
            {
                PrintInternal();
            }
        }

        public class WarnMessage : LogMessage<WarnMessage>
        {
            public override LogLevel LogLevel => LogLevel.Warn;

            public void Print()
            {
                PrintInternal();
            }
        }

        public class ErrorMessage : LogMessage<ErrorMessage>
        {
            public override LogLevel LogLevel => LogLevel.Error;

            public void Print()
            {
                PrintInternal();
            }
        }

        public class FatalMessage : LogMessage<FatalMessage>
        {
            public override LogLevel LogLevel => LogLevel.Fatal;

            public void Print()
            {
                PrintInternal();
            }
        }
    }
}