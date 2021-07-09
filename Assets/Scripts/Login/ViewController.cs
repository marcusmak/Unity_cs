using UnityEngine;
using Local.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class ViewController : MonoBehaviour{
    HttpClient httpClient;
    GameObject roomUI, signinUI, loadingUI;
    string currentUI;

    void Start() {
        httpClient = HttpClient.instance;
        roomUI = GameObject.Find("roomUI");
        signinUI = GameObject.Find("signinUI");
        loadingUI = GameObject.Find("loadingUI");
        roomUI.SetActive(true);
        loadingUI.SetActive(false);
        signinUI.SetActive(false);
    }

    public void Connect(InputField input){
        loadingUI.SetActive(true);
        roomUI.SetActive(false);

        httpClient.Connect(input.text,()=>{
            try
            {
                Debug.Log("connect on success");
                currentUI = "signinUI";
                Debug.Log("connect on success hrer2");
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }

        });
    }

    private void Update() {
        if(currentUI == "loadingUI"){
            roomUI.SetActive(false);
            loadingUI.SetActive(true);
            signinUI.SetActive(false);
        }else if (currentUI == "signinUI"){
            roomUI.SetActive(false);
            loadingUI.SetActive(false);
            signinUI.SetActive(true);
        }

        if(currentUI == "Classroom"){
            roomUI.SetActive(false);
            loadingUI.SetActive(true);
            signinUI.SetActive(false);
            SceneManager.LoadScene("Classroom");
        }
    }

    public void SubmitName(InputField input){
        httpClient.SubmitName(input.text,(others)=>{
            Debug.Log(others.Count);
            currentUI = "Classroom";
        });
    }
    

}