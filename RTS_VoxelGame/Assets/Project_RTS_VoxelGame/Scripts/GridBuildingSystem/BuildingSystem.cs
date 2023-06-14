using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NekraliusDevelopmentStudio.BuildingSystemUtility;

namespace NekraliusDevelopmentStudio
{
    public class BuildingSystem : MonoBehaviour
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //CompleteCodeName - (Code Version)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        #region - Singleton Pattern -
        public static BuildingSystem Instance;
        private void Awake() => Instance = this;
        #endregion

        [SerializeField] private GameObject mouseIndicator;
        [SerializeField] private InputManager inputManager => InputManager.Instance;
        [SerializeField] private Grid buildingGrid;

        [SerializeField] private ObjectsDatabaseSO objectDatabase;
        [SerializeField] private int selectedObjectIndex = -1;

        [SerializeField] private GameObject gridCellPrefab;
        [SerializeField] private Transform gridCellContent;

        public bool isInPlacementMode = false;

        ObjectGridData floorData;
        ObjectGridData furturineData;

        [SerializeField] private BuildingPreviewSystem previewSystem;
        private Vector3Int lastDetectedPosition = Vector3Int.zero;

        [SerializeField] private ObjectPlacer objectPlacer;

        IBuildingState buildingState;


        private void Start()
        {
            StopPlacement();

            GridWorldGenerator.Instance.currentMap.floorData = new ObjectGridData();
            GridWorldGenerator.Instance.currentMap.furnitureData = new ObjectGridData();
        }

        private void Update()
        {
            //if (selectedObjectIndex < 0) return;
            if (isInPlacementMode)
            {
                Vector3 mousePos = inputManager.GetSelectedMapPosition();
                Vector3Int gridPosition = buildingGrid.WorldToCell(mousePos);

                if (lastDetectedPosition != gridPosition)
                {
                    mouseIndicator.transform.position = mousePos;
                    buildingState.UpdateState(gridPosition);
                    lastDetectedPosition = gridPosition;
                }
            }
        }
        #region - Placement Interaction -
        public void StartPlacement(int ID)
        {
            StopPlacement();
            ChangeGridDrawState(true);

            buildingState = new PlacementState(ID, buildingGrid, previewSystem, objectDatabase, floorData, furturineData, objectPlacer);


            inputManager.OnClicked += PlaceStructure;
            inputManager.OnExit += StopPlacement;
        }
        private void StopPlacement()//This method stop the structure placement mode.
        {
            /* The method resets the selectedObjectIndex to an non object number, later the method deactivate the grid draw state, and finally the method unassign the method input actions.
             * 
             */
            if (buildingState == null) return;

            ChangeGridDrawState(false);
            buildingState.EndState();

            inputManager.OnClicked -= PlaceStructure;
            inputManager.OnExit -= StopPlacement;
            lastDetectedPosition = Vector3Int.zero;

            buildingState = null;
        }
        #endregion

        #region - Structure Spawning -
        private void PlaceStructure()
        {
            if (inputManager.IsPointerOverUI()) return;
            Vector3 mousePosition = inputManager.GetSelectedMapPosition();
            Vector3Int gridPosition = buildingGrid.WorldToCell(mousePosition);

            buildingState.OnAction(gridPosition);
        }
        #endregion

        #region - Grid Drawning Management -
        public void GenerateGridDraw()
        {
            //The below statement reset the complete grid draw, destroying all grid childs.
            if (gridCellContent.childCount > 0) foreach (Transform objTrans in gridCellContent) Destroy(objTrans.gameObject);        

            //The below statements walks all the grid cells and draw the grid cell if the cell is not ocupied and is not an water cell.
            Cell[,] gridData = GridWorldGenerator.Instance.grid;
            int size = GridWorldGenerator.Instance.currentMap.size;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    if (!(gridData[x,y].cellOcupied || gridData[x, y].cellType.CheckIfWater()))
                    {
                        Vector3 currentPos = new Vector3(gridData[x, y].cellCoord.x, 0.1f, gridData[x, y].cellCoord.y);
                        Instantiate(gridCellPrefab, currentPos, Quaternion.identity, gridCellContent);
                    }
                }
            }
        }
        public void ChangeGridDrawState(bool state) 
        {
            gridCellContent.gameObject.SetActive(state); 
            isInPlacementMode = state;
        } //This method changes the active state of the grid drawning.
        #endregion
    }
}