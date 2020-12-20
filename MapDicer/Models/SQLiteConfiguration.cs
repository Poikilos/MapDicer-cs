using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.SQLite;
using System.Data.SQLite.EF6;
// using System.Data.SQLite.Linq;
// ^ SQLiteProviderFactory can come from either System.Data.SQLite.EF6 or System.Data.SQLite.Linq.
using System.Linq;
using System.Text;

// See https://www.codeproject.com/Articles/1158937/SQLite-with-Csharp-Net-and-Entity-Framework?msg=5772111#xx5772111xx
// - This file must be in the same namespace as the DbContext and Models.

namespace MapDicer.Models
{
    class SQLiteConfiguration : DbConfiguration
    {
        public SQLiteConfiguration()
        {
            SetProviderFactory("System.Data.SQLite", SQLiteFactory.Instance);
            SetProviderFactory("System.Data.SQLite.EF6", SQLiteProviderFactory.Instance);
            // SetProviderFactory("System.Data.SQLite.Linq", SQLiteProviderFactory.Instance);
            SetProviderServices("System.Data.SQLite", (DbProviderServices)SQLiteProviderFactory.Instance.GetService(typeof(DbProviderServices)));
            // ^ See https://www.codeproject.com/Articles/1158937/SQLite-with-Csharp-Net-and-Entity-Framework?msg=5772111#xx5772111xx
        }
    }
}
