using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Skpic.Common
{
    public class LambdaHelper<T> where T : class
    {
        /// <summary>
        /// lambda expression helper. 
        /// get param in lambda expression.
        /// </summary>
        /// <param name="expression"></param>
        public LambdaHelper(Expression<Func<T, bool>> expression)
        {
            Expression = expression;

            if (expression.Body is BinaryExpression)
            {
                SplitExpression(expression.Body as BinaryExpression);
            }
            else if (expression.Body is MethodCallExpression)
            {
                GetStringMethodExpressions(expression.Body);
            }
            else if (expression.Body is UnaryExpression)
            {
                GetUnaryExpression(expression.Body);
            }

            if (_paramCollection.Count > 50)
            {
                throw new Exception("param count nust be less than 50.");
            }
        }

        /// <summary>
        /// expression.
        /// </summary>
        private Expression<Func<T, bool>> Expression { get; set; }

        /// <summary>
        /// sql builder.
        /// </summary>
        readonly StringBuilder _sb = new StringBuilder();

        /// <summary>
        /// param collection.
        /// </summary>
        readonly Dictionary<string, string> _paramCollection = new Dictionary<string, string>();

        /// <summary>
        /// Get model name.
        /// </summary>
        /// <returns></returns>
        public string GetModelName()
        {
            return Expression.Parameters[0].Type.Name;
        }

        /// <summary>
        /// get where sql in expression.
        /// </summary>
        /// <returns></returns>
        public string GetWhereSql()
        {
            var where = _sb.ToString()
             .Replace("()", " ")
             .Replace("  ", " ")
             .Replace("  ", " ")
             .Replace("and and", "and")
             .Replace("or or", "or")
             .Replace("( and (", "((")
             .Replace("( or (", "((")
             .Replace("( or (", "((")
             .Replace(") and )", "))")
             .Replace(") or )", "))");
            return where;
        }

        /// <summary>
        /// get parameter in expression.
        /// </summary>
        /// <returns></returns>
        public object GetParameters()
        {
            var x = _paramCollection.SerializeObjectToJson();
            return x.DeSerializeStringToObject<Param>();
        }

        /// <summary>
        /// split expression with left and right.
        /// </summary>
        /// <param name="binaryExpression">expression body.</param>
        private void SplitExpression(BinaryExpression binaryExpression)
        {

            var left = binaryExpression.Left as BinaryExpression;

            var right = binaryExpression.Right as BinaryExpression;
            if (left == null && right == null)
            {
                GetOperatorsExpressions(binaryExpression);
            }
            _sb.Append("(");


            if (left == null)
            {
                GetStringMethodExpressions(binaryExpression.Left);

                GetUnaryExpression(binaryExpression.Left);

                _sb.Append(GetLogical(binaryExpression.NodeType.ToString()));
            }
            else
            {
                GetOperatorsExpressions(left);
                if (right != null)
                {
                    _sb.Append(GetLogical(binaryExpression.NodeType.ToString()));
                }

                SplitExpression(left);
            }

            if (right == null)
            {
                _sb.Append(GetLogical(binaryExpression.NodeType.ToString()));

                GetUnaryExpression(binaryExpression.Right);

                GetStringMethodExpressions(binaryExpression.Right);

            }
            else
            {
                if (left != null)
                {
                    _sb.Append(GetLogical(binaryExpression.NodeType.ToString()));
                }

                GetOperatorsExpressions(right);

                SplitExpression(right);
            }
            _sb.Append(")");
        }

        /// <summary>
        ///  get unary in expression. Exp: NotEquals, NotContains.
        /// </summary>
        /// <param name="expression"></param>
        private void GetUnaryExpression(Expression expression)
        {
            var unExpression = expression as UnaryExpression;
            if (unExpression == null) return;
            GetStringMethodExpressions(unExpression.Operand, true);
        }

        /// <summary>
        /// get operators in expression. Exp: LessThan, GreaterThanOrEqual, GreaterThan, LessThanOrEqual, Equal.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="isNot"></param>
        private void GetOperatorsExpressions(BinaryExpression expression, bool isNot = false)
        {
            var left = expression.Left as MemberExpression;
            var right = expression.Right as ConstantExpression;
            if (left != null && right != null)
            {
                SetLambdaParameters(left.Member.Name, expression.NodeType.ToString(), right.Value == null ? "null" : right.Value.ToString(), isNot);
            }
        }

        /// <summary>
        /// get string method in expression. Exp: Equals, StartsWith, EndsWith, Contains.
        /// </summary>
        /// <param name="expression">expression data.</param>
        /// <param name="isNot"></param>
        private void GetStringMethodExpressions(Expression expression, bool isNot = false)
        {
            var data = expression as MethodCallExpression;
            if (data == null) return;

            var key = data.Object as MemberExpression;
            var argument = data.Arguments[0] as ConstantExpression;
            if (key != null && argument == null)
            {
                var keyValue = key.Expression as ConstantExpression;
                var inArgument = data.Arguments[0] as MemberExpression;
                if (keyValue != null && inArgument != null)
                {
                    var json = keyValue.Value.SerializeObjectToJson();
                    var o = json.DeSerializeStringToObject<Dictionary<string, List<string>>>();
                    var keyCollection = o[o.Keys.First()];
                    if (keyCollection != null)
                    {
                        var name = inArgument.Member.Name;

                        SetInParameters(name, "in", keyCollection, isNot);
                    }
                }
            }
            if (key != null && argument != null)
            {
                SetLambdaParameters(key.Member.Name, data.Method.Name, argument.Value == null ? "null" : argument.Value.ToString(), isNot);
            }
        }

        /// <summary>
        /// append in sql text.
        /// </summary>
        /// <param name="name">column name.</param>
        /// <param name="methodName"></param>
        /// <param name="valueCollection">in value collection.</param>
        /// <param name="isNot">is not in?</param>
        private void SetInParameters(string name, string methodName, IEnumerable<string> valueCollection, bool isNot)
        {
            var list = new List<string>();

            foreach (var value in valueCollection)
            {
                list.Add("@p" + _paramCollection.Count);
                _paramCollection.Add("p" + _paramCollection.Count, value);
            }
            _sb.Append(isNot
                ? string.Format("{0} not {1} ({2})", name, methodName.ToLower(), string.Join(",", list))
                : string.Format("{0} {1} ({2})", name, methodName.ToLower(), string.Join(",", list)));
        }

        /// <summary>
        /// set lambda param.
        /// </summary>
        /// <param name="name">column name.</param>
        /// <param name="methodName">operators method name.</param>
        /// <param name="value">value.</param>
        /// <param name="isNot">is not?</param>
        private void SetLambdaParameters(string name, string methodName, string value, bool isNot = false)
        {

            methodName = isNot ? ChoiseOperators("Not" + methodName, ref value) : ChoiseOperators(methodName, ref value);

            _sb.Append(name + methodName + "@p" + _paramCollection.Count + " ");
            _paramCollection.Add("p" + _paramCollection.Count, value);
        }

        /// <summary>
        /// get logical.
        /// </summary>
        /// <param name="nodeType">node type.</param>
        /// <returns></returns>
        private static string GetLogical(string nodeType)
        {
            switch (nodeType)
            {
                case "AndAlso":
                    return " and ";
                case "OrElse":
                    return " or ";
            }
            return "";
        }

        /// <summary>
        /// choise operators in sql text.
        /// </summary>
        /// <param name="methodName">the method name in expression.</param>
        /// <param name="value"></param>
        /// <returns>sql operators.</returns>
        private string ChoiseOperators(string methodName, ref string value)
        {
            switch (methodName)
            {
                case "Equals":
                case "Equal":
                    return " = ";
                case "NotEqual":
                case "NotEquals":
                    return " <> ";
                case "StartsWith":
                    value = value + "%";
                    return " like ";
                case "NotStartsWith":
                    value = value + "%";
                    return " not like ";
                case "EndsWith":
                    value = "%" + value;
                    return " like ";
                case "NotEndsWith":
                    value = "%" + value;
                    return " not like ";
                case "Contains":
                    value = "%" + value + "%";
                    return " like ";
                case "NotContains":
                    value = "%" + value + "%";
                    return " not like ";
                case "LessThan":
                    return " < ";
                case "NotLessThan":
                    return " >= ";
                case "LessThanOrEqual":
                    return " <= ";
                case "NotLessThanOrEqual":
                    return " > ";
                case "GreaterThan":
                    return " > ";
                case "NotGreaterThan":
                    return " <= ";
                case "GreaterThanOrEqual":
                    return " >= ";
                case "NotGreaterThanOrEqual":
                    return " < ";
            }
            return "";
        }
    }
}

