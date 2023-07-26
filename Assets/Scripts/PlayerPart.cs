using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerPart : MonoBehaviour
{
    float extended = 0f;
    
    [SerializeField] float minExtension = 0.3f;
    [SerializeField] float maxExtension = 3f;
    [SerializeField] float extensionSpeed = 9f;
    [SerializeField] float retractionSpeed = 0.75f;
    [SerializeField] string input;
    [SerializeField] GameObject controlGUI;

    GameObject gui;

    void Start()
    {
        gui = Instantiate(controlGUI);    
    }

    void Update()
    {
        Vector3 rayDirection = Camera.main.transform.position - transform.position;

        if (Input.GetButton(input))
        {
            extended += (extended)*Time.deltaTime * extensionSpeed;
            if (extended > maxExtension)
                extended = maxExtension;
        }
        else
        {
            extended -= Time.deltaTime * retractionSpeed;
            if (extended < minExtension)
                extended = minExtension;
        }
    }

    private void LateUpdate()
    {
        transform.localScale = new Vector3(0.2f, extended, 0.2f);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + transform.up * 0.5f);
        gui.GetComponentInChildren<TMP_Text>().GetComponent<RectTransform>().position = screenPos;
        gui.GetComponentInChildren<TMP_Text>().text = input;

        gui.GetComponent<Canvas>().sortingOrder = (int)(-Vector3.SqrMagnitude(Camera.main.transform.position-transform.position));
    }
}
