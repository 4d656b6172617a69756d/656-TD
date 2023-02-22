using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    void Start()
    {
        
    }

    public void Launch()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Shrek()
    {
        
    }

    public void Settings()
    {
        GameObject menuSettings = GameObject.Find("Button_Settings");
        RectTransform menuSettingsTransform = menuSettings.GetComponent<RectTransform>();
        CanvasGroup menuSettingsCanvasGroup = menuSettings.GetComponent<CanvasGroup>();

        // Animate the button's position to the left
        LeanTween.moveX(menuSettingsTransform, -100f, 1f).setEase(LeanTweenType.linear);

        // Fade-out the button over 0.3 seconds using a linear easing function
        LeanTween.alphaCanvas(menuSettingsCanvasGroup, 0f, 0.3f).setEase(LeanTweenType.linear);

        // Load the new scene after 0.3 seconds
        // LeanTween.delayedCall(0.3f, () => { SceneManager.LoadScene("NewSceneName"); });

    }

}
