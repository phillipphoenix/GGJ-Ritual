using UnityEngine;
using System.Collections;

public class NpcBaseAI : MonoBehaviour
{
    NavMeshAgent NPCAgentMode;
    public bool reached;
    private bool dead;
    public bool moving;
    public GameObject currentTarget;
    public float sightRange;
    private bool avoidingObstacle;
    public float turnSpeed;

    // Use this for initialization
    void Awake()
    {

        Init();

    }

    // Update is called once per frame
    void Update()
    {
        Idle();
        MoveToDestination(currentTarget);

    }

    #region Init
    void Init()
    {
        NPCAgentMode = gameObject.GetComponent<NavMeshAgent>();
    }
    #endregion
    #region State
    void Idle()
    {
       
        
    }

    void Dead()
    {

    }
    #endregion
    #region Movement
    void MoveToDestination(GameObject target)
    {
        if (!dead)
        {
            Debug.Log(Vector3.Distance(gameObject.transform.position, target.transform.position));
            if (Vector3.Distance(gameObject.transform.position, target.transform.position) <= NPCAgentMode.stoppingDistance)
            {
                if (moving)
                {
                    
                    moving = false;
                    reached = true;
                }
            }
            else
            {
                if (!moving)
                {
                    NPCAgentMode.Resume();
                    NPCAgentMode.SetDestination(target.transform.position);
                    moving = true;
                    reached = false;
                }



            }
        }
    }

    #endregion

    public void GiveDestination(GameObject target)
    {
        if (currentTarget != target)
        {
            currentTarget = target;
        }

    }
    #region Setters Getters
  
    #endregion
}
