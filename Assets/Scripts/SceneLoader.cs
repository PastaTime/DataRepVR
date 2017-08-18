using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private const string TESTING_CONTROLLER = "Assets/Scenes/testing/";
    private const string TESTING_LEAP = "Assets/Scenes/testing/";
    private const string CONTROLLER_DESKTOP = "Assets/Scenes/ControllerDesktop.unity";
    private const string CONTROLLER_VR = "Assets/Scenes/ControllervR.unity";
    private const string LEAP_VR = "Assets/Scenes/LeapVR.unity";

    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            SceneManager.LoadSceneAsync(TESTING_CONTROLLER);
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            SceneManager.LoadSceneAsync(TESTING_LEAP);
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            SceneManager.LoadSceneAsync(CONTROLLER_DESKTOP);
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            SceneManager.LoadSceneAsync(CONTROLLER_VR);
        }
        else if (Input.GetKey(KeyCode.Alpha5))
        {
            SceneManager.LoadSceneAsync(LEAP_VR);
        }
    }
}