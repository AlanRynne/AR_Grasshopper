using System;
using System.Collections.Generic;
using AR_Lib.HalfEdgeMesh;
using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace AR_Grasshopper.MeshTopology
{
    public class FaceTopologyComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the FaceTopology class.
        /// </summary>
        public FaceTopologyComponent()
          : base("Face Topology", "Face Topo",
              "Computes the face adjacency data for the given mesh",
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
            pManager.AddIntegerParameter("FV", "FV", "FV", GH_ParamAccess.tree);
            pManager.AddIntegerParameter("FE", "FE", "FE", GH_ParamAccess.tree);
            pManager.AddIntegerParameter("FF", "FF", "FF", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            HE_MeshGHData hE_MeshData = new HE_MeshGHData();

            if (!DA.GetData(0, ref hE_MeshData)) return;

            HE_Mesh hE_Mesh = hE_MeshData.Value;

            HE_MeshTopology topo = new HE_MeshTopology(hE_Mesh);

            topo.computeFaceAdjacency();

            DataTree<int> fvTopo = new DataTree<int>();
            DataTree<int> feTopo = new DataTree<int>();
            DataTree<int> ffTopo = new DataTree<int>();

            foreach (int key in topo.FaceVertex.Keys)
            {
                fvTopo.AddRange(topo.FaceVertex[key], new Grasshopper.Kernel.Data.GH_Path(key));
            }
            foreach (int key in topo.FaceVertex.Keys)
            {
                feTopo.AddRange(topo.FaceVertex[key], new Grasshopper.Kernel.Data.GH_Path(key));
            }
            foreach (int key in topo.FaceFace.Keys)
            {
                ffTopo.AddRange(topo.FaceFace[key], new Grasshopper.Kernel.Data.GH_Path(key));
            }

            DA.SetDataTree(0, fvTopo);
            DA.SetDataTree(1, feTopo);
            DA.SetDataTree(2, ffTopo);
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
                return Properties.Resources.AR_Lib_TopologyFace;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("9044ea40-e6d9-4a9f-97ba-fb85bf5ed429"); }
        }
    }
}