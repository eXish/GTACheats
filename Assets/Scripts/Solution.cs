using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KModkit;

public class Solution : MonoBehaviour
{

    public KMBombInfo Bomb;
    public CheatSheet Cheats;
    public string[] input_code;
    public int value;
    private string name;


    public void FindSolution(int starsNumber, int stars2counter)
    {

        switch (starsNumber)
        {
            case 5:
                FindSolution5();
                break;
            case 4:
                FindSolution4();
                break;
            case 3:
                FindSolution3();
                break;
            default:
                FindSolution2Less(stars2counter);
                break;
        }


        Debug.Log("Correct Cheat Name: " + name);
        Debug.Log("Correct Input Code: " + input_code.Join(","));

    }

    private bool HasEmptyPortPlate()
    {

        foreach (object[] plate in Bomb.GetPortPlates())
        {
            if (plate.Count() == 0)
            {
                return true;
            }
        }
        return false;

    }

    private bool HasOddNumber()
    {
        foreach (int number in Bomb.GetSerialNumberNumbers())
        {
            if (number % 2 == 1)
            {
                return true;
            }
        }
        return false;
    }

    private void FindSolution5()
    {
        bool BOB = Bomb.IsIndicatorPresent(Indicator.BOB);
        bool CAR = Bomb.IsIndicatorPresent(Indicator.CAR) && Bomb.IsIndicatorOff(Indicator.CAR);
        bool emptyPortPlate = HasEmptyPortPlate();
        bool odd = HasOddNumber();


        if (BOB && CAR)
        {
            value = Cheats.PAINKILLER_VALUE;
            name = "PAINKILLER";
            input_code = Cheats.PAINKILLER_INPUT;
            Debug.Log("BOB AND CAR");
        }
        else if (emptyPortPlate)
        {
            value = Cheats.HOTHANDS_VALUE;
            name = "HOTHANDS";
            input_code = Cheats.HOTHANDS_INPUT;
            Debug.Log("EMPTY PORT PLATE");
        }
        else if (odd)
        {
            value = Cheats.BUZZOFF_VALUE;
            name = "BUZZOFF";
            input_code = Cheats.BUZZOFF_INPUT;
            Debug.Log("ODD NUMBER IN THE SERIAL");
        }
        else
        {
            value = Cheats.LAWYERUP_VALUE;
            name = "LAWYERUP";
            input_code = Cheats.LAWYERUP_INPUT;
            Debug.Log("NOTHING");
        }
    }

    private void FindSolution4()
    {

        bool DVI = Bomb.IsPortPresent(Port.DVI);
        bool PS = Bomb.IsPortPresent(Port.PS2);
        bool Parallel = Bomb.IsPortPresent(Port.Parallel);
        bool D2Batteries = Bomb.GetBatteryCount(Battery.D) >= 2;
        bool Indicators = Bomb.GetOnIndicators().Count() >= 2;

        if (DVI && PS)
        {
            value = Cheats.TOOLUP_VALUE;
            name = "TOOLUP";
            input_code = Cheats.TOOLUP_INPUT;
            Debug.Log("DVI AND PS/2");
        }
        else if (Parallel || D2Batteries)
        {
            value = Cheats.HIGHEX_VALUE;
            name = "HIGHEX";
            input_code = Cheats.HIGHEX_INPUT;
            Debug.Log("PARALLEL OR 2 D BATTERIES");
        }
        else if (Indicators)
        {
            value = Cheats.RAPIDGT_VALUE;
            name = "RAPIDGT";
            input_code = Cheats.RAPIDGT_INPUT;
            Debug.Log("MORE THAN 2 LIT INDICATORS");
        }
        else
        {
            value = Cheats.LAWYERUP_VALUE;
            name = "LAWYERUP";
            input_code = Cheats.LAWYERUP_INPUT;
            Debug.Log("NOTHING");
        }
    }

    private void FindSolution3()
    {

        bool PortPlates = Bomb.GetPortPlateCount() >= 3;
        bool Lit_Unlit = Bomb.GetOnIndicators().Count() > Bomb.GetOffIndicators().Count();
        bool NoInd = Bomb.GetIndicators().Count() == 0;

        if (PortPlates)
        {
            value = Cheats.TURTLE_VALUE;
            name = "TURTLE";
            input_code = Cheats.TURTLE_INPUT;
            Debug.Log("3 OR MORE PORT PLATES");
        }
        else if (Lit_Unlit)
        {
            value = Cheats.GOTGILLS_JUMP_VALUE;
            name = "GOTGILLS_JUMP";
            input_code = Cheats.GOTGILLS_JUMP_INPUT;
            Debug.Log("MORE LIT THAN UNLIT INDICATORS");
        }
        else if (NoInd)
        {
            value = Cheats.GOTGILLS_SWIM_VALUE;
            name = "GOTGILLS_SWIM";
            input_code = Cheats.GOTGILLS_SWIM_INPUT;
            Debug.Log("NO INDICATORS");
        }
        else
        {
            value = Cheats.LAWYERUP_VALUE;
            name = "LAWYERUP";
            input_code = Cheats.LAWYERUP_INPUT;
            Debug.Log("NOTHING");
        }
    }

