using UnityEngine;
using System.Collections;
[RequireComponent(typeof(VillagerActions))]
[RequireComponent(typeof(VillagerState))]

public class VillagerBrain : MonoBehaviour
{

    private VillagerState _state;
    private VillagerActions _actions;
    private VillagerInventory _inventory;
    private VillagerStatsDecaying _globals;
    private storeManagement _storeRef;
    public executionState currentState;

    public bool wantToRefurnish_Store = false;
    public bool wantToRefurnish_WaterSource = false;

    public float gatheringTime = 2f;
    public float storingTime = 2f;

    private Vector3 _workplace;
    private Vector3 _warehouse;
    public workingState currentWorkingState;
    private float _dangerTimer;
    private float _dangerTime=10f;

    private float passedTime;
    private SpriteRenderer renderer;
    private int animIncrement;
    public Sprite[] WalkSprites;

    // Use this for initialization
    public void setState(executionState state)
    {
        currentState = state;
    }
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();


        _state = GetComponent<VillagerState>();
        _actions = GetComponent<VillagerActions>();
        _inventory = GetComponent<VillagerInventory>();
        _storeRef =  transform.parent.gameObject.transform.parent.gameObject.GetComponent<VillageGenerator>().StoreHouse.GetComponent<storeManagement>();
        currentState = executionState.Free;
        if (_state.workplace == null)
            _state.workplace = transform.parent.gameObject.transform.parent.gameObject.GetComponent<VillageGenerator>().Crop;
        _workplace = _state.workplace.transform.position;
        if (_state.wareHouse == null)
            _state.wareHouse = transform.parent.gameObject.transform.parent.gameObject.GetComponent<VillageGenerator>().Warehouse;
        _warehouse = _state.wareHouse.transform.position;
        currentWorkingState = workingState.standby;
        _dangerTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float hungerVal = _state.getHunger();
        float thirstVal = _state.getThirst();
        float sicknessVal = _state.getSickness();
        float happinessVal = _state.getHappiness();
        float fearVal = _state.getFear();
        
            if (hungerVal == 10 || thirstVal == 10 || sicknessVal == 10 || happinessVal == 10 || fearVal == 10)
                _dangerTimer = _dangerTime;
            else
                _dangerTimer = 0;
        
        if(_dangerTimer!=0) {
            _dangerTimer -= Time.deltaTime;
            if (_dangerTimer <= 0)
                currentState = executionState.Danger;
        }

        // Animate sprite.
        if (currentState == executionState.ExecutingAction || currentWorkingState == workingState.goingToWarehouse ||
            currentWorkingState == workingState.goingToWorkplace)
        {
            passedTime += Time.deltaTime;
            if (passedTime > 0.1)
            {
                renderer.sprite = WalkSprites[Mathf.RoundToInt(animIncrement++%WalkSprites.Length)];
                passedTime = 0;
            }
        }
        else
        {
            passedTime = 0;
            animIncrement = 0;
            renderer.sprite = WalkSprites[0];
        }

        if (currentState == executionState.Free)
        {


            if (thirstVal >= 5)
            {
                if (!_actions.drink())
                    _actions.searchForDrink();
            }
            else {
                if (hungerVal >= 5)
                {
                    if (!_actions.eat())
                        _actions.searchForFood();
                }
                else
                    if (sicknessVal >= 5) {
                    if (!_actions.takeDrug())
                        _actions.searchForMedicine();
                }
                else { 
                    
                        switch (currentWorkingState)
                        {
                            case workingState.standby:
                                _actions.goToWorkplace(_workplace);
                                break;
                            case workingState.goingToWorkplace:
                                _actions.checkIfArrivedToWorkplace();
                                break;
                            case workingState.goingToWarehouse:
                                _actions.checkIfArrivedToWarehouse();
                                break;
                            case workingState.gatheringResources:
                                _actions.gatherResourcesAndGoToWarehouse(_warehouse);
                                break;
                            case workingState.leavingResources:
                                _actions.leaveResourcesAndGoStandby();
                                break;
                        }
                    }

            }
        }
        else
            if (currentState == executionState.Danger)
            _actions.die();




    }
}
public enum executionState
{
    ExecutingAction,
    Free,
    Danger
}
public enum workingState
{
    standby,
    goingToWorkplace,
    gatheringResources,
    goingToWarehouse,
    leavingResources
}