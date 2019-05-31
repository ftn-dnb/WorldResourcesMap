using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Controls;

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
        private int etiquetteCounter = 0;
        private int typeCounter = 0;
        private int resourceCounter = 0;
        public void resetEtiquetteCounter()
        {
            etiquetteCounter = 0;
        }
        public void resetTypeCounter()
        {
            typeCounter = 0;
        }
        public void resetResourceCounter()
        {
            resourceCounter = 0;
        }
        public AutoCompleteFilterPredicate<object> EtiquetteFilter
        {
            get
            {
                return (searchText, obj) =>
                (obj as Etiquette).Id.ToString().StartsWith(searchText)
                && etiquetteCounter++ < 5;
            }
        }

        public AutoCompleteFilterPredicate<object> TypeFilter
        {
            get
            {
                return (searchText, obj) =>
                (obj as ResourceType).Name.StartsWith(searchText)
                && typeCounter++ < 5;
            }
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

        public void SaveResources()
        {

            string dataString = JsonConvert.SerializeObject(MapData.Resources);
            using(StreamWriter writer = new StreamWriter(resourcePath))
            {
                writer.Write(dataString);
            }
        }

        public void SaveEtiquettes()
        {

            string dataString = JsonConvert.SerializeObject(MapData.Etiquettes);
            using (StreamWriter writer = new StreamWriter(etiquettePath))
            {
                writer.Write(dataString);
            }
        }


        public void SaveResourceTypes()
        {

            string dataString = JsonConvert.SerializeObject(MapData.Types);
            using (StreamWriter writer = new StreamWriter(resourcePath))
            {
                writer.Write(dataString);
            }
        }



        public void ReadResources()
        {
            ObservableCollection<Resource> resourceList = new ObservableCollection<Resource>();
            if (File.Exists(resourcePath))
            {
                using (StreamReader reader = new StreamReader(resourcePath))
                {
                    string data = reader.ReadToEnd();
                    if(!data.Equals(""))
                        resourceList = JsonConvert.DeserializeObject<ObservableCollection<Resource>>(data);
                    //ubaci za null
                }

                MapData.Resources = resourceList;
            }
        }


        public void ReadResourcesTypes()
        {
            ObservableCollection<ResourceType> typeList = new ObservableCollection<ResourceType>();
            if (File.Exists(resourceTypePath)){
                using (StreamReader reader = new StreamReader(resourceTypePath))
                {
                    string data = reader.ReadToEnd();
                    if (!data.Equals(""))
                        typeList = JsonConvert.DeserializeObject<ObservableCollection<ResourceType>>(data);
                }

                MapData.Types = typeList;
            }
        }


        public void ReadEtiquette()
        {
            ObservableCollection<Etiquette> etiquetteList = new ObservableCollection<Etiquette>();
            if (File.Exists(etiquettePath))
            {
                using (StreamReader reader = new StreamReader(etiquettePath))
                {
                    string data = reader.ReadToEnd();
                    if (!data.Equals(""))
                        etiquetteList = JsonConvert.DeserializeObject<ObservableCollection<Etiquette>>(data);
                }

                MapData.Etiquettes = etiquetteList;
            }
        }

    }


    
}
