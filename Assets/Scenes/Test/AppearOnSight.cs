using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearOnSight : MonoBehaviour
{
    public Color colorBase;
    public Color colorActive;
    private Color currentColor;

    void Start() {
        GetComponent<Renderer>().material.color = colorBase;
    }

    public void setActiveColor(bool isActive) {
        currentColor = (isActive) ? colorActive : colorBase;
        GetComponent<Renderer>().material.color = currentColor;

        // Set a reset timer to go back to base colour

        // Expensive
        StartCoroutine("SetToBaseColor");
    }

    IEnumerator SetToBaseColor() {
        yield return new WaitForSeconds(0.1F);

        currentColor = colorBase;
        GetComponent<Renderer>().material.color = currentColor;
    }
}
