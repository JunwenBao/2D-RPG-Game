using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;
    private Material originalMat;

    [Header("Flash FX")]
    [SerializeField] private float flashDuration;   //Flash Time
    [SerializeField] private Material hitMat;

    [Header("Ailment Colors")]
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] shockColor;


    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        Color currentColor = sr.color;

        sr.color = Color.white;
        yield return new WaitForSeconds(flashDuration);

        sr.color = currentColor;
        sr.material = originalMat;
    }

    private void RedColorBlink()
    {
        if (sr.color != Color.white)
            sr.color = Color.white;
        else
            sr.color = Color.red;
    }

    private void CancelColorChange()
    {
        CancelInvoke(); //取消所有的invoke
        sr.color = Color.white;
    }

    #region Ailment Color Change
    public void IgniteFXFor(float _seconds)
    {
        InvokeRepeating("igniteColorFX", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    public void ChillFXFor(float _seconds)
    {
        InvokeRepeating("chillColorFX", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    public void shockFXFor(float _seconds)
    {
        InvokeRepeating("shockColorFX", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    private void igniteColorFX()
    {
        if (sr.color != igniteColor[0])
            sr.color = igniteColor[0];
        else
            sr.color = igniteColor[1];
    }
    private void chillColorFX()
    {
        if (sr.color != chillColor[0])
            sr.color = chillColor[0];
        else
            sr.color = chillColor[1];
    }
    private void shockColorFX()
    {
        if (sr.color != shockColor[0])
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1];
    }
    #endregion
}