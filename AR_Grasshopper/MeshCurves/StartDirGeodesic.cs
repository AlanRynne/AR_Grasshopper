using System;
using System.Collections.Generic;
using AR_Lib.HalfEdgeMesh;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace AR_Grasshopper.MeshCurves
{
    public class StartDirGeodesic : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the StartDirGeodesic class.
        /// </summary>
        public StartDirGeodesic()
          : base("Geodesic P/V", "P/V geod",
              "Computes a geodesic curve on a triangular mesh given a starting point and a direction vector.",
              "AR_Lib", "Mesh Curves")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh", "M", "Mesh", GH_ParamAccess.item);
            pManager.AddPointParameter("Point", "Pt", "Startting point for the geodesic",GH_ParamAccess.item);
            pManager.AddVectorParameter("Direction", "Dir", "Starting direction for the geodesic", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Geodeesic", "G", "Computed geodesic curve on the mesh", GH_ParamAccess.item);
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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("13df0941-dd3f-49ff-a0d4-951839c886e5"); }
        }
    }
}