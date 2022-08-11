using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCompiler.Models;

namespace FileCompiler.Visitors
{
    internal class ConditionVisitor<TItem> : Visitor<TItem>
    {
        internal static Predicate<TItem> Condition { get; set; }

        public override Func<TItem, string> VisitArgumentInternal(TokenHandler expression)
        {
            var condition = Condition;

            var token = expression.GetNextToken();
            var tokenVisitor = GetVisitor(token, expression);
            var trueArgAction = tokenVisitor.VisitArgument(expression);

            token = expression.GetNextToken();
            if (token.Type != TokenType.IfOperatorEnd)
                throw new Exception("The ':' expected.");

            token = expression.GetNextToken();
            tokenVisitor = GetVisitor(token, expression);
            var falseArgAction = tokenVisitor.VisitArgument(expression);

            return (item) => condition(item) ? trueArgAction(item) : falseArgAction(item);
        }

        public override Predicate<TItem> VisitInternal(TokenHandler expression)
        {
            var condition = Condition;

            var token = expression.GetNextToken();
            var tokenVisitor = GetVisitor(token, expression);
            var trueArgAction = tokenVisitor.Visit(expression);

            token = expression.GetNextToken();
            if (token.Type != TokenType.IfOperatorEnd)
                throw new Exception("The ':' expected.");

            token = expression.GetNextToken();
            tokenVisitor = GetVisitor(token, expression);
            var falseArgAction = tokenVisitor.Visit(expression);

            return (item) => condition(item) ? trueArgAction(item) : falseArgAction(item);
        }
    }
}
