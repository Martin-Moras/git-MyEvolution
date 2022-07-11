using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyCounter : MonoBehaviour
{
    public decimal AllEnergy;
    public decimal AllUsedEnergy;
    public decimal TotalEnergy;
    private string AllEnergyS;
    private string AllUsedEnergyS;
    private string TotalEnergyS;

    void Update()
    {
        //print(AllEnergy);
        //print(AllUsedEnergy);
        AllEnergyS = AllEnergy.ToString();
        AllUsedEnergyS = AllUsedEnergy.ToString();
       
        TotalEnergy = AllEnergy + AllUsedEnergy;
        TotalEnergyS = TotalEnergy.ToString();
        
        AllEnergy = 0;
        AllUsedEnergy = 0;
    }
}
