using System;
using System.Collections.Generic;

namespace FluentCli
{
    /// <summary>
    /// Handles command line arguments
    /// </summary>
    public class OptionHandler
    {
        /// <summary>
        /// The argument name.
        /// </summary>
        public string Argument { get; set; }

        /// <summary>
        /// The argument's alias.
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Whether or not the agrument must be provided.
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// A collection of handlers to call for the given argument.
        /// </summary>
        public List<Action<OptionContext>> Actions { get; } = new List<Action<OptionContext>>();

        /// <summary>
        /// Text that is displayed when the user requests help.
        /// </summary>
        public string HelpText { get; set; }

        /// <summary>
        /// Calls all the handlers.
        /// </summary>
        /// <param name="argument"></param>
        public void Handle(string argument)
        {
            var context = new OptionContext
            {
                Argument = Argument,
                Value = argument
            };

            foreach (var action in Actions)
            {
                action(context);
            }
        }

        /// <summary>
        /// Determines if this handler handles the particular argument.
        /// </summary>
        /// <param name="argument"></param>
        public bool Handles(string argument)
        {
            return string.Equals(Argument, argument, StringComparison.CurrentCultureIgnoreCase)
                || string.Equals(Alias, argument, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}