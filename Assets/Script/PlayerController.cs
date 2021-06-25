using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const float moveSpeed = 20.0F;
    const float rotationSpeed = 50.0F;
    Rigidbody _rigidBody;
    Animator animator;
    // UnityEngine.Video.VideoPlayer videoPlayer;
    GameObject videoPlayer;
    // VideoReceiver videoReceiver;


    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();  
        animator = GetComponentInChildren<Animator>();
        // videoReceiver = GetComponent<VideoReceiver>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        _rigidBody.velocity = Vector3.zero;
        if(Input.GetKey(KeyCode.W)){
            animator.SetBool("isWalking",true);
            _rigidBody.velocity = transform.forward * moveSpeed;
            
        }else if(Input.GetKey(KeyCode.S)){
            animator.SetBool("isWalking",true);
            _rigidBody.velocity = -transform.forward * moveSpeed;
       }else{
           if(animator.GetBool("isWalking")){
               animator.SetBool("isWalking",false);
           }
       }



        if (Input.GetKey(KeyCode.A))
        {
            //Rotate the sprite about the Y axis in the positive direction
            transform.Rotate(new Vector3(0, -1, 0) * Time.deltaTime * rotationSpeed, Space.World);
        }

        if (Input.GetKey(KeyCode.D))
        {
            //Rotate the sprite about the Y axis in the negative direction
            transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * rotationSpeed, Space.World);
        }
        
        // if (Input.GetKey(KeyCode.X)){
        //     videoPlayer.SetActive(!videoPlayer.activeSelf);
        //     if(videoPlayer.activeSelf){
        //         videoReceiver.StartSelfCam(videoPlayer.GetComponent<MeshRenderer>());
        //     }else{
        //         videoReceiver.StopSelfCam();
        //     }
        // }
    }
}
