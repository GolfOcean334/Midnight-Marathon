using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class CirclePuzzleGameManager : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference holdAction;
    [SerializeField] private InputActionReference positionAction;
    [SerializeField] private Vector2 inputPosition;
    [SerializeField] private float selectionValue;
    
    [Header("Game Objects")]
    [SerializeField] private List<GameObject> pictureParts;
    [SerializeField] private Camera cam;
    public HidePhone hidePhoneScript;

    [Header("Time")]
    [SerializeField] private float time;
    
    [Header("Gameplay")]
    [SerializeField] private float tolerance;
    [SerializeField] private float minimumDefaultRotation;
    [SerializeField] private float maximumDefaultRotation;
    
    [Header("Status")]
    [SerializeField] private bool isGameRunning;
    [SerializeField] private bool isGameOver;
    public int score;
    
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
        isGameRunning = true;
        PositionObjects();
        RotateObject();
    }

    // Update is called once per frame
    void Update()
    {
        GamePaused();
        if (isGameRunning)
        {
            DecreaseTime();
            GetInput();
            ReturnToZero();
            EndOfGame();
        }
    }

    private void FixedUpdate()
    {
        if (isGameRunning)
        {
            SelectObject();
        }
    }

    // Position the objects
    private void PositionObjects()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        float quarterHeight = screenHeight / 4;

        for (int i = 0; i < pictureParts.Count; i++)
        {
            float yPos = quarterHeight;
            Vector3 worldPosition = cam.ScreenToWorldPoint(new Vector3(0, yPos, cam.nearClipPlane));
            pictureParts[i].transform.position = new Vector3(0, worldPosition.y, 0);
        }
    }
    
    // Rotate the object randomly
    private void RotateObject()
    {
        pictureParts[0].transform.rotation = Quaternion.Euler(0, 0, Random.Range(minimumDefaultRotation, maximumDefaultRotation));
    }
    
    // Select the object
    private void SelectObject()
    {
        if (selectionValue > 0f)
        {
            RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(inputPosition), Vector2.zero);
            if (hit.collider != null && hit.collider.GetComponent<Picture>().isInner)
            {
                hit.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z + inputPosition.x);
            }
        }
    }
    
    private void ReturnToZero()
    {
        if (pictureParts[0].transform.rotation.z >= 360f)
        {
            pictureParts[0].transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (pictureParts[0].transform.rotation.z <= -360f)
        {
            pictureParts[0].transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    
    private void GetInput()
    {
        inputPosition = positionAction.action.ReadValue<Vector2>();
        selectionValue = holdAction.action.ReadValue<float>();
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
            // Vérification de la rotation et calcul du score
            if (Mathf.Abs(pictureParts[0].transform.localRotation.eulerAngles.z) <= tolerance)
            {
                score += 100;
                Debug.Log("You win!");
            }
            else
            {
                score -= 100;
                Debug.Log("You lose!");
            }
            isGameRunning = false;
            // Appeler la fonction de réinitialisation
            ResetGame();

            // Passer au prochain mini-jeu
            FindObjectOfType<ChangeMinigame>().OnGameOver();

        }
    }


    public void ResetGame()
    {
        // Réinitialiser le score
        score = 0;

        // Réinitialiser le temps
        time = 7f; // Remettez la valeur initiale du temps ici, par exemple 60 secondes.

        // Réinitialiser l'état des objets du puzzle (ici on remet la rotation des pièces à zéro)
        foreach (var part in pictureParts)
        {
            part.transform.rotation = Quaternion.Euler(0, 0, 0); // Réinitialiser la rotation de chaque pièce du puzzle
        }

        // Réinitialiser l'état du jeu
        isGameRunning = true;
        isGameOver = false;

        // Positionner à nouveau les objets
        PositionObjects();

        // Réinitialiser la position et la rotation des pièces du puzzle si nécessaire
        RotateObject();

        // Réinitialiser d'autres paramètres si nécessaire
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
