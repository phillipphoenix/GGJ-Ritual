using UnityEngine;
using System.Collections;

public class GenerateMap : MonoBehaviour
{

    public Vector2 MapOrigin;
    public Vector2 MapSize;

    // Perlin Noise and placement.
    public int Seed;
    public int FractionBrownianMotionIterations;
    public float MinDensityForTrees;
    public GameObject TreePrefab;
    public int SkipSpaces;

    // Random offset values.
    public Vector2 MaxOffset;

    public void Awake()
    {
        for (int y = 0; y < MapSize.y; y += SkipSpaces)
        {
            for (int x = 0; x < MapSize.x; x += SkipSpaces)
            {
                float density = FractionBrownianMotion(FractionBrownianMotionIterations, x / MapSize.x, y / MapSize.y);
                //Debug.Log("(" + x + "," + y +"), " + density + " > " + MinDensityForTrees);
                if (density > MinDensityForTrees)
                {
                    GameObject tree = GameObject.Instantiate(TreePrefab);
                    Vector2 offset = new Vector2(Random.value * MaxOffset.x, Random.value * MaxOffset.y);
                    Debug.Log(offset);
                    tree.transform.position = new Vector3(x + MapOrigin.x + offset.x, 0, y + MapOrigin.y + offset.y);
                }
            }
        }
    }

    private float FractionBrownianMotion(int iterations, float x, float y)
    {

        float value = 0;
        for (int i = 1; i <= iterations; ++i)
        {
            // TODO Instead of "iterations" inside the PerlinNoise function, use "i".
            value += Mathf.PerlinNoise(Seed + x * iterations, Seed + y * iterations) / iterations;
        }
        return value;
    }
}
