using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonTest : MonoBehaviour
{
    public Button button;

    void Start()
    {
        button.onClick.AddListener(OnClickDemo);
    }

    // TODO -- Add buttons to change between scenes
    void OnClickDemo()
    {
        Debug.Log("Button clicked");
        Debug.Log("Switching to Next scene");
        SceneManager.LoadScene("NextScene");
    }
}
