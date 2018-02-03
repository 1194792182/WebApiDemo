using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiWeb.Models.Logs
{
    public class LogModel
    {

        public LogModel()
        {
            UserCode = "000001";
            UserName = "Admin";
            LoginIP = "127.0.0.1";
            Type = (int)ApiWeb.LogType.Common;
            Operation = "普通操作";
            Remark = string.Empty;
            CreateDate = DateTime.Now;
        }

        public int Id { get; set; }

        public string UserCode { get; set; }

        public string UserName { get; set; }

        public string LoginIP { get; set; }

        public int Type { get; set; }

        public string Operation { get; set; }

        public string Remark { get; set; }

        public DateTime CreateDate { get; set; }

    }
}