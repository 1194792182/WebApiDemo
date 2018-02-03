using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiWeb
{
    public class LogInfo
    {
        public int Id { get; set; }

        public string UserCode { get; set; }

        public string UserName { get; set; }

        public string LoginIP { get; set; }

        public LogType LogType { get; set; }

        public string Operation { get; set; }

        public string Remark { get; set; }

        public DateTime CreateDate { get; set; }
    }

    public enum LogType
    {
        Common = 1
    }

}