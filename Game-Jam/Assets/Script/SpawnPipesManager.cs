using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] pipePrefab;
    private float spawnTime = 2f;
    private float maxHeight = -0.78f;
    private float minHeight = -2.16f;

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
        pipe.transform.position = new Vector3(pipe.transform.position.x, randomHeight, pipe.transform.position.z);
    }
}
