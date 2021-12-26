using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiny_Language
{
    public static class Tiny_Language
    {
        //Scanner
        public static Scanner Tiny_Scanner = new Scanner();
        public static List<Token> TokenStream = new List<Token>();

        //Parser
        public static Parser Tiny_Parser = new Parser();
        public static Node treeroot;

        public static void Start_Compiling(string SourceCode) //character by character
        {
            //Scanner
            Tiny_Scanner.StartScanning(SourceCode);
            //Parser
            Tiny_Parser.StartParsing(TokenStream);
            treeroot = Tiny_Parser.root;
        }
    }
}
