using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using cwContextGenerator.Model;

namespace cwContextGenerator.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        #region Events

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        // Raise PropertyChanged event
        void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Fields

        // Current person
        Configuration current;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets current person
        /// </summary>
        public Configuration Current
        {
            get { return current; }
            set
            {
                if (current == value) return;
                current = value;
                RaisePropertyChanged("Current");
            }
        }

        /// <summary>
        /// Gets persons
        /// </summary>
        public ConfigurationCollection Configurations { get; private set; }

        #endregion

        #region Commands

        #region Exit

        private RelayCommand _exitCommand;

        /// <summary>
        /// Exit from the application
        /// </summary>
        public ICommand ExitCommand
        {
            get
            {
                if (_exitCommand == null)
                    _exitCommand = new RelayCommand(x => System.Windows.Application.Current.Shutdown());
                return _exitCommand;
            }
        }

        #endregion

        #region Delete

        private RelayCommand _deleteCommand;

        /// <summary>
        /// Delete this person
        /// </summary>
        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                    _deleteCommand = new RelayCommand(x => DeleteCurrentConfiguration(), x => current != null);
                return _deleteCommand;
            }
        }

        // Deletes current person
        void DeleteCurrentConfiguration()
        {
            if (current == null)
                return;
            Configuration deleted = current;

            if (Configurations.Count != 1)
            {
                int index = Configurations.IndexOf(deleted);
                Current = Configurations[index == 0 ? 1 : index - 1];
            }
            else
            {
                Current = null;
                _deleteCommand.CanExecuteChanged();
            }

            Configurations.Remove(deleted);

        }

        #endregion

        #region Create

        private RelayCommand _createCommand;

        /// <summary>
        /// Delete this person
        /// </summary>
        public ICommand CreateCommand
        {
            get
            {
                if (_createCommand == null)
                    _createCommand = new RelayCommand(x => CreatConfiguration());
                return _createCommand;
            }
        }

        // Creates person
        void CreatConfiguration()
        {
            Configurations.Insert(0, new Configuration());
            Current = Configurations[0];
            _deleteCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #endregion

        #region Initialization

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainViewModel()
        {
            Configurations = ConfigurationCollection.Generate();
            if (Configurations.Count > 0)
                Current = Configurations[0];
        }

        #endregion
    }
}