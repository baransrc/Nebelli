using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    private void ChangePromptText(string text)
    {
        StopAllCoroutines();
        StartCoroutine(Helper.DisplayText(_prompt, text, 0.2f, 1f, 0.2f));
    }

    public void OnClickSaveButton()
    {
        var levelName = GetLevelName();

        if (levelName == "")
        {
            ChangePromptText("Level file name cannot be empty.");
            return;
        }

        var saveFile = _levelEditor.GetSaveFile(levelName);

        if (saveFile == Strings.Empty)
        {
            ChangePromptText("Level file is empty, therefore will not be saved.");
            
            return;
        }
        
        else if (saveFile == Strings.NoPlayer)
        {
            ChangePromptText("Level cannot be saved without player. Select \"AddPlayer\" from Insertion menu to add player.");

            return;
        }

        File.WriteAllText(Strings.LevelDataPath + levelName, saveFile);
    }

    public void OnClickPlayButton()
    {
        var levelName = GetLevelName();

        if (levelName == "")
        {
            ChangePromptText("Level file name cannot be empty.");
            return;
        }

        _levelEditor.Play(levelName);
    }

    public void OnClickLoadButton()
    {
        var levelName = GetLevelName();

        if (levelName == "")
        {
            ChangePromptText("Level file name cannot be empty.");
            return;
        }

        _levelEditor.Load(levelName);
    }
}
