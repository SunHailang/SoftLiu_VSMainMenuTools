using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PEG;
using L = PEG.LPEG;
using MiniScript.Nodes;

namespace MiniScript
{
    public unsafe static class Parser
    {
        private static readonly string[] Keywords =
        {
            "if", "else", "return", "for", "while", "function", "_G", "true", "false", "TRUE", "FALSE",
            "True", "False", "var", "int", "float", "break", "continue", "arg",
        };

        private static readonly string[] UnaryOperators =
        {
            "-", "+", "!", /*"~"*/
        };
        private static void UnaryTranslation(ref CaptureResult capture)
        {
            string s = (string)capture[0];

            if (s == "+") return;
            else capture.PushResult(s);
        }

        private static readonly string[][] BinaryOperators =
        {
            new string[] {"||"},
            new string[] {"&&"},
            //new string[] {"|"},
            //new string[] {"^"},
            //new string[] {"&"},
            new string[] {"==", "!="},
            new string[] {">=", "<=", ">", "<"},
            //new string[] {"<<", ">>"},
            new string[] {"+", "-"},
            new string[] {"*", "/", "%"},
        };

        private static void NumberCapture(ref CaptureResult result)
        {
            var text = (string)result[0];
            result.PushResult(new NumberNode(text));
        }

        private static void BooleanCapture(ref CaptureResult result)
        {
            var text = (string)result[0];
            result.PushResult(new BooleanNode(text));
        }

        private static void VariableCapture(ref CaptureResult result)
        {
            int position = (int)result[0];
            var text = (string)result[1];
            result.PushResult(new VariableNode(text) { codePosition = position });
        }

        private static void UnaryCapture(ref CaptureResult result)
        {
            var op = (string)result[0];
            var operand = (ExpressionNode)result[1];

            result.PushResult(new UnaryNode(op, operand));
        }

        private static void BinaryCapture(ref CaptureResult result)
        {
            if(result.captureCount == 1)
            {
                result.PushResult(result[0]);
                return;
            }

            var operand1 = (Node)result[0];
            var op = (string)result[1];
            var operand2 = (Node)result[2];

            result.PushResult(new BinaryNode(op, operand1, operand2));
        }

        private static void TernaryCapture(ref CaptureResult result)
        {
            var condition = (ExpressionNode)result[0];
            var trueState = (ExpressionNode)result[1];
            var falseState = (ExpressionNode)result[2];

            result.PushResult(new TernaryNode(condition, trueState, falseState));
        }

        private static void FunctionCallCapture(ref CaptureResult result)
        {
            var name = (VariableNode)result[0];
            var arguments = new ExpressionNode[result.captureCount - 1];
            for(int it = 0; it < arguments.Length; it++)
            {
                arguments[it] = (ExpressionNode)result[it + 1];
            }
            result.PushResult(new FunctionCallNode(name, arguments));
        }

        private static void FunctionCallStatementCapture(ref CaptureResult result)
        {
            var func = (FunctionCallNode)result[0];

            result.PushResult(new FunctionCallStatementNode(func));
        }

        private static void IfStatementCapture(ref CaptureResult result)
        {
            var condition = (ExpressionNode)result[0];
            var trueState = (StatementNode)result[1];
            var falseState = result.captureCount == 3 ? (StatementNode)result[2] : null;

            result.PushResult(new IfStatementNode(condition, trueState, falseState));
        }

        private static void GroupStatementCapture(ref CaptureResult result)
        {
            var statements = new StatementNode[result.captureCount];
            for (int it = 0; it < statements.Length; it++) statements[it] = (StatementNode)result[it];

            result.PushResult(new GroupStatementNode(statements));
        }

        private static void VarTypeCapture(ref CaptureResult result)
        {
            result.PushResult(new TypeNode((string)result[0]));
        }

        private static void AssignStatementCapture(ref CaptureResult result)
        {
            var variable = (VariableNode)result[0];
            var op = (string)result[1];
            var expression = (ExpressionNode)result[2];

            if(op != "=") expression = new BinaryNode(op.Substring(0, op.Length - 1), variable, expression);

            result.PushResult(new AssignStatementNode(variable, expression));
        }

