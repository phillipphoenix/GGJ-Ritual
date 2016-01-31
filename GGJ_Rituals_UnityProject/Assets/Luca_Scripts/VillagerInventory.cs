using UnityEngine;
using System.Collections;

public class VillagerInventory : MonoBehaviour {
    public int[] food; //from 0 to 3
    public int[] drink; //from 0 to 3
    public int[] medicine; //from 0 to 3
    public float money;
    public bool weapon;

    private float _hungerDecayingTime;
    private float _thirstDecayingTime;
    private float _sicknessDecayingTime;
    private storeManagement _storeRef;
	// Use this for initialization
	void Start () {
        VillagerStatsDecaying state =GameObject.FindGameObjectWithTag("GameController").GetComponent<VillagerStatsDecaying>();
        _storeRef = transform.parent.gameObject.transform.parent.gameObject.GetComponent<VillageGenerator>().StoreHouse.GetComponent<storeManagement>();
        _hungerDecayingTime = state.hungerDecayingTime;
        _thirstDecayingTime = state.thirstDecayingTime;
        _sicknessDecayingTime = state.sicknessDecayingTime;
        int nfood = (int)Random.Range(0, 10000) % 4;
        food =new int[3];
        for (int i = 0; i < nfood; i++)
            food[i] = (int)Random.Range(0, 10000000) % (int)_hungerDecayingTime / 2;

        int ndrink = (int)Random.Range(0, 10000) % 4;
        drink = new int[3];
        for (int i = 0; i < ndrink; i++)
        {
            int val= (int)Random.Range(0, 10000000) % (int)_thirstDecayingTime / 2;
            if (val == 0)
                drink[i] = 1;
            else
                drink[i] = val;
        }
           

        int nmedicine = (int)Random.Range(0, 10000) % 4;
        medicine = new int[3];
        for (int i = 0; i < nmedicine; i++)
        {
           
            int val = (int)Random.Range(0, 10000000) % (int)_sicknessDecayingTime / 2;
            if (val == 0)
                medicine[i] = 1;
            else
                medicine[i] = val;
        }
       

        if ((int)Random.Range(0, 10000) % 2 == 0)
            weapon = false;
        else
            weapon = true;
        money = (int)Random.Range(0, 10000000) % 1000;
    }
	
	public int hasFood()
    {
        if (food.Length > 0)
        {
            for (int i = 0; i < food.Length; i++)
                if (food[i] > 0)
                    return i;
                 
        }
        return -1;
    }
    public int hasDrink()
    {
        if (drink.Length > 0)
        {
            for (int i = 0; i < drink.Length; i++)
                if (drink[i] > 0)
                    return i;
               
        }
        return -1;
    }
    public int hasMedicine()
    {
        if (medicine.Length > 0)
        {
            for (int i = 0; i < medicine.Length; i++)
                if (medicine[i] > 0)
                    return i;
                
        }
        return -1;
    }

    public void addFood(float n)
    {
        for(int i=0;i< n&&i<3;i++)
           food[i]= (int)Random.Range(0, 10000000) % (int)_hungerDecayingTime / 2;
    }
    public void addDrink(float n)
    {
        for (int i = 0; i < n && i < 3; i++)
            drink[i]=(int)Random.Range(0, 10000000) % (int)_thirstDecayingTime / 2;
    }
    public void addMedicine(float n)
    {
        for (int i = 0; i < n && i < 3; i++)
            medicine[i]=(int)Random.Range(0, 10000000) % (int)_sicknessDecayingTime / 2;
    }

    public void consumeFood(int index)
    {
        food[index] = 0;
    }
    public void consumeDrink(int index)
    {
        drink[index] = 0;
    }
    public void consumeMedicine(int index)
    {
        medicine[index] = 0;
    }
    public bool outOfMoney()
    {
        if (money < _storeRef.foodPrice && money < _storeRef.medicinePrice)
            return false;
        else
            return true;
    }
}
