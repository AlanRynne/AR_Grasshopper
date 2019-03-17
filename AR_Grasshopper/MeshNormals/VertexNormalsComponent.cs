using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AR_Lib.HalfEdgeMesh;

using Grasshopper.Kernel;
using Rhino.Geometry;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace AR_Grasshopper
{
    public class VertexNormals : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public VertexNormals()
          : base("Mesh Normals", " V Norm",
              "Computes the mesh normals in several ways.\n\n" +
                "You can select the different options in tºnhe right-click menu of this component:\n\n" +
                "- Equally weighted normals\n" +
                "- Area weighted normals\n" +
                "- Angle weighted normals\n" +
                "- Gauss curvature weighted normals\n" +
                "- Mean curvature weighted normals\n" +
                "- Sphere inscribed normals\n\n" +
                "Created by Alan Rynne - www.rynne.es",
              "AR_Lib", "Mesh")
        {
            Selection = NormalSelection.Equal;
            this.Message = "Equally Weighted";
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new HE_MeshParam(), "Half-Edge Mesh", "hE", "Half-Edge Mesh", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddVectorParameter("Normals", "N", "Computed normals according to selection.\nYou can select another method by right clicking the component.", GH_ParamAccess.list);
        }

        public enum NormalSelection
        {
            Equal,
            Area,
            Angle,
            Gauss,
            Mean,
            Sphere
        }
        public NormalSelection Selection;


        private void onEquallyClick(Object sender, EventArgs e)
        {
            Selection = NormalSelection.Equal;
            changeMessage(sender, e);
        }
        private void onAngleClick(Object sender, EventArgs e)
        {
            Selection = NormalSelection.Angle;
            changeMessage(sender, e);
        }
        private void onAreaClick(Object sender, EventArgs e)
        {
            Selection = NormalSelection.Area;
            changeMessage(sender, e);
        }
        private void onGaussClick(Object sender, EventArgs e)
        {
            Selection = NormalSelection.Gauss;
            changeMessage(sender, e);
        }
        private void onMeanClick(Object sender, EventArgs e)
        {
            Selection = NormalSelection.Mean;
            changeMessage(sender, e);
        }
        private void onSphereClick(Object sender, EventArgs e)
        {
            Selection = NormalSelection.Sphere;
            changeMessage(sender, e);
        }

        private void changeMessage(Object sender, EventArgs e)
        {
            switch (Selection)
            {
                case NormalSelection.Equal:
                    this.Message = "Equally Weighted";
                    break;
                case NormalSelection.Angle:
                    this.Message = "Angle Weighted";
                    break;
                case NormalSelection.Area:
                    this.Message = "Area Weighted";
                    break;
                case NormalSelection.Gauss:
                    this.Message = "Gauss Curvature";
                    break;
                case NormalSelection.Mean:
                    this.Message = "Mean Curvature";
                    break;
                case NormalSelection.Sphere:
                    this.Message = "Sphere Inscribed";
                    break;

            }
            ExpirePreview(true);
            ExpireSolution(true);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            HE_MeshGHData hE_MeshData = new HE_MeshGHData();

            if (!DA.GetData(0, ref hE_MeshData)) return;

            HE_Mesh hE_Mesh = hE_MeshData.Value;

            if (!hE_Mesh.isTriangularMesh())
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Mesh is not triangular!");
                return;
            }

            List<Vector3d> normals = new List<Vector3d>();

            foreach (HE_Vertex v in hE_Mesh.Vertices)
            {
                AR_Lib.Geometry.Vector3d vect = new AR_Lib.Geometry.Vector3d();

                switch (Selection)
                {
                    case NormalSelection.Equal:
                        vect = AR_Lib.Geometry.HE_MeshGeometry.VertexNormalEquallyWeighted(v);
                        break;
                    case NormalSelection.Area:
                        vect = AR_Lib.Geometry.HE_MeshGeometry.VertexNormalAreaWeighted(v);
                        break;
                    case NormalSelection.Angle:
                        vect = AR_Lib.Geometry.HE_MeshGeometry.VertexNormalAngleWeighted(v);
                        break;
                    case NormalSelection.Gauss:
                        vect = AR_Lib.Geometry.HE_MeshGeometry.VertexNormalGaussCurvature(v);
                        break;
                    case NormalSelection.Mean:
                        vect = AR_Lib.Geometry.HE_MeshGeometry.VertexNormalMeanCurvature(v);
                        break;
                    case NormalSelection.Sphere:
                        vect = AR_Lib.Geometry.HE_MeshGeometry.VertexNormalSphereInscribed(v);
                        break;
                }

                normals.Add(new Vector3d(vect.X, vect.Y, vect.Z));
            }

            DA.SetDataList(0, normals);

        }

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);

            Menu_AppendItem(menu, "Equally Weighted", onEquallyClick);
            Menu_AppendItem(menu, "Area Weighted", onAreaClick);
            Menu_AppendItem(menu, "Angle Weighted", onAngleClick);
            Menu_AppendItem(menu, "Gauss Curvature", onGaussClick);
            Menu_AppendItem(menu, "Mean Curvature", onMeanClick);
            Menu_AppendItem(menu, "Sphere Inscribed", onSphereClick);

        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return Properties.Resources.AR_Lib_VertexNormals;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("59ee78ea-be29-4fbf-8afa-9da3ef695361"); }
        }
    }
}
