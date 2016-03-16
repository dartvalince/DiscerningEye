/* ===================================================================
 License:
    DiscerningEye - FFXIV Gathering Dictionary and Alarm
    ChangeThemeCommand.cs


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

using MahApps.Metro;
using System;
using System.Windows;
using System.Windows.Input;

namespace DiscerningEye.Commands
{
    public class ChangeThemeCommand : ICommand
    {

        private ViewModel.SettingsViewModel _viewModel;

        public ChangeThemeCommand(ViewModel.SettingsViewModel viewModel)
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
            var theme = ThemeManager.DetectAppStyle(Application.Current);

            ThemeManager.ChangeAppStyle(Application.Current,
                ThemeManager.GetAccent((string)Properties.Settings.Default["UIAccent"]),
                ThemeManager.GetAppTheme((string)Properties.Settings.Default["UIAppTheme"]));
        }
    }
}
