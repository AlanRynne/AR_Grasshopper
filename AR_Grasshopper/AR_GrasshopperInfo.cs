using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace AR_Grasshopper
{
    public class AR_GrasshopperInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "AR_Grasshopper";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "Component that utilizes the AR_Lib library for triangular mesh calculations";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("5dc3c051-9ffa-4c34-9027-10c3a7f8c2c3");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "Alan Rynne";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "www.rynne.es / www.paramdigma.com";
            }
        }
    }
}
