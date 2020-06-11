using System;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class LevelFileReader : MonoBehaviour
{
    [SerializeField] private string _fileName = "denemeLevel.lvl";

    private void Awake()
    {

    }

    public void SetFileName(string fileName)
    {
        _fileName = fileName;
    }

    public LevelProperties GetCurrentLevelProperties()
    {
        var lines = File.ReadAllLines(Strings.LevelDataPath + _fileName);


        var gridWidth = int.Parse(string.Join("", lines[0].Split(':').Skip(1)));
        var gridHeight = int.Parse(string.Join("", lines[1].Split(':').Skip(1)));

        var levelProperties = new LevelProperties(gridWidth, gridHeight);

        var playerData = string.Join("", lines[2].Split(':').Skip(1));
        var playerIndices = playerData.GetUntilOrEmpty(" ").Split(',');

        levelProperties.PlayerPosX = int.Parse(playerIndices[0]);
        levelProperties.PlayerPosY = int.Parse(playerIndices[1]);
        levelProperties.PlayerColor = (PredefinedColor)int.Parse(playerData.Split(' ')[1]);

        for (var i = 3; i < lines.Length; ++i)
        {
            var indices = lines[i].GetUntilOrEmpty(":").Split(',');
            var xIndex = int.Parse(indices[0]);
            var yIndex = int.Parse(indices[1]);

            var itemCellData = lines[i].Split(':')[1].Split(' ');

            levelProperties.SetCellEnabled(xIndex, yIndex, itemCellData[0] == "1" ? true : false);
            levelProperties.SetItemType(xIndex, yIndex, (ItemType)int.Parse(itemCellData[1]));
            levelProperties.SetItemColor(xIndex, yIndex, (PredefinedColor)int.Parse(itemCellData[2]));
        }

        return levelProperties;
    }
}