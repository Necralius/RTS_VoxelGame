using System;
using System.Collections.Generic;
using UnityEngine;
using static NekraliusDevelopmentStudio.BuildingSystemUtility;

namespace NekraliusDevelopmentStudio
{
    public class WorldGenerationUtility : MonoBehaviour
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //WorldGenerationUtility - (0.4)
        //Code State: Functional
        //This code represents an World Generation Support Library that holds several types of Data Structures ou classes that are essential to the world generation system, also, this class carries an dseing pattern called singleton that make this class acessible from any other class in the project. 

        #region - Singleton Pattern -
        public static WorldGenerationUtility Instance;
        private void Awake() => Instance = this;
        #endregion

        public TerrainTypeData terrainTypes;
    }

    #region - World Generation Utility Package -

    #region - World Cell Data Type -
    /* The below class represents an map block cell data structured, holds a terrainy type, the cell height value, the cell ocupied state and the cell coordinates on
     * the map.
     */

    public class BlockCell
    {       
        #region - Cell Data -
        public TerrainType cellType;
        public float heightValue;
        public bool cellOcupied = false;

        public Vector2Int cellCoordinate;
        #endregion
        
        //The below statements represents an constructor that receive the cell coordinates and his height, thus automatically generating the cell aspects based on this data.
        public BlockCell(float heightValue, int xCoord, int yCoord)
        {
            this.heightValue = heightValue;
            cellType = WorldGenerationUtility.Instance.terrainTypes.GetType(heightValue);
            cellCoordinate = new Vector2Int(xCoord, yCoord);
        }

        public bool CheckType(TerrainType typeToCheck) => cellType.Equals(typeToCheck);//This method verifies if the passed Terrain Type is equals or not to this cell current terrain type.
    }
    #endregion

    #region - Terrain Type Data -
    [CreateAssetMenu(fileName ="Terrain Type Data", menuName = "RTS_Voxel/World Generation/Terrain Type Database")]
    public class TerrainTypeData : ScriptableObject
    {
        //This statements represents an scriptable object that saves all the terrain types in a serializable list.
        public List<TerrainType> terrainTypes;

        //The below method receives an height from a noise scale height map and return an valid terrain type that fits in the height value proximity.
        public TerrainType GetType(float height)
        {
            foreach (var terrainType in terrainTypes) if (Math.Abs(height - terrainType.heightValue) <= 0.2f) return terrainType;
            return terrainTypes[0];
        }
    }

    //The below struture represents an terrain type, that holds an terrain height value, an texture color, an block material and a name for indentification purpose.
    //Also the statement holds an method that verifies if the current type instance is an water type (In this system water type is binding).
    [Serializable]
    public class TerrainType
    {
        public string typeName = "Block_";
        public float heightValue = 1f;
        public Color blockColor = new Color(255, 255, 255, 255);
        public Material blockMaterial;

        public bool CheckIfWater() => typeName == "Water";
    }
    #endregion

    #region - World Generation Utility -
    //The below statements describes an Map data structure that holds the map structures types, the map settings, debug optins, complete cells data and holds the complete builded structure data.
    [Serializable]
    public struct MapData
    {
        [Header("Map General Settings")]
        public int size;
        public float noiseScale;

        [Header("Grid Building System")]
        public float gridCellSize;

        [Header("Object Placement Settings")]
        public List<ProceduralStructures> mapStructures;

        [HideInInspector] public BlockCell[,] mapCompleteData;
        public bool DrawnGrid;

        public ObjectGridData floorData;
        public ObjectGridData structureData;
    }

    //The below statements describes an structure for map procedural structure generation, this structure holds the structure prefab, density, noise, and a structure name only for indentification purpuse.
    [Serializable]
    public class ProceduralStructures
    {
        public string structureName = "structure_";
        public GameObject structurePrefab;
        public float noiseScale = 0.05f;
        public float density = 0.5f;
        public int BuildingStructureID = 99;
    }
    #endregion

    #region - Terrain State -
    public enum TerrainState
    {
        NewGeneration,
        SaveLoading
    }
    #endregion

    #endregion
}