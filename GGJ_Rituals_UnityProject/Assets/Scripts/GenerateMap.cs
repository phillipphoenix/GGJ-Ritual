using UnityEngine;
using System.Collections;

public class GenerateMap : MonoBehaviour
{

    public Vector2 MapOrigin;
    public Vector2 MapSize;

    // General values.
    public int Seed;
    public int FractionBrownianMotionIterations;

    // Values for generating trees.
    public float MinDensityForTrees;
    public GameObject TreePrefab;
    public int SkipSpaces;
    public bool UseIterationsForTrees;

    // Random offset values.
    public Vector2 MaxOffset;

    public void Awake()
    {
        Random.seed = Seed;
        GenerateTrees();
    }

    private void GenerateTrees()
    {
        for (int y = 0; y < MapSize.y; y += SkipSpaces)
        {
            for (int x = 0; x < MapSize.x; x += SkipSpaces)
            {
                float density = FractionBrownianMotion(FractionBrownianMotionIterations, x / MapSize.x, y / MapSize.y);
                if (density > MinDensityForTrees)
                {
                    GameObject tree = GameObject.Instantiate(TreePrefab);
                    Vector2 offset = new Vector2(Random.value * MaxOffset.x, Random.value * MaxOffset.y);
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
}
