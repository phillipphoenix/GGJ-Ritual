using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GamepadInput;

public class Shaman : MonoBehaviour
{
    [Header("Input")]
    public GamePad.Index ControllerIndex;

    [Header("Sprites")]
    public Sprite IdleSprite;
    public Sprite[] WalkSprites;

    [Header("In ritual mode")]
    public bool InRitualMode;
    private bool prevInRitualMode;

    [Header("RitualList")]
    public List<Ritual> RitualList;
    public List<Totem> TotemList;
    public Totem ActivatableTotem;

    // Game Handler.
    private GameHandler gameHandler;

    // Movement.
    private Vector3 prevWalkToPoint;
    private Vector3 walkToPoint;

    // Animation settings.
    private float passedTime;
    private int animIncrement;

    [Header("Ritual settings")]
    public int RitualSequenceCount;
    public float ButtonTimingDistance;
    public float ButtonTimingAllowedOffset;
    private AudioSource _buttonSoundsSource;
    private AudioSource _cheeringSoundsSource;
    public bool ChipMunkSounds;
    public AudioClip ButtonASound;
    public AudioClip ButtonASoundChipMunk;
    public AudioClip ButtonBSound;
    public AudioClip ButtonBSoundChipMunk;
    public AudioClip ButtonXSound;
    public AudioClip ButtonXSoundChipMunk;
    public AudioClip ButtonYSound;
    public AudioClip ButtonYSoundChipMunk;
    public AudioClip ShortCheer;
    public AudioClip LongCheer;
    private int _curSequenceCount;
    private float _ritualTimer;
    private bool _ritualStarted;
    private int _ritualSelected;
    private int _ritualButtonIndex;
    private bool _ritualButtonSuccess;

    // Component references.
    private NavMeshAgent navAgent;
    private SpriteRenderer renderer;

    public void Awake()
    {
        gameHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameHandler>();
    }

    // Use this for initialization
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        renderer = GetComponent<SpriteRenderer>();
        _buttonSoundsSource = gameObject.AddComponent<AudioSource>();
        _buttonSoundsSource.volume = 0.5f;
        _cheeringSoundsSource = gameObject.AddComponent<AudioSource>();
        _cheeringSoundsSource.volume = 0.5f;
        walkToPoint = transform.position;

        // Find all explorable totems in the world.
        TotemList = GameObject.FindObjectsOfType<Totem>().ToList();

