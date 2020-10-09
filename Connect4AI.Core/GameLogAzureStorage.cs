using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Azure.Storage.Blobs;
using Newtonsoft.Json;

namespace Connect4AI.Core
{
    public class GameLogAzureStorage : IGameLog
    {
        public List<GameLogEntry> Log { get; set; } = new List<GameLogEntry>();
        public int Winner { get; set; }

        public void Add(GameLogEntry entry)
        {
            Log.Add(entry);
        }

        public void Save()
        {
            var connectionString = "https://connect4storage.blob.core.windows.net/?sv=2019-12-12&ss=bfqt&srt=sco&sp=rwdlacupx&se=2021-10-10T01:40:23Z&st=2020-10-09T17:40:23Z&spr=https&sig=INqFjtU8ERk3Io%2FPAPX3b62QxBN0FGSaFTAGnNmYb7M%3D";
            var json = JsonConvert.SerializeObject(this);
            byte[] byteArray = Encoding.ASCII.GetBytes(json);
            MemoryStream stream = new MemoryStream(byteArray);


            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient("gamelogs");
            containerClient.UploadBlob(Guid.NewGuid().ToString(), stream);

        }
    }
}
