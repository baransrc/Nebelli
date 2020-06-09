using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsDisplayer : MonoBehaviour
{
    private float _timeElapsedSinceLastUpdate = 0f;
    private float _timeNeededToUpdateMinFPSCounter = 5f;
    private int _minFPS = 144;
    private int _maxFPS = 0;
    private bool _firstFrame = true;

    [SerializeField] private float _refreshInterval = 2.0f;
    [SerializeField] private TMPro.TextMeshProUGUI _fpsTextUI = NullObject.TextMeshProUGUI;
    [SerializeField] private bool _showMinMax = false;

    private void UpdateFPSCounter()
    {
        var currentFPS = (int)(1.0f / Time.unscaledDeltaTime);
        
        _minFPS = (currentFPS < _minFPS) && (_timeNeededToUpdateMinFPSCounter <= 0f) ? currentFPS : _minFPS;
        _maxFPS = (currentFPS > _maxFPS) && (_timeNeededToUpdateMinFPSCounter <= 0f) ? currentFPS : _maxFPS;

        var fpsString = "FPS: " + (currentFPS).ToString() + "\n";
        var minMaxString = (_timeNeededToUpdateMinFPSCounter <= 0f) ? "MIN: " + _minFPS.ToString() + "\n" + "MAX: " + _maxFPS.ToString()  
                                                                : "MIN: ???" + "\n" + "MAX: ???";

        _fpsTextUI.text = (_showMinMax) ? fpsString + minMaxString : fpsString;
    }


    private void Update()
    {
        _timeElapsedSinceLastUpdate += Time.unscaledDeltaTime;

        if (_timeNeededToUpdateMinFPSCounter > 0) _timeNeededToUpdateMinFPSCounter -= Time.unscaledDeltaTime;

        if (_timeElapsedSinceLastUpdate >= _refreshInterval || _firstFrame)
        {
            UpdateFPSCounter();
            _timeElapsedSinceLastUpdate = 0.0f;
            _firstFrame = false;
        }
    }
}