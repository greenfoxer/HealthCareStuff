using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkDelimiter.Infrastructure
{
    public class Mediator : IMediator
    {
        private static Mediator instance;
        public static Mediator GetInstance()
        {
            if (instance == null)
                instance = new Mediator();
            return instance;
        }
        static IDictionary<string, List<Action<object>>> pl_dict = new Dictionary<string, List<Action<object>>>();

        public void Register(string token, Action<object> callback)
        {
            if (!pl_dict.ContainsKey(token))
            {
                var list = new List<Action<object>>();
                list.Add(callback);
                pl_dict.Add(token, list);
            }
            else
            {
                bool found = false;
                foreach (var item in pl_dict[token])
                    if (item.Method.ToString() == callback.Method.ToString() && item.Target == callback.Target)
                        found = true;
                if (!found)
                    pl_dict[token].Add(callback);
            }
        }
        public void Unregister(string token, Action<object> callback)
        {
            if (pl_dict.ContainsKey(token))
                pl_dict[token].RemoveAll(t => t == callback && t.Target == callback.Target);

        }
        public void NotifyColleagues(string token, object args)
        {
            List<Task> Tasks = new List<Task>();
            if (pl_dict.ContainsKey(token))
                pl_dict[token].ForEach(callback => Tasks.Add(Task.Run(() => { callback(args); })));
            Task.WaitAll(Tasks.ToArray());
            //foreach (var callback in pl_dict[token])
            //    callback(args);
        }

        public void ReleaseViewModel(ViewModelBase vm)
        {
            foreach (var item in pl_dict.Values)
                item.RemoveAll(t => t.Target == vm);
        }
    }
}
