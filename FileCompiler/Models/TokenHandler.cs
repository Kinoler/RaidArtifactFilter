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
        public int Position;

        public TokenHandler(Token[] tokens)
        {
            _tokens = tokens;
            Position = -1;
        }

        public Token GetNextToken()
        {
            return GetToken(++Position);
        }

        public Token GetCurrentToken()
        {
            return GetToken(Position);
        }

        public Token SeeNextToken()
        {
            return GetToken(Position + 1);
        }

        private Token GetToken(int pos)
        {
            return pos < _tokens.Length ? _tokens[pos] : null;
        }
    }
}
