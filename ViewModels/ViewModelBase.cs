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

using Prism.Mvvm;
using System;
using System.ComponentModel;

namespace DiscerningEye.ViewModels
{
    public abstract class ViewModelBase : BindableBase, IDisposable
    {
        protected ViewModelBase()
        {

        }



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
