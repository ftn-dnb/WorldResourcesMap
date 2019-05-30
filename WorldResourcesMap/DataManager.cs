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
        private string resourcePath = @"../../resources/resource_data.json";
        private string resourceTypePath = @"../../resources/type_data.json";
        private string etiquettePath = @"../../resources/etiquette_data.json";
        public MapData MapData { get; set; }
        

        public DataManager()
        {
            MapData = new MapData();
            ReadDataFromFile();
        }


        public void SaveDataToFile()
        {
            //string dataString = JsonConvert.SerializeObject(MapData);

           // using (StreamWriter writer = new StreamWriter(mapDataPath))
            //{
            //    writer.Write(dataString);
           // }
        }


        public void ReadDataFromFile() //TODO promeniti ime f-je
        {
            ReadEtiquette();
            ReadResourcesTypes();
            ReadResources();
        }

        
        public void SaveEtiquette(Etiquette e)
        {
            MapData.Etiquettes.Add(e);
            //SaveDataToFile();

            string dataString = JsonConvert.SerializeObject(MapData.Etiquettes);

            using (StreamWriter writer = new StreamWriter(etiquettePath))
            {
                writer.Write(dataString);
            }

        }

        public void SaveResourceType(ResourceType r)
        {
            MapData.Types.Add(r);
            //SaveDataToFile();

            string dataString = JsonConvert.SerializeObject(MapData.Types);

            using (StreamWriter writer = new StreamWriter(resourceTypePath))
            {
                writer.Write(dataString);
            }

        }

        public void SaveResource(Resource r)
        {
            MapData.Resources.Add(r);
            //SaveDataToFile()

            string dataString = JsonConvert.SerializeObject(MapData.Resources);

            using (StreamWriter writer = new StreamWriter(resourcePath))
            {
                writer.Write(dataString);
            }
        }


        public void ReadResources()
        {
            List<Resource> resourceList = new List<Resource>();
            if (File.Exists(resourcePath))
            {
                using (StreamReader reader = new StreamReader(resourcePath))
                {
                    string data = reader.ReadToEnd();
                    resourceList = JsonConvert.DeserializeObject<List<Resource>>(data);
                }

                MapData.Resources = resourceList;
            }
        }


        public void ReadResourcesTypes()
        {
            List<ResourceType> typeList = new List<ResourceType>();
            if (File.Exists(resourceTypePath)){
                using (StreamReader reader = new StreamReader(resourceTypePath))
                {
                    string data = reader.ReadToEnd();
                    typeList = JsonConvert.DeserializeObject<List<ResourceType>>(data);
                }

                MapData.Types = typeList;
            }
        }


        public void ReadEtiquette()
        {
            List<Etiquette> etiquetteList = new List<Etiquette>();
            if (File.Exists(etiquettePath))
            {
                using (StreamReader reader = new StreamReader(etiquettePath))
                {
                    string data = reader.ReadToEnd();
                    etiquetteList = JsonConvert.DeserializeObject<List<Etiquette>>(data);
                }

                MapData.Etiquettes = etiquetteList;
            }
        }

    }


    
}
