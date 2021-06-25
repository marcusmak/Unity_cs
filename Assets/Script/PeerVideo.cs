using UnityEngine;
using UnityEngine.UI;
using Unity.WebRTC;
using System.Collections;

namespace Local.WebRTC{
    public class PeerVideo {
        WebCamDevice _webcamDevice;
        WebCamTexture _wct;
        VideoStreamTrack _videoStreamTrack;
        bool isStreaming = false;

        RawImage sourceImage;
        public PeerVideo(RawImage sourceImage){
            this.sourceImage = sourceImage;       
            sourceImage.gameObject.SetActive(false);
        }

        private bool CheckCamAvaliability(){
            if(WebCamTexture.devices.Length <= 0)
                return false;
            //check permission
            
            if (!(Application.HasUserAuthorization(UserAuthorization.WebCam | UserAuthorization.Microphone))) {
                return false;
            }

            return true;

        }
        public VideoStreamTrack StartWebCam(){
            //check webcam exist
            if(!CheckCamAvaliability())
                return null;
            WebCamDevice webCam = WebCamTexture.devices[0];

            if (_videoStreamTrack == null)
            {
                // videoStream = cam.CaptureStream(width, height, 1000000);
                _wct = new WebCamTexture(webCam.name);
                _wct.Play();
                _videoStreamTrack = new VideoStreamTrack("video",_wct);
                sourceImage.texture = _wct;
                sourceImage.color = Color.white;
                sourceImage.gameObject.SetActive(true);
                isStreaming = true;
            }
            return _videoStreamTrack;
        }
        
        public void StopWebCam(){
            _wct.Stop();
            _wct = null;
            _videoStreamTrack = null;
            sourceImage.texture = null;
            sourceImage.gameObject.SetActive(false);
            isStreaming= false;
        }

        public bool IsStreaming(){
            return this.isStreaming;
        }

    }
}