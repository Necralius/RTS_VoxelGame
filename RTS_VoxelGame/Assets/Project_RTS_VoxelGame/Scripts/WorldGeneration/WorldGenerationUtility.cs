using System;
using System.Collections.Generic;
using UnityEngine;
using static NekraliusDevelopmentStudio.BuildingSystemUtility;

namespace NekraliusDevelopmentStudio
{
    public class WorldGenerationUtility : MonoBehaviour
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //CompleteCodeName - (Code Version)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        #region - Singleton Pattern -
        public static WorldGenerationUtility Instance;
        private void Awake() => Instance = this;
        #endregion


        public List<TerrainType> terrainTypes;

        public TerrainType GetType(float height)
        {
            foreach (var terrainType in terrainTypes)
            {
                if (Math.Abs(height - terrainType.heightValue) <= 0.2f) return terrainType;
            }
            return terrainTypes[0];
        }
    }

    [Serializable]
    public class TerrainType
    {
        public string typeName = "Block_";
        public float heightValue = 1f;
        public Color blockColor = new Color(255, 255, 255, 255);
        public Material blockMaterial;

        public bool CheckIfWater() => typeName == "Water";
    }

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

        [HideInInspector] public Cell[,] mapCompleteData;
        public bool DrawnGrid;

        public ObjectGridData floorData;
        public ObjectGridData furnitureData;
    }

    [Serializable]
    public class ProceduralStructures
    {
        public string structureName = "structure_";
        public GameObject structurePrefab;
        public float noiseScale = 0.05f;
        public float density = 0.5f;
    }
}