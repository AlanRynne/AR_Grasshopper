using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;


namespace AR_Grasshopper.NurbsSurfaces
{
    public class NurbsSurfaceTestComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the NurbsSurfaceTest class.
        /// </summary>
        public NurbsSurfaceTestComponent()
          : base("NurbsSurfaceTest", "NurbsTest",
              "Nurbs Surface implementation test",
              "AR_Lib", "Nurbs")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("U Degree", "Udeg", "U Degree", GH_ParamAccess.item, 3);
            pManager.AddIntegerParameter("V Degree", "Vdeg", "V Degree", GH_ParamAccess.item, 3);
            pManager.AddIntegerParameter("U Control Points", "Upts", "U Control Points", GH_ParamAccess.item);
            pManager.AddIntegerParameter("V Control Points", "Vpts", "V Control Points", GH_ParamAccess.item);
            pManager.AddPointParameter("Control Points", "Pts", "Control Points", GH_ParamAccess.list);
            pManager.AddNumberParameter("U Knot Valuees", "Uknot", "U Knot Values", GH_ParamAccess.list);
            pManager.AddNumberParameter("V Knot Valuees", "Vknot", "V Knot Values", GH_ParamAccess.list);
            pManager.AddIntegerParameter("U Tesselations", "Utess", "U Tesselations", GH_ParamAccess.item, 10);
            pManager.AddIntegerParameter("V Tesselations", "Vtess", "V Tesselations", GH_ParamAccess.item, 10);
            pManager.AddNumberParameter("Weights", "W", "Weights", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Nurbs Points", "Pts", "Nurbs computed points", GH_ParamAccess.list);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int uDegree = 0, vDegree = 0, uCount = 0, vCount = 0, uTess = 0, vTess = 0;

            List<Point3d> controlPoints = new List<Point3d>();
            List<double> uKnotValues = new List<double>();
            List<double> vKnotValues = new List<double>();
            List<double> weightValues = new List<double>();

            if (!DA.GetData(0, ref uDegree)) return;
            if (!DA.GetData(1, ref vDegree)) return;
            if (!DA.GetData(2, ref uCount)) return;
            if (!DA.GetData(3, ref vCount)) return;

            if (!DA.GetDataList(4, controlPoints)) return;
            if (!DA.GetDataList(5, uKnotValues)) return;
            if (!DA.GetDataList(6, vKnotValues)) return;

            if (!DA.GetData(7, ref uTess)) return;
            if (!DA.GetData(8, ref vTess)) return;

            if (!DA.GetDataList(9, weightValues)) return;

            List<AR_Lib.Geometry.Point4d> control4d = new List<AR_Lib.Geometry.Point4d>();

            for(int i = 0; i < controlPoints.Count; i++)
            {
                Point3d pt = controlPoints[i];
                control4d.Add(new AR_Lib.Geometry.Point4d(pt.X, pt.Y, pt.Z, weightValues[i]));
            }

            AR_Lib.Geometry.Nurbs.Surface surf = 
                new AR_Lib.Geometry.Nurbs.Surface(uDegree, vDegree, uCount, vCount, control4d, uKnotValues, vKnotValues, uTess, vTess);
            surf.TessellateSurface();
            List<Point3d> pts = new List<Point3d>(surf.PVertices.Count);
            foreach(AR_Lib.Geometry.Point3d arPt in surf.PVertices)
            {
                pts.Add(new Point3d(Math.Round(arPt.X,4), Math.Round(arPt.Y,4), Math.Round(arPt.Z,4)));
            }

            DA.SetDataList(0, pts);
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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("040f451f-8364-4206-a46d-03b75096bf9c"); }
        }
    }
}