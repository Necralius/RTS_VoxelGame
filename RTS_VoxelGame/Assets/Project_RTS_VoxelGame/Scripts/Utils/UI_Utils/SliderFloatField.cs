using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

namespace NekraliusDevelopmentStudio
{
    public class SliderFloatField : MonoBehaviour
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //CompleteCodeName - (Code Version)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        private Slider mainSliderComponenet => GetComponent<Slider>();
        private TMP_InputField floatField => GetComponentInChildren<TMP_InputField>();

        [Header("Value")]
        public float minValue;
        public float maxValue;
        public float currentValue;

        [Header("On Value Change Event")]
        public UnityEvent onChangeOrder;

        private void Start()
        {
            mainSliderComponenet.maxValue = maxValue;
            mainSliderComponenet.minValue = minValue;

            OnSliderEdit();

            mainSliderComponenet.onValueChanged.AddListener(delegate { OnSliderEdit(); });
            floatField.onEndEdit.AddListener(delegate { OnFloatFieldEdit(); });
        }

        public void OnFloatFieldEdit()
        {
            float currentValue = (float)Convert.ToDouble(floatField.text);
            
            if (currentValue > maxValue) currentValue = maxValue;
            else if (currentValue < minValue) currentValue = minValue;

            mainSliderComponenet.value = currentValue;
            this.currentValue = currentValue;

            ValueChanged();
        }
        public void OnSliderEdit()
        {
            float currentValue = mainSliderComponenet.value;

            if (currentValue > maxValue) currentValue = maxValue;
            else if (currentValue < minValue) currentValue = minValue;

            floatField.text = string.Format("{0:0.0}", currentValue);
            this.currentValue = currentValue;

            ValueChanged();
        }
        public float GetValue() => currentValue;

        public void ValueChanged()
        {
            onChangeOrder.Invoke();
        }
    }
}