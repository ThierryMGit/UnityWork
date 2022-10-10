using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Permet de garder le script gérant les données des scores
public class DontDestroyOnLoadScene : MonoBehaviour
{
    private static GameObject objectToKeep;
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if(objectToKeep == null) {
            objectToKeep = gameObject;
        } else {
            Destroy(gameObject);
        }
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
