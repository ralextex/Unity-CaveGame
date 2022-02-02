/** 
 *  @brief  Cave Generation Class
 *  @author A Lesage 
 *  @date   01-2022
 ***********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WorleyNoise;

/**
    * @brief Main programm implements marching cube and mesh generation
*/
public class CaveGeneration : MonoBehaviour
{	
	// width, lenght and height of marching cube 
    public int width, length, height; 
	// resolution
	public int res;

	// declare mesh object
	public Mesh mesh;
	private MeshCollider meshCol;
	
	 
	// List of Vector3, Corner point of cubes
	List<Vector3> verticies = new List<Vector3>();
	// List of int, triangles needed to create Mesh
	List<int> triangles = new List<int>();
	// How full the cave space should be
	[Range(400, 600)] public float fill = 500; 
	// Configuration for the look-up table
	int[,,] cubeConfiguration;
	// corner point densities
	int[,,] densities;
	// Binary densities (inside or outside) 
	int[,,] binDensities;

	// declare worley noise object
	private WorleyNoise wNoise;
	// number of feature points
	public int Npoint;
	// degree of neighboor i'm looking at
	public int kFactor = 1;
	// Noise value range
	public int precision = 5000;
	
	// Only for Debug: feature points, start/end point  
	public Vector3[] dbg_fPoint;
	public Vector3 dbg_start, dbg_end;
	
	private void Start()
	{
		// bigger due to mantle enclossing the cave 
		binDensities = new int[width + 2, height + 2, length + 2];
		cubeConfiguration = new int[width + 1, height + 1, length + 1];
		densities = new int[width + 1, height + 1, length + 1];

		// initialize worley noise
		wNoise = new WorleyNoise(width * res, height * res, length * res, res, Npoint, kFactor);
		
		// flag for generation
		bool genGood = false;

		// Generate a cave
		do 
		{
			wNoise.InitializeFeaturePoint();
			GenerateDensities();
			genGood = GenerateFlag();

			//Only for debug
			dbg_fPoint = new Vector3[Npoint];
			for (int i = 0; i < Npoint; i++)
			{
				dbg_fPoint[i] = wNoise.fPoint[i];
			}
		}while (!genGood);
 
		// Generate Triangles for the Mesh
		MarchingCube();

		// Generate Mesh from triangles
		mesh = new Mesh();
		mesh.vertices = verticies.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.RecalculateNormals();
		GetComponent<MeshFilter>().mesh = mesh;
		meshCol = gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
		meshCol.sharedMesh = mesh;
	}

	/**
    * @brief Generate density for every point 
    */
	void GenerateDensities()
	{
		for (int x = 0; x < width + 2; x++)
		{
			for (int y = 0; y < height + 2; y++)
			{
				for (int z = 0; z < length + 2; z++)
				{
					// Outer mantel
					if(x == 0 || x == width + 1 || y == 0 || y == height + 1 || z == 0 || z == length + 1)
					{
						binDensities[x, y, z] = 0; // 0 = Empty
					}
					// Inner mantel
					else if(x == 1 || x == width || y == 1 || y == height || z == 1 || z == length)
					{
						binDensities[x, y, z] = 1; // 1 = Full 
						densities[x, y, z] = 1;
					}
					// Inside the cave
					else
					{
						Vector3 v = new Vector3(x * res, y * res, z * res);
						int noiseValue = wNoise.ComputeValue(v, precision);
						densities[x, y, z] = noiseValue;


						if (noiseValue < fill)
						{
							binDensities[x, y, z] = 0; // Empty
						}
						else
						{
							binDensities[x, y ,z] = 1; // Full
						}
					}
				}
			}
		}
	}

	/**
    * @brief Creates Start and Exit 
	* @return generation succesfull flag  
    */
	private bool GenerateFlag()
	{
		// List of possible points
		List<Vector3> posEnd = new List<Vector3>();
		List<Vector3> posStart = new List<Vector3>();
		
		// boundaries between wall and points
		int bord = 3;
		
		// Find possible points
		for (int x = 2+bord; x < width-bord; x++)
        {
			for (int z = 2+bord; z < length-bord; z++)
			{
				// if point empty on bottom
				if(binDensities[x, 2, z] == 0)
				{
					Vector3 p = new Vector3(x,1,z);
					posEnd.Add(p);
				}
				
				// if point empty on top
				else if(binDensities[x, height - 1, z] == 0)
				{
					Vector3 p = new Vector3(x, height - 1, z);
					posStart.Add(p);
				} 
			}
		}
		
		if(posEnd.Count == 0 || posStart.Count == 0)
		{
			return false;
		}
		else
		{
			// choose a random ending point from possible
			int idxEnd = (int) Random.Range(0,posEnd.Count-1);
			GlobalPoints.Instance.end = posEnd[idxEnd]*res;
			dbg_end = posEnd[idxEnd]*res; 
			// debug print
			// print("idx end");
			// print(posEnd.Count);
			// print(idxEnd);
			
			// choose a random starting point from possible
			int idxStart = (int) Random.Range(0,posStart.Count-1);
			GlobalPoints.Instance.start = posStart[idxStart]*res;
			dbg_start = posStart[idxStart]*res;
			// debug print
			// print("idx start");
			// print(posStart.Count);
			// print(idxStart);
			
			GlobalPoints.Instance.finishedGen = true;

			return true;
		}
	}

