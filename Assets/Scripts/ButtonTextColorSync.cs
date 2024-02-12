using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTextColorSync : MonoBehaviour
{
    private TMPro.TMP_Text text;
    private Button button;

    private void Start()
    {
        text = GetComponentInChildren<TMPro.TMP_Text>();
        button = GetComponent<Button>();
    }

    private void Update()
    {
        text.color = button.targetGraphic.canvasRenderer.GetColor();
    }
}
