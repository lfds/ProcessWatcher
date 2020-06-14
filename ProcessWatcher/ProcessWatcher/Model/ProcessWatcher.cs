using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using ProcessWatcher.Utils;

namespace ProcessWatcher.Model
{
    /// <summary>
    /// Класс для запуска, остановки и отслеживания процесса текстового редактора
    /// </summary>
    public class ProcessWatcher : IDisposable
    {
        private Process _process = new Process();

        private readonly string _path;

        /// <summary>
        /// Событие, информирует о том что процесс завершился извне
        /// </summary>
        public event EventHandler Exited;

        /// <summary>
        /// Процесс запущен
        /// </summary>
        public bool IsRunning
        {
            get
            {
                if (_process == null)
                    return false;

                try
                {
                    Process.GetProcessById(_process.Id);
                }
                catch (ArgumentException)
                {
                    return false;
                }
                catch (Exception exc)
                {
                    Logger.SaveLogFile("Error getting process status", exc);
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Имя
        /// </summary>
        public string Name => !string.IsNullOrEmpty(_path)
            ? Path.GetFileName(_path)
            : null;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="path">путь к редактируемому файлу</param>
        public ProcessWatcher(string path)
        {
            _path = path;
        }

        /// <summary>
        /// Запуск
        /// </summary>
        public void Start()
        {
            try
            {
                if (_process != null)
                {
                    _process.Exited -= _process_Exited;

                    _process.Dispose(); 
                }

                _process = Process.Start(_path);
                _process.EnableRaisingEvents = true;
                _process.Exited += _process_Exited;
            }
            catch (Exception exc)
            {
                Logger.SaveLogFile($"Error opening file {_path}", exc);
            }
        }

        private void _process_Exited(object sender, EventArgs e)
        {
            Exited?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Остановка
        /// </summary>
        public void Stop()
        {
            try
            {
                _process.Exited -= _process_Exited;
                _process?.Kill();
            }
            catch (Exception exc)
            {
                Logger.SaveLogFile($"Error closing process for file {_path}", exc);
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsRunning)
                _process?.Kill();

            _process?.Dispose();
        }

        /// <summary>
        /// Финализатор
        /// </summary>
        ~ProcessWatcher()
        {
            Dispose(false);
        }
    }
}
