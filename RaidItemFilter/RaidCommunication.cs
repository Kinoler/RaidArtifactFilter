using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Raid.Client;
using Raid.DataModel;
using Artifact = Raid.DataModel.Artifact;

namespace RaidArtifactsFilter
{
    public class RaidCommunication
    {
        private static RaidCommunication? _instance;
        public static RaidCommunication Instance => _instance ??= new RaidCommunication();
        
        public async Task<Artifact[]> GetArtifacts()
        {

            var client = new RaidToolkitClient();
            try
            {
                client.Connect();
                var accounts = await client.AccountApi.GetAccounts();
                return await client.AccountApi.GetArtifacts(accounts[0].Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            finally
            {
                client.Disconnect();
            }
        }

        public async Task<StaticArtifactData> GetStaticArtifactData()
        {
            var client = new RaidToolkitClient();
            try
            {
                client.Connect();
                return await client.StaticDataApi.GetArtifactData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            finally
            {
                client.Disconnect();
            }
        }

        public async Task<IReadOnlyDictionary<string, string>> GetLocalizedStrings()
        {
            var client = new RaidToolkitClient();
            try
            {
                client.Connect();
                return await client.StaticDataApi.GetLocalizedStrings();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            finally
            {
                client.Disconnect();
            }
        }
    }
}