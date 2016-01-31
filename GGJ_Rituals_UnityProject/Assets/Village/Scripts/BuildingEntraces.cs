using UnityEngine;
using System.Collections.Generic;

public class BuildingEntraces : MonoBehaviour {

    private List<Entrance> entrances;
	// Use this for initialization
	void Awake ()
    {
        entrances = new List<Entrance>();
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            entrances.Add(gameObject.transform.GetChild(i).GetComponent<Entrance>());
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool FreeEntrance(out GameObject freeEntrace)
    {
        freeEntrace = null;
        foreach (Entrance entrance in entrances)
        {
            if (!entrance.isOccupied())
            {
                freeEntrace = entrance.gameObject;
                entrance.Occupy();
                return true;
            }
        }
        return false;
    }
}
