namespace FluentCli
{
    /// <summary>
    /// The context of handling an argument.
    /// </summary>
    public struct OptionContext
    {
        /// <summary>
        /// The argument name.
        /// </summary>
        public string Argument { get; set; }

        /// <summary>
        /// The value of the argument.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>A <see cref="T:System.String" /> containing a fully qualified type name.</returns>
        public override string ToString()
        {
            return $"{Argument} = {Value}";
        }
    }
}
