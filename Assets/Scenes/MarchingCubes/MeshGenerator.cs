// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using static Cube;


// [RequireComponent(typeof(MeshFilter))]

// public class MeshGenerator : MonoBehaviour
// {
//     Mesh mesh;

//     Vector3[] vertices;
//     int[] triangles;
//     Vector2[] uvs;
//     Color[] colors;

//     public int xSize = 20;
//     public int zSize = 20;
//     public int ySize = 20;

//     public Gradient gradient;

//     float minTerrainHeight;
//     float maxTerrainHeight;

//     // Start is called before the first frame update
//     void Start()
//     {
//         mesh = new Mesh();
//         GetComponent<MeshFilter>().mesh = mesh;

//         CreateSurface(); 
//         //CreateShape();
//         UpdateMesh();
//         //CreateCube();
       
//     }

//     void CreateSurface ()
//     {
//         vertices = new Vector3[(xSize + 1) * (zSize + 1) ];

//         for (int i = 0, z = 0; z <= zSize; z++)
//         {
//             for (int x = 0; x <= xSize; x++)
//             {
//                 float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 3f;
//                 vertices[i] = new Vector3(x, y, z);

//                 if (y > maxTerrainHeight)
//                 {
//                     maxTerrainHeight = y;
//                 }
//                 if (y < minTerrainHeight)
//                 {
//                     minTerrainHeight = y;
//                 }
//                 i++;
//             }
//         }

//         triangles = new int[xSize * zSize * 6];

//         int vert = 0;
//         int tris = 0;

//         for (int z = 0; z < zSize; z++)
//         {
//             for (int x = 0; x < xSize; x++)
//             {
                
//                 triangles[tris + 0] = vert + 0;
//                 triangles[tris + 1] = vert + xSize + 1;
//                 triangles[tris + 2] = vert + 1;
//                 triangles[tris + 3] = vert + 1;
//                 triangles[tris + 4] = vert + xSize + 1;
//                 triangles[tris + 5] = vert + xSize + 2;

//                 vert++;
//                 tris+= 6;
//             }
//             vert++;
//         }
        
//         colors = new Color[vertices.Length];
        
//         for (int i = 0, z = 0; z <= zSize; z++)
//         {
//             for (int x = 0; x <= xSize; x++)
//             {
//                 float height = Mathf.InverseLerp(minTerrainHeight, minTerrainHeight, vertices[i].y);
//                 colors[i] = gradient.Evaluate(height);
//                 i++;
//             }
//         }
//     }

//     void CreateShape()
//     {
//         vertices = new Vector3[(xSize + 1) * (zSize + 1) * (ySize + 1) ];

//         int i = 0;
//         for (int y = 0; y < ySize; y++)
//         {
//             for (int z = 0; z <= zSize; z++)
//             {
//                 for (int x = 0; x <= xSize; x++)
//                 {
//                     vertices[i] = new Vector3(x, y, z);
//                     i++;
//                 }
//             }
//         }
        
//     }    

//     void UpdateMesh()
//     {
//         mesh.Clear();

//         mesh.vertices = vertices;
//         mesh.triangles = triangles;
//         mesh.colors = colors;

//         mesh.RecalculateNormals();
//     }
// }
