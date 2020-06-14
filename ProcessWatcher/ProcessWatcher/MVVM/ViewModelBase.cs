using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Linq.Expressions;

namespace ProcessWatcher.MVVM
{
    /// <summary>
    /// Базовый класс для ViewModel 
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Вызов события <see cref="PropertyChanged"/>
        /// </summary>
        /// <param name="propertyName">Имя изменившегося свойства</param>
        private void RaisePropertyChangedInternal(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Вызов события <see cref="PropertyChanged"/>
        /// </summary>
        /// <typeparam name="TProperty">Тип изменившегося свойства</typeparam>
        /// <param name="projection">лямбда выражение, из которого извлекается имя свойства</param>
        protected void RaisePropertyChanged<TProperty>(Expression<Func<TProperty>> projection)
        {
            MemberExpression memberExpression = (MemberExpression)projection.Body;
            RaisePropertyChangedInternal(memberExpression.Member.Name);
        }

    }
}
