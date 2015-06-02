
namespace StoreAppTest.Utilities
{
    using System;
    using System.Windows.Input;

    public class UICommand : ICommand
    {
        private Action<Object> _action;

        public UICommand(Action<Object> action)
        {
            _action = action;
        }

        #region Implementation of ICommand

        /// <summary>
        /// Определяет метод, который определяет, может ли данная команда выполняться в ее текущем состоянии.
        /// </summary>
        /// <returns>
        /// Значение true, если команда может быть выполнена; в противном случае — значение false.
        /// </returns>
        /// <param name="parameter">Данные, используемые данной командой.Если для данной команды не требуется передача данных, можно присвоить этому объекту значение null.</param>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Определяет метод, вызываемый при вызове данной команды.
        /// </summary>
        /// <param name="parameter">Данные, используемые данной командой.Если для данной команды не требуется передача данных, можно присвоить этому объекту значение null.</param>
        public void Execute(object parameter)
        {
            if (_action != null)
                _action(parameter);
        }

        public event EventHandler CanExecuteChanged;

        #endregion
    }
}
