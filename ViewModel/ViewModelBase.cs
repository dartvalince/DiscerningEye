/* ===================================================================
 License:
    DiscerningEye - FFXIV Gathering Companion App
    VIewModelBase.cs


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

using System;
using System.ComponentModel;

namespace DiscerningEye.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        protected ViewModelBase()
        {

        }

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        #endregion INotifyPropertyChanged Implementation


        #region IDisposible Implementation

        public void Dispose()
        {
            this.OnDispose();
        }

        protected virtual void OnDispose()
        {

        }

        #endregion IDisposible Implementation
    }
}
