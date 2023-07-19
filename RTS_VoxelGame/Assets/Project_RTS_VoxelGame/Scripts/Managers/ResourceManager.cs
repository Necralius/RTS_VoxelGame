using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NekraliusDevelopmentStudio
{
    public class ResourceManager : MonoBehaviour, ILoadableData
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //CompleteCodeName - (Code Version)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        #region - Singleton Pattern -
        public static ResourceManager Instance;
        private void Awake() => Instance = this;
        #endregion

        [HideInInspector] public float woodQuantity = 0;
        [HideInInspector] public float metalQuantity = 0;

        public TextMeshProUGUI woodQuantityDisplay;
        public TextMeshProUGUI metalQuantityDisplay;

        public GameObject addFeedback;
        public GameObject removeFeedback;

        public float WoodQuantity
        {
            get { return this.woodQuantity; }
            set
            {
                if (woodQuantity <= 0) woodQuantity = 0;
                else woodQuantity = value;
            }
        }

        public float MetalQuantity
        {
            get { return this.metalQuantity; }
            set
            {
                if (metalQuantity <= 0) metalQuantity = 0;
                else metalQuantity = value;
            }
        }

        private void Start()
        {
            UpdateUI();
        }

        public void AddQuantity(float quantity, ResourceType type)
        {
            switch (type)
            {
                case ResourceType.RawWood:
                    WoodQuantity += quantity;
                    break;
                case ResourceType.RawMetal:
                    MetalQuantity += quantity;
                    break;
                default:
                    Debug.LogWarning("Not an valid ResourceType!");
                    break;
            }
            UpdateUI();
            AddFeedback();
        }
        public void RemoveQuantity(int quantity, ResourceType type)
        {
            switch (type)
            {
                case ResourceType.RawWood:
                    WoodQuantity -= quantity;
                    break;
                case ResourceType.RawMetal:
                    MetalQuantity -= quantity;
                    break;
                default:
                    Debug.LogWarning("Not an valid resource type!");
                    break;
            }
            UpdateUI();
            RemoveFeedback();
        }
        private void AddFeedback()
        {
            addFeedback.SetActive(true);
        }
        private void RemoveFeedback()
        {
            removeFeedback.SetActive(true);
        }

        public bool CanRemoveQuantity(int quantity, ResourceType type)
        {
            bool result = false;
            switch (type)
            {
                case ResourceType.RawWood:
                    if (WoodQuantity - quantity >= 0) result = true;
                    break;
                case ResourceType.RawMetal:
                    if (MetalQuantity - quantity >= 0) result = true;
                    break;
                default:
                    result = false;
                    break;
            }
            return result;
        } 
        public bool CheckForBuild(List<BuildTable> buildNeeds)
        {
            bool canBuild = false;

            if (buildNeeds.Count <= 0) return true;
            for (int i = 0; i < buildNeeds.Count; i++) canBuild = CanRemoveQuantity(buildNeeds[i].quantity, buildNeeds[i].type);

            if (canBuild)
            {
                for (int i = 0; i < buildNeeds.Count; i++) RemoveQuantity(buildNeeds[i].quantity, buildNeeds[i].type);
                return true;                                                                                                                           
            }
            return false;
        }
        private void UpdateUI()                                     
        {
            woodQuantityDisplay.text = $"X{WoodQuantity}";
            metalQuantityDisplay.text = $"X{MetalQuantity}";
        }

        #region - Data Load -
        public void Load(GameStateData data)
        {
            metalQuantity = data.resourceData.metalQuantity;
            woodQuantity = data.resourceData.woodQuantity;
            Debug.Log("Loading Data!");
        }
        #endregion
    }
}