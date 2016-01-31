using UnityEngine;
using System.Collections;
[RequireComponent(typeof(VillagerState))]
[RequireComponent(typeof(VillagerInventory))]
public class VillagerActions : MonoBehaviour {
    private VillagerState _villagerStateRef;
    private VillagerInventory _villagerInventoryRef;
    private VillagerBrain _brainRef;
    private GameObject _storeRef;
    private GameObject _waterSourceRef;
    private NavMeshAgent _navMeshComp;
    private float _maxHunger;
    private float _maxThirst;
    private float _maxSickness;
    private float _maxHappiness;
    private float _maxFear;

    private float _gatheringResourceTimers;
    private float _storingResourcesTimers;
    void Start()
    {
        _villagerStateRef = GetComponent<VillagerState>();
        _villagerInventoryRef = GetComponent<VillagerInventory>();
        _brainRef = GetComponent<VillagerBrain>();
        _storeRef = transform.parent.gameObject.transform.parent.gameObject.GetComponent<VillageGenerator>().StoreHouse;
        _waterSourceRef = transform.parent.gameObject.transform.parent.gameObject.GetComponent<VillageGenerator>().Well;
        _navMeshComp = GetComponent<NavMeshAgent>();
        _maxHunger = _villagerStateRef.getMaxHungerTime();
        _maxThirst = _villagerStateRef.getMaxThirstTime();
        _maxSickness = _villagerStateRef.getMaxSicknessTime();
        _maxFear = _villagerStateRef.getMaxFearTime();
    }
    //Lows the hunger by x seconds
    public bool eat()
    {
        int foodI = _villagerInventoryRef.hasFood();
        if (foodI > -1)
        {
            float currentHunger = _villagerStateRef.getHungerTime();
            float hungerSeconds = currentHunger - _villagerInventoryRef.food[foodI];
            if (hungerSeconds < 0)
                hungerSeconds = 0;
            _villagerStateRef.setHungerTime(hungerSeconds);
            _villagerStateRef.makeHungerImmune();
            _villagerInventoryRef.consumeFood(foodI);
            return true;
        }
        return false;
    }
    //Increases the hunger by x seconds
    public void sufferFamine(float seconds)
    {
        float currentHunger = _villagerStateRef.getHungerTime();
        float hungerSeconds = currentHunger + seconds;
        if (hungerSeconds > _maxHunger)
            hungerSeconds = _maxHunger;
        _villagerStateRef.setHungerTime(hungerSeconds);
    }
    //Lows the thirst by x seconds
    public bool drink()
    {
        int drinkI = _villagerInventoryRef.hasDrink();
        if (drinkI > -1)
        {
            float currentThirst = _villagerStateRef.getThirstTime();
            float thirstSeconds = currentThirst - _villagerInventoryRef.drink[drinkI];
            if (thirstSeconds < 0)
                thirstSeconds = 0;
            _villagerStateRef.setThirstTime(thirstSeconds);
            _villagerStateRef.makeThirstImmune();
            _villagerInventoryRef.consumeDrink(drinkI);
            return true;
        }
        return false;
    }
    //Increases the thirst by x seconds
    public void thirsty(float seconds)
    {
        float currentThirst = _villagerStateRef.getThirstTime();
        float thirstSeconds = currentThirst - seconds;
        if (thirstSeconds > _maxThirst)
            thirstSeconds = _maxThirst;
        _villagerStateRef.setThirstTime(thirstSeconds);
    }
    //Lows the sickness by x seconds
    public bool takeDrug()
    {
        int medicineI = _villagerInventoryRef.hasMedicine();
        if (medicineI > -1)
        {
            float currentSickness = _villagerStateRef.getSicknessTime();
            float sicknessSeconds = currentSickness - _villagerInventoryRef.medicine[medicineI];
            if (sicknessSeconds < 0)
                sicknessSeconds = 0;
            _villagerStateRef.setSicknessTime(sicknessSeconds);
            _villagerStateRef.makeSicknessImmune();
            _villagerInventoryRef.consumeMedicine(medicineI);
            return true;
        }
        return false;
    }
    //Increases the sickness by x seconds
    public void lowImmunitariyDef(float seconds)
    {
        float currentSickness = _villagerStateRef.getSicknessTime();
        float sicknessSeconds = currentSickness - seconds;
        if (sicknessSeconds > _maxSickness)
            sicknessSeconds = _maxSickness;
        _villagerStateRef.setThirstTime(sicknessSeconds);
    }
    //Lows the fear by x seconds
    public void calmDown(float seconds)
    {
        float currentFear = _villagerStateRef.getFearTime();
        float fearSeconds = currentFear - seconds;
        if (fearSeconds < 0)
            fearSeconds = 0;
        _villagerStateRef.setFearTime(fearSeconds);
    }
    //Increases the fear by x seconds
    public void scare(float seconds)
    {
        float currentFear = _villagerStateRef.getFearTime();
        float fearSeconds = currentFear - seconds;
        if (fearSeconds > _maxFear)
            fearSeconds = _maxFear;
        _villagerStateRef.setFearTime(fearSeconds);
    }
    //Increases the happiness by x seconds
    public void haveFun(float seconds)
    {
        float currentHappiness = _villagerStateRef.getHappinessTime();
        float happinessSeconds = currentHappiness - seconds;
        if (happinessSeconds < 0)
            happinessSeconds = 0;
        _villagerStateRef.setHappinessTime(happinessSeconds);
    }
    //Lows the happiness by x seconds
    public void annoy(float seconds)
    {
        float currentHappiness = _villagerStateRef.getHappinessTime();
        float happinessSeconds = currentHappiness - seconds;
        if (happinessSeconds > _maxHappiness)
            happinessSeconds = _maxHappiness;
        _villagerStateRef.setHappinessTime(happinessSeconds);
    }
    //Refurnish from the store(either food or medicines)
    public void refurnish()
    {
        float currentMoney = _villagerInventoryRef.money;
            
            int itemNo = (int)Random.Range(0, 10000) % 4;
            if (_villagerStateRef.getHunger() > _villagerStateRef.getSickness())
        {
            float money = _villagerInventoryRef.money;
            money -= _storeRef.GetComponent<storeManagement>().foodPrice * itemNo;
            if (money >= 0)
            {
                _villagerInventoryRef.money = money;
                _villagerInventoryRef.addFood(itemNo);
                _brainRef.wantToRefurnish_Store = false;
                _brainRef.setState(executionState.Free);
                _brainRef.currentWorkingState = workingState.standby;
            }
            else { _brainRef.setState(executionState.Danger); }
            
        }

        else
        {
            float money = _villagerInventoryRef.money;
            money -= _storeRef.GetComponent<storeManagement>().medicinePrice * itemNo;
            if (money >= 0)
            {
                _villagerInventoryRef.money = money;
                _villagerInventoryRef.addMedicine(itemNo);
                _brainRef.wantToRefurnish_Store = false;
                _brainRef.setState(executionState.Free);
                _brainRef.currentWorkingState = workingState.standby;
            }else
            { _brainRef.setState(executionState.Danger); }
        }
                
        
    }
    public void refillWater()
    {
        int itemNo = (int)Random.Range(0, 10000) % 4;
        _villagerInventoryRef.addDrink(itemNo);
        for (int i = 0; i < _villagerInventoryRef.drink.Length; i++)
            _waterSourceRef.GetComponent<waterSourceManager>().takeWater(_villagerInventoryRef.drink[i]);
        _brainRef.wantToRefurnish_WaterSource = false;
        _brainRef.setState(executionState.Free);
        _brainRef.currentWorkingState = workingState.standby;
    }
    //Search for some food
    public void searchForFood()
    {
        _brainRef.setState(executionState.ExecutingAction);
        _brainRef.wantToRefurnish_Store = true;
        _navMeshComp.destination = _storeRef.GetComponent<Transform>().position;
    }
    //Search for some drink
    public void searchForDrink()
    {
        _brainRef.setState(executionState.ExecutingAction);
        _brainRef.wantToRefurnish_WaterSource = true;
        _navMeshComp.destination = _waterSourceRef.GetComponent<Transform>().position;
    }
    //Search for some medicine
    public void searchForMedicine()
    {
        _brainRef.setState(executionState.ExecutingAction);
        _brainRef.wantToRefurnish_Store = true;
        _navMeshComp.destination = _storeRef.GetComponent<Transform>().position;
    }
   
