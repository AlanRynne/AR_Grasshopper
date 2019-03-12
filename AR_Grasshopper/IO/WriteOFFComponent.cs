using System;
using System.IO;
using System.Collections.Generic;
using AR_Lib.HalfEdgeMesh;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace AR_Grasshopper.IO
{
    public class WriteOFFComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the WriteOFFComponent class.
        /// </summary>
        public WriteOFFComponent()
          : base("Write .OFF file", "Write .OFF",
              "Write an .OFF file to the specified file path",
              "AR_Lib", "I/O")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh", "M", "Mesh", GH_ParamAccess.item);
            pManager.AddTextParameter("File Path", "P", "File Path", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Mesh mesh = new Mesh();
            string filePath = "";

            if (!DA.GetData(0, ref mesh)) return;
            if (!DA.GetData(1, ref filePath)) return;

            if(!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Specified location doesn't exist!");
            }

            HE_Mesh hE_Mesh = new HE_Mesh();

            AR_Rhino.FromRhinoMesh(mesh, out hE_Mesh);

            if (!hE_Mesh.isTriangularMesh())
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Mesh is not triangular!");
                return;
            }

            AR_Lib.IO.OFFResult result = AR_Lib.IO.OFFWritter.WriteMeshToFile(hE_Mesh, filePath);

            if (result != AR_Lib.IO.OFFResult.OK)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Export failed! :(");
            }
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
                return Properties.Resources.AR_Lib_WriteOFF;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("8349a0fe-b600-49aa-a9cb-4f226e26209b"); }
        }
    }
}