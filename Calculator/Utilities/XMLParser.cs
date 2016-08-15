using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

namespace Calculator.Utilities
{
    /// <summary>
    /// XML Parser
    /// 
    /// For processing the xml data
    /// </summary>
    class XMLParser
    {
        /// <summary>
        /// Read XML data from file 
        /// </summary>
        /// <param name="path">If file path not given, it will use its default path</param>
        /// <returns>Data in Dictionary</returns>
        public static Dictionary<String,String> readXML(String path = "")
        {
            String fileName = "UserSettings.xml";
            Dictionary<String, String> data = new Dictionary<String, String>();
            if (path != "")
            {
                fileName = path + "/" + fileName;
            }
            try
            {
                XmlTextReader reader = new XmlTextReader(fileName);
                while (reader.Read())
                {
                    String itemName = "";
                    String itemValue = "";
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            itemName = reader.Name;
                            break;
                        case XmlNodeType.Text:
                            itemValue = reader.Value;
                            break;
                        case XmlNodeType.EndElement:
                            data.Add(itemName, itemValue);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (XmlException ex)
            {
                ErrorLog.logErrorMessage(ex.Message + "; " + ex.Source + "; " + ex.StackTrace.ToString());
            }
            return data; 
        }
        /// <summary>
        /// Write XML data to file 
        /// 
        /// From the given object 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="path"></param>
        public static void writeToXML(Object mydata, String path="")
        {
            String fileName = "UserSettings.xml";
            Dictionary<String, String> data = new Dictionary<String, String>();
            if (path != "")
            {
                fileName = path + "/" + fileName;

            }
            if (mydata == null)
            {
                return;
            }
            else
            {
                try
                {
                    System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(Object));
                    System.IO.FileStream file = System.IO.File.Create(fileName);
                    writer.Serialize(file, mydata);
                    file.Close();
                }
                catch (XmlException ex)
                {
                    ErrorLog.logErrorMessage(ex.Message + "; " + ex.Source + "; " + ex.StackTrace.ToString());
                }
            }
           
        }
    }
}
