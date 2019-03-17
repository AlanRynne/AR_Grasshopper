using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace AR_Grasshopper.MeshTopology
{
    public class MeshTopologyVEFComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MeshTopologyVEFComponent class.
        /// </summary>
        public MeshTopologyVEFComponent()
          : base("Half-Edge Mesh VEF", "V E F",
              "Returns the vertices edges and faces of a Half-Edge Mesh",
              "AR_Lib", "Topology")
        {
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
            pManager.AddPointParameter("Vertices", "V", "List of vertices of the mesh", GH_ParamAccess.list);
            pManager.AddLineParameter("Edges", "E", "List of edges of the mesh", GH_ParamAccess.list);
            pManager.AddMeshParameter("Faces", "F", "List of meshes representing each face of the mesh", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
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
                return Properties.Resources.AR_Lib_TopologyVEF;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("b72a840e-ee01-4292-bb39-149e830fd3ab"); }
        }
    }
}