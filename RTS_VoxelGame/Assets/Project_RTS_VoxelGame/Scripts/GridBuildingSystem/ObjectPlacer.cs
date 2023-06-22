using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NekraliusDevelopmentStudio
{
    public class ObjectPlacer : MonoBehaviour
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //ObjectPlacer - (0.3)
        //State: Functional
        //This code represents an Object Placer system that instatiate all the building system structures on the 3D world, also saving this objects references.

        #region - Singleton Pattern -
        public static ObjectPlacer Instance;
        private void Awake() => Instance = this;
        #endregion

        #region - Placed Strutures Data -
        public List<GameObject> placedGameObjects = new List<GameObject>();
        #endregion

        #region - Object Placement -
        public int PlaceObject(GameObject prefab, Vector3 position)
        {
            //This method instatiate the object prefab in the 3D World using an prefab and an position as the main data, also, the method saves this object data on
            //an list of GameObjects.
            GameObject newObject = Instantiate(prefab);
            newObject.transform.position = position;
            placedGameObjects.Add(newObject);
            return placedGameObjects.Count - 1;
        }
        public int PlaceObject(GameObject prefab)
        {
            //This method is an simple override that only adds an object on the list, object or structure that already exists in the world and only need to get added
            //on the structure layer.
            placedGameObjects.Add(prefab);
            return placedGameObjects.Count - 1;
        }
        #endregion

        #region - Object Removing -
        internal void RemoveObjectAt(int gameObjectIndex)
        {
            //This method is an object remove system that receives an GameObject index, and using this, find the object on the list and removes it first from the 3D
            //world, and later removes it from the instatiated structure data.
            if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex] == null) return;
            Destroy(placedGameObjects[gameObjectIndex]);
            placedGameObjects[gameObjectIndex] = null;
        }
        #endregion
    }
}