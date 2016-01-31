using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GamepadInput;

public class Totem : MonoBehaviour
{
    public Ritual TotemRitual;
    public List<Shaman> PlayerList;
    public float ActivationDistance;

    [Header("Totem pieces")]
    public float StartHeight;
    public float PieceHeight;
    public GameObject BasePrefab;
    public GameObject HeadPrefab;
    public GameObject PieceAPrefab;
    public GameObject PieceBPrefab;
    public GameObject PieceXPrefab;
    public GameObject PieceYPrefab;

	// Use this for initialization
	void Start () {
        PlayerList = new List<Shaman>();
	    TotemRitual = new Ritual();
        TotemRitual.RandomizeSequence();

	    GameObject basePiece = GameObject.Instantiate(BasePrefab);
	    basePiece.transform.position = transform.position;
	    basePiece.transform.parent = transform;

        var y = basePiece.transform.localScale.y * 9;
        float totalHeight = 0;
	    for (int i = TotemRitual.Sequence.Length-1; i >= 0 ; i--)
	    {
	        GameObject piece = null;
	        switch (TotemRitual[TotemRitual.Sequence.Length-1-i])
	        {
                case GamePad.Button.A:
	                piece = GameObject.Instantiate(PieceAPrefab);
	                break;
                case GamePad.Button.B:
                    piece = GameObject.Instantiate(PieceBPrefab);
                    break;
                case GamePad.Button.X:
                    piece = GameObject.Instantiate(PieceXPrefab);
                    break;
                case GamePad.Button.Y:
                    piece = GameObject.Instantiate(PieceYPrefab);
                    break;
	        }
	        if (piece != null)
	        {
	            var height = piece.transform.localScale.y*1.2f;
                piece.transform.position = new Vector3(transform.position.x, y + (i * height), transform.position.z);
                piece.transform.parent = transform;
	            totalHeight = height*4;

	        }
        }

        GameObject headPiece = GameObject.Instantiate(HeadPrefab);
        headPiece.transform.position = new Vector3(transform.position.x, y + totalHeight, transform.position.z);
        headPiece.transform.parent = transform;
    }
	
	// Update is called once per frame
	void Update () {
	    foreach (var shaman in PlayerList)
	    {
	        if (Vector3.Distance(shaman.transform.position, transform.position) < ActivationDistance)
	        {
	            shaman.ActivatableTotem = this;
	            GetComponent<SpriteRenderer>().enabled = true;
	        }
	        else if (shaman.ActivatableTotem == this)
	        {
	            shaman.ActivatableTotem = null;
                GetComponent<SpriteRenderer>().enabled = false;
            }
	    }
	}

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ActivationDistance);
    }
}
