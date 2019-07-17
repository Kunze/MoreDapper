using System;
using System.Collections.Generic;

namespace MoreDapper.Scanner
{
    internal class SqlParameterScanner
    {
        internal static List<Parameter> Scan(string values)
        {
            if (string.IsNullOrWhiteSpace(values))
            {
                throw new ArgumentNullException("values can not be null or white space.");
            }

            var parameters = new List<Parameter>();
            var parameter = false;
            var insideString = false;
            var text = "";
            var startIndex = 0;
            var delimiters = new List<char>() { ',', '@' };

            for (int i = 0; i < values.Length; i++)
            {
                var character = values[i];
                var lastCharacter = i == values.Length - 1;

                if (lastCharacter)
                {
                    if (delimiters.Contains(character))
                    {
                        throw new Exception($"Last character can not end with {delimiters.ToArray()}");
                    }
                }

                if (character == '@')
                {
                    if (parameter)
                    {
                        throw new Exception("Many parameter identifier");
                    }

                    if (!insideString)
                    {
                        parameter = true;
                        startIndex = i;
                        text += character;
                        continue;
                    }
                }

                if (character == '\'')
                {
                    //<TODO>verificar string até o final
                    insideString = !insideString;
                    continue;
                }

                if (character == ',')
                {
                    if (!insideString && parameter)
                    {
                        parameters.Add(new Parameter(startIndex, i, text.Trim()));
                        text = "";
                        parameter = false;
                    }
                }

                if (parameter)
                {
                    text += character;

                    if (lastCharacter)
                    {
                        parameters.Add(new Parameter(startIndex, i, text));
                    }
                }
            }

            return parameters;
        }
    }
}
