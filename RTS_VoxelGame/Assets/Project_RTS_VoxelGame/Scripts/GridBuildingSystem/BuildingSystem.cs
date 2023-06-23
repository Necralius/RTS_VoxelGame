using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using static NekraliusDevelopmentStudio.BuildingSystemUtility;
using static UnityEngine.ParticleSystem;

namespace NekraliusDevelopmentStudio
{
    public class BuildingSystem : MonoBehaviour
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //BuildingSystem - (0.5)
        //State: Functional
        //This code represents and build system main class, that holds all the need data and connect all interactions with data models, structures and input.

        #region - Singleton Pattern -
        public static BuildingSystem Instance;
        #endregion

        #region - Building System Data and Dependencies -
        private InputManager inputManager => InputManager.Instance;
        public Grid buildingGrid;

        public ObjectsDatabaseSO objectDatabase;

        [SerializeField] private GameObject gridCellPrefab;
        [SerializeField] private Transform gridCellContent;

        [HideInInspector] public ObjectGridData floorData;
        [HideInInspector] public ObjectGridData structureData;       

        [SerializeField] private BuildingPreviewSystem previewSystem;
        [SerializeField] private ObjectPlacer objectPlacer;

        private Vector3Int lastDetectedPosition = Vector3Int.zero;

        public IBuildingState buildingState;

        public bool isOnPlacementMode;
        #endregion

        //================================Methods================================//

        #region - BuiltIn Methods -
        private void Awake()
        {
            //The below statements get the current objects data references.
            floorData = GridWorldGenerator.Instance.currentMap.floorData = new ObjectGridData();
            structureData = GridWorldGenerator.Instance.currentMap.structureData = new ObjectGridData();

            Instance = this;
        }
        private void Start()
        {
            //The below statement stops any placement action.
            StopPlacement();
            ChangeBuildingMode(false);                     
        }
        private void Update()
        {            
            //The below statements execute all mouse interactions and calculations, first, the system verifies if the placement mode is active, if it is, the mouse position is calculated and the grid detection is deiplayed usiing the building state interface interaction structure.
            if (isOnPlacementMode)
            {
                Vector3 mousePos = inputManager.GetSelectedMapPosition();
                Vector3Int gridPosition = buildingGrid.WorldToCell(mousePos);

                //Also, the system detects if the current mouse position is the last detected position, if it is, the system stops the mouse calculation for optimization purpose.
                if (lastDetectedPosition != gridPosition)
                {                  
                    buildingState.UpdateState(gridPosition);
                    lastDetectedPosition = gridPosition;
                }
            }
        }
        #endregion

        #region - Structure Build Interaction -
        public void StartPlacement(int ID)
        {
            /* This method starts the structure placement system, firstly the method stop any placement that can be active before, preventing any bug that can occur, 
             * posteriorly, the method active the grid building state also instatiating the current bulding state as an placement state and finaly the method assing 
             * the correct methods to the unity event actions.
             */
            StopPlacement();
            ChangeBuildingMode(true);

            buildingState = new PlacementState(ID, buildingGrid, previewSystem, objectDatabase, objectPlacer, structureData, floorData);

            inputManager.OnClicked += PlaceStructure;
            inputManager.OnExit += StopPlacement;
        }
        private void StopPlacement()
        {
            /* This method stops the structure placement mode, firstly verifing if the buildingState is null, if is not null the method proceed with the
             * placement cancelation, later, the method execute some actions, like deactivate the grid building state, call the end state, unassing all the
             * unity event action with the correct methods, and finally the method resets the last detected position and the building state variables, so preparing the
             * system to an new interaction.
             */
            if (buildingState == null) return;

            ChangeBuildingMode(false);
            buildingState.EndState();

            inputManager.OnClicked -= PlaceStructure;
            inputManager.OnExit -= StopPlacement;
            lastDetectedPosition = Vector3Int.zero;

            buildingState = null;
        }
        public void StartRemoving()
        {
            /* This method starts the structure removing system, firstly the method stop any placement that can be active before, preventing any bug that can occur,
             * posteriorly, the method active the grid building state also instatiating the current bulding state as an Removing state and finaly the method assing
             * the correct methods to the unity event actions.
             */
            StopPlacement();
            ChangeBuildingMode(true);
            buildingState = new RemovingState(buildingGrid, previewSystem, objectPlacer, structureData, floorData);

            inputManager.OnClicked += PlaceStructure;
            inputManager.OnExit += StopPlacement;
        }
        #endregion

        #region - Structure Spawning -
        private void PlaceStructure()
        {
            /* This method updates and calculates some needed data, the method calls the OnAction method from the building state, also calculates the mouse position
             * and from he position calculates the grid position.
             * NOTE: The method also verifies if the mouse is pointing an UI object, if it is, the method returns, so generation more optimization to the system.
             */

            if (inputManager.IsPointerOverUI()) return;
            Vector3 mousePosition = inputManager.GetSelectedMapPosition();
            Vector3Int gridPosition = buildingGrid.WorldToCell(mousePosition);

            buildingState.OnAction(gridPosition);
        }
        #endregion

        #region - Grid Drawning Management -
        public void GenerateGridDraw() 
        {
            /*This method generates the complete grid draw that will be active in the building mode, first, as an recurrent map generator system, the method destroy
             * all the previewsly grid items, and generate all again every time that the method is called.
             * Grid Generating -> The system travels all the block cells data, only verifing if the cell is an Water cell or if it is occupied, and all of this 
             * statements returns as false, the method instatiate an grid visual GameObject.
             */

            if (gridCellContent.childCount > 0) foreach (Transform objTrans in gridCellContent) Destroy(objTrans.gameObject);  
            
            BlockCell[,] gridData = GridWorldGenerator.Instance.grid;
            int size = GridWorldGenerator.Instance.currentMap.size;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    if (!(gridData[x,y].cellOcupied || gridData[x, y].cellType.CheckIfWater()))
                    {
                        Vector3 currentPos = new Vector3(gridData[x, y].cellCoordinate.x, 0.1f, gridData[x, y].cellCoordinate.y);
                        Instantiate(gridCellPrefab, currentPos, Quaternion.identity, gridCellContent);
                    }
                }
            }
        }
        public void ChangeBuildingMode(bool state) //This method merely deactivate and activate the bulding state using an bool argument pased on the method call.
        {
            gridCellContent.gameObject.SetActive(state);
            isOnPlacementMode = state;

            if (state) ModeManager.Instance.SetMode(ModeType.BuildPlacementMode);
        }
        #endregion
    }
}