using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;
using RimWorld;

namespace IntegratedGenes
{
    class MentalBreakWorker_FaintingSpell : MentalBreakWorker
    {
        private const int MaxTicks = GenTicks.TicksPerRealSecond * 30;

        public override bool BreakCanOccur(Pawn pawn)
        {
            if (!base.BreakCanOccur(pawn))
                return false;
            return AnyRecentBattleAttacksTowards(pawn);
        }

        public override bool TryStart(
            Pawn pawn,
            string reason,
            bool causedByMood
        )
        {
            // Send an alert first, in case we snap a slave out of its rebellion
            TrySendAlert(pawn, reason);
            pawn.health.AddHediff(
                MyDefOf.Turn_Hediff_TerrifiedFaintingSpell);
            return true;
        }
        
        // Send a letter under most circumstances, but just a message if the pawn is crazy or slave-rebelling
        public void TrySendAlert(Pawn pawn, string reason)
        {
            if (SlaveRebellionUtility.IsRebelling(pawn) ||
                pawn.InAggroMentalState)
            {
                Messages.Message(
                    "LetterTerrifiedFaintingMentalBreakShort".Translate(
                        pawn.LabelCap, reason, pawn.Named("PAWN")
                    ),
                    pawn,
                    MessageTypeDefOf.NeutralEvent);
                return;
            }
            TrySendLetter(pawn, "LetterTerrifiedFaintingMentalBreak", reason);
        }

        public bool AnyRecentBattleAttacksTowards(Pawn pawn)
        {
            foreach (Battle battle in Find.BattleLog.Battles)
            {
                if (!battle.Concerns(pawn))
                    continue;
                foreach (LogEntry entry in battle.Entries)
                {
                    // Newer entries come first, so stop here if we've exceeded
                    // the maximum time
                    if (entry.Timestamp + MaxTicks < GenTicks.TicksAbs)
                        break;

                    if (EntryIsHostilityTowards(entry, pawn))
                        return true;
                }
            }
            return false;
        }

        public bool EntryIsHostilityTowards(LogEntry entry, Pawn pawn)
        {
            return (entry is BattleLogEntry_RangedFire ||
                entry is BattleLogEntry_MeleeCombat ||
                entry is BattleLogEntry_ExplosionImpact) &&
                entry.Concerns(pawn);
        }
    }
}
