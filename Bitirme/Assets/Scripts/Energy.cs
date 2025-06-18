using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Energy : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private IntEvent onEnergyChanged;

    [Header("UI")] 
    [SerializeField] private GameObject energyUI;
    [SerializeField] private Image energy;
    
    [SerializeField] private float energyValue = 100;

    private void OnEnable()
    {
        onEnergyChanged.AddListener(UpdateEnergyBar);
    }

    private void OnDisable()
    {
        onEnergyChanged.RemoveListener(UpdateEnergyBar);
    }

    private void UpdateEnergyBar(int value)
    {
        energyValue += value;
        energy.fillAmount = energyValue / 100;
    }
}
