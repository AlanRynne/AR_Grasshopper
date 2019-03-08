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

            DA.SetData(0, hE_Mesh);
        }


        public static class AR_Rhino
        {
            public static RhinoMeshResult ToRhinoMesh(HE_Mesh mesh, out Mesh rhinoMesh)
            {
                //CHECKS FOR NGON MESHES!
                if (mesh.isNgonMesh())
                {
                    rhinoMesh = null;
                    return RhinoMeshResult.Invalid;
                }

                rhinoMesh = new Rhino.Geometry.Mesh();

                foreach (HE_Vertex vertex in mesh.Vertices)
                {
                    rhinoMesh.Vertices.Add(vertex.X, vertex.Y, vertex.Z);
                }

                foreach (HE_Face face in mesh.Faces)
                {
                    List<HE_Vertex> faceVertices = face.adjacentVertices();
                    if (faceVertices.Count == 3) rhinoMesh.Faces.AddFace(new Rhino.Geometry.MeshFace(faceVertices[0].Index, faceVertices[1].Index, faceVertices[2].Index));
                    if (faceVertices.Count == 4) rhinoMesh.Faces.AddFace(new Rhino.Geometry.MeshFace(faceVertices[0].Index, faceVertices[1].Index, faceVertices[2].Index, faceVertices[3].Index));
                }

                return RhinoMeshResult.OK;

            }

            public static RhinoMeshResult FromRhinoMesh(Rhino.Geometry.Mesh rhinoMesh, out HE_Mesh mesh)
            {
                List<AR_Lib.Geometry.Point3d> vertices = new List<AR_Lib.Geometry.Point3d>();
                List<List<int>> faceVertexIndexes = new List<List<int>>();

                foreach (Rhino.Geometry.Point3d vertex in rhinoMesh.Vertices)
                {
                    vertices.Add(new AR_Lib.Geometry.Point3d(vertex.X, vertex.Y, vertex.Z));
                }
                foreach (Rhino.Geometry.MeshNgon face in rhinoMesh.GetNgonAndFacesEnumerable())
                {

                    List<int> list = new List<int>();

                    foreach (int i in face.BoundaryVertexIndexList()) list.Add(i);

                    faceVertexIndexes.Add(list);

                }
                mesh = new HE_Mesh(vertices, faceVertexIndexes);
                return RhinoMeshResult.OK;
            }

            public enum RhinoMeshResult
            {
                OK,
                Empty,
                Invalid
            }
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
