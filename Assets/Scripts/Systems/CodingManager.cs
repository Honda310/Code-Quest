using UnityEngine;
using System.Collections.Generic;
using System;
public class CodingManager : MonoBehaviour
{
    Dictionary<string, Variable> variables = new();
    private List<Token> tokens;
    private int current = 0;
    public enum VarType
    {
        Int,
        Float,
        Double,
        Bool,
        String,
        Char,
        IntArray,
        FloatArray,
        DoubleArray,
        BoolArray,
        StringArray,
        CharArray
    }
    public enum TokenType
    {
        Number,
        Identifier,
        Plus,
        Minus,
        Multiply,
        Divide,
        Mod,
        Assign,
        Less,
        Greater,
        Equal,
        EqualEqual,
        NotEqual,
        If,
        For,
        Else,
        While,
        LeftParen,
        RightParen,
        LeftBrace,
        RightBrace,
        Semicolon, 
        Print,
    }
    public class Variable
    {
        public VarType Type;
        public object Value;

        public Variable(VarType type, object value)
        {
            Type = type;
            Value = value;
        }
    }
    public class Token
    {
        public TokenType Type;
        public string Lexeme;

        public Token(TokenType type, string lexeme)
        {
            Type = type;
            Lexeme = lexeme;
        }
    }
    abstract class Expr
    {
        public abstract int Evaluate(Dictionary<string, Variable> variables);
    }

    abstract class Stmt
    {
        public abstract void Execute(Dictionary<string, Variable> variables);
    }

    class BinaryExpr : Expr
    {
        Expr left;
        Token op;
        Expr right;

        public BinaryExpr(Expr left, Token op, Expr right)
        {
            this.left = left;
            this.op = op;
            this.right = right;
        }
        public override int Evaluate(Dictionary<string, Variable> variables)
        {
            int l = left.Evaluate(variables);
            int r = right.Evaluate(variables);

            switch (op.Type)
            {
                case TokenType.Plus: return l + r;
                case TokenType.Minus: return l - r;
                case TokenType.Multiply: return l * r;
                case TokenType.Divide: return l / r;
                case TokenType.Less: return l < r ? 1 : 0;
                case TokenType.Greater:  return l > r ? 1 : 0;
                case TokenType.EqualEqual: return l == r ? 1 : 0;
                case TokenType.NotEqual: return l != r ? 1 : 0;
            }

            throw new Exception("不明な演算子");
        }

    }
    class NumberExpr : Expr
    {
        int value;

        public NumberExpr(int value)
        {
            this.value = value;
        }

        public override int Evaluate(Dictionary<string, Variable> variables)
        {
            return value;
        }
    }
    class VariableExpr : Expr
    {
        string name;

        public VariableExpr(string name)
        {
            this.name = name;
        }

        public override int Evaluate(Dictionary<string, Variable> variables)
        {
            return (int)variables[name].Value;
        }
    }
    class AssignmentStmt : Stmt
    {
        string name;
        Expr valueExpr;

        public AssignmentStmt(string name, Expr valueExpr)
        {
            this.name = name;
            this.valueExpr = valueExpr;
        }

        public override void Execute(Dictionary<string, Variable> variables)
        {
            int value = valueExpr.Evaluate(variables);
            variables[name] = new Variable(VarType.Int, value);
        }
    }
    class IfStmt : Stmt
    {
        Expr condition;
        List<Stmt> thenBranch;
        List<Stmt> elseBranch;

        public IfStmt(Expr condition, List<Stmt> thenBranch, List<Stmt> elseBranch)
        {
            this.condition = condition;
            this.thenBranch = thenBranch;
            this.elseBranch = elseBranch;
        }

        public override void Execute(Dictionary<string, Variable> variables)
        {
            if (condition.Evaluate(variables) != 0)
            {
                foreach (var stmt in thenBranch)
                    stmt.Execute(variables);
            }
            else if (elseBranch != null)
            {
                foreach (var stmt in elseBranch)
                    stmt.Execute(variables);
            }
        }
    }
    ///<summary>
    ///Print文の作成場所。ここにあるvalueの使用方法を変えれば本番にもそのまま流用できる。
    ///多分。
    ///</summary>
    class PrintStmt : Stmt
    {
        Expr expression;

