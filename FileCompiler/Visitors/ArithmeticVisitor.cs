using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCompiler.Models;

namespace FileCompiler.Visitors
{
    internal class ArithmeticVisitor<TItem> : Visitor<TItem>
    {
        internal static Func<TItem, string> FirstArgument { get; set; }

        public override Func<TItem, string> VisitArgumentInternal(TokenHandler expression)
        {
            var firstArgument = FirstArgument;
            var comparisonType = expression.GetCurrentToken().Value;

            var token = expression.GetNextToken();
            var tokenVisitor = GetVisitor(token, expression);
            var secondArgument = tokenVisitor.VisitArgument(expression);
            
            if (comparisonType == "+")
            {
                return item => (int.Parse(firstArgument(item)) + int.Parse(secondArgument(item))).ToString();
            }
            if (comparisonType == "-")
            {
                return item => (int.Parse(firstArgument(item)) - int.Parse(secondArgument(item))).ToString();
            }
            if (comparisonType == "/")
            {
                return item => (int.Parse(firstArgument(item)) / int.Parse(secondArgument(item))).ToString();
            }
            if (comparisonType == "*")
            {
                return item => (int.Parse(firstArgument(item)) * int.Parse(secondArgument(item))).ToString();
            }

            throw new Exception("The '+*-/' expected.");
        }
    }
}
