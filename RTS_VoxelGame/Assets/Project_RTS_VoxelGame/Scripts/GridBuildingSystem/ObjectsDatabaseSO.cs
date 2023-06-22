using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NekraliusDevelopmentStudio
{
    [CreateAssetMenu(fileName = "New Object Database", menuName = "RTS_Voxel/Placeable Objects/Object Database")]
    public class ObjectsDatabaseSO : ScriptableObject
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //ObjectsDatabaseSO - (0.2)
        //Code State: Functional
        //This code represents all the structure types that can be builded on the building system, also this class is using the scriptable object inheritance, 
        //whitch means that this that will be always saved on the game assets.

        public List<ObjectData> objectsData;
    }

    [Serializable]
    public class ObjectData
    {
        //This class represents an structure object data type that holds some structure data like, structure ID, Bidimensional size, structure 3D Prefab and structure
        //name that exists only for indentification purpose.
        public string Name;
        public int ID;
        public Vector2Int Size;
        public GameObject prefab;
    }
}