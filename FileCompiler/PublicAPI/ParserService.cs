using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCompiler.Analyzers;

namespace FileCompiler.PublicAPI
{
    public class ParserService
    {
        public static Predicate<TItem> GenerateItemFilter<TItem>(string str)
        {
            var lexicalAnalyzer = new LexicalAnalyzer();
            var tokenHandler = lexicalAnalyzer.Analyze(str);

            var syntacticAnalyzer = new SyntacticAnalyzer();
            return syntacticAnalyzer.Analyze<TItem>(tokenHandler);
        }
    }
}
