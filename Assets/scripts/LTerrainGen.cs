using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SmoothOp
{
    MEAN, EXPONENTIAL
}

public class LTerrainGen : MonoBehaviour {

	public void GenTerrain(LSystem system, int LoDLevel, SmoothOp oper)
    {
        LSymbol[,] systemString = system.systemString[LoDLevel];
        int xDim = systemString.GetLength(0);
        int yDim = systemString.GetLength(1);

        float[,] tileHeightmap = GenLowResHeightMap(system, LoDLevel, oper);
        //TODO:
        // - create terrainData (heightmap, textures, foliage, etc)
        // - instantiate terrain and assign terrain data
        // - water/ocean
        // - place object prefabs (rocks, etc)

    }

    public float[,] GenLowResHeightMap(LSystem system, int LoDLevel, SmoothOp oper)
    {
        LSymbol[,] systemString = system.systemString[LoDLevel];
        int xDim = systemString.GetLength(0);
        int yDim = systemString.GetLength(1);
        float[,] retMap = new float[xDim, yDim];

        for (int i = 0; i < xDim; ++i)
        {
            for (int j = 0; j < yDim; ++j)
            {
                LSymbol curSymbol = systemString[i, j];
                LPatch matchingPatch = system.GetLPatchMatch(curSymbol);
                retMap[i, j] = Random.Range(matchingPatch.minHeight, matchingPatch.maxHeight);
            }
        }

        //TODO: smoothing operations?

        return retMap;
    }

    public TerrainData GenTerrainData(LSystem system, int LoDLevel, float[,] lowResHeightMap)
    {
        LSymbol[,] systemString = system.systemString[LoDLevel];
        int xDim = systemString.GetLength(0);
        int yDim = systemString.GetLength(1);
        TerrainData retData = new TerrainData();

        //TODO: create more detailed heightmap from low res one passed in
        //TODO: assign textures and blend
        //TODO: 

        return retData;
    }
}
