using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCompiler.Visitors
{
    internal class FunctionSingleArgsVisitor<TItem> : FunctionMultiArgsVisitor<TItem>
    {
        public FunctionSingleArgsVisitor(Func<TItem, Func<TItem, string>, bool> filerArgAction) :
            base((item, list) => filerArgAction(item, list.Single()))
        { }
    }
}
