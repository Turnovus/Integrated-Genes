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
    class MentalState_Rampage : MentalState
    {
        public override bool ForceHostileTo(Thing t) => !(t is Pawn pawn) || StaticUtil.IsFreeColonist(pawn);

        public override bool ForceHostileTo(Faction f) => f.IsPlayer;

        public override RandomSocialMode SocialModeMax() => RandomSocialMode.Off;
    }
}
