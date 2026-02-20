using UnityEngine;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using UnityEngine.SceneManagement;
public class CodingManager : MonoBehaviour
{
    private Stack<Dictionary<string, Variable>> scopes = new Stack<Dictionary<string, Variable>>();
    private List<Token> tokens;
    private int current = 0;
    private QuestManager questManager;
    private object AnswerObject="";
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
        And,
        Or,
        Not,
        True,
        False,
        Int,
        Bool,
        LessEqual,
        GreaterEqual,
        Increment,
        Decrement,
        LeftBracket,
        RightBracket,
        New,
        StringLiteral,
        String,
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
    class BoolExpr : Expr
    {
        bool value;

        public BoolExpr(bool value)
        {
            this.value = value;
        }

        public override object Evaluate(CodingManager manager)
        {
            return value;
        }
    }

    abstract class Expr
    {
        public abstract object Evaluate(CodingManager manager);
    }

    abstract class Stmt
    {
        public abstract void Execute(CodingManager manager);
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
        public override object Evaluate(CodingManager manager)
        {
            object leftVal = left.Evaluate(manager);
            object rightVal = right.Evaluate(manager);

            switch (op.Type)
            {
                case TokenType.LessEqual:
                    return (int)leftVal <= (int)rightVal;

                case TokenType.GreaterEqual:
                    return (int)leftVal >= (int)rightVal;

                case TokenType.Plus:
                    if (leftVal is string || rightVal is string)
                    {
                        return leftVal.ToString() + rightVal.ToString();
                    }
                    return (int)leftVal + (int)rightVal;

                case TokenType.Minus:
                    return (int)leftVal - (int)rightVal;

                case TokenType.Multiply:
                    return (int)leftVal * (int)rightVal;

                case TokenType.Divide:
                    return (int)leftVal / (int)rightVal;

                case TokenType.Less:
                    return (int)leftVal < (int)rightVal;

                case TokenType.Greater:
                    return (int)leftVal > (int)rightVal;

                case TokenType.EqualEqual:
                    return Equals(leftVal, rightVal);

                case TokenType.NotEqual:
                    return !Equals(leftVal, rightVal);

                case TokenType.And:
                    return (bool)leftVal && (bool)rightVal;

                case TokenType.Or:
                    return (bool)leftVal || (bool)rightVal;
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

        public override object Evaluate(CodingManager manager)
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
        public override object Evaluate(CodingManager manager)
        {
            Variable v = manager.Resolve(name);

            return v.Value;
        }
    }
    class UnaryExpr : Expr
    {
        Token op;
        Expr right;

        public UnaryExpr(Token op, Expr right)
        {
            this.op = op;
            this.right = right;
        }
        public override object Evaluate(CodingManager manager)
        {
            object r = right.Evaluate(manager);

            switch (op.Type)
            {
                case TokenType.Not:
                    return !(bool)r;
            }

            throw new Exception("不明な単項演算子");
        }
    }
    class StringExpr : Expr
    {
        string value;

        public StringExpr(string value)
        {
            this.value = value;
        }

        public override object Evaluate(CodingManager manager)
        {
            return value;
        }
    }

    class ArrayAccessExpr : Expr
    {
        string name;
        Expr indexExpr;

        public ArrayAccessExpr(string name, Expr indexExpr)
        {
            this.name = name;
            this.indexExpr = indexExpr;
        }

        public override object Evaluate(CodingManager manager)
        {
            Variable v = manager.Resolve(name);

            if (v.Type != VarType.IntArray)
                throw new Exception($"{name} は配列ではありません");

            if (v.Type != VarType.IntArray)
                throw new Exception($"{name} はint配列ではありません");
            int[] array = (int[])v.Value;
            int index = (int)indexExpr.Evaluate(manager);

            if (index < 0 || index >= array.Length)
                throw new Exception("配列の範囲外アクセス");

            return array[index];
        }
    }


    class ArrayDeclarationStmt : Stmt
    {
        string name;
        Expr sizeExpr;

        public ArrayDeclarationStmt(string name, Expr sizeExpr)
        {
            this.name = name;
            this.sizeExpr = sizeExpr;
        }

        public override void Execute(CodingManager manager)
        {
            int size = (int)sizeExpr.Evaluate(manager);

            if (size <= 0)
                throw new Exception("配列サイズは1以上必要です");

            manager.scopes.Peek()[name] = new Variable(VarType.IntArray, new int[size]);
        }
    }
    class ArrayAssignmentStmt : Stmt
    {
        string name;
        Expr indexExpr;
        Expr valueExpr;

        public ArrayAssignmentStmt(string name, Expr indexExpr, Expr valueExpr)
        {
            this.name = name;
            this.indexExpr = indexExpr;
            this.valueExpr = valueExpr;
        }

        public override void Execute(CodingManager manager)
        {
            Variable v = manager.Resolve(name);

            int[] array = (int[])v.Value;

            int index = (int)indexExpr.Evaluate(manager);
            int value = (int)valueExpr.Evaluate(manager);

            if (index < 0 || index >= array.Length)
                throw new Exception("配列の範囲外アクセス");

            array[index] = value;
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

        public override void Execute(CodingManager manager)
        {
            Variable existing = manager.Resolve(name);
            object value = valueExpr.Evaluate(manager);

            if (existing.Type == VarType.Int && value is not int)
                throw new Exception("int型にint以外を代入できません");

            if (existing.Type == VarType.Bool && value is not bool)
                throw new Exception("bool型にbool以外を代入できません");

            if (existing.Type == VarType.String && value is not string)
                throw new Exception("string型にstring以外を代入できません");

            existing.Value = value;
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

        public override void Execute(CodingManager manager)
        {
            object cond = condition.Evaluate(manager);

            if (cond is not bool)
                throw new Exception("ifの条件式はboolでなければなりません");
            if ((bool)cond)
            {
                manager.PushScope();

                foreach (var stmt in thenBranch)
                    stmt.Execute(manager);
                manager.PopScope();
            }
            else if (elseBranch != null)
            {
                manager.PushScope();

                foreach (var stmt in elseBranch)
                    stmt.Execute(manager);
                manager.PopScope();
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

        public override void Execute(CodingManager manager)
        {
            manager.AnswerObject = expression.Evaluate(manager);
        }
    }
    class WhileStmt : Stmt
    {
        Expr condition;
        List<Stmt> body;

        public WhileStmt(Expr condition, List<Stmt> body)
        {
            this.condition = condition;
            this.body = body;
        }

        public override void Execute(CodingManager manager)
        {
            Stopwatch sw = Stopwatch.StartNew();

            while (true)
            {
                if (sw.ElapsedMilliseconds > 1500)
                    throw new Exception("無限ループ");

                object cond = condition.Evaluate(manager);

                if (cond is not bool)
                    throw new Exception("whileの条件式はboolでなければなりません");

                if (!(bool)cond)
                    break;

                manager.PushScope();

                foreach (var stmt in body)
                    stmt.Execute(manager);

                manager.PopScope();
            }
        }
    }
    class VarDeclarationStmt : Stmt
    {
        string name;
        VarType type;
        Expr initializer;

        public VarDeclarationStmt(VarType type, string name, Expr initializer)
        {
            this.type = type;
            this.name = name;
            this.initializer = initializer;
        }

        public override void Execute(CodingManager manager)
        {
            object value = initializer.Evaluate(manager);

            if (type == VarType.Int && value is not int)
                throw new Exception("int型にint以外を代入できません");

            if (type == VarType.Bool && value is not bool)
                throw new Exception("bool型にbool以外を代入できません");

            if (type == VarType.String && value is not string)
                throw new Exception("string型にstring以外を代入できません");

            manager.scopes.Peek()[name] = new Variable(type, value);
        }
    }
    class ForStmt : Stmt
    {
        Stmt initializer;
        Expr condition;
        Stmt increment;
        List<Stmt> body;

        public ForStmt(Stmt initializer, Expr condition, Stmt increment, List<Stmt> body)
        {
            this.initializer = initializer;
            this.condition = condition;
            this.increment = increment;
            this.body = body;
        }

        public override void Execute(CodingManager manager)
        {
            manager.PushScope();

            initializer.Execute(manager);

            Stopwatch sw = Stopwatch.StartNew();

            while (true)
            {
                if (sw.ElapsedMilliseconds > 1500)
                    throw new Exception("実行時間制限を超過しています\n無限ループの可能性があります");

                object cond = condition.Evaluate(manager);

                if (cond is not bool)
                    throw new Exception("for条件はboolが型のみです");

                if (!(bool)cond)
                    break;

                manager.PushScope();

                foreach (var stmt in body)
                    stmt.Execute(manager);

                manager.PopScope();

                increment.Execute(manager);
            }

            manager.PopScope();
        }

    }
    class IncrementStmt : Stmt
    {
        string name;

        public IncrementStmt(string name)
        {
            this.name = name;
        }

        public override void Execute(CodingManager manager)
        {
            Variable v = manager.Resolve(name);

            if (v.Type != VarType.Int)
                throw new Exception("++はint型にしか使えません");

            v.Value = (int)v.Value + 1;
        }
    }
    class DecrementStmt : Stmt
    {
        string name;

        public DecrementStmt(string name)
        {
            this.name = name;
        }

        public override void Execute(CodingManager manager)
        {
            Variable v = manager.Resolve(name);

            if (v.Type != VarType.Int)
                throw new Exception("--はint型にしか使えません");

            v.Value = (int)v.Value - 1;
        }
    }
    private Expr ParseExpression()
    {
        return ParseOr();
    }
    private Expr ParseOr()
    {
        Expr expr = ParseAnd();

        while (Match(TokenType.Or))
        {
            Token op = Previous();
            Expr right = ParseAnd();
            expr = new BinaryExpr(expr, op, right);
        }

        return expr;
    }
    private Expr ParseAnd()
    {
        Expr expr = ParseComparison();

        while (Match(TokenType.And))
        {
            Token op = Previous();
            Expr right = ParseComparison();
            expr = new BinaryExpr(expr, op, right);
        }

        return expr;
    }
    private Expr ParseComparison()
    {
        Expr expr = ParseTerm();

        while (Match(TokenType.Less) ||
               Match(TokenType.Greater) ||
               Match(TokenType.LessEqual) ||
               Match(TokenType.GreaterEqual) ||
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
        if (Match(TokenType.Not))
        {
            Token op = Previous();
            Expr right = ParseFactor();
            return new UnaryExpr(op, right);
        }
        if (Match(TokenType.True))
            return new BoolExpr(true);

        if (Match(TokenType.False))
            return new BoolExpr(false);
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
        if (Match(TokenType.StringLiteral))
        {
            return new StringExpr(Previous().Lexeme);
        }
        if (Match(TokenType.Number))
        {
            return new NumberExpr(int.Parse(Previous().Lexeme));
        }

        if (Match(TokenType.Identifier))
        {
            string name = Previous().Lexeme;

            if (Match(TokenType.LeftBracket))
            {
                Expr index = ParseExpression();
                Consume(TokenType.RightBracket);
                return new ArrayAccessExpr(name, index);
            }

            return new VariableExpr(name);
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
                else if (identifier == "while")
                    tokens.Add(new Token(TokenType.While, identifier));
                else if (identifier == "true")
                    tokens.Add(new Token(TokenType.True, identifier));
                else if (identifier == "false")
                    tokens.Add(new Token(TokenType.False, identifier));
                else if (identifier == "int")
                    tokens.Add(new Token(TokenType.Int, identifier));
                else if (identifier == "bool")
                    tokens.Add(new Token(TokenType.Bool, identifier));
                else if (identifier == "string")
                    tokens.Add(new Token(TokenType.String, identifier));
                else if (identifier == "for")
                    tokens.Add(new Token(TokenType.For, identifier));
                else if (identifier == "new")
                    tokens.Add(new Token(TokenType.New, identifier));
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
                if (input[i] == '&' && input[i + 1] == '&')
                {
                    tokens.Add(new Token(TokenType.And, "&&"));
                    i += 2;
                    continue;
                }
                if (input[i] == '|' && input[i + 1] == '|')
                {
                    tokens.Add(new Token(TokenType.Or, "||"));
                    i += 2;
                    continue;
                }
                if (input[i] == '<' && input[i + 1] == '=')
                {
                    tokens.Add(new Token(TokenType.LessEqual, "<="));
                    i += 2;
                    continue;
                }
                if (input[i] == '>' && input[i + 1] == '=')
                {
                    tokens.Add(new Token(TokenType.GreaterEqual, ">="));
                    i += 2;
                    continue;
                }

                if (input[i] == '+' && input[i + 1] == '+')
                {
                    tokens.Add(new Token(TokenType.Increment, "++"));
                    i += 2;
                    continue;
                }
                if (input[i] == '-' && input[i + 1] == '-')
                {
                    tokens.Add(new Token(TokenType.Decrement, "--"));
                    i += 2;
                    continue;
                }
            }
            if (c == '"')
            {
                i++;
                string str = "";

                while (i < input.Length && input[i] != '"')
                {
                    str += input[i];
                    i++;
                }

                if (i >= input.Length)
                {
                    throw new Exception("文字列が閉じられていません");
                }
                i++;
                tokens.Add(new Token(TokenType.StringLiteral, str));
                continue;
            }
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
                case '!': tokens.Add(new Token(TokenType.Not, "!")); break;
                case '[': tokens.Add(new Token(TokenType.LeftBracket, "[")); break;
                case ']': tokens.Add(new Token(TokenType.RightBracket, "]")); break;
            }
            i++;
        }
    }
    private Stmt ParseStatement()
    {
        if (Match(TokenType.For))
        {
            Consume(TokenType.LeftParen);

            Stmt initializer;
            if (Match(TokenType.Int))
            {
                Consume(TokenType.Identifier);
                string name = Previous().Lexeme;
                Consume(TokenType.Assign);
                Expr initExpr = ParseExpression();
                initializer = new VarDeclarationStmt(VarType.Int, name, initExpr);
            }
            else
            {
                Consume(TokenType.Identifier);
                string name = Previous().Lexeme;
                Consume(TokenType.Assign);
                Expr initExpr = ParseExpression();
                initializer = new AssignmentStmt(name, initExpr);
            }

            Consume(TokenType.Semicolon);

            Expr condition = ParseExpression();
            Consume(TokenType.Semicolon);

            Consume(TokenType.Identifier);
            string incName = Previous().Lexeme;

            Stmt increment;

            if (Match(TokenType.Increment))
                increment = new IncrementStmt(incName);
            else if (Match(TokenType.Decrement))
                increment = new DecrementStmt(incName);
            else
            {
                Consume(TokenType.Assign);
                Expr valueExpr = ParseExpression();
                increment = new AssignmentStmt(incName, valueExpr);
            }

            Consume(TokenType.RightParen);
            Consume(TokenType.LeftBrace);

            List<Stmt> body = new();
            while (!Check(TokenType.RightBrace))
            {
                body.Add(ParseStatement());
            }

            Consume(TokenType.RightBrace);

            return new ForStmt(initializer, condition, increment, body);
        }
        if (Check(TokenType.Int) && CheckNext(TokenType.LeftBracket))
        {
            Advance();
            Advance();

            Consume(TokenType.RightBracket);
            Consume(TokenType.Identifier);
            string name = Previous().Lexeme;

            Consume(TokenType.Assign);
            Consume(TokenType.New);
            Consume(TokenType.Int);
            Consume(TokenType.LeftBracket);
            Expr sizeExpr = ParseExpression();
            Consume(TokenType.RightBracket);
            Consume(TokenType.Semicolon);

            return new ArrayDeclarationStmt(name, sizeExpr);
        }

        if (Match(TokenType.While))
        {
            Consume(TokenType.LeftParen);
            Expr condition = ParseExpression();
            Consume(TokenType.RightParen);
            Consume(TokenType.LeftBrace);

            List<Stmt> body = new List<Stmt>();
            while (!Check(TokenType.RightBrace))
            {
                body.Add(ParseStatement());
            }

            Consume(TokenType.RightBrace);

            return new WhileStmt(condition, body);
        }
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
                if (Check(TokenType.If))
                {
                    List<Stmt> nested = new List<Stmt>();
                    nested.Add(ParseStatement());
                    elseBranch = nested;
                }
                else
                {
                    Consume(TokenType.LeftBrace);

                    elseBranch = new List<Stmt>();
                    while (!Check(TokenType.RightBrace))
                    {
                        elseBranch.Add(ParseStatement());
                    }

                    Consume(TokenType.RightBrace);
                }
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
        if (Match(TokenType.Int) || Match(TokenType.Bool) || Match(TokenType.String))
        {
            Token typeToken = Previous();

            VarType varType = typeToken.Type switch
            {
                TokenType.Int => VarType.Int,
                TokenType.Bool => VarType.Bool,
                TokenType.String => VarType.String,_ => throw new Exception("未対応型")
            };

            Consume(TokenType.Identifier);
            string name = Previous().Lexeme;

            Consume(TokenType.Assign);
            Expr initializer = ParseExpression();
            Consume(TokenType.Semicolon);

            return new VarDeclarationStmt(varType, name, initializer);
        }
        if (Match(TokenType.Identifier))
        {
            string name = Previous().Lexeme;

            if (Match(TokenType.LeftBracket))
            {
                Expr index = ParseExpression();
                Consume(TokenType.RightBracket);
                Consume(TokenType.Assign);
                Expr value = ParseExpression();
                Consume(TokenType.Semicolon);

                return new ArrayAssignmentStmt(name, index, value);
            }

            if (Match(TokenType.Increment))
            {
                Consume(TokenType.Semicolon);
                return new IncrementStmt(name);
            }

            if (Match(TokenType.Decrement))
            {
                Consume(TokenType.Semicolon);
                return new DecrementStmt(name);
            }

            Consume(TokenType.Assign);
            Expr valueExpr = ParseExpression();
            Consume(TokenType.Semicolon);

            return new AssignmentStmt(name, valueExpr);
        }
        throw new Exception("不正な文です");
    }
    private bool CheckNext(TokenType type)
    {
        if (current + 1 >= tokens.Count) return false;
        return tokens[current + 1].Type == type;
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
    public void PushScope()
    {
        scopes.Push(new Dictionary<string, Variable>());
    }

    public void PopScope()
    {
        scopes.Pop();
    }
    public Variable Resolve(string name)
    {
        foreach (var scope in scopes)
        {
            if (scope.ContainsKey(name))
                return scope[name];
        }
        throw new Exception($"未宣言の変数 {name}");
    }
    private void Awake()
    {
        scopes.Clear();
        scopes.Push(new Dictionary<string, Variable>());
    }
    public void CodeSending(string code)
    {
        Tokenize(code);

        List<Stmt> program = ParseProgram();

        foreach (var stmt in program)
        {
            stmt.Execute(this);
        }
    }
    public bool AnswerCheck()
    {
        String ans="";
        switch (SceneManager.GetActiveScene().name)
        {
            case "LamentForest":
                ans = questManager.GetCodingQuestion(0).CorrectAnswer;
                break;
            case "PoisonedSpring":
                ans = questManager.GetCodingQuestion(1).CorrectAnswer;
                break;
            case "CorrupedTown":
                ans = questManager.GetCodingQuestion(2).CorrectAnswer;
                break;
            case "Temple":
                ans = questManager.GetCodingQuestion(3).CorrectAnswer;
                break;
            case "defalut":
                ans = "";
                break;
        }
        if (ans == AnswerObject.ToString())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void Start()
    {
        CodeSending(@" int x = 5;
                    print(x);

                    if (x > 3) {
                        int y = 100;
                        print(y);
                    }

                    for (int i = 0; i < 3; i++) {
                        print(i);
                    }

                    int[] arr = new int[3];
                    arr[0] = 10;
                    arr[1] = 20;
                    print(arr[1]);

                    while (x > 0) {
                        print(x);
                        x--;
                    }");
    }
}
