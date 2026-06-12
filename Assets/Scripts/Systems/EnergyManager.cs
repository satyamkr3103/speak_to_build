using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager Instance;

    [Header("Energy")]
    public int maxEnergy = 100;
    public int currentEnergy;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentEnergy = maxEnergy;
    }

    public bool CanAfford(int amount)
    {
        return currentEnergy >= amount;
    }

    public bool SpendEnergy(int amount)
    {
        if (!CanAfford(amount))
            return false;

        currentEnergy -= amount;

        Debug.Log("Energy Left: " + currentEnergy);

        return true;
    }

    public void RechargeEnergy(int amount)
    {
        currentEnergy += amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
    }
}