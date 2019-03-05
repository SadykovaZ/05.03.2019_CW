using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
namespace _05._03._2019
{
    public struct HabrNews
    {
        public string title { get; set; }
        public string link { get; set; }
        public string description { get; set; }
        public DateTime pubDate { get; set; }

        public override string ToString()
        {
            string str = string.Format("{0}\n-->{1}\n-->{2:dd.MM.yyyy}\n", title, link, pubDate);
            return str;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            foreach (HabrNews item in GetHabrNews())
                Console.WriteLine(item);          

        }

        static List<HabrNews> GetHabrNews()
        {
            List<HabrNews> habrNewses = new List<HabrNews>();
            foreach (XmlNode item in GetXmlDocument("https://habr.com/ru/rss/interesting/").SelectNodes("//rss/channel/item"))
            {
                HabrNews hn = new HabrNews();
                hn.title = item.SelectSingleNode("title").InnerText;
                hn.link = item.SelectSingleNode("link").InnerText;
                hn.link = item.SelectSingleNode("description").InnerText;
                hn.pubDate = Convert.ToDateTime(item.SelectSingleNode("pubDate").InnerText);
                habrNewses.Add(hn);
            }


            return habrNewses;
        }

        static XmlDocument GetXmlDocument(string link)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(link);
            return doc;
        }
        static void Ex1()
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "utf-8", null);

            //<note></note>
            XmlElement note = doc.CreateElement("note");
            XmlAttribute date = doc.CreateAttribute("date");
            date.InnerText = DateTime.Now.ToShortDateString();
            note.Attributes.Append(date);
            XmlElement to = doc.CreateElement("to");
            to.InnerText = "Tove";
            XmlElement from = doc.CreateElement("from");
            from.InnerText = "Jani";

            XmlElement heading = doc.CreateElement("heading");
            heading.InnerText = "Напоминание";

            XmlElement body = doc.CreateElement("body");
            body.InnerText = "Не забудь";


            note.AppendChild(to);
            //<note> <to></to></note>
            note.AppendChild(from);

            //<note> <to></to><from</from>></note>

            note.AppendChild(heading);
            note.AppendChild(body);

            doc.AppendChild(note);
            //doc.InsertBefore(xmlDeclaration, note);
            doc.Save("note.xml");          //
        }
        static string GetXml()
        {
            return @"< note date = '05.03.2019' > < to > Tove </ to > < from > Jani </ from > < heading > Напоминание </ heading > < body > Не забудь </ body >  </ note > ";
        }

        static void Ex2()
        {
            XmlDocument doc = new XmlDocument();
            ////ссылка на ресурс
            doc.Load("https://news.rambler.ru/rss/world/");

            //doc.Load("note.xml");

            //doc.LoadXml(GetXml());

            foreach (XmlNode root in doc.DocumentElement.ChildNodes)
            {
                foreach (XmlNode channel in root.ChildNodes)
                {
                    if (channel.Name.Equals("title"))
                    {
                        string title = channel.InnerText;
                        Console.WriteLine(title);
                    }
                }

            }
            XmlNode titleNode = doc.SelectSingleNode("//rss/channel/title");
            Console.WriteLine(titleNode.InnerText);
            foreach (XmlNode item in doc.SelectNodes("//rss/channel/item"))
            {
                XmlNode guideNode = item.SelectSingleNode("guid");
                Console.WriteLine(guideNode.InnerText);

                //var isPermaLink = item.Attributes;
                foreach (XmlAttribute attr in guideNode.Attributes)
                {
                    Console.WriteLine("{0} - {1}", attr.Name, attr.InnerText);
                }
            }

        }
    }
}

