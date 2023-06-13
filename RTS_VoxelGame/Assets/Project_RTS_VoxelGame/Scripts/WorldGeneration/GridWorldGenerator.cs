using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static NekraliusDevelopmentStudio.NDS_Utility;

namespace NekraliusDevelopmentStudio
{
    public class GridWorldGenerator : MonoBehaviour
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //CompleteCodeName - (Code Version)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        #region - Singleton Pattern -
        public static GridWorldGenerator Instance;
        private void Awake() => Instance = this;
        #endregion

        [Header("Grid Settings")]
        public MapData currentMap = new MapData();
        [HideInInspector] public Cell[,] grid;
     
        [Header("Grid Dependencies")]
        public Material terrainMaterial;
        public Material edgeMaterial;

        private float[,] noiseMap;
        private float[,] falloffMap;

        private Mesh mesh;
        private MeshFilter meshFilter;
        private MeshCollider meshCollider;
        private MeshRenderer meshRenderer;
        private GameObject edgeObject;

        public bool DrawGizmos = false;

        private void Start()
        {
            GenerateCompleteMap();
        }
        public void GenerateCompleteMap()
        {
            foreach(Transform objTrans in transform) Destroy(objTrans.gameObject);

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
                    Cell cell = new Cell(noiseValue, x, y);
                    grid[x, y] = cell;
                }
            }

            DrawTerrainMesh(grid, currentMap);
            DrawEdgeMesh(grid, currentMap);
            DrawTexture(grid, currentMap);
            GenerateAllStructures(currentMap);
            BuildingSystem.Instance.GenerateGridDraw();
        }

        #region - Noise Map Generation -
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
        #endregion

        #region - Falloff Map Generation -
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
        #endregion

        #region - Terrain Mesh Drawning -
        private void DrawTerrainMesh(Cell[,] grid, MapData data)
        {
            if (mesh != null) mesh.Clear();
            else mesh = new Mesh();

            int size = data.size;

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
                        Vector3 a = new Vector3(x - data.gridCellSize, 0, y + data.gridCellSize);
                        Vector3 b = new Vector3(x + data.gridCellSize, 0, y + data.gridCellSize);
                        Vector3 c = new Vector3(x - data.gridCellSize, 0, y - data.gridCellSize);
                        Vector3 d = new Vector3(x + data.gridCellSize, 0, y - data.gridCellSize);
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

            if (meshFilter == null) { meshFilter = gameObject.AddComponent<MeshFilter>(); }
            meshFilter.mesh = mesh;
            if (meshRenderer == null) { meshRenderer = gameObject.AddComponent<MeshRenderer>(); }
            if (meshCollider == null) { meshCollider = gameObject.AddComponent<MeshCollider>(); }
            meshCollider.sharedMesh = mesh;
        }
        #endregion

        #region - Edge Mesh Generation -
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
                                Vector3 a = new Vector3(x - data.gridCellSize, 0, y + data.gridCellSize);
                                Vector3 b = new Vector3(x - data.gridCellSize, 0, y - data.gridCellSize);
                                Vector3 c = new Vector3(x - data.gridCellSize, -1, y + data.gridCellSize);
                                Vector3 d = new Vector3(x - data.gridCellSize, -1, y - data.gridCellSize);
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
                                Vector3 a = new Vector3(x + data.gridCellSize, 0, y - data.gridCellSize);
                                Vector3 b = new Vector3(x + data.gridCellSize, 0, y + data.gridCellSize);
                                Vector3 c = new Vector3(x + data.gridCellSize, -1, y - data.gridCellSize);
                                Vector3 d = new Vector3(x + data.gridCellSize, -1, y + data.gridCellSize);
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
                                Vector3 a = new Vector3(x - data.gridCellSize, 0, y - data.gridCellSize);
                                Vector3 b = new Vector3(x + data.gridCellSize, 0, y - data.gridCellSize);
                                Vector3 c = new Vector3(x - data.gridCellSize, -1, y - data.gridCellSize);
                                Vector3 d = new Vector3(x + data.gridCellSize, -1, y - data.gridCellSize);
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
                                Vector3 a = new Vector3(x + data.gridCellSize, 0, y + data.gridCellSize);
                                Vector3 b = new Vector3(x - data.gridCellSize, 0, y + data.gridCellSize);
                                Vector3 c = new Vector3(x + data.gridCellSize, -1, y + data.gridCellSize);
                                Vector3 d = new Vector3(x - data.gridCellSize, -1, y + data.gridCellSize);
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

            if (edgeObject == null) edgeObject = new GameObject("EdgeMesh");

            edgeObject.transform.SetParent(transform);
            edgeObject.transform.position = Vector3.zero;

            if (!edgeObject.GetComponent<MeshFilter>())
            {
                MeshFilter meshFilter = edgeObject.AddComponent<MeshFilter>();
                meshFilter.mesh = mesh;
            }
            else edgeObject.GetComponent<MeshFilter>().mesh = mesh;

            if (!edgeObject.GetComponent<MeshRenderer>())
            {
                MeshRenderer meshRenderer = edgeObject.AddComponent<MeshRenderer>();
                meshRenderer.material = edgeMaterial;
            }
            else edgeObject.GetComponent<MeshRenderer>().material = edgeMaterial;

            if (!edgeObject.GetComponent<MeshCollider>()) 
            {
                meshCollider = edgeObject.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = mesh;
            }
            else edgeObject.GetComponent<MeshCollider>().sharedMesh = mesh;
        }
        #endregion

        #region - Texture Generation
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
        #endregion

        #region - Objects and Structures Placement -
        private void GenerateAllStructures(MapData data)
        {
            foreach(ProceduralStructures structureToGenerate in data.mapStructures) GenerateStructure(grid, data, structureToGenerate);
        }

        private void GenerateStructure(Cell[,] grid, MapData data, ProceduralStructures structure)
        {
            if (structure.density <= 0 || structure.noiseScale == 0) return;

            int size = data.size;
            float scale = structure.noiseScale;

            float[,] noiseMap = new float[size, size];

            (float xOffset, float yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000, 10000));

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float noiseValuue = Mathf.PerlinNoise(x * scale + xOffset, y * scale + yOffset);
                    noiseMap[x, y] = noiseValuue;
                }
            }

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    Cell cell = grid[x, y];
                    if (!cell.cellType.CheckIfWater() && !cell.cellOcupied)
                    {
                        float v = Random.Range(0f, structure.density);
                        if (noiseMap[x,y] < v)
                        {
                            cell.cellOcupied = true;

                            GameObject structureSpawned = Instantiate(structure.structurePrefab, transform);
                            structureSpawned.transform.position = new Vector3(x, 0, y) + new Vector3(data.gridCellSize, 0, data.gridCellSize) * 0.5f;
                            structureSpawned.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
                            structureSpawned.transform.localScale = Vector3.one * Random.Range(0.8f, 1.2f);
                        }
                    }
                }
            }        
        }
        private (bool, Cell) ValidateCell(Vector2Int cellPos, MapData data)
        {
            int xCoord = cellPos.x;
            int yCoord = cellPos.y;
            // Verifies if the coordinate is negative, witch is impossible and verifies if the coordinate is inside of the grid size.
            if (xCoord < 0 || yCoord < 0 || xCoord > data.size || yCoord > data.size) return (false, null);
            else if (grid[xCoord, yCoord].cellOcupied || grid[xCoord, yCoord].cellType.CheckIfWater()) return (false, null);
            else
            {
                return (true, grid[xCoord, yCoord]);
                // -> Verifies if the cell is free of space and if is water, then return the result
            }
            //return (true, grid[xCoord, yCoord]);
        }
        #endregion

        #region - Map Gizmos Draw Generation -
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;
            for (int y = 0; y < currentMap.size; y++)
            {
                for (int x = 0; x < currentMap.size; x++)
                {
                    if (DrawGizmos)
                    {
                        Cell cell = grid[x, y];
                        if (cell.cellOcupied) Gizmos.color = Color.red;
                        else Gizmos.color = cell.cellType.blockColor;

                        Vector3 pos = new Vector3(x, 0, y) + new Vector3(currentMap.gridCellSize, 0, currentMap.gridCellSize) * 0.5f;
                        Gizmos.DrawCube(pos, Vector3.one);
                    }

                    if (currentMap.DrawnGrid)
                    {
                        currentMap.mapCompleteData = grid;
                        if (!(currentMap.mapCompleteData[x, y].cellType.CheckIfWater() || currentMap.mapCompleteData[x,y].cellOcupied))
                        {
                            Gizmos.color = Color.white;
                            Gizmos.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1));
                            Gizmos.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y));

                            Gizmos.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x - 1, y));
                            Gizmos.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y - 1));

                            Gizmos.DrawLine(GetWorldPosition(x, y + 1), GetWorldPosition(x + 1, y + 1));
                            Gizmos.DrawLine(GetWorldPosition(x + 1, y), GetWorldPosition(x + 1, y + 1));     
                        }
                    }
                }
            }
        }
        private Vector3 GetWorldPosition(int x, int y) => new Vector3(x, 0, y) * currentMap.gridCellSize;
        #endregion
    }
}