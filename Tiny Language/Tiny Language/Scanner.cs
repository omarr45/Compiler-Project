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
    EqualOp, LessThanOp, GreaterThanOp, NotEqualOp, AssignmentOp, AndOp, OrOp,
    PlusOp, MinusOp, MultiplyOp, DivideOp,
    Idenifier, Constant
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
            Operators.Add("&&", Token_Class.AndOp);
            Operators.Add("||", Token_Class.OrOp);
            // TODO: Add Operators
        }

        public void StartScanning(string SourceCode)
        {
            const string whiteSpace = " \r\n\t";
            string currentLexeme = "";
            for (int i = 0; i < SourceCode.Length; i++)
            {
                char c = SourceCode[i];

                if (whiteSpace.Contains(c))
                {
                    if (currentLexeme.Length > 0)
                    {
                        FindTokenClass(currentLexeme);
                        currentLexeme = "";
                    }
                    continue;
                }
                else if (c == '"')
                {
                    currentLexeme += c;
                    char string_char = SourceCode[++i];
                    while (string_char != '"')
                    {
                        currentLexeme += string_char;
                        string_char = SourceCode[++i];
                    }
                    currentLexeme += c;
                    FindTokenClass(currentLexeme);
                    currentLexeme = "";
                }
                else if (i < SourceCode.Length -1 && SourceCode.Substring(i, 2) == "/*")
                {
                    while (i < SourceCode.Length -1 && SourceCode.Substring(i, 2) != "*/")
                        i+=2;
                    i++;
                }
                else if (!char.IsLetterOrDigit(c))
                {
                    if (currentLexeme.Length > 0)
                    {
                        if(!char.IsLetterOrDigit(currentLexeme[currentLexeme.Length - 1]))
                        {
                            currentLexeme += c;
                        }
                        else
                        {
                            FindTokenClass(currentLexeme);
                            currentLexeme = c.ToString();
                        }
                    }
                    else
                        currentLexeme += c;
                }
                else
                {
                    if (currentLexeme.Length > 0)
                    {
                        if (currentLexeme.Length > 0 && char.IsLetterOrDigit(currentLexeme[currentLexeme.Length - 1]))
                        {
                            currentLexeme += c;
                        }
                        else
                        {
                            FindTokenClass(currentLexeme);
                            currentLexeme = c.ToString();
                        }
                    }
                    else
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
            FindTokenClass(currentLexeme);
        }

        void FindTokenClass(string Lex)
        {
            Token Tok = new Token();
            Tok.lex = Lex;
            
            if (ReservedWords.ContainsKey(Lex))
            {
                Tok.token_type = ReservedWords[Lex];
                Tokens.Add(Tok);
            }
            else if (isIdentifier(Lex))
            {
                Tok.token_type = Token_Class.Idenifier;
                Tokens.Add(Tok);
            }
            else if (isConstant(Lex))
            {
                Tok.token_type = Token_Class.Constant;
                Tokens.Add(Tok);
            }
            else if(isString(Lex))
            {
                Tok.token_type = Token_Class.String;
                Tokens.Add(Tok);
            }
            else if (Operators.ContainsKey(Lex))
            {
                Tok.token_type = Operators[Lex];
                Tokens.Add(Tok);
            }
            else if(Lex.Length > 0 && !char.IsLetterOrDigit(Lex[Lex.Length - 1]))
            {
                foreach(char c in Lex)
                {
                    string singleSymbol = c.ToString();
                    if (Operators.ContainsKey(singleSymbol))
                    {
                        Tok = new Token();
                        Tok.token_type = Operators[singleSymbol];
                        Tok.lex = singleSymbol;
                        Tokens.Add(Tok);
                    }
                    else    
                        Errors.Error_List.Add(singleSymbol);
                }
            }
            else
            {
                Errors.Error_List.Add(Lex);
            }

        }

        private bool isString(string lex)
        {
            var exp = new Regex("^\".*\"$");
            return exp.IsMatch(lex);
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
