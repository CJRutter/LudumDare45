using Cami.Collections;
using UnityEngine;

namespace CamiFramwork.Geometry
{
    [System.Serializable]
    public class Geometry
    {
        public Geometry()
        {
            Vertices = new ArrayList<Vector3>();
            UVs = new ArrayList<Vector2>();
            Indices = new ArrayList<int>();
            Normals = new ArrayList<Vector3>();
            Colours = new ArrayList<Color>();
        }

        public void Clear()
        {
            Vertices.Clear();
            UVs.Clear();
            Indices.Clear();
            Normals.Clear();
            Colours.Clear();
        }

        public void Build(MeshFilter meshFilter)
        {
    #if UNITY_EDITOR
            Mesh mesh = meshFilter.sharedMesh;
    #else
            Mesh mesh = meshFilter.mesh;
    #endif
            if (mesh == null)
            {
                mesh = new Mesh();

                if (Dynamic)
                    mesh.MarkDynamic();
            }
        
            mesh.Clear();


            // Set verts
            mesh.vertices = Vertices.TrimAndGetBuffer();

            // Set indices
            mesh.triangles = Indices.TrimAndGetBuffer();

            // Set uvs
            mesh.uv = UVs.TrimAndGetBuffer();

            // Set normals
            mesh.normals = Normals.TrimAndGetBuffer();

            // Set Colours
            if (Colours.Count > 0)
            {
                mesh.colors = Colours.TrimAndGetBuffer();
            }

            meshFilter.mesh = mesh;
        }

        public void CreateDefaultNormals()
        {
            while (Normals.Count < Vertices.Count)
                Normals.Add(new Vector3(0, 0, 1));
        }

        public void CreateDefaultUVs()
        {
            while (UVs.Count < Vertices.Count)
                UVs.Add(Vector2.zero);
        }

        public void FillColours(Color colour)
        {
            for(int i = Colours.Count; i < Vertices.Count; ++i)
            {
                Colours.Add(colour);
            }
        }
    
        public void AddVertex(Vector3 position, Vector3 normal, Vector2 textureCoord)
        {
            Vertices.Add(position);
            Normals.Add(normal);
            UVs.Add(textureCoord);
        }

        public void AddIndex(int index)
        {
            Indices.Add(index);
        }

        #region Properties
        public ArrayList<Vector3> Vertices { get; set; }
        public ArrayList<Vector2> UVs { get; set; }
        public ArrayList<int> Indices { get; set; }
        public ArrayList<Vector3> Normals { get; set; }
        public ArrayList<Color> Colours { get; set; }
        public bool Dynamic { get; set; }
        #endregion Properties

        #region Fields
        #endregion Fields
    }
}