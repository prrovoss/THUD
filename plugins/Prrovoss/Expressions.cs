using Turbo.Plugins.Default;

namespace Turbo.Plugins.Prrovoss
{
    using System;
    using System.Globalization;
    using System.Collections.Generic;

    public class ExpressionsPlugin : BasePlugin
    {
        // public static IController Hud { get; set; }
        public IPlayerDefenseInfo Def { get { return Hud.Game.Me.Defense; } }
        public IPlayerOffenseInfo Dmg { get { return Hud.Game.Me.Offense; } }
        public IPlayerStatInfo Stats { get { return Hud.Game.Me.Stats; } }
        public IPlayer Me { get { return Hud.Game.Me; } }

        private Dictionary<string, Func<string>> list;


        public string Value(string type)
        {
            return list[type].Invoke();
        }

        public override void Load(IController hud)
        {
            base.Load(hud);
        }


        public ExpressionsPlugin()
        {
            Enabled = true;
            list = new Dictionary<string, Func<string>>();

            list["*bloodshards-total"] = (() => BasePlugin.ValueToString(Me.Materials.BloodShard, ValueFormat.LongNumber));
            list["*monsters-in-20"] = (() => BasePlugin.ValueToString(Hud.GetPlugin<UtilityCollection>().monsterDensityAroundPlayer(20), ValueFormat.NormalNumber));
            list["*cursor-monsters-in-20"] = (() => BasePlugin.ValueToString(Hud.GetPlugin<UtilityCollection>().monsterDensityAroundCursor(20), ValueFormat.NormalNumber));
            list["*shield"] = (() => Def.CurShield > 0 ? BasePlugin.ValueToString(Def.CurShield, ValueFormat.LongNumber) : null);
            list["*shield-nok"] = (() => Def.CurShield > 0 ? BasePlugin.ValueToString(Def.CurShield, ValueFormat.NormalNumberNoDecimal) : null);
            list["*gold-in-stash"] = (() => BasePlugin.ValueToString(Me.Materials.Gold, ValueFormat.LongNumber));
            list["*gold-in-stash-nok"] = (() => BasePlugin.ValueToString(Me.Materials.Gold, ValueFormat.NormalNumberNoDecimal));
            list["*hp-cur"] = (() => BasePlugin.ValueToString(Def.HealthCur, ValueFormat.LongNumber));
            list["*hp-cur-nok"] = (() => BasePlugin.ValueToString(Def.HealthCur, ValueFormat.NormalNumberNoDecimal));
            list["*hp-cur-pct"] = (() => Def.HealthPct.ToString("F0", CultureInfo.InvariantCulture) + "%");
            list["*hp-max"] = (() => Def.HealthMax.ToString("F0", CultureInfo.InvariantCulture));
            list["*ehp-max"] = (() => BasePlugin.ValueToString(Def.EhpMax, ValueFormat.LongNumber));
            list["*ehp-max-nok"] = (() => BasePlugin.ValueToString(Def.EhpMax, ValueFormat.NormalNumberNoDecimal));
            list["*ehp-cur"] = (() => BasePlugin.ValueToString(Def.EhpCur, ValueFormat.NormalNumberNoDecimal));
            list["*ehp-cur-nok"] = (() => BasePlugin.ValueToString(Def.EhpCur, ValueFormat.NormalNumberNoDecimal));
            list["*resource-cur-pri"] = (() => Stats.ResourceCurPri.ToString("F0", CultureInfo.InvariantCulture));
            list["*resource-cur-sec"] = (() => Stats.ResourceCurSec.ToString("F0", CultureInfo.InvariantCulture));
            list["*resource-max-pri"] = (() => Stats.ResourceMaxPri.ToString("F0", CultureInfo.InvariantCulture));
            list["*resource-max-sec"] = (() => Stats.ResourceMaxSec.ToString("F0", CultureInfo.InvariantCulture));
            list["*resource-pct-pri"] = (() => Stats.ResourcePctPri.ToString("F0", CultureInfo.InvariantCulture) + "%");
            list["*resource-pct-sec"] = (() => Stats.ResourcePctSec.ToString("F0", CultureInfo.InvariantCulture) + "%");
            list["*resource-reg-pri"] = (() => Stats.ResourceRegPri.ToString("F0", CultureInfo.InvariantCulture));
            list["*resource-reg-sec"] = (() => Stats.ResourceRegSec.ToString("F0", CultureInfo.InvariantCulture));
            list["*critchance"] = (() => Dmg.CriticalHitChance.ToString("F1", CultureInfo.InvariantCulture) + "%");
            list["*attackspeed"] = (() => Dmg.AttackSpeed.ToString("F2", CultureInfo.InvariantCulture) + "/s");
            list["*dps-sheet"] = (() => BasePlugin.ValueToString(Dmg.SheetDps, ValueFormat.LongNumber));


            list["*dps-cur"] = (() => BasePlugin.ValueToString(Me.Damage.CurrentDps, ValueFormat.LongNumber));
            list["*dps-run"] = (() => BasePlugin.ValueToString(Me.Damage.RunDps, ValueFormat.LongNumber));
            list["*magicfind"] = (() => Stats.MagicFind.ToString("F0", CultureInfo.InvariantCulture) + "%");
            list["*goldfind"] = (() => Stats.GoldFind.ToString("F0", CultureInfo.InvariantCulture) + "%");
            list["*expbonus"] = (() => (Stats.ExperiencePercentBonus).ToString("F0", CultureInfo.InvariantCulture) + "%");
            list["*exponkill"] = (() => BasePlugin.ValueToString(Stats.ExperienceOnKillBonus, ValueFormat.LongNumber));
            list["*ingame-latency-average"] = (() => BasePlugin.ValueToString(Hud.Game.AverageLatency, ValueFormat.NormalNumberNoDecimal));
            list["*ingame-latency-current"] = (() => BasePlugin.ValueToString(Hud.Game.CurrentLatency, ValueFormat.NormalNumberNoDecimal));
            list["*ingame-latency-combined"] = (() => BasePlugin.ValueToString(Hud.Game.AverageLatency, ValueFormat.NormalNumberNoDecimal) + " / " + BasePlugin.ValueToString(Hud.Game.CurrentLatency, ValueFormat.NormalNumberNoDecimal));
            list["*ingame-ip"] = (() => Hud.Game.ServerIpAddress);
            list["*current-time"] = (() => new TimeSpan(Hud.Game.CurrentRealTimeMilliseconds * 10000).ToString(@"hh\:mm\:ss"));
            list["*game-duration"] = (() => new TimeSpan(Convert.ToInt64(Def.CurrentEffectiveHealingPercent / 60.0f * 10000000)).ToString(@"hh\:mm\:ss"));
            list["*net-healing"] = (() => BasePlugin.ValueToString(Def.CurrentEffectiveHealingPercent, ValueFormat.LongNumber) + "%");
            list["*net-healing-nok"] = (() => BasePlugin.ValueToString(Def.CurrentEffectiveHealingPercent, ValueFormat.NormalNumber) + "%");
            list["*hps-cur"] = (() => Def.CurrentHealingPerSecond > 0 ? BasePlugin.ValueToString(Def.CurrentHealingPerSecond, ValueFormat.LongNumber) : "");
            list["*hps-cur-nok"] = (() => Def.CurrentHealingPerSecond > 0 ? BasePlugin.ValueToString(Def.CurrentHealingPerSecond, ValueFormat.NormalNumberNoDecimal) : "");
            list["*hps-cur-plus"] = (() => Def.CurrentHealingPerSecond > 0 ? "+" + BasePlugin.ValueToString(Def.CurrentHealingPerSecond, ValueFormat.LongNumber) : "");
            list["*hps-cur-plus-nok"] = (() => Def.CurrentHealingPerSecond > 0 ? "+" + BasePlugin.ValueToString(Def.CurrentHealingPerSecond, ValueFormat.NormalNumberNoDecimal) : "");
            list["*dtps-cur"] = (() => Def.CurrentDamageTakenPerSecond > 0 ? BasePlugin.ValueToString(Def.CurrentDamageTakenPerSecond, ValueFormat.LongNumber) : "");
            list["*dtps-cur-nok"] = (() => Def.CurrentDamageTakenPerSecond > 0 ? BasePlugin.ValueToString(Def.CurrentDamageTakenPerSecond, ValueFormat.NormalNumberNoDecimal) : "");
            list["*dtps-cur-minus"] = (() => Def.CurrentDamageTakenPerSecond > 0 ? "-" + BasePlugin.ValueToString(Def.CurrentDamageTakenPerSecond, ValueFormat.LongNumber) : "");
            list["*dtps-cur-minus-nok"] = (() => Def.CurrentDamageTakenPerSecond > 0 ? "-" + BasePlugin.ValueToString(Def.CurrentDamageTakenPerSecond, ValueFormat.NormalNumberNoDecimal) : "");
            list["*dmg-dealt-bonus-ph"] = (() => Dmg.BonusToPhysical == 0 ? "" : (Dmg.BonusToPhysical * 100).ToString("F0", CultureInfo.InvariantCulture));
            list["*dmg-dealt-bonus-f"] = (() => Dmg.BonusToFire == 0 ? "" : (Dmg.BonusToFire * 100).ToString("F0", CultureInfo.InvariantCulture));
            list["*dmg-dealt-bonus-l"] = (() => Dmg.BonusToLightning == 0 ? "" : (Dmg.BonusToLightning * 100).ToString("F0", CultureInfo.InvariantCulture));
            list["*dmg-dealt-bonus-c"] = (() => Dmg.BonusToCold == 0 ? "" : (Dmg.BonusToCold * 100).ToString("F0", CultureInfo.InvariantCulture));
            list["*dmg-dealt-bonus-p"] = (() => Dmg.BonusToPoison == 0 ? "" : (Dmg.BonusToPoison * 100).ToString("F0", CultureInfo.InvariantCulture));
            list["*dmg-dealt-bonus-a"] = (() => Dmg.BonusToArcane == 0 ? "" : (Dmg.BonusToArcane * 100).ToString("F0", CultureInfo.InvariantCulture));
            list["*dmg-dealt-bonus-h"] = (() => Dmg.BonusToHoly == 0 ? "" : (Dmg.BonusToHoly * 100).ToString("F0", CultureInfo.InvariantCulture));
            list["*dmg-dealt-bonus-elite"] = (() => Dmg.BonusToElites == 0 ? "" : (Dmg.BonusToElites * 100).ToString("F0", CultureInfo.InvariantCulture));
            list["*inventory-free-space"] = (() => BasePlugin.ValueToString(Me.InventorySpaceTotal - Hud.Game.InventorySpaceUsed, ValueFormat.NormalNumber));
            list["*cdr"] = (() => BasePlugin.ValueToString(Stats.CooldownReduction * 100, ValueFormat.NormalNumberNoDecimal) + "%");
            list["*pickup"] = (() => Stats.PickupRange.ToString("F0", CultureInfo.InvariantCulture));
            list["*exp-level-cur"] = (() => (Me.CurrentLevelNormal < Me.CurrentLevelNormalCap) ? Me.CurrentLevelNormal.ToString("0") : "p" + Me.CurrentLevelParagonFloat.ToString("0.##", CultureInfo.InvariantCulture));
            list["*exp-to-next-level"] = (() => BasePlugin.ValueToString(Me.ParagonExpToNextLevel, ValueFormat.LongNumber));
            list["*exp-to-next-level-nok"] = (() => BasePlugin.ValueToString(Me.ParagonExpToNextLevel, ValueFormat.NormalNumberNoDecimal));
            list["*exp-in-this-level"] = (() => BasePlugin.ValueToString(Me.ParagonExpInThisLevel, ValueFormat.LongNumber));
            list["*exp-in-this-level-nok"] = (() => BasePlugin.ValueToString(Me.ParagonExpInThisLevel, ValueFormat.NormalNumberNoDecimal));
            list["*exp-remaining-to-next-level"] = (() => BasePlugin.ValueToString(Me.ParagonExpToNextLevel - Me.ParagonExpInThisLevel, ValueFormat.LongNumber));
            list["*exp-remaining-to-next-level-nok"] = (() => BasePlugin.ValueToString(Me.ParagonExpToNextLevel - Me.ParagonExpInThisLevel, ValueFormat.NormalNumberNoDecimal));
            list["*exp-bonus-pool-percent"] = (() => BasePlugin.ValueToString(Me.BonusPoolPercent * 100, ValueFormat.NormalNumber) + "%");
            list["*exp-bonus-pool-remaining"] = (() => BasePlugin.ValueToString(Me.BonusPoolRemaining, ValueFormat.LongNumber));
            list["*exp-bonus-pool-remaining-nok"] = (() => BasePlugin.ValueToString(Me.BonusPoolRemaining, ValueFormat.NormalNumberNoDecimal));
            list["*rift-info-pct"] = (() => Hud.Game.RiftPercentage.ToString("F1", CultureInfo.InvariantCulture) + "%");
            list["*rift-info-pct-remaining"] = (() => (100.0 - Hud.Game.RiftPercentage).ToString("F1", CultureInfo.InvariantCulture) + "%");
            list["*perf-rendertime"] = (() => Hud.Stat.RenderPerfCounter.LastValue.ToString("F0") + " (" + Hud.Stat.RenderPerfCounter.LastCount.ToString("F0") + " FPS)");


            list["*damred"] = (() => Def.DamageReduction.ToString("F1", CultureInfo.InvariantCulture) + "%");
            list["*damred-elite"] = (() => Def.DRElite.ToString("F1", CultureInfo.InvariantCulture) + "%");
            list["*damred-melee"] = (() => Def.DRMelee.ToString("F1", CultureInfo.InvariantCulture) + "%");
            list["*damred-ranged"] = (() => Def.DRRanged.ToString("F1", CultureInfo.InvariantCulture) + "%");
            list["*damred-avr-fromtype"] = (() => Def.AverageDamageReductionFromType.ToString("F1", CultureInfo.InvariantCulture) + "%");

            list["*resist-physical"] = (() => Def.ResPhysical.ToString("F0", CultureInfo.InvariantCulture));
            list["*resist-cold"] = (() => Def.ResCold.ToString("F0", CultureInfo.InvariantCulture));
            list["*resist-fire"] = (() => Def.ResFire.ToString("F0", CultureInfo.InvariantCulture));
            list["*resist-lightning"] = (() => Def.ResLightning.ToString("F0", CultureInfo.InvariantCulture));
            list["*resist-poison"] = (() => Def.ResPoison.ToString("F0", CultureInfo.InvariantCulture));
            list["*resist-arcane"] = (() => Def.ResArcane.ToString("F0", CultureInfo.InvariantCulture));
            list["*resist-lowest"] = (() => Def.ResLowest.ToString("F0", CultureInfo.InvariantCulture));
            list["*armor"] = (() => Def.Armor.ToString("F0", CultureInfo.InvariantCulture));


            //expressions.Add("*perf-memory-usage-gc", (GC.GetTotalMemory(false) / 1024.0 / 1024.0).ToString("F0") + " MB");
            //expressions.Add("*dmg-weapon", BasePlugin.ValueToString(Me.DWHandLeft ? Me.WeaponDMGmh : Me.WeaponDMGoh, ValueFormat.LongNumber));
            //expressions.Add("*exp-all", BasePlugin.ValueToString(Me.ParagonTotalExp, ValueFormat.LongNumber));
            //expressions.Add("*elemental-dps", BasePlugin.ValueToString(Me.SheetDPS * (1 + Me.HighestElementalDamageBonus), ValueFormat.LongNumber));
            //expressions.Add("*elite-dps", BasePlugin.ValueToString(Me.SheetDPS * (1 + Me.HighestElementalDamageBonus) * (1 + Me.BonusToElites), ValueFormat.LongNumber));
            //expressions.Add("*movespeed", Me.MoveSpeed.ToString("F0", CultureInfo.InvariantCulture));
            //expressions.Add("*movespeed-bonus", Me.MoveSpeedBonus.ToString("F0", CultureInfo.InvariantCulture));
            //list["*life-per-second"] = (() => BasePlugin.ValueToString(Hud.Collections.CurHealingPerSecond, ValueFormat.LongNumber));
            //list["*life-per-second-nok"] = (() => BasePlugin.ValueToString(Hud.Collections.CurHealingPerSecond, ValueFormat.NormalNumber));
            //expressions.Add("*attackspeed-pets", Me.AttackSpeedPets.ToString("F2", CultureInfo.InvariantCulture) + "/s");
            //expressions.Add("*healing-sheet", BasePlugin.ValueToString(Me.SheetHealing, ValueFormat.LongNumber));
            //expressions.Add("*healing-sheet-nok", BasePlugin.ValueToString(Me.SheetHealing, ValueFormat.NormalNumberNoDecimal));
            //expressions.Add("*healing-sheet-plus", "+" + BasePlugin.ValueToString(Me.SheetHealing, ValueFormat.LongNumber));
            //expressions.Add("*healing-sheet-plus-nok", "+" + BasePlugin.ValueToString(Me.SheetHealing, ValueFormat.NormalNumberNoDecimal));

            /*
            expressions.Add("*exp-level-time-to-next1", Engine.TimeToParagonLevel(Me.CurrentLevelParagon + 1, true));
            expressions.Add("*exp-level-time-to-next2", Engine.TimeToParagonLevel(Me.CurrentLevelParagon + 2, true));
            expressions.Add("*exp-level-time-to-next3", Engine.TimeToParagonLevel(Me.CurrentLevelParagon + 3, true));
            expressions.Add("*exp-level-time-to-next10", Engine.TimeToParagonLevel(Me.CurrentLevelParagon + 10, true));
            expressions.Add("*exp-level-time-to-next20", Engine.TimeToParagonLevel(Me.CurrentLevelParagon + 20, true));
            expressions.Add("*exp-level-time-to-next50", Engine.TimeToParagonLevel(Me.CurrentLevelParagon + 50, true));
            expressions.Add("*exp-level-time-to-next100", Engine.TimeToParagonLevel(Me.CurrentLevelParagon + 100, true));
            expressions.Add("*exp-level-time-to-next1-value", Engine.TimeToParagonLevel(Me.CurrentLevelParagon + 1, false));
            expressions.Add("*exp-level-time-to-next2-value", Engine.TimeToParagonLevel(Me.CurrentLevelParagon + 2, false));
            expressions.Add("*exp-level-time-to-next3-value", Engine.TimeToParagonLevel(Me.CurrentLevelParagon + 3, false));
            expressions.Add("*exp-level-time-to-next10-value", Engine.TimeToParagonLevel(Me.CurrentLevelParagon + 10, false));
            expressions.Add("*exp-level-time-to-next20-value", Engine.TimeToParagonLevel(Me.CurrentLevelParagon + 20, false));
            expressions.Add("*exp-level-time-to-next50-value", Engine.TimeToParagonLevel(Me.CurrentLevelParagon + 50, false));
            expressions.Add("*exp-level-time-to-next100-value", Engine.TimeToParagonLevel(Me.CurrentLevelParagon + 100, false));
            expressions.Add("*exp-level-exp-to-next1", Engine.ExpToParagonLevel(Me.CurrentLevelParagon + 1));
            expressions.Add("*exp-level-exp-to-next2", Engine.ExpToParagonLevel(Me.CurrentLevelParagon + 2));
            expressions.Add("*exp-level-exp-to-next3", Engine.ExpToParagonLevel(Me.CurrentLevelParagon + 3));
            expressions.Add("*exp-level-exp-to-next10", Engine.ExpToParagonLevel(Me.CurrentLevelParagon + 10));
            expressions.Add("*exp-level-exp-to-next20", Engine.ExpToParagonLevel(Me.CurrentLevelParagon + 20));
            expressions.Add("*exp-level-exp-to-next50", Engine.ExpToParagonLevel(Me.CurrentLevelParagon + 50));
            expressions.Add("*exp-level-exp-to-next100", Engine.ExpToParagonLevel(Me.CurrentLevelParagon + 100));
            expressions.Add("*life-on-hit", BasePlugin.ValueToString(Me.LifeOnHit, ValueFormat.LongNumber));
            expressions.Add("*life-on-hit-nok", BasePlugin.ValueToString(Me.LifeOnHit, ValueFormat.NormalNumber));
            expressions.Add("*life-on-kill", BasePlugin.ValueToString(Me.LifeOnKill, ValueFormat.LongNumber));
            expressions.Add("*life-on-kill-nok", BasePlugin.ValueToString(Me.LifeOnKill, ValueFormat.NormalNumber));
            expressions.Add("*hp-globe-bonus", BasePlugin.ValueToString(Me.GlobeBonus, ValueFormat.LongNumber));
            expressions.Add("*hp-globe-bonus-nok", BasePlugin.ValueToString(Me.GlobeBonus, ValueFormat.NormalNumber));
            expressions.Add("*dps-cur-party": { long t = 0); for (int i = 0) ; i < Collect.Players.Length); i++) t += (Collect.Players[i].ActorKnown() ? Collect.Players[i].CurDPS : 0)); S = BasePlugin.ValueToString(t, ValueFormat.LongNumber)); }
            expressions.Add("*dps-run-party": { long t = 0); for (int i = 0) ; i < Collect.Players.Length); i++) t += Collect.Players[i].RunDPS()); S = BasePlugin.ValueToString(t, ValueFormat.LongNumber)); }
            expressions.Add("*monster-damage", BasePlugin.ValueToString(Collect.Actors.MonsterHitpointDecreaseCounter.LastValue, ValueFormat.LongNumber));
            expressions.Add("*dmg-total", BasePlugin.ValueToString(Me.TotalDMG, ValueFormat.LongNumber));
            expressions.Add("*dmg-total-party": { double t = 0); for (int i = 0) ; i < Collect.Players.Length); i++) t += Collect.Players[i].TotalDMG); S = BasePlugin.ValueToString(t, ValueFormat.LongNumber)); }

            */
        }


    }

}