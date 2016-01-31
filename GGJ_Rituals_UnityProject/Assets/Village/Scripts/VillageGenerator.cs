using UnityEngine;
using System.Collections.Generic;

public enum VillageSize
{
    Small,
    Medium,
    Large
}
public class VillageGenerator : MonoBehaviour {

    Vector3 villageCenter;
    public VillageSize size;
    [SerializeField]
    float outerRadius;
    [SerializeField]
    float radiusOffset;
    [SerializeField]
    float innerRadius;
    [SerializeField]
    GameObject housePrefab;
    [SerializeField]
    GameObject wellPrefab;
    [SerializeField]
    GameObject storehousePrefab;
    [SerializeField]
    GameObject villagerPrefab;
    [SerializeField]
    GameObject buildingSpawner;
    [SerializeField]
    GameObject villagerSpawner;
    [SerializeField]
    GameObject cropPrefab;
    [SerializeField]
    GameObject wareHousePrefab;
    public int initialNumberOfHouses;
    public int initialNumberOfVillagers;
    GameObject storeHouse,well,crop,wareHouse;
    List<GameObject> houses;
    List<BuildingEntraces> houseEntrances;
    BuildingEntraces sHouseEntrances, wellEntrances,cropEntrances,wHouseEntrances;
    
    float houseOffset;
	// Use this for initialization
	void Start () {

        Init();
	
	}


    void Init()
    {
        switch (size)
        {
           
            case VillageSize.Medium:
                {
                    outerRadius = outerRadius * 1.5f;
                    radiusOffset = radiusOffset * 1.5f;
                    innerRadius = innerRadius * 1.5f;
                    initialNumberOfHouses = (int)(initialNumberOfHouses * 1.5f);

                    break;
                }

            case VillageSize.Large:
                {
                    outerRadius = outerRadius * 1.5f;
                    radiusOffset = radiusOffset * 1.5f;
                    innerRadius = innerRadius * 1.5f;
                    initialNumberOfHouses = initialNumberOfHouses*2;
                    break;
                }
        }
        villageCenter = gameObject.transform.position;
        houses = new List<GameObject>();
        houseEntrances = new List<BuildingEntraces>();


        for (int i = 0; i < initialNumberOfHouses-1; i++)
        {
            Vector3 hPos = RandomPosInCircle(villageCenter, outerRadius);
            if (houses.Count == 0)
            {
                GameObject house = Instantiate(housePrefab, hPos, Quaternion.identity) as GameObject;
               
                houseOffset = house.GetComponent<Renderer>().bounds.extents.magnitude;
                house.transform.eulerAngles = new Vector3(-90, Random.value * 360, 0);
                houses.Add(house);
                house.transform.SetParent(buildingSpawner.transform);
                house.transform.localScale = gameObject.transform.localScale;   
                house = null;
            }
            else
            {
                bool found=false;
                while (!found)
                {
                    for (int j = 0; j < houses.Count; j++)
                    {
                        if (Vector3.Distance(hPos,houses[j].transform.position) <= houseOffset)
                        {
                            hPos = RandomPosInCircle(villageCenter, outerRadius-Random.value*innerRadius);
                            break;
                        }
                        else if (j == houses.Count - 1)
                        {
                            found = true;
                        }
                    }
                }
                GameObject house = Instantiate(housePrefab, hPos, Quaternion.identity) as GameObject;
               
                house.transform.eulerAngles = new Vector3(-90, Random.value * 360, 0);
                houses.Add(house);
                house.transform.SetParent(buildingSpawner.transform);
                house.transform.localScale = gameObject.transform.localScale;
                house = null;
            }
          
        }

       
        
        Vector3 SPos = RandomPosInCircle(villageCenter, innerRadius);
        storeHouse = Instantiate(storehousePrefab, SPos, Quaternion.identity) as GameObject;
       
        float storehouseOffsetstore = storeHouse.GetComponent<Renderer>().bounds.extents.magnitude;
        Vector3 WPos = RandomPosInCircle(villageCenter, innerRadius+storehouseOffsetstore);
        well = Instantiate(wellPrefab, WPos, Quaternion.identity) as GameObject;

        storeHouse.transform.SetParent(buildingSpawner.transform);
        storeHouse.transform.localScale = gameObject.transform.localScale;
        well.transform.SetParent(buildingSpawner.transform);
        well.transform.localScale = gameObject.transform.localScale;
        float wellOffset = well.GetComponent<Renderer>().bounds.extents.magnitude;
        Vector3 CPos = RandomPosInCircle(villageCenter, outerRadius + storehouseOffsetstore);
        crop = Instantiate(cropPrefab, CPos, Quaternion.identity) as GameObject;
       
        crop.transform.SetParent(buildingSpawner.transform);
        //crop.transform.localScale = gameObject.transform.localScale;
        float cropOffset = crop.GetComponent<Renderer>().bounds.extents.magnitude;
        Vector3 WHousePos = RandomPosInCircle(villageCenter, innerRadius + cropOffset);
        wareHouse = Instantiate(wareHousePrefab, WHousePos, Quaternion.identity) as GameObject;
        
        wareHouse.transform.SetParent(buildingSpawner.transform);
        wareHouse.transform.localScale = gameObject.transform.localScale;
        sHouseEntrances = storeHouse.GetComponent<BuildingEntraces>();
        wellEntrances = well.GetComponent<BuildingEntraces>();
        cropEntrances = crop.GetComponent<BuildingEntraces>();
        wHouseEntrances = wareHouse.GetComponent<BuildingEntraces>();
        foreach (GameObject entrances in houses)
        {
            houseEntrances.Add(entrances.GetComponent<BuildingEntraces>());
        }
        
        Vector3 vpos = RandomPosInCircle(villageCenter, innerRadius / 2);
        GameObject villager = Instantiate(villagerPrefab, vpos, Quaternion.identity) as GameObject;
        villager.transform.SetParent(villagerSpawner.transform);
        // villager.transform.localScale = gameObject.transform.localScale;
        float villagerOffset = villager.GetComponent<Renderer>().bounds.extents.magnitude;
        villager = null;
        for (int i = 0; i < initialNumberOfVillagers-1; i++)
        {
            Vector3 sVpos = RandomPosInCircle(villageCenter, (innerRadius/2) + villagerOffset);
            GameObject sVillager = Instantiate(villagerPrefab, sVpos, Quaternion.identity) as GameObject;
           // sVillager.transform.localScale = gameObject.transform.localScale;
            sVillager.transform.SetParent(villagerSpawner.transform);
            sVillager = null;

        }
    }

	// Update is called once per frame
	void Update () {
	
	}

    #region Random In Circle
    Vector3 RandomPosInCircle(Vector3 center, float radius)
    {
        Vector3 pos;
        float angle = Random.value * 360;
        pos.x = center.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        pos.y = center.y ;
        pos.z = center.z + radius * Mathf.Cos(angle * Mathf.Deg2Rad);


        return pos;
    }
    #endregion
    #region Getter/Setter
    public List<GameObject> Houses
    {
        get
        {
            return houses;
        }
    }
    public GameObject StoreHouse
    {
        get
        {
            return storeHouse;
        }
    }

    public GameObject Well
    {
        get
        {
            return well;
        }
    }

    public GameObject Crop
    {
        get
        {
            return crop;
        }
    }

    public GameObject Warehouse
    {
        get
        {
            return wareHouse;
        }
    }
    #endregion
}

