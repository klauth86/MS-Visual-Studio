using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Input;
using System.Linq;

using Simple3DModelEditor.Common;
using Simple3DModelEditor.Interfaces;
using Simple3DModelEditor.Model;

namespace Simple3DModelEditor.ViewModel {
    class Model3DSetVm : BaseVm<IModel3DSet> {

        #region PROPS

        public string Description {
            get { return Model.Description; }
            set { Model.Description = value; }
        }

        public ICollection<Model3DVm> Models => Model.Models.Select(model3D => new Model3DVm(model3D, this)).ToList();

        private Model3DVm _selectedModel;
        public Model3DVm SelectedModel {
            get {
                return _selectedModel;
            }
            set {
                if (_selectedModel != value) {
                    _selectedModel = value;
                    NotifyWithCallerPropName();
                }
            }
        }

        #endregion

        #region CTOR

        public Model3DSetVm(IModel3DSet model3DSet) {

            Model = model3DSet;

            var collectionNotify = Model.Models as INotifyCollectionChanged;
            if (collectionNotify != null) {
                collectionNotify.CollectionChanged += (o, e) => { Notify("Models"); };
            }

            var notify = Model as Notifier;
            if (notify != null)
                notify.PropertyChanged += (o, e) => { Notify("Description"); };
        }

        #endregion

        #region COMMANDS

        private ICommand _addNewModelCommand;
        public ICommand AddNewModelCommand => _addNewModelCommand ?? (_addNewModelCommand = new RelayCommand(AddNewModel));

        private ICommand _removeSelectedModelCommand;
        public ICommand RemoveSelectedModelCommand => _removeSelectedModelCommand ?? (_removeSelectedModelCommand = new RelayCommand(RemoveSelectedModel, () => SelectedModel != null));

        #endregion

        #region COMMAND IMPLEMENTATION

        private void AddNewModel() {
            Model.Models.Add(new Model3D());
        }

        private void RemoveSelectedModel() {
            Model.Models.Remove(SelectedModel.Model);
            SelectedModel = null;
        }

        #endregion

    }
}
