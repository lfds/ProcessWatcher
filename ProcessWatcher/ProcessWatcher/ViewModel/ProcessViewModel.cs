using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessWatcher.Model;
using System.Windows.Input;
using ProcessWatcher.MVVM;

namespace ProcessWatcher.ViewModel
{
    public class ProcessViewModel:ViewModelBase, IDisposable
    {
        private Model.ProcessWatcher _processWatcher;

        /// <summary>
        /// Имя 
        /// </summary>
        public string Name => _processWatcher?.Name;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ProcessViewModel(Model.ProcessWatcher processWatcher)
        {
            _processWatcher = processWatcher;
            _processWatcher.Exited += _processWatcher_Exited;
            StartCommand = new RelayCommand(Start, CanStart);
            StopCommand = new RelayCommand(Stop, CanStop);
        }

        private void _processWatcher_Exited(object sender, EventArgs e)
        {
            RaisePropertyChanged(() => IsRunning);
        }

        /// <summary>
        /// Процесс запущен
        /// </summary>
        public bool IsRunning
        {
            get => _processWatcher.IsRunning;
        }

        /// <summary>
        /// Команда запуска редактора
        /// </summary>
        public ICommand StartCommand { get; private set; }

        /// <summary>
        /// Команда остановки редактора
        /// </summary>
        public ICommand StopCommand { get; private set; }

        /// <summary>
        /// Запуск редактора
        /// </summary>
        private void Start()
        {
            _processWatcher?.Start();
            RaisePropertyChanged(() => IsRunning);
        }

        private bool CanStart()
        {
            return _processWatcher != null && !_processWatcher.IsRunning;
        }

        /// <summary>
        /// остановка редактора
        /// </summary>
        private void Stop()
        {
            _processWatcher?.Stop(); 
            RaisePropertyChanged(() => IsRunning);
        }

        private bool CanStop()
        {
            return _processWatcher != null && _processWatcher.IsRunning;
        }

        public void Dispose()
        {
            _processWatcher.Exited -= _processWatcher_Exited;
            _processWatcher?.Dispose();
        }
    }
}
