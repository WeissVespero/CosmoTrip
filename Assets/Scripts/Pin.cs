using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Pin : MonoBehaviour
{
    private PinStatus _status;
    private int _levelID;
    [SerializeField] private Button _button;
    [SerializeField] private Image _buttonImage;

    public Color ColorPassed;
    public Color ColorAvailable;
    public Color ColorUnavailable;
    public Color ColorActive;

    public event Action<int> OnPinClick;

    public void Initialize(int levelID, PinStatus status)
    {
        _levelID = levelID;
        Subscribe();
        SetStatus(status);
    }

    public void SetStatus(PinStatus status)
    {
        _status = status;
        SetColor();
    }

    private void Subscribe()
    {
        _button.onClick.AddListener(PinClicked);
    }

    private void PinClicked()
    {
        if (_status == PinStatus.Available)
        {
            OnPinClick?.Invoke(_levelID);
        }
    }

    private void SetColor()
    {
        switch (_status)
        {
            case PinStatus.Passed:
                _buttonImage.color = ColorPassed;
                break;
            case PinStatus.Available:
                _buttonImage.color = ColorAvailable;
                break;
            case PinStatus.Unavailable:
                _buttonImage.color = ColorUnavailable;
                break;
            case PinStatus.Active:
                _buttonImage.color = ColorActive;
                break;
            default:
                break;
        }
    }
}

public enum PinStatus
{
    Available,
    Unavailable,
    Active,
    Passed
}
