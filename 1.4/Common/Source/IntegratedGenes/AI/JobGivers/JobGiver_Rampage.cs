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
    class JobGiver_Rampage : ThinkNode_JobGiver
    {
        public const float WaitChance = 0.25f;
        public const int WaitTicks = 45;
        public const int MinMeleeChaseTicks = 420;
        public const int MaxMeleeChaseTicks = 900;
        public const float MaxAttackDistance = 120f;

        protected override Job TryGiveJob(Pawn pawn)
        {
            Job job;

            if (Rand.Chance(WaitChance))
            {
                job = JobMaker.MakeJob(JobDefOf.Wait_Combat);
                job.expiryInterval = WaitTicks;
                return job;
            }

            if (pawn.TryGetAttackVerb(null) == null)
                return null;
            Pawn target = FindPawnTarget(pawn);
            if (target == null)
                return null;

            job = JobMaker.MakeJob(JobDefOf.AttackMelee, target);
            job.maxNumMeleeAttacks = 3;
            job.expiryInterval = Rand.RangeInclusive(MinMeleeChaseTicks, MaxMeleeChaseTicks);
            job.canBashDoors = true;
            return job;
        }

        public static Pawn FindPawnTarget(Pawn pawn) =>
            AttackTargetFinder.BestAttackTarget(
                pawn,
                TargetScanFlags.NeedReachable,
                t => t is Pawn other && IsValidTarget(other),
                maxDist: MaxAttackDistance,
                canBashDoors: true
            ) as Pawn;

        public static bool IsValidTarget(Pawn pawn) =>
            pawn.Spawned &&
            !pawn.Downed &&
            !pawn.IsInvisible();
    }
}
