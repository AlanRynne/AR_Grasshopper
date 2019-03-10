using System;
using System.Collections.Generic;
using AR_Lib.HalfEdgeMesh;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace AR_Grasshopper.MeshUtilities
{
    public class VertexCurvatureComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MeshCurvatureComponent class.
        /// </summary>
        public VertexCurvatureComponent()
          : base("Vertex Curvature", "Curvature",
              "Computes the scalar and vector curvature of a given triangular mesh",
              "AR_Lib", "Mesh")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh", "M", "Mesh", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("k1", "k1", "k1 scalar value of curvature", GH_ParamAccess.list);
            pManager.AddNumberParameter("k2", "k2", "k2 scalar value of curvature", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Mesh mesh = new Mesh();

            if (!DA.GetData(0, ref mesh)) return;

            HE_Mesh hE_Mesh = new HE_Mesh();

            AR_Rhino.FromRhinoMesh(mesh, out hE_Mesh);

            if (!hE_Mesh.isTriangularMesh())
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Mesh is not triangular!");
                return;
            }

            List<double> k1 = new List<double>();
            List<double> k2 = new List<double>();

            foreach (HE_Vertex v in hE_Mesh.Vertices)
            {
                double[] k = AR_Lib.Geometry.HE_MeshGeometry.PrincipalCurvatures(v);
                k1.Add(k[0]);
                k2.Add(k[1]);
            }

            DA.SetDataList(0, k1);
            DA.SetDataList(1, k2);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.AR_Lib_VertexCurvature;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("714d95a4-fde5-43dd-b4ac-7d600fd008de"); }
        }
    }
}