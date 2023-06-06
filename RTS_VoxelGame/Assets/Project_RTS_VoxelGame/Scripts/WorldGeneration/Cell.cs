using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NekraliusDevelopmentStudio
{
    [Serializable]
    public class Cell
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //CompleteCodeName - (Code Version)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)
        public TerrainType cellType;
        public float heightValue;

        public Cell(float heightValue)
        {
            this.heightValue = heightValue;
            cellType = WorldGenerationUtility.Instance.GetType(heightValue);
        }

        public bool CheckType(TerrainType typeToCheck) => cellType.Equals(typeToCheck);
    }
}