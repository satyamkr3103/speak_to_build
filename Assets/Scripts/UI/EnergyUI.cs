using TMPro;
using UnityEngine;

public class EnergyUI : MonoBehaviour
{
    public TMP_Text energyText;

    private void Update()
    {
        if (EnergyManager.Instance == null)
            return;

        energyText.text =
            "Energy: " +
            EnergyManager.Instance.currentEnergy;
    }
}