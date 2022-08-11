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

        public override Predicate<TItem> VisitInternal(TokenHandler expression)
        {
            var token = expression.GetNextToken();
            if (token.Type != TokenType.BracketStart)
                throw new Exception("The '(' expected.");

            token = expression.GetNextToken();
            var argVisitor = GetVisitor(token, expression);
            var argList = new List<Func<TItem, string>> { argVisitor.VisitArgument(expression) };

            token = expression.GetNextToken();
            while (token.Type == TokenType.ArgumentDelimiter)
            {
                token = expression.GetNextToken();
                argVisitor = GetVisitor(token, expression);
                argList.Add(argVisitor.VisitArgument(expression));
                token = expression.GetNextToken();
            }

            if (token.Type != TokenType.BracketEnd)
                throw new Exception("The ')' expected.");

            return item => FilerMultiArgsAction(item, argList);
        }
    }
}
