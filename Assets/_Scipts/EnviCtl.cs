using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviCtl : MonoBehaviour
{
    public GameObject obs_prefab;

    public Transform obs_parent;

    public List<ObsCtl> SpawnedObs { private set; get; }

    public GameObject START_ORI_OBS;

    // Start is called before the first frame update
    void Start()
    {
        SpawnFirstObs();
    }

    public void SpawnFirstObs()
    {
        SpawnedObs = new List<ObsCtl>();
        if (SpawnedObs == null)
        {
            for (int i = 0; i < 5; i++)
            {
                SpawnNextObs();
            }
        }
        else
        {
            for (int i = 0; i < SpawnedObs.Count; i++)
            {
                SpawnedObs[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < 5; i++)
            {
                SpawnNextObs();
            }
        }    
    }

    public void SpawnNextObs() //hàm tạo obs(ground) "public" bên playercrtl
    {
        int rand = UnityEngine.Random.Range(3, 4); // random độ dài các groud từ 2 đến 6
        int rand_dis = UnityEngine.Random.Range(2, Const.MAX_OBS_DIS);

        GameObject goj = SpawnObs(rand * 2);

        goj.transform.position = new Vector3(0, -3.9f, MyGameManager.Instance.LastSpawnedPos + rand + rand_dis/* + i*/);
        goj.GetComponent<Renderer>().material = MyGameManager.Instance.uICtl.mats[MyGameManager.Instance.NextObsColorType];
        goj.SetActive(true);

        MyGameManager.Instance.LastSpawnedPos += (rand * 2);
    }

    GameObject  SpawnObs(int rand_length)
    {
        //nếu có 1 phần tử element nằm trong list mà có độ dài bằng với random lengt mình muốn sinh ra && nó có vị trí sau của player
        for (int i =0; i < SpawnedObs.Count; i++)
        {
            if (SpawnedObs[i].ObsLength == rand_length && !MyGameManager.Instance.enviCtl.SpawnedObs[i].gameObject.activeInHierarchy )
            {
                return SpawnedObs[i].gameObject;
            } 
        }
        //if not
        GameObject goj = Instantiate(obs_prefab, obs_parent, false);
        goj.transform.localScale = new Vector3(goj.transform.localScale.x, goj.transform.localScale.y, rand_length); // khoảng cách theo rand
        ObsCtl obsCtl = goj.GetComponent<ObsCtl>();
        obsCtl.ObsLength = rand_length;
        SpawnedObs.Add(obsCtl);
        App.Trace("Spawned Obs List Length =" + SpawnedObs.Count, DebugColor.white);
        return goj;

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Object Pooling - re-used the spawned obj, ở đây thay vì tạo 1 vòng lập 100 cái ground mình dùng thư viện này để tái sử dụng lại các ground mình đã qua

}
