using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int day = 1;
    public int stage = 1;
    [SerializeField] private int daysInStage;

    // In seconds
    public float time = 0;
    // In seconds
    [SerializeField] public float lengthOfDay = 64800;

    [SerializeField] private int maxHP = 100;
    [SerializeField] private int hp;

    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp < 0)
        {
            Lose();
        }

        if (time > lengthOfDay)
        {
            NextDay();
        }

        if (day > daysInStage)
        {
            NextStage();
        }
    }

    void Lose()
    {
        // rip
    }

    void NextDay()
    {
        time = 0;
    }

    void NextStage()
    {
        day = 1;
    }

    public void takeDamage(int dmg)
    {
        hp -= dmg;
    }

}
