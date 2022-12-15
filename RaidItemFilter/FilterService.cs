using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FileCompiler.PublicAPI;
using HellHades.ArtifactExtractor.LiveUpdates;
using HellHades.ArtifactExtractor.Models;
using HellHades.ArtifactExtractor.RaidReader;
using HellHades.ArtifactExtractor.RaidReader.Windows;
using Newtonsoft.Json;
using RaidArtifactsFilter.Extensions;

namespace RaidArtifactsFilter
{
    public class FilterService
    {
        public Artifact[] Artifacts { get; set; }
         
        public string FilePath { get; set; }

        public async Task Init()
        {
            SyntaxRegistration.Register();

           
            if (!File.Exists(LocalizationHelper.LocalizationFileName))
            {
                /*
                Raid = new RaidCommunication();
                var localization = await Raid.GetLocalizedStrings();
                await using var fs = File.CreateText(Path.Combine(Directory.GetCurrentDirectory(), LocalizationHelper.LocalizationFileName));
                JsonSerializer.Create().Serialize(fs, localization);
                */
            }
            using var sr = File.OpenText(LocalizationHelper.LocalizationFileName);
            var localizationDictionary = JsonSerializer.Create().Deserialize<IReadOnlyDictionary<string, string>>(new JsonTextReader(sr));
            LocalizationHelper.Init(localizationDictionary);

            await UpdateArtifacts();
        }

        public async Task UpdateArtifacts()
        {
            var raidData = GetData();
            
            var artifacts = raidData.Artifacts.ToArray();
            var heroes = raidData.Heroes;

            var equippedArtifactsIds = heroes.SelectMany(el => el.Artifacts ?? new List<int>()).Distinct().ToArray();

            artifacts = artifacts.Where(el => !equippedArtifactsIds.Contains(el.Id)).ToArray();

            Array.Sort(artifacts, new ArtifactComparer());

            Artifacts = artifacts;
        }

        public RaidData GetData()
        {
            var options = new UpdateRaidDataRequestHandlerOptions();
            var raidReaders = Assembly
                .GetAssembly(typeof(RaidReader))?
                .GetTypes()
                .Where(t => typeof(IRaidReader).IsAssignableFrom(t) && typeof(IRaidReader) != t)
                .Select(Activator.CreateInstance)
                .Cast<IRaidReader>();

            if (raidReaders != null)
            {
                var reader = new RaidReader(options, raidReaders,
                    new RaidMemoryReader(new WindowsMemoryReader()));

                if (reader.IsRaidRunning())
                {
                    return reader.LoadData();
                }
            }

            return null;
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
                    return right.GetRankNumber() - left.GetRankNumber();

                if (left.GetRarityNumber() != right.GetRarityNumber())
                    return right.GetRarityNumber() - left.GetRarityNumber();

                if (left.Level != right.Level)
                    return right.Level - left.Level;


                if (left.PrimaryBonus.GetStatNumber() != right.PrimaryBonus.GetStatNumber())
                    return right.PrimaryBonus.GetStatNumber() - left.PrimaryBonus.GetStatNumber();

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
