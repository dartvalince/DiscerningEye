/* ===================================================================
 License:
    DiscerningEye - FFXIV Gathering Companion App
    GatheringItemViewModel.cs


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


using DiscerningEye.DataAccess;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace DiscerningEye.ViewModels
{
    public class GatheringDictionaryViewModel : ViewModelBase
    {

        private GatheringItemRepository _gatheringItemRepository;


        private ObservableCollection<Model.XIVDBSharp.ItemRoot> _gatheirngItemsCollection;
        /// <summary>
        /// Gets or sets the list of items
        /// </summary>
        /// <remarks>
        /// This is bound to as the ItemsSource for a datagrid on GatheringItemListView.xaml
        /// </remarks>
        public ObservableCollection<Model.XIVDBSharp.ItemRoot> GatheirngItemsCollection
        {
            get { return _gatheirngItemsCollection; }
            set { SetProperty(ref this._gatheirngItemsCollection, value); }
        }


        private Model.XIVDBSharp.ItemRoot _selectedGatheringItem;
        /// <summary>
        /// Gets or sets the current selected item in the items list datagrid
        /// </summary>
        /// <remarks>
        /// This is bound to the SelectedValue of a datagrid on GatheringItemListView.xaml
        /// </remarks>
        public Model.XIVDBSharp.ItemRoot SelectedGatheringItem
        {
            get { return this._selectedGatheringItem; }
            set { SetProperty(ref this._selectedGatheringItem, value); }
        }


        private Model.XIVDBSharp.Gathering _selectedGatheringType;
        /// <summary>
        /// Gets or sets the current selected gathering item in the gathering information datagrid
        /// </summary>
        /// <remarks>
        /// This is bound to the SelectedValue of a datagrid on GatheringItemListView.xaml
        /// </remarks>
        public Model.XIVDBSharp.Gathering SelectedGatheringType
        {
            get { return this._selectedGatheringType; }
            set { SetProperty(ref this._selectedGatheringType, value); }
        }


        private Model.XIVDBSharp.Node _selectedNodeInformation;
        /// <summary>
        /// Gets or sets the current selected node inofmraiton item in the node information datagrid
        /// </summary>
        /// <remarks>
        /// This is bound to the SelectedValue of a datagrid on GatheringItemListView.xaml
        /// </remarks>
        public Model.XIVDBSharp.Node SelectedNodeInformation
        {
            get { return this._selectedNodeInformation; }
            set { SetProperty(ref this._selectedNodeInformation, value); }
        }


        private string _searchText;
        /// <summary>
        /// Gets or sets the text used to filter the GatheirngItemsCollection view
        /// </summary>
        /// <remarks>
        /// This is bound to the Text property of a TextBox on GatheringItemListView.xaml
        /// </remarks>
        public string SearchText
        {
            get { return this._searchText; }
            set
            {
                SetProperty(ref this._searchText, value);
                this.FilterView(value);
            }
        }


        //=========================================================
        //  Constructor
        //=========================================================
        public GatheringDictionaryViewModel()
        {
            if (_gatheringItemRepository == null)
                _gatheringItemRepository = new GatheringItemRepository();


            //  Initilize the GatheirngItemsCollection
            this.GatheirngItemsCollection = new ObservableCollection<Model.XIVDBSharp.ItemRoot>(_gatheringItemRepository.GetGatheringItems());

            this.SelectedGatheringItem = this.GatheirngItemsCollection[0];

        }


        //=========================================================
        //  Interface implementations
        //=========================================================
        #region IDisposeable Implementation

        protected override void OnDispose()
        {
            this.GatheirngItemsCollection.Clear();
            this.SelectedGatheringType = null;
            this.SelectedNodeInformation = null;
            this.SelectedGatheringItem = null;
        }

        #endregion IDisposeable Implementation



        //=========================================================
        //  Methods
        //=========================================================
        /// <summary>
        /// Filters the GatheringItemCollection based on the filter text given
        /// </summary>
        /// <param name="filterText">The string in which to filter the view by</param>
        private void FilterView(string filterText)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(this.GatheirngItemsCollection);
            if(view.CanFilter)
            {
                view.Filter = item =>
                {
                    Model.XIVDBSharp.ItemRoot vItem = item as Model.XIVDBSharp.ItemRoot;
                    if (vItem == null) return false;
                    return vItem.name.ToLower().Contains(this.SearchText.ToLower());
                };
            }
        }
    }
}
