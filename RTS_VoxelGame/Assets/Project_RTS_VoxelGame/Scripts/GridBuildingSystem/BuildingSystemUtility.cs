using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NekraliusDevelopmentStudio
{
    public static class BuildingSystemUtility
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //BuildingSystemUtility - (0.5)
        //State: Functional
        //This code represents (Code functionality or code meaning)

        #region - Placement System -
        public class PlacementState : IBuildingState
        {
            #region - Placement State Data -
            private int selectedObjectIndex = -2;
            int ID;
            Grid grid;
            BuildingPreviewSystem previewSystem;
            ObjectGridData furnitureData;
            ObjectGridData floorData;

            ObjectsDatabaseSO objectDatabase;

            ObjectPlacer objectPlacer;
            #endregion

            #region - Placement State Constructor -
            public PlacementState(int iD, Grid grid, BuildingPreviewSystem previewSystem, ObjectsDatabaseSO objectDatabase, ObjectPlacer objectPlacer, ObjectGridData furnitureData, ObjectGridData floorData)
            {
                ID = iD;
                this.grid = grid;
                this.previewSystem = previewSystem;
                this.objectDatabase = objectDatabase;
                this.objectPlacer = objectPlacer;
                this.furnitureData = furnitureData;
                this.floorData = floorData;

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

                ObjectGridData selectedData = objectDatabase.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;

                selectedData.AddObjectAt(gridPosition, objectDatabase.objectsData[selectedObjectIndex].Size, objectDatabase.objectsData[selectedObjectIndex].ID, index);

                previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
            }
            private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
            {
                ObjectGridData selectedData = objectDatabase.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;

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
        #endregion

        #region - Structure Remove System -
        public class RemovingState : IBuildingState
        {
            private int gameObjectIndex = -2;
            Grid grid;
            BuildingPreviewSystem previewSystem;
            ObjectGridData floorData;
            ObjectGridData furnitureData;
            ObjectPlacer objectPlacer;

            public RemovingState(Grid grid, BuildingPreviewSystem previewSystem, ObjectPlacer objectPlacer, ObjectGridData furnitureData, ObjectGridData floorData)
            {
                this.grid = grid;
                this.previewSystem = previewSystem;
                this.objectPlacer = objectPlacer;
                this.furnitureData = furnitureData;
                this.floorData = floorData;

                previewSystem.ShowPlacementRemovePreview();
            }

            public void EndState() => previewSystem.DisablePlacementPreview();

            public void OnAction(Vector3Int gridPosition)
            {
                ObjectGridData selectedData = null;
                if (!furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one)) selectedData = furnitureData;
                else if (!floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one)) selectedData = floorData;

                if (selectedData == null)
                {
                    //Cant remove structure
                    return;
                }
                else
                {
                    gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);

                    if (gameObjectIndex == -1) return;

                    selectedData.RemoveObjectAt(gridPosition);
                    objectPlacer.RemoveObjectAt(gameObjectIndex);
                }
                Vector3 cellPosition = grid.CellToWorld(gridPosition);
                previewSystem.UpdatePosition(cellPosition, CheckSelection(gridPosition));
            }
            private bool CheckSelection(Vector3Int gridPosition) => !(furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one) && floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one)); 

            public void UpdateState(Vector3Int gridPosition)
            {
                bool validity = CheckSelection(gridPosition);
                previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), validity);
            }
        }

        #endregion

        #region - Object Grid Data -
        public class ObjectGridData
        {
            Dictionary<Vector3Int, PlacementData> placedObjects = new();

            public void AddObjectAt(Vector3Int gridPos, Vector2Int objectSize, int id, int objectIndex)
            {
                List<Vector3Int> positionsToOccupy = CalculatePositions(gridPos, objectSize);
                PlacementData data = new PlacementData(positionsToOccupy, id, objectIndex);
                foreach (var pos in positionsToOccupy)
                {
                    if (placedObjects.ContainsKey(pos)) throw new Exception($"Dictionary already contains this cell position {pos}");
                    placedObjects[pos] = data;
                }
            }
            private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
            {
                List<Vector3Int> returnValue = new();

                for (int x = 0; x < objectSize.x; x++) for (int y = 0; y < objectSize.y; y++) returnValue.Add(gridPosition + new Vector3Int(x, 0, y));
                return returnValue;
            }
            public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
            {
                List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
                foreach (var pos in positionToOccupy) if (placedObjects.ContainsKey(pos)) return false;
                return true;
            }
            internal int GetRepresentationIndex(Vector3Int gridPosition)
            {
                if (!placedObjects.ContainsKey(gridPosition)) return -1;
                return placedObjects[gridPosition].PlacedObjectIndex;
            }
            internal void RemoveObjectAt(Vector3Int gridPosition)
            {
                foreach (var pos in placedObjects[gridPosition].occupiedPositions) placedObjects.Remove(pos);
            }          
        }
        #endregion

        #region - Placement Data -
        public class PlacementData
        {
            public List<Vector3Int> occupiedPositions;
            public int ID { get; private set; }
            public int PlacedObjectIndex { get; private set; }

            public PlacementData(List<Vector3Int> occupiedPositions, int ID, int PlacedObjectIndex)
            {
                this.occupiedPositions = occupiedPositions;
                this.ID = ID;
                this.PlacedObjectIndex = PlacedObjectIndex;
            }
        }
        #endregion
    }

}