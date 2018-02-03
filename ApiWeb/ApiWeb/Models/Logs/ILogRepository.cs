using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiWeb.Models.Logs
{
    public interface ILogRepository
    {
        IEnumerable<LogModel> GetAll();
        LogModel Get(int id);
        LogModel Add(LogModel item);
        void Remove(int id);
        bool Update(LogModel item);
    }
}
