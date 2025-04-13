using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp
{
    class TaskModel : INotifyPropertyChanged
    {
        private string _taskDescription;
		private bool _isDone;

		public DateTime CreationDate { get; set;  } = DateTime.Now;

        public bool IsDone
		{
			get { return _isDone; }
			set 
            { 
                _isDone = value;
                NotifyPropertyChanged();
            }
        }

        public string TaskDescription
        {
            get { return _taskDescription; }
            set
            {
                _taskDescription = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
