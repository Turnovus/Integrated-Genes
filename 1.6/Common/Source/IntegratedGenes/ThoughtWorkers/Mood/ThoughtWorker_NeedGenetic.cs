using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace IntegratedGenes
{
    abstract class ThoughtWorker_NeedGenetic : ThoughtWorker
    {
        public abstract NeedDef Need { get; }

        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            Need_Genetic need =
                p.needs.TryGetNeed(Need) as Need_Genetic;
            if (need == null) return ThoughtState.Inactive;

            float level = need.CurLevel;

            int? stage = need.NeedStage;
            if (stage == null) return ThoughtState.Inactive;
            return ThoughtState.ActiveAtStage((int)stage);
        }
    }
}
