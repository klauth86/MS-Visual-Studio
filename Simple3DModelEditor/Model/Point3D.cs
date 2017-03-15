using Simple3DModelEditor.Common;
using Simple3DModelEditor.Interfaces;


namespace Simple3DModelEditor.Model {
    class Point3D : Notifier, IPoint3D {

        #region PROPS

        private double _x;
        public double X {
            get {
                return _x;
            }

            set {
                if (_x != value) {
                    _x = value;
                    NotifyWithCallerPropName();
                    Notify("Coordinates");
                }
            }
        }

        private double _y;
        public double Y {
            get {
                return _y;
            }

            set {
                if (_y != value) {
                    _y = value;
                    NotifyWithCallerPropName();
                    Notify("Coordinates");
                }
            }
        }

        private double _z;
        public double Z {
            get {
                return _z;
            }

            set {
                if (_z != value) {
                    _z = value;
                    NotifyWithCallerPropName();
                    Notify("Coordinates");
                }
            }
        }

        public string Coordinates=> $"({X},{Y},{Z})";

        #endregion

        #region CTOR

        public Point3D(double x = 0, double y = 0, double z = 0) {
            X = x;
            Y = y;
            Z = z;
        }

        #endregion

        public IVector3D DistanceTo(IPoint3D endPoint) {
            return new Vector3D(endPoint.X - X, endPoint.Y - Y, endPoint.Z - Z);
        }

        #region OPERATORS

        public static IVector3D operator -(Point3D endPoint, Point3D startPoint) {
            return startPoint.DistanceTo(endPoint);
        }

        #endregion
    }
}
