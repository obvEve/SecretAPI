namespace SecretAPI.Features.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using CommandSystem;
    using LabApi.Features.Wrappers;
    using NorthwoodLib.Pools;
    using SecretAPI.Attribute;
    using SecretAPI.Features.Commands.Parsing;

    /// <summary>
    /// Handles parsing <see cref="CustomCommand"/>.
    /// </summary>
    public static class CustomCommandHandler
    {
        /// <summary>
        /// The name of the <see cref="Player"/> or <see cref="ReferenceHub"/> argument represensing the command sender.
        /// </summary>
        public const string SenderPlayerName = "sender";

        private static Dictionary<CustomCommand, MethodInfo[]> commandExecuteMethods = new();

        /// <summary>
        /// Attempts to call the correct command and gives a result.
        /// </summary>
        /// <param name="command">The command currently being called from.</param>
        /// <param name="sender">The sender of the command.</param>
        /// <param name="arguments">The arguments provided to the command.</param>
        /// <param name="response">The response to give to the player.</param>
        /// <returns>Whether the command was a success.</returns>
        public static bool TryCall(CustomCommand command, ICommandSender sender, ArraySegment<string> arguments, out string response)
        {
            Player senderPlayer = Player.Get(sender) ?? Server.Host!;

            CommandParseResult parseParseResult = TryParse(command, senderPlayer, arguments);
            if (!parseParseResult.CouldParse)
            {
                response = parseParseResult.FailedResponse;
                return false;
            }

            parseParseResult.Method.Invoke(null, parseParseResult.ProvidedArguments);

            // TODO: get result & put it into response
            return true;
        }

        private static CommandParseResult TryParse(CustomCommand command, Player sender, ArraySegment<string> arguments)
        {
            foreach (MethodInfo method in GetMethods(command))
            {
                CommandMethodParseResult result = ValidateAllMethodParameters(method, sender, arguments);

                // parsed correctly, return with correct arguments
                if (result.CouldParse)
                {
                    return new CommandParseResult()
                    {
                        CouldParse = true,
                        FailedResponse = string.Empty,
                        Method = method,
                        ProvidedArguments = result.Arguments,
                    };
                }

                // failed to parse, return and show failure
                return new CommandParseResult()
                {
                    CouldParse = false,
                    FailedResponse = result.FailedResponse,
                };
            }
        }

        private static CommandMethodParseResult ValidateAllMethodParameters(MethodInfo method, Player sender, ArraySegment<string> arguments)
        {
            ParameterInfo[] parameters = method.GetParameters();
            List<object> returns = ListPool<object>.Shared.Rent();

            for (int index = 0; index < parameters.Length; index++)
            {
                ParameterInfo parameter = parameters[index];
                CommandArgParseResult validateResult = ValidateParameter(parameter, sender, arguments.ElementAtOrDefault(index));
                if (!validateResult.CouldParse)
                {
                    return new CommandMethodParseResult()
                    {
                        CouldParse = false,
                        FailedResponse = validateResult.FailedResponse,
                    };
                }

                returns.Add(validateResult.ParamArgument);
            }

            CommandMethodParseResult result = new()
            {
                CouldParse = true,
                Arguments = returns.ToArray(),
            };

            ListPool<object>.Shared.Return(returns);
            return result;
        }

        private static CommandArgParseResult ValidateParameter(ParameterInfo parameter, Player sender, string? argument)
        {
            // if arg doesnt exist & param is optional, then its validated
            if (argument == null && parameter.IsOptional)
            {
                return new CommandArgParseResult()
                {
                    CouldParse = true,
                    ParamArgument = parameter.DefaultValue!,
                };
            }

            Type type = parameter.ParameterType;

            if (type.IsEnum)
            {
                if (Enum.TryParse(type, argument, true, out object? enumValue))
                {
                    return new CommandArgParseResult()
                    {
                        CouldParse = true,
                        ParamArgument = enumValue,
                    };
                }

                return new CommandArgParseResult()
                {
                    CouldParse = false,
                    FailedResponse = $"Could not pass into valid enum value. Enum required: {type.Name}.",
                };
            }

            if (parameter.Name == SenderPlayerName)
            {
                if (typeof(Player).IsAssignableFrom(parameter.ParameterType))
                {
                    return new CommandArgParseResult()
                    {
                        CouldParse = true,
                        ParamArgument = sender,
                    };
                }
                else if (typeof(ReferenceHub).IsAssignableFrom(parameter.ParameterType))
                {
                    return new CommandArgParseResult()
                    {
                        CouldParse = true,
                        ParamArgument = sender.ReferenceHub,
                    };
                }
            }

            // all parsing failed
            return new CommandArgParseResult()
            {
                CouldParse = false,
                FailedResponse = $"Failed to parse {argument ?? null} into type of {parameter.ParameterType.Name}.",
            };
        }

        private static MethodInfo[] GetMethods(CustomCommand command)
        {
            const BindingFlags methodFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

            if (!commandExecuteMethods.TryGetValue(command, out MethodInfo[] methods))
            {
                methods = command.GetType().GetMethods(methodFlags).Where(m => m.GetCustomAttribute<ExecuteCommandAttribute>() != null).ToArray();
                commandExecuteMethods.Add(command, methods);
            }

            return methods;
        }
    }
}