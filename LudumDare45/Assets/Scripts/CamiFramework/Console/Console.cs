using CamiFramwork.TypeConvertion;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace CamiFramwork.ConsoleUtil
{
    public class Console : SingletonBehavior<Console>
    {
	    void Start()
	    {
            Converters = new TypeConverterCollection(
                new StringToIntConverter(),
                new StringToFloatConverter(),
                new StringToBoolConverter());

            AddDefaultCommands();
        }
		
        private void AddDefaultCommands()
        {
            AddCommand("ListCommands", new ActionCommand(ListCommands, "Lists all commands"));
            AddAlt("lc", "ListCommands");
        }

        public void ProcessLine(string line)
        {
            tokens.Clear();
            StringHelper.Tokenise(line, " ", tokens);

            if (tokens.Count == 0)
                return;

            string commandKey = tokens[0].ToLower();
            if (!commands.ContainsKey(commandKey))
            {
                Log("No command \"{0}\" exists", commandKey);
                return;
            }

            IConsoleCommand command = commands[commandKey];
            command.Execute(this, tokens);
        }

        public void AddCommand(string key, IConsoleCommand command)
        {
            key = key.ToLower();
            commands[key] = command;
        }

        public void AddCommand(MethodInfo method, object instance, TypeConverterCollection converterCollection, string description)
        {
            string key = method.Name.ToLower();
            AddCommand(key, method, instance, converterCollection, description);
        }

        public void AddCommand(string key, MethodInfo method, object instance, TypeConverterCollection converterCollection, string description)
        {
            AddCommand(key, new MethodCommand(method, instance, converterCollection, description));
        }

        public void AddCommand(string key, object instance, TypeConverterCollection converterCollection, string description)
        {
            MethodInfo method = instance.GetType().GetMethod(key);
            if (method == null)
            {
                Log("No method named \"{0}\" found.", key);
                return;
            }
            
            AddCommand(key, method, instance, converterCollection, description);
        }

        public void AddCommand(string key, object instance, string description, string methodName = null)
        {
            if(methodName == null)
                methodName = key;

            MethodInfo method = instance.GetType().GetMethod(methodName);
            if (method == null)
            {
                Log("No method named \"{0}\" found.", methodName);
                return;
            }
            
            AddCommand(key, method, instance, Converters, description);
        }

        public void RemoveCommand(string key)
        {
            key = key.ToLower();

            IConsoleCommand command;
            if (commands.TryGetValue(key, out command) == false)
                return;

            commands.Remove(key);
        }

        public void AddAlt(string altKey, string key)
        {
            key = key.ToLower();
            altKey = altKey.ToLower();

            if (!commands.ContainsKey(key))
                return;

            commands[altKey] = commands[key];
        }
    
        public void ListCommands()
        {
            Log("-----------------------------------------");
            Log("-- Commands  ----------------------------");
            Log("-----------------------------------------");

            foreach(KeyValuePair<string, IConsoleCommand> kvp in commands)
            {
                Log("  {0}", kvp.Key);

                if(kvp.Value.Description != null)
                {
                    Log("    - {0}", kvp.Value.Description);
                }
            }

            Log("-----------------------------------------");
        }

        public static void Log(string text)
        {
            instance.LogInternal(text, LogTypes.Info);
        }

        public static void Log(string format, params object[] args)
        {
            instance.LogInternal(format, LogTypes.Info, args);
        }
        
        public static void LogWarning(string text)
        {
            instance.LogInternal(text, LogTypes.Warning);
        }

        public static void LogWarning(string format, params object[] args)
        {
            instance.LogInternal(format, LogTypes.Warning, args);
        }
        
        public static void LogError(string text)
        {
            instance.LogInternal(text, LogTypes.Error);
        }

        public static void LogError(string format, params object[] args)
        {
            instance.LogInternal(format, LogTypes.Error, args);
        }

        private void LogInternal(string text, LogTypes logType)
        {
            lock(this)
            {
                Line line = GetFreeLine(logType);
                line.Buffer.Append(text);
                AddLine(line);
            }
        }

        private void LogInternal(string format, LogTypes logType, params object[] args)
        {
            lock(this)
            {
                Line line = GetFreeLine(logType);
                line.Buffer.AppendFormat(format, args);
                AddLine(line);
            }
        }

        private void AppendColorTag(Line line)
        {
            switch (line.LogType)
            {
                case LogTypes.Warning:
                    line.Buffer.Append("<color=orange>");
                    break;
                case LogTypes.Error:
                    line.Buffer.Append("<color=red>");
                    break;
            }
        }

        private void AddLine(Line line)
        {
            if(line.LogType != LogTypes.Info)
                line.Buffer.Append("</color>");

            if (OutputToDebugLog)
            {
                switch (line.LogType)
                {
                    case LogTypes.Info:
                        Debug.Log(line.Buffer.ToString());
                        break;
                    case LogTypes.Warning:
                        Debug.LogWarning(line.Buffer.ToString());
                        break;
                    case LogTypes.Error:
                        Debug.LogError(line.Buffer.ToString());
                        break;
                }
            }
                
            line.Buffer.AppendLine();

            if (OnLineAdded != null)
                OnLineAdded();
        }

        private Line GetFreeLine(LogTypes logType)
        {
            Line line;
            if(lineCount >= MaxLines)
            {
                line = first;
                first = first.Next;
                line.Next = null;
            }
            else // lineCount < MaxLines
            {
                line = new Line();
                ++lineCount;
            }
            if (first == null)
            {
                first = line;
                last = line;
            }

            if(line != first)
                last.Next = line;

            last = line;
            
            line.LogType = logType;
            line.Buffer.Length = 0;
            AppendColorTag(line);
            return line;
        }

        #region Properties
        public TypeConverterCollection Converters { get; set; }

        public int LineCount { get { return lineCount; } }

        public StringBuilder LastLine { get { return last != null ? last.Buffer : null; } }

        public IEnumerable<StringBuilder> Lines
        {
            get
            {
                Line current = first;

                int iter = 0;
                while(current != null && iter < MaxLines)
                {
                    yield return current.Buffer;

                    current = current.Next;
                    ++iter;
                }
            }
        }
        #endregion Properties

        #region Fields
        private Dictionary<string, IConsoleCommand> commands = new Dictionary<string, IConsoleCommand>();

        private List<string> tokens = new List<string>();

        public int MaxLines = 100;
        public bool OutputToDebugLog = true;
        private Line first;
        private Line last;
        private int lineCount;
        #endregion Fields

        #region Events
        public delegate void ConsoleEventHandler();

        public event ConsoleEventHandler OnLineAdded;
        #endregion Events
        
        enum LogTypes
        {
            Info,
            Warning,
            Error
        }

        class Line
        {
            public Line Next;
            public StringBuilder Buffer = new StringBuilder();
            public LogTypes LogType;
        }
    }
}