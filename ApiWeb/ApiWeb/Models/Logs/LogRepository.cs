using EntityFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiWeb.Models.Logs
{
    public class LogRepository : ILogRepository
    {
        public LogModel Add(LogModel item)
        {
            LogModel logModel;

            using (var db = new LogDb())
            {
                var entity = new LogInfo()
                {
                    UserCode = item.UserCode,
                    UserName = item.UserName,
                    LoginIP = item.LoginIP,
                    LogType = (LogType)item.Type,
                    Operation = item.Operation,
                    Remark = item.Remark,
                    CreateDate = item.CreateDate
                };
                db.LogInfos.Add(entity);
                var result = db.SaveChanges();
                if (result > 0)
                {
                    logModel = item;
                    logModel.Id = entity.Id;
                }
                else
                {
                    logModel = null;
                }
            }

            return logModel;
        }

        public LogModel Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LogModel> GetAll(int pageIndex = 0, int pageSize = 10)
        {
            var logModels = new List<LogModel>();

            using (var db = new LogDb())
            {
                var entities = db.LogInfos.AsNoTracking().OrderByDescending(q => q.Id).Skip(pageIndex * pageSize).Take(pageSize);

                foreach (var entity in entities)
                {
                    var model = new LogModel()
                    {
                        Id = entity.Id,
                        UserCode = entity.UserCode,
                        UserName = entity.UserName,
                        LoginIP = entity.LoginIP,
                        Type = (int)entity.LogType,
                        Operation = entity.Operation,
                        Remark = entity.Remark,
                        CreateDate = entity.CreateDate
                    };
                    logModels.Add(model);
                }
            }

            return logModels;
        }

        public IEnumerable<LogModel> GetAll(string op, int pageIndex = 0, int pageSize = 10)
        {
            var logModels = new List<LogModel>();

            using (var db = new LogDb())
            {
                var query = db.LogInfos.Where(q => q.Id > 0);

                if (!string.IsNullOrEmpty(op))
                {
                    query = query.Where(q => q.Operation.Contains(op));
                }

                var entities = query.OrderByDescending(q => q.Id).Skip(pageIndex * pageSize).Take(pageSize);

                foreach (var entity in entities)
                {
                    var model = new LogModel()
                    {
                        Id = entity.Id,
                        UserCode = entity.UserCode,
                        UserName = entity.UserName,
                        LoginIP = entity.LoginIP,
                        Type = (int)entity.LogType,
                        Operation = entity.Operation,
                        Remark = entity.Remark,
                        CreateDate = entity.CreateDate
                    };
                    logModels.Add(model);
                }
            }

            return logModels;
        }

        public int Remove(int id)
        {
            var result = 0;
            using (var db = new LogDb())
            {
                result = db.LogInfos.Where(q => q.Id == id).Delete();
            }
            return result;
        }

        public bool Update(LogModel item)
        {
            throw new NotImplementedException();
        }

        public int RemoveAll()
        {
            var result = 0;
            using (var db = new LogDb())
            {
                result = db.LogInfos.Where(q => q.Id > 0).Delete();
            }
            return result;
        }
    }
}