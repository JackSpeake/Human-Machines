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
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        t.position = new Vector3 (Mathf.Clamp(mousePosition.x, -22.6f, -6.5f), Mathf.Clamp(mousePosition.y, -4.4f, 4.3f), 1.0f);
    }
}
