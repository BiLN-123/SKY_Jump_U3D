using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MyGameManager : MonoBehaviour
{
    #region //GET, SET
    //game state -- trạng thái của game
    public GameSate currGameState { get; set; }
    public float LastSpawnedPos { get; set; }
    public Vector3 playerBeforeDeathPos { get; set; }  // vị trí trước khi người chơi chết

    public long Score { get; set; }
    public long HighScore { get; set; }

    public int MissionTotalJumpAmount { get; set; }
    public int MissionJumpAmountInOneRun { get; set; }
    public long MissionScoreAmountReach { get; set; }
    public int NextObsColorType { get; set; }
    #endregion

    #region //PUBLIC VALUE
    public PlayerCtrl playerCtrl;
    public EnviCtl enviCtl;
    public UICtl uICtl;

    /// <summary>
	/// 0: die| 1->3: Jump
	/// </summary>
    public AudioClip[] sounds;
    public AudioSource audioSource;
    #endregion

    public static MyGameManager Instance { private set; get; }

    #region  //PRIVATE VALUES //Continue game, tiếp tục cho người chơi game, làm xong lúc build có thể xoá

    private UserInfo userInfo;

    #endregion

    #region //Private Value

    
    //private long score = 0;
    //private long highScore = 0;
    #endregion

    private void Awake()
    {
        Instance = this;

        //declare some default values
        LastSpawnedPos = Const.LAST_POS_ORI; // set the last pos to spawn the obs

        currGameState = GameSate.ready; //start the game - sẵn sàng cho 1 game mới
        //BiLN random màu
        NextObsColorType = UnityEngine.Random.Range(4, uICtl.mats.Length);
        playerCtrl.gameObject.GetComponent<Renderer>().material = uICtl.mats[UnityEngine.Random.Range(0, 3)];
        enviCtl.START_ORI_OBS.gameObject.GetComponent<Renderer>().material = uICtl.mats[NextObsColorType];
    }
    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("MyGameManager Start");

        //Set User Info, set thông tin người dùng để get, lưu đc
        string userInfoStr = PlayerPrefs.GetString("user_info", "{\"HighScore\":0,\"MissionTotalJumpAmount\":0,\"MissionJumpAmountInOneRun\":0,\"MissionScoreAmountReach\":0}"); //này để lưu trên bộ  nhớ các thiết bị ví dụ như đt
        userInfo = JsonUtility.FromJson<UserInfo>(userInfoStr); //truyền vào 1 kiểu sau đó chuyền vào 1 chuỗi string lấy đc ở trên
        App.Trace("highScore: " + userInfo.HighScore + "|MissionTotalJumpAmount: " + userInfo.MissionTotalJumpAmount + "|MissionJumpAmountInOneRun: " +userInfo.MissionJumpAmountInOneRun + "|MissionScoreAmountReach: " + userInfo.MissionScoreAmountReach, DebugColor.yellow);

        uICtl.missionText[0].text = "Total Jump Amount: " + MissionTotalJumpAmount + "/9999";
        uICtl.missionText[1].text = "Jump Amount In One Run: " + MissionJumpAmountInOneRun + "/500";
        uICtl.missionText[2].text = "ReachScoreAmount: " + MissionScoreAmountReach + "/9999";
        //Invoke("StartTheGame", 2f);

        //Invoke("Test", 1f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTheGame()
    {
        uICtl.gojs[2].SetActive(true);

        StartCoroutine(CountDown(delegate
        {
            uICtl.scoreText.text = "100";   // bắt đầu chạy set điểm cho nó

            Debug.Log("Start The Game. Player Can Move");
            currGameState = GameSate.playing;
        }));
        //hiểu nôm na là sau khi đếm xong nó sẽ thực hiện những hành động trên
    }
    public void Death()
    {
        //audioSource.clip = sounds[0];
        //audioSource.Play();

        audioSource.PlayOneShot(sounds[0]);

        uICtl.gojs[2].SetActive(false);

        currGameState = GameSate.death;

        //Stop movement
        playerCtrl.Rb.useGravity = false;
        playerCtrl.Rb.velocity = Vector3.zero;

        //show game over POP (hiển thị game over)

        uICtl.ShowGameOver();

        MissionScoreAmountReach += Score;

        uICtl.missionText[2].text = "ReachScoreAmount: " + MissionScoreAmountReach + "/9999";

        //Save user info, lưu thông tin người chơi sau khi người chơi chết
        userInfo.HighScore = HighScore;
        userInfo.MissionScoreAmountReach = MissionScoreAmountReach;
        userInfo.MissionJumpAmountInOneRun = MissionJumpAmountInOneRun;
        userInfo.MissionTotalJumpAmount = MissionTotalJumpAmount;

        PlayerPrefs.SetString("user_info", JsonUtility.ToJson(userInfo)); //set lại chuỗi ở trên
        // này là để sau khi nó chết nó lưu vào dưới dạng chuỗi sau khi set
        PlayerPrefs.Save(); //lưu lại gtrị vừa set vào bộ nhớ đt hoặc máy tính

    }
    //Object Pooling - re-used the spawned obj, ở đây thay vì tạo 1 vòng lập 100 cái ground mình dùng thư viện này để tái sử dụng lại các ground mình đã qua

    //Hàm tiếp tục game
    public void ContinueGame()
    {
        uICtl.gojs[2].SetActive(true);
        playerCtrl.transform.position = new Vector3(playerBeforeDeathPos.x, 2f, playerBeforeDeathPos.z); //set toạ độ cho continue game
        uICtl.gojs[1].SetActive(false);

        StartCoroutine(CountDown(delegate
        {
            uICtl.scoreText.text = "100"; //set điểm ở tiếp tục game

            currGameState = GameSate.playing;

            //set active trọng lực do ở trên sau death là cho dừng hẳn trọng lực ở 1 nơi nhất định = -3.9f
            playerCtrl.Rb.useGravity = true;
        }));
    }

    public void NewGame()
    {
        uICtl.gojs[2].SetActive(true);
        
        LastSpawnedPos = Const.LAST_POS_ORI;
        MissionJumpAmountInOneRun = 0;
        uICtl.missionText[1].text = "Jump Amount In One Run: " + MissionJumpAmountInOneRun + "/500";

        enviCtl.SpawnFirstObs();
        ////////////////////////////////////////
        playerCtrl.transform.position = new Vector3(0, 3, 0);
        uICtl.gojs[1].SetActive(false);

        StartCoroutine(CountDown(delegate //bắt đầu chạy game
        {
            uICtl.scoreText.text = "100";   // bắt đầu chạy set điểm cho nó

            currGameState = GameSate.playing;

            //set active trọng lực do ở trên sau death là cho dừng hẳn trọng lực ở 1 nơi nhất định = -3.9f
            playerCtrl.Rb.useGravity = true;
        }));
    }

    //count down 3 2 1 playing hoặc continue hoặc sau khi start game:)))
    IEnumerator CountDown(Action callback) // khi mình gọi hàm countdown nó sẽ truyền vào 1 hàm khác 1 action khác khi chạy hết CD nó gọi call back
    {
        uICtl.scoreText.text = "";
        int time = 3;
        while (time > 0)
        {
            uICtl.countDownText.text = time.ToString();

            yield return new WaitForSeconds(1f); //trước khi về 1s

            time--;
        }
        uICtl.countDownText.text = ""; //hiển thị text 321 vào

        if (callback != null)
        {
            callback();
        }    
    }

    public void UpdateScore(long score) //hàm điểm
    {
        Score = score; // /////////// k dùng this nên k care (biểu thị cho public Score = long score)
        uICtl.scoreText.text = score.ToString();
        if(score > HighScore)
        {
            HighScore = score; // nếu điểm chơi mà lớn hơn highscore thì lấy score đó làm highscore
        }    
    }

    public void UpdateJumpMission()
    {
        //đếm số lần nhảy trong nvụ
        MissionTotalJumpAmount++;
        //đếm số lần nhảy trong 1 game trong nvụ, mỗi lần nhảy + 1 đvị
        MissionJumpAmountInOneRun++;

        uICtl.missionText[0].text = "Total Jump Amount: " + MissionTotalJumpAmount + "/9999";
        uICtl.missionText[1].text = "Jump Amount In One Run: " + MissionJumpAmountInOneRun + "/500";
    }

    public void Test()
    {
        uICtl.gojs[0].SetActive(false);
        uICtl.ShowDialog("This is my Dialog Content...", delegate {
            uICtl.gojs[0].SetActive(true);

            uICtl.ShowNoti("Show noti after click btn OK");
        });
    }
}

public enum GameSate
{
// trạng thái hiện tại của game là gì
ready, playing, death
}

public class UserInfo // tạo class hđt để lưu thong tin người chơi
{
    public long HighScore;
    public int MissionTotalJumpAmount;
    public int MissionJumpAmountInOneRun;
    public long MissionScoreAmountReach;
}