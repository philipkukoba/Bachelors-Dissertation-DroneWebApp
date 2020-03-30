using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneWebApp.Models.PointcloudControlTool
{
    public class Vector
    {
        PointCloudXYZ p1;
        PointCloudXYZ p2;

        public double X { get { return (double)p2.X - (double)p1.X; } }
        public double Y { get { return (double)p2.Y - (double)p1.Y; } }
        public double Z { get { return (double)p2.Z - (double)p1.Z; } }

        public Vector(PointCloudXYZ p1, PointCloudXYZ p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }

        public static Vector operator *(Vector u, Vector v)
        {
            double x = u.Y * v.Z - u.Z * v.Y;
            double y = u.Z * v.X - u.X * v.Z;
            double z = u.X * v.Y - u.Y * v.X;

            PointCloudXYZ point1 = v.p1;
            PointCloudXYZ point2 = new PointCloudXYZ
            {
                X = point1.X + x,
                Y = point1.Y + y,
                Z = point1.Z + z
            };
                
            return new Vector(point1, point2);
        }
    }
}