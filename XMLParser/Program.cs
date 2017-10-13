using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace XMLParser
{
	class Program
	{
		static void Main(string[] args)
		{
			Dictionary<string, Dictionary<string, int>> settings = getOrderWriteData(@"C:\Users\admin\Desktop\Settings.xml");
			makeCSV_File(settings, @"C:\Users\admin\Desktop\test.xml", @"C:\Users\admin\Desktop\test.csv");
			Console.ReadKey();
		}

		private static Dictionary<string, Dictionary<string, int>> getOrderWriteData(string path)
		{
			Dictionary<string, Dictionary<string, int>> result = new Dictionary<string, Dictionary<string, int>>();
			XmlDocument doc = new XmlDocument();
			string dataXml = File.ReadAllText(path);
			doc.LoadXml(dataXml);
			string name;
			int order;

			foreach (XmlNode firstNode in doc.DocumentElement)
			{
				if (firstNode.HasChildNodes)
				{
					Dictionary<string, int> infoNode = new Dictionary<string, int>();
					foreach (XmlNode secondNode in firstNode.ChildNodes)
					{
						name = "";
						order = 0;

						name = secondNode.Attributes.GetNamedItem("NAME").Value;
						order = Int32.Parse(secondNode.Attributes.GetNamedItem("ORDER").Value);

						infoNode.Add(name, order); 
						Console.WriteLine("Имя: " + name + " Порядок: " + order);
					}
					var attr = firstNode.Attributes.GetNamedItem("NAME").Value; ;
					result.Add(attr, infoNode);
				}
				else
				{
					Console.WriteLine("ERROR");
				}		
			}

			return result;
		}

		private static void makeCSV_File(Dictionary<string, Dictionary<string, int>> settings, 
			string pathToXML, string pathToCSV)
		{
			XmlDocument document = new XmlDocument();
			string dataXml = File.ReadAllText(pathToXML);
			document.LoadXml(dataXml);
			var doc = document.FirstChild.FirstChild;
			string name;
			int order;
			string writer = "";
			writer += "DBM_NAME|F1|F2|F3|F4|F5|F6|F7|F8|F9|F10|F11|F12|F13|F14|F15|F16|F17|F18|F19|F20" +
			"|F21|F22|F23|F24|F25|F26|F27|F28|F29|F30|F31|F32|F33|F34|F35|F36|F37\n";
			string temp = "";
			foreach (KeyValuePair<string, Dictionary<string, int>> kvp in settings)
			{
				var firstNode = doc.SelectSingleNode(kvp.Key);
				writer += kvp.Key + ".dbm";
				foreach (KeyValuePair<string, int> properety in kvp.Value)
				{
					try
					{
						temp = "|" + firstNode.Attributes.GetNamedItem(properety.Key).Value;
					}
					catch (System.NullReferenceException e)
					{
						temp = "";
					}
					writer += temp;
				}
				writer += "\n";
			}
			File.WriteAllText(pathToCSV, writer);
		}
	}
}