public class Param
{
    public string p0 { get; set; }
    public string p1 { get; set; }
    public string p2 { get; set; }
    public string p3 { get; set; }
    public string p4 { get; set; }
    public string p5 { get; set; }
    public string p6 { get; set; }
    public string p7 { get; set; }
    public string p8 { get; set; }
    public string p9 { get; set; }
    public string p10 { get; set; }
    public string p11 { get; set; }
    public string p12 { get; set; }
    public string p13 { get; set; }
    public string p14 { get; set; }
    public string p15 { get; set; }
    public string p16 { get; set; }
    public string p17 { get; set; }
    public string p18 { get; set; }
    public string p19 { get; set; }
    public string p20 { get; set; }
    public string p21 { get; set; }
    public string p22 { get; set; }
    public string p23 { get; set; }
    public string p24 { get; set; }
    public string p25 { get; set; }
    public string p26 { get; set; }
    public string p27 { get; set; }
    public string p28 { get; set; }
    public string p29 { get; set; }
    public string p30 { get; set; }
    public string p31 { get; set; }
    public string p32 { get; set; }
    public string p33 { get; set; }
    public string p34 { get; set; }
    public string p35 { get; set; }
    public string p36 { get; set; }
    public string p37 { get; set; }
    public string p38 { get; set; }
    public string p39 { get; set; }
    public string p40 { get; set; }
    public string p41 { get; set; }
    public string p42 { get; set; }
    public string p43 { get; set; }
    public string p44 { get; set; }
    public string p45 { get; set; }
    public string p46 { get; set; }
    public string p47 { get; set; }
    public string p48 { get; set; }
    public string p49 { get; set; }
}
