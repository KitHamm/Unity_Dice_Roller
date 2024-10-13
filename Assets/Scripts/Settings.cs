using UnityEngine;

public class Settings : MonoBehaviour
{
    public int targetFrameRate = 61;

    public bool showHigh;
    public bool showLow;
    public bool rerollCockedOnly;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = targetFrameRate;
        showHigh = true;
        showLow = true;
        rerollCockedOnly = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ToggleShowHigh()
    {
        showHigh = !showHigh;
    }

    public void ToggleShowLow()
    {
        showLow = !showLow;
    }
    public void ToggleReRollCockedOnly()
    {
        rerollCockedOnly = !rerollCockedOnly;
    }
}
