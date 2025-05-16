using System;
using System.IO;
using System.Xml;

namespace Project_QuanAo.Helpers
{
    public class XmlConfigurationHelper
    {
        private XmlDocument xmlDoc;

        public XmlConfigurationHelper()
        {
            xmlDoc = new XmlDocument();
            string xmlFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../serverConfig.xml");
            xmlDoc.Load(xmlFilePath);
        }

        public string GetServerName()
        {
            return xmlDoc.SelectSingleNode("//DatabaseSettings/ServerName")?.InnerText;
        }

        public string GetDBName()
        {
            return xmlDoc.SelectSingleNode("//DatabaseSettings/DBName")?.InnerText;
        }
    }
}
