using Cinemachine;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceController : MonoBehaviour
{
    public CameraController cameraController;
    public CanvasContoller canvasController;
    public Settings settings;
    public float targetFOV = 15f;

    public GameObject actionButtonGO;
    public Button actionButton;
    public TMP_Text actionButtonText;

    public GameObject scoreBox;
    public TMP_Text totalText;
    public TMP_Text highText;
    public TMP_Text lowText;

    public GameObject D4Prefab;
    public GameObject D6Prefab;
    public GameObject D8Prefab;
    public GameObject D10Prefab;
    public GameObject D12Prefab;
    public GameObject D20Prefab;
    public GameObject numberIdentifier;

    private List<GameObject> diceToRoll = new List<GameObject>();
    private List<GameObject> activeDice = new List<GameObject>();
    public List<GameObject> cockedDie = new List<GameObject>();

    private bool cockedCheck;

    public int D4count = 0;
    public int D6count = 0;
    public int D8count = 0;
    public int D10count = 0;
    public int D12count = 0;
    public int D20count = 0;

    public GameObject[] StartPositions;
    private GameObject activeStart;
    private GameObject prevActiveStart;

    public bool isRolled;
    private bool allSettled;

    private int total;
    private int high;
    private int low;

    void Start()
    {
        actionButton = actionButtonGO.GetComponent<Button>();

        actionButton.onClick.AddListener(ThrowDice);

        total = 0;
        high = 0;
        low = 1000;
        isRolled = false;
        allSettled = false;
        cockedCheck = false;
    }


    void Update()
    {
        if (diceToRoll.Count == 0)
        {
            activeStart = null;
        }
        else
        {
            for (int i = 0; i < StartPositions.Length; i++)
            {
                if (i == diceToRoll.Count - 1)
                {
                    StartPositions[i].SetActive(true);
                    activeStart = StartPositions[i];
                }
                else
                {
                    StartPositions[i].SetActive(false);
                }
            }
        }

        if (prevActiveStart != activeStart)
        {
            if (prevActiveStart != null)
            {
                DestroyDice(prevActiveStart);
            }

            InstantiateDice();

            prevActiveStart = activeStart;
        }


        if (isRolled)
        {
            foreach (GameObject die in activeDice)
            {
                SingleDieController controller = die.GetComponent<SingleDieController>();
                if (!controller.settled)
                {
                    return;
                }
            }

            if (!cockedCheck)
            {
                foreach (GameObject die in activeDice)
                {
                    cameraController.AddTargetToTargetGroup(die.transform);
                    SingleDieController controller = die.GetComponent<SingleDieController>();
                    if (controller.isCocked)
                    {
                        cockedDie.Add(die);
                    }
                }
                cockedCheck = true;
            }
            else
            {
                if (cockedDie.Count == 0)
                {
                    if (total == 0)
                    {
                        foreach (GameObject die in activeDice)
                        {
                            SingleDieController controller = die.GetComponent<SingleDieController>();
                            total = total + controller.rolledNumber;
                            if (controller.rolledNumber > high)
                            {
                                high = controller.rolledNumber;
                            }

                            if (controller.rolledNumber < low)
                            {
                                low = controller.rolledNumber;
                            }
                        }
                        cameraController.cameraSelect = 3;
                        actionButtonText.text = "RESET";
                        actionButton.onClick.RemoveAllListeners();
                        actionButton.onClick.AddListener(ResetDice);
                        actionButtonGO.SetActive(true);

                        totalText.text = total.ToString();
                        highText.text = high.ToString();
                        lowText.text = low.ToString();
                        scoreBox.SetActive(true);
                    }
                }
                else
                {
                    cameraController.cameraSelect = 3;
                    if (settings.rerollCockedOnly)
                    {
                        actionButtonText.text = cockedDie.Count + " COCKED!";
                        actionButton.onClick.RemoveAllListeners();
                        actionButton.onClick.AddListener(ResetCocked);
                        actionButtonGO.SetActive(true);
                    } else
                    {
                        actionButtonText.text = "REROLL";
                        actionButton.onClick.RemoveAllListeners();
                        actionButton.onClick.AddListener(ResetDice);
                        actionButtonGO.SetActive(true);
                    }
                    totalText.text = "!";
                    highText.text = "!";
                    lowText.text = "!";
                    scoreBox.SetActive(true);
                }
            }
        }
        else
        {
            actionButtonText.text = "ROLL";
            actionButtonGO.SetActive(diceToRoll.Count > 0);
        }


    }

    public void ThrowDice()
    {
        // Roll
        actionButtonGO.SetActive(false);
        cameraController.cameraSelect = 2;

        // Roll Dice
        foreach (GameObject die in activeDice)
        {
            SingleDieController controller = die.GetComponent<SingleDieController>();
            controller.ThrowDie();
        }

        isRolled = true;

    }

    public void ResetDice()
    {
        // Reset
        cockedCheck = false;
        cockedDie.Clear();
        allSettled = false;
        total = 0;
        high = 0;
        low = 1000;
        scoreBox.SetActive(false);
        cameraController.cameraSelect = 1;
        cameraController.ClearGroup();

        // Reset Dice
        foreach (GameObject die in activeDice)
        {
            SingleDieController controller = die.GetComponent<SingleDieController>();
            controller.ResetDie();
        }
        isRolled = false;
        actionButtonText.text = "ROLL";
        actionButton.onClick.RemoveAllListeners();
        actionButton.onClick.AddListener(ThrowDice);
    }

    public void ReRollCocked()
    {
        // Roll
        actionButtonGO.SetActive(false);
        cameraController.cameraSelect = 2;

        // Roll Dice
        foreach (GameObject die in cockedDie)
        {
            SingleDieController controller = die.GetComponent<SingleDieController>();
            controller.ThrowDie();
        }
        isRolled = true;
        cockedDie.Clear();
    }


    public void ResetCocked()
    {
        // Reset
        cockedCheck = false;
        allSettled = false;
        total = 0;
        high = 0;
        low = 1000;
        scoreBox.SetActive(false);
        cameraController.cameraSelect = 1;
        cameraController.ClearGroup();

        // Reset Dice
        foreach (GameObject die in cockedDie)
        {
            SingleDieController controller = die.GetComponent<SingleDieController>();
            controller.ResetDie();
        }
        isRolled = false;

        actionButtonText.text = "ROLL";
        actionButton.onClick.RemoveAllListeners();
        actionButton.onClick.AddListener(ReRollCocked);

    }

    private void DestroyDice(GameObject activeStart)
    {
        foreach (Transform startPos in activeStart.transform)
        {
            Destroy(startPos.transform.GetChild(0).gameObject);
            Destroy(startPos.transform.GetChild(1).gameObject);
        }
        activeDice.Clear();
    }

    private void InstantiateDice()
    {
        for (int i = 0; i < diceToRoll.Count; i++)
        {
            GameObject newDie = Instantiate(diceToRoll[i]);
            GameObject _numberIdentifier = Instantiate(numberIdentifier);
            newDie.transform.SetParent(activeStart.transform.GetChild(i), false);
            _numberIdentifier.transform.SetParent(activeStart.transform.GetChild(i), false);
            newDie.transform.position = activeStart.transform.GetChild(i).position;
            newDie.transform.rotation = new Quaternion(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            activeDice.Add(newDie);
        }
    }

    public void AddD4()
    {
        if (diceToRoll.Count < 8)
        {
            diceToRoll.Add(D4Prefab);
            D4count = D4count + 1;
        }
    }

    public void RemoveD4()
    {
        for (int i = diceToRoll.Count - 1; i > -1; i--)
        {
            if (diceToRoll[i] == D4Prefab)
            {
                diceToRoll.RemoveAt(i);
                D4count = D4count - 1;
                return;
            }
        }
    }
    public void AddD6()
    {
        if (diceToRoll.Count < 8)
        {
            diceToRoll.Add(D6Prefab);
            D6count = D6count + 1;
        }
    }

    public void RemoveD6()
    {
        for (int i = diceToRoll.Count - 1; i > -1; i--)
        {
            if (diceToRoll[i] == D6Prefab)
            {
                diceToRoll.RemoveAt(i);
                D6count = D6count - 1;
                return;
            }
        }
    }
    public void AddD8()
    {
        if (diceToRoll.Count < 8)
        {
            diceToRoll.Add(D8Prefab);
            D8count = D8count + 1;
        }
    }

    public void RemoveD8()
    {
        for (int i = diceToRoll.Count - 1; i > -1; i--)
        {
            if (diceToRoll[i] == D8Prefab)
            {
                diceToRoll.RemoveAt(i);
                D8count = D8count - 1;
                return;
            }
        }
    }
    public void AddD10()
    {   
        if (diceToRoll.Count < 8)
        {
            diceToRoll.Add(D10Prefab);
            D10count = D10count + 1;
        }
    }

    public void RemoveD10()
    {
        for (int i = diceToRoll.Count - 1; i > -1; i--)
        {
            if (diceToRoll[i] == D10Prefab)
            {
                diceToRoll.RemoveAt(i);
                D10count = D10count - 1;
                return;
            }
        }
    }
    public void AddD12()
    {
        if (diceToRoll.Count < 8)
        {
            diceToRoll.Add(D12Prefab);
            D12count = D12count + 1;
        }
    }

    public void RemoveD12()
    {
        for (int i = diceToRoll.Count - 1; i > -1; i--)
        {
            if (diceToRoll[i] == D12Prefab)
            {
                diceToRoll.RemoveAt(i);
                D12count = D12count - 1;
                return;
            }
        }
    }
    public void AddD20()
    {
        if (diceToRoll.Count < 8)
        {
            diceToRoll.Add(D20Prefab);
            D20count = D20count + 1;
        }
    }

    public void RemoveD20()
    {
        for (int i = diceToRoll.Count - 1; i > -1; i--)
        {
            if (diceToRoll[i] == D20Prefab)
            {
                diceToRoll.RemoveAt(i);
                D20count = D20count - 1;
                return;
            }
        }
    }

    public void ResetDiceToZero()
    {
        diceToRoll.Clear();
        D4count = D6count = D8count = D10count = D12count = D20count = 0;
    }

}
