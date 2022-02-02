/** 
 *  @brief  Worley Noise Class
 *  @author A Lesage 
 *  @date   01-2022
 ***********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static KDTree;

/**
* @brief Implents worley noise generation and evaluation
*/
public class WorleyNoise : MonoBehaviour
{
    // number of feature points
    private int Npoint;  
    // degree of neighboors i'm looking at
    public int kFactor = 1;
    // resolution 
    public int res; 
    // dimension in X,Y,Z Axis
    private int dimX, dimY, dimZ;
    // feature points defining the noise 
    public Vector3[] fPoint; 
    // defining a KDtree to find neighbor efficiently
    private KDTree tree = new KDTree(); 

    /**
    * @brief Constructor with values
    * @param x,y,z dimension X,Y,Z-axis
    * @param r resolution
    * @param n number of feauture points
    * @param k degree of neighbor
    */
    public WorleyNoise(int x, int y, int z, int r, int n, int k)
    {   
        Npoint = n;
        dimX = x;
        dimY = y;
        dimZ = z;
        res = r;
        fPoint = new Vector3[Npoint];
        kFactor = k;
    }

    /**
    * @brief Evaluate value for 1 point
    * @param point point to evaluate
    * @param precision precision for the computing
    * @return value of noise between 0-precision
    */
    public int ComputeValue(Vector3 point, int precision) // 
    {
        // find the index of the next neighbor of degree kFactor
        int idx = tree.FindNearestK(point, kFactor);

        float d = Vector3.Distance(point, fPoint[idx]);
        float diag = Mathf.Sqrt(dimX * dimX + dimY * dimY + dimZ * dimZ); // Maximal value in a cuboid
        int val = (int) MapValue(0, diag, 0, precision, d); 
        
        // debug print
        // print("WorleyNoise");
        // print(d);
        // print(val);
        
        return val;
    }

    /**
    * @brief projection of a scale to another
    * @param oldlow, oldhigh old extreme values 
    * @param newlow, newhigh new extreme values
    * @param val value to be normalized
    * @return normalized value
    */
    public float MapValue(float oldlow, float oldhigh, float newlow, float newhigh, float val)
    {
        return newlow + (newhigh - newlow) * ((val-oldlow)/(oldhigh-oldlow));
    }

    /**
    * @brief Initialize random feature points in space
    */
    public void InitializeFeaturePoint()
    {   
        // print("Initialize Feature Point");
        for (int i = 0; i < Npoint; i++)
        {              
            // Define a random point in space
            fPoint[i] = new Vector3(Random.Range(2*res, dimX-res), Random.Range(2*res, dimY-res), Random.Range(2*res, dimZ-res));
        }

        // Generate KDTree from feature points
        tree = KDTree.MakeFromPoints(fPoint);
    }
}
