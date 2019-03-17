using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grasshopper;
using Grasshopper.Kernel;

using AR_Lib.HalfEdgeMesh;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace AR_Grasshopper
{
    public class HE_MeshGHData : GH_Goo<HE_Mesh>

    {
        /// <summary>
        /// Constructs an empty MeshGHData object
        /// </summary>
        public HE_MeshGHData()
        {
            this.Value = null;
        }

        /// <summary>
        /// Constructs a MeshGHData object from a Rhino.Geometry.Mesh
        /// </summary>
        /// <param name="mesh">A Rhino Mesh</param>
        public HE_MeshGHData(Rhino.Geometry.Mesh mesh)
        {
            HE_Mesh hE;
            AR_Rhino.FromRhinoMesh(mesh, out hE);
            this.Value = hE;
        }

        /// <summary>
        /// Constructs a MeshGHData object from an AR_Lib HalfEdgeMesh
        /// </summary>
        /// <param name="hE_mesh">A Half-Edge Mesh</param>
        public HE_MeshGHData(HE_Mesh hE_mesh)
        {
            this.Value = hE_mesh;
        }

        /// <summary>
        /// Returns true if object is valid.
        /// TODO: Currently it ALWAYS returns true.
        /// </summary>
        public override bool IsValid => true;

        public override string TypeName => "Half-Edge Mesh";

        public override string TypeDescription => "Half-edge Mesh data structure";

        public override IGH_Goo Duplicate() => new HE_MeshGHData(Value);

        public override string ToString() => "Half-Edge Mesh";

        public override object ScriptVariable()
        {
            return Value;
        }

        public override bool CastFrom(object source)
        {
            if (source == null) { return false; }
            if (source is GH_Mesh)
            {
                Mesh mesh = (source as GH_Mesh).Value;
                if (mesh != null)
                {
                    if (AR_Rhino.FromRhinoMesh(mesh, out HE_Mesh hE) == AR_Rhino.RhinoMeshResult.OK)
                    {
                        Value = hE;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return false;
        }
        public override bool CastTo<Q>(ref Q target)
        {
            if (target is GH_Mesh)
            {
                Mesh mesh;
                AR_Rhino.RhinoMeshResult result = AR_Rhino.ToRhinoMesh(Value, out mesh);
                object obj = new GH_Mesh(mesh);
                if(result == AR_Rhino.RhinoMeshResult.OK)
                {
                    target = (Q)obj;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Grasshopper GH_Param encapsulating a MeshGHData object.
    /// </summary>
    public class HE_MeshParam: GH_Param<HE_MeshGHData>
    {
        // We need to supply a constructor without arguments that calls the base class constructor.
        public HE_MeshParam() : base("Half-edge mesh", "HE Mesh", "Half-edge mesh", "AR_Lib", "Params", GH_ParamAccess.item)
        {

        }

        public override System.Guid ComponentGuid
        {
            // Always generate a new Guid, but never change it once
            // you've released this parameter to the public.
            get { return new Guid("{d397c3bc-5c0c-4435-88a6-bbc988b1e6ca}"); }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.AR_Lib_Param_HalfEdgeMesh;
            }
        }

    }
}
