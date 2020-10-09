using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Azure.Storage.Blobs;
using Newtonsoft.Json;

namespace Connect4AI.Game
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
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=connect4simulate;AccountKey=9o9X4Unqj7+MsjgFKy0a02nnBs17kDbFnjv284bOmD0D3DvG1yE+OsfAxpOL9MLGJeVqLptWvfj9uO0Rh4XeGQ==;EndpointSuffix=core.windows.net";
            var json = JsonConvert.SerializeObject(this);
            byte[] byteArray = Encoding.ASCII.GetBytes(json);
            MemoryStream stream = new MemoryStream(byteArray);


            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient("gamelogs");
            containerClient.CreateIfNotExists();
            containerClient.UploadBlob(Guid.NewGuid().ToString(), stream);

        }
    }
}
