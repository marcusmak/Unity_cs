// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using Local.Networking;
using Local.Util;
public class MainPlayerController : MonoBehaviour
{
    const float moveSpeed = 20.0F;
    const float rotationSpeed = 50.0F;
    Rigidbody _rigidBody;
    Animator animator;
    GameObject videoPlayer;
    RayCaster playerCaster;

    // UnityEngine.Video.VideoPlayer videoPlayer;
    // VideoReceiver videoReceiver;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();  
        animator = GetComponentInChildren<Animator>();
        HttpClient httpClient = HttpClient.instance;
        Debug.Log(httpClient.room_id);
        SetupRayCaster();
    }

    void SetupRayCaster(){
        playerCaster = new RayCaster();
        playerCaster.RayLength = 10;
        playerCaster.LayerMask = 1 << 3;
        playerCaster.OnRayEnter += (collider)=>{
            OtherPlayerController opc = collider.gameObject.GetComponentInParent<OtherPlayerController>();
            Debug.Log("Detected: " + opc.name.ToString());
            opc.SetPrompt(true);
        };

        playerCaster.OnRayStay += (collider)=>{
            OtherPlayerController opc = collider.gameObject.GetComponentInParent<OtherPlayerController>();
            if (Input.GetKey(KeyCode.X))
                opc.Interact();
        };

        playerCaster.OnRayExit += (collider)=>{
            OtherPlayerController opc = collider.gameObject.GetComponentInParent<OtherPlayerController>();
            Debug.Log("Un-Detected: " + opc.name);
            opc.SetPrompt(false);
        };
    }

    // Update is called once per frame
    void Update()
    {
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
        
        
    }

    private void FixedUpdate() {
        Vector3 Origin = transform.position + new Vector3(0,10,0);
        Vector3 Direction = transform.TransformDirection(Vector3.forward);
        playerCaster.CastRay(Origin,Direction);
    }
}




        // if (Input.GetKey(KeyCode.X)){
        //     videoPlayer.SetActive(!videoPlayer.activeSelf);
        //     if(videoPlayer.activeSelf){
        //         videoReceiver.StartSelfCam(videoPlayer.GetComponent<MeshRenderer>());
        //     }else{
        //         videoReceiver.StopSelfCam();
        //     }
        // }