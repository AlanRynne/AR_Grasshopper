using System;
using System.Collections.Generic;
using AR_Lib.HalfEdgeMesh;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace AR_Grasshopper.MeshNormals
{
    public class FaceNormalsComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the FaceNormalsComponent class.
        /// </summary>
        public FaceNormalsComponent()
          : base("Face Normals", "F Norm",
              "Computes the face normals of a given triangular mesh.",
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
            pManager.AddVectorParameter("Face Normals", "N", "The computed face normals",GH_ParamAccess.list);
            pManager.AddPointParameter("Centroids", "C", "Face centroids", GH_ParamAccess.list);
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

            List<Vector3d> normals = new List<Vector3d>();
            List<Point3d> centroids = new List<Point3d>();
            List<Point3d> circumcenters = new List<Point3d>();

            foreach (HE_Face face in hE_Mesh.Faces)
            {
                AR_Lib.Geometry.Vector3d v = AR_Lib.Geometry.HE_MeshGeometry.FaceNormal(face);
                AR_Lib.Geometry.Point3d centroid = AR_Lib.Geometry.HE_MeshGeometry.Centroid(face);

                normals.Add(new Vector3d(v.X, v.Y, v.Z));
                centroids.Add(new Point3d(centroid.X, centroid.Y, centroid.Z));

            }

            DA.SetDataList(0, normals);
            DA.SetDataList(1, centroids);
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
                return Properties.Resources.AR_Lib_FaceNormals;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("5d7430f8-3718-4e7d-8f06-75f0c4e67333"); }
        }
    }
}