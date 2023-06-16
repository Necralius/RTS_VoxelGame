using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using static NekraliusDevelopmentStudio.NDS_Utility;
using static NekraliusDevelopmentStudio.BuildingSystemUtility;

namespace NekraliusDevelopmentStudio
{
    public class GridWorldGenerator : MonoBehaviour
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //GridWorldGenerator - (1.3)
        //State: Functional
        //This code represents an voxel world generation class that uses noise maps and falloff maps to generate meshes, also the system generates the world meshes and the world procedural structures.

        #region - Singleton Pattern -
        public static GridWorldGenerator Instance;
        private void Awake() => Instance = this;
        #endregion

        #region - Grid General Data - 
        [Header("Grid Settings")]
        public MapData currentMap = new MapData();
        [HideInInspector] public BlockCell[,] grid;
     
        [Header("Grid Dependencies")]
        public Material terrainMaterial;
        public Material edgeMaterial;
        #endregion

        #region - Grid Raw Data -
        private float[,] noiseMap;
        private float[,] falloffMap;

        private Mesh mesh;
        private MeshFilter meshFilter;
        private MeshCollider meshCollider;
        private MeshRenderer meshRenderer;
        private GameObject edgeObject;
        #endregion

        #region - Debug Settings -
        [Header("Debug Settings")]
        public bool DrawGizmos = false;
        #endregion

        //================================Methods================================//

