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
    public int ContinuousMissCount { get; private set; } = 0;
    public int TotalHitCount { get; private set; } = 0;

    public ScoreDisplay chc, cmc, thc;
    #endregion
    public HPBar hpBar;
    private bool isStart;
    public Text startGametext;
    #region ʧ����Ƶ
    public GameObject videoPlayer;
    private bool isPlayingVideo;
    private bool isLose;
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
        if (Input.GetMouseButtonDown(0) && !isStart)
        {
            StartGame();
        }
        if (Input.GetMouseButtonDown(0) && isPlayingVideo)
        {
            SceneManager.LoadScene(0);
        }


        if (!isStart) return;
        if (isLose) return;

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
        isStart = true;
        startGametext.gameObject.SetActive(false);
    }
    public void LoseGame()
    {
        isLose = true;
        TimeDelay.Instance.Delay(1, () =>
        {
            isPlayingVideo = true;
            videoPlayer.gameObject.SetActive(true);
            SoundManager.Instance.MuteSound();
            SoundManager.Instance.MuteMusic();
            TimeDelay.Instance.Delay(13, () => SceneManager.LoadScene(0));
        });


    }

    public float GetNowTimer()
    {
        return GenerateTimer;
    }
}
