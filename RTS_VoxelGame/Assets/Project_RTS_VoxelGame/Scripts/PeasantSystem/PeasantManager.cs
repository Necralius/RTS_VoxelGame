using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NekraliusDevelopmentStudio.BuildingSystemUtility;

namespace NekraliusDevelopmentStudio
{
    public class PeasantManager : MonoBehaviour
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //PeasantManager - (0.4)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        #region - Singleton Pattern -
        public static PeasantManager Instance;
        void Awake() => Instance = this;
        #endregion

        #region - Dependencies -
        private InputManager inputManager => InputManager.Instance;
        MapData mapData => GridWorldGenerator.Instance.currentMap;
        #endregion

        public List<PeasantModel> allPeasantsUnits = new List<PeasantModel>();

        public GameObject peasantPrefab;

        public SerializableDictionary<PeasantType, GameObject> peasantsTypes = new();

        #region - Preview System -
        [SerializeField] private Material previewMaterialPrefab;
        private Material previewMaterialInstance;
        #endregion

        public GameObject currentPreview;

        #region - BuildIn Methods -
        private void Start()
        {
            previewMaterialInstance = new Material(previewMaterialPrefab);
        }
        #endregion

        #region - Preview Mode -
        private void StartPreviewMode(GameObject preview)
        {
            currentPreview = preview;
            Renderer[] renderers = currentPreview.GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                Material[] materials = renderer.materials;
                for (int i = 0; i < materials.Length; i++) materials[i] = previewMaterialInstance;
                renderer.materials = materials;
            }
        }
        private void StopPreviewMode()
        {
            Destroy(currentPreview.gameObject);
        }
        #endregion

        private void SpawnPeasantModel(Vector3 peasantPosition)
        {
            PeasantModel peasant = Instantiate(peasantPrefab, peasantPosition, Quaternion.identity, transform).GetComponent<PeasantModel>();
            allPeasantsUnits.Add(peasant);
        }
        private void SpawnPesantOnGrid()
        {
            Vector3 mousePos = inputManager.GetSelectedMapPosition();
            Vector3Int gridPosition = BuildingSystem.Instance.buildingGrid.WorldToCell(mousePos);

            if (CanSpawnPeasantAt(mapData.mapCompleteData[gridPosition.x, gridPosition.y], gridPosition))
            {
                StopPreviewMode();
                SpawnPeasantModel(mousePos);
            }
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
            GameObject preview = Instantiate(peasantPrefab, transform);
            StartPreviewMode(preview);
        }
        private void Update()
        {
            if (ModeManager.Instance.IsOnState(ModeType.PeasantSpawningMode))
            {
                if (Input.GetMouseButtonDown(0)) SpawnPesantOnGrid();

                if (!inputManager.IsPointerOverUI())
                {
                    if (currentPreview.Equals(null)) return;
                    Vector3 mousePos = inputManager.GetSelectedMapPosition();
                    currentPreview.transform.position = mousePos;
                }
            }
        }
    }
}