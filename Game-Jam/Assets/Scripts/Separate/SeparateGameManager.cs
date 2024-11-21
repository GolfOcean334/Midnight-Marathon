using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class SeparateGameManager : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference holdAction;
    [SerializeField] private InputActionReference positionAction;
    [SerializeField] private Vector2 inputPosition;

    [Header("Game Objects")]
    [SerializeField] private List<GameObject> walls;
    [SerializeField] private List<GameObject> objects;
    [SerializeField] private GameObject objectSpawnPoint;
    [SerializeField] private Camera cam;
    
    [Header("Time")]
    [SerializeField] private float time;
    [SerializeField] private float timeToLastSpawn;
    [SerializeField] private float interval;
    [SerializeField] private float intervalTime;
    
    public enum ElementType
    {
        Null,
        Left,
        Right
    }
    
    private void OnEnable()
    {
        holdAction.action.Enable();
        positionAction.action.Enable();
    }
    
    private void OnDisable()
    {
        holdAction.action.Disable();
        positionAction.action.Disable();
    }
    
    private void Awake()
    {
        holdAction.action.Enable();
        positionAction.action.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        FitBorderToScreen();
        objects = Resources.LoadAll<GameObject>("Separate").ToList();
        InstantiateObject();
    }

    // Update is called once per frame
    void Update()
    {
        InstantiateObject();
        DecreaseTime();
        EndOfGame();
        GetInput();
        if (holdAction.action.ReadValue<float>() > 0f)
        {
            RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(inputPosition), Vector2.zero);
            if (hit.collider != null && hit.collider.GetComponent<Object>() != null)
            {
                hit.transform.position = cam.ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y, cam.nearClipPlane));
            }
        }
    }

    private void GetInput()
    {
        inputPosition = positionAction.action.ReadValue<Vector2>();
    }
    
    private void DecreaseTime()
    {
        if (time <= 0) return;
        time -= Time.deltaTime;
    }
    
    private void EndOfGame()
    {
        if (time <= 0)
        {
            
        }
    }

    private void FitBorderToScreen()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = cam.orthographicSize * 2;
        float cameraWidth = cameraHeight * screenAspect;

        bool isWallLeft = true;
        
        foreach (var wall in walls)
        {
            if (isWallLeft)
            {
                wall.transform.position = new Vector3(-cameraWidth / 2, 0, 0);
                isWallLeft = false;
            }
            else
            {
                wall.transform.position = new Vector3(cameraWidth / 2, 0, 0);
            }
        }
    }
    
    private void InstantiateObject()
    {
        if (time >= timeToLastSpawn && intervalTime >= interval)
        {
            int randomIndex = UnityEngine.Random.Range(0, objects.Count);
            GameObject obj = Instantiate(objects[randomIndex], objectSpawnPoint.transform.position, Quaternion.identity);
            obj.GetComponent<Object>().ObjectType = (ElementType)UnityEngine.Random.Range(2, 3);
            intervalTime = 0;
        }
        intervalTime += Time.deltaTime;

    }
    
}
