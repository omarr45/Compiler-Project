using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Token_Class
{
    If, PlusOp
    // TODO: Add Tokens
}
namespace Tiny_Language
{
    public class Token
    {
        public string lex;
        public Token_Class token_type;
    }
    public class Scanner
    {
        public List<Token> Tokens = new List<Token>();
        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();

        public Scanner()
        {
            ReservedWords.Add("if", Token_Class.If);
            // TODO: Add Reserved Words
            Operators.Add("+", Token_Class.PlusOp);
            // TODO: Add Operators
        }

        public void StartScanning(string SourceCode)
        {
            // i: Outer loop to check on lexemes.
            for (int i = 0; i < SourceCode.Length; i++)
            {
                // j: Inner loop to check on each character in a single lexeme.
                int j = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = CurrentChar.ToString();

                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n')
                    continue;

                if (char.IsLetter(CurrentChar))
                {
                    // The possible Token Classes that begin with a character are
                    // an Idenifier or a Reserved Word.

                    // (1) Update the CurrentChar and validate its value.

                    // (2) Iterate to build the rest of the lexeme while satisfying the
                    // conditions on how the Token Classes should be.
                    // (2.1) Append the CurrentChar to CurrentLexeme.
                    // (2.2) Update the CurrentChar.

                    // (3) Call FindTokenClass on the CurrentLexeme.

                    // (4) Update the outer loop pointer (i) to point on the next lexeme.
                }
                else if (char.IsDigit(CurrentChar))
                {

                }
                else if (CurrentChar == '{')
                {

                }
                else
                {
                    Errors.Error_List.Add(CurrentLexeme);
                }
            }

            Tiny_Language.TokenStream = Tokens;
        }

        void FindTokenClass(string Lex)
        {
            Token_Class TC;
            Token Tok = new Token();
            Tok.lex = Lex;
            //Is it a reserved word?

            //Is it an identifier?

            //Is it a Constant?

            //Is it an operator?

            //Is it an undefined?

        }

        bool isIdentifier(string lex)
        {
            bool isValid = true;
            // Check if the lex is an identifier or not.

            return isValid;
        }
        bool isConstant(string lex)
        {
            bool isValid = true;
            // Check if the lex is a constant (Number) or not.

            return isValid;
        }
    }
}
