﻿using Lib.SQL.Adapter.Session;

namespace Lib.SQL.SQLite
{
    internal abstract class Session : ISession
    {
        public abstract ITransaction BeginTransaction();
    }
}