    private bool IsVowelInAnyIndicator()
    {
        foreach (string ind in Bomb.GetIndicators())
        {
            IEnumerable<Char> intersect = ind.Intersect("AIEOU");
            if (intersect.Count() > 0) return true;
        }
        return false;
    }

    private bool matchingLettersIndicatorsSerial()
    {

        foreach (string ind in Bomb.GetIndicators())
        {
            IEnumerable<Char> intersect = ind.Intersect(Bomb.GetSerialNumberLetters());
            if (intersect.Count() > 0) return true;
        }
        return false;

    }

    private void FindSolution2Less(int stars2counter)
    {

        int numberOfNumbersInSerial = Bomb.GetSerialNumberNumbers().Count();
        int numberOfLettersInSerial = Bomb.GetSerialNumberLetters().Count();
        int pairsOfAA = (int)(Bomb.GetBatteryCount(Battery.AA) / 2);
        bool stereo = Bomb.IsPortPresent(Port.StereoRCA);
        bool RJ45 = Bomb.IsPortPresent(Port.RJ45);
        bool Serial = Bomb.IsPortPresent(Port.Serial);
        bool vowelInAnyIndicator = IsVowelInAnyIndicator();
        bool ind_ser = matchingLettersIndicatorsSerial();

        Debug.Log("Occourrence #" + stars2counter);

        if (stars2counter == 1)
        {
            if (numberOfNumbersInSerial > numberOfLettersInSerial)
            {
                value = Cheats.CATCHME_VALUE;
                name = "CATCHME";
                input_code = Cheats.CATCHME_INPUT;
                Debug.Log("MORE DIGITS THAN LETTERS IN THE SERIAL");
            }
            else if (pairsOfAA >= 2)
            {
                value = Cheats.INCENDIARY_VALUE;
                name = "INCENDIARY";
                input_code = Cheats.INCENDIARY_INPUT;
                Debug.Log("MORE THAN 2 PAIRS OF AA BATTERIES");
            }
            else if (stereo)
            {
                value = Cheats.BANDIT_VALUE;
                name = "BANDIT";
                input_code = Cheats.BANDIT_INPUT;
                Debug.Log("STEREO RCA PORT");
            }
            else
            {
                value = Cheats.LAWYERUP_VALUE;
                name = "LAWYERUP";
                input_code = Cheats.LAWYERUP_INPUT;
                Debug.Log("NOTHING");
            }
        }
        else if (stars2counter == 2)
        {
            if (ind_ser)
            {
                value = Cheats.BANDIT_VALUE;
                name = "BANDIT";
                input_code = Cheats.BANDIT_INPUT;
                Debug.Log("AN INDICATOR HAS A MATCHING LETTER WITH THE SERIAL");
            }
            else if (vowelInAnyIndicator)
            {
                value = Cheats.CATCHME_VALUE;
                name = "CATCHME";
                input_code = Cheats.CATCHME_INPUT;
                Debug.Log("AN INDICATOR HAS A VOWEL");
            }
            else if (numberOfLettersInSerial > numberOfNumbersInSerial)
            {
                value = Cheats.INCENDIARY_VALUE;
                name = "INCENDIARY";
                input_code = Cheats.INCENDIARY_INPUT;
                Debug.Log("MORE LETTERS THAN DIGITS IN THE SERIAL");
            }
            else
            {
                value = Cheats.LAWYERUP_VALUE;
                name = "LAWYERUP";
                input_code = Cheats.LAWYERUP_INPUT;
                Debug.Log("NOTHING");
            }
        }
        else
        {
            if (numberOfLettersInSerial == numberOfNumbersInSerial)
            {
                value = Cheats.INCENDIARY_VALUE;
                name = "INCENDIARY";
                input_code = Cheats.INCENDIARY_INPUT;
                Debug.Log("SAME NUMBER OF DIGITS AND LETTERS IN THE SERIAL");
            }
            else if (RJ45)
            {
                value = Cheats.BANDIT_VALUE;
                name = "BANDIT";
                input_code = Cheats.BANDIT_INPUT;
                Debug.Log("RJ45");
            }
            else if (Serial)
            {
                value = Cheats.CATCHME_VALUE;
                name = "CATCHME";
                input_code = Cheats.CATCHME_INPUT;
                Debug.Log("SERIAL PORT");
            }
            else
            {
                value = Cheats.LAWYERUP_VALUE;
                name = "LAWYERUP";
                input_code = Cheats.LAWYERUP_INPUT;
                Debug.Log("NOTHING");
            }
        }

    }

}
