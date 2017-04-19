using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreDapper.Cache
{
    internal enum Operation
    {
        Insert,
        Update,
        Delete
    }

    internal class CommandKey
    {
        internal readonly Type Type;
        internal readonly Operation Operation;

        public CommandKey(Type type, Operation operation)
        {
            Type = type;
            Operation = operation;
        }

        public override bool Equals(object obj)
        {
            var commandKey = obj as CommandKey;

            if (commandKey == null)
            {
                return false;
            }

            return (commandKey.Operation == Operation) && (commandKey.Type == Type);
        }
    }

    internal class CachedCommand
    {
        internal readonly CommandKey CommandKey;
        internal readonly string Command;

        public CachedCommand(Type type, Operation operation, string command)
        {
            CommandKey = new CommandKey(type, operation);
            Command = command;
        }

        public override bool Equals(object obj)
        {
            var cachedCommand = obj as CachedCommand;

            if (cachedCommand == null)
            {
                return false;
            }

            return cachedCommand.CommandKey.Equals(CommandKey);
        }
    }

    internal static class StringCache
    {
        internal static List<CachedCommand> Cache = new List<CachedCommand>();

        public static void Add(Type type, Operation operation, string command)
        {
            var cachedCommand = new CachedCommand(type, operation, command);
            if (Cache.Any(x => x.Equals(cachedCommand)))
            {
                return;
            }

            Cache.Add(new CachedCommand(type, operation, command));
        }

        public static bool TryGetCommand(Type type, Operation operation, out string command)
        {
            command = null;
            var key = new CommandKey(type, operation);
            var cachedCommand = Cache.FirstOrDefault(x => x.CommandKey.Equals(key));

            if (cachedCommand == null)
            {
                return false;
            }

            command = cachedCommand.Command;

            return true;
        }
    }
}
