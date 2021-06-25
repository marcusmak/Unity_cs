using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.WebRTC;
using Local.WebRTC;
using UnityEngine.UI;

public class WebRTCController : MonoBehaviour
{
    // Start is called before the first frame update
    private MediaStream videoStream, receiveStream;
    private List<RTCRtpSender> pc1Senders;
    private RTCPeerConnection localConnection, remoteConnection;
    private RTCDataChannel sendChannel,receiveChannel;
    private bool stratChannel = false;
    public RTCConfiguration config = default;

    private PeerVideo peerVideo;
            
    void Start()
    {
        peerVideo = new PeerVideo(GameObject.Find("SelfCam").GetComponent<RawImage>());
    }

    private void Awake() {
        // Debug.Log("Awake P2PVideo");
        WebRTC.Initialize(WebRTCSettings.EncoderType, WebRTCSettings.LimitTextureSize);
        config.iceServers = new[] {new RTCIceServer {urls = new[] {"stun:stun.l.google.com:19302"}}};
     }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.O))
            SetUpWebRTC();

        if(Input.GetKeyDown(KeyCode.P))
            SendMsg("Test");

        if(Input.GetKeyDown(KeyCode.V))
            if(peerVideo.IsStreaming())
                peerVideo.StopWebCam();
            else
                peerVideo.StartWebCam();
        


    }

    private void SetUpWebRTC(){
        CreateLocalPeer();
        CreateRemotePeer();
        RegCommunicationPaths();
        CheckICEConnection();
        CreateSendChannel();
        StartCoroutine(StartSignalling());
    }
    private void CreateLocalPeer(){
        localConnection = new RTCPeerConnection(ref config);
        Debug.Log("Create Local Peer: " + localConnection);

    }

    private void CreateRemotePeer(){
        remoteConnection = new RTCPeerConnection(ref config);
        Debug.Log("Create Remote Peer: " + localConnection);
        remoteConnection.OnDataChannel = channel =>{
            receiveChannel = channel;
            receiveChannel.OnMessage = HandleReceiveMessage;  
        };

    }
    private void CreateSendChannel(){
        RTCDataChannelInit conf = new RTCDataChannelInit();
        sendChannel = localConnection.CreateDataChannel("sendChannel", conf);
        Debug.Log("Create Local Data Channel: " + sendChannel);
        sendChannel.OnOpen = handleSendChannelStatusChange;
        sendChannel.OnClose = handleSendChannelStatusChange;
    }
    private void RegCommunicationPaths(){
        localConnection.OnIceCandidate = e => { 
            if(!string.IsNullOrEmpty(e.Candidate))
                remoteConnection.AddIceCandidate(e); 
        };

        remoteConnection.OnIceCandidate = e => { 
            if(!string.IsNullOrEmpty(e.Candidate))
                localConnection.AddIceCandidate(e); 
        };
    }


    private void handleSendChannelStatusChange(){
        Debug.Log("handleSendChannelStatusChange");

    }

    
    //Signalling 
    IEnumerator StartSignalling(){
        var op = localConnection.CreateOffer();
        Debug.Log("Create Offer");
        yield return op;

        if (!op.IsError)
        {
            yield return StartCoroutine(OnCreateOfferSuccess(op.Desc));
        }
    }

     IEnumerator OnCreateOfferSuccess(RTCSessionDescription desc)
    {
        Debug.Log($"Offer from pc1\n{desc.sdp}");
        Debug.Log("pc1 setLocalDescription start");
        var op = localConnection.SetLocalDescription(ref desc);
        yield return op;

        if (!op.IsError)
        {
            OnSetLocalSuccess(localConnection);
        }
        else
        {
            var error = op.Error;
            OnSetSessionDescriptionError(ref error);
        }

        Debug.Log("pc2 setRemoteDescription start");
        var op2 = remoteConnection.SetRemoteDescription(ref desc);
        yield return op2;
        if (!op2.IsError)
        {
            OnSetRemoteSuccess(remoteConnection);
        }
        else
        {
            var error = op2.Error;
            OnSetSessionDescriptionError(ref error);
        }
        Debug.Log("pc2 createAnswer start");
        // Since the 'remote' side has no media stream we need
        // to pass in the right constraints in order for it to
        // accept the incoming offer of audio and video.

        var op3 = remoteConnection.CreateAnswer();
        yield return op3;
        if (!op3.IsError)
        {
            yield return OnCreateAnswerSuccess(op3.Desc);
        }
    }

    void OnSetLocalSuccess(RTCPeerConnection pc)
    {
        Debug.Log($" SetLocalDescription complete");
    }

    void OnSetSessionDescriptionError(ref RTCError error) { }

    void OnSetRemoteSuccess(RTCPeerConnection pc)
    {
        Debug.Log($" SetRemoteDescription complete");
    }

    IEnumerator OnCreateAnswerSuccess(RTCSessionDescription desc)
    {
        Debug.Log($"Answer from pc2:\n{desc.sdp}");
        Debug.Log("pc2 setLocalDescription start");
        var op = remoteConnection.SetLocalDescription(ref desc);
        yield return op;

        if (!op.IsError)
        {
            OnSetLocalSuccess(remoteConnection);
        }
        else
        {
            var error = op.Error;
            OnSetSessionDescriptionError(ref error);
        }

        Debug.Log("pc1 setRemoteDescription start");

        var op2 = localConnection.SetRemoteDescription(ref desc);
        yield return op2;
        if (!op2.IsError)
        {
            OnSetRemoteSuccess(localConnection);
        }
        else
        {
            var error = op2.Error;
            OnSetSessionDescriptionError(ref error);
        }
    }

    private void CheckICEConnection(){
        // localConnection.OnIceConnectionChange = state => {
        //     Debug.Log("Local ICE: " + state);
        // };
        // remoteConnection.OnIceConnectionChange = state => {
        //     Debug.Log("Remote ICE: " + state);
        // };

        localConnection.OnIceConnectionChange = state => OnIceConnectionChange("local",state);
        localConnection.OnIceConnectionChange = state => OnIceConnectionChange("remote",state);
    }

    void SendMsg(string message)
    {
        sendChannel.Send(message);
    }

    void SendBin(byte[] bytes)
    {
        sendChannel.Send(bytes);
    }

    void HandleReceiveMessage(byte[] bytes)
    {
        var message = System.Text.Encoding.UTF8.GetString(bytes);
        Debug.Log(message);
    }

    void OnIceConnectionChange(string pc, RTCIceConnectionState state)
    {
        switch (state)
        {
            case RTCIceConnectionState.New:
                Debug.Log($"{pc} IceConnectionState: New");
                break;
            case RTCIceConnectionState.Checking:
                Debug.Log($"{pc} IceConnectionState: Checking");
                break;
            case RTCIceConnectionState.Closed:
                Debug.Log($"{pc} IceConnectionState: Closed");
                break;
            case RTCIceConnectionState.Completed:
                Debug.Log($"{pc} IceConnectionState: Completed");
                break;
            case RTCIceConnectionState.Connected:
                Debug.Log($"{pc} IceConnectionState: Connected");
                break;
            case RTCIceConnectionState.Disconnected:
                Debug.Log($"{pc} IceConnectionState: Disconnected");
                break;
            case RTCIceConnectionState.Failed:
                Debug.Log($"{pc} IceConnectionState: Failed");
                break;
            case RTCIceConnectionState.Max:
                Debug.Log($"{pc} IceConnectionState: Max");
                break;
            default:
                break;
        }
    }


    private void OnDestroy()
    {
        if(peerVideo.IsStreaming())
            peerVideo.StopWebCam();
            
        localConnection.Close();
        remoteConnection.Close();

        sendChannel.Close();
        receiveChannel.Close();

        WebRTC.Dispose();
    }


}
