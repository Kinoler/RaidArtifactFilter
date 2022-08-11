using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCompiler.Models;
using FileCompiler.Visitors;

namespace FileCompiler.Analyzers
{
    internal class SyntacticAnalyzer
    {
        public Predicate<TItem> Analyze<TItem>(TokenHandler tokenHandler) 
        {
            try
            {
                var generalVisitor = new BlockVisitor<TItem>();
                var result = generalVisitor.Visit(tokenHandler);

                return result;
            }
            catch (Exception e)
            {
                throw new Exception($"{e.Message} Position {tokenHandler.Position}");
            }
        }
    }
}
