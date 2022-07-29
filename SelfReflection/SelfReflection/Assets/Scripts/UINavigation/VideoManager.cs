using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VideoManager : MonoBehaviour
{
    public TMP_Dropdown resDropdown;
    public TMP_Dropdown displayDropdown;
    private List<TMP_Dropdown.OptionData> resOptions = new List<TMP_Dropdown.OptionData>();
    private List<TMP_Dropdown.OptionData> displayOptions = new List<TMP_Dropdown.OptionData>();

    private void Start()
    {
        resOptions = resDropdown.GetComponent<TMP_Dropdown>().options;
        displayOptions = displayDropdown.GetComponent<TMP_Dropdown>().options;
        LoadScreenResolution();
        LoadDisplayMode();
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


    public void ChangeScreenResolution()
    {
        List<int> getSize = ParseScreenResolution();
        switch (ReturnDisplayMode())
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
        PlayerPrefs.SetInt("ResValue", resDropdown.value);
    }

    public string ReturnDisplayMode()
    {
        return displayOptions[displayDropdown.value].text;
    }

    public void ChangeDisplayMode()
    {
        List<int> getSize = ParseScreenResolution();
        switch (ReturnDisplayMode())
        {
            case "Windowed":
                Screen.SetResolution(getSize[0], getSize[1], FullScreenMode.Windowed);
                break;
            case "Borderless":
                Screen.SetResolution(getSize[0], getSize[1], FullScreenMode.FullScreenWindow);
                break;
            case "Fullscreen":
                Screen.SetResolution(getSize[0], getSize[1], FullScreenMode.ExclusiveFullScreen);
                break;
        }
        PlayerPrefs.SetInt("DisplayValue", displayDropdown.value);
    }

    public void LoadScreenResolution()
    {
        if (PlayerPrefs.HasKey("ResValue"))
        {
            resDropdown.value = PlayerPrefs.GetInt("ResValue");
        }
    }

    public void LoadDisplayMode()
    {
        if (PlayerPrefs.HasKey("DisplayValue"))
        {
            displayDropdown.value = PlayerPrefs.GetInt("DisplayValue");
        }
    }
}

