using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class SeparateGameManager : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference holdAction;
    [SerializeField] private InputActionReference positionAction;
    [SerializeField] private Vector2 inputPosition;
    [SerializeField] private float selectionValue;

    [Header("Game Objects")]
    [SerializeField] private List<GameObject> walls;
    [SerializeField] private List<GameObject> objectsToSpawn;
    [SerializeField] private List<GameObject> objects;
    [SerializeField] private GameObject objectSpawnPoint;
    [SerializeField] private GameObject groundToSpawn;
    [SerializeField] private GameObject groundParent;
    [SerializeField] private GameObject objectParent;
    [SerializeField] private Camera cam;
    public HidePhone hidePhoneScript;

    [Header("Time")]
    [SerializeField] private float time;
    [SerializeField] private float interval;
    [SerializeField] private float intervalTime;

    [Header("Status")]
    [SerializeField] private bool isGameRunning;
    [SerializeField] private bool isGameOver;
    public int score;

    public enum ElementType
    {
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

        // Si hidePhoneScript n'est pas assigné dans l'inspecteur, essayer de le trouver automatiquement
        if (hidePhoneScript == null)
        {
            hidePhoneScript = FindObjectOfType<HidePhone>();  // Trouver le premier objet HidePhone dans la scène
        }

        if (hidePhoneScript == null)
        {
            Debug.LogError("HidePhone script not found in the scene!");
        }
        holdAction.action.Enable();
        positionAction.action.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        objectsToSpawn = Resources.LoadAll<GameObject>("Separate/Objects").ToList();
        groundToSpawn = Resources.Load<GameObject>("Separate/Ground/Ground");
        FitBordersToScreen();
        InitializeGrounds();
        isGameRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        GamePaused();
        if (isGameRunning)
        {
            InstantiateObject();
            DecreaseTime();
            EndOfGame();
            GetInput();
            SelectObject();
        }
    }

    private void GetInput()
    {
        inputPosition = positionAction.action.ReadValue<Vector2>();
        selectionValue = holdAction.action.ReadValue<float>();
    }

    // Select the object
    private void SelectObject()
    {
        if (selectionValue > 0f)
        {
            RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(inputPosition), Vector2.zero);
            if (hit.collider != null && hit.collider.GetComponent<Object>() != null)
            {
                hit.transform.position = cam.ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y, cam.nearClipPlane));
            }
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
        if (time <= 0f) // If the time is over
        {
            foreach (var finishedObject in objects)
            {
                finishedObject.GetComponent<Rigidbody2D>().simulated = false; // Stop the object
                if (finishedObject.GetComponent<Object>().isOnCorrectGround && !finishedObject.GetComponent<Object>().isInTheAir)
                {
                    score += 100; // Add points if the object is correctly placed
                }
                else if (!finishedObject.GetComponent<Object>().isInTheAir)
                {
                    score -= 100; // Deduct points if the object is incorrectly placed
                }
            }

            Debug.Log("Score: " + score);

            isGameRunning = false;
            isGameOver = true;

            // Call reset game function
            ResetGame();

            // Transition to the next mini-game
            FindObjectOfType<ChangeMinigame>().OnGameOver();
        }
    }

    // Fit the borders to the screen
    private void FitBordersToScreen()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height; // Get the screen aspect ratio
        float cameraHeight = cam.orthographicSize * 2; // Get the camera height
        float cameraWidth = cameraHeight * screenAspect; // Get the camera width

        var isWallLeft = true;

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

    // Instantiate the object
    private void InstantiateObject()
    {
        if (intervalTime >= interval)
        {
            int randomIndex = UnityEngine.Random.Range(0, objectsToSpawn.Count); // Get a random index from the objects to spawn list
            GameObject obj = Instantiate(objectsToSpawn[randomIndex], objectSpawnPoint.transform.position, Quaternion.identity, objectParent.transform);
            obj.GetComponent<Object>().ObjectType = (ElementType)UnityEngine.Random.Range(0, 1); // Set the object type to a random value (left or right)
            objects.Add(obj);
            intervalTime = 0; // Reset the interval time
        }
        intervalTime += Time.deltaTime;
    }

    // Initialize the ground objects
    private void InitializeGrounds()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height; // Get the screen aspect ratio
        float cameraHeight = cam.orthographicSize * 2; // Get the camera height
        float cameraWidth = cameraHeight * screenAspect; // Get the camera width

        for (int i = 0; i < 2; i++)
        {
            GameObject ground = Instantiate(groundToSpawn, Vector3.zero, Quaternion.identity, groundParent.transform);
            ground.GetComponent<Ground>().GroundType = (ElementType)i;
            ground.transform.localScale = new Vector3(cameraWidth / 2, 0.5f, 1); // Set the scale of the ground object
            ground.transform.localPosition = new Vector3((i == 0 ? -cameraWidth / 4 : cameraWidth / 4), -cameraHeight / 2 + ground.transform.localScale.y / 2, 0); // Set the position of the ground object
        }
    }

    // Reset the game state
    public void ResetGame()
    {
        // Reset score and time
        score = 0;
        time = 15f; // Reset to initial time value

        // Clear and destroy all existing objects
        foreach (var obj in objects)
        {
            Destroy(obj);
        }
        objects.Clear();

        // Reset the interval time
        intervalTime = 0f;

        // Reactivate the game and reset the game over state
        isGameRunning = true;
        isGameOver = false;

        // Re-initialize the ground and borders
        InitializeGrounds();
        FitBordersToScreen();
    }


    public void GamePaused()
    {
        // Vérifier si hidePhoneScript est assigné
        if (hidePhoneScript == null)
        {
            Debug.LogError("HidePhone script is not assigned.");
            return;  // Retourner si la référence est null
        }

        if (hidePhoneScript.isvisble == false)
        {
            // Si le téléphone est caché, on met le jeu en pause
            isGameRunning = false;
        }
        else if (hidePhoneScript.isvisble == true)
        {
            // Si le téléphone est visible, on redémarre le jeu
            isGameRunning = true;
        }
    }
}
