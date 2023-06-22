using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NekraliusDevelopmentStudio.BuildingSystemUtility;

namespace NekraliusDevelopmentStudio
{
    public class PeasantManager : MonoBehaviour
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //CompleteCodeName - (Code Version)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        #region - Singleton Pattern -
        public static PeasantManager Instance;
        void Awake() => Instance = this;
        #endregion

        public List<PeasantModel> allPeasantsUnits = new List<PeasantModel>();

        public GameObject peasantPrefab;

        private InputManager inputManager => InputManager.Instance;

        public Grid mapGrid;
        MapData mapData => GridWorldGenerator.Instance.currentMap;

        private void SpawnPeasantModel(Vector3 peasantPosition)
        {
            PeasantModel peasant = Instantiate(peasantPrefab, peasantPosition, Quaternion.identity, transform).GetComponent<PeasantModel>();
            allPeasantsUnits.Add(peasant);
        }
        private void SpawnPesantOnGrid()
        {
            Vector3 mousePos = inputManager.GetSelectedMapPosition();
            Vector3Int gridPosition = mapGrid.WorldToCell(mousePos);

            if (CanSpawnPeasantAt(mapData.mapCompleteData[gridPosition.x, gridPosition.y], gridPosition)) SpawnPeasantModel(gridPosition);
            else Debug.Log("Cannot Spawn peasant at this place!");
        }
        private bool CanSpawnPeasantAt(BlockCell cell, Vector3Int GridPos)
        {
            ObjectGridData floorData = BuildingSystem.Instance.floorData;
            ObjectGridData structureData = BuildingSystem.Instance.structureData;
            return floorData.CanPlaceObjectAt(GridPos, new Vector2Int(1, 1)) && structureData.CanPlaceObjectAt(GridPos, new Vector2Int(1, 1)) && !cell.cellOcupied;
        }
        public void StartSpawnMode()
        {
            ModeManager.Instance.SetMode(ModeType.PeasantSpawningMode);
        }
        private void Update()
        {
            if (ModeManager.Instance.IsOnState(ModeType.PeasantSpawningMode))
            {
                if (Input.GetMouseButtonDown(0)) SpawnPesantOnGrid();
            }
        }
    }
}