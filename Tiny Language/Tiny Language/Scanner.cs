using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public enum Token_Class
{
    If, Int , Float , String , Read , Write , Repeat , Until , Elseif , Else , Then , Return , Endl, Comment,
    Semicolon, Comma, LParanthesis, RParanthesis, LCurlyBracket, RCurlyBracket,
    EqualOp, LessThanOp, GreaterThanOp, NotEqualOp, AssignmentOp, AndOp, OrOp,
    PlusOp, MinusOp, MultiplyOp, DivideOp,
    Idenifier, Constant
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
            // List of Reserved Words
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
            
            // List of Operators
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
        }

        public void StartScanning(string SourceCode)
        {
            const string whiteSpace = " \r\n\t";

            for (int i = 0; i < SourceCode.Length; i++)
            {
                string currentLexeme = "";
                char currentLetter = SourceCode[i];

                // is it a white space ?
                if(whiteSpace.Contains(currentLetter))
                    continue;

                // is it the start of an identifier or reserverd word ? 
                if(char.IsLetter(currentLetter))
                {
                    // iterate and build up the current lexeme until currentLetter is a white space
                    while (i < SourceCode.Length && char.IsLetterOrDigit(SourceCode[i]))
                    {
                        currentLetter = SourceCode[i++];
                        currentLexeme += currentLetter;
                    }
                    i--;
                }
                // is it the start of a comment ?
                else if (currentLetter == '/' && SourceCode[i + 1] == '*')
                {
                    i += 2;
                    currentLetter = SourceCode[i];
                    currentLexeme += "/*";
                    while (i + 1 < SourceCode.Length && (currentLetter != '*' || SourceCode[i + 1] != '/'))
                    {
                        currentLexeme += currentLetter;
                        currentLetter = SourceCode[++i];
                    }
                    if (i + 1 < SourceCode.Length)
                    {
                        currentLexeme += currentLetter;
                        currentLexeme += SourceCode[++i];
                    }
                }
                // is it the start of a string ? 
                else if(currentLetter == '\"')
                {   
                    currentLexeme += currentLetter;
                    currentLetter = SourceCode[++i];
                    while(i + 1 < SourceCode.Length && currentLetter != '\"')
                    {
                        currentLexeme += currentLetter;
                        currentLetter = SourceCode[++i];
                    }
                    currentLexeme += currentLetter;
                }
                // is it the start of a constant ? 
                else if(char.IsDigit(currentLetter))
                {
                    while(i < SourceCode.Length && (char.IsDigit(SourceCode[i]) || SourceCode[i] == '.'))
                    {
                        currentLetter = SourceCode[i++];
                        currentLexeme += currentLetter;
                    }
                    i--;
                }
                // is it the start of an operator ?
                else
                {
                    currentLexeme += currentLetter;

                    if (i != SourceCode.Length - 1)
                    {
                        // is it a double char operator ? 
                        // in other words, is the lexeme one of those "<>" ":=" "&&" "||" ?

                        char nextLetter = SourceCode[i + 1];
                        if ((currentLetter == '<' && nextLetter == '>') || (currentLetter == ':' && nextLetter == '=')
                            || (currentLetter == '&' && nextLetter == '&') || (currentLetter == '|' && nextLetter == '|'))
                        {
                            currentLexeme += nextLetter;
                            i++;
                        }
                    }

                }
                // call Find TokenClass with the current lexeme
                FindTokenClass(currentLexeme);
            }
            Tiny_Language.TokenStream = Tokens;
            
        }

        void FindTokenClass(string Lex)
        {
            if (Lex == "")
                return;
            Token Tok = new Token();
            Tok.lex = Lex;


            // is the lex a reserved word ? 
            if (ReservedWords.ContainsKey(Lex))
            {
                Tok.token_type = ReservedWords[Lex];
                Tokens.Add(Tok);
            }
            // is the lex an identifier ?
            else if (isIdentifier(Lex))
            {
                Tok.token_type = Token_Class.Idenifier;
                Tokens.Add(Tok);
            }
            // is the lex a constant ?
            else if (isConstant(Lex))
            {
                Tok.token_type = Token_Class.Constant;
                Tokens.Add(Tok);
            }
            // is the lex a string ?
            else if (isString(Lex))
            {
                Tok.token_type = Token_Class.String;
                Tokens.Add(Tok);
            }
            // is the lex an operator ?
            else if (Operators.ContainsKey(Lex))
            {
                Tok.token_type = Operators[Lex];
                Tokens.Add(Tok);
            }
            else if (isComment(Lex))
            {
                Tok.token_type = Token_Class.Comment;
                Tokens.Add(Tok);
            }
            else
            {
                Errors.Error_List.Add("Unrecognized token:\t" + Lex);
            }

        }

        private bool isComment(string lex)
        {
            var exp = new Regex(@"^/\*[\s\S]*\*/$");
            return exp.IsMatch(lex);
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
            var exp = new Regex(@"^[0-9]+(\.[0-9]+)?$",RegexOptions.Compiled);
            if (exp.IsMatch(lex))
                isValid = true;
            return isValid;
        }
    }
}