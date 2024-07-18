using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FadeScreen : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void fadeOut() => anim.SetTrigger("fadeOut");
    public void fadeIn() => anim.SetTrigger("fadeIn");
}
