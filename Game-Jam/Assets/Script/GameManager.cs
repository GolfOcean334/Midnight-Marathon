using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Plane")]
    [SerializeField] GameObject planePrefab;
    [SerializeField] Vector3 planeSpawnPosition;
    
    [Header("Background")]
    [SerializeField] List<GameObject> envs;
    [SerializeField] List<GameObject> backgrounds;
    [SerializeField] GameObject envParent;
    [SerializeField] GameObject background;
    [SerializeField] GameObject roof;
    [SerializeField] GameObject ground;
    
    [Header("Values")]
    [SerializeField] float speed = 2f;
    
    [Header("Score")]
    public static int score = 0;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ground = Resources.Load<GameObject>("Flappy Bird/Ground");
        roof = Resources.Load<GameObject>("Flappy Bird/Roof");
        background = Resources.Load<GameObject>("Flappy Bird/background");
        InitBackground();
        UpdateScoreUI();
        
        Camera mainCamera = Camera.main;
        Vector3 screenMiddleBottomHalf = new Vector3(mainCamera.pixelWidth / 2f, mainCamera.pixelHeight / 4f, mainCamera.nearClipPlane);
        Vector3 worldMiddleBottomHalf = mainCamera.ScreenToWorldPoint(screenMiddleBottomHalf);
        worldMiddleBottomHalf.z = 0; // Ensure the z position is correct for your game

        GameObject obj = Instantiate(planePrefab, worldMiddleBottomHalf, Quaternion.identity);
    }
    
    private void InitBackground()
    {
        Camera mainCamera = Camera.main;
        Vector3 screenCenter = new Vector3(mainCamera.pixelWidth / 2f, mainCamera.pixelHeight / 2f, 0);
        Vector3 worldCenter = mainCamera.ScreenToWorldPoint(screenCenter);
        worldCenter.z = 0; // Assurez-vous que la position z est correcte pour votre jeu
        GameObject roofObj = Instantiate(roof, worldCenter, Quaternion.identity, envParent.transform);
        float screenWidthInWorldUnits = mainCamera.orthographicSize * 2 * mainCamera.aspect;
        roofObj.transform.localScale = new Vector3(screenWidthInWorldUnits, 0.5f, 1);
        
        Vector3 screenBottom = new Vector3(mainCamera.pixelWidth / 2f, 0, mainCamera.nearClipPlane);
        Vector3 worldBottom = mainCamera.ScreenToWorldPoint(screenBottom);
        worldBottom.z = 0; // Ensure the z position is correct for your game
        worldBottom.y += ground.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        GameObject groundObj = Instantiate(ground, worldBottom, Quaternion.identity, envParent.transform);

        Vector3 screenMiddleBottomHalf = new Vector3(mainCamera.pixelWidth / 2f, mainCamera.pixelHeight / 4f, mainCamera.nearClipPlane);
        Vector3 worldMiddleBottomHalf = mainCamera.ScreenToWorldPoint(screenMiddleBottomHalf);
        worldMiddleBottomHalf.z = mainCamera.nearClipPlane; // Ensure the z position is correct for your game

        GameObject bg1 = Instantiate(background, worldMiddleBottomHalf, Quaternion.identity);
        GameObject bg2 = Instantiate(background, new Vector3(bg1.GetComponent<SpriteRenderer>().bounds.size.x, worldMiddleBottomHalf.y, worldMiddleBottomHalf.z), Quaternion.identity);
        backgrounds.Add(bg1);
        backgrounds.Add(bg2);
    }
    
    private void AnimateBackground()
    {
        foreach (var bg in backgrounds)
        {
            bg.transform.position += Vector3.left * (speed * Time.deltaTime);
            if (bg.transform.position.x <= -bg.GetComponent<SpriteRenderer>().bounds.size.x)
            {
                bg.transform.position = new Vector3(bg.GetComponent<SpriteRenderer>().bounds.size.x, bg.transform.position.y, bg.transform.position.z);
            }
        }
    }

    private void Update()
    {
        AnimateBackground();
    }

    public void IncrementScore()
    {
        score++;  
        UpdateScoreUI();  
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score : " + score;
    }
}