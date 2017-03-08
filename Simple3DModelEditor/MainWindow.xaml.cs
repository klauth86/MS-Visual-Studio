using Simple3DModelEditor.Model;
using Simple3DModelEditor.ViewModel;
using Simple3DModelEditor.Views;
using System.Windows;

namespace Simple3DModelEditor {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            var p1 = new Point3D(0, 0, 10);
            var p2 = new Point3D(0, 10, 0);
            var p3 = new Point3D(10, 0, 0);

            var model = new Model3D("ExampleModel");
            model.Points.Add(p1);
            model.Points.Add(p2);
            model.Points.Add(p3);

            model.Triangles.Add(new Triangle3D(model, p1, p3, p2));

            Content = new Model3DSetV() {
                DataContext = new Model3DSetVm(new Model3DSet("ExampleSet",
                new Model3D[] { model }))
            };
        }
    }
}
