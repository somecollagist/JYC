using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JYCEngine
{
    public interface IPool
    {
        public object Get(int id);
        public void Set(int id, object item);
        public int Reserve();
        public void Recycle(int id);
    }
}
