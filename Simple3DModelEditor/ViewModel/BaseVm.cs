using Simple3DModelEditor.Common;

namespace Simple3DModelEditor.ViewModel {

    public abstract class BaseVm<TModel> : Notifier {
        TModel _model;
        public TModel Model {
            get { return _model; }
            set {
                _model = value;
                NotifyWithCallerPropName();
            }
        }
    }

    public abstract class BaseVm<TModel, TParentVM> : BaseVm<TModel> {
        public BaseVm(TModel model = default(TModel), TParentVM parentVM = default(TParentVM)) {
            Model = model;
            Parent = parentVM;
        }
        public TParentVM Parent { get; }
    }
}
