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
            private int selectedObjectIndex = -1;
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
                //This statements represents an object Placement State contructor, that collects all the needed data and also set up and starts the placement preview system.
                ID = iD;
                this.grid = grid;
                this.previewSystem = previewSystem;
                this.objectDatabase = objectDatabase;
                this.objectPlacer = objectPlacer;
                this.furnitureData = furnitureData;
                this.floorData = floorData;

                selectedObjectIndex = objectDatabase.objectsData.FindIndex(data => data.ID == ID);

                if (selectedObjectIndex > -1) previewSystem.ShowPlacementPreview(objectDatabase.objectsData[selectedObjectIndex]);
                else throw new System.Exception($"No object with ID: {iD}");
            }
            #endregion

            #region - Placement Preview State -
            //The below method deactivate the preview system.
            public void EndState() => previewSystem.DisablePlacementPreview();
            #endregion

            #region - Placement Action -
            public void OnAction(Vector3Int gridPosition)
            {
                /* The below method makes an series of verifications and calculations to execute an structure placement action. 
                 * 
                 * Placement Action -> First the method check for placemente viability or validity using the CheckPlacementValidity method, if the placement is valid,
                 * the method gets the object index on the object placer using the PlaceObject method, which in turn already set the object on the 3D World, after this,
                 * the method place the object data on his correct "Object Layer" or ObjectGridData, also the method updates the position of cursor and preview object 
                 * using the UpdatePosition on the PreviewSystem Instance.
                 */

                bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
                bool priceValidity = ResourceManager.Instance.CheckForBuild(objectDatabase.objectsData[ID].buildNeeds);

                if (!placementValidity || !priceValidity) return;

                int index = objectPlacer.PlaceObject(objectDatabase.objectsData[selectedObjectIndex].prefab, grid.CellToWorld(gridPosition), BuildingPreviewSystem.Instance.currentEulerAngles);

                ObjectGridData selectedData = objectDatabase.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;

                selectedData.AddObjectAt(gridPosition, objectDatabase.objectsData[selectedObjectIndex], index);

                previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
                AudioManager.Instance.PlayClip(AudioManager.Instance.audioDatabase.GetClip("PlaceStructure"), AudioType.SoundEffect);
            }
            private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
            {
                //This method verifies the structure placement viability verifing if the object exists in the current object layer.
                ObjectGridData selectedData = objectDatabase.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;

                return selectedData.CanPlaceObjectAt(gridPosition, objectDatabase.objectsData[selectedObjectIndex].Size);
            }
            #endregion

            #region - Placement General Update -
            public void UpdateState(Vector3Int gridPosition)
            {
                /*This method is called in update method on the Build System class, is used to recurrently check the placement validity, also updating the Preview System, 
                * that automatically updates the preview object and the cursor color feedbacks.
                */

                bool placementValidity = false;
                if (BuildingSystem.Instance.isOnPlacementMode && selectedObjectIndex >= 0) placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

                previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
            }
            #endregion
        }
        #endregion

        #region - Structure Remove System -
        public class RemovingState : IBuildingState
        {
            #region - Removing State Data -
            private int gameObjectIndex = -2;
            Grid grid;
            BuildingPreviewSystem previewSystem;
            ObjectGridData floorData;
            ObjectGridData furnitureData;
            ObjectPlacer objectPlacer;
            #endregion

            #region - Removing State Constructor -
            public RemovingState(Grid grid, BuildingPreviewSystem previewSystem, ObjectPlacer objectPlacer, ObjectGridData furnitureData, ObjectGridData floorData)
            {
                //This statements represents an object Removing State contructor, that collects all the needed data and also set up and starts the remove preview system.
                this.grid = grid;
                this.previewSystem = previewSystem;
                this.objectPlacer = objectPlacer;
                this.furnitureData = furnitureData;
                this.floorData = floorData;

                previewSystem.StartRemovePreview();
            }
            #endregion

            #region - Placement Preview State -
            //The below method deactivate the preview system.
            public void EndState() => previewSystem.DisablePlacementPreview();
            #endregion

            #region - Remove Main Action -
            public void OnAction(Vector3Int gridPosition)
            {
                /* This method represents an remove action from the remove building state.
                 * Removing Object -> First the method select an structure layer using the following logic, if the structure is present in some object layer, means 
                 * that the structure can be removed, later the system verifies if the selected layer is valid or not, if it is the method starts the structure remove
                 * actions, first the method get the structure index from the layer, layer using this index to remove the item from the world, also the method use the 
                 * object position on world to remove it from the structure layer, and finally the system updates the preview system, that automatically updates the
                 * cursor and the object preview using the current grid position and also using his validity.
                 */

                ObjectGridData selectedData = null;
                if (!furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one)) selectedData = furnitureData;
                else if (!floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one)) selectedData = floorData;

                if (selectedData == null)
                {
                    //Cant remove structure
                    AudioManager.Instance.PlayClip(AudioManager.Instance.audioDatabase.GetClip(""), AudioType.SoundEffect);
                    return;
                }
                else
                {
                    gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);

                    if (gameObjectIndex == -1) return;

                    AudioManager.Instance.PlayClip(AudioManager.Instance.audioDatabase.GetClip("RemoveStructure"), AudioType.SoundEffect);
                    selectedData.RemoveObjectAt(gridPosition);
                    objectPlacer.RemoveObjectAt(gameObjectIndex);
                }
                Vector3 cellPosition = grid.CellToWorld(gridPosition);
                previewSystem.UpdatePosition(cellPosition, CheckSelection(gridPosition));
            }
            //The below method returns an boolean that informs if an structure can be placed in one of the structure layers.
            private bool CheckSelection(Vector3Int gridPosition) => !(furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one) && floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one));
            #endregion

            #region - Placement General Update -
            public void UpdateState(Vector3Int gridPosition)
            {
                //This method send the needed data to the preview system, system which in turn automatically update the cursor and object preview using the validity
                //to set the feedback color correctly. 

                bool validity = CheckSelection(gridPosition);
                previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), validity);
            }
            #endregion
        }

        #endregion

        #region - Object Grid Data -
        public class ObjectGridData
        {
            /* This class represents an Layer of placed structures, the class holds an dictionary of placedObjects and some methods that represents the structure 
             * layer interaction.
             * 
             * CalculatePositions -> This method receive some data like the object startPosition and his size, later the method makes an loop of the object size on X float
             * and join all this positions values in an Vector3Int List and finally returns this list.
             * 
             * AddObjectAt -> This method  receive all the object data and calculate the positions to occupy, later the method creates an placementData instance and
             * add this to the dictionary also verifing if the data already exists, if already exists, the method return an excepcion.
             * 
             * CanPlaceObjectAt -> This method search for space for the structure and at the same time, verifies if the space is listed on the aready spawned structures,
             * later the method returns an boolead result that tells if the current structure is valid or not.
             * 
             * GetRepresentationIndex -> this method gets the structure index on the list and return it, using as base his first cell position on the grid passed as an argument.
             * 
             * RemoveObjectAt -> This method travels every occupied position and remove it from the dictionary, thus removing the item presence from the layer.
             */

            Dictionary<Vector3Int, PlacementData> placedObjects = new();

            #region - Structure Add -
            public void AddObjectAt(Vector3Int gridPos, ObjectData objectData, int objectIndex)
            {
                List<Vector3Int> positionsToOccupy = CalculatePositions(gridPos, objectData.Size);
                PlacementData data = new PlacementData(positionsToOccupy, objectData.ID, objectIndex);
                foreach (var pos in positionsToOccupy)
                {
                    if (placedObjects.ContainsKey(pos)) throw new Exception($"Dictionary already contains this cell position {pos}");
                    placedObjects[pos] = data;
                }
            }
            public void ResetGrid() => placedObjects.Clear();
            #endregion

            #region - Map Position Calculation -
            private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
            {
                List<Vector3Int> returnValue = new();

                for (int x = 0; x < objectSize.x; x++) for (int y = 0; y < objectSize.y; y++) returnValue.Add(gridPosition + new Vector3Int(x, 0, y));
                return returnValue;
            }
            #endregion

            #region - Map Position Search -
            public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
            {
                List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
                foreach (var pos in positionToOccupy) if (placedObjects.ContainsKey(pos)) return false;
                return true;
            }
            #endregion

            #region - Item Layer Search -
            internal int GetRepresentationIndex(Vector3Int gridPosition)
            {
                if (!placedObjects.ContainsKey(gridPosition)) return -1;
                return placedObjects[gridPosition].PlacedObjectIndex;
            }
            #endregion

            #region - Item Remove -
            internal void RemoveObjectAt(Vector3Int gridPosition)
            {
                foreach (var pos in placedObjects[gridPosition].occupiedPositions) placedObjects.Remove(pos);
            }
            #endregion
        }
        #endregion

        #region - Placement Data -
        public class PlacementData
        {
            //This class represents an placement data that holds all data of an placed structure data, the object holds data like all the positions that the structure occupy, the object ID and index.

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

    #region - Building State Model -
    public interface IBuildingState
    {
        //The below statements represents and interface structure for an building state.
        void EndState();
        void OnAction(Vector3Int gridPosition);
        void UpdateState(Vector3Int gridPosition);
    }
    #endregion
}