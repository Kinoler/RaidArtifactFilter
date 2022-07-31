using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raid.DataModel;

namespace RaidArtifactsFilter.Extensions
{
    public static class ArtifactExtensions
    {
        public static int GetRankNumber(this Artifact artifact)
        {
            return Configuration.Instance.ArtifactRankTransfer.ContainsKey(artifact.Rank) ? Configuration.Instance.ArtifactRankTransfer[artifact.Rank] : 0;
        }

        public static int GetRarityNumber(this Artifact artifact)
        {
            return Configuration.Instance.ArtifactRarityTransfer.ContainsKey(artifact.RarityId) ? Configuration.Instance.ArtifactRarityTransfer[artifact.RarityId] : 0;
        }

        public static Color GetRarityColor(this Artifact artifact)
        {
            return Configuration.Instance.ArtifactRarityColorsTransfer.ContainsKey(artifact.RarityId) ? Configuration.Instance.ArtifactRarityColorsTransfer[artifact.RarityId] : Color.Black;
        }

        public static string GetSetKind(this Artifact artifact)
        {
            if (artifact.SetKindId == "None")
                return Configuration.Instance.ArtifactFractionTransfer.ContainsKey(artifact.Faction) ? Configuration.Instance.ArtifactFractionTransfer[artifact.Faction].ToString() : "0";
            return artifact.GetSetKindNumber().ToString();
        }

        public static int GetSetKindNumber(this Artifact artifact)
        {
            return Configuration.Instance.ArtifactSetKindTransfer.ContainsKey(artifact.SetKindId) ? Configuration.Instance.ArtifactSetKindTransfer[artifact.SetKindId] : 1000;
        }

        public static int GetFractionNumber(this Artifact artifact)
        {
            return Configuration.Instance.ArtifactFractionNumberTransfer.ContainsKey(artifact.Faction) ? Configuration.Instance.ArtifactFractionNumberTransfer[artifact.Faction] : 1000;
        }

        public static int GetTypeNumber(this Artifact artifact)
        {
            return Configuration.Instance.ArtifactTypeTransfer.ContainsKey(artifact.KindId) ? Configuration.Instance.ArtifactTypeTransfer[artifact.KindId] : 1000;
        }

        public static string GetStatKindId(this ArtifactStatBonus stat)
        {
            return stat.Absolute ? stat.KindId : $"{stat.KindId}%";
        }

        public static int GetStatNumber(this ArtifactStatBonus stat)
        {
            return Configuration.Instance.ArtifactStatTransfer.ContainsKey(stat.GetStatKindId()) ? Configuration.Instance.ArtifactStatTransfer[stat.GetStatKindId()] : 1000;
        }

        public static int GetStatValue(this ArtifactStatBonus stat)
        {
            return (int)(stat.Absolute ? stat.Value : stat.Value * 100);
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
