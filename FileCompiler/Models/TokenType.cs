using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCompiler.Models
{
    public enum TokenType
    {
        KeyWord,
        ArithmeticOperator,
        LogicOperator,
        NegateOperator,
        CompareOperator,
        IfOperator,
        IfOperatorEnd,
        ArgumentDelimiter,
        BracketStart,
        BracketEnd,
        Argument,
        Other
    }
}
