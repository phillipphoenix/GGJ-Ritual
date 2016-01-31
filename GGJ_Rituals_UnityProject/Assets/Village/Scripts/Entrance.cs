using UnityEngine;
using System.Collections;

public class Entrance : MonoBehaviour {

    [SerializeField]
    bool occupied;

    public bool isOccupied()
    {
        return occupied;
    }

    public void Occupy()
    {
        occupied = true;
    }

    public void Vacate()
    {
        occupied = false;
    }
}
