using System;

namespace MMOServer.ConsoleStuff
{
    public class ConsoleCommand
    {
        public string Command { get; private set; }

        public string Usage { get; private set; }

        public Action<ConsoleCommand, string[]> Handler { get; private set; }

        public int ArgumentCount { get; private set; }

        public string Help { get; private set; }

        public ConsoleCommand(string command, string usage, string help, Action<ConsoleCommand, string[]> function, int argumentCount)
        {
            Command = command;
            Usage = usage;
            Help = help;
            Handler = function;
            ArgumentCount = argumentCount;
        }
    }
}
