using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FileCompiler.Models;

namespace FileCompiler.Analyzers
{
    internal class LexicalAnalyzer
    {
        private readonly string tokenRegex = "(?<Stat>\"[^\"]*\")|" +
                                       "(?<LogicOperator>\\b(?:and|or)\\b)|" +
                                       "(?<KeyWord>\\b[A-Za-z][A-Za-z0-9]*\\b)|" +
                                       "(?<Number>[0-9]+)|" +
                                       "(?<Comment>\\/\\/[^\\n\\r]*)|" +
                                       "(?<IfParts>[?:])|" +
                                       "(?<Bracket>[\\(\\)])|" +
                                       "(?<ArgumentDelimiter>[,])|" +
                                       "(?<NegateOperator>[!])|" +
                                       "(?<ArithmeticOperator>[+\\-\\*/=])";

        public TokenHandler Analyze(string str)
        {
            var regex = new Regex(tokenRegex);
            var matches = regex.Matches(str);

            var tokens = new List<Token>();
            foreach (Match match in matches)
            {
                tokens.Add(new Token()
                {
                    Value = match.Value,
                    Position = match.Index,
                    Type = ParsetokenType(match.Value, regex.GetGroupNames().Skip(1).First(group => match.Groups[group].Captures.Count > 0))
                });
            }

            return new TokenHandler(tokens.Where(token => token.Type != TokenType.Other).ToArray());
        }

        private TokenType ParsetokenType(string value, string groupName)
        {
            if (groupName == "KeyWord")
                return TokenType.KeyWord;
            if (groupName == "ArithmeticOperator")
                return TokenType.ArithmeticOperator;
            if (groupName == "Stat" || groupName == "Number")
                return TokenType.Argument;
            if (groupName == "LogicOperator")
                return TokenType.LogicOperator;
            if (groupName == "NegateOperator")
                return TokenType.NegateOperator;
            if (groupName == "ArgumentDelimiter")
                return TokenType.ArgumentDelimiter;
            if (groupName == "IfParts")
                return value == "?" ? TokenType.IfOperator : TokenType.IfOperatorEnd;
            if (groupName == "Bracket")
                return value == "(" ? TokenType.BracketStart : TokenType.BracketEnd;

            return TokenType.Other;
        }
    }
}
