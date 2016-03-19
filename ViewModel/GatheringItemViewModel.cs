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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace DiscerningEye.ViewModel
{
    public class GatheringItemViewModel : ViewModelBase
    {
        //=========================================================
        //  Private Fields
        //=========================================================
        private GatheringItemRepository _gatheringItemRepository;
        private ObservableCollection<Model.XIVDBSharp.ItemRoot> _gatheirngItemsCollection;
        private ObservableCollection<Model.XIVDBSharp.Gathering> _gatheringInformationCollection;
        private ObservableCollection<Model.XIVDBSharp.Node> _nodeInformationCollection;
        private Model.XIVDBSharp.ItemRoot _selectedGatheringItem;
        private Model.XIVDBSharp.Gathering _selectedGatheringInformation;
        private Model.XIVDBSharp.Node _selectedNodeInformation;
        private string _searchText;




        //=========================================================
        //  Propertes
        //=========================================================

        /// <summary>
        /// Gets or sets the list of items
        /// </summary>
        /// <remarks>
        /// This is bound to as the ItemsSource for a datagrid on GatheringItemListView.xaml
        /// </remarks>
        public ObservableCollection<Model.XIVDBSharp.ItemRoot> GatheirngItemsCollection
        {
            get { return _gatheirngItemsCollection; }
            set
            {
                if (this._gatheirngItemsCollection == value) return;
                this._gatheirngItemsCollection = value;
                OnPropertyChanged("GatheirngItemsCollection");
            }
        }

        /// <summary>
        /// Gets or sets the collection of Gathering Information for the current selecteditme
        /// </summary>
        /// <remarks>
        /// This is bound to as the ItemsSource for a datagrid on GatheringItemListView.xaml
        /// </remarks>
        public ObservableCollection<Model.XIVDBSharp.Gathering> GatheringInformationCollection
        {
            get { return this._gatheringInformationCollection; }
            set
            {
                if (this._gatheringInformationCollection == value) return;
                this._gatheringInformationCollection = value;

                OnPropertyChanged("GatheringInformationCollection");

                //  Set the selected Gathering Informaiton item 
                if (value != null)
                    this.SelectedGatheringInformation = value[0];
            }
        }

        /// <summary>
        /// Gets or sets the collection of Node Information for the current selected gathering
        /// information item
        /// </summary>
        /// <remarks>
        /// This is bound as the ItemSource for a datagrid on GatheringItemListView.xaml
        /// </remarks>
        public ObservableCollection<Model.XIVDBSharp.Node> NodeInformationCollection
        {
            get { return this._nodeInformationCollection; }
            set
            {
                if (this._nodeInformationCollection == value) return;
                this._nodeInformationCollection = value;
                OnPropertyChanged("NodeInformationCollection");

                //  Set the selected Gathering Informaiton item 
                if (value != null)
                    this.SelectedNodeInformation = value[0];
            }
        }

        /// <summary>
        /// Gets or sets the current selected item in the items list datagrid
        /// </summary>
        /// <remarks>
        /// This is bound to the SelectedValue of a datagrid on GatheringItemListView.xaml
        /// </remarks>
        public Model.XIVDBSharp.ItemRoot SelectedGatheringItem
        {
            get { return this._selectedGatheringItem; }
            set
            {
                if (this._selectedGatheringItem == value) return;
                this._selectedGatheringItem = value;
                OnPropertyChanged("SelectedGatheringItem");

                //  Initilize the GatheringInformationCollection based on this
                if (value != null)
                    this.GatheringInformationCollection = new ObservableCollection<Model.XIVDBSharp.Gathering>(this.RetreiveGatheringListFromItemRoot(value));
                
            }

        }


        /// <summary>
        /// Gets or sets the current selected gathering item in the gathering information datagrid
        /// </summary>
        /// <remarks>
        /// This is bound to the SelectedValue of a datagrid on GatheringItemListView.xaml
        /// </remarks>
        public Model.XIVDBSharp.Gathering SelectedGatheringInformation
        {
            get { return this._selectedGatheringInformation; }
            set
            {
                if (this._selectedGatheringInformation == value) return;
                this._selectedGatheringInformation = value;
                OnPropertyChanged("SelectedGatheringInformation");

                //  Initilize the NodeInformationCollection based on this
                if(value != null)
                    this.NodeInformationCollection = new ObservableCollection<Model.XIVDBSharp.Node>(this.RetreiveNodeListFromGathering(value));
            }

        }

        /// <summary>
        /// Gets or sets the current selected node inofmraiton item in the node information datagrid
        /// </summary>
        /// <remarks>
        /// This is bound to the SelectedValue of a datagrid on GatheringItemListView.xaml
        /// </remarks>
        public Model.XIVDBSharp.Node SelectedNodeInformation
        {
            get { return this._selectedNodeInformation; }
            set
            {
                if (this._selectedNodeInformation == value) return;
                this._selectedNodeInformation = value;
                OnPropertyChanged("SelectedNodeInformation");
            }
        }


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
                if (this._searchText == value) return;
                this._searchText = value;
                this.FilterView(value);
                OnPropertyChanged("SearchText");
            }
        }


        //=========================================================
        //  Constructor
        //=========================================================
        //public GatheringItemViewModel(Canvas mapCanvas)
        public GatheringItemViewModel()
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
            this.SelectedGatheringInformation = null;
            this.SelectedNodeInformation = null;
            this.SelectedGatheringItem = null;
        }

        #endregion IDisposeable Implementation



        //=========================================================
        //  Methods
        //=========================================================
        /// <summary>
        /// Retreives a list of Gathering information from the given ItemRoot object
        /// </summary>
        /// <param name="itemRoot">The ItemRoot item to retrieve the gathering information and build a list upon</param>
        /// <returns></returns>
        private List<Model.XIVDBSharp.Gathering> RetreiveGatheringListFromItemRoot(Model.XIVDBSharp.ItemRoot itemRoot)
        {
            if (itemRoot == null) return new List<Model.XIVDBSharp.Gathering>();
            List<Model.XIVDBSharp.Gathering> gatherings = new List<Model.XIVDBSharp.Gathering>();

            foreach (Model.XIVDBSharp.Gathering gathering in itemRoot.gathering)
            {
                gathering.stars_html = System.Net.WebUtility.HtmlDecode(gathering.stars_html);
                gatherings.Add(gathering);
            }
            return gatherings;
        }


        /// <summary>
        /// Retreives a list of node information from the given Gathering object
        /// </summary>
        /// <param name="gathering">The Gathering object to retrieve the node information and build a list upon</param>
        /// <returns></returns>
        private List<Model.XIVDBSharp.Node> RetreiveNodeListFromGathering(Model.XIVDBSharp.Gathering gathering)
        {
            if (gathering == null) return new List<Model.XIVDBSharp.Node>();

            List<Model.XIVDBSharp.Node> nodes = new List<Model.XIVDBSharp.Node>();
            foreach (Model.XIVDBSharp.Node node in gathering.nodes)
            {
                node.gathering = gathering.type.name;
                nodes.Add(node);
            }
            return nodes;

        }

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
