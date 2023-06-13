using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NekraliusDevelopmentStudio
{
    [CreateAssetMenu(fileName = "New Object Database", menuName = "RST_Voxel/Placeable Objects/Object Database")]
    public class ObjectsDatabaseSO : ScriptableObject
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //CompleteCodeName - (Code Version)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)
        public List<ObjectData> objectsData;
    }

    [Serializable]
    public class ObjectData
    {
        public string Name;
        public int ID;
        public Vector2Int Size;
        public GameObject prefab;
    }
}