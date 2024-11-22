using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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
    [SerializeField] private GameObject selectedPicture;
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
        if (hidePhoneScript == null)
        {
            hidePhoneScript = FindObjectOfType<HidePhone>();
        }

        if (hidePhoneScript == null)
        {
            Debug.LogError("HidePhone script not found in the scene!");
        }

        holdAction.action.Enable();
        positionAction.action.Enable();
    }

    void Start()
    {
        isGameRunning = true;
        PositionObjects();
        RotateObject();
    }

    void Update()
    {
        GamePaused();
        if (isGameRunning)
        {
            DecreaseTime();
            GetInput();
            ReturnToZero();
            EndOfGame();
            SelectObject();
            if (selectedPicture != null)
            {
                selectedPicture.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z + inputPosition.x);
            }
        }
    }

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

    private void RotateObject()
    {
        pictureParts[0].transform.rotation = Quaternion.Euler(0, 0, Random.Range(minimumDefaultRotation, maximumDefaultRotation));
    }

    private void SelectObject()
    {
        if (selectionValue > 0f)
        {
            RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(inputPosition), Vector2.zero);
            if (hit.collider != null && hit.collider.GetComponent<Picture>().isInner)
            {
                selectedPicture = pictureParts.FirstOrDefault(x => x == hit.collider.gameObject);
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
        if (selectionValue == 0f) selectedPicture = null;
    }

    private void DecreaseTime()
    {
        if (time <= 0) return;
        time -= Time.deltaTime;
    }

    private void EndOfGame()
    {
        if (time <= 0f)
        {
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

            foreach (var picture in pictureParts)
            {
                picture.transform.DOLocalRotate(new Vector3(0, 0, 360), 3f, RotateMode.FastBeyond360);
                picture.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 1.5f);
            }
            
            isGameRunning = false;
        }
    }
    public void GamePaused()
    {
        if (hidePhoneScript == null)
        {
            Debug.LogError("HidePhone script is not assigned.");
            return;
        }

        if (hidePhoneScript.isvisble == false)
        {
            isGameRunning = false;
        }
        else if (hidePhoneScript.isvisble == true)
        {
            isGameRunning = true;
        }
    }
}
