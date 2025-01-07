using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchRadius : MonoBehaviour
{
    public GameObject obj1;
    public GameObject obj2;
    public float threshold;
    public string newScene;

    void Update()
    {
        // TODO -- Switch to end scene if the distance between objects is less than the threshold
        // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/SceneManagement.SceneManager.LoadScene.html

        // Gets the distance between 2 positions:
        //Vector2.Distance(obj1.transform.position, obj2.transform.position);

        // Switches scenes
        //SceneManager.LoadScene(newScene);
    }
}
