using Skpic.Async;
using System.Collections.Generic;
using System.Linq;

namespace Skpic.SqlBuilder
{
    /// <summary>
    /// sql text build extensions class.
    /// </summary>
    public class SqlBuilder
    {
        private readonly Dictionary<string, Clauses> _data = new Dictionary<string, Clauses>();
        private int _seq;

        private class Clause
        {
            public string Sql { get; set; }

            public object Parameters { get; set; }
        }

        private class Clauses : List<Clause>
        {
            private readonly string _joiner;
            private readonly string _prefix;
            private readonly string _postfix;

            public Clauses(string joiner, string prefix = "", string postfix = "")
            {
                _joiner = joiner;
                _prefix = prefix;
                _postfix = postfix;
            }

            public string ResolveClauses(DynamicParameters p)
            {
                foreach (var item in this)
                {
                    p.AddDynamicParams(item.Parameters);
                }
                return _prefix + string.Join(_joiner, this.Select(c => c.Sql)) + _postfix;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public class Template
        {
            private readonly string _sql;
            private readonly SqlBuilder _builder;
            private readonly object _initParams;
            private int _dataSeq = -1; // Unresolved

            /// <summary>
            /// Template default constructor.
            /// </summary>
            /// <param name="builder">the builder.</param>
            /// <param name="sql">sql text.</param>
            /// <param name="parameters">param array.</param>
            public Template(SqlBuilder builder, string sql, dynamic parameters)
            {
                _initParams = parameters;
                _sql = sql;
                _builder = builder;
            }

            private static readonly System.Text.RegularExpressions.Regex Regex =
                new System.Text.RegularExpressions.Regex(@"\/\*\*.+\*\*\/", System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.Multiline);

            private void ResolveSql()
            {
                if (_dataSeq == _builder._seq) return;
                var p = new DynamicParameters(_initParams);

                _rawSql = _sql;

                foreach (var pair in _builder._data)
                {
                    _rawSql = _rawSql.Replace("/**" + pair.Key + "**/", pair.Value.ResolveClauses(p));
                }
                _parameters = p;

                // replace all that is left with empty
                _rawSql = Regex.Replace(_rawSql, "");

                _dataSeq = _builder._seq;
            }

            private string _rawSql;
            private object _parameters;

            /// <summary>
            ///
            /// </summary>
            public string RawSql { get { ResolveSql(); return _rawSql; } }

            /// <summary>
            ///
            /// </summary>
            public object Parameters { get { ResolveSql(); return _parameters; } }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Template AddTemplate(string sql, dynamic parameters = null)
        {
            return new Template(this, sql, parameters);
        }

        private void AddClause(string name, string sql, object parameters, string joiner, string prefix = "", string postfix = "")
        {
            Clauses clauses;
            if (!_data.TryGetValue(name, out clauses))
            {
                clauses = new Clauses(joiner, prefix, postfix);
                _data[name] = clauses;
            }
            clauses.Add(new Clause { Sql = sql, Parameters = parameters });
            _seq++;
        }

        /// <summary>
        /// append inner join text.
        /// </summary>
        /// <param name="sql">sql text.</param>
        /// <param name="parameters">param array default null.</param>
        /// <returns></returns>
        public SqlBuilder InnerJoin(string sql, dynamic parameters = null)
        {
            AddClause("innerjoin", sql, parameters, joiner: "\nINNER JOIN ", prefix: "\nINNER JOIN ", postfix: "\n");
            return this;
        }

        /// <summary>
        /// append left join text.
        /// </summary>
        /// <param name="sql">sql text.</param>
        /// <param name="parameters">param array default null.</param>
        /// <returns></returns>
        public SqlBuilder LeftJoin(string sql, dynamic parameters = null)
        {
            AddClause("leftjoin", sql, parameters, joiner: "\nLEFT JOIN ", prefix: "\nLEFT JOIN ", postfix: "\n");
            return this;
        }

        /// <summary>
        /// append right join text.
        /// </summary>
        /// <param name="sql">sql text. </param>
        /// <param name="parameters">param array default null.</param>
        /// <returns></returns>
        public SqlBuilder RightJoin(string sql, dynamic parameters = null)
        {
            AddClause("rightjoin", sql, parameters, joiner: "\nRIGHT JOIN ", prefix: "\nRIGHT JOIN ", postfix: "\n");
            return this;
        }

        /// <summary>
        /// append where text.
        /// </summary>
        /// <param name="sql">sql text.</param>
        /// <param name="parameters">param array default null.</param>
        /// <returns></returns>
        public SqlBuilder Where(string sql, dynamic parameters = null)
        {
            AddClause("where", sql, parameters, " AND ", prefix: "WHERE ", postfix: "\n");
            return this;
        }

        /// <summary>
        /// append order by text.
        /// </summary>
        /// <param name="sql">sql text.</param>
        /// <param name="parameters">param array default null.</param>
        /// <returns></returns>
        public SqlBuilder OrderBy(string sql, dynamic parameters = null)
        {
            AddClause("orderby", sql, parameters, " , ", prefix: "ORDER BY ", postfix: "\n");
            return this;
        }

        /// <summary>
        /// apend select text.
        /// </summary>
        /// <param name="sql">sql text.</param>
        /// <param name="parameters">param array default null.</param>
        /// <returns></returns>
        public SqlBuilder Select(string sql, dynamic parameters = null)
        {
            AddClause("select", sql, parameters, " , ", prefix: "", postfix: "\n");
            return this;
        }

        /// <summary>
        /// append param.
        /// </summary>
        /// <param name="parameters">param array.</param>
        /// <returns></returns>
        public SqlBuilder AddParameters(dynamic parameters)
        {
            AddClause("--parameters", "", parameters, "");
            return this;
        }

        /// <summary>
        /// append join text.
        /// </summary>
        /// <param name="sql">sql text.</param>
        /// <param name="parameters">param array default null.</param>
        /// <returns></returns>
        public SqlBuilder Join(string sql, dynamic parameters = null)
        {
            AddClause("join", sql, parameters, joiner: "\nJOIN ", prefix: "\nJOIN ", postfix: "\n");
            return this;
        }

        /// <summary>
        /// append group by text.
        /// </summary>
        /// <param name="sql">sql text.</param>
        /// <param name="parameters">param array default null.</param>
        /// <returns></returns>
        public SqlBuilder GroupBy(string sql, dynamic parameters = null)
        {
            AddClause("groupby", sql, parameters, joiner: " , ", prefix: "\nGROUP BY ", postfix: "\n");
            return this;
        }

        /// <summary>
        /// append having text.
        /// </summary>
        /// <param name="sql">sql text.</param>
        /// <param name="parameters">param array default null.</param>
        /// <returns></returns>
        public SqlBuilder Having(string sql, dynamic parameters = null)
        {
            AddClause("having", sql, parameters, joiner: "\nAND ", prefix: "HAVING ", postfix: "\n");
            return this;
        }
    }
}