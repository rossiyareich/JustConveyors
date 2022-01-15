using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JustConveyors.Source.Drawing;
using JustConveyors.Source.Rendering;
using JustConveyors.Source.Scripting;

namespace JustConveyors.Source.UI
{
    internal static class ArwConveyorHelper
    {
        public static TexturePool GetArwFromBlock(this Drawable drawable)
        {
            if (drawable.ParentPool.ScriptType == typeof(TitaniumConveyorBaseScript))
            {
                return PoolResources.BlueConveyorArrowPool;
            }
            else if (drawable.ParentPool.ScriptType == typeof(ArmoredConveyorBaseScript))
            {
                return PoolResources.RedConveyorArrowPool;
            }
            else if (drawable.ParentPool.ScriptType == typeof(ThoriumConveyorBaseScript))
            {
                return PoolResources.PurpleConveyorArrowPool;
            }

            throw new Exception("Not a conveyor");
        }
    }
}
