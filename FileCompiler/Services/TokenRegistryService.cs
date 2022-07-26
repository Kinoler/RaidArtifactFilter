using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCompiler.Models;
using FileCompiler.Visitors;

namespace FileCompiler.Services
{
    internal class TokenRegistryService<TItem>
    {
        private static TokenRegistryService<TItem>? _instance;
        public static TokenRegistryService<TItem> Instance => _instance ??= new TokenRegistryService<TItem>();

        private Dictionary<string, Visitor<TItem>> FunctionKeyWordActions;
        private Dictionary<string, Visitor<TItem>> PropertyKeyWordActions;

        public TokenRegistryService()
        {
            FunctionKeyWordActions = new Dictionary<string, Visitor<TItem>>();
            PropertyKeyWordActions = new Dictionary<string, Visitor<TItem>>();
        }

        public void RegisterFunctionAction(string name, Visitor<TItem> visitor)
        {
            FunctionKeyWordActions.Add(name, visitor);
        }

        public void RegisterPropertyAction(string name, Visitor<TItem> visitor)
        {
            PropertyKeyWordActions.Add(name, visitor);
        }

        internal Visitor<TItem> ResolveFunction(Token token)
        {
            return FunctionKeyWordActions.ContainsKey(token.Value)
                ? FunctionKeyWordActions[token.Value]
                : throw new Exception($"The function {token.Value} does not found. Position: {token.Position}.");
        }

        internal Visitor<TItem> ResolveProperty(Token token)
        {
            return FunctionKeyWordActions.ContainsKey(token.Value)
                ? FunctionKeyWordActions[token.Value]
                : throw new Exception($"The Property {token.Value} does not found. Position: {token.Position}.");
        }
    }
}
