using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICtl : MonoBehaviour
{
    /// <summary>
	/// 0: start the game :)) | 1: game over (you die) | 2: In Game :)) | 3:setting á | 4: là info của tui á :))) | 5: Dialog - thông báo :((( | 6: show noti | 7: mission
	/// </summary>

    //start game
    public GameObject[] gojs;

    public Transform popTargetPos;

    public Text countDownText;
    public Text scoreText;
    public Text dialogContentText;
    public Text notiContentText;
    public Text gameOverScoreText;
    public Text gameOverHighScoreText;

    /// <summary>
	/// 0: MissionTotalJumpAmount| 1:MissionJumpAmountInOneRun| 2:MissionScoreAmountReach
	/// </summary>
    // do nhiều nhiệm vụ nên tạo mảng :)))
    public Text[] missionText;

    public Material[] mats;

    private Action callBackOnBtnOk = null; //gắn call back cho hành động khi nhấn nút OK


    // Start is called before the first frame update
    void Start() //hàm này dùng for để chạy hết tất cả các gojs, các cái popup này sẽ được ẩn do dùng false ngoại trừ gojs[0] để true thì chỉ mình nó hiện
    {
        for(int i = 0; i < gojs.Length; i++)
        {
            gojs[i].SetActive(false);
            gojs[i].transform.position = popTargetPos.position;
        }
        gojs[0].SetActive(true);
    }

    public void PlayGame()
    {
        gojs[0].SetActive(false);
        MyGameManager.Instance.StartTheGame();
    }

    public void ShowGameOver()
    {
        gameOverScoreText.text = "SCORE: " + MyGameManager.Instance.Score.ToString();
        gameOverHighScoreText.text = "HIGH SCORE: " + MyGameManager.Instance.HighScore.ToString();

        gojs[1].SetActive(true);
    }
    public void ChooseColorPlayer(int type)   // màu của player
    {
        MyGameManager.Instance.playerCtrl.gameObject.GetComponent<Renderer>().material = mats[type]; // để chọn màu thì phải truyền type màu vào đây
    }
    public void ChooseColorGround(int type) // màu của ground
    {
        MyGameManager.Instance.NextObsColorType = type;

        MyGameManager.Instance.enviCtl.START_ORI_OBS.gameObject.GetComponent<Renderer>().material = mats[type]; //chuyển màu của thằng parents

        for (int i = 0; i < MyGameManager.Instance.enviCtl.SpawnedObs.Count; i++) //dùng vòng for để chuyển màu của obs sinh ra
        {
            MyGameManager.Instance.enviCtl.SpawnedObs[i].gameObject.GetComponent<Renderer>().material = mats[type]; //chuyển màu của obs sinh ra SpawnedObs
        }    
    }
    public void ShowChooseColor() //hàm hiển thị settings
    {
        gojs[0].SetActive(false);
        gojs[3].SetActive(true);
    }
    public void ShowInfo() //hiển thị thông tin game
    {
        gojs[0].SetActive(false);
        gojs[4].SetActive(true);
    }

    public void ShowDialog(string content/*chuyền chuỗi, chuyền câu thông báo vào*/, Action callBack = null)
    {
        // hiển thị thông báo lên, show noti
        //gojs[5].SetActive(false);
        dialogContentText.text = content;
        if (callBack != null)
        {
            callBackOnBtnOk = callBack; // gán để lưu lại
        }
        gojs[5].SetActive(true);
    }
    public void CloseDialog()
    {
        gojs[5].SetActive(false);
        if (callBackOnBtnOk != null)
        {
            callBackOnBtnOk();
        }    
    }
    public void ShowNoti(string content) //hàm show thông báo
    {
        notiContentText.text = content;
        gojs[6].SetActive(true);
        Invoke("_ShowNoti", 2f);
    }

    void _ShowNoti() //hàm tắt show thông báo
    {
        gojs[6].SetActive(false);
    }
}
