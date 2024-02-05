using System.Collections;
using System.Collections.Generic;
using UnityEngine;

private Transform t;

public class FisheyeMouse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        T = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
    }
}
