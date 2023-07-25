using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SnakeSegmentReleaser : MonoBehaviour
{
    SpriteRenderer sr;
    bool shouldDestroy = false;

    private void Start() {
        sr = GetComponent<SpriteRenderer>();
        if (shouldDestroy) {
            StartCoroutine(FadeAndDestroy());
        }
    }
    public void Release() {
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        if (sr != null) {
            StartCoroutine(FadeAndDestroy());
        }
        else shouldDestroy = true;
    }

    //private void OnTriggerEnter2D(Collider2D collision) {
    //    StopCoroutine(FadeAndDestroy());
    //    GameObject.Destroy(gameObject);
    //}

    private IEnumerator FadeAndDestroy() {
        for (float fade = 1f; fade >= 0; fade -= 0.1f) {
            Color color = sr.material.color;
            color.a = fade;
            sr.color = color;
            yield return new WaitForSeconds(0.05f);
        }

        GameObject.Destroy(gameObject);
    }
}
