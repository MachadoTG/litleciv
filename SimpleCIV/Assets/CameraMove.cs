using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    public float scrollSpeed;

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize -= scroll * scrollSpeed;
        if (Input.GetKey(KeyCode.A))
            transform.position = new Vector3(transform.position.x -1,transform.position.y,transform.position.z);
        if (Input.GetKey(KeyCode.D))
            transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
        if (Input.GetKey(KeyCode.W))
            transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        if (Input.GetKey(KeyCode.S))
            transform.position = new Vector3(transform.position.x + 1, transform.position.y -1, transform.position.z);
    }
}
