using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorInputHandler : MonoBehaviour
{
    [SerializeField] TMPro.TMP_InputField _levelInputField = NullObject.TMP_InputField;
    [SerializeField] TMPro.TextMeshProUGUI _prompt = NullObject.TextMeshProUGUI;
    [SerializeField] LevelEditor _levelEditor = NullObject.LevelEditor;

    private string GetLevelName()
    {
        return _levelInputField.text;
    }

    public void OnClickSaveButton()
    {
        var levelName = GetLevelName();

        if (levelName == "")
        {
            _prompt.text = "Level file name cannot be empty.";
            return;
        }

        _levelEditor.Save(levelName);
    }

    public void OnClickPlayButton()
    {
        var levelName = GetLevelName();

        if (levelName == "")
        {
            _prompt.text = "Level file name cannot be empty.";
            return;
        }

        _levelEditor.Play(levelName);
    }

    public void OnClickLoadButton()
    {
        var levelName = GetLevelName();

        if (levelName == "")
        {
            _prompt.text = "Level file name cannot be empty.";
            return;
        }

        _levelEditor.Load(levelName);
    }
}
