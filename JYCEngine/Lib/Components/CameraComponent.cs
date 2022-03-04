using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JYCEngine.StdLib;

public struct CameraComponent
{
    public RenderBuffer renderBuffer;

    public CameraComponent(RenderBuffer renderBuffer)
    {
        this.renderBuffer = renderBuffer;
    }
}
