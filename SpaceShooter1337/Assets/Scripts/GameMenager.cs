using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMenager : MonoBehaviour
{
    public static Vector2 bottomLeftPosition { get; set; }

    public static Vector2 topRightPosition { get; set; }

    private int coins { get; set; }

    //public PlayerBehaviour player { get; set; }
    [SerializeField] PlayerBehaviour player;

    [SerializeField] MonsterBehaviour monster;

    [SerializeField] GameObject wave;

    [SerializeField] private TMP_Text points;

    [SerializeField] private BossBehaviour boss;

    private int waveCount = 3;
    private int waves { get; set; }
    private float monsterSpeed = 2f;

    void Start()
    {
        bottomLeftPosition = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        topRightPosition = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        PlayerBehaviour p = Instantiate(player) as PlayerBehaviour;
        p.GainCoin += HandleCoinGaining;
        p.PlayerDied += HandlePlayerDied;
        BossBehaviour.BossDied += HandleBossDeath;
        waves = waveCount;
        StartGeneratingMonster();
        
    }

    void HandleBossDeath()
    {
        monsterSpeed+=0.2f;
        ++waves;
        StartGeneratingMonster();
    }
    void HandlePlayerDied()
    {
        Debug.Log("PlayerDied");
        Time.timeScale = 0;
    }

    void HandleCoinGaining()
    {
        ++coins;
        points.text = coins.ToString();
    }

    void StartGeneratingMonster()
    {
        InvokeRepeating("GenerateWave", 2, 3);
    }

    void GenerateWave()
    {
        if (waves == 0)
        {
            CancelInvoke();
            Invoke("GenerateBoss",5f);
            waves = Random.Range(3,7);
            return;
        }
        
        waves--;
        
        GameObject monsterWave = Instantiate(wave, Vector2.zero, Quaternion.identity, transform);

        for (int i = 0; i < 5; ++i)
        {
            float position = (i + 0.5f) / 5;
            Vector2 monsterPosition =
                Camera.main.ScreenToWorldPoint((new Vector2(Screen.width * position, Screen.height)));
            monsterPosition += Vector2.up * monster.transform.localScale.y;

            MonsterBehaviour monsterBehaviour = Instantiate(monster, monsterPosition, Quaternion.identity, monsterWave.transform) as MonsterBehaviour;
            monsterBehaviour.SetSpeed(monsterSpeed);
        }
    }

    void GenerateBoss()
    {
        Vector2 position = new Vector2(0, topRightPosition.y + 1.5f);
        Instantiate(boss, position, Quaternion.identity, transform);
    }
}