        // Register with the totems.
        foreach (var totem in TotemList)
        {
            totem.PlayerList.Add(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (InRitualMode && !prevInRitualMode)
        {
            StartRitual();
            prevInRitualMode = true;
        }
        if (!InRitualMode && prevInRitualMode)
        {
            StopRitual();
            prevInRitualMode = false;
        }
        if (!InRitualMode)
        {
            HandleMovement();
            HandleTotemActivation();
        }
    }

    private void HandleMovement()
    {
        var leftStickValue = GamePad.GetAxis(GamePad.Axis.LeftStick, ControllerIndex);
        if (leftStickValue.sqrMagnitude > 0)
        {
            walkToPoint = transform.position + new Vector3(leftStickValue.x, 0, leftStickValue.y) * 3;
        }
        if (walkToPoint != prevWalkToPoint)
        {
            navAgent.SetDestination(walkToPoint);
            prevWalkToPoint = walkToPoint;
            if (walkToPoint.x > transform.position.x)
            {
                renderer.flipX = true;
            }
            else
            {
                renderer.flipX = false;
            }
            passedTime += Time.deltaTime;
            if (passedTime > 0.1)
            {
                renderer.sprite = WalkSprites[Mathf.RoundToInt(animIncrement++ % WalkSprites.Length)];
                passedTime = 0;
            }
        }
        else
        {
            renderer.sprite = IdleSprite;
        }
    }

    private void HandleTotemActivation()
    {
        if (ActivatableTotem != null && GamePad.GetButtonDown(GamePad.Button.A, ControllerIndex))
        {
            //Ritual found = RitualList.FirstOrDefault(ritual => ritual.Id == ActivatableTotem.TotemRitual.Id);
            if (RitualList.FirstOrDefault(ritual => ritual.Id == ActivatableTotem.TotemRitual.Id) == null)
            {
                Debug.Log("Found new ritual!");
                RitualList.Add(ActivatableTotem.TotemRitual);
            }
            InRitualMode = true;
        }
    }

    private void StartRitual()
    {
        gameHandler.PlayRitualMusic();
        _ritualSelected = -1;
        _ritualButtonIndex = 0;
        _ritualTimer = 0;
        _curSequenceCount = 0;
        _ritualStarted = false;
        _ritualButtonSuccess = false;
        StartCoroutine(RitualTracker());
        Debug.Log("Ritual Mode Started!");
    }

    private void StopRitual()
    {
        gameHandler.PlaySoundtrackMusic();
        StopAllCoroutines();
        InRitualMode = false;
    }

    private IEnumerator RitualTracker()
    {
        while (true)
        {
            _ritualTimer += Time.deltaTime;

            /*if (Math.Abs(ritualTimer - ButtonTimingDistance) < 0.01f)
            {
                Debug.Log("BEAT!");
            }*/
            
            GamePad.Button? buttonPressed = GetButtonPressed();

            // If button pressed before timing, FAIL.
            if (_ritualTimer < ButtonTimingDistance - ButtonTimingAllowedOffset)
            {
                if (buttonPressed != null)
                {
                    Debug.LogError("Too early, birdy!");
                    StopRitual();
                }
            }
            else
            // If button pressed within timing, CORRECT!
            if (_ritualTimer >= ButtonTimingDistance - ButtonTimingAllowedOffset &&
                _ritualTimer <= ButtonTimingDistance + ButtonTimingAllowedOffset)
            {
                if (buttonPressed != null)
                {
                    Debug.Log("Button: " + _ritualButtonIndex);
                    if (_ritualStarted)
                    {

                        if (CanContinueRitual(buttonPressed.Value, _ritualSelected, _ritualButtonIndex % 4))
                        {
                            if (_ritualButtonIndex % 4 == 3)
                            {
                                _cheeringSoundsSource.clip = ShortCheer;
                                _cheeringSoundsSource.Play();
                                _curSequenceCount++;
                                Debug.Log("Ritual sequence performed count: " + _curSequenceCount);
                            }
                            if (_curSequenceCount == RitualSequenceCount)
                            {
                                _cheeringSoundsSource.clip = LongCheer;
                                _cheeringSoundsSource.Play();
                                Debug.Log("RITUAL FINISHED!");
                                StopRitual();
                                yield return null;
                            }
                            _ritualButtonIndex++;
                            Debug.Log("Continuing on ritual");
                            _ritualButtonSuccess = true;
                        }
                        else
                        {
                            Debug.LogError("Continue fail - Wrong button.");
                            StopRitual();
                        }
                    }
                    else
                    {
                        if (_ritualSelected != -1 && CanContinueRitual(buttonPressed.Value, _ritualSelected, 0))
                        {
                            _ritualStarted = true;
                            _ritualButtonIndex = 1;
                            Debug.Log("Starting new ritual");
                            _ritualButtonSuccess = true;
                        }
                        else
                        if (CanStartRitual(buttonPressed.Value, out _ritualSelected))
                        {
                            _ritualStarted = true;
                            _ritualButtonIndex = 1;
                            Debug.Log("Starting new ritual");
                            _ritualButtonSuccess = true;
                        }
                        else
                        {
                            Debug.LogError("Starging fail - No ritual with pressed button.");
                            StopRitual();
                        }
                    }
                }
            }
            else
            if (_ritualTimer > ButtonTimingDistance + ButtonTimingAllowedOffset)
            {
                // If ritual started and missed the timing, FAIL.
                if (!_ritualButtonSuccess && buttonPressed == null && _ritualStarted)
                {
                    Debug.LogError("Too late, mate!");
                    StopRitual();
                }
                
                // Reset timer.
                _ritualButtonSuccess = false;
                _ritualTimer = ButtonTimingAllowedOffset;
            }
            yield return null;
        }
    }

    private bool CanStartRitual(GamePad.Button button, out int ritualIndex)
    {
        ritualIndex = -1;
        for (int i = 0; i < RitualList.Count; i++)
        {
            if (RitualList[i][0] == button)
            {
                ritualIndex = i;
                return true;
            }
        }
        return false;
    }

    private bool CanContinueRitual(GamePad.Button button, int ritualIndex, int ritualButtonIndex)
    {
        return RitualList[ritualIndex][ritualButtonIndex] == button;
    }

    private GamePad.Button? GetButtonPressed()
    {
        GamePad.Button? buttonPressed = null;
        if (GamePad.GetButtonDown(GamePad.Button.A, ControllerIndex))
        {
            buttonPressed = GamePad.Button.A;
            if (ChipMunkSounds)
            {
                _buttonSoundsSource.clip = ButtonASoundChipMunk;
            }
            else
            {
                _buttonSoundsSource.clip = ButtonASound;
            }
            
            _buttonSoundsSource.Play();
        }
        if (GamePad.GetButtonDown(GamePad.Button.B, ControllerIndex))
        {
            buttonPressed = GamePad.Button.B;
            if (ChipMunkSounds)
            {
                _buttonSoundsSource.clip = ButtonBSoundChipMunk;
            }
            else
            {
                _buttonSoundsSource.clip = ButtonBSound;
            }
            _buttonSoundsSource.Play();
        }
        if (GamePad.GetButtonDown(GamePad.Button.X, ControllerIndex))
        {
            buttonPressed = GamePad.Button.X;
            if (ChipMunkSounds)
            {
                _buttonSoundsSource.clip = ButtonXSoundChipMunk;
            }
            else
            {
                _buttonSoundsSource.clip = ButtonXSound;
            }
            _buttonSoundsSource.Play();
        }
        if (GamePad.GetButtonDown(GamePad.Button.Y, ControllerIndex))
        {
            buttonPressed = GamePad.Button.Y;
            if (ChipMunkSounds)
            {
                _buttonSoundsSource.clip = ButtonYSoundChipMunk;
            }
            else
            {
                _buttonSoundsSource.clip = ButtonYSound;
            }
            _buttonSoundsSource.Play();
        }
        
        return buttonPressed;
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(walkToPoint, 2);
    }
}
