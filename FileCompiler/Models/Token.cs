﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCompiler.Models
{
    public class Token
    {
        public TokenType Type { get; set; }
        public string Value { get; set; }
        public int Position { get; set; }

        public override string ToString()
        {
            return $"{Type}: {Value}";
        }
    }
}
