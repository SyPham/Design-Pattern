using DMR_API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EC_API.Data
{
    public interface IRepositoryFactory
    {
        IECRepository<T> GetRepository<T>() where T : class;
    }
}
