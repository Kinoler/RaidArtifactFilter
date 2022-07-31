using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using FileCompiler.PublicAPI;
using Raid.DataModel;
using RaidArtifactsFilter.Extensions;

namespace RaidArtifactsFilter
{
    public class FilterService
    {
        public RaidCommunication Raid { get; set; }

        public Artifact[] Artifacts { get; set; }
         
        public string FilePath { get; set; }

        public async Task Init()
        {
            SyntaxRegistration.Register();

            Raid = new RaidCommunication();
            var localization = await Raid.GetLocalizedStrings();
            LocalizationHelper.Init(localization);


            await UpdateArtifacts();
        }

        public async Task UpdateArtifacts()
        {
            Artifacts = await Raid.GetArtifacts();

            Array.Sort(Artifacts, new ArtifactComparer());
            Array.Reverse(Artifacts);
        }

        public class ArtifactComparer : IComparer<Artifact>
        {
            public int Compare(Artifact x, Artifact y)
            {
                var left = x;
                var right = y;

                if (left == null)
                    return -1;
                if (right == null)
                    return 1;

                if (left.GetSetKindNumber() != right.GetSetKindNumber())
                    return left.GetSetKindNumber() - right.GetSetKindNumber();

                if (left.GetFractionNumber() != right.GetFractionNumber())
                    return left.GetFractionNumber() - right.GetFractionNumber();

                if (left.GetTypeNumber() != right.GetTypeNumber())
                    return left.GetTypeNumber() - right.GetTypeNumber();

                if (left.GetRankNumber() != right.GetRankNumber())
                    return left.GetRankNumber() - right.GetRankNumber();

                if (left.GetRarityNumber() != right.GetRarityNumber())
                    return left.GetRarityNumber() - right.GetRarityNumber();

                if (left.Level != right.Level)
                    return left.Level - right.Level;


                if (left.PrimaryBonus.GetStatNumber() != right.PrimaryBonus.GetStatNumber())
                    return left.PrimaryBonus.GetStatNumber() - right.PrimaryBonus.GetStatNumber();

                var leftSubStatNumber = left.SecondaryBonuses
                    .Select(el => el.GetStatNumber())
                    .Aggregate(0, (seed, el) => seed + el);
                var rightSubStatNumber = right.SecondaryBonuses
                    .Select(el => el.GetStatNumber())
                    .Aggregate(0, (seed, el) => seed + el);

                if (leftSubStatNumber != rightSubStatNumber)
                    return leftSubStatNumber - rightSubStatNumber;

                return 0;
            }
        }

        public void SetFile(string path)
        {
            if (File.Exists(path))
            {
                FilePath = path;
            }
        }

        public Artifact[] GetFilteredItems(bool isKeep)
        {
            var filteredItems = new ConcurrentBag<Artifact>();
            try
            {
                var fileText = File.ReadAllText(FilePath);

                var action = ParserService.GenerateItemFilter<Artifact>(fileText);
                foreach (var artifact in Artifacts)
                {
                    if (isKeep)
                    {
                        if (!action(artifact))
                            filteredItems.Add(artifact);
                    }
                    else
                    {
                        if (action(artifact))
                            filteredItems.Add(artifact);
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception($"File {Path.GetFileNameWithoutExtension(FilePath)}. Error: {e.Message}");
            }

            return filteredItems.ToArray();
        }
    }
}
