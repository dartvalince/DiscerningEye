using DiscerningEye.Models.AlarmItem;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscerningEye.Events
{
    public class AddAlarmEvent : PubSubEvent<AlarmItem>
    {
    }
}
