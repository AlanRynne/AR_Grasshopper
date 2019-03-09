using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AR_Lib.HalfEdgeMesh;

namespace AR_Grasshopper
{
    public static class AR_Rhino
    {
        public static RhinoMeshResult ToRhinoMesh(HE_Mesh mesh, out Rhino.Geometry.Mesh rhinoMesh)
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

}
