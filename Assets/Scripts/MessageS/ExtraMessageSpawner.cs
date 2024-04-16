using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct yapperMessageSpecific
{
    public int day;
    public int stage;
    public float time;
    public string message;
}




public class ExtraMessageSpawner : MonoBehaviour
{
    private GameManager gm;
    private int day, stage;
    private float time;

    [SerializeField] private List<yapperMessageSpecific> messages;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        day = gm.day;
        stage = gm.stage;
        time =  gm.time / gm.lengthOfDay;

        List<yapperMessageSpecific> m = new List<yapperMessageSpecific>();

        foreach (yapperMessageSpecific y in messages)
        {
            if (y.stage == stage)
                if (y.day == day)
                    if ( Mathf.Abs(y.time - time) < .1f)
                    {
                        gm.SendNotification(y.message);
                        m.Add(y);
                    }
        }

        foreach (yapperMessageSpecific y in m)
            messages.Remove(y);
    }
}
