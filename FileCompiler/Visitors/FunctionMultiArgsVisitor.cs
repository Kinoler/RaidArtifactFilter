using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCompiler.Models;

namespace FileCompiler.Visitors
{
    internal class FunctionMultiArgsVisitor<TItem> : Visitor<TItem>
    {
        public Func<TItem, List<Func<TItem, string>>, bool> FilerMultiArgsAction { get; }

        public FunctionMultiArgsVisitor(Func<TItem, List<Func<TItem, string>>, bool> filerMultiArgsAction)
        {
            FilerMultiArgsAction = filerMultiArgsAction;
        }

        private Func<TItem, string> GetArgument(TokenHandler expression)
        {
            var token = expression.GetNextToken();
            if (token.Type == TokenType.Argument)
                return item => token.Value.Replace("\"", "");

            var tokenVisitor = GetVisitor(token, expression);
            var action = tokenVisitor.Visit(expression);

            token = expression.GetNextToken();
            if (token.Type != TokenType.IfOperator)
                throw new Exception();

            var trueArgAction = GetArgument(expression);

            var token3 = expression.GetNextToken();
            if (token3.Type != TokenType.IfOperatorEnd)
                throw new Exception();
            var falseArgAction = GetArgument(expression);
            return (item) => action(item) ? trueArgAction(item) : falseArgAction(item);
        }

        public override Predicate<TItem> Visit(TokenHandler expression)
        {
            var token = expression.GetNextToken();
            if (token.Type != TokenType.BracketStart)
                throw new Exception();

            var argList = new List<Func<TItem, string>> { GetArgument(expression) };
            token = expression.GetNextToken();
            while (token.Type == TokenType.ArgumentDelimiter)
            {
                argList.Add(GetArgument(expression));
                token = expression.GetNextToken();
            }

            if (token.Type != TokenType.BracketEnd)
                throw new Exception();

            return item => FilerMultiArgsAction(item, argList);
        }
    }
}
