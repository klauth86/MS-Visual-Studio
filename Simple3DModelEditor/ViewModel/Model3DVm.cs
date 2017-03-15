using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Windows.Media;

using Simple3DModelEditor.Interfaces;
using Simple3DModelEditor.Common;

namespace Simple3DModelEditor.ViewModel {
    class Model3DVm : BaseVm<IModel3D, Model3DSetVm> {

        #region PROPS

        public string Description {
            get { return Model.Description; }
            set { Model.Description = value; }
        }

        public IEnumerable<IPoint3D> Points => Model.Points;

        public IEnumerable<ITriangle3D> Triangles => Model.Triangles;

        private IPoint3D _selectedPoint;
        public IPoint3D SelectedPoint {
            get { return _selectedPoint; }
            set {
                if (_selectedPoint != value) {
                    _selectedPoint = value;
                    NotifyWithCallerPropName();
                }
            }
        }

        private ITriangle3D _selectedTriangle;
        public ITriangle3D SelectedTriangle {
            get { return _selectedTriangle; }
            set {
                if (_selectedTriangle != value) {
                    _selectedTriangle = value;
                    NotifyWithCallerPropName();
                }
            }
        }

        private ProjectionCamera _camera;
        public ProjectionCamera Camera {
            get { return _camera; }
            set {
                _camera = value;
                NotifyWithCallerPropName();
            }
        }

        public GeometryModel3D GeometryModel3D => GetGeometryModel3D(Model);

        public Light LightModel3D => GetLightModel3D();

        private double _axisOfRotationX;
        public double AxisOfRotationX {
            get {
                return _axisOfRotationX;
            }
            set {
                if (_axisOfRotationX != value) {
                    _axisOfRotationX = value;
                    NotifyWithCallerPropName();
                    Notify("AxisOfRotationBinded");
                }
            }
        }

        private double _axisOfRotationY;
        public double AxisOfRotationY {
            get {
                return _axisOfRotationY;
            }
            set {
                if (_axisOfRotationY != value) {
                    _axisOfRotationY = value;
                    NotifyWithCallerPropName();
                    Notify("AxisOfRotationBinded");
                }
            }
        }

        private double _axisOfRotationZ;
        public double AxisOfRotationZ {
            get {
                return _axisOfRotationZ;
            }
            set {
                if (_axisOfRotationZ != value) {
                    _axisOfRotationZ = value;
                    NotifyWithCallerPropName();
                    Notify("AxisOfRotationBinded");
                }
            }
        }
        public Vector3D AxisOfRotationBinded => new Vector3D(AxisOfRotationX, AxisOfRotationY, AxisOfRotationZ);

        #endregion

        #region CTOR

        public Model3DVm(IModel3D model, Model3DSetVm parentVm, IVector3D axisOfRotation = null) : base(model, parentVm) {
            var collectionNotify = Model.Triangles as INotifyCollectionChanged;
            if (collectionNotify != null) {
                collectionNotify.CollectionChanged += (o, e) => { Notify("GeometryModel3D"); };
            }

            collectionNotify = Model.Points as INotifyCollectionChanged;
            if (collectionNotify != null) {
                collectionNotify.CollectionChanged += (o, e) => { Notify("GeometryModel3D"); };
            }

            var notify = Model as Notifier;
            if (notify != null)
                notify.PropertyChanged += (o, e) => {
                    if (e.PropertyName == "Description")
                        Notify("Description");
                    else
                        Notify("GeometryModel3D");
                };

            var camera = new PerspectiveCamera();
            camera.Position = new Point3D(50, 0, 0);
            camera.LookDirection = new Vector3D(-1, 0, 0);
            Camera = camera;

            AxisOfRotationX = axisOfRotation?.X ?? 1;
            AxisOfRotationY = axisOfRotation?.Y ?? 1;
            AxisOfRotationZ = axisOfRotation?.Z ?? 1;
        }

        #endregion

        #region COMMANDS

        private ICommand _addPointCommand;
        public ICommand AddPointCommand => _addPointCommand ?? (_addPointCommand = new RelayCommand(AddPoint));

        private ICommand _removePointCommand;
        public ICommand RemovePointCommand => _removePointCommand ?? (_removePointCommand = new RelayCommand(RemovePoint, () => SelectedPoint != null));

        private ICommand _addTriangleCommand;
        public ICommand AddTriangleCommand => _addTriangleCommand ?? (_addTriangleCommand = new RelayCommand(AddTriangle, Model.Points.Any));

        private ICommand _removeTriangleCommand;
        public ICommand RemoveTriangleCommand => _removeTriangleCommand ?? (_removeTriangleCommand = new RelayCommand(RemoveTriangle, () => SelectedTriangle != null));

        #endregion

        #region COMMANDS IMPLEMENTATION

        private void AddPoint() {
            var newPoint = new Model.Point3D();
            Model.Points.Add(newPoint);
            SelectedPoint = newPoint;
        }
        private void RemovePoint() {

            List<ITriangle3D> trianglesForDelete = new List<ITriangle3D>();
            foreach(var triangle in Model.Triangles) {
                if (triangle.Point1 == SelectedPoint ||
                    triangle.Point2 == SelectedPoint ||
                    triangle.Point3 == SelectedPoint
                    )
                    trianglesForDelete.Add(triangle);
            }

            foreach (var triangle in trianglesForDelete)
                Model.Triangles.Remove(triangle);

            Model.Points.Remove(SelectedPoint);
        }

        private void AddTriangle() {
            var newTriangle = new Model.Triangle3D(Model, Model.Points.First(), Model.Points.First(), Model.Points.First());
            Model.Triangles.Add(newTriangle);
            SelectedTriangle = newTriangle;
        }
        private void RemoveTriangle() {
            Model.Triangles.Remove(SelectedTriangle);
        }

        #endregion

        private GeometryModel3D GetGeometryModel3D(IModel3D model) {
            var result = new GeometryModel3D();
            var geometry = new MeshGeometry3D();

            var indexes = new Dictionary<IPoint3D, int>();
            int i = 0;

            foreach (var point in model.Points) {
                geometry.Positions.Add(new Point3D(point.X, point.Y, point.Z));
                indexes.Add(point, i++);
            }

            geometry.TriangleIndices = new Int32Collection(model.Triangles.SelectMany(triangle => new List<int>() { indexes[triangle.Point1], indexes[triangle.Point2], indexes[triangle.Point3] }));

            result.Geometry = geometry;
            result.Material = new DiffuseMaterial(Brushes.Blue);
            result.BackMaterial = new DiffuseMaterial(Brushes.Red);
            return result;
        }

        private Light GetLightModel3D() {
            var light = new DirectionalLight();
            light.Color = Colors.White;
            light.Direction = new Vector3D(-1, -1, -1);
            return light;
        }
    }
}
