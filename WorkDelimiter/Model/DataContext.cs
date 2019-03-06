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
        public void DeleteTask(ITask del)
        {
            if (del.GetType() == typeof(TaskOneTime))
                ListTasksOneTime.Items = ListTasksOneTime.Items.Where(t => t.id != ((TaskOneTime)del).id).ToArray();
            if (del.GetType() == typeof(TaskRegular))
                ListTasksRegular.Items = ListTasksRegular.Items.Where(t => t.id != ((TaskRegular)del).id).ToArray();
        }
        public void UpdateChange()
        {
            SerializeObject<TasksOneTime>(dbOne, ListTasksOneTime);
            SerializeObject<TasksRegular>(dbReg, ListTasksRegular);
        }
        public void UpdateTimeRegular()
        {
            foreach (TaskRegular t in ListTasksRegular.Items.Where(p => p.isActual == 1))
                t.StartTicker();
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
