using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace OutOfOrderGenerator.ViewModels
{
    public class GongsiName : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify(string propName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        private string name { get; set; }
        public string Name
        {
            get { return name; }
            set
            {
                if(name == value) { return; }
                this.name = value;
                Notify("name");
            }
        }
        
    }

    public class GongsiNameList : ObservableCollection<GongsiName> { }
}
