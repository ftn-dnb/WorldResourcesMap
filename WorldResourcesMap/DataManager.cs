using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace WorldResourcesMap
{
    public class DataManager
    {
        private string mapDataPath = @"../../resources/map_data.json";
        public MapData MapData { get; set; }
        

        public DataManager()
        {
            MapData = new MapData();
            ReadDataFromFile();
        }


        public void SaveDataToFile()
        {
            string dataString = JsonConvert.SerializeObject(MapData);

            using (StreamWriter writer = new StreamWriter(mapDataPath))
            {
                writer.Write(dataString);
            }
        }


        public void ReadDataFromFile()
        {
            if (!File.Exists(mapDataPath))
            {
                MapData = new MapData();
                return;
            }
               
            using (StreamReader reader = new StreamReader(mapDataPath))
            {
                string data = reader.ReadToEnd();
                MapData = JsonConvert.DeserializeObject<MapData>(data);
            }
        }

        


    }


    
}
