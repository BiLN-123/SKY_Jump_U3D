using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public Transform target; //vật ban đầu

    public float smoothSpeed = .125f; //0,125

    private Vector3 offset; //Khoảng cách từ camera đến vật, vector 3 dành cho góc nhìn 3d

    void Start()
    {
        offset = target.position - transform.position; // hiệu của vật ban đầu sẽ ra được khoảng cách từ camera đến target (Vật)
    }

    private void LateUpdate() // ở nhưng file kia tạo update vật di chuyển trước, còn camera di chuyển theo sau nên ta tạo hàm lateupdate
    {
    	Vector3 desiredPos = target.position - offset; // sẽ ra được khoảng cách của camera theo sau

        Vector3 smoothPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);

        if (target.position.y < .8f) //độ cao cao nhất gần 1.8f nên smooth cho .8 để khỏi bị giật màn hình
        {
            smoothPos.y = transform.position.y;
        }    

    	//tiếp theo ta set toạ sđộ khoảng cách camera này = với toạ độ với vật để vật và camera di chuyển luôn luôn bằng nhau
    	//transform.position = desiredPos;

        transform.position = smoothPos;
    }
}