        public PrintStmt(Expr expression)
        {
            this.expression = expression;
        }

        public override void Execute(Dictionary<string, Variable> variables)
        {
            int value = expression.Evaluate(variables);
            Debug.Log(value);
        }
    }

    private Expr ParseExpression()
    {
        return ParseComparison();
    }

    private Expr ParseComparison()
    {
        Expr expr = ParseTerm();

        while (Match(TokenType.Less) ||
               Match(TokenType.Greater) ||
               Match(TokenType.EqualEqual) ||
               Match(TokenType.NotEqual))
        {
            Token op = Previous();
            Expr right = ParseTerm();
            expr = new BinaryExpr(expr, op, right);
        }

        return expr;
    }
    private bool Match(TokenType type)
    {
        if (Check(type))
        {
            Advance();
            return true;
        }
        return false;
    }
    private bool Check(TokenType type)
    {
        if (IsAtEnd()) return false;
        return Peek().Type == type;
    }
    private Token Advance()
    {
        if (!IsAtEnd()) current++;
        return Previous();
    }
    private bool IsAtEnd()
    {
        return current >= tokens.Count;
    }
    private Token Peek()
    {
        return tokens[current];
    }
    private Token Previous()
    {
        return tokens[current - 1];
    }
    private void Consume(TokenType type)
    {
        if (Check(type))
        {
            Advance();
            return;
        }

        throw new Exception("想定外のトークンです");
    }
    private Expr ParseTerm()
    {
        Expr expr = ParseFactor();

        while (Match(TokenType.Plus) || Match(TokenType.Minus))
        {
            Token op = Previous();
            Expr right = ParseFactor();
            expr = new BinaryExpr(expr, op, right);
        }

        return expr;
    }
    private Expr ParseFactor()
    {
        Expr expr = ParsePrimary();

        while (Match(TokenType.Multiply) ||
               Match(TokenType.Divide) ||
               Match(TokenType.Mod))
        {
            Token op = Previous();
            Expr right = ParsePrimary();
            expr = new BinaryExpr(expr, op, right);
        }

        return expr;
    }

    private Expr ParsePrimary()
    {
        if (Match(TokenType.Number))
        {
            return new NumberExpr(int.Parse(Previous().Lexeme));
        }

        if (Match(TokenType.Identifier))
        {
            return new VariableExpr(Previous().Lexeme);
        }

        if (Match(TokenType.LeftParen))
        {
            Expr expr = ParseExpression();
            Consume(TokenType.RightParen);
            return expr;
        }

        throw new Exception("不正な式です");
    }


