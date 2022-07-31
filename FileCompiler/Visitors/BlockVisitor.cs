using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCompiler.Models;

namespace FileCompiler.Visitors
{
    internal class BlockVisitor<TItem> : Visitor<TItem>
    {
        private Predicate<TItem> GetArgument(TokenHandler expression)
        {
            var token = expression.GetNextToken();
            var tokenVisitor = GetVisitor(token, expression);
            var firstPredicate = tokenVisitor.Visit(expression);

            token = expression.SeeNextToken();
            if (token is { Type: TokenType.IfOperator })
            {
                expression.GetNextToken();
                var trueArgAction = GetArgument(expression);

                token = expression.GetNextToken();
                if (token.Type != TokenType.IfOperatorEnd)
                    throw new Exception();

                var falseArgAction = GetArgument(expression);

                return item => firstPredicate(item) ? trueArgAction(item) : falseArgAction(item);
            }

            return firstPredicate;
        }

        public override Predicate<TItem> Visit(TokenHandler expression)
        {
            var predicators = new List<List<Predicate<TItem>>>
            {
                new() {GetArgument(expression)},
            };

            var token = expression.GetNextToken();
            while (token is { Type: TokenType.LogicOperator })
            {
                if (token.Value == "and")
                    predicators.Last().Add(GetArgument(expression));

                if (token.Value == "or")
                    predicators.Add(new List<Predicate<TItem>> { GetArgument(expression) });

                token = expression.GetNextToken();
            }

            if (token != null && token.Type != TokenType.BracketEnd)
                throw new Exception();



            return item => predicators.Aggregate(false, (b, list) => b || list.Aggregate(true, (b1, predicate) => b1 && predicate(item)));
        }
    }
}
