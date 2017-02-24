using System;
using System.Collections.Generic;

namespace FluentCli
{
    /// <summary>
    /// The base class for Console Applications.
    /// </summary>
    public class ConsoleApp
    {
        /// <summary>
        /// A collection of handlers for all the different command line args.
        /// </summary>
        protected static Dictionary<string, OptionHandler> Options { get; } = new Dictionary<string, OptionHandler>();

        /// <summary>
        /// The entry pont to the application.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IConsoleApplication
    {
        /// <summary>
        /// 
        /// </summary>
        void Init();
    }

    /// <summary>
    /// Handles command line arguments
    /// </summary>
    public class OptionHandler
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Action<OptionContext>> Actions { get; } = new List<Action<OptionContext>>();
    }

    /// <summary>
    /// The context of handling an argument.
    /// </summary>
    public class OptionContext
    {
        /// <summary>
        /// The argument name.
        /// </summary>
        public string Argument { get; set; }

        /// <summary>
        /// The value of the argument.
        /// </summary>
        public string Value { get; set; }
    }
}
