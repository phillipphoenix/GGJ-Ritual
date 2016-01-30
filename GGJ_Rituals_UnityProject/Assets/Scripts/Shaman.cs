using UnityEngine;
using System.Collections;
using GamepadInput;

public class Shaman : MonoBehaviour
{
    [Header("Input")]
    public GamePad.Index ControllerIndex;

    [Header("Sprites")]
    public Sprite IdleSprite;
    public Sprite[] WalkSprites;

    private NavMeshAgent navAgent;
    private SpriteRenderer renderer;

    // Animation settings.
    private bool walking;

    private Vector3 prevWalkToPoint;
    private Vector3 walkToPoint;
    private float passedTime;
    private int animIncrement;

	// Use this for initialization
	void Start ()
	{
	    navAgent = GetComponent<NavMeshAgent>();
	    renderer = GetComponent<SpriteRenderer>();
	    walkToPoint = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    
	        var leftStickValue = GamePad.GetAxis(GamePad.Axis.LeftStick, ControllerIndex);
	        if (leftStickValue.sqrMagnitude > 0)
	        {
	            walkToPoint =  transform.position + new Vector3(leftStickValue.x, 0, leftStickValue.y) * 3;
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
	            renderer.sprite = WalkSprites[Mathf.RoundToInt(animIncrement++%WalkSprites.Length)];
	            passedTime = 0;
	        }
	    }
	    else
	    {
	        renderer.sprite = IdleSprite;
	    }
	}

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(walkToPoint, 2);
    }
}
