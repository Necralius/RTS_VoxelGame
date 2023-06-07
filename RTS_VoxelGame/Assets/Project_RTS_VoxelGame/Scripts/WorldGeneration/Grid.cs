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
        public Material edgeMaterial;

        private void Start()
        {
            GenerateNoiseMap(currentMap);
            GeneraterFallofMap(currentMap);

            int size = currentMap.size;
            grid = new Cell[size, size];

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float noiseValue = noiseMap[x, y];
                    noiseValue -= falloffMap[x, y];
                    Cell cell = new Cell(noiseValue);                   
                    grid[x, y] = cell;
                }
            }

            DrawTerrainMesh(grid, currentMap);
            DrawEdgeMesh(grid, currentMap);
            DrawTexture(grid, currentMap);
        }

        private void GenerateNoiseMap(MapData data)
        {
            int size = data.size;
            float scale = data.noiseScale;

            float[,] noiseMap = new float[size, size];
            (float xOffset, float yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float noiseValue = Mathf.PerlinNoise(x * scale + xOffset, y * scale + yOffset);
                    noiseMap[x, y] = noiseValue;
                }
            }
            this.noiseMap = noiseMap;
        }
        private void GeneraterFallofMap(MapData data)
        {
            int size = data.size;
            float[,] falloffMap = new float[size, size];
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float xv = x / (float)size * 2 - 1;
                    float yv = y / (float)size * 2 - 1;
                    float v = Mathf.Max(Mathf.Abs(xv), Mathf.Abs(yv));
                    falloffMap[x, y] = Mathf.Pow(v, 3f) / (Mathf.Pow(v, 3f) + Mathf.Pow(2.2f - 2.2f * v, 3f));
                }
            }
            this.falloffMap = falloffMap;
        }
        private void DrawTerrainMesh(Cell[,] grid, MapData data)
        {
            int size = data.size;

            Mesh mesh = new Mesh();
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Vector2> uvs = new List<Vector2>();
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    Cell cell = grid[x, y];
                    if (!cell.cellType.CheckIfWater())
                    {
                        Vector3 a = new Vector3(x - .5f, 0, y + .5f);
                        Vector3 b = new Vector3(x + .5f, 0, y + .5f);
                        Vector3 c = new Vector3(x - .5f, 0, y - .5f);
                        Vector3 d = new Vector3(x + .5f, 0, y - .5f);
                        Vector2 uvA = new Vector2(x / (float)size, y / (float)size);
                        Vector2 uvB = new Vector2((x + 1) / (float)size, y / (float)size);
                        Vector2 uvC = new Vector2(x / (float)size, (y + 1) / (float)size);
                        Vector2 uvD = new Vector2((x + 1) / (float)size, (y + 1) / (float)size);
                        Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                        Vector2[] uv = new Vector2[] { uvA, uvB, uvC, uvB, uvD, uvC };
                        for (int k = 0; k < 6; k++)
                        {
                            vertices.Add(v[k]);
                            triangles.Add(triangles.Count);
                            uvs.Add(uv[k]);
                        }
                    }
                }
            }
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.RecalculateNormals();

            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }
        void DrawEdgeMesh(Cell[,] grid, MapData data)
        {
            int size = data.size;

            Mesh mesh = new Mesh();
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    Cell cell = grid[x, y];
                    if (!cell.cellType.CheckIfWater())
                    {
                        if (x > 0)
                        {
                            Cell left = grid[x - 1, y];
                            if (left.cellType.CheckIfWater())
                            {
                                Vector3 a = new Vector3(x - .5f, 0, y + .5f);
                                Vector3 b = new Vector3(x - .5f, 0, y - .5f);
                                Vector3 c = new Vector3(x - .5f, -1, y + .5f);
                                Vector3 d = new Vector3(x - .5f, -1, y - .5f);
                                Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                                for (int k = 0; k < 6; k++)
                                {
                                    vertices.Add(v[k]);
                                    triangles.Add(triangles.Count);
                                }
                            }
                        }
                        if (x < size - 1)
                        {
                            Cell right = grid[x + 1, y];
                            if (right.cellType.CheckIfWater())
                            {
                                Vector3 a = new Vector3(x + .5f, 0, y - .5f);
                                Vector3 b = new Vector3(x + .5f, 0, y + .5f);
                                Vector3 c = new Vector3(x + .5f, -1, y - .5f);
                                Vector3 d = new Vector3(x + .5f, -1, y + .5f);
                                Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                                for (int k = 0; k < 6; k++)
                                {
                                    vertices.Add(v[k]);
                                    triangles.Add(triangles.Count);
                                }
                            }
                        }
                        if (y > 0)
                        {
                            Cell down = grid[x, y - 1];
                            if (down.cellType.CheckIfWater())
                            {
                                Vector3 a = new Vector3(x - .5f, 0, y - .5f);
                                Vector3 b = new Vector3(x + .5f, 0, y - .5f);
                                Vector3 c = new Vector3(x - .5f, -1, y - .5f);
                                Vector3 d = new Vector3(x + .5f, -1, y - .5f);
                                Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                                for (int k = 0; k < 6; k++)
                                {
                                    vertices.Add(v[k]);
                                    triangles.Add(triangles.Count);
                                }
                            }
                        }
                        if (y < size - 1)
                        {
                            Cell up = grid[x, y + 1];
                            if (up.cellType.CheckIfWater())
                            {
                                Vector3 a = new Vector3(x + .5f, 0, y + .5f);
                                Vector3 b = new Vector3(x - .5f, 0, y + .5f);
                                Vector3 c = new Vector3(x + .5f, -1, y + .5f);
                                Vector3 d = new Vector3(x - .5f, -1, y + .5f);
                                Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                                for (int k = 0; k < 6; k++)
                                {
                                    vertices.Add(v[k]);
                                    triangles.Add(triangles.Count);
                                }
                            }
                        }
                    }
                }
            }
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateNormals();

            GameObject edgeObj = new GameObject("Edge");
            edgeObj.transform.SetParent(transform);

            MeshFilter meshFilter = edgeObj.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            MeshRenderer meshRenderer = edgeObj.AddComponent<MeshRenderer>();
            meshRenderer.material = edgeMaterial;
        }
        void DrawTexture(Cell[,] grid, MapData data)
        {
            int size = data.size;

            Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            Color[] colorMap = new Color[size * size];

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    Cell cell = grid[x, y];
                    colorMap[y * size + x] = cell.cellType.blockColor;
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