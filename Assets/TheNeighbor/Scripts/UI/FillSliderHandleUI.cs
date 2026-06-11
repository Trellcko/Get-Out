using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FillSliderHandleUI : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField] private RectTransform sliderArea;
    [SerializeField] private RectTransform handle;
    [SerializeField] private Image fillImage;

    [field: SerializeField, Range(0f, 1f)] public float Value { get; private set; }
    
    public event Action<float> ValueChanged;

    private void OnValidate()
    {
        if(handle && fillImage && sliderArea)
            SetValue(Value);
    }

    private void Start()
    {
        SetValue(Value);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UpdateSlider(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateSlider(eventData);
    }

    private void UpdateSlider(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            sliderArea,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        );

        Rect rect = sliderArea.rect;

        float normalized = Mathf.InverseLerp(
            rect.yMin,
            rect.yMax,
            localPoint.y
        );

        SetValue(normalized);
    }

    private void SetValue(float newValue)
    {
        Value = Mathf.Clamp01(newValue);

        fillImage.fillAmount = Value;

        Rect rect = sliderArea.rect;

        Vector2 handlePosition = handle.anchoredPosition;
        handlePosition.y = Mathf.Lerp(rect.yMin, rect.yMax, Value);
        handle.anchoredPosition = handlePosition;
        
        ValueChanged?.Invoke(Value);
    }
}