	/**
    * @brief Marching cube algorithm
    */
	private void MarchingCube()
	{
		for (int x = 0; x < width + 1; x++)
        {
			for (int y = 0; y < height + 1; y++)
            {
				for (int z = 0; z < length + 1; z++)
                {
					
					// calculate configuration from density
					cubeConfiguration[x, y, z] = 
				    (binDensities[x, y + 1, z] * 128) + 
                    (binDensities[x + 1, y + 1, z] * 64) + 
				    (binDensities[x + 1, y + 1, z + 1] * 32) +
                    (binDensities[x, y + 1, z + 1] * 16) + 
				    (binDensities[x, y, z] * 8) +
                    (binDensities[x + 1, y, z] * 4) +
				    (binDensities[x + 1, y, z + 1] * 2) +
                    (binDensities[x, y, z + 1]);

					// Create Edgepoints between verticies
					Vector3[] edgePoints = new Vector3[]{
						new Vector3(x, y - 0.5f, z + 0.5f) * res, new Vector3(x + 0.5f, y - 0.5f, z) * res,
						new Vector3(x, y - 0.5f, z - 0.5f) * res, new Vector3(x - 0.5f, y - 0.5f, z) * res,
						new Vector3(x, y + 0.5f, z + 0.5f) * res, new Vector3(x + 0.5f, y + 0.5f, z) * res,
						new Vector3(x, y + 0.5f, z - 0.5f) * res, new Vector3(x - 0.5f, y + 0.5f, z) * res,
						new Vector3(x - 0.5f, y, z + 0.5f) * res, new Vector3(x + 0.5f, y, z + 0.5f) * res,
						new Vector3(x + 0.5f, y, z - 0.5f) * res, new Vector3(x - 0.5f, y, z - 0.5f) * res};

                    for (int i = 1; i < 5; i++)
                    {
						// Triangle points
						Vector3 a = new Vector3();
						Vector3 b = new Vector3();
						Vector3 c = new Vector3();

						// Looks in TriangulationTable if index is not -1 and appends it 
						if (TrigTable.Instance.TriangulationTable[cubeConfiguration[x, y, z], (i * 3) - 1] != -1)
                        {
							a = edgePoints[TrigTable.Instance.TriangulationTable[cubeConfiguration[x, y, z], (i * 3) - 1]];
                        }

						if (TrigTable.Instance.TriangulationTable[cubeConfiguration[x, y, z], (i * 3) - 2] != -1)
                        {
							b = edgePoints[TrigTable.Instance.TriangulationTable[cubeConfiguration[x, y, z], (i * 3) - 2]];
                        }

						if (TrigTable.Instance.TriangulationTable[cubeConfiguration[x, y, z], (i * 3) - 3] != -1)
                        {
							c = edgePoints[TrigTable.Instance.TriangulationTable[cubeConfiguration[x, y, z], (i * 3) - 3]];
                        }

						if (a != Vector3.zero || b != Vector3.zero || c != Vector3.zero)
                        {
							triangles.Add(verticies.Count); 
                            triangles.Add(verticies.Count + 1); 
                            triangles.Add(verticies.Count + 2);
							verticies.Add(a); 
                            verticies.Add(b); 
                            verticies.Add(c);
						}
					}
                }
			}
		}
	}

	private void OnDrawGizmos() {
        foreach (Vector3 item in dbg_fPoint)
        {
			Gizmos.color = Color.red;
            Gizmos.DrawSphere(item, 2);
        }
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(dbg_start, 2);

		Gizmos.color = Color.green;
		Gizmos.DrawSphere(dbg_end, 2);

		// Visualize all corner points and density values
		// for (int x = 1; x < width; x++)
		// {
		// 	for (int y = 1; y < length; y++)
		// 	{
		// 		for (int z = 1; z < height; z++)
		// 		{	
		// 			Vector3 v = new Vector3(x * res, y * res, z * res);
		// 			if(densities== null)
		// 			{
		// 				return;
		// 			}
		// 			else
		// 			{
		// 				Gizmos.color = new Color(densities[x, y, z]/precision, densities[x, y, z]/precision, densities[x, y, z]/precision);
		// 				Gizmos.DrawSphere(v, 1);
		// 			}
		// 		}
		// 	}
		// }
    }	
}
