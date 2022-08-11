using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCompiler.Models;

namespace FileCompiler.Visitors
{
    internal class ArgumentVisitor<TItem> : Visitor<TItem>
    {
        public override Func<TItem, string> VisitArgumentInternal(TokenHandler expression)
        {
            var token = expression.GetCurrentToken();
            return item => token.Value.Replace("\"", "");
        }
    }
}
