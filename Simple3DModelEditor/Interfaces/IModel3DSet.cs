using System.Collections.Generic;

namespace Simple3DModelEditor.Interfaces {
    interface IModel3DSet {
        string Description { get; set; }
        ICollection<IModel3D> Models { get; }
    }
}