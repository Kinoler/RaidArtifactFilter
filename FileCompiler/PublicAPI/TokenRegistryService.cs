using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCompiler.Services;
using FileCompiler.Visitors;

namespace FileCompiler.PublicAPI
{
    public class TokenRegistry<TItem>
    {
        public void RegisterFunctionAction(string name, Func<TItem, Func<TItem, string>, bool> filerArgAction)
        {
            TokenRegistryService<TItem>.Instance.RegisterFunctionAction(name, new FunctionSingleArgsVisitor<TItem>(filerArgAction));
        }

        public void RegisterFunctionAction(string name, Func<TItem, List<Func<TItem, string>>, bool> filerMultiArgsAction)
        {
            TokenRegistryService<TItem>.Instance.RegisterFunctionAction(name, new FunctionMultiArgsVisitor<TItem>(filerMultiArgsAction));
        }

        public void RegisterPropertyAction(string name, Func<TItem, string> func)
        {
            TokenRegistryService<TItem>.Instance.RegisterPropertyAction(name, null);
        }

        public void RegisterPropertyAction(string name, Func<TItem, int> func)
        {
            TokenRegistryService<TItem>.Instance.RegisterPropertyAction(name, null);
        }
    }
}
