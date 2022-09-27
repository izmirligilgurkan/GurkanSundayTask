using System;
using System.Collections.Generic;
using System.Linq;
using Curve;
using UnityEngine;

namespace _BallsToCup.Scripts.Runtime
{
    public static class SvgMeshUtility
    {
        public static Vector3[] SvgToPoints(Sprite svgImage, bool invertShape)
        {
            var spriteVertices = svgImage.vertices;
            var triangles = svgImage.triangles;
            var verticesList = new List<Vector2>();
            for (int i = 0; i < triangles.Length; i += 6)
            {
                Vector2 p1, p2, p3;
                p1 = spriteVertices[triangles[i]];
                p2 = spriteVertices[triangles[i + 1]];
                p3 = spriteVertices[triangles[i + 2]];
                var sum = p1 + p2 + p3;
                var center = sum / 3f;
                verticesList.Add(center);
            }

            var upwards = Vector2.Dot((verticesList[verticesList.Count - 1] - verticesList[0]).normalized, Vector2.up) > 0;

            if (!upwards)
            {
                verticesList.Reverse();
            }
            var offset = (Vector3)verticesList[0];

            return Array.ConvertAll(verticesList.ToArray(), point => new Vector3(point.x, point.y, 0) - offset);
        }

        public static Mesh[] CreateTubeMesh(float res, int seg, List<Vector3> controls)
        {
            var curve = new CatmullRomCurve(controls);
            // Build tubular mesh with Curve
            int tubularSegments = seg;
            float radius = res;
            int radialSegments = 16;
            bool closed = false; // closed curve or not
            var outerMesh = Tubular.Tubular.Build(curve, tubularSegments, radius, radialSegments, closed);
            var insideMesh = new Mesh();
            insideMesh.vertices = outerMesh.vertices;
            insideMesh.triangles = outerMesh.triangles.Reverse().ToArray();
            insideMesh.normals = outerMesh.normals;
            var insideVerts = insideMesh.vertices;
            var normals = insideMesh.normals;
            for (var index = 0; index < insideVerts.Length; index++)
            {
                insideVerts[index] -= normals[index] * (radius * .2f);
            }

            insideMesh.normals = Array.ConvertAll(normals, input => input * -1);
            insideMesh.vertices = insideVerts;

            // visualize mesh
            return new[] {outerMesh, insideMesh};
        }
    }
}