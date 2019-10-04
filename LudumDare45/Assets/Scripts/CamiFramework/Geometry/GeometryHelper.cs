using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CamiFramwork.Geometry
{
    public class GeometryHelper
    {
        public static void AddSquare(float x, float y,
            Geometry geometry,
            float size, List<Vector2> points=null)
        {
            int baseIndex = geometry.Vertices.Count;

            geometry.Vertices.Add(new Vector3(x, y + size));
            geometry.Vertices.Add(new Vector3(x + size, y));
            geometry.Vertices.Add(new Vector3(x + size, y + size));
            geometry.Vertices.Add(new Vector3(x, y));

            if (points != null)
            {
                points.Add((Vector2)geometry.Vertices[baseIndex + 3]);
                points.Add((Vector2)geometry.Vertices[baseIndex + 1]);
                points.Add((Vector2)geometry.Vertices[baseIndex + 2]);
                points.Add((Vector2)geometry.Vertices[baseIndex + 0]);
            }

            geometry.UVs.Add(new Vector2(0, 1));
            geometry.UVs.Add(new Vector2(1, 0));
            geometry.UVs.Add(new Vector2(1, 1));
            geometry.UVs.Add(new Vector2(0, 0));

            geometry.Indices.Add(baseIndex + 0);
            geometry.Indices.Add(baseIndex + 2);
            geometry.Indices.Add(baseIndex + 1);

            geometry.Indices.Add(baseIndex + 0);
            geometry.Indices.Add(baseIndex + 1);
            geometry.Indices.Add(baseIndex + 3);

            geometry.CreateDefaultNormals();
        }
    
        public static void AddRect(Rect rect,
            Geometry geometry)
        {
            int baseIndex = geometry.Vertices.Count;
        
            geometry.Vertices.Add(new Vector3(rect.x, rect.y + rect.height));
            geometry.Vertices.Add(new Vector3(rect.x + rect.width, rect.y));
            geometry.Vertices.Add(new Vector3(rect.x + rect.width, rect.y + rect.height));
            geometry.Vertices.Add(new Vector3(rect.x, rect.y));

            //if (geometry.ColliderPoints != null)
            //{
            //    for (int i = baseIndex; i < geometry.Vertices.Count; ++i)
            //    {
            //        geometry.ColliderPoints.Add((Vector2)geometry.Vertices[i]);
            //    }
            //}

            geometry.UVs.Add(new Vector2(0, 1));
            geometry.UVs.Add(new Vector2(1, 0));
            geometry.UVs.Add(new Vector2(1, 1));
            geometry.UVs.Add(new Vector2(0, 0));

            geometry.Indices.Add(baseIndex + 0);
            geometry.Indices.Add(baseIndex + 2);
            geometry.Indices.Add(baseIndex + 1);

            geometry.Indices.Add(baseIndex + 0);
            geometry.Indices.Add(baseIndex + 1);
            geometry.Indices.Add(baseIndex + 3);

            geometry.CreateDefaultNormals();
        }

        public static void AddSprite(Rect rect, Sprite sprite,
            Geometry geometry)
        {
            int baseIndex = geometry.Vertices.Count;
        
            geometry.Vertices.Add(new Vector3(rect.x, rect.y + rect.height));
            geometry.Vertices.Add(new Vector3(rect.x + rect.width, rect.y));
            geometry.Vertices.Add(new Vector3(rect.x + rect.width, rect.y + rect.height));
            geometry.Vertices.Add(new Vector3(rect.x, rect.y));

            //if (geometry.ColliderPoints != null)
            //{
            //    for (int i = baseIndex; i < geometry.Vertices.Count; ++i)
            //    {
            //        geometry.ColliderPoints.Add((Vector2)geometry.Vertices[i]);
            //    }
            //}
        
            geometry.UVs.Add(sprite.uv[0]);
            geometry.UVs.Add(sprite.uv[1]);
            geometry.UVs.Add(sprite.uv[2]);
            geometry.UVs.Add(sprite.uv[3]);

            geometry.Indices.Add(baseIndex + 0);
            geometry.Indices.Add(baseIndex + 2);
            geometry.Indices.Add(baseIndex + 1);

            geometry.Indices.Add(baseIndex + 0);
            geometry.Indices.Add(baseIndex + 1);
            geometry.Indices.Add(baseIndex + 3);

            geometry.CreateDefaultNormals();
        }

        public static void AddLine(Vector2 start, Vector2 end, Geometry geometry,
            float startThickness,
            float endThickness)
        {
            Vector2 dir = end - start;
            dir.Normalize();

            AddLine(start, end, dir, geometry,
            startThickness, endThickness);
        }

        public static void AddLine(
            Vector2 start, Vector2 end, Vector2 dir,
            Geometry geometry,
            float startThickness,
            float endThickness)
        {
            Vector2 right = MathsHelper.Perp2D(dir);
            Vector2 startRight = right * (startThickness / 2f);
            Vector2 endRight = right * (endThickness / 2f);

            int baseIndex = geometry.Vertices.Count;

            geometry.Vertices.Add(end - endRight);
            geometry.Vertices.Add(start + startRight);
            geometry.Vertices.Add(end + endRight);
            geometry.Vertices.Add(start - startRight);

            //if (geometry.ColliderPoints != null)
            //{
            //    for (int i = baseIndex; i < geometry.Vertices.Count; ++i)
            //    {
            //        geometry.ColliderPoints.Add((Vector2)geometry.Vertices[i]);
            //    }
            //}

            geometry.UVs.Add(new Vector2(0, 1));
            geometry.UVs.Add(new Vector2(1, 0));
            geometry.UVs.Add(new Vector2(1, 1));
            geometry.UVs.Add(new Vector2(0, 0));

            geometry.Indices.Add(baseIndex + 0);
            geometry.Indices.Add(baseIndex + 2);
            geometry.Indices.Add(baseIndex + 1);

            geometry.Indices.Add(baseIndex + 0);
            geometry.Indices.Add(baseIndex + 1);
            geometry.Indices.Add(baseIndex + 3);

            geometry.CreateDefaultNormals();
        }

        public static void AddSegmentedLine(
            Vector2 start, Vector2 end,
            Geometry geometry,
            float startThickness,
            float endThickness,
            float segmentLength)
        {
            Vector2 dir = end - start;
            float distance = dir.magnitude;
            dir /= distance;

            int segmentCount = (int)Mathf.Ceil(distance / segmentLength);

            float thicknessIncrement = (endThickness - startThickness) / (float)segmentCount;
            float currentThickness = startThickness;

            Vector2 increment = dir * segmentLength;
            Vector2 currentStart = start;
            Vector2 currentEnd = start + increment;

            for (int i = 0; i < segmentCount; ++i)
            {
                Vector2 currentIncrement = (float)i * increment;

                float nextThickness = currentThickness + thicknessIncrement;

                AddLine(currentStart, currentEnd, dir, geometry,
                    currentThickness, nextThickness);

                currentStart += increment;
                currentEnd += increment;
                currentThickness = nextThickness;
            }
        }
    
        public static void AddLine(Geometry mesh, Vector3 start, Vector3 end, float startThickness, float endThickness, int segments)
        {
            Vector3 dir = end - start;
            float length = dir.magnitude;
            dir /= length;

            Vector3 perp = Vector3.up;
            if (dir == perp || dir == Vector3.down)
                perp = Vector3.right;

            Vector3 right = Vector3.Cross(dir, perp);
            Vector3 up = Vector3.Cross(dir, right);

            int baseIndex = mesh.Vertices.Count;

            // Add tube
            float arc = MathsHelper.TAU / segments;
            for(int i = 0; i < segments; ++i)
            {
                Quaternion rot = Quaternion.AngleAxis((arc * (float)i) * Mathf.Rad2Deg, dir);
                Vector3 offset = rot * right;

                Vector3 offsetStart = offset * (startThickness / 2f);
                Vector3 offsetEnd = offset * (endThickness / 2f);
                
                mesh.AddVertex(start + offsetStart, offset, Vector2.zero);
                mesh.AddVertex(end + offsetEnd, offset, Vector2.zero);
            }

            for(int i = 0; i < segments; ++i)
            {
                int startIndex = baseIndex + (i * 2);
            
                mesh.AddIndex(startIndex + 1);
                mesh.AddIndex(startIndex + 0);

                if(i < segments - 1)
                {
                    mesh.AddIndex(startIndex + 2);
                
                    mesh.AddIndex(startIndex + 3);
                    mesh.AddIndex(startIndex + 1);
                    mesh.AddIndex(startIndex + 2);
                }
                else
                {
                    mesh.AddIndex(baseIndex + 0);
                    
                    mesh.AddIndex(baseIndex + 1);
                    mesh.AddIndex(startIndex + 1);
                    mesh.AddIndex(baseIndex + 0);
                }
            }

            // Add caps
            int capsBaseIndex = mesh.Vertices.Count;
            mesh.AddVertex(start, -dir, Vector2.zero);
            mesh.AddVertex(end, dir, Vector2.zero);
            
            for (int i = 0; i < segments; ++i)
            {
                int startIndex = baseIndex + (i * 2);

                Vector3 vert = mesh.Vertices[startIndex];
                mesh.AddVertex(vert, -dir, Vector2.zero);

                vert = mesh.Vertices[startIndex + 1];
                mesh.AddVertex(vert, dir, Vector2.zero);
            }
            
            for(int i = 0; i < segments; ++i)
            {
                int startIndex = (capsBaseIndex + 2) + (i * 2);
                
                mesh.AddIndex(capsBaseIndex);
                mesh.AddIndex(startIndex + 0);
                if (i < segments - 1)
                {
                    mesh.AddIndex(startIndex + 2);
                }
                else
                {
                    mesh.AddIndex(capsBaseIndex + 2);
                }
                
                mesh.AddIndex(capsBaseIndex + 1);
                if(i < segments - 1)
                {
                    mesh.AddIndex(startIndex + 3);
                }
                else
                {
                    mesh.AddIndex(capsBaseIndex + 3);
                }
                mesh.AddIndex(startIndex + 1);
            }
        }

        public static void AddVoxelSide(Geometry geometry, Vector3 position, float size, Vector3 normal)
        {
            Vector3 side1 = new Vector3(normal.y, normal.z, normal.x);
            Vector3 side2 = Vector3.Cross(normal, side1);

            int baseIndex = geometry.Vertices.Count;

            // Six indices (two triangles) per face.
            geometry.AddIndex(baseIndex + 2);
            geometry.AddIndex(baseIndex + 1);
            geometry.AddIndex(baseIndex + 0);

            geometry.AddIndex(baseIndex + 3);
            geometry.AddIndex(baseIndex + 2);
            geometry.AddIndex(baseIndex + 0);

            // Four vertices per face.
            geometry.AddVertex(position + (normal - side1 - side2) * size / 2, normal, Vector2.zero);
            geometry.AddVertex(position + (normal - side1 + side2) * size / 2, normal, Vector2.zero);
            geometry.AddVertex(position + (normal + side1 + side2) * size / 2, normal, Vector2.zero);
            geometry.AddVertex(position + (normal + side1 - side2) * size / 2, normal, Vector2.zero);

            // Colours
            for (int i = 0; i < 4; ++i)
            {
                geometry.Colours.Add(Color.gray);
            }

            //// UVs
            geometry.UVs[geometry.UVs.Count - 4] = new Vector2(0, 1);
            geometry.UVs[geometry.UVs.Count - 3] = new Vector2(1, 1);
            geometry.UVs[geometry.UVs.Count - 2] = new Vector2(1, 0);
            geometry.UVs[geometry.UVs.Count - 1] = new Vector2(0, 0);
        }

        public static void AddVoxel(Geometry geometry, Vector3 position, float size, 
            Sprite topSprite, Sprite leftSprite, Sprite rightSprite, Sprite forwardSprite, Sprite backSprite)
        {
            // A cube has six faces, each one pointing in a different direction.
            Vector3[] normals =
            {
                new Vector3(0, 0, 1),
                new Vector3(0, 0, -1),
                new Vector3(1, 0, 0),
                new Vector3(-1, 0, 0),
                new Vector3(0, 1, 0),
            };

            // Create each face in turn.
            foreach (Vector3 normal in normals)
            {
                // Get two vectors perpendicular to the face normal and to each other.
                Vector3 side1 = new Vector3(normal.y, normal.z, normal.x);
                Vector3 side2 = Vector3.Cross(normal, side1);

                int baseIndex = geometry.Vertices.Count;

                // Six indices (two triangles) per face.
                geometry.AddIndex(baseIndex + 2);
                geometry.AddIndex(baseIndex + 1);
                geometry.AddIndex(baseIndex + 0);

                geometry.AddIndex(baseIndex + 3);
                geometry.AddIndex(baseIndex + 2);
                geometry.AddIndex(baseIndex + 0);

                // Four vertices per face.
                geometry.AddVertex(position + (normal - side1 - side2) * size / 2, normal, Vector2.zero);
                geometry.AddVertex(position + (normal - side1 + side2) * size / 2, normal, Vector2.zero);
                geometry.AddVertex(position + (normal + side1 + side2) * size / 2, normal, Vector2.zero);
                geometry.AddVertex(position + (normal + side1 - side2) * size / 2, normal, Vector2.zero);

                // Colours
                for (int i = 0; i < 4; ++i)
                {
                    geometry.Colours.Add(Color.gray);
                }

                //// UVs
                geometry.UVs[geometry.UVs.Count - 4] = new Vector2(0, 1);
                geometry.UVs[geometry.UVs.Count - 3] = new Vector2(1, 1);
                geometry.UVs[geometry.UVs.Count - 2] = new Vector2(1, 0);
                geometry.UVs[geometry.UVs.Count - 1] = new Vector2(0, 0);
                //Sprite sprite;
                //if (normal == Vector3.up)
                //{
                //    sprite = topSprite;
                //    geometry.UVs[geometry.UVs.Count - 4] = sprite.uv[0];
                //    geometry.UVs[geometry.UVs.Count - 3] = sprite.uv[3];
                //    geometry.UVs[geometry.UVs.Count - 2] = sprite.uv[1];
                //    geometry.UVs[geometry.UVs.Count - 1] = sprite.uv[2];
                //}
                //else if (normal == Vector3.left)
                //{
                //    sprite = leftSprite;
                //    geometry.UVs[geometry.UVs.Count - 4] = sprite.uv[0];
                //    geometry.UVs[geometry.UVs.Count - 3] = sprite.uv[3];
                //    geometry.UVs[geometry.UVs.Count - 2] = sprite.uv[1];
                //    geometry.UVs[geometry.UVs.Count - 1] = sprite.uv[2];
                //}
                //else if (normal == Vector3.forward)
                //{
                //    sprite = forwardSprite;
                //    geometry.UVs[geometry.UVs.Count - 4] = sprite.uv[3];
                //    geometry.UVs[geometry.UVs.Count - 3] = sprite.uv[1];
                //    geometry.UVs[geometry.UVs.Count - 2] = sprite.uv[2];
                //    geometry.UVs[geometry.UVs.Count - 1] = sprite.uv[0];
                //}
                //else if (normal == Vector3.right)
                //{
                //    sprite = rightSprite;
                //    geometry.UVs[geometry.UVs.Count - 4] = sprite.uv[0];
                //    geometry.UVs[geometry.UVs.Count - 3] = sprite.uv[3];
                //    geometry.UVs[geometry.UVs.Count - 2] = sprite.uv[1];
                //    geometry.UVs[geometry.UVs.Count - 1] = sprite.uv[2];
                //}
                //else if (normal == Vector3.back)
                //{
                //    sprite = backSprite;
                //    geometry.UVs[geometry.UVs.Count - 4] = sprite.uv[2];
                //    geometry.UVs[geometry.UVs.Count - 3] = sprite.uv[0];
                //    geometry.UVs[geometry.UVs.Count - 2] = sprite.uv[3];
                //    geometry.UVs[geometry.UVs.Count - 1] = sprite.uv[1];
                //}
            }
        }
    }
}