using System;
using System.Collections.Generic;
using AR_Lib.HalfEdgeMesh;
using AR_Lib.IO;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace AR_Grasshopper.IO
{
    public class ReadOFFComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ReadOFFComponent class.
        /// </summary>
        public ReadOFFComponent()
          : base("Read .OFF file", "Read .OFF",
              "Read a .OFF file from a specified file path",
              "AR_Lib", "I/O")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("File Path", "P", "File path to read .off from", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh", "M", "Mesh from .off file", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string path = "";
            Mesh rhinoMesh = new Mesh();

            if (!DA.GetData(0, ref path)) return;

            OFFMeshData data;
            OFFResult result = OFFReader.ReadMeshFromFile(path, out data);

            switch (result)
            {
                case OFFResult.File_Not_Found:
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "File not found.");
                    return;
                case OFFResult.Incorrect_Face:
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, ".OFF file has incorrect face");
                    return;
                case OFFResult.Incorrect_Vertex:
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, ".OFF file has incorrect vertex");
                    return;
                case OFFResult.Incorrect_Format:
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, ".OFF file has incorrect format");
                    return;
                case OFFResult.Non_Matching_Faces_Size:
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, ".OFF file has non-matching faces size");
                    return;
                case OFFResult.Non_Matching_Vertices_Size:
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, ".OFF file has non-matching vertices size");
                    return;
            }

            HE_Mesh mesh = new HE_Mesh(data.vertices, data.faces);

            AR_Rhino.ToRhinoMesh(mesh, out rhinoMesh);

            DA.SetData(0, rhinoMesh);
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
                return Properties.Resources.AR_Lib_ReadOFF;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("70aa03ad-29b3-4099-bc1d-106d7d316f15"); }
        }
    }
}