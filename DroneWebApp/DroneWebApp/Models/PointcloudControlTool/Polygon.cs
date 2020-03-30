using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneWebApp.Models.PointcloudControlTool
{
    public class Polygon
    {
        public List<PointCloudXYZ> V { get; set; }
        public List<int> Index { get; set; }
        public int N { get { return V.Count; } }

        public Polygon(List<PointCloudXYZ> p)
        {
            V = p;
            Index = new List<int>();

            for (int i=0; i<p.Count; i++)
            {
                Index.Add(i);
            }
        }
    }
}