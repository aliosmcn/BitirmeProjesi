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
    [SerializeField] private GameObject loseUI;
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

    private void Start()
    {
        loseUI.SetActive(false);
    }

    private void UpdateEnergyBar(int value)
    {
        energyValue += value;
        if (energyValue >= 100) energyValue = 100;
        energy.fillAmount = energyValue / 100;
        if (energyValue <= 0f)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            loseUI.SetActive(true);
        }
    }
}
