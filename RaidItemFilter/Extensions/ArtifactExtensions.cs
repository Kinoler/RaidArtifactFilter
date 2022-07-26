using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raid.DataModel;

namespace RaidArtifactsFilter.Extensions
{
    public static class ArtifactExtensions
    {
        public static int ToNumber(this string str, string errorMessage = "The number expected.")
        {
            return int.TryParse(str, out var num) ? num : throw new Exception(errorMessage);
        }

        public static bool IsStatEquals(this ArtifactStatBonus stat, string str)
        {
            return stat.KindId == str;
        }

        public static bool ContainsSubStat(this Artifact artifact, string str)
        {
            return artifact.SecondaryBonuses.Any(stat => stat.IsStatEquals(str));
        }

        public static int GetProcCount(this Artifact artifact, string str)
        {
            return artifact.SecondaryBonuses.FirstOrDefault(stat => stat.IsStatEquals(str))?.Level ?? 0;
        }

        public static int GetRarityNumber(Artifact artifact, string str)
        {
            
            return artifact.SecondaryBonuses.FirstOrDefault(stat => stat.IsStatEquals(str))?.Level ?? 0;
        }

        /*
        public static Rarity GetRarity(string str, string errorMessage = "")
        {
            return Enum.TryParse<Rarity>(str, out var num) ? num : throw new Exception(errorMessage);
        }
        */
    }
}
