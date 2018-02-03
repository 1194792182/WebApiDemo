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

        public IEnumerable<LogModel> GetAllProducts()
        {
            return repository.GetAll();
        }

        [HttpPost]
        public IHttpActionResult InsertLog(LogModel item)
        {
            item = repository.Add(item);
            return Json(item);
        }

    }
}
