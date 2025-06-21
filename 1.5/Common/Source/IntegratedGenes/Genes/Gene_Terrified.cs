using RimWorld;
using UnityEngine;
using Verse;

namespace IntegratedGenes
{
    public class Gene_Terrified : Gene
    {
        public override void Tick()
        {
            //FIXME
            Debug.Log(GenTicks.TicksGame);
            
            // Most of these conditions are copied from the base method, but tweaked for our custom break code.
            // Some safety checks have been bypassed, since we know in advance how this Gene class will be implemented.
            if (!pawn.Spawned || !pawn.IsHashIntervalTick(60) || pawn.InMentalState || pawn.Downed || !def.mentalBreakDef.Worker.BreakCanOccur(pawn))
                return;

            if (Rand.MTBEventOccurs(pawn.GetStatValue(MyDefOf.Turn_Stat_TerrifiedFaintingInterval),
                    GenDate.TicksPerDay, 60))
            {
                def.mentalBreakDef.Worker.TryStart(pawn,
                    "MentalStateReason_Gene".Translate() + ": " + this.LabelCap, false);
            }
        }
    }
}