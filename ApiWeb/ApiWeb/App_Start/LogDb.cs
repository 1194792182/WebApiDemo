using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ApiWeb
{
    public class LogDb : DbContext
    {
        public LogDb() : base(@"server=.\SQLEXPRESS;uid=sa;pwd=123456;database=LogDB")
        {

        }

        public DbSet<LogInfo> LogInfos { get; set; }
    }
}