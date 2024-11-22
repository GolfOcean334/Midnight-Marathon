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
        if (time <= 0f) // Si le temps est écoulé
        {
            foreach (var finishedObject in objects)
            {
                finishedObject.GetComponent<Rigidbody2D>().simulated = false; // Arrêter l'objet
                if (finishedObject.GetComponent<Object>().isOnCorrectGround && !finishedObject.GetComponent<Object>().isInTheAir)
                {
                    score += 100; // Ajouter des points si l'objet est bien positionné
                }
                else if (!finishedObject.GetComponent<Object>().isInTheAir)
                {
                    score -= 100; // Retirer des points si l'objet est mal positionné
                }
            }

            Debug.Log("Score : " + score);

            isGameRunning = false;
            isGameOver = true;

            // Appeler la fonction de réinitialisation
            ResetGame();

            // Passer au prochain mini-jeu
            FindObjectOfType<ChangeMinigame>().OnGameOver();
        }
    }


    // Fit the borders to the screen
    private void FitBordersToScreen()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height; // get the screen aspect ratio
        float cameraHeight = cam.orthographicSize * 2; // get the camera height
        float cameraWidth = cameraHeight * screenAspect; // get the camera width

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
            int randomIndex = UnityEngine.Random.Range(0, objectsToSpawn.Count); // get a random index from the objects to spawn list
            GameObject obj = Instantiate(objectsToSpawn[randomIndex], objectSpawnPoint.transform.position, Quaternion.identity, objectParent.transform);
            obj.GetComponent<Object>().ObjectType = (ElementType)UnityEngine.Random.Range(0, 1); // set the object type to a random value (left or right)
            objects.Add(obj);
            intervalTime = 0; // reset the interval time
        }
        intervalTime += Time.deltaTime;
    }
    
    // Initialize the ground objects
    private void InitializeGrounds()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height; // get the screen aspect ratio
        float cameraHeight = cam.orthographicSize * 2; // get the camera height
        float cameraWidth = cameraHeight * screenAspect; // get the camera width

        for (int i = 0; i < 2; i++)
        {
            GameObject ground = Instantiate(groundToSpawn, Vector3.zero, Quaternion.identity, groundParent.transform);
            ground.GetComponent<Ground>().GroundType = (ElementType)i;
            ground.transform.localScale = new Vector3(cameraWidth / 2, 0.5f, 1); // set the scale of the ground object
            ground.transform.localPosition = new Vector3((i == 0 ? -cameraWidth / 4 : cameraWidth / 4), -cameraHeight / 2 + ground.transform.localScale.y / 2, 0); // set the position of the ground object
        }
    }

    public void ResetGame()
    {
        // Réinitialiser le score
        score = 0;

        // Réinitialiser le temps
        time = 60f; // Remettez la valeur initiale du temps ici, par exemple 60 secondes.

        // Réinitialiser l'état des objets (détruire les objets existants et réinitialiser les listes)
        foreach (var obj in objects)
        {
            Destroy(obj); // Détruire les objets existants dans la scène
        }
        objects.Clear(); // Vider la liste des objets

        // Réinitialiser les paramètres de l'intervalle de spawn
        intervalTime = 0f;

        // Réactiver le jeu en cours
        isGameRunning = true;
        isGameOver = false;

        // Réinitialiser les objets à leur état de départ
        InitializeGrounds(); // Réinitialiser le terrain (si nécessaire)
        FitBordersToScreen(); // Réinitialiser les murs (si nécessaire)

        // Optionnel : Vous pouvez aussi réinitialiser la position de la caméra, des personnages ou autres éléments
    }

}
