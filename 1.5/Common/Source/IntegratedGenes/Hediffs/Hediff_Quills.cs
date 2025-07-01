using Verse;

namespace IntegratedGenes
{
    public class Hediff_Quills : HediffWithComps
    {
        private Extension_HediffQuills Extension => def.GetModExtension<Extension_HediffQuills>();
        
        public override bool ShouldRemove => base.ShouldRemove || pawn.genes?.HasActiveGene(Extension.geneDef) != true;
        
        public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            base.Notify_PawnPostApplyDamage(dinfo, totalDamageDealt);

            if (dinfo.Def.isRanged || !Rand.Chance(Extension.meleeReflectChance) || Severity < Extension.severityPerReflect)
                return;

            Severity -= Extension.severityPerReflect;
            // TODO: Apply damage
        }

        public void ConsumeQuill() => Severity -= 1;
    }

    public class Extension_HediffQuills : DefModExtension
    {
        public GeneDef geneDef;
        public float meleeReflectChance;
        public float severityPerReflect;
    }
}