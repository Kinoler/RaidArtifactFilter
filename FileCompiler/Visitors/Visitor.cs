using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCompiler.Models;
using FileCompiler.Services;

namespace FileCompiler.Visitors
{

    internal abstract class Visitor<TItem>
    {
        private Visitor<TItem> GetPostActionVisitor(TokenHandler expression, Predicate<TItem> result)
        {
            var token = expression.SeeNextToken();
            if (token?.Type == TokenType.IfOperator)
            {
                token = expression.GetNextToken();
                var tokenVisitor = GetVisitor(token, expression);

                ConditionVisitor<TItem>.Condition = result;

                return tokenVisitor;
            }

            if (token?.Type == TokenType.ComparisonOperator)
            {
                token = expression.GetNextToken();
                var tokenVisitor = GetVisitor(token, expression);

                ComparisonVisitor<TItem>.FirstArgument = item => result(item).ToString();

                return tokenVisitor;
            }

            return null;
        }

        private Visitor<TItem> GetPostActionVisitorArgument(TokenHandler expression, Func<TItem, string> result)
        {
            var token = expression.SeeNextToken();

            if (token?.Type == TokenType.ComparisonOperator)
            {
                token = expression.GetNextToken();
                var tokenVisitor = GetVisitor(token, expression);

                ComparisonVisitor<TItem>.FirstArgument = result;

                return tokenVisitor;
            }

            if (token?.Type == TokenType.ArithmeticOperator)
            {
                token = expression.GetNextToken();
                var tokenVisitor = GetVisitor(token, expression);

                ArithmeticVisitor<TItem>.FirstArgument = result;

                return tokenVisitor;
            }

            return null;
        }

        public Predicate<TItem> Visit(TokenHandler expression)
        {
            var result = VisitInternal(expression);

            return GetPostActionVisitor(expression, result)?.Visit(expression) ?? result;
        }


        public virtual Predicate<TItem> VisitInternal(TokenHandler expression)
        {
            var result = VisitArgumentInternal(expression);

            return GetPostActionVisitorArgument(expression, result)?.Visit(expression);
        }

        public Func<TItem, string> VisitArgument(TokenHandler expression)
        {
            var result = VisitArgumentInternal(expression);

            return GetPostActionVisitorArgument(expression, result)?.VisitArgument(expression) ?? result;
        }

        public virtual Func<TItem, string> VisitArgumentInternal(TokenHandler expression)
        {
            var result = VisitInternal(expression);

            return GetPostActionVisitor(expression, result)?.VisitArgument(expression) ?? (item => result(item).ToString());
        }

        public Visitor<TItem> GetVisitor(Token token, TokenHandler expression)
        {
            if (token.Type == TokenType.KeyWord)
            {
                return expression.SeeNextToken().Type == TokenType.BracketStart 
                    ? TokenRegistryService<TItem>.Instance.ResolveFunction(token)
                    : TokenRegistryService<TItem>.Instance.ResolveProperty(token);
            }

            if (token.Type == TokenType.BracketStart)
            {
                return new BlockVisitor<TItem>();
            }

            if (token.Type == TokenType.NegateOperator)
            {
                return new NegateVisitor<TItem>();
            }

            if (token.Type == TokenType.Argument)
            {
                return new ArgumentVisitor<TItem>();
            }

            if (token.Type == TokenType.IfOperator)
            {
                return new ConditionVisitor<TItem>();
            }

            if (token.Type == TokenType.ComparisonOperator)
            {
                return new ComparisonVisitor<TItem>();
            }

            if (token.Type == TokenType.ArithmeticOperator)
            {
                return new ArithmeticVisitor<TItem>();
            }

            return null;
        }
    }
}
