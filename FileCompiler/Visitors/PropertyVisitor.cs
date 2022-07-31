using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCompiler.Models;

namespace FileCompiler.Visitors
{
    internal class PropertyVisitor<TItem> : Visitor<TItem>
    {
        public Func<TItem, int> PropertyIntAction { get; }
        public Func<TItem, string> PropertyStringAction { get; }

        public PropertyVisitor(Func<TItem, int> propertyIntAction)
        {
            PropertyIntAction = propertyIntAction;
        }

        public PropertyVisitor(Func<TItem, string> propertyStringAction)
        {
            PropertyStringAction = propertyStringAction;
        }

        public override Predicate<TItem> Visit(TokenHandler expression)
        {
            if (PropertyIntAction != null)
            {
               // return item => PropertyIntAction(item);
            }

            if (PropertyStringAction != null)
            {
               // return item => PropertyStringAction(item);
            }

            return null;
        }
    }
}
