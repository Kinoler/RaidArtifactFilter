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

        private readonly RaidToolkitClient _client;

        public RaidCommunication()
        {
            _client = new RaidToolkitClient();
        }

        public async Task<Artifact[]> GetArtifacts()
        {
            try
            {
                _client.Connect();
                var accounts = await _client.AccountApi.GetAccounts();
                return await _client.AccountApi.GetArtifacts(accounts[0].Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            finally
            {
                _client.Disconnect();
            }
        }

        public async Task<StaticArtifactData> GetStaticArtifactData()
        {
            try
            {
                _client.Connect();
                return await _client.StaticDataApi.GetArtifactData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            finally
            {
                _client.Disconnect();
            }
        }
        public async Task<IReadOnlyDictionary<string, string>> GetLocalizedStrings()
        {
            try
            {
                _client.Connect();
                return await _client.StaticDataApi.GetLocalizedStrings();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            finally
            {
                _client.Disconnect();
            }
        }
    }
}