using RimWorld;
using Verse;
using LudeonTK;

namespace IntegratedGenes
{
    public class Verb_AbilityGorehulkSpine : Verb_AbilityShoot
    {
        private Extension_AbilityGorehulkSpine Extension =>
            Ability.def.GetModExtension<Extension_AbilityGorehulkSpine>();

        private Hediff_Quills Hediff => Ability.pawn.health.hediffSet.GetFirstHediffOfDef(Extension.hediffDef) as Hediff_Quills;
        
        public override bool Available()
        {
            if (!base.Available())
                return false;

            Hediff quills = Hediff;
            return quills != null && quills.Severity >= 1f;
        }
        
        

        protected override bool TryCastShot()
        {
            Hediff_Quills quills = Hediff;
            if (quills == null)
                return false;
            
            bool success = base.TryCastShot();
            
            if (success)
                quills.ConsumeQuill();
            return success;
        }
    }

    public class Extension_AbilityGorehulkSpine : DefModExtension
    {
        public HediffDef hediffDef;
    }
}