        private static void DeclareStatementCapture(ref CaptureResult result)
        {
            var varType = (TypeNode)result[0];
            var variable = (VariableNode)result[1];

            result.PushResult(new DeclareStatementNode(varType, variable));

            if(result.captureCount > 2)
            {
                var expression = (ExpressionNode)result[2];
                result.PushResult(new AssignStatementNode(variable, expression));
            }
        }

        private static void ReturnStatementCapture(ref CaptureResult result)
        {
            var expression = result.captureCount == 1 ? null : (ExpressionNode)result[1];

            result.PushResult(new ReturnStatementNode(expression));
        }

        private static void WhileStatementCapture(ref CaptureResult result)
        {
            var condition = (ExpressionNode)result[0];
            var loopBody = (StatementNode)result[1];

            result.PushResult(new WhileStatementNode(condition, loopBody));
        }

        private static void ForStatementCapture(ref CaptureResult result)
        {
            int index;
            DeclareStatementNode declare;
            if(result.captureCount == 4)
            {
                declare = null;
                index = 0;
            }
            else
            {
                declare = (DeclareStatementNode)result[0];
                index = 1;
            }
            var init = (StatementNode)result[index];
            var condition = (ExpressionNode)result[index + 1];
            var loopHead = (StatementNode)result[index + 2];
            var loopBody = (StatementNode)result[index + 3];

            result.PushResult(new ForStatementNode(declare, init, condition, loopHead, loopBody));
        }

        private static void ContinueStatementCapture(ref CaptureResult result)
        {
            result.PushResult(ContinueStatementNode.Instance);
        }

        private static void BreakStatementCapture(ref CaptureResult result)
        {
            result.PushResult(BreakStatementNode.Instance);
        }

        private static void IncrementStatementCapture(ref CaptureResult result)
        {
            VariableNode variable;
            string op;

            var first = result[0];
            if(first is VariableNode)
            {
                variable = (VariableNode)result[0];
                op = (string)result[1];
            }
            else
            {
                variable = (VariableNode)result[1];
                op = (string)result[0];
            }

            result.PushResult(new AssignStatementNode(variable, new BinaryNode(op.Substring(1), variable, new NumberNode("1"))));
        }

        private static readonly Pattern rawSpace = L.S(" \n\t\r");
        private static readonly Pattern singleLineComment = L.P("//") * (L.P(1) - L.S("\n\r")).Repeat(0);
        private static readonly Pattern multiLineComment = L.P("/*") * ((L.P(1) - L.P("*")) + L.P("*") * (L.P(1) - L.P("/"))).Repeat(0) * L.P("*/");
        private static readonly Pattern space = (rawSpace + singleLineComment + multiLineComment).Repeat(0);
        private static readonly Pattern requiredSpace = (rawSpace + singleLineComment + multiLineComment).Repeat(1);
        private static Pattern Ps(string v) => L.P(v) * space; //p with space

