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
        public override Predicate<TItem> VisitInternal(TokenHandler expression)
        {
            var token = expression.GetNextToken();
            var tokenVisitor = GetVisitor(token, expression);
            var predicate = tokenVisitor.Visit(expression);

            var predicators = new List<List<Predicate<TItem>>>
            {
                new() { predicate },
            };

            token = expression.GetNextToken();
            while (token is { Type: TokenType.LogicOperator })
            {
                var value = token.Value;

                token = expression.GetNextToken();
                tokenVisitor = GetVisitor(token, expression);
                predicate = tokenVisitor.Visit(expression);

                if (value == "and")
                {
                    predicators.Last().Add(predicate);
                }
                
                if (value == "or") 
                {
                    predicators.Add(new List<Predicate<TItem>> { predicate });
                }

                token = expression.GetNextToken();
            }

            if (token != null && token.Type != TokenType.BracketEnd)
                throw new Exception("The ')' expected.");

            return item => predicators.Aggregate(false, (b, list) => b || list.Aggregate(true, (b1, predicate) => b1 && predicate(item)));
        }
    }
}
