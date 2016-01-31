using UnityEngine;
using System.Collections;

public class VillagerState : MonoBehaviour {
    public float _hunger;
    private float _hungerTime;
    private float _maxHungerTime;
    public float _thirst;
    private float _thirstTime;
    private float _maxThirstTime;
    public float _happiness;
    private float _happinessTime;
    private float _maxHappinessTime;
    public float _sickness;
    private float _sicknessTime;
    private float _maxSicknessTime;
    public float _fear;
    private float _fearTime;
    private float _maxFearTime;
    private float _resetImmunityTimer;
    private float _immunityHungerTimer;
    private float _immunitySicknessTimer;
    private float _immunityThirstTimer;
    private float _immunityFearTimer;
    private float _immunityHappinessTimer;

    public float startingHunger;
    public float startingThirst;
    public float startingHappiness;
    public float startingSickness;
    public float startingFear;

    private VillagerActions _actionsRef;
    private float _livingTime;

    public GameObject wareHouse;
    public GameObject workplace;

   
    void Start()
    {
        _hunger = startingHunger;
        _thirst = startingThirst;
        _happiness = startingHappiness;
        _sickness = startingSickness;
        _fear = startingSickness;
        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
        VillagerStatsDecaying decayingComponent= gameController.GetComponent<VillagerStatsDecaying>();
        _maxHungerTime = decayingComponent.hungerDecayingTime;
        _maxThirstTime = decayingComponent.thirstDecayingTime;
        _maxHappinessTime = decayingComponent.happinessDecayingTime;
        _maxSicknessTime = decayingComponent.sicknessDecayingTime;
        _maxFearTime = decayingComponent.fearDecayingTime;
        _resetImmunityTimer = decayingComponent.immunityTime;
        _actionsRef = GetComponent<VillagerActions>();
        _immunityHungerTimer=0f;
    _immunitySicknessTimer=0f;
   _immunityThirstTimer=0f;
     _immunityFearTimer=0f;
     _immunityHappinessTimer=0f;
}
    public void makeHungerImmune()
    {
        _immunityHungerTimer = _resetImmunityTimer;
    }
    public void makeThirstImmune()
    {
        _immunityThirstTimer = _resetImmunityTimer;
    }
    public void makeSicknessImmune()
    {
        _immunitySicknessTimer = _resetImmunityTimer;
    }
    public void makeFearImmmune()
    {
        _immunityFearTimer = _resetImmunityTimer;
    }
    public void makeHappinessImmune()
    {
        _immunityHappinessTimer = _resetImmunityTimer;
    }
    public float getMaxHungerTime()
    {
        return _maxHungerTime;
    }
    public float getMaxThirstTime()
    {
        return _maxThirstTime;
    }
    public float getMaxHappinessTime()
    {
        return _maxHappinessTime;
    }
    public float getMaxSicknessTime()
    {
        return _maxSicknessTime;
    }
    public float getMaxFearTime()
    {
        return _maxFearTime;
    }
    //Update the timers for sympthoms
    private void updateTimes()
    {
        float timeStep = Time.deltaTime;
        if (_immunityHungerTimer <= 0f)
            _hungerTime += timeStep;
        else
            _immunityHungerTimer -= Time.deltaTime;
        if (_immunityThirstTimer <= 0f)
            _thirstTime += timeStep;
        else
            _immunityThirstTimer -= Time.deltaTime;
        if (_immunityHappinessTimer <= 0f)
            _happinessTime += timeStep;
        else
            _immunityHappinessTimer -= Time.deltaTime;
        if (_immunitySicknessTimer <= 0f)
            _sicknessTime += timeStep;
        else
            _immunitySicknessTimer -= Time.deltaTime;
        if (_immunityFearTimer <= 0f)
            _fearTime += timeStep;
        else
            _immunityFearTimer -= Time.deltaTime;
    }
    //Update the gravity(the more the worse) of the sympthoms
    private void evaluateCurves()
    {
        _hunger = Mathf.Lerp(0, 10, _hungerTime / _maxHungerTime);
        _thirst = Mathf.Lerp(0, 10, _thirstTime / _maxThirstTime);
        _happiness = Mathf.Lerp(0, 10, _happinessTime / _maxHappinessTime);
        _sickness = Mathf.Lerp(0, 10, _sicknessTime / _maxSicknessTime);
        _fear = Mathf.Lerp(0, 10, _fearTime / _maxFearTime);
    }

    void Update()
    {
        updateTimes();
        evaluateCurves();
        
        
    }

    public string toString()
    {
        return "Hunger:" + _hunger + " Thirst:" + _thirst + " Happiness:" + _happiness + " Sickness:" + _sickness + " Fear:" + _fear;
    }

    public void setHungerTime(float h)
    {
        _hungerTime = h;
    }
    public float getHungerTime()
    {
        return _hungerTime;
    }
    public void setThirstTime(float t)
    {
        _thirstTime = t;
    }
    public float getThirstTime()
    {
        return _thirstTime;
    }
    public void setHappinessTime(float h)
    {
        _happinessTime = h;
    }
    public float getHappinessTime()
    {
        return _happinessTime;
    }
    public void setSicknessTime(float s)
    {
        _sicknessTime = s;
    }
    public float getSicknessTime()
    {
        return _sicknessTime;
    }
    public void setFearTime(float f)
    {
        _fearTime = f;
    }
    public float getFearTime()
    {
        return _fearTime;
    }
    public float getHunger()
    {
        return _hunger;
    }
    public float getThirst()
    {
        return _thirst;
    }
    public float getHappiness()
    {
        return _happiness;
    }
    public float getSickness()
    {
        return _sickness;
    }
    public float getFear()
    {
        return _fear;
    }
    
}
