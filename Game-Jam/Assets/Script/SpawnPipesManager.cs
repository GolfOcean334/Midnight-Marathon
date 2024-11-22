using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] pipePrefab;
    private float spawnTime = 2f;
    private float maxHeight = -0.45f;
    private float minHeight = -2.53f;

    public void OnEnable()
    {
        InvokeRepeating("SpawnPipe", 0, spawnTime);
    }

    private void OnDisable()
    {
        CancelInvoke("SpawnPipe");
    }

    private void SpawnPipe()
    {
        float randomHeight = Random.Range(minHeight, maxHeight);
        GameObject pipe = Instantiate(pipePrefab[Random.Range(0, pipePrefab.Length)]);
    }
}
