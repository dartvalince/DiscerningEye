/* ===================================================================
 License:
    DiscerningEye - FFXIV Gathering Companion App
    SelectNotificationFileCommand.cs


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

using Microsoft.Win32;
using System;
using System.Windows.Input;

namespace DiscerningEye.Commands.SettingsViewModelCommands
{
    public class SelectNotificationFileCommand : ICommand
    {

        private ViewModels.SettingsViewModel _viewModel;

        public SelectNotificationFileCommand(ViewModels.SettingsViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "MP3 (*.mp3)|*.mp3";
            if((bool)ofd.ShowDialog())
            {
                Properties.Settings.Default.NotificationToneUri = ofd.FileName;
            }
        }
    }
}
