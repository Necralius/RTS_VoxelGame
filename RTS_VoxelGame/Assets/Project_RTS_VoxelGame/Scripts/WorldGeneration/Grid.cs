using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NekraliusDevelopmentStudio
{
    public class Grid : MonoBehaviour
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //CompleteCodeName - (Code Version)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        [Header("Grid Settings")]
        public MapData currentMap = new MapData();
        Cell[,] grid;

        private float[,] noiseMap;
        private float[,] falloffMap;
        public Material terrainMaterial;

        private void Start()
        {
            GenerateNoiseMap(currentMap);
            GeneraterFallofMap(currentMap);
            grid = new Cell[currentMap.size, currentMap.size];

            for (int y = 0; y < currentMap.size; y++)
            {
                for (int x = 0; x < currentMap.size; x++)
                {
                    float noiseValue = noiseMap[x, y];
                    noiseValue -= falloffMap[x, y];
                    Cell cell = new Cell(noiseValue);                   
                    grid[x, y] = cell;
                }
            }

            DrawTerrainMesh(grid, currentMap);
            DrawTexture(grid, currentMap);
        }

        private void GenerateNoiseMap(MapData data)
        {
            float[,] noiseMap = new float[data.size, data.size];

            float xOffset = Random.Range(-10000f, 10000f);
            float yOffset = Random.Range(-10000f, 10000f);

            for (int y = 0; y < data.size; y++)
            {
                for (int x = 0; x < data.size; x++)
                {
                    float noiseValue = Mathf.PerlinNoise(x * currentMap.noiseScale + xOffset, y * currentMap.noiseScale + yOffset);
                    noiseMap[x, y] = noiseValue;
                }
            }
            this.noiseMap = noiseMap;
        }
        private void GeneraterFallofMap(MapData data)
        {
            float[,] falloffMap = new float[data.size, data.size];
            for (int y = 0; y < data.size; y++)
            {
                for (int x = 0; x < data.size; x++)
                {
                    float xv = x / (float)data.size * 2 - 1;
                    float yv = y / (float)data.size * 2 - 1;
                    float v = Mathf.Max(Mathf.Abs(xv), Mathf.Abs(yv));
                    falloffMap[x, y] = Mathf.Pow(v, 3f) / (Mathf.Pow(v, 3f) + Mathf.Pow(2.2f - 2.2f * v, 3f));
                }
            }
            this.falloffMap = falloffMap;
        }
        private void DrawTerrainMesh(Cell[,] grid, MapData data)
        {
            Mesh mesh = new Mesh();
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            for (int y = 0; y < data.size; y++)
            {
                for (int x = 0; x < data.size; x++)
                {
                    Cell currentCell = grid[x, y];
                    if (currentCell.cellType.typeName != "Water")
                    {
                        Vector3 a = new Vector3(x - .5f, 0, y + .5f);
                        Vector3 b = new Vector3(x + .5f, 0, y + .5f);
                        Vector3 c = new Vector3(x - .5f, 0, y - .5f);
                        Vector3 d = new Vector3(x + .5f, 0, y - .5f);
                        Vector3[] v = new Vector3[] { a, b, c, b, d, c };

                        for (int k = 0; k < 6; k++)
                        {
                            vertices.Add(v[k]);
                            triangles.Add(triangles.Count);
                        }
                    }
                }
            }
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateNormals();

            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }
        void DrawTexture(Cell[,] grid, MapData data)
        {
            Texture2D texture = new Texture2D(data.size, data.size);
            Color[] colorMap = new Color[data.size * data.size];

            for (int y = 0; y < data.size; y++)
            {
                for (int x = 0; x < data.size; x++)
                {
                    Cell cell = grid[x, y];
                    colorMap[y * data.size + x] = cell.cellType.blockColor;
                }
            }
            texture.filterMode = FilterMode.Point;
            texture.SetPixels(colorMap);
            texture.Apply();

            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.material = terrainMaterial;
            meshRenderer.material.mainTexture = texture;
        }


        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;
            for (int y = 0; y < currentMap.size; y++)
            {
                for (int x = 0; x < currentMap.size; x++)
                {
                    Cell cell = grid[x, y];
                    Color newColor = cell.cellType.blockColor;
                    Gizmos.color = newColor;

                    Vector3 pos = new Vector3(x, 0, y);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }

    }
}