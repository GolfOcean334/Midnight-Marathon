using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class RunnerGameManager : MonoBehaviour
{
    [Header("Time")]
    [SerializeField] private float time;
    [SerializeField] private float interval;
    [SerializeField] private float intervalTime;

    [Header("Obstacle Game Objects")]
    [SerializeField] private GameObject obstaclesParent;
    [SerializeField] private GameObject obstacle;
    [SerializeField] private List<GameObject> obstaclesList;
    [SerializeField] private GameObject obstacleTriggerResource;
    [SerializeField] private GameObject obstacleTrigger;
    
    [Header("Obstacle Spawn Points")]
    [SerializeField] private GameObject obstaclesParentSpawnPoint;
    [SerializeField] private List<GameObject> obstaclesSpawnPoints;
    
    [Header("Lanes")]
    [SerializeField] private GameObject laneParent;
    [SerializeField] private List<GameObject> lanes;
    
    [Header("Player")]
    [SerializeField] private GameObject player;
    
    [Header("Gameplay")]
    [SerializeField] private float obstacleSpeed;
    [SerializeField] private Camera cam;
    
    [Header("Status")] 
    [SerializeField] private bool isGameRunning;
    [SerializeField] private bool isGameOver;
    public int score;
    
    // Start is called before the first frame update
    void Start()
    {
        obstacle = Resources.Load<GameObject>("Runner/Obstacle");
        obstacleTriggerResource = Resources.Load<GameObject>("Runner/Trigger");
        SetObstacleTrigger();
        GetObstaclesSpawnPoints();
        GetLanes();
        SetObstaclesSpawnPoints();
        SetLanes();
        player.GetComponent<Player>().SetLanes(lanes);
        isGameRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameRunning)
        {
            SpawnObstacle();
            DecreaseTime();
            EndOfGame();
        }
    }

    private void FixedUpdate()
    {
        if (isGameRunning)
        {
            MoveObstacles();
        }
    }

    // Get the obstacles spawn points
    private void GetObstaclesSpawnPoints()
    {
        foreach (Transform child in obstaclesParentSpawnPoint.transform)
        {
            obstaclesSpawnPoints.Add(child.gameObject);
        }
    }
    
    // Get the lanes
    private void GetLanes()
    {
        foreach (Transform child in laneParent.transform)
        {
            lanes.Add(child.gameObject);
        }
    }
    
    // Set the obstacles spawn points position
    private void SetObstaclesSpawnPoints()
    {
        float screenWidth = Screen.width;
        float spacing = screenWidth / obstaclesSpawnPoints.Count;

        for (int i = 0; i < obstaclesSpawnPoints.Count; i++)
        {
            float xPos = (i + 1) * spacing - spacing / 2;
            Vector3 worldPosition = cam.ScreenToWorldPoint(new Vector3(xPos, 0, cam.nearClipPlane));
            obstaclesSpawnPoints[i].transform.position = new Vector3(worldPosition.x, obstaclesSpawnPoints[i].transform.position.y, obstaclesSpawnPoints[i].transform.position.z);
        }
    }

    // Set the lanes position
    private void SetLanes()
    {
        float screenWidth = Screen.width;
        float spacing = screenWidth / lanes.Count;

        for (int i = 0; i < lanes.Count; i++)
        {
            float xPos = (i + 1) * spacing - spacing / 2;
            Vector3 worldPosition = cam.ScreenToWorldPoint(new Vector3(xPos, 0, cam.nearClipPlane));
            lanes[i].transform.position = new Vector3(worldPosition.x, lanes[i].transform.position.y, lanes[i].transform.position.z);
        }
    }

    private void SetObstacleTrigger()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height; // get the screen aspect ratio
        float cameraHeight = cam.orthographicSize * 2; // get the camera height
        float cameraWidth = cameraHeight * screenAspect; // get the camera width
        
        obstacleTrigger = Instantiate(obstacleTriggerResource, new Vector3(0, 0 - cameraHeight / 2 - obstacleTriggerResource.transform.localScale.y, 0), Quaternion.identity);
        obstacleTrigger.transform.localScale = new Vector3(cameraWidth, obstacleTriggerResource.transform.localScale.y, obstacleTriggerResource.transform.localScale.z);
    }
    
    // Spawn the obstacles
    private void SpawnObstacle()
    {
        if (intervalTime <= 0)
        {
            int numberOfObstacles = obstaclesSpawnPoints.Count - 1;
            List<int> usedSpawnPoints = new List<int>();

            for (int i = 0; i < numberOfObstacles; i++)
            {
                int randomObstacleSpawnPoint;
                do
                {
                    randomObstacleSpawnPoint = Random.Range(0, obstaclesSpawnPoints.Count);
                } while (usedSpawnPoints.Contains(randomObstacleSpawnPoint));

                usedSpawnPoints.Add(randomObstacleSpawnPoint);

                Vector3 spawnPosition = new Vector3(obstaclesSpawnPoints[randomObstacleSpawnPoint].transform.position.x, obstaclesSpawnPoints[randomObstacleSpawnPoint].transform.position.y, obstaclesSpawnPoints[randomObstacleSpawnPoint].transform.position.z);
                GameObject obstacleObject = Instantiate(obstacle, spawnPosition, Quaternion.identity, obstaclesParent.transform);
                obstacleObject.GetComponent<Obstacle>().trigger = obstacleTrigger;
                obstaclesList.Add(obstacleObject);
            }

            intervalTime = interval;
        }
        else
        {
            intervalTime -= Time.deltaTime;
        }
    }
    
    private void MoveObstacles()
    {
        for (int i = 0; i < obstaclesList.Count; i++)
        {
            if (obstaclesList[i].GetComponent<Obstacle>().shouldBeDestroyed)
            {
                Destroy(obstaclesList[i]);
                obstaclesList.RemoveAt(i);
                score += 100;
                continue;
            }
            Rigidbody2D obsRb = obstaclesList[i].GetComponent<Rigidbody2D>();
            obsRb.velocity = new Vector3(0, -obstacleSpeed, 0);
        }
    }
    
    // Decrease the time
    private void DecreaseTime()
    {
        if (time <= 0) return;
        time -= Time.deltaTime;
    }

    // End of the game
    private void EndOfGame()
    {
        if (time <= 0f || !player.GetComponent<Player>().isAlive) // Si le temps est écoulé ou si le joueur est mort
        {
            isGameRunning = false;

            // Appeler la fonction de réinitialisation du jeu
            ResetGame();

            // Passer au prochain mini-jeu ou terminer
            FindObjectOfType<ChangeMinigame>().OnGameOver();
        }
    }


    public void ResetGame()
    {
        // Réinitialiser le score et le temps
        score = 0;
        time = 60f; // ou la valeur initiale du temps, par exemple 60 secondes

        // Réinitialiser l'état du jeu
        isGameRunning = true;
        isGameOver = false;

        // Réinitialiser la position du joueur (si nécessaire, pour un jeu de course cela peut inclure la remise à zéro de la position)
        player.transform.position = Vector3.zero; // ou la position initiale de votre joueur

        // Réinitialiser tous les obstacles existants
        foreach (var obstacle in obstaclesList)
        {
            Destroy(obstacle);
        }
        obstaclesList.Clear();

        // Réinitialiser l'intervalle de spawn des obstacles
        intervalTime = 0;

        // Recréer les obstacles à partir de leurs points de spawn
        SpawnObstacle();
    }

}
