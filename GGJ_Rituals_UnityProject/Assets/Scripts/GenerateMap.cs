using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GenerateMap : MonoBehaviour
{
    [Header("Map settings")]
    public Vector2 MapOrigin;
    public Vector2 MapSize;

    // General values.
    [Header("General values")]
    public int Seed;
    public int FbmIterations;

    // Values for generating trees.
    [Header("Tree settings")]
    public float MinDensityForTrees;
    public GameObject TreePrefab;
    public int SkipSpaces;
    public bool UseIterationsForTrees;
    public Vector2 MaxOffset; // Random offset values.

    // Values for placing totems.
    [Header("Totem settings")]
    public int NbOfTotems;
    public GameObject TotemPrefab;
    public float MinTotemDistance;
    public float TotemClearingDistance;
    private List<Transform> totemList;

    // Values for placing villages.
    [Header("Village settings")]
    public int NbOfVillages;
    public GameObject VillagePrefab;
    public float MinVillageDistance;
    public float VillageClearingDistance;
    private List<Transform> villageList; 

    // Values for placing shamans.
    [Header("Shaman settings")]
    public GameObject ShamanPrefab;

    public void Awake()
    {
        Random.seed = Seed;
        GenerateTrees();
        GenerateTotems();
        GenerateVillages();

        GameObject.Instantiate(ShamanPrefab);
    }

    private void GenerateTrees()
    {
        GameObject TreeParent = new GameObject("Trees");
        for (int y = 0; y < MapSize.y; y += SkipSpaces)
        {
            for (int x = 0; x < MapSize.x; x += SkipSpaces)
            {
                float density = FractionBrownianMotion(FbmIterations, x / MapSize.x, y / MapSize.y);
                if (density > MinDensityForTrees)
                {
                    GameObject tree = GameObject.Instantiate(TreePrefab);
                    Vector2 offset = new Vector2(Random.value * MaxOffset.x, Random.value * MaxOffset.y);
                    tree.transform.position = new Vector3(x + MapOrigin.x + offset.x, 0, y + MapOrigin.y + offset.y);
                    tree.transform.parent = TreeParent.transform;
                }
            }
        }
    }

    private void GenerateTotems()
    {
        totemList = new List<Transform>();
        GameObject totemParent = new GameObject("Totems");
        for (int i = 0; i < NbOfTotems; i++)
        {
            var x = Random.value * MapSize.x;
            var y = Random.value * MapSize.y;
            if (DistanceToNearestTransform(new Vector3(x, 0, y), totemList) < MinTotemDistance)
            {
                x = Random.value * MapSize.x;
                y = Random.value * MapSize.y;
                Debug.Log("Totem too close to other totem.");
            }
            GameObject totem = GameObject.Instantiate(TotemPrefab);
            totem.transform.position = new Vector3(x + MapOrigin.x, 0, y + MapOrigin.y);
            totem.transform.parent = totemParent.transform;
            totemList.Add(totem.transform);
            ClearLandscapeObstacles(totem.transform, TotemClearingDistance);
        }
    }

    private void GenerateVillages()
    {
        villageList = new List<Transform>();
        GameObject villageParent = new GameObject("Villages");
        for (int i = 0; i < NbOfVillages; i++)
        {
            var x = Random.value * MapSize.x;
            var y = Random.value * MapSize.y;
            while (DistanceToNearestTransform(new Vector3(x, 0, y), villageList.Concat(totemList).ToList()) < MinVillageDistance)
            {
                x = Random.value * MapSize.x;
                y = Random.value * MapSize.y;
                Debug.Log("Village too close to either other village or to totem.");
            }
            GameObject village = GameObject.Instantiate(VillagePrefab);
            village.transform.position = new Vector3(x + MapOrigin.x, 0, y + MapOrigin.y);
            village.transform.parent = villageParent.transform;
            villageList.Add(village.transform);
            ClearLandscapeObstacles(village.transform, VillageClearingDistance);
        }
    }

    private float DistanceToNearestTransform(Vector3 currentPos, List<Transform> transformList)
    {
        float minDist = Mathf.Infinity;
        foreach (Transform t in transformList)
        {
            float dist = Vector3.Distance(t.position, currentPos);
            if (dist < minDist)
            {
                minDist = dist;
            }
        }
        return minDist;
    }

    private void ClearLandscapeObstacles(Transform objTransform, float distance)
    {
        var transformArray = GameObject.FindGameObjectsWithTag("LandscapeObstacle")
         .Select(go => go.transform)
         .Where(t => Vector3.Distance(t.position, objTransform.position) < TotemClearingDistance)
         .ToArray();
        foreach (var obstacle in transformArray)
        {
            Debug.Log("Clearing landscape obstacle!");
            GameObject.Destroy(obstacle.gameObject);
        }
    }

    private float FractionBrownianMotion(int iterations, float x, float y)
    {
        float value = 0;
        for (int i = 1; i <= iterations; ++i)
        {
            if (UseIterationsForTrees)
            {
                // TODO Instead of "iterations" inside the PerlinNoise function, use "i".
                value += Mathf.PerlinNoise(Seed + x * iterations, Seed + y * iterations) / iterations;
            }
            else
            {
                // TODO Instead of "iterations" inside the PerlinNoise function, use "i".
                value += Mathf.PerlinNoise(Seed + x * i, Seed + y * i) / iterations;
            }
        }
        return value;
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(MapSize.x, 10, MapSize.y));
    }
}
