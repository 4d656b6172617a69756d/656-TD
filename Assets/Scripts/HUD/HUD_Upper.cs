using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD_Upper : MonoBehaviour
{
    public GameObject wavesList;
    public GameObject dragButton;
    public GameObject upperHUD;
    public TextMeshProUGUI timerText;

    private List<RectTransform> waveImages = new List<RectTransform>();
    private RectTransform wavesListRectTransform;
    private RectTransform upperHUDRectTransform;

    private Vector2 originalPosition;
    private Vector2 targetPosition;

    private float dragDuration = 0.5f;
    private float waveSpeed = 30f;
    private float startTime;

    private bool isDragging = false;

    void Start()
    {
        wavesListRectTransform = wavesList.GetComponent<RectTransform>();
        upperHUDRectTransform = upperHUD.GetComponent<RectTransform>();
        originalPosition = new Vector2(0, wavesListRectTransform.anchoredPosition.y + 50);
        targetPosition = new Vector2(0, wavesListRectTransform.anchoredPosition.y - 30);

        foreach (Transform waveImage in wavesList.transform)
        {
            RectTransform rectTransform = waveImage.GetComponent<RectTransform>();
            waveImages.Add(rectTransform);
        }
    }

    void Awake()
    {
        dragButton.GetComponent<Button>().onClick.AddListener(() => OnDragButtonClicked());
    }

    void Update()
    {
        ImageMovement();
        UpdateTime();
    }

    void OnDragButtonClicked()
    {
        isDragging = !isDragging;

        if (isDragging)
        {
            LeanTween.move(upperHUDRectTransform, targetPosition, dragDuration);
        }
        else
        {
            LeanTween.move(upperHUDRectTransform, originalPosition, dragDuration);
        }
    }

    void ImageMovement()
    {
        foreach (RectTransform waveImage in waveImages)
        {
            waveImage.anchoredPosition -= new Vector2(waveSpeed * Time.deltaTime, 0);

            if (waveImage.anchoredPosition.x < wavesListRectTransform.anchoredPosition.x+5)// - wavesListRectTransform.rect.width / 3)
            {
                waveImage.anchoredPosition += new Vector2(wavesListRectTransform.rect.width, 0);
            }
        }
    }

    void UpdateTime()
    {
        float elapsedTime = Time.time - startTime;
        string formattedTime = FormatTime(elapsedTime);
        timerText.text = formattedTime;
    }

    string FormatTime(float timeInSeconds)
    {
        int hours = Mathf.FloorToInt(timeInSeconds / 3600);
        int minutes = Mathf.FloorToInt((timeInSeconds - hours * 3600) / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds - hours * 3600 - minutes * 60);

        return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }


}


