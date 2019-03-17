using System;
using System.Collections.Generic;
using AR_Lib.HalfEdgeMesh;
using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace AR_Grasshopper.MeshTopology
{
    public class VertexTopologyComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the VertexTopology class.
        /// </summary>
        public VertexTopologyComponent()
          : base("Vertex Topology", "Vert. Topo",
              "Computes the vertex adjacency data for the given mesh.",
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
            pManager.AddIntegerParameter("VV", "VV", "VV", GH_ParamAccess.tree);
            pManager.AddIntegerParameter("VE", "VE", "VE", GH_ParamAccess.tree);
            pManager.AddIntegerParameter("VF", "VF", "VF", GH_ParamAccess.tree);

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

            topo.computeVertexAdjacency();

            DataTree<int> vvTopo = new DataTree<int>();
            DataTree<int> veTopo = new DataTree<int>();
            DataTree<int> vfTopo = new DataTree<int>();

            foreach (int key in topo.VertexVertex.Keys)
            {
                vvTopo.AddRange(topo.VertexVertex[key], new Grasshopper.Kernel.Data.GH_Path(key));
            }
            foreach (int key in topo.VertexEdges.Keys)
            {
                veTopo.AddRange(topo.VertexEdges[key], new Grasshopper.Kernel.Data.GH_Path(key));
            }
            foreach (int key in topo.VertexFaces.Keys)
            {
                vfTopo.AddRange(topo.VertexFaces[key], new Grasshopper.Kernel.Data.GH_Path(key));
            }

            DA.SetDataTree(0, vvTopo);
            DA.SetDataTree(1, veTopo);
            DA.SetDataTree(2, vfTopo);
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
                return Properties.Resources.AR_Lib_TopologyVertex;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("77b54c47-084b-4174-a761-969b6a699581"); }
        }
    }
}