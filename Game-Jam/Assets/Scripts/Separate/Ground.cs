using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField] private SeparateGameManager.ElementType groundType;
    public SeparateGameManager.ElementType GroundType
    {
        get => groundType;
        set => groundType = value;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
