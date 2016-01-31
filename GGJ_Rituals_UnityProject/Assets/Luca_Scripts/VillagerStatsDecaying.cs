using UnityEngine;
using System.Collections;

public class VillagerStatsDecaying : MonoBehaviour {
    private GameObject[] _listOfVillagers;
	public float hungerDecayingTime;
    public float thirstDecayingTime;
    public float happinessDecayingTime;
    public float sicknessDecayingTime;
    public float fearDecayingTime;
    public float immunityTime;

    void Start () {
        _listOfVillagers = GameObject.FindGameObjectsWithTag("villager");
	}
	
	
}
