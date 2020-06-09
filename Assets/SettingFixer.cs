using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingFixer : MonoBehaviour
{
    [SerializeField] int _fixedFPS = -1;

    private void FixSettings()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = _fixedFPS;
    }

    private void Awake()
    {
        FixSettings();
    }
}
