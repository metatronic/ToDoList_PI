using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList
{
    public partial class ToDoListItem : INotifyPropertyChanged
    {
        public int TaskId { get; set; }
        private string taskName;
        public string TaskName
        {
            get => taskName;
            set
            {
                taskName = value;
                OnPropertyChanged("TaskName");
            }
        }
        public string TaskDescription { get; set; }
        private bool taskCompleted;
        public bool TaskCompleted {
            get => taskCompleted;
            set
            {
                taskCompleted = value;
                OnPropertyChanged("TaskCompleted");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
