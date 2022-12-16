using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HellHades.ArtifactExtractor.Models;

namespace RaidArtifactsFilter.Extensions
{
    public static class ArtifactExtensions
    {
        public static int GetRankNumber(this Artifact artifact)
        {
            return (int)artifact.Rank;
        }

        public static int GetRarityNumber(this Artifact artifact)
        {
            return (int)artifact.Rarity;
        }

        public static Color GetRarityColor(this Artifact artifact)
        {
            return Configuration.Instance.ArtifactRarityColorsTransfer.ContainsKey(artifact.Rarity.ToString()) ? Configuration.Instance.ArtifactRarityColorsTransfer[artifact.Rarity.ToString()] : Color.Black;
        }

        public static string GetSetKind(this Artifact artifact)
        {
            if (artifact.Set == ArtifactSet.None)
                return Configuration.Instance.ArtifactFractionTransfer.ContainsKey(artifact.RequiredFraction.ToString()) ? Configuration.Instance.ArtifactFractionTransfer[artifact.RequiredFraction.ToString()] : artifact.RequiredFraction.ToString();
            return artifact.GetSetKindNumber().ToString();
        }

        public static int GetSetKindNumber(this Artifact artifact)
        {
            return (int)artifact.Set;
        }

        public static int GetFractionNumber(this Artifact artifact)
        {
            return (int)artifact.RequiredFraction;
        }

        public static int GetTypeNumber(this Artifact artifact)
        {
            return Configuration.Instance.ArtifactTypeTransfer.ContainsKey(artifact.Kind.ToString()) ? Configuration.Instance.ArtifactTypeTransfer[artifact.Kind.ToString()] : 1000;
        }

        public static string GetStatKindId(this ArtifactBonus stat)
        {
            return $"{stat.Kind}{(stat.IsAbsolute ? "" : "%")}";
        }

        public static int GetStatNumber(this ArtifactBonus stat)
        {
            return Configuration.Instance.ArtifactStatTransfer.ContainsKey(stat.GetStatKindId()) ? Configuration.Instance.ArtifactStatTransfer[stat.GetStatKindId()] : 1000;
        }

        public static int GetStatValue(this ArtifactBonus stat)
        {
            return (int)(stat.IsAbsolute ? stat.Value : stat.Value * 100);
        }

        public static (IEnumerable<T>, IEnumerable<T>) Determine<T>(this IEnumerable<T> list, Predicate<T> action)
        {
            var trueVal = new List<T>();
            var falseVal = new List<T>();
            foreach (var item in list)
            {
                if (action(item))
                {
                    trueVal.Add(item);
                }
                else
                {
                    falseVal.Add(item);
                }
            }
            return (trueVal, falseVal);
        }
    }
}
