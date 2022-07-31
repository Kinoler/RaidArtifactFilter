using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raid.DataModel;
using RaidArtifactsFilter.Extensions;

namespace RaidArtifactsFilter
{
    public static class ArgumentMatcher
    {
        public static bool IsStatEquals(this ArtifactStatBonus stat, string str)
        {
            var statKindId = stat.GetStatKindId();
            var argKindId = str.ResolveStat();
            return argKindId.Any(el => el == statKindId);
        }

        public static bool IsArtifactTypeEquals(this Artifact artifact, string type)
        {
            var artifactKindId = artifact.KindId;
            var argKindId = type.ResolveArtifactType();
            return argKindId.Any(el => el == artifactKindId);
        }

        public static bool IsArtifactSetEquals(this Artifact artifact, string set)
        {
            var artifactKindId = artifact.SetKindId;
            var argKindId = set.ResolveSet();
            return argKindId.Any(el => el == artifactKindId);
        }

        public static bool ContainsSubStat(this Artifact artifact, string str)
        {
            return artifact.SecondaryBonuses.Any(stat => stat.IsStatEquals(str));
        }

        public static int GetProcCount(this Artifact artifact, string str)
        {
            return artifact.SecondaryBonuses.FirstOrDefault(stat => stat.IsStatEquals(str))?.Level ?? 0;
        }
    }
}
