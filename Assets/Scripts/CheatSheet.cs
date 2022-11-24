using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KModkit;


public class CheatSheet : MonoBehaviour
{

    public string[] PAINKILLER_INPUT = { "Right", "X", "Right", "Left", "Right", "R1", "Right", "Left", "X", "Triangle" };
    public string[] CATCHME_INPUT = { "Triangle", "Left", "Right", "Right", "L2", "L1", "Square" };
    public string[] TURTLE_INPUT = { "Circle", "L1", "Triangle", "R2", "X", "Square", "Circle", "Right", "Square", "L1", "L1", "L1" };
    public string[] HIGHEX_INPUT = { "Right", "Square", "X", "Left", "R1", "R2", "Left", "Right", "Right", "L1", "L1", "L1" };
    public string[] LAWYERUP_INPUT = { "R1", "R1", "Circle", "R2", "Right", "Left", "Right", "Left", "Right", "Left" };
    public string[] TOOLUP_INPUT = { "Triangle", "R2", "Left", "L1", "X", "Right", "Triangle", "Down", "Square", "L1", "L1", "L1" };
    public string[] RAPIDGT_INPUT = { "R2", "L1", "Circle", "Right", "L1", "R1", "Right", "Left", "Circle", "R2" };
    public string[] BUZZOFF_INPUT = { "Circle", "Circle", "L1", "Circle", "Circle", "Circle", "L1", "L2", "R1", "Triangle", "Circle", "Triangle" };
    public string[] HOTHANDS_INPUT = { "Right", "Left", "X", "Triangle", "R1", "Circle", "Circle", "Circle", "L2" };
    public string[] GOTGILLS_JUMP_INPUT = { "L2", "L2", "Square", "Circle", "Circle", "L2", "Square", "Square", "Left", "Right", "X" };
    public string[] GOTGILLS_SWIM_INPUT = { "Left", "Left", "L1", "Right", "Right", "R2", "Left", "L2", "Right" };
    public string[] INCENDIARY_INPUT = { "L1", "R1", "Square", "R1", "Left", "R2", "R1", "Left", "Square", "Right", "L1", "L1" };
    public string[] BANDIT_INPUT = { "Left", "Left", "Right", "Right", "Left", "Right", "Square", "Circle", "Triangle", "R1", "R2" };

    public int PAINKILLER_VALUE = 3;
    public int CATCHME_VALUE = 1;
    public int TURTLE_VALUE = 2;
    public int HIGHEX_VALUE = 2;
    public int LAWYERUP_VALUE = 1;
    public int TOOLUP_VALUE = 2;
    public int RAPIDGT_VALUE = 1;
    public int BUZZOFF_VALUE = 2;
    public int HOTHANDS_VALUE = 2;
    public int GOTGILLS_JUMP_VALUE = 1;
    public int GOTGILLS_SWIM_VALUE = 1;
    public int INCENDIARY_VALUE = 1;
    public int BANDIT_VALUE = 1;


    public string GetInputName(string[] input)
    {
        if (Enumerable.SequenceEqual(INCENDIARY_INPUT, input)){
            return "incendiary";
        }
        else if (Enumerable.SequenceEqual(BANDIT_INPUT, input)){
            return "bandit";
        }
        else if (Enumerable.SequenceEqual(HIGHEX_INPUT, input)){
            return "highex";
        }
        else if (Enumerable.SequenceEqual(TURTLE_INPUT, input)){
            return "turtle";
        }
        else if (Enumerable.SequenceEqual(TOOLUP_INPUT, input)){
            return "toolup";
        }
        else if (Enumerable.SequenceEqual(BUZZOFF_INPUT, input)){
            return "buzzoff";
        }
        else if (Enumerable.SequenceEqual(CATCHME_INPUT, input)){
            return "catchme";
        }
        else if (Enumerable.SequenceEqual(RAPIDGT_INPUT, input)){
            return "rapidgt";
        }
        else if (Enumerable.SequenceEqual(GOTGILLS_JUMP_INPUT, input)){
            return "gotgills_jump";
        }
        else if (Enumerable.SequenceEqual(GOTGILLS_SWIM_INPUT, input)){
            return "gotgills_swim";
        }
        else if (Enumerable.SequenceEqual(LAWYERUP_INPUT, input)){
            return "lawyerup";
        }
        else if (Enumerable.SequenceEqual(HOTHANDS_INPUT, input)){
            return "hothands";
        }
        else{
            return "painkiller";
        }
    }
}

