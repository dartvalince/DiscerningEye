/* ===================================================================
 License:
    DiscerningEye - FFXIV Gathering Companion App
    AlarmsView.xaml.cs


    Copyright(C) 2015 - 2016  Christopher Whitley

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.If not, see<http://www.gnu.org/licenses/> .
  =================================================================== */

using DiscerningEye.Models.AlarmItem;
using System.Windows.Controls;


namespace DiscerningEye.Views
{
    /// <summary>
    /// Interaction logic for AlarmsView.xaml
    /// </summary>
    public partial class Alarms : UserControl
    {
        public static Alarms View;
        public Alarms()
        {
            InitializeComponent();
            View = this;
            this.Loaded += Alarms_Loaded;
            
        }

        private void Alarms_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void BotanistViewSource_Filter(object sender, System.Windows.Data.FilterEventArgs e)
        {
            AlarmItem i = e.Item as AlarmItem;
            if(i!= null)
            {
                if ( !string.IsNullOrEmpty( ((ViewModels.AlarmsViewModel)this.DataContext).SearchText))
                {
                    e.Accepted = i.Name.ToLower().Contains(((ViewModels.AlarmsViewModel)this.DataContext).SearchText.ToLower()) && i.Job == "Botanist";
                }
            }
        }

        private void SetAlarmsViewSource_Filter(object sender, System.Windows.Data.FilterEventArgs e)
        {
            AlarmItem i = e.Item as AlarmItem;
            if (i != null)
            {
                    e.Accepted = i.IsSet == true;
            }
        }
    }
}
