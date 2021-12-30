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
        private bool isDatatype(int InputPointer)
        {
            bool isInt = TokenStream[InputPointer].token_type == Token_Class.Int;
            bool isFloat = TokenStream[InputPointer].token_type == Token_Class.Float;
            bool isString = TokenStream[InputPointer].token_type == Token_Class.String;
            return (isInt || isFloat || isString);
        }

        private bool isStatement(int InputPointer)
        {
            bool isDecleration = isDatatype(InputPointer);
            bool isWrite = TokenStream[InputPointer].token_type == Token_Class.Write;
            bool isRead = TokenStream[InputPointer].token_type == Token_Class.Read;
            bool isReturn = TokenStream[InputPointer].token_type == Token_Class.Return;
            bool isConditionOrFunctionCallOrAssignment = TokenStream[InputPointer].token_type == Token_Class.Idenifier;
            bool isIf = TokenStream[InputPointer].token_type == Token_Class.If;
            bool isRepeat = TokenStream[InputPointer].token_type == Token_Class.Repeat;
            return (isDecleration || isWrite || isRead || isReturn || isConditionOrFunctionCallOrAssignment || isIf || isRepeat);
        }
        private bool isConditionOp(int InputPointer)
        {
            bool isLessThan = TokenStream[InputPointer].token_type == Token_Class.LessThanOp;
            bool isGreaterThan = TokenStream[InputPointer].token_type == Token_Class.GreaterThanOp;
            bool isEqual = TokenStream[InputPointer].token_type == Token_Class.EqualOp;
            bool isNotEqual = TokenStream[InputPointer].token_type == Token_Class.NotEqualOp;
            return (isEqual || isGreaterThan || isLessThan || isNotEqual);
        }

        private bool isBooleanOp(int InputPointer)
        {
            bool isOr = TokenStream[InputPointer].token_type == Token_Class.OrOp;
            bool isAnd = TokenStream[InputPointer].token_type == Token_Class.AndOp;
            return (isAnd|| isOr);
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
            if (InputPointer < TokenStream.Count && isDatatype(InputPointer))
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
            functionStatement.Children.Add(FunctionDeclaration());
            functionStatement.Children.Add(FunctionBody());
            return functionStatement;
        }

        private Node FunctionDeclaration()
        {
            Node functionDec = new Node("Function Declaration");
            functionDec.Children.Add(Datatype());
            functionDec.Children.Add(match(Token_Class.Idenifier));
            functionDec.Children.Add(match(Token_Class.LParanthesis));
            functionDec.Children.Add(Parameters());
            functionDec.Children.Add(match(Token_Class.RParanthesis));
            return functionDec;
        }

        private Node Parameters()
        {
            Node parameteres = new Node("Parameters");
            if (InputPointer < TokenStream.Count && isDatatype(InputPointer))
            {
                parameteres.Children.Add(Parameter());
                parameteres.Children.Add(ParametersD());
                return parameteres;
            }
            else
                return null;
        }

        private Node ParametersD()
        {
            Node parameterD = new Node("Parameter'");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                parameterD.Children.Add(match(Token_Class.Comma));
                parameterD.Children.Add(Parameter());
                parameterD.Children.Add(ParametersD());
                return parameterD;
            }
            else
                return null;
        }

        private Node Parameter()
        {
            Node parameter = new Node("Parameter");
            parameter.Children.Add(Datatype());
            parameter.Children.Add(match(Token_Class.Idenifier));
            return parameter;
        }

        private Node Datatype()
        {
            Node datatype = new Node("Datatype");
            if(TokenStream[InputPointer].token_type == Token_Class.Int)
            {
                datatype.Children.Add(match(Token_Class.Int));
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Float)
            {
                datatype.Children.Add(match(Token_Class.Float));
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.String)
            {
                datatype.Children.Add(match(Token_Class.String));
            }
            return datatype;
        }

        private Node MainFunction()
        {
            Node main = new Node("Main");
            main.Children.Add(Datatype());
            main.Children.Add(match(Token_Class.Main));
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
            Node returnStatement = new Node("Return Statement");
            returnStatement.Children.Add(match(Token_Class.Return));
            returnStatement.Children.Add(Expression());
            returnStatement.Children.Add(match(Token_Class.Semicolon));
            return returnStatement;
        }

        private Node Expression()
        {
            Node exp = new Node("Expression");
            if(TokenStream[InputPointer].token_type == Token_Class.String)
            {
                exp.Children.Add(match(Token_Class.String));
            }
            // TODO: Complete CFG
            return exp;
        }

        private Node Statements()
        {
            Node statements = new Node("Statements");
            statements.Children.Add(Statement());
            statements.Children.Add(StatementsD());
            
            return statements;
        }

        private Node StatementsD()
        {
            Node statementD = new Node("Statements'");
            if (InputPointer < TokenStream.Count && isStatement(InputPointer))
            {
                statementD.Children.Add(Statement());
                statementD.Children.Add(StatementsD());
                return statementD;
            }
            else
                return null;
        }

        private Node Statement()
        {
            Node statement = new Node("Statement");
            if(TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                // TODO: Test different character
                // Assignment
                if(TokenStream[InputPointer + 1].token_type == Token_Class.AssignmentOp)
                {
                    statement.Children.Add(AssignmentStatment());
                    statement.Children.Add(match(Token_Class.Semicolon));
                }
                //Condition
                else if(isConditionOp(InputPointer+1))
                {
                    statement.Children.Add(ConditionStatement());
                }
                //Function Call
                else if(TokenStream[InputPointer + 1].token_type == Token_Class.LParanthesis)
                {
                    statement.Children.Add(FunctionCall());
                    statement.Children.Add(match(Token_Class.Semicolon));
                }
            }
            // Declaration
            else if(isDatatype(InputPointer))
            {
                statement.Children.Add(DeclarationStatement());
            }
            else if(TokenStream[InputPointer].token_type == Token_Class.Write)
            {
                statement.Children.Add(WriteStatement());
            }
            else if(TokenStream[InputPointer].token_type == Token_Class.Read)
            {
                statement.Children.Add(ReadStatement());
            }
            else if(TokenStream[InputPointer].token_type == Token_Class.Return)
            {
                statement.Children.Add(ReturnStatement());
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.If)
            {
                statement.Children.Add(IfStatement());
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Repeat)
            {
                statement.Children.Add(RepeatStatement());
            }

            return statement;
        }

        private Node RepeatStatement()
        {
            Node repeatStatement = new Node("Repeat Statement");
            repeatStatement.Children.Add(match(Token_Class.Repeat));
            repeatStatement.Children.Add(Statements());
            repeatStatement.Children.Add(match(Token_Class.Until));
            repeatStatement.Children.Add(ConditionStatement());
            return repeatStatement;
        }

        private Node IfStatement()
        {
            Node ifStatement = new Node("If Statement");
            ifStatement.Children.Add(match(Token_Class.If));
            ifStatement.Children.Add(ConditionStatement());
            ifStatement.Children.Add(match(Token_Class.Then));
            ifStatement.Children.Add(Statements());
            ifStatement.Children.Add(RestIf());

            return ifStatement;
        }

        private Node RestIf()
        {
            Node restIf = new Node("Rest If");
            if(TokenStream[InputPointer].token_type == Token_Class.Elseif)
            {
                restIf.Children.Add(ElseIfStatement());
            }
            else if(TokenStream[InputPointer].token_type == Token_Class.Else)
            {
                restIf.Children.Add(ElseStatement());
            }
            else if(TokenStream[InputPointer].token_type == Token_Class.End)
            {
                restIf.Children.Add(match(Token_Class.End));
            }
            return restIf;
        }

        private Node ElseStatement()
        {
            Node elseStatement = new Node("Else Statement");
            elseStatement.Children.Add(match(Token_Class.Else));
            elseStatement.Children.Add(Statements());
            elseStatement.Children.Add(match(Token_Class.End));
            return elseStatement;
        }

        private Node ElseIfStatement()
        {
            Node elseIfStatement = new Node("Else If Statement");
            elseIfStatement.Children.Add(match(Token_Class.Elseif));
            elseIfStatement.Children.Add(ConditionStatement());
            elseIfStatement.Children.Add(match(Token_Class.Then));
            elseIfStatement.Children.Add(Statements());
            elseIfStatement.Children.Add(RestIf());
            return elseIfStatement;
        }

        private Node ReadStatement()
        {
            Node readStatement = new Node("Read Statement");
            readStatement.Children.Add(match(Token_Class.Read));
            readStatement.Children.Add(match(Token_Class.Idenifier));
            readStatement.Children.Add(match(Token_Class.Semicolon));
            return readStatement;
        }

        private Node WriteStatement()
        {
            Node writeStatement = new Node("Write Statement");
            writeStatement.Children.Add(match(Token_Class.Write));
            writeStatement.Children.Add(WriteD());
            writeStatement.Children.Add(match(Token_Class.Semicolon));
            return writeStatement;
        }

        private Node WriteD()
        {
            Node writeD = new Node("Write'");
            writeD.Children.Add(Expression());
            writeD.Children.Add(match(Token_Class.Endl));
            return writeD;
        }

        private Node AssignmentStatment()
        {
            Node assign = new Node("Assignment Statment");
            assign.Children.Add(match(Token_Class.Idenifier));
            assign.Children.Add(match(Token_Class.AssignmentOp));
            assign.Children.Add(Expression());
            return assign;
        }

        private Node DeclarationStatement()
        {
            Node declarationStatement = new Node("Declaration Statement");
            declarationStatement.Children.Add(Datatype());
            declarationStatement.Children.Add(Ids());
            declarationStatement.Children.Add(match(Token_Class.Semicolon));
            return declarationStatement;
        }

        private Node Ids()
        {
            Node ids = new Node("IDs");
            if(TokenStream[InputPointer + 1].token_type == Token_Class.Comma)
            {
                ids.Children.Add(match(Token_Class.Idenifier));
                ids.Children.Add(IdsD());
            }
            else if(TokenStream[InputPointer + 1].token_type == Token_Class.AssignmentOp)
            {
                ids.Children.Add(AssignmentStatment());
                ids.Children.Add(IdsD());
            }
            return ids;
        }

        private Node IdsD()
        {
            Node idsD = new Node("IDs'");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                idsD.Children.Add(match(Token_Class.Comma));
                idsD.Children.Add(Ids());
                return idsD;
            }
            else
                return null;
        }

        private Node ConditionStatement()
        {
            Node conditionStatement = new Node("Condition Statement");
            conditionStatement.Children.Add(Condition());
            conditionStatement.Children.Add(Conditions());
            return conditionStatement;
        }

        private Node Conditions()
        {
            Node conditions = new Node("Conditions");
            if (InputPointer < TokenStream.Count && isBooleanOp(InputPointer))
            {
                conditions.Children.Add(BooleanOp());
                conditions.Children.Add(Condition());
                conditions.Children.Add(Conditions());
                return conditions;
            }
            else
                return null;
        }

        private Node BooleanOp()
        {
            throw new NotImplementedException();
        }

        private Node Condition()
        {
            throw new NotImplementedException();
        }

        private Node FunctionCall()
        {
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