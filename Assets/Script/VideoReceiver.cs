using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.WebRTC;

public class VideoReceiver : MonoBehaviour
{
    // Start is called before the first frame update
    Camera _camera;
    WebCamDevice _defaultWebCam;
    WebCamTexture webcam;

    MeshRenderer videoPlayer;
    
    // MeshRenderer _videoRenderer;
    void Start()
    {
        _camera = GetComponentInChildren<Camera>();
        _defaultWebCam = WebCamTexture.devices[0];
        videoPlayer = GameObject.Find("Video").GetComponent<MeshRenderer>();
        videoPlayer.gameObject.SetActive(false);
        // Debug.Log(_defaultWebCam.name);
        // _videoRenderer = GameObject.Find("Video").GetComponent<MeshRenderer>();
    }

    public void StreamVideo(Texture trackTexture){
        Debug.Log("StreamVideo: " + trackTexture.name);
        videoPlayer.material.mainTexture = trackTexture;
        videoPlayer.gameObject.SetActive(true);
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
