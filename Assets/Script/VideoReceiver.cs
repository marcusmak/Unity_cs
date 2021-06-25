using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoReceiver : MonoBehaviour
{
    // Start is called before the first frame update
    Camera _camera;
    WebCamDevice _defaultWebCam;
    WebCamTexture webcam;
    
    // MeshRenderer _videoRenderer;
    void Start()
    {
        _camera = GetComponentInChildren<Camera>();
        _defaultWebCam = WebCamTexture.devices[0];
        // Debug.Log(_defaultWebCam.name);
        // _videoRenderer = GameObject.Find("Video").GetComponent<MeshRenderer>();
    }

    public WebCamDevice StartSelfCam(MeshRenderer renderer){
        webcam = new WebCamTexture(_defaultWebCam.name);
        renderer.material.mainTexture =  webcam;
        webcam.Play();
        return _defaultWebCam;
    }

    public void StopSelfCam(){
        if(webcam != null)
            webcam.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
