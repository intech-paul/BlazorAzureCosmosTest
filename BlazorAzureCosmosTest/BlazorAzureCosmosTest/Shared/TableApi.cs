using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorAzureCosmosTest.Shared
{
    class LineData
    {
        public int index;
        public string label;
        public string units;
        public float value;
        public int status;
        public DateTime lastalarm;

        public LineData(int index, float value, int status)
        {
            this.index = index;
            this.status = status;
            this.value = value;
        }
    }

    class MyData
    {
        public string JobName;
        public string version;
        public string version_api;
        public DateTime time;
        public List<LineData> linedata;

        public MyData()
        {
            linedata = new List<LineData>();
        }
    }

    public class DataEntity : TableEntity
    {
        public DataEntity()
        {
        }
        public DataEntity(DateTime date)
        {
            PartitionKey = date.ToLongDateString();
            RowKey = string.Format("{0:d2}{1:d2}{2:d2}", date.Hour, date.Minute, date.Second);
        }
        public string jsondata { get; set; }
    }

    public class TableApi
    {

        public string reply;
        public string message1="Start";

        public async Task GetAllMessages(CloudTable table)
        {
            TableQuery<DataEntity> query = new TableQuery<DataEntity>();
            bool flag = false;

            Console.WriteLine("GetAllMessages begin");
            foreach (DataEntity message in table.ExecuteQuery(query))
            {
                Console.WriteLine(message.jsondata);
                if (flag==false)
                    {
                    flag = true;
                    message1 = message.jsondata;
                    }
            }
            Console.WriteLine("GetAllMessages ends");
        }

        public async Task GetTable()
        {
            Console.WriteLine("GetTable:Start");

            CloudTable table = await GetCloudTableDB("demoData");

            await GetAllMessages(table);

            Console.WriteLine("GetTable:End");
        }

        public string constring;

        public void GetConnectionString()
        {
        constring= "DefaultEndpointsProtocol=https;AccountName=MicroScan;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw ==;TableEndpoint=https://localhost:8081";
        }


        public async Task<CloudTable> GetCloudTableDB(string tableName)
        {
            int i = 0;

            try
            {
                Console.WriteLine("GetCloudTableDB:" + i++);
                string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=MicroScan;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw ==;TableEndpoint=https://localhost:8081";
                CloudStorageAccount storageAccount;
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);

                // Create a table client for interacting with the table service
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
                CloudTable table = tableClient.GetTableReference(tableName);

                Console.WriteLine("GetCloudTableDB:" + i++);

                await table.CreateIfNotExistsAsync();

                Console.WriteLine("GetCloudTableDB:" + i++);

                return table;
            }
        catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                reply = ex.Message;
                return null;
            }
        }
    }
}
