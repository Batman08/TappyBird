using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

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
            anim.SetTrigger("Flap");
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Credits()
    {
        SceneManager.LoadScene(2);
    }
}
