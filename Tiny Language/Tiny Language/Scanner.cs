using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public enum Token_Class
{
    If, Int , Float , String , Read , Write , Repeat , Until , Elseif , Else , Then , Return , Endl,

    Semicolon, Comma, LParanthesis, RParanthesis, LCurlyBracket, RCurlyBracket,
    EqualOp, LessThanOp, GreaterThanOp, NotEqualOp, AssignmentOp,
    PlusOp, MinusOp, MultiplyOp, DivideOp,
    Idenifier, Constant
    //,Dot
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
            ReservedWords.Add("int", Token_Class.Int);
            ReservedWords.Add("float", Token_Class.Float);
            ReservedWords.Add("string", Token_Class.String);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("write", Token_Class.Write);
            ReservedWords.Add("repeat", Token_Class.Repeat);
            ReservedWords.Add("until", Token_Class.Until);
            ReservedWords.Add("elseif", Token_Class.Elseif);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("return", Token_Class.Return);
            ReservedWords.Add("endl", Token_Class.Endl);
            // TODO: Add Reserved Words
            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add("(", Token_Class.LParanthesis);
            Operators.Add(")", Token_Class.RParanthesis);
            Operators.Add("{", Token_Class.LCurlyBracket);
            Operators.Add("}", Token_Class.RCurlyBracket);
            Operators.Add("=", Token_Class.EqualOp);
            Operators.Add("<", Token_Class.LessThanOp);
            Operators.Add(">", Token_Class.GreaterThanOp);
            Operators.Add("<>", Token_Class.NotEqualOp);
            Operators.Add(":=", Token_Class.AssignmentOp);
            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("/", Token_Class.DivideOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            //Operators.Add(".", Token_Class.Dot);
            // TODO: Add Operators
        }

        public void StartScanning(string SourceCode)
        {
            //ISSUES: thinks that every word in a string is an id, ex. "x y z", thinks that x,y and z are ids
            const string oneCharTokens = "(){};";
            const string whiteSpace = " \r\n";
            string currentLexeme = "";
            for (int i = 0; i < SourceCode.Length; i++)
            {
                char c = SourceCode[i];

                if (whiteSpace.Contains(c) || oneCharTokens.Contains(c))
                {
                    if(currentLexeme.Length > 0)
                    {
                        FindTokenClass(currentLexeme);
                        currentLexeme = "";
                    }

                    if (oneCharTokens.Contains(c))
                        FindTokenClass(c.ToString());

                    continue;
                }

                if (char.IsLetterOrDigit(c))
                {
                    currentLexeme += c;
                }

                //TODO: in case of comments, What is the symbol for comments in tiny?
                //else if (c == '')
                //    while (c != '')
                //        if (i < SourceCode.Length)
                //            i++;
                //        else
                //            break;


                
            }
            
        }

        void FindTokenClass(string Lex)
        {
            Token Tok = new Token();
            Tok.lex = Lex;
            //Is it a reserved word?

            if (ReservedWords.ContainsKey(Lex))
            {
                Tok.token_type = ReservedWords[Lex];
                Tokens.Add(Tok);
            }

            //Is it an identifier?
            else if (isIdentifier(Lex))
            {
                Tok.token_type = Token_Class.Idenifier;
                Tokens.Add(Tok);
            }
            //Is it a Constant?
            else if (isConstant(Lex))
            {
                Tok.token_type = Token_Class.Constant;
                Tokens.Add(Tok);
            }

            //Is it an operator?
            else if (Operators.ContainsKey(Lex))
            {
                Tok.token_type = Operators[Lex];
                Tokens.Add(Tok);
            }

            //TODO: what else? function statments? parameters?

            //Is it an undefined?
            else
            {
                Errors.Error_List.Add(Lex);
            }

        }

        bool isIdentifier(string lex)
        {
            bool isValid = false;
            // Check if the lex is an identifier or not.
            var exp = new Regex(@"^[a-zA-Z][a-zA-Z0-9]*$", RegexOptions.Compiled);
            if (exp.IsMatch(lex))
                isValid = true;
            return isValid;
        }
        bool isConstant(string lex)
        {
            bool isValid = false;
            // Check if the lex is a constant (Number) or not.
            var exp = new  Regex(@"^[0-9](.[0-9]+)*$",RegexOptions.Compiled);
            if (exp.IsMatch(lex))
                isValid = true;
            return isValid;
        }
    }
}
