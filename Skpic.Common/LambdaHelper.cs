/*
 * Added by laoxu 2014-09-10 11:00:00
 * ---------------------------------------------------------------
 * for：lambda helper.
 * get sql string and param in lambda expression.
 * ---------------------------------------------------------------
 * version:1.0
 * mail:lovexurongquan@163.com
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Skpic.Common
{
    public class LambdaHelper<T> where T : class
    {
        /// <summary>
        /// lambda expression helper.
        /// get param in lambda expression.
        /// </summary>
        public LambdaHelper()
        {
            _paramCollection = new Dictionary<string, string>();
            _whereBuilder = new StringBuilder();
            _orderBuilder = new StringBuilder();
            _groupBuilder = new StringBuilder();
            _selectBuilder = new StringBuilder();
        }

        #region Where expression

        /// <summary>
        /// sql builder.
        /// </summary>
        private readonly StringBuilder _whereBuilder;

        /// <summary>
        /// param collection.
        /// </summary>
        private readonly Dictionary<string, string> _paramCollection;

        /// <summary>
        /// Initialization expression, if you gender sql by expression, you must use this to initialization.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        public void Init(Expression<Func<T, bool>> predicate)
        {
            if (_whereBuilder.Length > 0)
            {
                _whereBuilder.Append(" AND ");
            }
            ChoiseExpression(predicate.Body);
        }

        /// <summary>
        /// get where sql in expression.
        /// </summary>
        /// <returns></returns>
        public string GetWhereSql()
        {
            var where = _whereBuilder.ToString()
             .Replace("()", " ");
            //.Replace("and and", "and")
            //.Replace("or or", "or");

            where = Regex.Replace(where, @"or\s*or", "or");
            where = Regex.Replace(where, @"and\s*and", "and");

            where = Regex.Replace(where, @"and\s*\)", ")");
            where = Regex.Replace(where, @"\(\s*and", "(");

            where = Regex.Replace(where, @"\(\s*or", "(");
            where = Regex.Replace(where, @"or\s*\)", ")");

            return where;
        }

        /// <summary>
        /// get parameter in expression.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetParameters()
        {
            //var list = _paramCollection.Select(param => typeof(string)).ToList();
            //var dynamicObject = DynamicGenerator.DynamicObject(list);

            //object result = Activator.CreateInstance(dynamicObject);

            //for (int i = 0; i < _paramCollection.Count; i++)
            //{
            //    PropertyInfo pi = dynamicObject.GetProperty("P" + i);
            //    pi.SetValue(result, _paramCollection["p" + i], null);
            //}

            return _paramCollection;
        }

        /// <summary>
        /// get parameter in expression. return all dictionary.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetParameterDict()
        {
            return _paramCollection;
        }

        /// <summary>
        /// choise expression type.
        /// </summary>
        /// <param name="expression"></param>
        private void ChoiseExpression(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Lambda:
                case ExpressionType.Not:
                    GetUnaryExpression(expression);
                    break;

                case ExpressionType.Call:
                    GetStringMethodExpressions(expression);
                    break;

                case ExpressionType.OrElse:
                case ExpressionType.AndAlso:

                    SplitExpression(expression as BinaryExpression);
                    break;

                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:

                    GetOperatorsExpressions(expression as BinaryExpression);
                    break;
            }
        }

        /// <summary>
        /// split expression with left and right.
        /// </summary>
        /// <param name="binaryExpression">expression body.</param>
        private void SplitExpression(BinaryExpression binaryExpression)
        {
            var left = binaryExpression.Left as BinaryExpression;

            var right = binaryExpression.Right as BinaryExpression;

            _whereBuilder.Append("(");

            if (left == null)
            {
                GetStringMethodExpressions(binaryExpression.Left);

                GetUnaryExpression(binaryExpression.Left);

                _whereBuilder.Append(GetLogical(binaryExpression.NodeType.ToString()));
            }
            else
            {
                GetOperatorsExpressions(left);
                if (right != null)
                {
                    _whereBuilder.Append(GetLogical(binaryExpression.NodeType.ToString()));
                }

                SplitExpression(left);
            }

            if (right == null)
            {
                _whereBuilder.Append(GetLogical(binaryExpression.NodeType.ToString()));

                GetUnaryExpression(binaryExpression.Right);

                GetStringMethodExpressions(binaryExpression.Right);
            }
            else
            {
                if (left != null)
                {
                    _whereBuilder.Append(GetLogical(binaryExpression.NodeType.ToString()));
                }

                GetOperatorsExpressions(right);

                SplitExpression(right);
            }
            _whereBuilder.Append(")");
        }

        /// <summary>
        ///  get unary in expression.
        /// Exp: NotEquals, NotContains.
        /// </summary>
        /// <param name="expression"></param>
        private void GetUnaryExpression(Expression expression)
        {
            var unExpression = expression as UnaryExpression;
            if (unExpression == null) return;
            if (unExpression.Operand is MethodCallExpression)
            {
                GetStringMethodExpressions(unExpression.Operand, true);
            }
            else if (unExpression.Operand is BinaryExpression)
            {
                GetOperatorsExpressions(unExpression.Operand as BinaryExpression, true);
            }
        }

        /// <summary>
        /// get operators in expression.
        /// Exp: LessThan, GreaterThanOrEqual, GreaterThan, LessThanOrEqual, Equal.
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
        /// get string method in expression.
        /// Exp: Equals, StartsWith, EndsWith, Contains.
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
            _whereBuilder.Append(isNot
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

            _whereBuilder.Append(name + methodName + "@p" + _paramCollection.Count + " ");
            _paramCollection.Add("p" + _paramCollection.Count, value);
        }

        #endregion

        #region Order expression

        /// <summary>
        /// order name
        /// </summary>
        private readonly StringBuilder _orderBuilder;

        /// <summary>
        /// Initialization expression, if you gender sql by expression, you must use this to initialization.
        /// </summary>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <param name="isDesc">true or false desc.</param>
        public void Init<TKey>(Expression<Func<T, TKey>> keySelector, bool isDesc)
        {
            var body = keySelector.Body as MemberExpression;
            if (body == null) return;
            if (_orderBuilder.Length > 0)
            {
                _orderBuilder.AppendFormat(isDesc ? " , {0} DESC " : " , {0} ASC ", body.Member.Name);
            }
            else
            {
                _orderBuilder.AppendFormat(isDesc ? " {0} DESC " : " {0} ASC ", body.Member.Name);
            }
        }

        /// <summary>
        /// Get sorted statement.
        /// </summary>
        /// <returns></returns>
        public string GetOrderSql()
        {
            return _orderBuilder.ToString();
        }

        #endregion

        #region Group,Select,Distinct expression

        /// <summary>
        /// group builder.
        /// </summary>
        private readonly StringBuilder _groupBuilder;

        /// <summary>
        /// group builder.
        /// </summary>
        private readonly StringBuilder _selectBuilder;

        /// <summary>
        /// Initialization expression, if you gender sql by expression, you must use this to initialization.
        /// </summary>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <param name="type">enum of SqlType.</param>
        public void Init<TKey>(Expression<Func<T, TKey>> keySelector, SqlType type)
        {
            string selectorColumn;
            switch (type)
            {
                case SqlType.Select:
                    selectorColumn = GetSelectorColumn(keySelector);
                    if (string.IsNullOrEmpty(selectorColumn)) return;
                    _selectBuilder.AppendFormat(_selectBuilder.Length > 0 ? " , {0} " : " {0} ", selectorColumn);
                    break;
                case SqlType.Group:
                    selectorColumn = GetSelectorColumn(keySelector);
                    if (string.IsNullOrEmpty(selectorColumn)) return;
                    _groupBuilder.AppendFormat(_groupBuilder.Length > 0 ? " , {0} " : " {0} ", selectorColumn);
                    break;
            }
        }

        /// <summary>
        /// Get sorted statement.
        /// </summary>
        /// <returns></returns>
        public string GetGroupSql()
        {
            return _groupBuilder.ToString();
        }

        /// <summary>
        /// Get select statement.
        /// </summary>
        /// <returns></returns>
        public string GetSelectSql()
        {
            return _selectBuilder.ToString();
        }

        #endregion

        /// <summary>
        /// Get the name of a collection of selected columns.
        /// </summary>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <returns></returns>
        private string GetSelectorColumn<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            var memberBody = keySelector.Body as MemberExpression;
            if (memberBody != null)
            {
                //_groupBuilder.AppendFormat(_groupBuilder.Length > 0 ? " , {0} " : " GROUP BY {0} ", memberBody.Member.Name);
                return memberBody.Member.Name;
            }

            var newBody = keySelector.Body as NewExpression;
            if (newBody == null) return null;
            var memberNameCollection = newBody.Arguments.Select(argument => ((MemberExpression)argument).Member.Name);
            //_groupBuilder.AppendFormat(_groupBuilder.Length > 0 ? " , {0} " : " GROUP BY {0} ", string.Join(",", memberNameCollection));
            return string.Join(",", memberNameCollection);
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