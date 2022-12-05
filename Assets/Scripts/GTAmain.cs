using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KModkit;
using System.Text.RegularExpressions;
using Rnd = UnityEngine.Random;

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
    private int starsNumber;
    public GameObject[] Stars;
    public KMSelectable[] Buttons;
    private int inputIndex = 0;
    private bool plus_one;
    private int value;
    private string[] input_code;
    private int bonusNumber;
    private bool isAnimating;

    // Use this for initialization
    void Start()
    {
        moduleId = moduleIdCounter++;

        starsNumber = Rnd.Range(3, 6);
        HandleStars(starsNumber);

        foreach (KMSelectable button in Buttons)
        {
            button.OnInteract += delegate ()
            {
                if (!moduleSolved || isAnimating)
                    PressButton(button.name, input_code);
                return false;
            };
        }
        Init();
    }

    void Init()
    {
        Solution.FindSolution(starsNumber, stars2Counter);
        value = Solution.value;
        input_code = Solution.input_code;
        bonusNumber = GetBonusNumber();
        Debug.LogFormat("[GTA Cheats #{0}] {1}", moduleId, Solution.reason);
        Debug.LogFormat("[GTA Cheats #{0}] Cheat name: {1}", moduleId, Solution.name);
        Debug.LogFormat("[GTA Cheats #{0}] This will remove {1} star{2}.", moduleId, value, value == 1 ? "" : "s");
        Debug.LogFormat("[GTA Cheats #{0}] Solution: {1}", moduleId, input_code.Join(", "));
        Debug.LogFormat("[GTA Cheats #{0}] Bonus number: {1}", moduleId, bonusNumber);
    }

    void PressButton(string name, string[] input)
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
            Debug.LogFormat("[GTA Cheats #{0}] Pressed {1} instead of {2}. Strike. ", moduleId, name, input[inputIndex]);
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

        starsNumber = Math.Abs(value);
        Debug.LogFormat("[GTA Cheats #{0}] Number Of Stars: {1}", moduleId, starsNumber);
        if (value == 0)
        {
            Audio.PlaySoundAtTransform("pass", transform);
            moduleSolved = true;
            Module.HandlePass();
            Debug.LogFormat("[GTA Cheats #{0}] Module solved.", moduleId);
            return;
        }
        if (value <= 2) stars2Counter++;

        NextStage();
    }

    void NextStage()
    {
        inputIndex = 0;
        Init();
    }

    bool checkSecond()
    {
        int bonusNumber = GetBonusNumber();
        if (bonusNumber == ((int)Bomb.GetTime() % 60 / 10) || bonusNumber == ((int)Bomb.GetTime() % 10))
            return true;
        return false;
    }

    private int GetBonusNumber()
    {
        int maxSerialNumberDigit = Bomb.GetSerialNumberNumbers().OrderByDescending(item => item).First();
        int batteryHolders = Bomb.GetBatteryHolderCount();
        int batteries = Bomb.GetBatteryCount();

        int bonusNumber = maxSerialNumberDigit + (batteries * starsNumber) - batteryHolders;

        while (bonusNumber >= 10)
            bonusNumber = ((int)(bonusNumber / 10)) + (bonusNumber % 10);
        return bonusNumber;
    }


    void UnlitStars()
    {
        foreach (GameObject Star in Stars)
        {
            Star.SetActive(false);
        }
    }

    void LitStars(int value)
    {
        value = Math.Abs(value);
        for (int i = 0; i < value; i++)
        {
            Stars[i].SetActive(true);
        }
    }

    IEnumerator BlinkStars(string[] input)
    {
        isAnimating = true;
        if (plus_one)
        {
            value++;
            Debug.LogFormat("[GTA Cheats #{0}] Bonus Obtained.", moduleId);
        }
        Debug.LogFormat("[GTA Cheats #{0}] Removed {1} star{2}.", moduleId, value, value == 1 ? "" : "s");
        for (int i = 1; i <= value; i++)
        {
            UnlitStars();
            yield return new WaitForSecondsRealtime(0.3f);
            LitStars(starsNumber - i);
            yield return new WaitForSecondsRealtime(0.3f);
        }
        HandleStars(starsNumber - value);
        isAnimating = false;
    }

#pragma warning disable 0414
    private readonly string TwitchHelpMessage = "!{0} press up, right, down, left [Press these buttons.] | !{0} press X at 4 [Press X when the seconds timer contains a 4.] | Commands must be chained with commas. | Buttons are Up, Right, Down, Left, Triangle, Circle, X, Square, L1, L2, R1, R2.";
#pragma warning restore 0414

    struct TpPress
    {
        public int Button;
        public int? Time;
        public TpPress(int button, int? time)
        {
            Button = button;
            Time = time;
        }
    }

    private static readonly string[] btnList = new string[] { "triangle", "x", "circle", "square", "r1", "l1", "l2", "r2", "right", "down", "up", "left" };

    private IEnumerator ProcessTwitchCommand(string command)
    {
        command = command.Trim().ToLowerInvariant();
        if (!command.StartsWith("press "))
        {
            yield return "sendtochaterror The command must start with 'press'!";
            yield break;
        }
        command = command.Substring(5);
        var parameters = command.Split(',');
        var list = new List<TpPress>();
        for (int i = 0; i < parameters.Length; i++)
        {
            var m = Regex.Match(parameters[i],
                @"^\s*(up|right|down|left|triangle|circle|X|square|l1|l2|r1|r2)(\s+at\s+(?<digit>\d))?\s*$",
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            if (!m.Success)
                yield break;
            int btn = Array.IndexOf(btnList, m.Groups[1].Value);
            var d = m.Groups["digit"].Value;
            int? time;
            if (d == "")
                time = null;
            else
                time = int.Parse(d);
            list.Add(new TpPress(btn, time));
        }
        yield return null;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Time != null)
                while ((int)Bomb.GetTime() % 60 / 10 != list[i].Time.Value && (int)Bomb.GetTime() % 10 != list[i].Time.Value)
                    yield return null;
            Buttons[list[i].Button].OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator TwitchHandleForcedSolve()
    {
        while (!moduleSolved)
        {
            var solution = input_code.Select(i => Array.IndexOf(btnList, i.ToLowerInvariant())).ToArray();
            for (int i = inputIndex; i < solution.Length; i++)
            {
                if (i == 0 && starsNumber > Solution.value)
                    while ((int)Bomb.GetTime() % 60 / 10 != bonusNumber && (int)Bomb.GetTime() % 10 != bonusNumber)
                        yield return true;
                Buttons[solution[i]].OnInteract();
                yield return new WaitForSeconds(0.1f);
            }
            while (isAnimating)
                yield return true;
        }
    }
}
