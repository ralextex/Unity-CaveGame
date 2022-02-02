using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static KDTree;

public class WorleyNoise : MonoBehaviour
{
    public int Npoint, kFactor, res; 
    public int dimX, dimY, dimZ;
    public Vector3[] fPoint;
    public KDTree tree = new KDTree();

    public WorleyNoise(int x, int y, int z, int r, int n)
    {   
        Npoint = n;
        dimX = x;
        dimY = y;
        dimZ = z;
        res = r;
        fPoint = new Vector3[Npoint];
        kFactor = 2;
    }

    public int ComputeValue(Vector3 point) // Return value between 0-100
    {
        int idx = tree.FindNearestK(point, kFactor);
        float d = Vector3.Distance(point, fPoint[idx]);
        float diag = Mathf.Sqrt(dimX * dimX + dimY * dimY + dimZ * dimZ);
        int per = (int) MapValue(0, diag, 0, 5000, d);
        // print("WorleyNoise");
        // print(d);
        // print(per);
        return per;
    }

    public float MapValue(float oldlow, float oldhigh, float newlow, float newhigh, float val)
    {
        return newlow + (newhigh - newlow) * ((val-oldlow)/(oldhigh-oldlow));
    }

    public void InitializeFeaturePoint()
    {   
        // print("Initialize Feature Point");
        for (int i = 0; i < Npoint; i++)
        {   
            // print(i);
            fPoint[i] = new Vector3(Random.Range(2*res, dimX-res), Random.Range(2*res, dimY-res), Random.Range(2*res, dimZ-res));
        }
        tree = KDTree.MakeFromPoints(fPoint);
    }

    

    // public List<Vector3> getfPoint()
    // {
    //     return fPoint;
    // }
}
