﻿using ApiWeb.Filters;
using ApiWeb.Models.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiWeb.Controllers
{
    public class LogsController : ApiController
    {
        static readonly ILogRepository repository = new LogRepository();

        [SecurityFilter]
        public IEnumerable<LogModel> GetAllLogs(int pageIndex = 0, int pageSize = 10)
        {
            return repository.GetAll(pageIndex, pageSize);
        }

        public IHttpActionResult GetDelAllResult(string operType)
        {
            if (!string.IsNullOrEmpty(operType) && operType.ToLower().Equals("delall"))
            {
                return Ok(repository.RemoveAll());
            }
            else
            {
                return Ok(0);
            }
        }

        public IEnumerable<LogModel> GetByOperation(string op, int pageIndex = 0, int pageSize = 10)
        {
            return repository.GetAll(op, pageIndex, pageSize);
        }


        [SecurityFilter]
        [HttpPost]
        public IHttpActionResult InsertLog(LogModel item)
        {
            item = repository.Add(item);
            return Json(item);
        }

        [SecurityFilter]
        [HttpDelete]
        public IHttpActionResult RemoveLogById(int id)
        {
            return Ok(repository.Remove(id));
        }

    }
}
