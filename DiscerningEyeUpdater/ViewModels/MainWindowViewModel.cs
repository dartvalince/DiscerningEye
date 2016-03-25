using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscerningEyeUpdater.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        private string _windowTitle;

        public string WindowTitle
        {
            get { return _windowTitle; }
            set { SetProperty(ref _windowTitle, value); }
        }

        private string _statusMessage;

        public string StatusMessage
        {
            get { return _statusMessage; }
            set { SetProperty(ref _statusMessage, value); }
        }

        private int _progress;

        public int Progress
        {
            get { return _progress; }
            set { SetProperty(ref _progress, value); }
        }






        public MainWindowViewModel()
        {
            this.WindowTitle = string.Format("Discerning Eye Updater (v{0})", typeof(MainWindowViewModel).Assembly.GetName().Version);


            
        }

        private void UpdateProgress(int percentage, string message = "")
        {
            if (message != "") this.StatusMessage = message;
            this.Progress = percentage;
        }




        private void CheckForUpdates()
        {
            this.UpdateProgress(0, "Checking GitHub for updates");




        }












    }
}
