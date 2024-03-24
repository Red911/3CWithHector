using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private Texture2D perlinNoise;
    [SerializeField] private Mesh mesh;
    
    
    private int _widthNoise = 0;
    private int _heigthNoise = 0;
    private Color[] _perlinPixels;
    [SerializeField] private float sizeTile;
    private Vector3[] vertices;
    
    private Vector2[] texCoord;
    [SerializeField] private float heigthMax = 10f;
    
    //Array Triangle
    private int[] triangles;
    
    

    private void Awake()
    {
        _perlinPixels = perlinNoise.GetPixels();
        _widthNoise = perlinNoise.width;
        _heigthNoise = perlinNoise.height;

        vertices = new Vector3[_widthNoise * _heigthNoise];
        
        //Triangle initialise
        triangles = new int[((_widthNoise - 1) * (_heigthNoise - 1 )) * 6];
        
        
        texCoord = new Vector2[vertices.Length];

        for (int i = 0; i < perlinNoise.width; i++)
        {
            for (int j = 0; j < perlinNoise.height; j++)
            {
                float height = _perlinPixels[Index2Dto1D(i,j,_widthNoise)].r;
                
                vertices[Index2Dto1D(i,j,_widthNoise)] = CoordToWorldPostion(i, height * heigthMax, j);
                texCoord[Index2Dto1D(i, j, _widthNoise)] = PixelToUV(i, j);
            }
        }
        
        //Triangle fonction
        CreateTriangle();
        
        
    }

    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = texCoord; 
        
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }

    void CreateTriangle()
    {
        int trian = 0;

        for (int X = 0; X < _widthNoise - 1; X++)
        {
            for (int Y = 0; Y < _heigthNoise - 1; Y++)
            {
                triangles[trian + 0] = Index2Dto1D(X, Y, _widthNoise);
                triangles[trian + 1] = Index2Dto1D(X, Y + 1, _widthNoise);
                triangles[trian + 2] = Index2Dto1D(X + 1, Y, _widthNoise);
                
                triangles[trian + 3] = Index2Dto1D(X + 1, Y, _widthNoise);
                triangles[trian + 4] = Index2Dto1D(X, Y + 1, _widthNoise);
                
                triangles[trian + 5] = Index2Dto1D(X + 1, Y + 1, _widthNoise);

                trian += 6;
            }
        }
    }
    
    int Index2Dto1D(int i, int j, int width) { return i + j * width; }

    Vector3 CoordToWorldPostion(int x, int y, int z) { return new Vector3(x,y,z);}
    
    Vector3 CoordToWorldPostion(float x, float y, float z) { return new Vector3(x,y,z);}

    Vector2 PixelToUV(int i, int j)
    {
        if (i == 0 && j == 0)
        {
            return new Vector2(0, 0);
        }
        else if (i == 0 && j != 0)
        {
            return new Vector2(0, 1 / j);
        }
        else if (i != 0 && j == 0)
        {
            return new Vector2(1/i, 0);
        }
        
        return new Vector2(1 / i, 1 / j);

    }

   
}
