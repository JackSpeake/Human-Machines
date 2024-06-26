using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpText : MonoBehaviour
{

    [SerializeField] private TMP_Text textbox;
    [SerializeField] private Transform mousePos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPopUpText(string newText) {
        textbox.text = newText;
    }

    public void SetPosition() {
        if (mousePos.position.x > -15) {
            GetComponent<Transform>().position = new Vector2(mousePos.position.x - 5f, mousePos.position.y);
        }
        else {
            GetComponent<Transform>().position = new Vector2(mousePos.position.x + 5f, 0f);
        }
    }


}
