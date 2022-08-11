using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCompiler.Models;

namespace FileCompiler.Visitors
{
    internal class NegateVisitor<TItem> : Visitor<TItem>
    {
        public override Predicate<TItem> VisitInternal(TokenHandler expression)
        {
            var token = expression.GetNextToken();
            if (token != null && token.Type != TokenType.KeyWord &&  token.Type != TokenType.BracketStart)
                throw new Exception($"The {token.Value} is not a keyword. Position: {token.Position}");

            var tokenVisitor = GetVisitor(token, expression);
            var action = tokenVisitor.Visit(expression);

            return item => !action(item);
        }
    }
}
