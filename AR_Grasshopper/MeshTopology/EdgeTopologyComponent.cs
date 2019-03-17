using System;
using System.Collections.Generic;
using AR_Lib.HalfEdgeMesh;
using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace AR_Grasshopper.MeshTopology
{
    public class EdgeTopologyComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the EdgeTopology class.
        /// </summary>
        public EdgeTopologyComponent()
          : base("Edge Topology", "Edge Topo",
              "Computes the edge adjacency data for the given mesh",
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
            pManager.AddIntegerParameter("EV", "EV", "EV", GH_ParamAccess.tree);
            pManager.AddIntegerParameter("EE", "EE", "EE", GH_ParamAccess.tree);
            pManager.AddIntegerParameter("EF", "EF", "EF", GH_ParamAccess.tree);
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

            topo.computeEdgeAdjacency();

            DataTree<int> evTopo = new DataTree<int>();
            DataTree<int> efTopo = new DataTree<int>();
            DataTree<int> eeTopo = new DataTree<int>();

            foreach (int key in topo.EdgeVertex.Keys)
            {
                evTopo.AddRange(topo.EdgeVertex[key], new Grasshopper.Kernel.Data.GH_Path(key));
            }
            foreach (int key in topo.EdgeEdge.Keys)
            {
                eeTopo.AddRange(topo.EdgeEdge[key], new Grasshopper.Kernel.Data.GH_Path(key));
            }
            foreach (int key in topo.EdgeFace.Keys)
            {
                efTopo.AddRange(topo.EdgeFace[key], new Grasshopper.Kernel.Data.GH_Path(key));
            }

            DA.SetDataTree(0, evTopo);
            DA.SetDataTree(1, eeTopo);
            DA.SetDataTree(2, efTopo);

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
                return Properties.Resources.AR_Lib_TopologyEdge;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("a06f9af2-a97a-4bf0-a363-620c76118c27"); }
        }
    }
}