using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        [SerializeField] private GameObject cellIndicator;
        [SerializeField] private InputManager inputManager => InputManager.Instance;
        [SerializeField] private Grid buildingGrid;

        [SerializeField] private ObjectsDatabaseSO objectDatabase;
        [SerializeField] private int selectedObjectIndex = -1;

        [SerializeField] private GameObject gridCellPrefab;
        [SerializeField] private Transform gridCellContent;

        private Renderer previewRenderer;

        private List<GameObject> placedGameObjects = new List<GameObject>();

        [SerializeField] private bool isInPlacementMode = false;

        private void Start()
        {
            StopPlacement();

            GridWorldGenerator.Instance.currentMap.floorData = new ObjectGridData();
            GridWorldGenerator.Instance.currentMap.furnitureData = new ObjectGridData();
            previewRenderer = cellIndicator.GetComponentInChildren<Renderer>();
        }

        private void Update()
        {
            //if (selectedObjectIndex < 0) return;
            Vector3 mousePos = inputManager.GetSelectedMapPosition();
            Vector3Int gridPosition = buildingGrid.WorldToCell(mousePos);

            bool placementValidity = true;
            if (isInPlacementMode && selectedObjectIndex >= 0) placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

            previewRenderer.material.color = placementValidity ? Color.yellow : Color.red;

            mouseIndicator.transform.position = mousePos;
            cellIndicator.transform.position = buildingGrid.CellToWorld(gridPosition);
        }
        public void StartPlacement(int ID)
        {
            StopPlacement();

            selectedObjectIndex = objectDatabase.objectsData.FindIndex(data => data.ID == ID);
            if (selectedObjectIndex < 0)
            {
                Debug.LogError($"No ID Found {ID}");
                return;
            }
            ChangeGridDrawState(true);
            inputManager.OnClicked += PlaceStructure;
            inputManager.OnExit += StopPlacement;
        }
        private void PlaceStructure()
        {
            if (inputManager.IsPointerOverUI()) return;
            Vector3 mousePosition = inputManager.GetSelectedMapPosition();
            Vector3Int gridPosition = buildingGrid.WorldToCell(mousePosition);

            bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

            if (!placementValidity) return;

            GameObject newObject = Instantiate(objectDatabase.objectsData[selectedObjectIndex].prefab);
            newObject.transform.position = buildingGrid.CellToWorld(gridPosition);
            placedGameObjects.Add(newObject);

            ObjectGridData selectedData = objectDatabase.objectsData[selectedObjectIndex].ID == 0 ? GridWorldGenerator.Instance.currentMap.floorData : GridWorldGenerator.Instance.currentMap.furnitureData;

            selectedData.AddObjectAt(gridPosition, objectDatabase.objectsData[selectedObjectIndex].Size, objectDatabase.objectsData[selectedObjectIndex].ID, placedGameObjects.Count - 1);
        }
        private void StopPlacement()
        {
            selectedObjectIndex = -1;

            ChangeGridDrawState(false);
            inputManager.OnClicked -= PlaceStructure;
            inputManager.OnExit -= StopPlacement;
        }
        private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
        {
            ObjectGridData selectedData = objectDatabase.objectsData[selectedObjectIndex].ID == 0 ? GridWorldGenerator.Instance.currentMap.floorData : GridWorldGenerator.Instance.currentMap.furnitureData;

            return selectedData.CanPlaceObjectAt(gridPosition, objectDatabase.objectsData[selectedObjectIndex].Size);
        }
        public void GenerateGridDraw()
        {
            if (gridCellContent.childCount > 0) foreach (Transform objTrans in gridCellContent) Destroy(objTrans.gameObject);        

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
        public void ChangeGridDrawState(bool state) { gridCellContent.gameObject.SetActive(state); isInPlacementMode = state; }
    }
}