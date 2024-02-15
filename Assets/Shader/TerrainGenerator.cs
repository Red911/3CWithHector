using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private Texture2D perlinNoise;
    private int _widthNoise;
    private float _heigthNoise;
    private Color[] _perlinPixels;

    private void Awake()
    {
        _perlinPixels = perlinNoise.GetPixels();
        _widthNoise = perlinNoise.width;

        for (int i = 0; i < perlinNoise.width; i++)
        {
            for (int j = 0; j < perlinNoise.height; j++)
            {
                _heigthNoise = _perlinPixels[Index2Dto1D(i,j,_widthNoise)].r;
            }
        }
        
    }
    
    int Index2Dto1D(int i, int j, int width) { return i + j * width; }

   
}
