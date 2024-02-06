using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    [SerializeField] private float openSpeed = 1.0f;
    private Vector2 origin;
    private Vector2 destination;
    private RectTransform rt;
    private float width;
    private float height;
    private bool started = false;


    // Start is called before the first frame update
    void Start()
    {
        // width = rt.anchoredPosition.x
        rt = GetComponent<RectTransform>();
        started = true;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Open() {
        if (!started) {
            Start();
        }
        origin = new Vector2(700.0f, 250.0f);
        destination = new Vector2(Random.Range(-400, 500), Random.Range(0, 500));
        StartCoroutine(Move(true));
    }

    public void Close() {
        origin = rt.anchoredPosition;
        destination = new Vector2(700.0f, 250.0f);
        StartCoroutine(Move(false));
    }

    public void MoveLeft(RectTransform panel)
	{
		
	}

	IEnumerator Move(bool grow)
	{
        if (grow) {
            float step = 0;
            while (step < openSpeed)
            {
                rt.anchoredPosition = Vector2.Lerp(origin, destination, step * (1.0f / openSpeed));
                rt.localScale = new Vector3(step * (1.0f / openSpeed), step * (1.0f / openSpeed), 1.0f);
                step += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
        else {
            float step = openSpeed;
            while (step > 0)
            {
                rt.anchoredPosition = Vector2.Lerp(destination, origin, step * (1.0f / openSpeed));
                rt.localScale = new Vector3(step * (1.0f / openSpeed), step * (1.0f / openSpeed), 1.0f);
                step -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            gameObject.SetActive(false);
        }

		
	}
}
