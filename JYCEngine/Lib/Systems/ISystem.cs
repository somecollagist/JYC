using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JYCEngine;

public interface ISystem
{
    public void Execute(double deltaTime);
}
