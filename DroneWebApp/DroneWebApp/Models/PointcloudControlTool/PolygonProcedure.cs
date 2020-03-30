﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneWebApp.Models.PointcloudControlTool
{
    public class PolygonProcedure
    {
        double maxUnitMeasureError = 0.001;

        // Polygon Boundary
        double x1, x2, y1, y2, z1, z2;

        // Polygon faces
        List<Face> faces;

        // Polygon face planes
        List<Plane> planes;

        // Number of faces
        int numberOfFaces;

        // Maximum point to face plane distance error, 
        // point is considered in the face plane if its distance is less than this error
        double maxDisError;

        #region public methods

        public PolygonProcedure(Polygon polygon)
        {

            List<Face> faces = new List<Face>();

            List<Plane> facePlanes = new List<Plane>();

            int numberOfFaces = 0;

            double x1 = 0, x2 = 0, y1 = 0, y2 = 0, z1 = 0, z2 = 0;

            // Get boundary
            this.Get3DPolygonBoundary(polygon, ref x1, ref x2, ref y1, ref y2, ref z1, ref z2);

            // Get maximum point to face plane distance error, 
            // point is considered in the face plane if its distance is less than this error
            double maxDisError = this.Get3DPolygonUnitError(polygon);

            // Get face planes        
            this.GetConvex3DFaces(polygon, maxDisError, faces, facePlanes, ref numberOfFaces);

            // Set data members
            this.x1 = x1;
            this.x2 = x2;
            this.y1 = y1;
            this.y2 = y2;
            this.z1 = z1;
            this.z2 = z2;
            this.faces = faces;
            this.planes = facePlanes;
            this.numberOfFaces = numberOfFaces;
            this.maxDisError = maxDisError;
        }

        public void GetBoundary(ref double xmin, ref double xmax,
                                ref double ymin, ref double ymax,
                                ref double zmin, ref double zmax)
        {
            xmin = x1;
            xmax = x2;
            ymin = y1;
            ymax = y2;
            zmin = z1;
            zmax = z2;
        }

        public bool PointInside3DPolygon(double x, double y, double z)
        {
            PointCloudXYZ p = new PointCloudXYZ
            {
                X = x,
                Y = y,
                Z = z
            };

            return this.PointInside3DPolygon(p, planes, numberOfFaces);
        }

        #endregion

        #region private methods    

        private double Get3DPolygonUnitError(Polygon polygon)
        {
            List<PointCloudXYZ> vertices = polygon.V;
            int n = polygon.N;

            double measureError = 0;

            double xmin = 0, xmax = 0, ymin = 0, ymax = 0, zmin = 0, zmax = 0;

            this.Get3DPolygonBoundary(polygon,
                         ref xmin, ref xmax, ref ymin, ref ymax, ref zmin, ref zmax);

            measureError = ((Math.Abs(xmax) + Math.Abs(xmin) + Math.Abs(ymax) + Math.Abs(ymin) +
                             Math.Abs(zmax) + Math.Abs(zmin)) / 6 * maxUnitMeasureError);

            return measureError;
        }

        private void Get3DPolygonBoundary(Polygon polygon,
                ref double xmin, ref double xmax,
                ref double ymin, ref double ymax,
                ref double zmin, ref double zmax)
        {
            List<PointCloudXYZ> vertices = polygon.V;

            int n = polygon.N;

            xmin = xmax = (double)vertices[0].X;
            ymin = ymax = (double)vertices[0].Y;
            zmin = zmax = (double)vertices[0].Z;

            for (int i = 1; i < n; i++)
            {
                if (vertices[i].X < xmin) xmin = (double)vertices[i].X;
                if (vertices[i].Y < ymin) ymin = (double)vertices[i].Y;
                if (vertices[i].Z < zmin) zmin = (double)vertices[i].Z;
                if (vertices[i].X > xmax) xmax = (double)vertices[i].X;
                if (vertices[i].Y > ymax) ymax = (double)vertices[i].Y;
                if (vertices[i].Z > zmax) zmax = (double)vertices[i].Z;
            }
        }

        private bool PointInside3DPolygon(double x, double y, double z,
                                     List<Plane> planes, int numberOfFaces)
        {
            PointCloudXYZ p = new PointCloudXYZ
            {
                X = x,
                Y = y,
                Z = z
            };

            return PointInside3DPolygon(p, planes, numberOfFaces);
        }

        private bool PointInside3DPolygon(PointCloudXYZ p, List<Plane> planes, int numberOfFaces)
        {
            for (int i = 0; i < numberOfFaces; i++)
            {

                double dis = p * planes[i];

                // If the point is in the same half space with normal vector 
                // for any facet of the cube, then it is outside of the 3D polygon        
                if (dis > 0)
                {
                    return false;
                }
            }

            // If the point is in the opposite half space with normal vector for all 6 facets, 
            // then it is inside of the 3D polygon
            return true;
        }

        // Input: polgon, maxError
        // Return: faces, facePlanes, numberOfFaces
        private void GetConvex3DFaces(Polygon polygon, double maxError,
                        List<Face> faces, List<Plane> planes, ref int numberOfFaces)
        {
            // vertices of 3D polygon
            List<PointCloudXYZ> vertices = polygon.V;

            int n = polygon.N;

            // vertice indexes for all faces
            // vertice index is the original index value in the input polygon
            List<List<int>> faceVerticeIndex = new List<List<int>>();

            // face planes for all faces
            List<Plane> fpOutward = new List<Plane>();

            for (int i = 0; i < n; i++)
            {
                // triangle point 1
                PointCloudXYZ p1 = vertices[i];

                for (int j = i + 1; j < n; j++)
                {
                    // triangle point 2
                    PointCloudXYZ p2 = vertices[j];

                    for (int k = j + 1; k < n; k++)
                    {
                        // triangle point 3
                        PointCloudXYZ p3 = vertices[k];

                        Plane trianglePlane = new Plane(p1, p2, p3);

                        int onLeftCount = 0;
                        int onRightCount = 0;

                        // indexes of points that lie in same plane with face triangle plane
                        List<int> pointInSamePlaneIndex = new List<int>();

                        for (int l = 0; l < n; l++)
                        {
                            // any point other than the 3 triangle points
                            if (l != i && l != j && l != k)
                            {
                                PointCloudXYZ p = vertices[l];

                                double dis = p * trianglePlane;

                                // next point is in the triangle plane 
                                if (Math.Abs(dis) < maxError)
                                {
                                    pointInSamePlaneIndex.Add(l);
                                }

                                else
                                {
                                    if (dis < 0)
                                    {
                                        onLeftCount++;
                                    }
                                    else
                                    {
                                        onRightCount++;
                                    }
                                }
                            }
                        }

                        // This is a face for a CONVEX 3D polygon.
                        // For a CONCAVE 3D polygon, this maybe not a face
                        if (onLeftCount == 0 || onRightCount == 0)
                        {
                            List<int> verticeIndexInOneFace = new List<int>();

                            // triangle plane
                            verticeIndexInOneFace.Add(i);
                            verticeIndexInOneFace.Add(j);
                            verticeIndexInOneFace.Add(k);

                            int m = pointInSamePlaneIndex.Count;

                            if (m > 0) // there are other vetirces in this triangle plane
                            {
                                for (int p = 0; p < m; p++)
                                {
                                    verticeIndexInOneFace.Add(pointInSamePlaneIndex[p]);
                                }
                            }

                            // if verticeIndexInOneFace is a new face, 
                            // add it in the faceVerticeIndex list, 
                            // add the trianglePlane in the face plane list fpOutward
                            if (!Utility.ContainsList(faceVerticeIndex, verticeIndexInOneFace))
                            {
                                faceVerticeIndex.Add(verticeIndexInOneFace);

                                if (onRightCount == 0)
                                {
                                    fpOutward.Add(trianglePlane);
                                }
                                else if (onLeftCount == 0)
                                {
                                    fpOutward.Add(-trianglePlane);
                                }
                            }

                        }
                        else
                        {
                            // possible reasons:
                            // 1. the plane is not a face of a convex 3d polygon, 
                            //    it is a plane crossing the convex 3d polygon.
                            // 2. the plane is a face of a concave 3d polygon
                        }

                    } // k loop
                } // j loop        
            } // i loop                        

            // return number of faces
            numberOfFaces = faceVerticeIndex.Count;

            for (int i = 0; i < numberOfFaces; i++)
            {
                // return face planes
                planes.Add(new Plane(fpOutward[i].A, fpOutward[i].B, fpOutward[i].C,
                                            fpOutward[i].D));

                List<PointCloudXYZ> points = new List<PointCloudXYZ>();

                List<int> vi = new List<int>();

                for (int j = 0; j < faceVerticeIndex[i].Count; j++)
                {
                    vi.Add(faceVerticeIndex[i][j]);
                    PointCloudXYZ pointXYZ = new PointCloudXYZ
                    {
                        X = vertices[vi[j]].X,
                        Y = vertices[vi[j]].Y,
                        Z = vertices[vi[j]].Z
                    };
                    points.Add(pointXYZ);
                }

                // return faces
                faces.Add(new Face(points, vi));
            }
        }

        #endregion
    }
}