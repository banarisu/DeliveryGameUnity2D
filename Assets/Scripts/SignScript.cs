using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignScript : MonoBehaviour
{
    public string layerToPushTo;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().sortingLayerName = layerToPushTo;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
