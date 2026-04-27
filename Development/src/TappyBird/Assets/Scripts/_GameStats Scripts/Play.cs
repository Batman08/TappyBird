using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    private Animator anim;
    private bool playAnim = true;

    void Start()
    {
        anim = GetComponent<Animator>();

    }

    private void Update()
    {
        if (playAnim == transform)
        {
            anim.Play("Flap");
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(sceneName: SceneNameConstants.MainSceneName);
    }

    public void Credits()
    {
        SceneManager.LoadScene(sceneName: SceneNameConstants.CreditSceneName);
    }
}
