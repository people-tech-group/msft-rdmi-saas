using System;
using System.Collections.Generic;
using System.Text;

namespace MSFT.WVDSaaS.API.Model
{
   public  class Enums
    {
        public enum loadBalancer
        {
            BreadthFirst = 0,
            DepthFirst = 1,
            Persistent = 2,
        }

        public enum assignmentType
        {
            Automatic=1,
            Direct=2
        }
    }
}
