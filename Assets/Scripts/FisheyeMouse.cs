using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FisheyeMouse : MonoBehaviour
{

    private Transform t;

    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
    }
}
