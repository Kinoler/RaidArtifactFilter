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
        public abstract Predicate<TItem> Visit(TokenHandler expression);

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

            return null;
        }
    }
}