        #region - BuiltIn Methods -
        private void Start()
        {
            GenerateCompleteMap();
        }
        public void GenerateCompleteMap()
        {
            foreach(Transform objTrans in transform) Destroy(objTrans.gameObject);

            #region - Terrain Formation Data -
            GenerateNoiseMap(currentMap);
            GeneraterFallofMap(currentMap);
            #endregion

            #region - Map Block Cell Generation -
            int size = currentMap.size;
            grid = new BlockCell[size, size];

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float noiseValue = noiseMap[x, y];
                    noiseValue -= falloffMap[x, y];
                    BlockCell cell = new BlockCell(noiseValue, x, y);
                    grid[x, y] = cell;
                }
            }
            #endregion

            #region - Mesh, Texture and Structure Generation -
            DrawTerrainMesh(grid, currentMap);
            DrawEdgeMesh(grid, currentMap);
            DrawTexture(grid, currentMap);
            GenerateAllStructures(currentMap);
            BuildingSystem.Instance.GenerateGridDraw();
            #endregion
        }
        #endregion

        #region - Noise Map Generation -
        private void GenerateNoiseMap(MapData data)
        {
            //This method generates an noise map using the perlin noise gradient that will represent the map terrain formation.

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
            //This method generates an FallofMap that turn the map formation to an island formation type.

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
        private void DrawTerrainMesh(BlockCell[,] grid, MapData data)
        {
            /* This complex method creates an mesh for the terrain, as an recurrent map generator the method first verifies if the mesh is null or not, if is not, the
             * mesh is cleaned, if the mesh reference is null, the method creates a new mesh.
             * Mesh Creation -> The complete map cells are traveled and for each block that is not water is created some vertices, triangles and an UV map is 
             * calculated and saved on lists, posteriorly the method pass all this data to the mesh and recalculate his normals, thus forming an complete map mesh.
             */

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
                    BlockCell cell = grid[x, y];
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

            //Also, some needed components are added to the object, like an mesh filter, and mesh renderer and a mesh collider, also the method assign the current mesh to this needed components.

            if (meshFilter == null) { meshFilter = gameObject.AddComponent<MeshFilter>(); }
            if (meshRenderer == null) { meshRenderer = gameObject.AddComponent<MeshRenderer>(); }
            if (meshCollider == null) { meshCollider = gameObject.AddComponent<MeshCollider>(); }

            meshFilter.mesh = mesh;
            meshCollider.sharedMesh = mesh;
        }
        #endregion

        #region - Edge Mesh Generation -
        void DrawEdgeMesh(BlockCell[,] grid, MapData data)
        {
            /* This complex method creates an mesh for the blocks edge.
             * Mesh Creation -> The complete map cells are traveled and for each block traveled, the system tries to find any water cell nearby, if has an water cell,
             * an edge mesh is calculated an created, some vertices and triangles are calculated and saved on lists, posteriorly later the method pass all this data 
             * to the mesh and recalculate his normals, thus forming edge meshes to the map blocks.
             */

            int size = data.size;

            Mesh mesh = new Mesh();

            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    BlockCell cell = grid[x, y];
                    if (!cell.cellType.CheckIfWater())
                    {
                        if (x > 0)// Verifing the cell in the left cell position of the block
                        {
                            BlockCell left = grid[x - 1, y];
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
                        if (x < size - 1)// Verifing the cell in the right cell position of the block
                        {
                            BlockCell right = grid[x + 1, y];
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
                        if (y > 0)// Verifing the cell in down cell position of the block
                        {
                            BlockCell down = grid[x, y - 1];
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
                        if (y < size - 1)// Verifing the cell in the up cell position of the block
                        {
                            BlockCell up = grid[x, y + 1];
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

            //The method also verifies if the edge gameobject is null, if it is the method creates the gameobject from scratch, adding his needed components and assinging the mesh needed data.

            if (edgeObject == null) edgeObject = new GameObject("EdgeMesh");

            //Also the method resets the edge object position, so that he stays in the same position of the map gameobject.
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
        void DrawTexture(BlockCell[,] grid, MapData data)
        {
            /* This method generates an terrain texture for the map mesh.
             * Texture Generation -> First the system instatiate an 2D Texture on a local variable, later, the system travel all the cells on the map data and assing 
             * the color to the texture using as base the cell type of the current cell, this type also saves an block color, which in turn is assimilated to the 
             * current cell pixels.
             * After this generation, the method passes this texture to the current map material and posteriorly passes this material instance to the current 
             * meshRenderer.
             */

            int size = data.size;

            Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            Color[] colorMap = new Color[size * size];

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    BlockCell cell = grid[x, y];
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
            //This method travel all the possible structures on the map data and generate one by one using the GenerateStructure method.
            foreach(ProceduralStructures structureToGenerate in data.mapStructures) GenerateStructure(grid, data, structureToGenerate);
        }

        private void GenerateStructure(BlockCell[,] grid, MapData data, ProceduralStructures structure)
        {
            /*This method generates procedural structures on the map mesh.
             * Structure Generation -> First the method verifies if the scale or density is equals zero, if it is the generation is canceled. 
             * Otherwise, the method generates an noisemap using the structure noise value. Later the method travels all the cells and verifies some conditions, 
             * first if the cell is an water type cell, or if the cell is ocupied, this will be ignored, otherwise the method will get an random number and if this 
             * number is near to the noise height value, the structure will be instatiated in this cell. (Also the structure will receive an random rotation, and an 
             * random scale for variability purpose.) 
             */

            if (structure.density <= 0 || structure.noiseScale == 0) return;

            #region - Structure Noise Map -
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
            #endregion

            #region - Structure Instatiation -

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    BlockCell cell = grid[x, y];
                    if (!cell.cellType.CheckIfWater() && !cell.cellOcupied)
                    {
                        float v = Random.Range(0f, structure.density);

                        if (noiseMap[x, y] < v)
                        {
                            cell.cellOcupied = true;

                            GameObject structureSpawned = Instantiate(structure.structurePrefab, transform);
                            structureSpawned.transform.position = new Vector3(x, 0, y) + new Vector3(data.gridCellSize, 0, data.gridCellSize) * 0.5f;
                            structureSpawned.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
                            structureSpawned.transform.localScale = Vector3.one * Random.Range(0.8f, 1.2f);

                            ObjectsDatabaseSO objectDatabase = BuildingSystem.Instance.objectDatabase;

                            int selectedObjIndex = BuildingSystem.Instance.objectDatabase.objectsData.FindIndex(data => data.ID == 99);

                            int index = ObjectPlacer.Instance.PlaceObject(structureSpawned);

                            BuildingSystem.Instance.structureData.AddObjectAt(new Vector3Int(x, 0, y), new Vector2Int(1,1), 99, ObjectPlacer.Instance.PlaceObject(structureSpawned));
                        }
                    }
                }
            }
            #endregion



        }
        #endregion

        #region - Map Gizmos Draw Generation -
        private void OnDrawGizmos()
        {
            /*This method draw gizmos for debugging purpose.
            * Debugging -> First the method verifies if the applicatino is playing and if the DrawGizmos bool is true, if it is true, the method will draw an cube on
            * each cell of the grid, also each cell will have the correponding calors of the cell block type. (Also, if the cell is occupied the gizmos block will 
            * receive the red color to indicate this information.)
            */

            if (Application.isPlaying && DrawGizmos)
            {
                for (int y = 0; y < currentMap.size; y++)
                {
                    for (int x = 0; x < currentMap.size; x++)
                    {

                        BlockCell cell = grid[x, y];
                        if (cell.cellOcupied) Gizmos.color = Color.red;
                        else Gizmos.color = cell.cellType.blockColor;

                        Vector3 pos = new Vector3(x, 0, y) + new Vector3(currentMap.gridCellSize, 0, currentMap.gridCellSize) * 0.5f;
                        Gizmos.DrawCube(pos, Vector3.one);                    
                    }
                }
            }
        }
        #endregion
    }
}