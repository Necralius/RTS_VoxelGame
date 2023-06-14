using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NekraliusDevelopmentStudio
{
    public class BuildingSystemUtility : MonoBehaviour
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //BuildingSystemUtility - (0.1)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)
        public class PlacementState : IBuildingState
        {
            //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
            //CompleteCodeName - (Code Version)
            //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
            //This code represents (Code functionality or code meaning)

            private int selectedObjectIndex = -1;
            int ID;
            Grid grid;
            BuildingPreviewSystem previewSystem;
            ObjectsDatabaseSO objectDatabase;

            ObjectPlacer objectPlacer;

            #region - Placement State Start Active -
            public PlacementState(int iD, Grid grid, BuildingPreviewSystem previewSystem, ObjectsDatabaseSO objectDatabase, ObjectGridData floorData, ObjectGridData furturineData, ObjectPlacer objectPlacer)
            {
                ID = iD;
                this.grid = grid;
                this.previewSystem = previewSystem;
                this.objectDatabase = objectDatabase;
                this.objectPlacer = objectPlacer;

                selectedObjectIndex = objectDatabase.objectsData.FindIndex(data => data.ID == ID);

                if (selectedObjectIndex > -1) previewSystem.ShowPlacementPreview(objectDatabase.objectsData[selectedObjectIndex].prefab, objectDatabase.objectsData[selectedObjectIndex].Size);
                else throw new System.Exception($"No object with ID: {iD}");
            }
            #endregion

            #region - Placement Preview State -
            public void EndState() => previewSystem.DisablePlacementPreview();
            #endregion

            #region - Placement Action -
            public void OnAction(Vector3Int gridPosition)
            {
                bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

                if (!placementValidity) return;

                int index = objectPlacer.PlaceObject(objectDatabase.objectsData[selectedObjectIndex].prefab, grid.CellToWorld(gridPosition));

                ObjectGridData selectedData = objectDatabase.objectsData[selectedObjectIndex].ID == 0 ? GridWorldGenerator.Instance.currentMap.floorData : GridWorldGenerator.Instance.currentMap.furnitureData;

                selectedData.AddObjectAt(gridPosition, objectDatabase.objectsData[selectedObjectIndex].Size, objectDatabase.objectsData[selectedObjectIndex].ID, index - 1);

                previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
            }
            private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
            {
                ObjectGridData selectedData = objectDatabase.objectsData[selectedObjectIndex].ID == 0 ? GridWorldGenerator.Instance.currentMap.floorData : GridWorldGenerator.Instance.currentMap.furnitureData;

                return selectedData.CanPlaceObjectAt(gridPosition, objectDatabase.objectsData[selectedObjectIndex].Size);
            }
            #endregion

            #region - Placement General Update -
            public void UpdateState(Vector3Int gridPosition)
            {
                bool placementValidity = false;
                if (BuildingSystem.Instance.isInPlacementMode && selectedObjectIndex >= 0) placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

                previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
            }
            #endregion
        }
    }
}