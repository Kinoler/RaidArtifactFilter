using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCompiler.Models
{
    public class TokenHandler
    {
        private readonly Token[] _tokens;
        private int _position;

        public TokenHandler(Token[] tokens)
        {
            _tokens = tokens;
        }

        public Token GetNextToken()
        {
            return GetToken(_position += 1);
        }

        public Token SeeNextToken()
        {
            return GetToken(_position + 1);
        }

        private Token GetToken(int pos)
        {
            return pos < _tokens.Length ? _tokens[pos] : null;
        }
    }
}
