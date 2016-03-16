using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardcodet.Wpf.TaskbarNotification;
using System.Drawing;

namespace DiscerningEye.Utilities
{
    public class NotificationController
    {
        Hardcodet.Wpf.TaskbarNotification.TaskbarIcon _tbIcon;


       public TaskbarIcon TbIcon
        {
            get { return this._tbIcon; }
            set
            {
                if (this._tbIcon == value) return;
                this._tbIcon = value;
            }
        }








        public NotificationController(TaskbarIcon tbIcon)
        {
            this.TbIcon = tbIcon;
        }











    }
}
