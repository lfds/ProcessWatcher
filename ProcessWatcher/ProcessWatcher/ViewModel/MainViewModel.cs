using ProcessWatcher.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessWatcher.ViewModel
{
    public class MainViewModel:ViewModelBase, IDisposable
    {
        public MainViewModel()
        {
            Processes.Add(new ProcessViewModel(new Model.ProcessWatcher("a.txt")));
            Processes.Add(new ProcessViewModel(new Model.ProcessWatcher("b.txt")));
            Processes.Add(new ProcessViewModel(new Model.ProcessWatcher("c.txt")));
        }

        public ObservableCollection<ProcessViewModel> Processes { get; } = new ObservableCollection<ProcessViewModel>();

        public void Dispose()
        {
            foreach(var ProcessViewModel in Processes)
            {
                ProcessViewModel?.Dispose();
            }
        }
    }
}
