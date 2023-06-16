using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NekraliusDevelopmentStudio
{
    [CreateAssetMenu(fileName = "New Input Database", menuName = "RTS_Voxel/SaveSystem/New Input Database")]
    public class InputDataSO : ScriptableObject
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //InputDataSO - (0.1)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        public CameraInputData inputData = new CameraInputData();

        [Serializable]
        public class CameraInputData
        {
            [Range(0, 10)] public float CameraMoveSpeed = 0.3f;
            [Range(0, 10)] public float CameraMoveSmoothing = 5f;

            [Range(0, 10)] public float CameraRotationSpeed = 0.7f;
            [Range(0, 10)] public float CameraRotationSmoothing = 5f;

            [Range(0, 10)] public float ZoomSpeed = 50f;
            [Range(0, 10)] public float ZoomSmoothing = 5f;
        }
        public enum InputType
        {
            Input_Camera,
            Key_Binding
        }
    }
}