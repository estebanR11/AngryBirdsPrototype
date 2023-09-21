using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] GameObject ballPrefab;
    [SerializeField] Rigidbody2D pivot;
    [SerializeField] float respawn;

    Camera cam;
    Vector2 touchPos;
    bool isDragging;
    SpringJoint2D actualSpring;
    [SerializeField] float timer;
     Rigidbody2D ballRb;

    GameObject oldBall;
    // Start is called before the first frame update
    void Start()
    {
            cam = Camera.main;
        SpawnBall();
    }

    // Update is called once per frame
    void Update()
    {
        if(ballRb == null) { return; }

        if(!Touchscreen.current.primaryTouch.press.isPressed)
        {
         
            if (isDragging)
            {
                LaunchBall();
            }
            return;
        }
        else
        {
            touchPos = Touchscreen.current.primaryTouch.position.ReadValue(); 
            ballRb.isKinematic = true;
            isDragging = true;
        }

        Vector3 worldPosition = cam.ScreenToWorldPoint(touchPos);
        ballRb.position = worldPosition;

    }

    void LaunchBall()
    {
        ballRb.isKinematic = false;
        ballRb = null;
        Invoke(nameof(DetachBall), timer);
    }

    void DetachBall()
    {
        actualSpring.enabled = false;
        actualSpring = null;
        Invoke(nameof(SpawnBall), respawn);
    }

    void SpawnBall()
    {
        if(oldBall!=null)
        {
            Destroy(oldBall);
            oldBall = null;
        }

        isDragging = false;
        oldBall = Instantiate(ballPrefab, pivot.position, Quaternion.identity);

        ballRb = oldBall.GetComponent<Rigidbody2D>();
        actualSpring = oldBall.GetComponent<SpringJoint2D>();
        actualSpring.enabled = true;
        // Configura el connectedBody
        actualSpring.connectedBody = pivot;

        // Habilita el SpringJoint2D
 
    }

}
