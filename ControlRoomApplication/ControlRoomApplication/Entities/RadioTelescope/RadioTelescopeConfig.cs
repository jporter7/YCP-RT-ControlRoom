using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace ControlRoomApplication.Entities
{
    /// <summary>
    /// Data class for serializing and deserializing JSON strings into
    /// RadioTelescopeConfig objects, used to tell the ControlRoom software
    /// which telescope to run from the database given an ID.
    /// 
    /// If no ID is given, the newTelescope flag is set which tells the 
    /// ControlRoom to create a new one.
    /// </summary>
    public class RadioTelescopeConfig
    {
        private static string FILE_NAME = "RadioTelescopeConfig.json";
        private static string TEST_FILE_NAME = "RadioTelescopeConfigTest.json";
        public static string DEFAULT_JSON_CONTENTS = "{\"telescopeID\":0,\"newTelescope\":true}";
        public int telescopeID { get; set; }
        public bool newTelescope { get; set; }

        /// <contains>An integer representing the ID of a RadioTelescope instance to be retrived from the database</contains>
        /// <contains>A boolean value to represent whether or not a new telescope should be created </contains>
        public RadioTelescopeConfig(int telescopeID, bool newTelescope)
        {
            this.telescopeID = telescopeID;
            this.newTelescope = newTelescope;
        }


        /// <summary>
        /// Method to deserialize the output of the JSON file into a RadioTelescopConfig object. 
        /// File.ReadAllText is guranteed to close the file.
        /// </summary>
        /// <returns>null, if any errors occur in parsing or creating a new RT instance. Otherwise, it will return an instance of an RT</returns>
        public static RadioTelescopeConfig DeserializeRTConfig(bool test = false)
        {
            string fileContents;
            try
            {
                if (!test)
                {
                    fileContents = File.ReadAllText(FILE_NAME);
                }
                else
                {
                    fileContents = File.ReadAllText(TEST_FILE_NAME);
                }
                
            }catch(FileNotFoundException e)
            {
                if (!test)
                {
                    CreateAndWriteToNewJSONFile(DEFAULT_JSON_CONTENTS, false);
                    fileContents = File.ReadAllText(FILE_NAME);
                }
                else
                {
                    CreateAndWriteToNewJSONFile(DEFAULT_JSON_CONTENTS, true);
                    fileContents = File.ReadAllText(TEST_FILE_NAME);
                }
               
            }
            RadioTelescopeConfig RTConfig;

            // check to make sure JSON file contains valid JSON
            try
            {
                JObject.Parse(fileContents);
                RTConfig = JsonConvert.DeserializeObject<RadioTelescopeConfig>(fileContents);
            }
            // file is either corrupted or contains invalid JSON. Return null if this is the case
            catch(JsonReaderException e)
            {
                Console.WriteLine(e);
                return null;
            }
            // also check to ensure the value was not null after parsing (if the user entered no value)
            if(RTConfig.telescopeID.Equals(null) || RTConfig.newTelescope.Equals(null))
            {
                return null;
            }

            return RTConfig;
        }

        /// <summary>
        /// Method to serialize an instance of RadioTelescopeConfig into JSON format and write it to the config file
        /// </summary>
        /// <param name="RTConfig"></param>
        public static void SerializeRTConfig(RadioTelescopeConfig RTConfig, bool test = false)
        { 
            string toJson = JsonConvert.SerializeObject(RTConfig);
            if (!test)
            {
                RadioTelescopeConfig.CreateAndWriteToNewJSONFile(toJson, false);
            }
            else
            {
                RadioTelescopeConfig.CreateAndWriteToNewJSONFile(toJson, true);
            }
            
 
        }


        /// <summary>
        /// Helper method to write to the JSON file, and create it if it does not exist.
        /// </summary>
        /// <param name="fileContents">JSON Contents you wish to write to the JSON file</param>
        public static void CreateAndWriteToNewJSONFile(string fileContents, bool test = false)
        {
            // create the file if it does not exist, and write to it.
            if (!test)
            {
                using (FileStream fs = File.Create(FILE_NAME))
                {
                    byte[] contentsToBytes = new UTF8Encoding(true).GetBytes(fileContents);
                    fs.Write(contentsToBytes, 0, contentsToBytes.Length);
                }
            }
            else
            {
                using (FileStream fs = File.Create(TEST_FILE_NAME))
                {
                    byte[] contentsToBytes = new UTF8Encoding(true).GetBytes(fileContents);
                    fs.Write(contentsToBytes, 0, contentsToBytes.Length);
                }
            }
            
        }
    }
}