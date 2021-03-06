﻿using System;
using Lib.SQL.Executor;
using Lib.SQL.QueryBuilder;
using Lib.SQL.QueryBuilder.Operator;
using Lib.SQL.QueryBuilder.Sequences.Where;

namespace Lib.SQL.Tables.Operation
{
    internal class TableDelete : TableOperation<Delete, int>, IWhereFilterable<TableDelete, int>
    {
        public TableDelete(Table table) : base(table, Delete.From(table.Name), new AffectedLinesExecutor())
        {
        }

        public IWhereFilterable<TableDelete, int> And(Action<SubSequence> sub)
        {
            Statement.And(sub);
            return this;
        }

        public IWhereFilterable<TableDelete, int> Where(string key, IBinaryOperator comparisonOperator, IConvertible value)
        {
            Statement.Where(key, comparisonOperator, value);
            return this;
        }

        public IWhereFilterable<TableDelete, int> Or(string key, IBinaryOperator comparisonOperator, IConvertible value)
        {
            Statement.Or(key, comparisonOperator, value);
            return this;
        }

        public IWhereFilterable<TableDelete, int> And(string key, IBinaryOperator comparisonOperator, IConvertible value)
        {
            Statement.And(key, comparisonOperator, value);
            return this;
        }

        public IWhereFilterable<TableDelete, int> Or(Action<SubSequence> sub)
        {
            Statement.Or(sub);
            return this;
        }
    }
}
