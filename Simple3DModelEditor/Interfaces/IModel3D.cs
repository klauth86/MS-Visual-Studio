using System.Collections.Generic;

namespace Simple3DModelEditor.Interfaces {
    interface IModel3D {
        string Description { get; set; }
        ICollection<IPoint3D> Points { get; }
        ICollection<ITriangle3D> Triangles { get; }
    }
}
