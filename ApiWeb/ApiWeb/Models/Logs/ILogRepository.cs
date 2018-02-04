using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiWeb.Models.Logs
{
    public interface ILogRepository
    {
        IEnumerable<LogModel> GetAll(int pageIndex = 0, int pageSize = 10);

        IEnumerable<LogModel> GetAll(string op, int pageIndex = 0, int pageSize = 10);

        LogModel Get(int id);

        LogModel Add(LogModel item);

        int Remove(int id);

        bool Update(LogModel item);

        int RemoveAll();
    }
}
