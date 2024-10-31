using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOutlineToStart : MonoBehaviour
{
    private float delay = 0.01f;
    [SerializeField] private Outline outline;
    private void OnDrawGizmosSelected()
    {
        outline = GetComponent<Outline>();
    }
    void Start()
    {
        StartCoroutine(DisableOutLine());
    }

    private IEnumerator DisableOutLine()
    {
        yield return new WaitForSeconds(delay);
        outline.enabled = false;
    }
}
