using UnityEngine;
using System.Collections.Generic;

public class VillagerData : MonoBehaviour {

    List<GameObject> villagers;
    VillageGenerator vData;
	// Use this for initialization
	void Awake ()
    {
        vData = gameObject.transform.parent.GetComponent<VillageGenerator>();
        VillagerSetUp();
	
	}

    void VillagerSetUp()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            villagers.Add(gameObject.transform.GetChild(i).gameObject);
        }

    }


    // Update is called once per frame
    void Update()
    {

    }

    public void AddVillager(GameObject villager)
    {
        villagers.Add(villager);
    }

    public void RemoveVillager(GameObject villager)
    {
        villagers.Remove(villager);
    }
}
