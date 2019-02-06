using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.IO;

namespace WorkDelimiter.Model
{
    public class DataContext
    {
        string dbReg = @"DataBase\dbTaskRegular.xml";
        string dbOne = @"DataBase\dbTaskOneTime.xml";
        public TasksOneTime ListTasksOneTime { get; set; }
        public TasksRegular ListTasksRegular { get; set; }
        public DataContext()
        {
            ListTasksOneTime = DeserializeObject<TasksOneTime>(dbOne);

            ListTasksRegular = DeserializeObject<TasksRegular>(dbReg);
        }
        public void UpdateChange()
        {
            SerializeObject<TasksOneTime>(dbOne, ListTasksOneTime);
        }
        public void UpdateTimeRegular()
        {
            foreach (TaskRegular t in ListTasksRegular.Items.Where(p => p.isActual == 1))
                t.UpdateTime();
        }
        T DeserializeObject<T>(string xml)
        {
            FileStream stream = new FileStream(xml,FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stream);
        }
        void SerializeObject<T>(string xml, T val)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (var stream = XmlWriter.Create(xml))
            {
                serializer.Serialize(stream, val);
            }
        }
    }
}