    public void Tokenize(string input)
    {
        tokens = new List<Token>();
        current = 0;

        int i = 0;

        while (i < input.Length)
        {
            char c = input[i];
            if (char.IsWhiteSpace(c))
            {
                i++;
                continue;
            }
            if (char.IsDigit(c))
            {
                string number = "";
                while (i < input.Length && char.IsDigit(input[i]))
                {
                    number += input[i];
                    i++;
                }
                tokens.Add(new Token(TokenType.Number, number));
                continue;
            }
            if (char.IsLetter(c))
            {
                string identifier = "";
                while (i < input.Length && char.IsLetterOrDigit(input[i]))
                {
                    identifier += input[i];
                    i++;
                }

                if (identifier == "if")
                    tokens.Add(new Token(TokenType.If, identifier));
                else if (identifier == "else")
                    tokens.Add(new Token(TokenType.Else, identifier));
                else if (identifier == "print")
                    tokens.Add(new Token(TokenType.Print, identifier));
                else
                    tokens.Add(new Token(TokenType.Identifier, identifier));
                continue;
            }
            if (i + 1 < input.Length)
            {
                if (input[i] == '=' && input[i + 1] == '=')
                {
                    tokens.Add(new Token(TokenType.EqualEqual, "=="));
                    i += 2;
                    continue;
                }
                if (input[i] == '!' && input[i + 1] == '=')
                {
                    tokens.Add(new Token(TokenType.NotEqual, "!="));
                    i += 2;
                    continue;
                }
            }
            // 記号
            switch (c)
            {
                case '+': tokens.Add(new Token(TokenType.Plus, "+")); break;
                case '-': tokens.Add(new Token(TokenType.Minus, "-")); break;
                case '*': tokens.Add(new Token(TokenType.Multiply, "*")); break;
                case '/': tokens.Add(new Token(TokenType.Divide, "/")); break;
                case '%': tokens.Add(new Token(TokenType.Mod, "%")); break;
                case '=': tokens.Add(new Token(TokenType.Assign, "=")); break;
                case '(': tokens.Add(new Token(TokenType.LeftParen, "(")); break;
                case ')': tokens.Add(new Token(TokenType.RightParen, ")")); break;
                case ';': tokens.Add(new Token(TokenType.Semicolon, ";")); break;
                case '<': tokens.Add(new Token(TokenType.Less, "<")); break;
                case '>': tokens.Add(new Token(TokenType.Greater, ">")); break;
                case '{': tokens.Add(new Token(TokenType.LeftBrace, "{")); break;
                case '}': tokens.Add(new Token(TokenType.RightBrace, "}")); break;
            }
            i++;
        }
    }
    //private void ParseAssignment()
    //{
    //    string name = Previous().Lexeme;

    //    Consume(TokenType.Assign);

    //    int value = ParseExpression();

    //    variables[name] = new Variable(VarType.Int, value);
    //}
    //private bool ParseCondition()
    //{
    //    int left = ParseExpression();

    //    if (Match(TokenType.Less))
    //        return left < ParseExpression();

    //    if (Match(TokenType.Greater))
    //        return left > ParseExpression();

    //    throw new Exception("条件式エラー");
    //}
    private Stmt ParseStatement()
    {
        if (Match(TokenType.If))
        {
            Consume(TokenType.LeftParen);
            Expr condition = ParseExpression();
            Consume(TokenType.RightParen);
            Consume(TokenType.LeftBrace);

            List<Stmt> thenBranch = new List<Stmt>();
            while (!Check(TokenType.RightBrace))
            {
                thenBranch.Add(ParseStatement());
            }
            Consume(TokenType.RightBrace);

            List<Stmt> elseBranch = null;

            if (Match(TokenType.Else))
            {
                Consume(TokenType.LeftBrace);

                elseBranch = new List<Stmt>();
                while (!Check(TokenType.RightBrace))
                {
                    elseBranch.Add(ParseStatement());
                }
                Consume(TokenType.RightBrace);
            }

            return new IfStmt(condition, thenBranch, elseBranch);
        }
        if (Match(TokenType.Print))
        {
            Consume(TokenType.LeftParen);
            Expr expr = ParseExpression();
            Consume(TokenType.RightParen);
            Consume(TokenType.Semicolon);

            return new PrintStmt(expr);
        }
        if (Match(TokenType.Identifier))
        {
            string name = Previous().Lexeme;
            Consume(TokenType.Assign);
            Expr valueExpr = ParseExpression();
            Consume(TokenType.Semicolon);

            return new AssignmentStmt(name, valueExpr);
        }

        throw new Exception("不正な文です");
    }


















    void Start()
    {
        Tokenize(@"
                a = 5;
                print(a);
                print(a + 3);

                if (a > 3) {
                    print(100);
                }
                ");

        List<Stmt> program = ParseProgram();

        foreach (var stmt in program)
        {
            stmt.Execute(variables);
        }

        Debug.Log("a = " + variables["a"].Value);
    }
    private List<Stmt> ParseProgram()
    {
        List<Stmt> statements = new List<Stmt>();

        while (!IsAtEnd())
        {
            statements.Add(ParseStatement());
        }

        return statements;
    }
}
