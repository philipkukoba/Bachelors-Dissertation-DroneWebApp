using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneWebApp.Models.PointcloudControlTool
{
    public class Face
    {
        public List<PointCloudXYZ> V { get; set; }
        public List<int> Index { get; set; }
        public int N { get { return V.Count; } }

        public Face(List<PointCloudXYZ> p, List<int> i)
        {
            V = p;
            Index = i;
        }
    }
}