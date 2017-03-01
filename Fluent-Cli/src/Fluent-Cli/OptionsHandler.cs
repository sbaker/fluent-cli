using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentCli
{
    /// <summary>
    /// Builds handlers for provided CLI arguments.
    /// </summary>
    public class OptionsHandler : IOptionsHandler
    {
        private bool _readLine;

        /// <summary>
        /// A collection of handlers for all the different command line args.
        /// </summary>
        protected List<OptionHandler> Options { get; } = new List<OptionHandler>();
        
        /// <summary>
        /// Creates a new <see cref="IOptionsHandler"/>.
        /// </summary>
        /// <returns></returns>
        public static IOptionsHandler Create(bool readLine = false, bool allowCanceling = true)
        {
            if (allowCanceling)
            {
                Console.CancelKeyPress += (sender, args) => args.Cancel = true;
            }

            var builder = new OptionsHandler();
            return readLine ? builder.ReadLine() : builder;
        }

        /// <summary>
        /// Adds the handler to the collection.
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public IOptionsHandler Option(OptionHandler handler)
        {
            if (string.IsNullOrEmpty(handler?.Argument))
            {
                throw new ArgumentException($"Invalid argument name: {handler?.Argument}", nameof(handler));
            }

            var optionHandler = Options.Find(o => o.Handles(handler.Argument));

            if (optionHandler == null)
            {
                Options.Add(handler);
            }
            else
            {
                optionHandler.Actions.AddRange(handler.Actions);
            }

            return this;
        }

        /// <summary>
        /// Adds a handler for the option.
        /// </summary>
        /// <param name="option"></param>
        /// <param name="helpText"></param>
        /// <param name="handler"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public IOptionsHandler Option(string option, string helpText, Action<OptionContext> handler, string alias = "")
        {
            return Option(option, helpText, false, handler);
        }

        /// <summary>
        /// Adds a handler for the option.
        /// </summary>
        /// <param name="option"></param>
        /// <param name="helpText"></param>
        /// <param name="required"></param>
        /// <param name="handler"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public IOptionsHandler Option(string option, string helpText, bool required, Action<OptionContext> handler, string alias = "")
        {
            var optionHandler = Options.FirstOrDefault(o => o.Handles(option));

            if (optionHandler == null)
            {
                optionHandler = new OptionHandler();
                Options.Add(optionHandler);
                optionHandler.Argument = option;
                optionHandler.Required = required;
                optionHandler.Alias = alias;
                optionHandler.HelpText = helpText;
            }

            optionHandler.Actions.Add(handler);

            return this;
        }

        /// <summary>
        /// Calls the handlers that were registered for the given arguments.
        /// </summary>
        /// <param name="args"></param>
        public void Handle(params string[] args)
        {
            if (args.Length == 0 || IsHelpCommand(args[0]))
            {
                var text = GetHelpText();
                Console.WriteLine(text);
                Console.ReadLine();
                return;
            }

            HandleOptions(args);

            if (_readLine)
            {
                Console.WriteLine("Press enter to exit..");
                Console.ReadLine();
            }
        }

        /// <summary>
        /// Tells the handler to call <see cref="Console.ReadLine"/> after handling arguments.
        /// </summary>
        /// <returns></returns>
        public IOptionsHandler ReadLine()
        {
            _readLine = true;

            return this;
        }

        /// <summary>
        /// Gets the help text.
        /// </summary>
        /// <returns></returns>
        public string GetHelpText()
        {
            const string format = "{0}/{1}\t{2}\t{3}";

            var builder = new StringBuilder()
                .AppendFormat(format, "arg", "alias", "required", "desc")
                .AppendLine();

            foreach (var option in Options)
            {
                builder.AppendFormat(format,
                    option.Argument,
                    option.Alias ?? "n/a",
                    option.Required ? "y\t" : "\t\t",
                    option.HelpText
                ).AppendLine();
            }

            return builder.ToString();
        }

        private void HandleOptions(params string[] args)
        {
            if (AreValidArguments(args.Where(a => a != null && a.StartsWith("-"))))
            {
                var options = Options.Where(o => o.Required && !args.Contains(o.Argument) && !args.Contains(o.Alias)).ToArray();

                if (options.Any())
                {
                    Console.WriteLine($"{string.Join(", ", options.Select(o => o.Argument))} {(options.Length == 1 ? " is" : "are")} required.");
                    return;
                }

                for (var i = 0; i < args.Length; i++)
                {
                    var option = Options.First(o => o.Handles(args[i]));

                    var arg = string.Empty;

                    if (i + 1 < args.Length)
                    {
                        arg = args[++i];
                    }

                   option.Handle(arg);
                }
            }
        }

        private bool AreValidArguments(IEnumerable<string> args)
        {
            var strings = args as string[] ?? args.ToArray();

            if (!strings.All(a => Options.Exists(o => o.Handles(a))))
            {
                Console.WriteLine($"Invalid argument(s): {string.Join(", ", strings.Where(a => !Options.Exists(o => o.Handles(a))))}");
                return false;
            }

            return true;
        }

        private static bool IsHelpCommand(string arg)
        {
            arg = arg?.ToLowerInvariant();
            return string.IsNullOrEmpty(arg) || arg == "-help" || arg == "-h";
        }
    }
}