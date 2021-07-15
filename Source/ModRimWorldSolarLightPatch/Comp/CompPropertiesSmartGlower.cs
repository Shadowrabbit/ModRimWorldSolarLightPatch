// ******************************************************************
//       /\ /|       @file       CompPropertiesSmartGlower.cs
//       \ V/        @brief      智能光照组件参数
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-15 15:06:30
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************
using JetBrains.Annotations;
using RimWorld;

namespace SR.ModRimWorld.SolarLightPatch
{
    [UsedImplicitly]
    public class CompPropertiesSmartGlower : CompProperties_Glower
    {
        public CompPropertiesSmartGlower() => compClass = typeof(CompSmartGlower);
    }
}
