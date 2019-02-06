using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkDelimiter.Infrastructure
{
    public interface IMediator
    {
        void Register(string token, Action<object> callback);
        void Unregister(string token, Action<object> callback);
        void NotifyColleagues(string token, object args);
    }
}
