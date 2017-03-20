using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAM.RD._2017F.Interfaces
{
    public interface IService<TEntity>
    {
        void Add(TEntity e);
        void Delete(TEntity e);
        IEnumerable<TEntity> Search(Func<TEntity, bool> predicate);
    }
}
