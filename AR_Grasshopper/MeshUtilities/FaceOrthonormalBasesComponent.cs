using System;
using System.Collections.Generic;
using AR_Lib.HalfEdgeMesh;
using Grasshopper.Kernel;

using Rhino.Geometry;

namespace AR_Grasshopper.MeshNormals
{
    public class FaceOrthonormalBasesComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the OrthonormalBasesComponent class.
        /// </summary>
        public FaceOrthonormalBasesComponent()
          : base("OrthonormalBasesComponent", "Ortho",
              "Computes the orthonormal bases on each face of a triangular mesh",
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
            pManager.AddVectorParameter("e1", "e1", "e1 vector", GH_ParamAccess.list);
            pManager.AddVectorParameter("e2", "e2", "e2 vector", GH_ParamAccess.list);
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

            List<Vector3d> meshE1 = new List<Vector3d>();
            List<Vector3d> meshE2 = new List<Vector3d>();

            foreach (HE_Face face in hE_Mesh.Faces)
            {
                List<Vector3d> rhinoV = new List<Vector3d>();
                AR_Lib.Geometry.Vector3d[] vects = AR_Lib.Geometry.HE_MeshGeometry.OrthonormalBases(face);
                meshE1.Add(new Vector3d(vects[0].X, vects[0].Y, vects[0].Z));
                meshE2.Add(new Vector3d(vects[1].X, vects[1].Y, vects[1].Z));    
            }

            DA.SetDataList(0, meshE1);
            DA.SetDataList(1, meshE2);
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
                return Properties.Resources.AR_Lib_FaceOrthonormalBases;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("9b5feeba-913f-43f7-8cc7-9b5694b1eac9"); }
        }
    }
}