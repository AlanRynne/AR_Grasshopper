using System;
using System.Collections.Generic;
using AR_Lib.HalfEdgeMesh;

using Grasshopper.Kernel;
using Rhino.Geometry;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace AR_Grasshopper
{
    public class MeshNormals : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public MeshNormals()
          : base("Mesh Normals", "Normals",
              "Computes the mesh normals in several ways.",
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
            pManager.AddVectorParameter("Area Weighted", "Aw", "Area weighted normals", GH_ParamAccess.list);
            pManager.AddVectorParameter("Angle Weighted", "AngW", "Angle weighted normals", GH_ParamAccess.list);
            pManager.AddVectorParameter("Equally Weighted", "Ew", "Equally weighted normals", GH_ParamAccess.list);
            pManager.AddVectorParameter("Sphere Inscribed", "Sph", "Sphere inscribed normals", GH_ParamAccess.list);
            pManager.AddVectorParameter("Gauss Curvature", "Gauss", "Gauss curvature normals", GH_ParamAccess.list);
            pManager.AddVectorParameter("Mean Curvature", "Mean", "Mean curvature normals", GH_ParamAccess.list);
            pManager.AddTextParameter("out", "out", "out", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Mesh mesh = new Mesh();

            if (!DA.GetData(0, ref mesh)) return;


            HE_Mesh hE_Mesh = new HE_Mesh();

            AR_Rhino.FromRhinoMesh(mesh, out hE_Mesh);
            List<Vector3d> areaWeightedNormals = new List<Vector3d>();
            List<Vector3d> angleWeightedNormals = new List<Vector3d>();
            List<Vector3d> equalWeightedNormals = new List<Vector3d>();
            List<Vector3d> sphereInscribedNormals = new List<Vector3d>();
            List<Vector3d> gaussCurvatureNormals = new List<Vector3d>();
            List<Vector3d> meanCurvatureNormals = new List<Vector3d>();

            foreach (HE_Vertex v in hE_Mesh.Vertices)
            {
                AR_Lib.Geometry.Vector3d vect = AR_Lib.Geometry.HE_MeshGeometry.VertexNormalAreaWeighted(v);
                areaWeightedNormals.Add(new Vector3d(vect.X, vect.Y, vect.Z));
                vect = AR_Lib.Geometry.HE_MeshGeometry.VertexNormalAngleWeighted(v);
                angleWeightedNormals.Add(new Vector3d(vect.X, vect.Y, vect.Z));
                vect = AR_Lib.Geometry.HE_MeshGeometry.VertexNormalEquallyWeighted(v);
                equalWeightedNormals.Add(new Vector3d(vect.X, vect.Y, vect.Z));
                vect = AR_Lib.Geometry.HE_MeshGeometry.VertexNormalSphereInscribed(v);
                sphereInscribedNormals.Add(new Vector3d(vect.X, vect.Y, vect.Z));
                vect = AR_Lib.Geometry.HE_MeshGeometry.VertexNormalGaussCurvature(v);
                gaussCurvatureNormals.Add(new Vector3d(vect.X, vect.Y, vect.Z));
                vect = AR_Lib.Geometry.HE_MeshGeometry.VertexNormalMeanCurvature(v);
                meanCurvatureNormals.Add(new Vector3d(vect.X, vect.Y, vect.Z));
            }
            DA.SetDataList(0, areaWeightedNormals);
            DA.SetDataList(1, angleWeightedNormals);
            DA.SetDataList(2, equalWeightedNormals);
            DA.SetDataList(3, sphereInscribedNormals);
            DA.SetDataList(4, gaussCurvatureNormals);
            DA.SetDataList(5, meanCurvatureNormals);
            DA.SetData(6, hE_Mesh); 
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
                return null;
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
