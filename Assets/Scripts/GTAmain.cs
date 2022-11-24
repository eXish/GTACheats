using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KModkit;

public class GTAmain : MonoBehaviour
{

    public CheatSheet CheatSheet;
    public Solution Solution;
    public KMBombModule Module;
    public KMBombInfo Bomb;
    public int stars2Counter = 0;
    public KMAudio Audio;
    int moduleId;
    static int moduleIdCounter = 1;
    private bool moduleSolved;
    private System.Random rnd = new System.Random();
    private int starsNumber;
    public GameObject[] Stars;
    public KMSelectable[] Buttons;
    private int inputIndex = 0;
    private bool plus_one;
    private int value;
    private string[] input_code;


    void Awake()
    {
        moduleId = moduleIdCounter++;

        starsNumber = rnd.Next(3, 6);
        HandleStars(starsNumber);

        foreach (KMSelectable button in Buttons)
        {
            button.OnInteract += delegate () { PressButton(button.name, input_code, value); return false; };
        }
    }

    // Use this for initialization
    void Start()
    {
        Init();
    }

    void Init()
    {
        Solution.FindSolution(starsNumber, stars2Counter);
        value = Solution.value;
        input_code = Solution.input_code;
    }

    void PressButton(string name, string[] input, int value)
    {

        Audio.PlaySoundAtTransform("buttonpress", transform);
        if (inputIndex == 0)
        {
            plus_one = checkSecond();
        }
        if (name == input[inputIndex])
        {

            ++inputIndex;

            if (inputIndex == input.Length)
            {
                Audio.PlaySoundAtTransform(CheatSheet.GetInputName(input), transform);
                StartCoroutine(BlinkStars(input));
            }

        }
        else
        {
            Debug.Log("Pressed button: " + name);
            Debug.Log("Correct button: " + input[inputIndex]);
            Audio.PlaySoundAtTransform("strike", transform);
            Module.HandleStrike();
            HandleStars(starsNumber + 1);
            inputIndex = 0;
            Init();
        }
    }

    void HandleStars(int value)
    {
        if (value > 5)
        {
            value = 5;
        }

        UnlitStars();
        LitStars(value);

        if (value == 0)
        {
            Audio.PlaySoundAtTransform("pass", transform);
            Module.HandlePass();
        }
        if (value <= 2) stars2Counter++;

        starsNumber = Math.Abs(value);
        Debug.Log("Number Of Stars: " + starsNumber);

    }

    void NextStage(string[] input)
    {
        inputIndex = 0;
        Init();
    }

    bool checkSecond()
    {

        int seconds = (int)Bomb.GetTime() % 60;
        int maxSerialNumberDigit = Bomb.GetSerialNumberNumbers().OrderByDescending(item => item).First();
        int batteryHolders = Bomb.GetBatteryHolderCount();
        int batteries = Bomb.GetBatteryCount();

        int bonusNumber = maxSerialNumberDigit + (batteries * starsNumber) - batteryHolders;

        while (bonusNumber >= 10)
        {
            bonusNumber = ((int)(bonusNumber / 10)) + (bonusNumber % 10);
        }

        Debug.Log("Bonus Number: " + bonusNumber);

        if (bonusNumber == ((int)seconds / 10) || bonusNumber == (seconds % 10))
        {
            return true;
        }

        return false;
    }

    void UnlitStars(){
        foreach (GameObject Star in Stars)
        {
            Star.SetActive(false);
        }
    }

    void LitStars(int value){
        value = Math.Abs(value);
        for (int i = 0; i < value; i++)
        {
            Stars[i].SetActive(true);
        }
    }

    IEnumerator BlinkStars(string[] input)
    {
        if (plus_one){
            value++;
            Debug.Log("Bonus Obtained");
        }
        for(int i=1;i<=value;i++){
            UnlitStars();
            yield return new WaitForSecondsRealtime(0.3f);
            LitStars(starsNumber-i);
            yield return new WaitForSecondsRealtime(0.3f);
        }
        HandleStars(starsNumber-value);
        NextStage(input);
    }
}
