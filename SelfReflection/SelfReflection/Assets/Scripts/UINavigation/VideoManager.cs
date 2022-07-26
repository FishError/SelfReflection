using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VideoManager : MonoBehaviour
{
    public TMP_Dropdown resDropdown;
    public TMP_Dropdown displayMode;
    private List<TMP_Dropdown.OptionData> resOptions = new List<TMP_Dropdown.OptionData>();
    private List<TMP_Dropdown.OptionData> displayOptions = new List<TMP_Dropdown.OptionData>();

    private void Start()
    {
        resOptions = resDropdown.GetComponent<TMP_Dropdown>().options;
        displayOptions = displayMode.GetComponent<TMP_Dropdown>().options;
        LoadScreenResolution();
    }

    public List<int> ParseScreenResolution()
    {
        string[] res = resOptions[resDropdown.value].text.Split('x');
        int results = 0;
        List<int> screenRes = new List<int>();
        foreach (var item in res)
        {
            var cur = item.Replace(" ", "");
            int.TryParse(cur, out results);
            screenRes.Add(results);
        }
        return screenRes;
    }

    public string GetDisplayMode()
    {
        return displayOptions[displayMode.value].text;
    }


    public void ChangeScreenResolution()
    {
        List<int> getSize = ParseScreenResolution();
        string displayMode = GetDisplayMode();
        switch (displayMode)
        {
            case "Windowed":
                Screen.SetResolution(getSize[0], getSize[1], FullScreenMode.Windowed);
                print("Windowed - " + getSize[0] + "x" + getSize[1]);
                break;
            case "Borderless":
                Screen.SetResolution(getSize[0], getSize[1], FullScreenMode.FullScreenWindow);
                print("Borderless - " + getSize[0] + "x" + getSize[1]);
                break;
            case "Fullscreen":
                Screen.SetResolution(getSize[0], getSize[1], FullScreenMode.ExclusiveFullScreen);
                print("Fullscreen - " + getSize[0] + "x" + getSize[1]);
                break;
        }
        PlayerPrefs.SetString("displayMode", displayMode);
    }

    public void LoadScreenResolution()
    {
        if (PlayerPrefs.HasKey("displayMode"))
        {
            resDropdown.RefreshShownValue();
            displayMode.RefreshShownValue();
        }
    }
}

