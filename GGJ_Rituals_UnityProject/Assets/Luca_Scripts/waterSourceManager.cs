using UnityEngine;
using System.Collections;

public class waterSourceManager : MonoBehaviour {
    public float initialVolume = 5000.0f;

    private float _currentVolume;

    void Awake()
    {
        _currentVolume = initialVolume;
    }
    public void takeWater(float quantity)
    {
        initialVolume -= quantity;
    }

    void addWater(float quantity)
    {
        _currentVolume += quantity;
    }
	
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "villager" && col.gameObject.GetComponent<VillagerBrain>().wantToRefurnish_WaterSource)
            col.gameObject.GetComponent<VillagerActions>().refillWater();
            
    }
    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "villager" && col.gameObject.GetComponent<VillagerBrain>().wantToRefurnish_WaterSource)
            col.gameObject.GetComponent<VillagerActions>().refillWater();

    }
}
