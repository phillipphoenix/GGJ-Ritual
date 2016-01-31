using UnityEngine;
using System.Collections;

public class NpcAvoidanceAI : MonoBehaviour {

    public Transform target; // where we're going
    private NavMeshAgent NPCNavMeshAgent; // Unity nav agent
    public float probeRange; // how far the character can "see"
    private bool obstacleAvoid = false; // internal var
    public float turnSpeed = 50f; // how fast to turn

    // create empty game objects and place them appropriately infront, to the left and right of our object
    // This creates a little buffer around the character, and I had some trouble with never raycasting
    // outside the character rigidbody/collider
    public Vector3 fwProbe; // forward probe point
    public Vector3 leftProbe; // left probe point
    public Vector3 rightProbe; // right probe point
    private RaycastHit hit;
    private Transform obstacleInPath; // we found something!  
    private bool startPathing;

    // Use this for initialization
    void Start()
    {
        NPCNavMeshAgent = this.GetComponent<NavMeshAgent>();
        fwProbe = new Vector3(transform.position.x, transform.position.y, transform.position.z + probeRange);
        leftProbe = new Vector3(transform.position.x - probeRange, transform.position.y, transform.position.z);
        rightProbe = new Vector3(transform.position.x + probeRange, transform.position.y, transform.position.z);
          
    }


    void Update()
    {
        UpdateProbes();
        AvoidanceMovement();
    }

    void AvoidanceMovement()
    {
        if (target != null)
        {
            if (!startPathing)
            {
                NPCNavMeshAgent.SetDestination(target.position);
                startPathing = true;
            }
            Vector3 dir = (target.position - transform.position).normalized;
            bool previousCastMissed = true;
            if (previousCastMissed && Physics.Raycast(fwProbe, transform.forward, out hit, probeRange))
            {
                if (obstacleInPath != target.transform)
                {
                    previousCastMissed = false;
                    obstacleAvoid = true;
                    NPCNavMeshAgent.Stop();
                    NPCNavMeshAgent.ResetPath();
                    if (hit.transform != transform)
                    {
                        obstacleInPath = hit.transform;
                        dir += hit.normal * turnSpeed;

                    }
                }
            }
            else if (!obstacleAvoid && previousCastMissed && Physics.Raycast(leftProbe, transform.forward, out hit, probeRange))
            {
                if (obstacleInPath != target.transform)
                {
                    obstacleAvoid = true;
                    NPCNavMeshAgent.Stop();
                    if (hit.transform != transform)
                    {
                        obstacleInPath = hit.transform;
                        previousCastMissed = false;
                        dir += hit.normal * turnSpeed;
                    }
                }
            }
            else if (!obstacleAvoid && previousCastMissed && Physics.Raycast(rightProbe, transform.forward, out hit, probeRange))
            {
                if (obstacleInPath != target.transform)
                {
                    obstacleAvoid = true;
                    NPCNavMeshAgent.Stop();
                    if (hit.transform != transform)
                    {
                        obstacleInPath = hit.transform;
                        dir += hit.normal * turnSpeed;
                    }
                }
            }

            if (obstacleInPath != null)
            {
                Vector3 forward = transform.TransformDirection(Vector3.forward);
                Vector3 toOther = obstacleInPath.position - transform.position;
                if (Vector3.Dot(forward, toOther) < 0)
                {

                    obstacleAvoid = false;
                    obstacleInPath = null;
                    NPCNavMeshAgent.ResetPath();
                    NPCNavMeshAgent.SetDestination(target.position);
                    NPCNavMeshAgent.Resume(); // Unity nav can resume movement control
                }

            }
            //     
            // this is what actually moves the character when under avoidance control
            if (obstacleAvoid)
            {
                Quaternion rot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime);
                transform.position += transform.forward * NPCNavMeshAgent.speed * Time.deltaTime;
            }
        }
       
    }
    void UpdateProbes()
    {
        fwProbe = new Vector3(transform.position.x, transform.position.y, transform.position.z + probeRange);
        leftProbe = new Vector3(transform.position.x - probeRange, transform.position.y, transform.position.z);
        rightProbe = new Vector3(transform.position.x + probeRange, transform.position.y, transform.position.z);
    }

    public void SetNewTarget(GameObject newTarget)
    {
        target.GetComponent<Entrance>().Vacate();
        target = newTarget.transform;
        startPathing = true;
    }
}
