using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public float jumpVelocity; //= 10f; //(Khai báo vtốc độ cao nó nhảy lên)

    public float fallSpeed; //= 2.5f; //(Khai báo vtốc nó rơi xuống)

	private float speed;

    public float fallDownSpeed; //= 150f;

	public Rigidbody Rb { private set; get; }

	private void Awake()
	{
		speed = Const.SPEED_ORI;
	}

    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MyGameManager.Instance.currGameState != GameSate.playing)
        {
            return;
        }    

    	//=============Player Movement (Chuyển động của người chơi)=============

        //Check xem vật thể Death chưa mới cho chuyển động
        if (CheckDeath()) 
        {
            App.Trace("Death", DebugColor.yellow);//EndGame ở đây
            MyGameManager.Instance.Death();
        }

        //Move by Z (Chuyển động theo chiều Z)
        transform.Translate(0, 0, speed * Time.deltaTime);
        App.Trace("Player Y = " + transform.position.y);

        //Jump (Nhân vật nhảy)
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) 
        {
        	Jump();
        }
        if (Rb.velocity.y < 0) 
        {
        	Rb.velocity += Vector3.up * Physics.gravity.y * (fallSpeed - 1) * Time.deltaTime;
        	Rb.AddForce( Vector3.down * fallDownSpeed ); 
        }

        //spawn obs if can, tạo ra thêm ground nếu có thể đúng với trường hợp bạn đã qua các obs(ground)
        if (MyGameManager.Instance.enviCtl.SpawnedObs != null)
        {
            for (int i = 0; i < MyGameManager.Instance.enviCtl.SpawnedObs.Count; i++)
            {
                if (transform.position.z > MyGameManager.Instance.enviCtl.SpawnedObs[i].gameObject.transform.position.z + 15f && MyGameManager.Instance.enviCtl.SpawnedObs[i].gameObject.activeInHierarchy)
                {
                    MyGameManager.Instance.enviCtl.SpawnedObs[i].gameObject.SetActive(false);
                    MyGameManager.Instance.enviCtl.SpawnNextObs();
                    break;
                }
            }
        }
        // tính điểm :))
        MyGameManager.Instance.UpdateScore((long)transform.position.z);
    }
    void Jump() //hàm này để vật nhảy lên
    {
    	if (IsGround()) //vật nằm trong khoảng đã cho nhỏ hơn -0.8f và lớn hơn -1.1f thì nó sẽ nhảy lên
    	{
            App.Trace("Player start jumping - người chơi đang nhảy :)))");

            MyGameManager.Instance.audioSource.PlayOneShot(MyGameManager.Instance.sounds[Random.Range(1,4)]);

            Rb.velocity = Vector3.zero;//(ss nhảy lên khi nó bằng 0, là khi nó chạm đất)
    		Rb.angularVelocity = Vector3.zero;

    		Rb.velocity = Vector3.up * jumpVelocity;

            //gọi hàm nhiệm vụ nhảy Jump
            MyGameManager.Instance.UpdateJumpMission();
        }
    }
    bool IsGround() //hàm này để kiểm tra vật cách cỏ bao nhiêu để set vật đó sống or chết
    {
    	return transform.position.y < -0.8f && transform.position.y > -1.1f;
    }

    bool CheckDeath()
    {
        return transform.position.y < Const.DEATH_POS_Y ;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "obs")
        {
            App.Trace("Hit obs");
            MyGameManager.Instance.playerBeforeDeathPos = collision.transform.position;
        }     
    }
}
  