    public void psycopathic()
    {
        Debug.Log("I am psycho");
    }
    public void goToWorkplace(Vector3 position)
    {
        _brainRef.currentWorkingState = workingState.goingToWorkplace;
        _navMeshComp.destination = position;
    }
    public void checkIfArrivedToWorkplace()
    {
        float dist = _navMeshComp.remainingDistance;
        if (  _navMeshComp.remainingDistance < 5f)
        {
            _brainRef.currentWorkingState = workingState.gatheringResources;
            _gatheringResourceTimers = _brainRef.gatheringTime;
        }
    }

    public void gatherResourcesAndGoToWarehouse(Vector3 position)
    {
        _gatheringResourceTimers -= Time.deltaTime;
        if(_gatheringResourceTimers<=0)
        
            _brainRef.currentWorkingState = workingState.goingToWarehouse;
        _navMeshComp.destination = position;
    }
    public void checkIfArrivedToWarehouse()
    {
        float dist = _navMeshComp.remainingDistance;
        if (   _navMeshComp.remainingDistance <5f)
        {
            _brainRef.currentWorkingState = workingState.leavingResources;
            _storingResourcesTimers = _brainRef.storingTime;
        }
    }
    public void leaveResourcesAndGoStandby()
    {
        _storingResourcesTimers -= Time.deltaTime;
        if(_storingResourcesTimers<=0)
            _brainRef.currentWorkingState = workingState.standby;
    }
    public void die()
    {
        Destroy(gameObject);
    }
}
