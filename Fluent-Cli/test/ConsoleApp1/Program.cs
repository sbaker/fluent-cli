using System;
using FluentCli;

namespace ConsoleApp1
{
    public static class Program
    {
        private static readonly IOptionsHandler Builder = OptionsHandler.Create(true);

        public static void Main(string[] args)
        {
            Builder.Option(
                "-test",
                "help text is good",
                context => Console.WriteLine(context)
            )
            .Option(
                "-param",
                "we like parameters",
                true,
                context => Console.WriteLine(context),
                "-p"
            )
            .Handle(args);
        }
    }
}
