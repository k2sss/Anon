using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<Hole> holes = new();

    private float GenerateTimer;
    public State NowState;
    #region �Ʒ����
    public int MaxLife = 5;
    public int Life { get; private set; } = 5;//����ֵ
    public int ContinuousHitCount { get; private set; } = 0;
    public int MaxContinuousHitCount { get; private set; } = 0;
    public int ContinuousMissCount { get; private set; } = 0;
    public int TotalHitCount { get; private set; } = 0;

    public ScoreDisplay chc, cmc, thc;
    #endregion
    public HPBar hpBar;
    public Text startGametext;
    #region ʧ����Ƶ
    public bool isSakiHit;//���һ���Ƿ����SAKI
    public GameObject failureTextsGameObject;
    public VideManager vManager;
    private GameState gameState;
    [HideInInspector] public Text totalHitText;
    [HideInInspector] public Text MaxContinusHitText;
    #endregion
    #region �Ѷ�����
    [Header("�Ѷȵ���")]
    public static float Multiplier = 1f;
    public AnimationCurve curve;//�Ѷ�����
    private float curveX;
    public float speed;//�Ѷ������ٶ�
    #endregion

    private void Start()
    {
        foreach (Hole hole in holes)
        {
            hole.Init(this);
        }
        NowState.Init(this);
    }
    private void Update()
    {



        if (Input.GetMouseButtonDown(0))
        {
            switch (gameState)
            {
                case GameState.Ready:
                    StartGame();
                    break;
                case GameState.PlayVideo:
                    Score();
                    break;
                case GameState.Scroing:
                    SceneManager.LoadScene(0);
                    break;
            }
        }

        if (gameState != GameState.Run) return;

        GenerateTimer += Time.deltaTime * Multiplier;
        if (GenerateTimer > NowState.Interval)
        {
            GenerateTimer = 0;
            GenerateNext();
        }

        //�Ѷȵ���
        if (curveX <= 1f)
        {
            Multiplier = curve.Evaluate(curveX);
            curveX += speed * Time.deltaTime;
        }


    }
    public void GenerateNext()
    {
        if (NowState == null) return;

        //��ȡ��һ��״̬
        State s = NowState.Next();
        if (s != null)
        {
            NowState = s;
            NowState.Init(this);
        }

    }


    public void Miss()
    {
        Life--;
        hpBar.SetHP(Life / (float)MaxLife);
        if (Life == 0) LoseGame();

        SoundManager.Instance.PlaySound("Sounds/error");
        ContinuousHitCount = 0;
        ContinuousMissCount++;
        DisplayScore();
    }

    public void Hit()
    {
        ContinuousHitCount++;
        MaxContinuousHitCount = Mathf.Max(MaxContinuousHitCount, ContinuousHitCount);
        TotalHitCount++;
        ContinuousMissCount = 0;
        DisplayScore();
    }

    private void DisplayScore()
    {
        chc.SetScore(ContinuousHitCount);
        cmc.SetScore(ContinuousMissCount);
        thc.SetScore(TotalHitCount);
    }
    public void StartGame()
    {
        gameState = GameState.Run;
        startGametext.gameObject.SetActive(false);
    }
    public void LoseGame()
    {
        gameState = GameState.Lose;
        TimeDelay.Instance.Delay(1, () =>
        {
            gameState = GameState.PlayVideo;
            if(isSakiHit)
            vManager.Play(1);
            else
                vManager.Play(0);
            SoundManager.Instance.MuteSound();
            SoundManager.Instance.MuteMusic();
            TimeDelay.Instance.Delay(6, () => Score());
        });
    }
    //���Ʒ�
    public void Score()
    {
        gameState = GameState.Scroing;
        failureTextsGameObject.SetActive(true);
        totalHitText.text = $"�ܹ����� {TotalHitCount}��";
        MaxContinusHitText.text = $"������� {MaxContinuousHitCount}��";
    }

    public float GetNowTimer()
    {
        return GenerateTimer;
    }
}
public enum GameState
{
    Ready,
    Run,
    Lose,
    PlayVideo,
    Scroing,
}