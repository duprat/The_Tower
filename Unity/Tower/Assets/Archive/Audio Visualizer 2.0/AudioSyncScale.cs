using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncScale : AudioSyncerBlock
{
    public Vector3 beatScale;
    public Vector3 restScale;

    private IEnumerator MoveToScale(Vector3 target) {
        Vector3 current = transform.localScale;
        Vector3 initial = current;
        float timer = 0;
        while (current != target) {
            current = Vector3.Lerp(initial, target, timer / timeToBeat);
            timer += Time.deltaTime;

            transform.localScale = current;

            yield return null;
        }

        isBeat = false;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (isBeat) return;

        transform.localScale = Vector3.Lerp(transform.localScale, restScale, restSmoothSpeed * Time.deltaTime);
    }

    public override void OnBeat()
    {
        Debug.Log("Beat detected !");
        timer = 0;
        isBeat = true;

        StopCoroutine("MoveToScale");
        StartCoroutine("MoveToScale", beatScale);
    }

}
