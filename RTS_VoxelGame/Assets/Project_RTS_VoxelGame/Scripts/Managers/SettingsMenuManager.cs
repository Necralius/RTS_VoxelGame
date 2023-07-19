using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NekraliusDevelopmentStudio
{
    public class SettingsMenuManager : MonoBehaviour, ILoadableData
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //CompleteCodeName - (Code Version)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        //Drp = Dropdown

        #region - Singleton Pattern -
        public static SettingsMenuManager Instance;
        private void Awake() => Instance = this;
        #endregion

        [Header("Current Rendering Pipeline Asset")]
        public UniversalRenderPipelineAsset renderingPipeline;

        [Header("General Settings")]
        public Toggle vsyncTgl;
        public Toggle fullscreenTgl;

        public SliderFloatField renderScaleField;
        public SliderFloatField targetFramerateField;

        [Header("Antialising System")]
        public Toggle antialisingActive;
        public TMP_Dropdown antialisingQualityDrp;

        private int antialisingQualityIndex;
        private List<MsaaQuality> antialisingModes = new();

        [Header("Resolutions")]
        public TMP_Dropdown resolutionDrp;

        private int currentResolutionIndex;
        private List<Resolution> resolutions;

        [Header("Shadow Resolutions")]
        public Toggle castShadow;

        [Header("HDR System")]
        public Toggle hdrMode;
        public TMP_Dropdown hdr_BitsDrp;
        public int hdrBitsIndex = 0;
        private List<HDRColorBufferPrecision> hdr_Bits = new();

        [Header("Upscaling Filter System")]
        public TMP_Dropdown upscalingFilterDrp;
        public Toggle overriddeUpscalingMode;

        private List<UpscalingFilterSelection> upscalingModes = new();
        private int upscalingFilterModeIndex = 0;

        [Header("FSR Sharpeness")]
        public Toggle overrideFSR_Sharpeness;
        public SliderFloatField FSR_Sharpeness;

        [Header("Texture Quality")]
        public TMP_Dropdown textureQualityDrp;
        private int currentTextureQuality = 0;

        [Header("VSync Settings")]
        public TMP_Dropdown VSyncDrp;
        private int currentVSyncIndex = 0;

        public TerrainState currentTerrainState;

        public UnityEvent onSceneLoad;

        #region - BuiltIn Methods -
        private void Start()
        {
            onSceneLoad.AddListener(delegate { SaveManager.Instance.FullGameDataSave(); });

            FeedLists();

            UpdateTextureFilteringDropdown();
            UpdateVsyncDropDown();

            FeedAntialisingModes();
            FeedResolutions();
            FeedUpscalingFilterModes();
            FeedHDR_Bits();

            SaveManager.Instance.LoadAllData();

            UpdateSettings();
        }
        #endregion

        private void UpdateSettings()
        {
            UpdateAntialisingMode();
            UpdateCastShadow();
            UpdateFSR_Override();
            UpdateFSR_Sharpeness();
            UpdateHDRState();
            UpdateRenderScale();
            UpdateResolution();
            UpdateScreenMode();
            UpdateTargetFramerate();
            UpdateVsyncActive();
            UpdateVsyncDropDown();            
        }

        private void FeedLists()
        {
            resolutions = Screen.resolutions.ToList();

            upscalingModes = Enum.GetValues(typeof(UpscalingFilterSelection)).Cast<UpscalingFilterSelection>().ToList();
            antialisingModes = Enum.GetValues(typeof(MsaaQuality)).Cast<MsaaQuality>().ToList();
            hdr_Bits = Enum.GetValues(typeof(HDRColorBufferPrecision)).Cast<HDRColorBufferPrecision>().ToList();
        }

        #region - Main Game Interaction -
        public void QuitGame() => Application.Quit();
        #endregion

        #region - Resolution Setting System -
        private void FeedResolutions()
        {
            resolutionDrp.ClearOptions();

            List<string> options = new();

            for (int i = 0; i < resolutions.Count; i++)
            {
                string newOption = resolutions[i].width + "x" + resolutions[i].height;

                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) currentResolutionIndex = i;

                options.Add(newOption);
            }

            resolutionDrp.AddOptions(options);

            resolutionDrp.value = currentResolutionIndex;
            resolutionDrp.RefreshShownValue();
            UpdateResolution();
        }
        public void UpdateResolution()
        {
            currentResolutionIndex = resolutionDrp.value;
            Resolution selectedResolution = resolutions[resolutionDrp.value];
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, fullscreenTgl.isOn);

        }
        #endregion

        #region - Antialising Setting System -
        private void FeedAntialisingModes()
        {
            antialisingQualityDrp.ClearOptions();
            List<string> datas = new();

            for (int i = 0; i < antialisingModes.Count; i++)
            {

                string newOption = antialisingModes[i].ToString();
                string newStringData = "";

                for (int y = 0; y < newOption.Length; y++) newStringData += newOption.Substring(y, 1) == "_" ? "" : newOption.Substring(y, 1);

                if (renderingPipeline.msaaSampleCount == (int)antialisingModes[i]) antialisingQualityIndex = i;

                datas.Add(newStringData);
            }

            antialisingQualityDrp.AddOptions(datas);

            antialisingQualityDrp.value = antialisingQualityIndex;
            antialisingQualityDrp.RefreshShownValue();
            UpdateAntialisingMode();
        }
        public void UpdateAntialisingMode()
        {
            if (!antialisingActive.isOn)
            {
                antialisingQualityIndex = 0;
                renderingPipeline.msaaSampleCount = (int)antialisingModes[antialisingQualityIndex];
                return;
            }
            else
            {
                antialisingQualityIndex = (int)antialisingModes[antialisingQualityDrp.value];

                renderingPipeline.msaaSampleCount = (int)antialisingModes[antialisingQualityDrp.value];
            }
        }

        #endregion

        #region - Upscaling Filter Setting System -
        private void FeedUpscalingFilterModes()
        {
            upscalingFilterDrp.ClearOptions();
            List<string> datas = new();

            for (int i = 0; i < upscalingModes.Count; i++)
            {
                if (renderingPipeline.upscalingFilter == upscalingModes[i]) upscalingFilterModeIndex = i;
                datas.Add(upscalingModes[i].ToString());
            }

            upscalingFilterDrp.AddOptions(datas);

            upscalingFilterDrp.value = upscalingFilterModeIndex;
            upscalingFilterDrp.RefreshShownValue();
            UpdateUpscalingFilterMode();
        }
        public void UpdateUpscalingFilterMode()
        {
            if (!overriddeUpscalingMode.isOn)
            {
                upscalingFilterModeIndex = 0;
                renderingPipeline.upscalingFilter = upscalingModes[upscalingFilterModeIndex];
                return;
            }
            else
            {
                upscalingFilterModeIndex = (int)upscalingModes[upscalingFilterDrp.value];

                renderingPipeline.upscalingFilter = upscalingModes[upscalingFilterDrp.value];
            }
        }
        #endregion

        #region - HDR Setting System -
        private void FeedHDR_Bits()
        {
            hdr_BitsDrp.ClearOptions();
            List<string> datas = new();

            for (int i = 0; i < hdr_Bits.Count; i++)
            {

                string newOption = hdr_Bits[i].ToString();
                string newStringData = "";

                for (int y = 0; y < newOption.Length; y++) newStringData += newOption.Substring(y, 1) == "_" ? "" : newOption.Substring(y, 1);

                if (renderingPipeline.hdrColorBufferPrecision == hdr_Bits[i]) hdrBitsIndex = i;

                datas.Add(newStringData);
            }

            hdr_BitsDrp.AddOptions(datas);

            hdr_BitsDrp.value = hdrBitsIndex;
            hdr_BitsDrp.RefreshShownValue();
            UpdateHDR_Bits();
        }
        public void UpdateHDR_Bits()
        {
            hdrBitsIndex = hdr_BitsDrp.value;
            renderingPipeline.hdrColorBufferPrecision = hdr_Bits[hdrBitsIndex];
        }
        public void UpdateHDRState() => renderingPipeline.supportsHDR = hdrMode.isOn;
        #endregion

        #region - FSR Override Settings -
        public void UpdateFSR_Override() => renderingPipeline.fsrOverrideSharpness = overrideFSR_Sharpeness.isOn;
        public void UpdateFSR_Sharpeness()
        {
            if (overrideFSR_Sharpeness.isOn)
            {
                float newFSR_Sharpeness = FSR_Sharpeness.currentValue;
                if (renderingPipeline.fsrSharpness == newFSR_Sharpeness) return;
                renderingPipeline.fsrSharpness = newFSR_Sharpeness;
            }
        }
        #endregion

        #region - Texture Filtering Quality -
        public void UpdateTextureFilteringQuality()
        {
            currentTextureQuality = textureQualityDrp.value;

            if (currentTextureQuality < 0 || currentTextureQuality > 3) return;

            QualitySettings.globalTextureMipmapLimit = currentTextureQuality;
        }
        private void UpdateTextureFilteringDropdown()
        {
            currentTextureQuality = QualitySettings.globalTextureMipmapLimit;
            textureQualityDrp.value = currentTextureQuality;
            textureQualityDrp.RefreshShownValue();
        }
        #endregion

        #region - Shadow Setting -
        public void UpdateCastShadow() => QualitySettings.shadows = castShadow.isOn ? UnityEngine.ShadowQuality.All : UnityEngine.ShadowQuality.Disable;
        #endregion

        #region - General Graphics Settings - 
        public void UpdateScreenMode() => Screen.fullScreen = fullscreenTgl.isOn;  
        public void UpdateRenderScale()
        {
            float newRenderScale = renderScaleField.currentValue;
            if (renderingPipeline.renderScale == newRenderScale) return;
            renderingPipeline.renderScale = newRenderScale;
        }
        public void UpdateTargetFramerate() => Application.targetFrameRate = (int)targetFramerateField.currentValue;
        #endregion

        #region - VSync Settings -
        public void UpdateVSyncSettings()
        {
            QualitySettings.vSyncCount = VSyncDrp.value;
            UpdateVsyncDropDown();
        }
        public void UpdateVsyncDropDown()
        {
            currentVSyncIndex = QualitySettings.vSyncCount;
            VSyncDrp.value = currentVSyncIndex;
            VSyncDrp.RefreshShownValue();
        }
        public void UpdateVsyncActive()
        {
            if (!vsyncTgl.isOn) QualitySettings.vSyncCount = 0;
            UpdateVsyncDropDown();
        }
        #endregion

        public void Load(GameStateData data)
        {
            Debug.Log("Loading Data!");
            VSyncDrp.value = data.settingsData.vsyncCount;
            textureQualityDrp.value = data.settingsData.textureQuality;
            resolutionDrp.value = data.settingsData.resolutionIndex;
            targetFramerateField.currentValue = data.settingsData.targetFramerate;
            targetFramerateField.OnSliderEdit();

            UpdateSettings();
        }
        public void NewGame()
        {
            currentTerrainState = TerrainState.NewGeneration;
            SceneManager.LoadScene(1);
        }
        public void LoadGame()
        {
            currentTerrainState = TerrainState.SaveLoading;
            SceneManager.LoadSceneAsync(1);
            GridWorldGenerator.Instance.StartMapInstance();
        }
    }
}