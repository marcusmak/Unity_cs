using UnityEngine;

public class OtherPlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isInteracting = false;
    GameObject promptText;
    void Start()
    {
        promptText = GetComponentInChildren<TextMesh>().gameObject;
        promptText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPrompt(bool active){
        promptText.SetActive(active);
    }

    public void Interact(){
        if(isInteracting){
            Debug.Log("interact with me");
            isInteracting = true;
        }
    }
}
