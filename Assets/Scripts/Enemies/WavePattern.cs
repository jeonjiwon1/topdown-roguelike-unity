using UnityEngine;

[System.Serializable]
public class SpawnGroup
{
    public GameObject enemyPrefab;
    public int count = 5;

    public SpawnMode spawnMode = SpawnMode.Sequential;

    public float spawnInterval = 1f;
}

public enum SpawnMode
{
    Sequential,
    Burst
}

[System.Serializable]
public class WavePattern
{
    public string waveName;

    public SpawnGroup[] spawnGroups;
}