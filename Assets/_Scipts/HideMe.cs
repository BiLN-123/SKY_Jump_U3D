using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideMe : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        /*if (Random.Range(0, 3) == 2)
        {
            gameObject.SetActive(false);
        }
        else if (Random.Range(0, 3) == 3)
        {
            gameObject.SetActive(false);
        }
        else
            gameObject.SetActive(true);*/

        gameObject.SetActive(Random.Range(0, 3) == 2 ? false : true);
    } 
}
