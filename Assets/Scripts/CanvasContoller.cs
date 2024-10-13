using TMPro;
using UnityEngine;

public class CanvasContoller : MonoBehaviour
{
    public DiceController diceController;
    public Settings settings;

    public GameObject GameUI;
    public GameObject MenuUI;

    public GameObject Menu;
    public GameObject MenuButton;

    public GameObject noDiceWarning;

    public GameObject showHighCheck;
    public GameObject showLowCheck;
    public GameObject showRerollRuleCheck;

    public GameObject highDisplay;
    public GameObject lowDisplay;

    public GameObject actionButton;
    public GameObject ResetDiceButton;

    public TMP_Text D4Text;
    public TMP_Text D6Text;
    public TMP_Text D8Text;
    public TMP_Text D10Text;
    public TMP_Text D12Text;
    public TMP_Text D20Text;

    public bool menuOpen;

    private int diceCount;

    // Start is called before the first frame update
    void Start()
    {
        menuOpen = false;
        diceCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        D4Text.text = diceController.D4count.ToString();
        D6Text.text = diceController.D6count.ToString();
        D8Text.text = diceController.D8count.ToString();
        D10Text.text = diceController.D10count.ToString();
        D12Text.text = diceController.D12count.ToString();
        D20Text.text = diceController.D20count.ToString();

        if (diceController.isRolled)
        {
            MenuButton.SetActive(false);
        }
        else
        {
            MenuButton.SetActive(true);
        }

        diceCount = diceController.D4count + diceController.D6count + diceController.D8count + diceController.D10count + diceController.D12count + diceController.D20count;

        noDiceWarning.SetActive(diceCount == 0);

        showHighCheck.SetActive(settings.showHigh);
        showLowCheck.SetActive(settings.showLow);
        showRerollRuleCheck.SetActive(settings.rerollCockedOnly);
        highDisplay.SetActive(settings.showHigh);
        lowDisplay.SetActive(settings.showLow);
        MenuUI.SetActive(menuOpen);
        GameUI.SetActive(!menuOpen);
        ResetDiceButton.SetActive(diceCount > 0);
    }

    public void MenuToggle()
    {
        menuOpen = !menuOpen;
    }

}
