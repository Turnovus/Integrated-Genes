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
    class ThinkNode_Priority_GetMeditationGenetic : ThinkNode_Priority
    {
        private const float PRIORITY_OFFSET = 0.5f;

        public override float GetPriority(Pawn pawn)
        {
            if (pawn.needs == null)
                return 0f;

            if (!MeditationUtility.CanMeditateNow(pawn))
                return 0f;

            Need need = pawn.needs.TryGetNeed(
                MyDefOf.Turn_Need_GeneticMeditation);

            // If the pawn is hungry, don't meditate
            if (pawn.needs.food.CurCategory > HungerCategory.UrgentlyHungry)
                return 0;

            if (need == null) return 0f;
            float curLevel = need.CurLevel;

            TimeAssignmentDef assignment = pawn.timetable?.CurrentAssignment ??
                TimeAssignmentDefOf.Anything;

            // Base priorities copied from ThinkNode_Priority_GetJoy
            // Switch doesn't work here because DefOfs are non-constant
            if (assignment == TimeAssignmentDefOf.Anything)
                return curLevel < Need_Genetic.THRESH_LOW ?
                    6f + PRIORITY_OFFSET : 0f; 
            if (assignment == TimeAssignmentDefOf.Joy)
                return curLevel < Need_Genetic.THRESH_HIGH ?
                    //Only meditate if nothing better to do.
                    7f + PRIORITY_OFFSET : 0.01f;
            if (assignment == TimeAssignmentDefOf.Sleep)
                return curLevel < Need_Genetic.THRESH_LOW ?
                    2f + PRIORITY_OFFSET : 0f;
            // If we're forced to meditate, then let normal meditation
            // behavior take over.
            if (assignment == TimeAssignmentDefOf.Meditate)
                return 0f;
            //throw new NotImplementedException();
            return PRIORITY_OFFSET;
        }
    }
}
