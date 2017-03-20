using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAM.RD._2017F.Interfaces
{
    public interface IStorage<T>
        where T:class
    {
        void SaveState(string fileName, T obj);
        T RestoreState(string fileName);
    }
}