        public static Pattern BuildPattern()
        {
            var any = L.P(1);
            var number = L.R("09").Repeat(1).SimpleCapture() / NumberCapture * space;
            var boolean = (L.P("true") + L.P("TRUE") + L.P("True") + L.P("false") + L.P("FALSE") + L.P("False")) / BooleanCapture;
            Pattern variable;
            {
                var keywordsToExcludes = L.P(false);
                foreach (var keyword in Keywords) keywordsToExcludes += L.P(keyword);
                keywordsToExcludes *= any - L.R("azAZ09__").Repeat(1);

                variable = L.Cp() * (L.R("azAZ__") * L.R("azAZ09__").Repeat(0) - keywordsToExcludes).SimpleCapture() / VariableCapture * space;
            }
            var varType = (L.P("var") + L.P("int") + L.P("float") + L.P("bool")).SimpleCapture() * requiredSpace / VarTypeCapture;
            var unary = L.S(string.Join(string.Empty, UnaryOperators)).SimpleCapture() / UnaryTranslation * space;
            var binary = new Pattern[BinaryOperators.Length];
            for(int it = 0; it < binary.Length; it++)
            {
                var operators = BinaryOperators[it];
                var op = L.P(operators[0]);
                for (int jt = 1; jt < operators.Length; jt++) op += L.P(operators[jt]);
                op = op.SimpleCapture() * space;

                var target = it == binary.Length - 1 ? L.V("unary") : L.V("binary" + (it + 1));
                binary[it] = target * (op * L.V("binary" + it)).Repeat(-1) / BinaryCapture;
            }
            var opAssign = L.P("=");
            foreach(var operators in BinaryOperators)
            {
                foreach(var binaryOperator in operators)
                {
                    opAssign += L.P(binaryOperator + "=");
                }
            }
            opAssign = opAssign.SimpleCapture() * space;
            var opIncrement = (L.P("++") + L.P("--")).SimpleCapture() * space;

            var exp = L.V("exp");
            var statement = L.V("statement");

            var grammar = new Grammar
            {
                firstRuler = "grammar",
                grammars = new Dictionary<string, Pattern>
                {
                    { "grammar", statement.Repeat(1) + exp },
                    { "statement", L.V("groupStatement")
                                    + L.V("ifStatement")
                                    + L.V("whileStatement")
                                    + L.V("forStatement")
                                    + L.V("incrementStatement")
                                    + L.V("assignStatement")
                                    + L.V("declareStatement")
                                    + L.V("returnStatement")
                                    + L.V("functionCallStatement")
                                    + L.V("continueStatement")
                                    + L.V("breakStatement")
                                    },
                    { "groupStatement", Ps("{") * statement.Repeat(0) * Ps("}") / GroupStatementCapture },
                    { "ifStatement", Ps("if") * Ps("(") * exp * Ps(")") * statement
                                    * (L.P("else") * requiredSpace * statement).Repeat(-1)
                                    / IfStatementCapture },
                    { "assignExpression", variable * opAssign * exp / AssignStatementCapture },
                    { "assignStatement", L.V("assignExpression") * Ps(";")  },
                    { "declareExpression", varType * variable * (Ps("=") * exp).Repeat(-1) / DeclareStatementCapture },
                    { "declareStatement", L.V("declareExpression") * Ps(";") },
                    { "returnStatement", L.P("return").SimpleCapture() * (requiredSpace * exp + space) * Ps(";") / ReturnStatementCapture },
                    { "functionCallStatement", L.V("functionCall") * Ps(";") / FunctionCallStatementCapture },
                    { "whileStatement", Ps("while") * Ps("(") * exp * Ps(")") * statement / WhileStatementCapture },
                    { "continueStatement", Ps("continue") * Ps(";") / ContinueStatementCapture },
                    { "breakStatement", Ps("break") * Ps(";") / BreakStatementCapture },
                    { "forStatement", Ps("for")*Ps("(")* L.V("declareExpression") * Ps(";") * exp * Ps(";") * (L.V("assignExpression") + L.V("incrementExpression")) * Ps(")") * statement / ForStatementCapture},
                    { "incrementExpression", (variable * opIncrement + opIncrement * variable) / IncrementStatementCapture },
                    { "incrementStatement", L.V("incrementExpression") * Ps(";") },
                    

                    { "exp", L.V("ternary") + L.V("binary0") },
                    { "ternary", L.V("binary0") * Ps("?") * exp * Ps(":") * exp / TernaryCapture },
                    { "unary", unary * L.V("unary") / UnaryCapture + L.V("single") },
                    { "single", number
                                + L.V("functionCall")
                                + variable 
                                + boolean
                                + Ps("(") * exp * Ps(")")
                                },
                    { "functionCall", variable * Ps("(") * (exp * (Ps(",") * exp).Repeat(0)).Repeat(-1) * Ps(")") / FunctionCallCapture },
                }
            };
            for (int it = 0; it < binary.Length; it++) grammar.grammars["binary" + it] = binary[it];

            var pattern = space * L.P(grammar) * (L.P(-1) + L.P(ParseError));
            return pattern;
        }

        private static void ParseError(string originalString, int index, ref CaptureResult captures)
        {
            var bytes = Encoding.UTF8.GetBytes(originalString);

            int to = index + 20 >= bytes.Length ? bytes.Length : index + 20;
            int line = 1;
            for(int it = 0; it <= index; it++)
            {
                var ch = bytes[it];
                if (ch == '\n') line++;
            }

            throw new Exception($"parse error near: [{Encoding.UTF8.GetString(bytes, index, to - index)}], line:{line}, index:{index}");
        }
    }
}
