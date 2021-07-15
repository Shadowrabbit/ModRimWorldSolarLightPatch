// ******************************************************************
//       /\ /|       @file       CompSmartGlower.cs
//       \ V/        @brief      智能光照组件
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-15 13:44:16
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************
using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace SR.ModRimWorld.SolarLightPatch
{
    [UsedImplicitly]
    public class CompSmartGlower : CompGlower
    {
        private const float StartTime = 0.25f;
        private const float EndTime = 0.8f;
        private float _powerCost; //正常功率消耗
        private float _lowPowerCost; //低功耗消耗
        private ColorInt glowColor; //正常功率光照
        private ColorInt lowPowerGlowColor; //低功耗光照
        private bool _isInLowPowerMode; //模式
        private bool? cacheMode; //模式缓存

        public override void CompTick()
        {
            base.CompTick();
            if (Find.TickManager.TicksGame % 600 != 0)
            {
                return;
            }
            CalcMode();
            if (cacheMode == _isInLowPowerMode)
            {
                return;
            }
            cacheMode = _isInLowPowerMode;
            UpdateGlow();
        }

        /// <summary>
        /// 序列化
        /// </summary>
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref _isInLowPowerMode, "_isInLowPowerMode");
        }

        /// <summary>
        /// 生成回调
        /// </summary>
        /// <param name="respawningAfterLoad"></param>
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            //初始化参数
            glowColor = Props.glowColor;
            lowPowerGlowColor = glowColor * 0.6f;
            var compPowerTrader = parent.GetComp<CompPowerTrader>();
            _powerCost = compPowerTrader.Props.basePowerConsumption;
            _lowPowerCost = _powerCost / 2;
        }

        /// <summary>
        /// 更新光照
        /// </summary>
        private void UpdateGlow()
        {
            //更新电力消耗
            var compPowerTrader = parent.GetComp<CompPowerTrader>();
            if (compPowerTrader == null)
            {
                Log.Error($"[SR.ModRimWorld.SolarLightPatch]can't find CompPowerTrader in {parent.Label}");
                return;
            }
            compPowerTrader.Props.basePowerConsumption = _isInLowPowerMode ? _powerCost : _lowPowerCost;
            //更新光照
            Props.glowColor = _isInLowPowerMode ? glowColor : lowPowerGlowColor;
            //重新绘制光照网格
            UpdateLit(parent.Map);
        }

        /// <summary>
        /// 模式计算
        /// </summary>
        private void CalcMode()
        {
            //夜间低功率模式
            var num = GenLocalDate.DayPercent(parent);
            _isInLowPowerMode = num <= StartTime && num >= EndTime;
        }
    }
}
