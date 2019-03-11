using System;
using System.Collections.Generic;
using AR_Lib.HalfEdgeMesh;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace AR_Grasshopper.MeshCurves
{
    public class LevelSetsComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the LevelSetsComponent class.
        /// </summary>
        public LevelSetsComponent()
          : base("Mesh Level Sets", "Level Sets",
              "Computes the level sets given a list of numeric values for each vertex of the mesh and a step size for the level set calculation.",
              "AR_Lib", "Mesh Curves")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh", "M", "Triangular Mesh", GH_ParamAccess.item);
            pManager.AddNumberParameter("Scalar values", "V", "List of numerical values to place on each vertex of the mesh", GH_ParamAccess.list);
            pManager.AddNumberParameter("Step size", "S", "Step size of the level set curves", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Level Sets", "L", "Level set curves on the mesh", GH_ParamAccess.list);
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
                return Properties.Resources.AR_Lib_LevelSets;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("6f7b2c13-d5da-4932-97c0-cf751bfec6ef"); }
        }
    }
}