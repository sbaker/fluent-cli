using System;

namespace FluentCli
{
    /// <summary>
    /// Provides methods to register handlers for cli arguments.
    /// </summary>
    public interface IOptionsHandler
    {
        /// <summary>
        /// Adds the handler to the collection.
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        IOptionsHandler Option(OptionHandler handler);

        /// <summary>
        /// Adds a handler for the option.
        /// </summary>
        /// <param name="option"></param>
        /// <param name="helpText"></param>
        /// <param name="handler"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        IOptionsHandler Option(string option, string helpText, Action<OptionContext> handler, string alias = "");

        /// <summary>
        /// Adds a handler for the option.
        /// </summary>
        /// <param name="option"></param>
        /// <param name="required"></param>
        /// <param name="helpText"></param>
        /// <param name="handler"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        IOptionsHandler Option(string option, string helpText, bool required, Action<OptionContext> handler, string alias = "");

        /// <summary>
        /// Calls the handlers that were registered for the given arguments.
        /// </summary>
        /// <param name="args"></param>
        void Handle(string[] args);

        /// <summary>
        /// Gets the Help text for printing to the console.
        /// </summary>
        /// <returns></returns>
        string GetHelpText();
    }
}