/* ===================================================================
 License:
    DiscerningEye - FFXIV Gathering Companion App
    GatheringItemListView.xaml.cs


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

using System.Windows.Controls;
using System.Windows.Input;

namespace DiscerningEye.Views
{
    /// <summary>
    /// Interaction logic for GatheringItemListView.xaml
    /// </summary>
    public partial class GatheringItemListView : UserControl
    {
        public GatheringItemListView()
        {
            InitializeComponent();
            this.DataContext = new ViewModels.GatheringDictionaryViewModel();
        }
    }
}

