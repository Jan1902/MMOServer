using MMOServer.Networking;
using System;
using System.Collections.Generic;

namespace MMOServer.ConsoleStuff
{
    class ConsoleManager
    {
        private Dictionary<string, ConsoleCommand> _commands = new Dictionary<string, ConsoleCommand>();

        private bool _quit;
        private GameServer _gameServer;

        public ConsoleManager(GameServer gameServer)
        {
            _gameServer = gameServer;

            RegisterCommand(new ConsoleCommand("stop", "stop", "Shuts down the server", HandleStopCommand, 0));
            RegisterCommand(new ConsoleCommand("help", "help <command>", "Shows the help text for the given command", HandleHelpCommand, 1));
            RegisterCommand(new ConsoleCommand("list", "list <object>", "Lists all the instances of the given object [client, token, player]", HandleListCommand, 1));

            ConsoleUtils.Info("Console Command Handler set up");
            Run();
        }

        private void RegisterCommand(ConsoleCommand command)
        {
            _commands.Add(command.Command, command);
        }

        private void Run()
        {
            string input;
            string[] tokens;

            while (!_quit)
            {
                input = Console.ReadLine();
                tokens = input.Split(' ');
                if (_commands.ContainsKey(tokens[0]))
                {
                    _commands[tokens[0]].Handler(_commands[tokens[0]], tokens);
                }
                else
                {
                    HandleUnknownCommand(tokens[0]);
                }
            }
        }

        private bool HandleUnknownCommand(string command)
        {
            ConsoleUtils.Error("The command '{0}' does not exist", command);
            return true;
        }

        private void HandleStopCommand(ConsoleCommand command, string[] tokens)
        {
            if (tokens.Length - 1 != command.ArgumentCount)
            {
                ConsoleUtils.Error("The given arguments do not fit this command");
                return;
            }
            _quit = true;
            _gameServer.Shutdown();
        }

        private void HandleHelpCommand(ConsoleCommand command, string[] tokens)
        {
            if (tokens.Length - 1 != command.ArgumentCount)
            {
                ConsoleUtils.Error("The given arguments do not fit this command");
                return;
            }

            if (!_commands.ContainsKey(tokens[1]))
            {
                ConsoleUtils.Error("The command '{0}' does not exist", tokens[1]);
                return;
            }

            ConsoleUtils.Info("Usage: {0} ", _commands[tokens[1]].Usage);
            ConsoleUtils.Info("Help: {0}", _commands[tokens[1]].Help);
        }

        private void HandleListCommand(ConsoleCommand command, string[] tokens)
        {
            if (tokens.Length - 1 != command.ArgumentCount)
            {
                ConsoleUtils.Error("The given arguments do not fit this command");
                return;
            }

            switch (tokens[1])
            {
                case "client":
                    if(_gameServer.Connections.Count == 0)
                    {
                        ConsoleUtils.Info("There are no clients connected currently");
                        break;
                    }
                    foreach (ClientConnectionInfo connection in _gameServer.Connections)
                    {
                        ConsoleUtils.Info(connection.Peer.GetRemoteAddress().ToString());
                    }
                    break;
                default:
                    ConsoleUtils.Error("The given type is invalid");
                    break;
            }
        }
    }
}
