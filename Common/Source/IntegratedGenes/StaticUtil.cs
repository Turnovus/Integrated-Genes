using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace IntegratedGenes
{
    public static class StaticUtil
    {
        private static IEnumerable<RoyalTitleDef> royalTitlesSorted;

        private static IEnumerable<RoyalTitleDef> RoyalTitlesSorted
        {
            get
            {
                if (royalTitlesSorted == null)
                {
                    royalTitlesSorted = DefDatabase<RoyalTitleDef>.AllDefs.OrderBy(t => t.seniority);
                }
                return royalTitlesSorted;
            }
        }

        public static bool IsFreeColonist(Pawn pawn)
        {
            if (pawn.Faction?.IsPlayer != true)
                return false;

            if (!pawn.RaceProps.Humanlike)
                return true;

            return !pawn.IsSlave && !pawn.IsPrisoner;
        }

        public static bool HasAnyTitleOrRole(this Pawn pawn)
        {
            if (pawn.HasRoyalTitle())
                return true;

            return pawn.Ideo?.GetRole(pawn) != null;
        }

        public static bool HasRoyalTitle(this Pawn pawn)
        {
            if (Faction.OfEmpire == null)
                return false;

            RoyalTitleDef title = pawn.GetCurrentTitleIn(Faction.OfEmpire);
            return title != null && title.seniority > 0;
        }

        public static int RoyalTitleLevel(this Pawn pawn)
        {
            if (Faction.OfEmpire == null)
                return 0;

            RoyalTitleDef title = pawn.GetCurrentTitleIn(Faction.OfEmpire);
            if (title == null || title.seniority <= 0)
                return 0;

            int n = 0;
            foreach (RoyalTitleDef checkingTitle in RoyalTitlesSorted)
            {
                if (checkingTitle.seniority > 0)
                    n += 1;
                if (title == checkingTitle)
                    return n;
            }

            return 0;
        }

        public static ERoleType RoleType(this Pawn pawn)
        {
            if (pawn.Ideo == null)
                return ERoleType.None;

            Precept_Role role = pawn.Ideo.GetRole(pawn);
            if (role == null)
                return ERoleType.None;

            if (role.def == PreceptDefOf.IdeoRole_Leader)
                return ERoleType.Leader;
            if (role.def == PreceptDefOf.IdeoRole_Moralist)
                return ERoleType.Moralist;

            return ERoleType.Specialist;
        }

        public enum ERoleType
        {
            None,
            Leader,
            Moralist,
            Specialist,
        }
    }
}
