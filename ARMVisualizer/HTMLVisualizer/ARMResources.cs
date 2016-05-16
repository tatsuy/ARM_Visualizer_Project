using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLVisualizer
{

    enum ARMResourceType
    {
        ARM_Vnet = 0,
        ARM_Subnet,
        ARM_LoadBalancer,
        ARM_AvailabilitySet,
        ARM_VirtualMachine,
        ARM_Nic,
        ARM_NetworkSecurityGroup,
        ARM_PublicAddress
    };

    public class ARMResources
    {
        public String resname;
        public String resparent;
        public List<String> reschild;
        public int level;

        public int x;
        public int y;
        public int width;
        public int height;

        // Set ARMResourceType
        public int restype;

        bool nsgpresent;

        public static ARMResources[] PositionCaluculater(ARMResources[] res)
        {
//            ARMResources restemp1 = res.OrderBy(s => s.level);
  //          ARMResources restemp2 = restemp1.OrderBy(s => s.type);


            return res;
        }

        public ARMResources(String name, int type, String parent)
        {
            x = 0;
            y = 0;
            width = 0;
            height = 0;

            restype = type;
            resname = name;
            resparent = parent;

            reschild = new List<string>();

            if (parent == null)
            {
                level = 0;
            }
            else
            {
                level = -1;
            }
        }
    }
    
}
