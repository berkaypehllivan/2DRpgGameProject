using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private UI_FadeScreen fadeScreen;

    private void Start()
    {
        if (SaveManager.instance.HasSavedData() == false)
            continueButton.SetActive(false);

        AudioManager.instance.PlayMenuMusic();
    }

    public void ContinueGame()
    {
        SaveManager.instance.SaveGame();
        StartCoroutine(LoadSceneWithFadeEffect(1.5f, sceneName));
    }

    public void NewGame()
    {
        SaveManager.instance.DeleteSavedData();
        StartCoroutine(LoadSceneWithFadeEffect(1.5f, sceneName));
    }

    public void ExitGame()
    {
        StartCoroutine(ExitGameWithDelay(2));
    }

    private IEnumerator ExitGameWithDelay(float _delay)
    {
        SaveManager.instance.SaveGame();
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(_delay);
        Application.Quit();
    }

    public IEnumerator LoadSceneWithFadeEffect(float _delay, string _sceneName)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(_sceneName);
    }
}
