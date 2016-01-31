using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class storeManagement : MonoBehaviour {
    public float timeForInteraction=1f;


    public float foodPrice=100f;
    public float medicinePrice = 200f;
	// Use this for initialization
	void Awake () {
      
       
	}
	
	// Update is called once per frame
	void Update () {
      
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag=="villager"&&col.gameObject.GetComponent<VillagerBrain>().wantToRefurnish_Store)
        col.gameObject.GetComponent<VillagerActions>().refurnish();
    }
    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "villager" && col.gameObject.GetComponent<VillagerBrain>().wantToRefurnish_Store)
            col.gameObject.GetComponent<VillagerActions>().refurnish();
    }

}

