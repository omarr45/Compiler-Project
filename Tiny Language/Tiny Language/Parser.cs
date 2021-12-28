using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tiny_Language
{
    public class Node
    {
        public List<Node> Children = new List<Node>();

        public string Name;
        public Node(string N)
        {
            this.Name = N;
        }
    }

    public class Parser
    {
        int InputPointer = 0;
        List<Token> TokenStream;
        public Node root;

        public Node StartParsing(List<Token> TokenStream)
        {
            this.InputPointer = 0;
            this.TokenStream = TokenStream;
            root = new Node("Program");
            root.Children.Add(Program());
            return root;
        }

        Node Program()
        {
            Node program = new Node("Program");
            program.Children.Add(ProgramD());
            program.Children.Add(MainFunction());
            MessageBox.Show("Success");
            return program;
        }

        private Node ProgramD()
        {
            Node programD = new Node("Program'");
            bool dataType = TokenStream[InputPointer].token_type == Token_Class.Int || TokenStream[InputPointer].token_type == Token_Class.Float || TokenStream[InputPointer].token_type == Token_Class.String;
            if (InputPointer < TokenStream.Count && dataType)
            {
                programD.Children.Add(FunctionStatement());
                programD.Children.Add(ProgramD());
                return programD;
            }
            else
                return null;
        }

        private Node FunctionStatement()
        {
            Node functionStatement = new Node("Function Statement");

            return functionStatement;
        }

        private Node MainFunction()
        {
            Node main = new Node("Main");
            Token_Class dataTypeToken = Token_Class.Int;
            switch (TokenStream[InputPointer].token_type)
            {
                case Token_Class.Float:
                case Token_Class.String:
                    dataTypeToken = TokenStream[InputPointer].token_type;
                    break;
            }
            main.Children.Add(match(dataTypeToken));
            main.Children.Add(match(Token_Class.LParanthesis));
            main.Children.Add(match(Token_Class.RParanthesis));
            main.Children.Add(FunctionBody());
            return main;
        }

        private Node FunctionBody()
        {
            Node functionBody = new Node("Function Body");
            functionBody.Children.Add(match(Token_Class.LCurlyBracket));
            functionBody.Children.Add(Statements());
            functionBody.Children.Add(ReturnStatement());
            functionBody.Children.Add(match(Token_Class.RCurlyBracket));
            return functionBody;
        }

        private Node ReturnStatement()
        {
            // TODO: Return CFG
            throw new NotImplementedException();
        }

        private Node Statements()
        {
            Node statements = new Node("Statements");
            bool isAssignment = TokenStream[InputPointer].token_type == Token_Class.Idenifier;
            bool isDecleration = TokenStream[InputPointer].token_type == Token_Class.Int || TokenStream[InputPointer].token_type == Token_Class.Float || TokenStream[InputPointer].token_type == Token_Class.String;
            bool isWrite = TokenStream[InputPointer].token_type == Token_Class.Write;
            bool isRead = TokenStream[InputPointer].token_type == Token_Class.Read;
            bool isReturn = TokenStream[InputPointer].token_type == Token_Class.Return;
            // TODO: Complete booleans to check if one of them or epsilon
            //bool isCondition = TokenStream[InputPointer].token_type == Token_Class.Int
            statements.Children.Add(Statement());
            statements.Children.Add(StatementsD());
            
            return statements;
        }

        private Node StatementsD()
        {
            Node statementD = new Node("Statements'");
            statementD.Children.Add(Statement());

            return statementD;
        }

        private Node Statement()
        {
            // TODO
            throw new NotImplementedException();
        }
        
        // TODO: complete the rest of CFGs

        public Node match(Token_Class ExpectedToken)
        {

            if (InputPointer < TokenStream.Count)
            {
                if (ExpectedToken == TokenStream[InputPointer].token_type)
                {
                    InputPointer++;
                    Node newNode = new Node(ExpectedToken.ToString());

                    return newNode;

                }

                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + " and " +
                        TokenStream[InputPointer].token_type.ToString() +
                        "  found\r\n");
                    InputPointer++;
                    return null;
                }
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + "\r\n");
                InputPointer++;
                return null;
            }
        }

        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintTree(Node root)
        {
            if (root == null || root.Name == null)
                return null;
            TreeNode tree = new TreeNode(root.Name);
            if (root.Children.Count == 0)
                return tree;
            foreach (Node child in root.Children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
    }
}