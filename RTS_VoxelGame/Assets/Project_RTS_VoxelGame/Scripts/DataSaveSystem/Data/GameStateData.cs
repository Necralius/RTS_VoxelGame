using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NekraliusDevelopmentStudio
{
    [Serializable]
    public class GameStateData
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //CompleteCodeName - (Code Version)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        public CameraData cameraData;

        public ResourceData resourceData;

        public SettingsData settingsData;

        public InputData inputData;

        public List<StructureData> structureData;

        [HideInInspector] public MapCompleteSave mapCompleteData;

        public GameStateData()
        {
            cameraData = new CameraData();
            resourceData = new ResourceData();
            settingsData = new SettingsData();
            inputData = new InputData();
            structureData = new List<StructureData>();
        }
        
        public void SaveGame(Camera cam, ResourceManager resourceManager, GridWorldGenerator gridData)
        {
            Debug.Log("Making a full game save!");
            cameraData = new CameraData(cam);
            resourceData = new ResourceData(resourceManager.metalQuantity, resourceManager.woodQuantity);
            settingsData.SaveData();

            Mesh map, edge;
            (map, edge) = gridData.GetMapMeshData();

            mapCompleteData = new MapCompleteSave(map, edge, gridData.mapTexture);
        }
        public void SaveGame(SettingsData currentData) => settingsData = currentData;
    }

    public enum SaveType
    {
        FullSave,
        Settings,
        StructureSave
    }

    [Serializable]
    public struct CameraData
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;

        public CameraData(Camera camera)
        {
            Position = camera.gameObject.transform.position;
            Rotation = camera.gameObject.transform.rotation;
            Scale = camera.gameObject.transform.localScale;
        }
    }

    [Serializable]
    public struct ResourceData
    {
        public float metalQuantity;
        public float woodQuantity;

        public ResourceData(float metal, float wood)
        {
            this.metalQuantity = metal;
            this.woodQuantity = wood;
        }
    }
    [Serializable]
    public struct StructureData
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public StructureData(GameObject structure)
        {
            position = structure.transform.position;
            rotation = structure.transform.rotation;
            scale = structure.transform.localScale;
        }
    }
    [Serializable]
    public struct SettingsData
    {
        public int vsyncCount;
        public int textureQuality;
        public int resolutionIndex;
        public int targetFramerate;

        public float masterVolume;
        public float audioEffectsVolume;
        public float musicVolume;

        public void SaveData()
        {
            vsyncCount = QualitySettings.vSyncCount;
            textureQuality = QualitySettings.globalTextureMipmapLimit;
            targetFramerate = Application.targetFrameRate;
            resolutionIndex = SettingsMenuManager.Instance.resolutionDrp.value;

            masterVolume = AudioManager.Instance.masterSlider.value;
            audioEffectsVolume = AudioManager.Instance.effectsSlider.value;
            musicVolume = AudioManager.Instance.musicSlider.value;
        }
    }
    public struct InputData
    {
        [Header("Camera Input Data")]
        public float CameraMoveSpeed;
        public float CameraMoveSmoothing;

        public float CameraRotationSpeed;
        public float CameraRotationSmoothing;

        public float ZoomSpeed;
        public float ZoomSmoothing;

        [Header("Key Bindings")]
        public KeyCode key1;
    }

    [Serializable]
    public class MapCompleteSave
    {
        public MeshPersistentData terrainMeshData = new MeshPersistentData();
        public MeshPersistentData edgeMeshData = new MeshPersistentData();

        public Color[] textureColourMap;

        public MapCompleteSave(Mesh mapMesh, Mesh edgeMesh, Color[] colourMap)
        {
            terrainMeshData = new MeshPersistentData(mapMesh, true);
            edgeMeshData = new MeshPersistentData(edgeMesh, false);

            textureColourMap = colourMap;
        }
    }
    [Serializable]
    public struct MeshPersistentData
    {
        public List<Vector3> vertices;
        public List<Vector2> uvs;
        public List<int> triangles;

        public MeshPersistentData(Mesh meshToSave, bool carryUV)
        {
            vertices = new List<Vector3>();
            triangles = new List<int>();

            vertices.AddRange(meshToSave.vertices.ToList());
            triangles = meshToSave.triangles.ToList();
            if (carryUV) uvs = meshToSave.uv.ToList();
            else uvs = new List<Vector2>();
        }
    }
}