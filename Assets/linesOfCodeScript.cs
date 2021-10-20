using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;
using System.Text.RegularExpressions;

public class linesOfCodeScript : MonoBehaviour {

    public KMBombInfo Bomb;
    public KMAudio Audio;

    //Logging
    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    public TextMesh code;
    public KMSelectable[] numButtons;   // 0 1 2 3 4 5 6 7 8 9
    public KMSelectable[] otherButtons; // . - < E

    public List<int> varOrder = new List<int> { 0, 1, 2 };
    string lines = "";
    string digits = "0123456789";
    //string varNames = "xyz";
    //int forOrder = 0;
    int x = 0;
    int y = 0;
    int z = 0;
    int goal = 0;
    int final = 0;
    string strN = "0";
    int n = 0;
    int currentIx = 0;
    int Poss = 0;
    int Posse = 0;
    int linesLeft = 8;
    int rngA = 0;
    int rngB = 0;
    string strX = "";
    string strY = "";
    string strZ = "";
    bool goalFound = false;

    void Awake () {
        moduleId = moduleIdCounter++;
        foreach (KMSelectable num in numButtons) {
            KMSelectable pressedNum = num;
            num.OnInteract += delegate () { numPress(pressedNum); return false; };
        }

        foreach (KMSelectable other in otherButtons) {
            KMSelectable pressedOther = other;
            other.OnInteract += delegate () { otherPress(pressedOther); return false; };
        }

    }

    // Use this for initialization
    void Start () {
        GenerateFullPuzzle();
	}

    void GenerateFullPuzzle ()
    {
        lines = "int n = #;"; //Replace the # later with the number the user typed!

        x = UnityEngine.Random.Range(0, 10);
        y = UnityEngine.Random.Range(0, 10);
        z = UnityEngine.Random.Range(0, 10);

        //lines += String.Format("\nint {0} = {1};", varNames[j], forOrder.ToString());
        lines += String.Format("\nint x = {0};\nint y = {1};\nint z = {2};", x, y, z);

        strX = "x = " + x;
        strY = "y = " + y;
        strZ = "z = " + z;

        GenerateLines();

        GenerateGoal();

        lines += "\n  Pass();\n} else {\n  Strike();\n}";

        code.text = lines.Replace("#", strN);

        Debug.LogFormat("[Lines of Code #{0}] The code: \'{1}\'", moduleId, lines.Replace("\n", " "));
        Debug.LogFormat("[Lines of Code #{0}] {1}", moduleId, strX);
        Debug.LogFormat("[Lines of Code #{0}] {1}", moduleId, strY);
        Debug.LogFormat("[Lines of Code #{0}] {1}", moduleId, strZ);
        Debug.LogFormat("[Lines of Code #{0}] To have the code run correctly, n must be {1}.", moduleId, goal);
    }

    void GenerateLines ()
    {
        while (linesLeft != 0)
        {
            Poss = UnityEngine.Random.Range(0, 342);
            rngA = UnityEngine.Random.Range(0, 10);
            rngB = UnityEngine.Random.Range(0, 10);

            switch (Poss)
            {
                case 0: lines += String.Format("\nx = y;"); x = y; UpdateStrings(); linesLeft -= 1; break;
                case 1: lines += String.Format("\nx = z;"); x = z; UpdateStrings(); linesLeft -= 1; break;
                case 2: lines += String.Format("\nx = x + {0};", rngA); x = x + rngA; UpdateStrings(); linesLeft -= 1; break;
                case 3: lines += String.Format("\nx = y + {0};", rngA); x = y + rngA; UpdateStrings(); linesLeft -= 1; break;
                case 4: lines += String.Format("\nx = z + {0};", rngA); x = z + rngA; UpdateStrings(); linesLeft -= 1; break;
                case 5: lines += String.Format("\nx = {0} + x;", rngA); x = rngA + x; UpdateStrings(); linesLeft -= 1; break;
                case 6: lines += String.Format("\nx = {0} + y;", rngA); x = rngA + y; UpdateStrings(); linesLeft -= 1; break;
                case 7: lines += String.Format("\nx = {0} + z;", rngA); x = rngA + z; UpdateStrings(); linesLeft -= 1; break;
                case 8: lines += String.Format("\nx = {0} + {1};", rngA, rngB); x = rngA + rngB; UpdateStrings(); linesLeft -= 1; break;
                case 9: lines += String.Format("\nx = x - {0};", rngA); x = x - rngA; UpdateStrings(); linesLeft -= 1; break;
                case 10: lines += String.Format("\nx = y - {0};", rngA); x = y - rngA; UpdateStrings(); linesLeft -= 1; break;
                case 11: lines += String.Format("\nx = z - {0};", rngA); x = z - rngA; UpdateStrings(); linesLeft -= 1; break;
                case 12: lines += String.Format("\nx = {0} - x;", rngA); x = rngA - x; UpdateStrings(); linesLeft -= 1; break;
                case 13: lines += String.Format("\nx = {0} - y;", rngA); x = rngA - y; UpdateStrings(); linesLeft -= 1; break;
                case 14: lines += String.Format("\nx = {0} - z;", rngA); x = rngA - z; UpdateStrings(); linesLeft -= 1; break;
                case 15: lines += String.Format("\nx = {0} - {1};", rngA, rngB); x = rngA - rngB; UpdateStrings(); linesLeft -= 1; break;
                case 16: lines += String.Format("\nx = x * {0};", rngA); x = x * rngA; UpdateStrings(); linesLeft -= 1; break;
                case 17: lines += String.Format("\nx = y * {0};", rngA); x = y * rngA; UpdateStrings(); linesLeft -= 1; break;
                case 18: lines += String.Format("\nx = z * {0};", rngA); x = z * rngA; UpdateStrings(); linesLeft -= 1; break;
                case 19: lines += String.Format("\nx = {0} * x;", rngA); x = rngA * x; UpdateStrings(); linesLeft -= 1; break;
                case 20: lines += String.Format("\nx = {0} * y;", rngA); x = rngA * y; UpdateStrings(); linesLeft -= 1; break;
                case 21: lines += String.Format("\nx = {0} * z;", rngA); x = rngA * z; UpdateStrings(); linesLeft -= 1; break;
                case 22: lines += String.Format("\nx = {0} * {1};", rngA, rngB); x = rngA * rngB; UpdateStrings(); linesLeft -= 1; break;
                case 23: if (rngA == 0) { break; } lines += String.Format("\nx = x / {0};", rngA); x = x / rngA; UpdateStrings(); linesLeft -= 1; break;
                case 24: if (rngA == 0) { break; } lines += String.Format("\nx = y / {0};", rngA); x = y / rngA; UpdateStrings(); linesLeft -= 1; break;
                case 25: if (rngA == 0) { break; } lines += String.Format("\nx = z / {0};", rngA); x = z / rngA; UpdateStrings(); linesLeft -= 1; break;
                case 26: if (x == 0) { break; } lines += String.Format("\nx = {0} / x;", rngA); x = rngA / x; UpdateStrings(); linesLeft -= 1; break;
                case 27: if (y == 0) { break; } lines += String.Format("\nx = {0} / y;", rngA); x = rngA / y; UpdateStrings(); linesLeft -= 1; break;
                case 28: if (z == 0) { break; } lines += String.Format("\nx = {0} / z;", rngA); x = rngA / z; UpdateStrings(); linesLeft -= 1; break;
                case 29: if (rngB == 0) { break; } lines += String.Format("\nx = {0} / {1};", rngA, rngB); x = rngA / rngB; UpdateStrings(); linesLeft -= 1; break;
                case 30: if (rngA == 0) { break; } lines += String.Format("\nx = x % {0};", rngA); x = x % rngA; UpdateStrings(); linesLeft -= 1; break;
                case 31: if (rngA == 0) { break; } lines += String.Format("\nx = y % {0};", rngA); x = y % rngA; UpdateStrings(); linesLeft -= 1; break;
                case 32: if (rngA == 0) { break; } lines += String.Format("\nx = z % {0};", rngA); x = z % rngA; UpdateStrings(); linesLeft -= 1; break;
                case 33: if (x == 0) { break; } lines += String.Format("\nx = {0} % x;", rngA); x = rngA % x; UpdateStrings(); linesLeft -= 1; break;
                case 34: if (y == 0) { break; } lines += String.Format("\nx = {0} % y;", rngA); x = rngA % y; UpdateStrings(); linesLeft -= 1; break;
                case 35: if (z == 0) { break; } lines += String.Format("\nx = {0} % z;", rngA); x = rngA % z; UpdateStrings(); linesLeft -= 1; break;
                case 36: if (rngB == 0) { break; } lines += String.Format("\nx = {0} % {1};", rngA, rngB); x = rngA % rngB; UpdateStrings(); linesLeft -= 1; break;
                case 37: lines += String.Format("\nx = x & {0};", rngA); x = x & rngA; UpdateStrings(); linesLeft -= 1; break;
                case 38: lines += String.Format("\nx = y & {0};", rngA); x = y & rngA; UpdateStrings(); linesLeft -= 1; break;
                case 39: lines += String.Format("\nx = z & {0};", rngA); x = z & rngA; UpdateStrings(); linesLeft -= 1; break;
                case 40: lines += String.Format("\nx = {0} & x;", rngA); x = rngA & x; UpdateStrings(); linesLeft -= 1; break;
                case 41: lines += String.Format("\nx = {0} & y;", rngA); x = rngA & y; UpdateStrings(); linesLeft -= 1; break;
                case 42: lines += String.Format("\nx = {0} & z;", rngA); x = rngA & z; UpdateStrings(); linesLeft -= 1; break;
                case 43: lines += String.Format("\nx = {0} & {1};", rngA, rngB); x = rngA & rngB; UpdateStrings(); linesLeft -= 1; break;
                case 44: lines += String.Format("\nx = x | {0};", rngA); x = x | rngA; UpdateStrings(); linesLeft -= 1; break;
                case 45: lines += String.Format("\nx = y | {0};", rngA); x = y | rngA; UpdateStrings(); linesLeft -= 1; break;
                case 46: lines += String.Format("\nx = z | {0};", rngA); x = z | rngA; UpdateStrings(); linesLeft -= 1; break;
                case 47: lines += String.Format("\nx = {0} | x;", rngA); x = rngA | x; UpdateStrings(); linesLeft -= 1; break;
                case 48: lines += String.Format("\nx = {0} | y;", rngA); x = rngA | y; UpdateStrings(); linesLeft -= 1; break;
                case 49: lines += String.Format("\nx = {0} | z;", rngA); x = rngA | z; UpdateStrings(); linesLeft -= 1; break;
                case 50: lines += String.Format("\nx = {0} | {1};", rngA, rngB); x = rngA | rngB; UpdateStrings(); linesLeft -= 1; break;
                case 51: lines += String.Format("\nx = x ^ {0};", rngA); x = x ^ rngA; UpdateStrings(); linesLeft -= 1; break;
                case 52: lines += String.Format("\nx = y ^ {0};", rngA); x = y ^ rngA; UpdateStrings(); linesLeft -= 1; break;
                case 53: lines += String.Format("\nx = z ^ {0};", rngA); x = z ^ rngA; UpdateStrings(); linesLeft -= 1; break;
                case 54: lines += String.Format("\nx = {0} ^ x;", rngA); x = rngA ^ x; UpdateStrings(); linesLeft -= 1; break;
                case 55: lines += String.Format("\nx = {0} ^ y;", rngA); x = rngA ^ y; UpdateStrings(); linesLeft -= 1; break;
                case 56: lines += String.Format("\nx = {0} ^ z;", rngA); x = rngA ^ z; UpdateStrings(); linesLeft -= 1; break;
                case 57: lines += String.Format("\nx = {0} ^ {1};", rngA, rngB); x = rngA ^ rngB; UpdateStrings(); linesLeft -= 1; break;
                case 58: lines += String.Format("\nx = x << {0};", rngA); x = x << rngA; UpdateStrings(); linesLeft -= 1; break;
                case 59: lines += String.Format("\nx = y << {0};", rngA); x = y << rngA; UpdateStrings(); linesLeft -= 1; break;
                case 60: lines += String.Format("\nx = z << {0};", rngA); x = z << rngA; UpdateStrings(); linesLeft -= 1; break;
                case 61: lines += String.Format("\nx = {0} << x;", rngA); x = rngA << x; UpdateStrings(); linesLeft -= 1; break;
                case 62: lines += String.Format("\nx = {0} << y;", rngA); x = rngA << y; UpdateStrings(); linesLeft -= 1; break;
                case 63: lines += String.Format("\nx = {0} << z;", rngA); x = rngA << z; UpdateStrings(); linesLeft -= 1; break;
                case 64: lines += String.Format("\nx = {0} << {1};", rngA, rngB); x = rngA << rngB; UpdateStrings(); linesLeft -= 1; break;
                case 65: lines += String.Format("\nx = x >> {0};", rngA); x = x >> rngA; UpdateStrings(); linesLeft -= 1; break;
                case 66: lines += String.Format("\nx = y >> {0};", rngA); x = y >> rngA; UpdateStrings(); linesLeft -= 1; break;
                case 67: lines += String.Format("\nx = z >> {0};", rngA); x = z >> rngA; UpdateStrings(); linesLeft -= 1; break;
                case 68: lines += String.Format("\nx = {0} >> x;", rngA); x = rngA >> x; UpdateStrings(); linesLeft -= 1; break;
                case 69: lines += String.Format("\nx = {0} >> y;", rngA); x = rngA >> y; UpdateStrings(); linesLeft -= 1; break;
                case 70: lines += String.Format("\nx = {0} >> z;", rngA); x = rngA >> z; UpdateStrings(); linesLeft -= 1; break;
                case 71: lines += String.Format("\nx = {0} >> {1};", rngA, rngB); x = rngA >> rngB; UpdateStrings(); linesLeft -= 1; break;
                case 72: lines += String.Format("\nx += x;"); x += x; UpdateStrings(); linesLeft -= 1; break;
                case 73: lines += String.Format("\nx += y;"); x += y; UpdateStrings(); linesLeft -= 1; break;
                case 74: lines += String.Format("\nx += z;"); x += z; UpdateStrings(); linesLeft -= 1; break;
                case 75: lines += String.Format("\nx += {0};", rngA); x += rngA; UpdateStrings(); linesLeft -= 1; break;
                case 76: lines += String.Format("\nx -= x;"); x -= x; UpdateStrings(); linesLeft -= 1; break;
                case 77: lines += String.Format("\nx -= y;"); x -= y; UpdateStrings(); linesLeft -= 1; break;
                case 78: lines += String.Format("\nx -= z;"); x -= z; UpdateStrings(); linesLeft -= 1; break;
                case 79: lines += String.Format("\nx -= {0};", rngA); x -= rngA; UpdateStrings(); linesLeft -= 1; break;
                case 80: lines += String.Format("\nx *= x;"); x *= x; UpdateStrings(); linesLeft -= 1; break;
                case 81: lines += String.Format("\nx *= y;"); x *= y; UpdateStrings(); linesLeft -= 1; break;
                case 82: lines += String.Format("\nx *= z;"); x *= z; UpdateStrings(); linesLeft -= 1; break;
                case 83: lines += String.Format("\nx *= {0};", rngA); x *= rngA; UpdateStrings(); linesLeft -= 1; break;
                case 84: if (x == 0) { break; } lines += String.Format("\nx /= x;"); x /= x; UpdateStrings(); linesLeft -= 1; break;
                case 85: if (y == 0) { break; } lines += String.Format("\nx /= y;"); x /= y; UpdateStrings(); linesLeft -= 1; break;
                case 86: if (z == 0) { break; } lines += String.Format("\nx /= z;"); x /= z; UpdateStrings(); linesLeft -= 1; break;
                case 87: if (rngA == 0) { break; } lines += String.Format("\nx /= {0};", rngA); x /= rngA; UpdateStrings(); linesLeft -= 1; break;
                case 88: if (x == 0) { break; } lines += String.Format("\nx %= x;"); x %= x; UpdateStrings(); linesLeft -= 1; break;
                case 89: if (y == 0) { break; } lines += String.Format("\nx %= y;"); x %= y; UpdateStrings(); linesLeft -= 1; break;
                case 90: if (z == 0) { break; } lines += String.Format("\nx %= z;"); x %= z; UpdateStrings(); linesLeft -= 1; break;
                case 91: if (rngA == 0) { break; } lines += String.Format("\nx %= {0};", rngA); x %= rngA; UpdateStrings(); linesLeft -= 1; break;
                case 92: lines += String.Format("\nx &= x;"); x &= x; UpdateStrings(); linesLeft -= 1; break;
                case 93: lines += String.Format("\nx &= y;"); x &= y; UpdateStrings(); linesLeft -= 1; break;
                case 94: lines += String.Format("\nx &= z;"); x &= z; UpdateStrings(); linesLeft -= 1; break;
                case 95: lines += String.Format("\nx &= {0};", rngA); x &= rngA; UpdateStrings(); linesLeft -= 1; break;
                case 96: lines += String.Format("\nx |= x;"); x |= x; UpdateStrings(); linesLeft -= 1; break;
                case 97: lines += String.Format("\nx |= y;"); x |= y; UpdateStrings(); linesLeft -= 1; break;
                case 98: lines += String.Format("\nx |= z;"); x |= z; UpdateStrings(); linesLeft -= 1; break;
                case 99: lines += String.Format("\nx |= {0};", rngA); x |= rngA; UpdateStrings(); linesLeft -= 1; break;
                case 100: lines += String.Format("\nx ^= x;"); x ^= x; UpdateStrings(); linesLeft -= 1; break;
                case 101: lines += String.Format("\nx ^= y;"); x ^= y; UpdateStrings(); linesLeft -= 1; break;
                case 102: lines += String.Format("\nx ^= z;"); x ^= z; UpdateStrings(); linesLeft -= 1; break;
                case 103: lines += String.Format("\nx ^= {0};", rngA); x ^= rngA; UpdateStrings(); linesLeft -= 1; break;
                case 104: lines += String.Format("\nx <<= x;"); x <<= x; UpdateStrings(); linesLeft -= 1; break;
                case 105: lines += String.Format("\nx <<= y;"); x <<= y; UpdateStrings(); linesLeft -= 1; break;
                case 106: lines += String.Format("\nx <<= z;"); x <<= z; UpdateStrings(); linesLeft -= 1; break;
                case 107: lines += String.Format("\nx <<= {0};", rngA); x <<= rngA; UpdateStrings(); linesLeft -= 1; break;
                case 108: lines += String.Format("\nx >>= x;"); x >>= x; UpdateStrings(); linesLeft -= 1; break;
                case 109: lines += String.Format("\nx >>= y;"); x >>= y; UpdateStrings(); linesLeft -= 1; break;
                case 110: lines += String.Format("\nx >>= z;"); x >>= z; UpdateStrings(); linesLeft -= 1; break;
                case 111: lines += String.Format("\nx >>= {0};", rngA); x >>= rngA; UpdateStrings(); linesLeft -= 1; break;
                case 112: lines += String.Format("\nx++;"); x++; UpdateStrings(); linesLeft -= 1; break;
                case 113: lines += String.Format("\nx--;"); x--; UpdateStrings(); linesLeft -= 1; break;
                case 114: lines += String.Format("\ny = z;"); y = z; UpdateStrings(); linesLeft -= 1; break;
                case 115: lines += String.Format("\ny = x;"); y = x; UpdateStrings(); linesLeft -= 1; break;
                case 116: lines += String.Format("\ny = y + {0};", rngA); y = y + rngA; UpdateStrings(); linesLeft -= 1; break;
                case 117: lines += String.Format("\ny = z + {0};", rngA); y = z + rngA; UpdateStrings(); linesLeft -= 1; break;
                case 118: lines += String.Format("\ny = x + {0};", rngA); y = x + rngA; UpdateStrings(); linesLeft -= 1; break;
                case 119: lines += String.Format("\ny = {0} + y;", rngA); y = rngA + y; UpdateStrings(); linesLeft -= 1; break;
                case 120: lines += String.Format("\ny = {0} + z;", rngA); y = rngA + z; UpdateStrings(); linesLeft -= 1; break;
                case 121: lines += String.Format("\ny = {0} + x;", rngA); y = rngA + x; UpdateStrings(); linesLeft -= 1; break;
                case 122: lines += String.Format("\ny = {0} + {1};", rngA, rngB); y = rngA + rngB; UpdateStrings(); linesLeft -= 1; break;
                case 123: lines += String.Format("\ny = y - {0};", rngA); y = y - rngA; UpdateStrings(); linesLeft -= 1; break;
                case 124: lines += String.Format("\ny = z - {0};", rngA); y = z - rngA; UpdateStrings(); linesLeft -= 1; break;
                case 125: lines += String.Format("\ny = x - {0};", rngA); y = x - rngA; UpdateStrings(); linesLeft -= 1; break;
                case 126: lines += String.Format("\ny = {0} - y;", rngA); y = rngA - y; UpdateStrings(); linesLeft -= 1; break;
                case 127: lines += String.Format("\ny = {0} - z;", rngA); y = rngA - z; UpdateStrings(); linesLeft -= 1; break;
                case 128: lines += String.Format("\ny = {0} - x;", rngA); y = rngA - x; UpdateStrings(); linesLeft -= 1; break;
                case 129: lines += String.Format("\ny = {0} - {1};", rngA, rngB); y = rngA - rngB; UpdateStrings(); linesLeft -= 1; break;
                case 130: lines += String.Format("\ny = y * {0};", rngA); y = y * rngA; UpdateStrings(); linesLeft -= 1; break;
                case 131: lines += String.Format("\ny = z * {0};", rngA); y = z * rngA; UpdateStrings(); linesLeft -= 1; break;
                case 132: lines += String.Format("\ny = x * {0};", rngA); y = x * rngA; UpdateStrings(); linesLeft -= 1; break;
                case 133: lines += String.Format("\ny = {0} * y;", rngA); y = rngA * y; UpdateStrings(); linesLeft -= 1; break;
                case 134: lines += String.Format("\ny = {0} * z;", rngA); y = rngA * z; UpdateStrings(); linesLeft -= 1; break;
                case 135: lines += String.Format("\ny = {0} * x;", rngA); y = rngA * x; UpdateStrings(); linesLeft -= 1; break;
                case 136: lines += String.Format("\ny = {0} * {1};", rngA, rngB); y = rngA * rngB; UpdateStrings(); linesLeft -= 1; break;
                case 137: if (rngA == 0) { break; } lines += String.Format("\ny = y / {0};", rngA); y = y / rngA; UpdateStrings(); linesLeft -= 1; break;
                case 138: if (rngA == 0) { break; } lines += String.Format("\ny = z / {0};", rngA); y = z / rngA; UpdateStrings(); linesLeft -= 1; break;
                case 139: if (rngA == 0) { break; } lines += String.Format("\ny = x / {0};", rngA); y = x / rngA; UpdateStrings(); linesLeft -= 1; break;
                case 140: if (y == 0) { break; } lines += String.Format("\ny = {0} / y;", rngA); y = rngA / y; UpdateStrings(); linesLeft -= 1; break;
                case 141: if (z == 0) { break; } lines += String.Format("\ny = {0} / z;", rngA); y = rngA / z; UpdateStrings(); linesLeft -= 1; break;
                case 142: if (x == 0) { break; } lines += String.Format("\ny = {0} / x;", rngA); y = rngA / x; UpdateStrings(); linesLeft -= 1; break;
                case 143: if (rngB == 0) { break; } lines += String.Format("\ny = {0} / {1};", rngA, rngB); y = rngA / rngB; UpdateStrings(); linesLeft -= 1; break;
                case 144: if (rngA == 0) { break; } lines += String.Format("\ny = y % {0};", rngA); y = y % rngA; UpdateStrings(); linesLeft -= 1; break;
                case 145: if (rngA == 0) { break; } lines += String.Format("\ny = z % {0};", rngA); y = z % rngA; UpdateStrings(); linesLeft -= 1; break;
                case 146: if (rngA == 0) { break; } lines += String.Format("\ny = x % {0};", rngA); y = x % rngA; UpdateStrings(); linesLeft -= 1; break;
                case 147: if (y == 0) { break; } lines += String.Format("\ny = {0} % y;", rngA); y = rngA % y; UpdateStrings(); linesLeft -= 1; break;
                case 148: if (z == 0) { break; } lines += String.Format("\ny = {0} % z;", rngA); y = rngA % z; UpdateStrings(); linesLeft -= 1; break;
                case 149: if (x == 0) { break; } lines += String.Format("\ny = {0} % x;", rngA); y = rngA % x; UpdateStrings(); linesLeft -= 1; break;
                case 150: if (rngB == 0) { break; } lines += String.Format("\ny = {0} % {1};", rngA, rngB); y = rngA % rngB; UpdateStrings(); linesLeft -= 1; break;
                case 151: lines += String.Format("\ny = y & {0};", rngA); y = y & rngA; UpdateStrings(); linesLeft -= 1; break;
                case 152: lines += String.Format("\ny = z & {0};", rngA); y = z & rngA; UpdateStrings(); linesLeft -= 1; break;
                case 153: lines += String.Format("\ny = x & {0};", rngA); y = x & rngA; UpdateStrings(); linesLeft -= 1; break;
                case 154: lines += String.Format("\ny = {0} & y;", rngA); y = rngA & y; UpdateStrings(); linesLeft -= 1; break;
                case 155: lines += String.Format("\ny = {0} & z;", rngA); y = rngA & z; UpdateStrings(); linesLeft -= 1; break;
                case 156: lines += String.Format("\ny = {0} & x;", rngA); y = rngA & x; UpdateStrings(); linesLeft -= 1; break;
                case 157: lines += String.Format("\ny = {0} & {1};", rngA, rngB); y = rngA & rngB; UpdateStrings(); linesLeft -= 1; break;
                case 158: lines += String.Format("\ny = y | {0};", rngA); y = y | rngA; UpdateStrings(); linesLeft -= 1; break;
                case 159: lines += String.Format("\ny = z | {0};", rngA); y = z | rngA; UpdateStrings(); linesLeft -= 1; break;
                case 160: lines += String.Format("\ny = x | {0};", rngA); y = x | rngA; UpdateStrings(); linesLeft -= 1; break;
                case 161: lines += String.Format("\ny = {0} | y;", rngA); y = rngA | y; UpdateStrings(); linesLeft -= 1; break;
                case 162: lines += String.Format("\ny = {0} | z;", rngA); y = rngA | z; UpdateStrings(); linesLeft -= 1; break;
                case 163: lines += String.Format("\ny = {0} | x;", rngA); y = rngA | x; UpdateStrings(); linesLeft -= 1; break;
                case 164: lines += String.Format("\ny = {0} | {1};", rngA, rngB); y = rngA | rngB; UpdateStrings(); linesLeft -= 1; break;
                case 165: lines += String.Format("\ny = y ^ {0};", rngA); y = y ^ rngA; UpdateStrings(); linesLeft -= 1; break;
                case 166: lines += String.Format("\ny = z ^ {0};", rngA); y = z ^ rngA; UpdateStrings(); linesLeft -= 1; break;
                case 167: lines += String.Format("\ny = x ^ {0};", rngA); y = x ^ rngA; UpdateStrings(); linesLeft -= 1; break;
                case 168: lines += String.Format("\ny = {0} ^ y;", rngA); y = rngA ^ y; UpdateStrings(); linesLeft -= 1; break;
                case 169: lines += String.Format("\ny = {0} ^ z;", rngA); y = rngA ^ z; UpdateStrings(); linesLeft -= 1; break;
                case 170: lines += String.Format("\ny = {0} ^ x;", rngA); y = rngA ^ x; UpdateStrings(); linesLeft -= 1; break;
                case 171: lines += String.Format("\ny = {0} ^ {1};", rngA, rngB); y = rngA ^ rngB; UpdateStrings(); linesLeft -= 1; break;
                case 172: lines += String.Format("\ny = y << {0};", rngA); y = y << rngA; UpdateStrings(); linesLeft -= 1; break;
                case 173: lines += String.Format("\ny = z << {0};", rngA); y = z << rngA; UpdateStrings(); linesLeft -= 1; break;
                case 174: lines += String.Format("\ny = x << {0};", rngA); y = x << rngA; UpdateStrings(); linesLeft -= 1; break;
                case 175: lines += String.Format("\ny = {0} << y;", rngA); y = rngA << y; UpdateStrings(); linesLeft -= 1; break;
                case 176: lines += String.Format("\ny = {0} << z;", rngA); y = rngA << z; UpdateStrings(); linesLeft -= 1; break;
                case 177: lines += String.Format("\ny = {0} << x;", rngA); y = rngA << x; UpdateStrings(); linesLeft -= 1; break;
                case 178: lines += String.Format("\ny = {0} << {1};", rngA, rngB); y = rngA << rngB; UpdateStrings(); linesLeft -= 1; break;
                case 179: lines += String.Format("\ny = y >> {0};", rngA); y = y >> rngA; UpdateStrings(); linesLeft -= 1; break;
                case 180: lines += String.Format("\ny = z >> {0};", rngA); y = z >> rngA; UpdateStrings(); linesLeft -= 1; break;
                case 181: lines += String.Format("\ny = x >> {0};", rngA); y = x >> rngA; UpdateStrings(); linesLeft -= 1; break;
                case 182: lines += String.Format("\ny = {0} >> y;", rngA); y = rngA >> y; UpdateStrings(); linesLeft -= 1; break;
                case 183: lines += String.Format("\ny = {0} >> z;", rngA); y = rngA >> z; UpdateStrings(); linesLeft -= 1; break;
                case 184: lines += String.Format("\ny = {0} >> x;", rngA); y = rngA >> x; UpdateStrings(); linesLeft -= 1; break;
                case 185: lines += String.Format("\ny = {0} >> {1};", rngA, rngB); y = rngA >> rngB; UpdateStrings(); linesLeft -= 1; break;
                case 186: lines += String.Format("\ny += y;"); y += y; UpdateStrings(); linesLeft -= 1; break;
                case 187: lines += String.Format("\ny += z;"); y += z; UpdateStrings(); linesLeft -= 1; break;
                case 188: lines += String.Format("\ny += x;"); y += x; UpdateStrings(); linesLeft -= 1; break;
                case 189: lines += String.Format("\ny += {0};", rngA); y += rngA; UpdateStrings(); linesLeft -= 1; break;
                case 190: lines += String.Format("\ny -= y;"); y -= y; UpdateStrings(); linesLeft -= 1; break;
                case 191: lines += String.Format("\ny -= z;"); y -= z; UpdateStrings(); linesLeft -= 1; break;
                case 192: lines += String.Format("\ny -= x;"); y -= x; UpdateStrings(); linesLeft -= 1; break;
                case 193: lines += String.Format("\ny -= {0};", rngA); y -= rngA; UpdateStrings(); linesLeft -= 1; break;
                case 194: lines += String.Format("\ny *= y;"); y *= y; UpdateStrings(); linesLeft -= 1; break;
                case 195: lines += String.Format("\ny *= z;"); y *= z; UpdateStrings(); linesLeft -= 1; break;
                case 196: lines += String.Format("\ny *= x;"); y *= x; UpdateStrings(); linesLeft -= 1; break;
                case 197: lines += String.Format("\ny *= {0};", rngA); y *= rngA; UpdateStrings(); linesLeft -= 1; break;
                case 198: if (y == 0) { break; } lines += String.Format("\ny /= y;"); y /= y; UpdateStrings(); linesLeft -= 1; break;
                case 199: if (z == 0) { break; } lines += String.Format("\ny /= z;"); y /= z; UpdateStrings(); linesLeft -= 1; break;
                case 200: if (x == 0) { break; } lines += String.Format("\ny /= x;"); y /= x; UpdateStrings(); linesLeft -= 1; break;
                case 201: if (rngA == 0) { break; } lines += String.Format("\ny /= {0};", rngA); y /= rngA; UpdateStrings(); linesLeft -= 1; break;
                case 202: if (y == 0) { break; } lines += String.Format("\ny %= y;"); y %= y; UpdateStrings(); linesLeft -= 1; break;
                case 203: if (z == 0) { break; } lines += String.Format("\ny %= z;"); y %= z; UpdateStrings(); linesLeft -= 1; break;
                case 204: if (x == 0) { break; } lines += String.Format("\ny %= x;"); y %= x; UpdateStrings(); linesLeft -= 1; break;
                case 205: if (rngA == 0) { break; } lines += String.Format("\ny %= {0};", rngA); y %= rngA; UpdateStrings(); linesLeft -= 1; break;
                case 206: lines += String.Format("\ny &= y;"); y &= y; UpdateStrings(); linesLeft -= 1; break;
                case 207: lines += String.Format("\ny &= z;"); y &= z; UpdateStrings(); linesLeft -= 1; break;
                case 208: lines += String.Format("\ny &= x;"); y &= x; UpdateStrings(); linesLeft -= 1; break;
                case 209: lines += String.Format("\ny &= {0};", rngA); y &= rngA; UpdateStrings(); linesLeft -= 1; break;
                case 210: lines += String.Format("\ny |= y;"); y |= y; UpdateStrings(); linesLeft -= 1; break;
                case 211: lines += String.Format("\ny |= z;"); y |= z; UpdateStrings(); linesLeft -= 1; break;
                case 212: lines += String.Format("\ny |= x;"); y |= x; UpdateStrings(); linesLeft -= 1; break;
                case 213: lines += String.Format("\ny |= {0};", rngA); y |= rngA; UpdateStrings(); linesLeft -= 1; break;
                case 214: lines += String.Format("\ny ^= y;"); y ^= y; UpdateStrings(); linesLeft -= 1; break;
                case 215: lines += String.Format("\ny ^= z;"); y ^= z; UpdateStrings(); linesLeft -= 1; break;
                case 216: lines += String.Format("\ny ^= x;"); y ^= x; UpdateStrings(); linesLeft -= 1; break;
                case 217: lines += String.Format("\ny ^= {0};", rngA); y ^= rngA; UpdateStrings(); linesLeft -= 1; break;
                case 218: lines += String.Format("\ny <<= y;"); y <<= y; UpdateStrings(); linesLeft -= 1; break;
                case 219: lines += String.Format("\ny <<= z;"); y <<= z; UpdateStrings(); linesLeft -= 1; break;
                case 220: lines += String.Format("\ny <<= x;"); y <<= x; UpdateStrings(); linesLeft -= 1; break;
                case 221: lines += String.Format("\ny <<= {0};", rngA); y <<= rngA; UpdateStrings(); linesLeft -= 1; break;
                case 222: lines += String.Format("\ny >>= y;"); y >>= y; UpdateStrings(); linesLeft -= 1; break;
                case 223: lines += String.Format("\ny >>= z;"); y >>= z; UpdateStrings(); linesLeft -= 1; break;
                case 224: lines += String.Format("\ny >>= x;"); y >>= x; UpdateStrings(); linesLeft -= 1; break;
                case 225: lines += String.Format("\ny >>= {0};", rngA); y >>= rngA; UpdateStrings(); linesLeft -= 1; break;
                case 226: lines += String.Format("\ny++;"); y++; UpdateStrings(); linesLeft -= 1; break;
                case 227: lines += String.Format("\ny--;"); y--; UpdateStrings(); linesLeft -= 1; break;
                case 228: lines += String.Format("\nz = x;"); z = x; UpdateStrings(); linesLeft -= 1; break;
                case 229: lines += String.Format("\nz = y;"); z = y; UpdateStrings(); linesLeft -= 1; break;
                case 230: lines += String.Format("\nz = z + {0};", rngA); z = z + rngA; UpdateStrings(); linesLeft -= 1; break;
                case 231: lines += String.Format("\nz = x + {0};", rngA); z = x + rngA; UpdateStrings(); linesLeft -= 1; break;
                case 232: lines += String.Format("\nz = y + {0};", rngA); z = y + rngA; UpdateStrings(); linesLeft -= 1; break;
                case 233: lines += String.Format("\nz = {0} + z;", rngA); z = rngA + z; UpdateStrings(); linesLeft -= 1; break;
                case 234: lines += String.Format("\nz = {0} + x;", rngA); z = rngA + x; UpdateStrings(); linesLeft -= 1; break;
                case 235: lines += String.Format("\nz = {0} + y;", rngA); z = rngA + y; UpdateStrings(); linesLeft -= 1; break;
                case 236: lines += String.Format("\nz = {0} + {1};", rngA, rngB); z = rngA + rngB; UpdateStrings(); linesLeft -= 1; break;
                case 237: lines += String.Format("\nz = z - {0};", rngA); z = z - rngA; UpdateStrings(); linesLeft -= 1; break;
                case 238: lines += String.Format("\nz = x - {0};", rngA); z = x - rngA; UpdateStrings(); linesLeft -= 1; break;
                case 239: lines += String.Format("\nz = y - {0};", rngA); z = y - rngA; UpdateStrings(); linesLeft -= 1; break;
                case 240: lines += String.Format("\nz = {0} - z;", rngA); z = rngA - z; UpdateStrings(); linesLeft -= 1; break;
                case 241: lines += String.Format("\nz = {0} - x;", rngA); z = rngA - x; UpdateStrings(); linesLeft -= 1; break;
                case 242: lines += String.Format("\nz = {0} - y;", rngA); z = rngA - y; UpdateStrings(); linesLeft -= 1; break;
                case 243: lines += String.Format("\nz = {0} - {1};", rngA, rngB); z = rngA - rngB; UpdateStrings(); linesLeft -= 1; break;
                case 244: lines += String.Format("\nz = z * {0};", rngA); z = z * rngA; UpdateStrings(); linesLeft -= 1; break;
                case 245: lines += String.Format("\nz = x * {0};", rngA); z = x * rngA; UpdateStrings(); linesLeft -= 1; break;
                case 246: lines += String.Format("\nz = y * {0};", rngA); z = y * rngA; UpdateStrings(); linesLeft -= 1; break;
                case 247: lines += String.Format("\nz = {0} * z;", rngA); z = rngA * z; UpdateStrings(); linesLeft -= 1; break;
                case 248: lines += String.Format("\nz = {0} * x;", rngA); z = rngA * x; UpdateStrings(); linesLeft -= 1; break;
                case 249: lines += String.Format("\nz = {0} * y;", rngA); z = rngA * y; UpdateStrings(); linesLeft -= 1; break;
                case 250: lines += String.Format("\nz = {0} * {1};", rngA, rngB); z = rngA * rngB; UpdateStrings(); linesLeft -= 1; break;
                case 251: if (rngA == 0) { break; } lines += String.Format("\nz = z / {0};", rngA); z = z / rngA; UpdateStrings(); linesLeft -= 1; break;
                case 252: if (rngA == 0) { break; } lines += String.Format("\nz = x / {0};", rngA); z = x / rngA; UpdateStrings(); linesLeft -= 1; break;
                case 253: if (rngA == 0) { break; } lines += String.Format("\nz = y / {0};", rngA); z = y / rngA; UpdateStrings(); linesLeft -= 1; break;
                case 254: if (z == 0) { break; } lines += String.Format("\nz = {0} / z;", rngA); z = rngA / z; UpdateStrings(); linesLeft -= 1; break;
                case 255: if (x == 0) { break; } lines += String.Format("\nz = {0} / x;", rngA); z = rngA / x; UpdateStrings(); linesLeft -= 1; break;
                case 256: if (y == 0) { break; } lines += String.Format("\nz = {0} / y;", rngA); z = rngA / y; UpdateStrings(); linesLeft -= 1; break;
                case 257: if (rngB == 0) { break; } lines += String.Format("\nz = {0} / {1};", rngA, rngB); z = rngA / rngB; UpdateStrings(); linesLeft -= 1; break;
                case 258: if (rngA == 0) { break; } lines += String.Format("\nz = z % {0};", rngA); z = z % rngA; UpdateStrings(); linesLeft -= 1; break;
                case 259: if (rngA == 0) { break; } lines += String.Format("\nz = x % {0};", rngA); z = x % rngA; UpdateStrings(); linesLeft -= 1; break;
                case 260: if (rngA == 0) { break; } lines += String.Format("\nz = y % {0};", rngA); z = y % rngA; UpdateStrings(); linesLeft -= 1; break;
                case 261: if (z == 0) { break; } lines += String.Format("\nz = {0} % z;", rngA); z = rngA % z; UpdateStrings(); linesLeft -= 1; break;
                case 262: if (x == 0) { break; } lines += String.Format("\nz = {0} % x;", rngA); z = rngA % x; UpdateStrings(); linesLeft -= 1; break;
                case 263: if (y == 0) { break; } lines += String.Format("\nz = {0} % y;", rngA); z = rngA % y; UpdateStrings(); linesLeft -= 1; break;
                case 264: if (rngB == 0) { break; } lines += String.Format("\nz = {0} % {1};", rngA, rngB); z = rngA % rngB; UpdateStrings(); linesLeft -= 1; break;
                case 265: lines += String.Format("\nz = z & {0};", rngA); z = z & rngA; UpdateStrings(); linesLeft -= 1; break;
                case 266: lines += String.Format("\nz = x & {0};", rngA); z = x & rngA; UpdateStrings(); linesLeft -= 1; break;
                case 267: lines += String.Format("\nz = y & {0};", rngA); z = y & rngA; UpdateStrings(); linesLeft -= 1; break;
                case 268: lines += String.Format("\nz = {0} & z;", rngA); z = rngA & z; UpdateStrings(); linesLeft -= 1; break;
                case 269: lines += String.Format("\nz = {0} & x;", rngA); z = rngA & x; UpdateStrings(); linesLeft -= 1; break;
                case 270: lines += String.Format("\nz = {0} & y;", rngA); z = rngA & y; UpdateStrings(); linesLeft -= 1; break;
                case 271: lines += String.Format("\nz = {0} & {1};", rngA, rngB); z = rngA & rngB; UpdateStrings(); linesLeft -= 1; break;
                case 272: lines += String.Format("\nz = z | {0};", rngA); z = z | rngA; UpdateStrings(); linesLeft -= 1; break;
                case 273: lines += String.Format("\nz = x | {0};", rngA); z = x | rngA; UpdateStrings(); linesLeft -= 1; break;
                case 274: lines += String.Format("\nz = y | {0};", rngA); z = y | rngA; UpdateStrings(); linesLeft -= 1; break;
                case 275: lines += String.Format("\nz = {0} | z;", rngA); z = rngA | z; UpdateStrings(); linesLeft -= 1; break;
                case 276: lines += String.Format("\nz = {0} | x;", rngA); z = rngA | x; UpdateStrings(); linesLeft -= 1; break;
                case 277: lines += String.Format("\nz = {0} | y;", rngA); z = rngA | y; UpdateStrings(); linesLeft -= 1; break;
                case 278: lines += String.Format("\nz = {0} | {1};", rngA, rngB); z = rngA | rngB; UpdateStrings(); linesLeft -= 1; break;
                case 279: lines += String.Format("\nz = z ^ {0};", rngA); z = z ^ rngA; UpdateStrings(); linesLeft -= 1; break;
                case 280: lines += String.Format("\nz = x ^ {0};", rngA); z = x ^ rngA; UpdateStrings(); linesLeft -= 1; break;
                case 281: lines += String.Format("\nz = y ^ {0};", rngA); z = y ^ rngA; UpdateStrings(); linesLeft -= 1; break;
                case 282: lines += String.Format("\nz = {0} ^ z;", rngA); z = rngA ^ z; UpdateStrings(); linesLeft -= 1; break;
                case 283: lines += String.Format("\nz = {0} ^ x;", rngA); z = rngA ^ x; UpdateStrings(); linesLeft -= 1; break;
                case 284: lines += String.Format("\nz = {0} ^ y;", rngA); z = rngA ^ y; UpdateStrings(); linesLeft -= 1; break;
                case 285: lines += String.Format("\nz = {0} ^ {1};", rngA, rngB); z = rngA ^ rngB; UpdateStrings(); linesLeft -= 1; break;
                case 286: lines += String.Format("\nz = z << {0};", rngA); z = z << rngA; UpdateStrings(); linesLeft -= 1; break;
                case 287: lines += String.Format("\nz = x << {0};", rngA); z = x << rngA; UpdateStrings(); linesLeft -= 1; break;
                case 288: lines += String.Format("\nz = y << {0};", rngA); z = y << rngA; UpdateStrings(); linesLeft -= 1; break;
                case 289: lines += String.Format("\nz = {0} << z;", rngA); z = rngA << z; UpdateStrings(); linesLeft -= 1; break;
                case 290: lines += String.Format("\nz = {0} << x;", rngA); z = rngA << x; UpdateStrings(); linesLeft -= 1; break;
                case 291: lines += String.Format("\nz = {0} << y;", rngA); z = rngA << y; UpdateStrings(); linesLeft -= 1; break;
                case 292: lines += String.Format("\nz = {0} << {1};", rngA, rngB); z = rngA << rngB; UpdateStrings(); linesLeft -= 1; break;
                case 293: lines += String.Format("\nz = z >> {0};", rngA); z = z >> rngA; UpdateStrings(); linesLeft -= 1; break;
                case 294: lines += String.Format("\nz = x >> {0};", rngA); z = x >> rngA; UpdateStrings(); linesLeft -= 1; break;
                case 295: lines += String.Format("\nz = y >> {0};", rngA); z = y >> rngA; UpdateStrings(); linesLeft -= 1; break;
                case 296: lines += String.Format("\nz = {0} >> z;", rngA); z = rngA >> z; UpdateStrings(); linesLeft -= 1; break;
                case 297: lines += String.Format("\nz = {0} >> x;", rngA); z = rngA >> x; UpdateStrings(); linesLeft -= 1; break;
                case 298: lines += String.Format("\nz = {0} >> y;", rngA); z = rngA >> y; UpdateStrings(); linesLeft -= 1; break;
                case 299: lines += String.Format("\nz = {0} >> {1};", rngA, rngB); z = rngA >> rngB; UpdateStrings(); linesLeft -= 1; break;
                case 300: lines += String.Format("\nz += z;"); z += z; UpdateStrings(); linesLeft -= 1; break;
                case 301: lines += String.Format("\nz += x;"); z += x; UpdateStrings(); linesLeft -= 1; break;
                case 302: lines += String.Format("\nz += y;"); z += y; UpdateStrings(); linesLeft -= 1; break;
                case 303: lines += String.Format("\nz += {0};", rngA); z += rngA; UpdateStrings(); linesLeft -= 1; break;
                case 304: lines += String.Format("\nz -= z;"); z -= z; UpdateStrings(); linesLeft -= 1; break;
                case 305: lines += String.Format("\nz -= x;"); z -= x; UpdateStrings(); linesLeft -= 1; break;
                case 306: lines += String.Format("\nz -= y;"); z -= y; UpdateStrings(); linesLeft -= 1; break;
                case 307: lines += String.Format("\nz -= {0};", rngA); z -= rngA; UpdateStrings(); linesLeft -= 1; break;
                case 308: lines += String.Format("\nz *= z;"); z *= z; UpdateStrings(); linesLeft -= 1; break;
                case 309: lines += String.Format("\nz *= x;"); z *= x; UpdateStrings(); linesLeft -= 1; break;
                case 310: lines += String.Format("\nz *= y;"); z *= y; UpdateStrings(); linesLeft -= 1; break;
                case 311: lines += String.Format("\nz *= {0};", rngA); z *= rngA; UpdateStrings(); linesLeft -= 1; break;
                case 312: if (z == 0) { break; } lines += String.Format("\nz /= z;"); z /= z; UpdateStrings(); linesLeft -= 1; break;
                case 313: if (x == 0) { break; } lines += String.Format("\nz /= x;"); z /= x; UpdateStrings(); linesLeft -= 1; break;
                case 314: if (y == 0) { break; } lines += String.Format("\nz /= y;"); z /= y; UpdateStrings(); linesLeft -= 1; break;
                case 315: if (rngA == 0) { break; } lines += String.Format("\nz /= {0};", rngA); z /= rngA; UpdateStrings(); linesLeft -= 1; break;
                case 316: if (z == 0) { break; } lines += String.Format("\nz %= z;"); z %= z; UpdateStrings(); linesLeft -= 1; break;
                case 317: if (x == 0) { break; } lines += String.Format("\nz %= x;"); z %= x; UpdateStrings(); linesLeft -= 1; break;
                case 318: if (y == 0) { break; } lines += String.Format("\nz %= y;"); z %= y; UpdateStrings(); linesLeft -= 1; break;
                case 319: if (rngA == 0) { break; } lines += String.Format("\nz %= {0};", rngA); z %= rngA; UpdateStrings(); linesLeft -= 1; break;
                case 320: lines += String.Format("\nz &= z;"); z &= z; UpdateStrings(); linesLeft -= 1; break;
                case 321: lines += String.Format("\nz &= x;"); z &= x; UpdateStrings(); linesLeft -= 1; break;
                case 322: lines += String.Format("\nz &= y;"); z &= y; UpdateStrings(); linesLeft -= 1; break;
                case 323: lines += String.Format("\nz &= {0};", rngA); z &= rngA; UpdateStrings(); linesLeft -= 1; break;
                case 324: lines += String.Format("\nz |= z;"); z |= z; UpdateStrings(); linesLeft -= 1; break;
                case 325: lines += String.Format("\nz |= x;"); z |= x; UpdateStrings(); linesLeft -= 1; break;
                case 326: lines += String.Format("\nz |= y;"); z |= y; UpdateStrings(); linesLeft -= 1; break;
                case 327: lines += String.Format("\nz |= {0};", rngA); z |= rngA; UpdateStrings(); linesLeft -= 1; break;
                case 328: lines += String.Format("\nz ^= z;"); z ^= z; UpdateStrings(); linesLeft -= 1; break;
                case 329: lines += String.Format("\nz ^= x;"); z ^= x; UpdateStrings(); linesLeft -= 1; break;
                case 330: lines += String.Format("\nz ^= y;"); z ^= y; UpdateStrings(); linesLeft -= 1; break;
                case 331: lines += String.Format("\nz ^= {0};", rngA); z ^= rngA; UpdateStrings(); linesLeft -= 1; break;
                case 332: lines += String.Format("\nz <<= z;"); z <<= z; UpdateStrings(); linesLeft -= 1; break;
                case 333: lines += String.Format("\nz <<= x;"); z <<= x; UpdateStrings(); linesLeft -= 1; break;
                case 334: lines += String.Format("\nz <<= y;"); z <<= y; UpdateStrings(); linesLeft -= 1; break;
                case 335: lines += String.Format("\nz <<= {0};", rngA); z <<= rngA; UpdateStrings(); linesLeft -= 1; break;
                case 336: lines += String.Format("\nz >>= z;"); z >>= z; UpdateStrings(); linesLeft -= 1; break;
                case 337: lines += String.Format("\nz >>= x;"); z >>= x; UpdateStrings(); linesLeft -= 1; break;
                case 338: lines += String.Format("\nz >>= y;"); z >>= y; UpdateStrings(); linesLeft -= 1; break;
                case 339: lines += String.Format("\nz >>= {0};", rngA); z >>= rngA; UpdateStrings(); linesLeft -= 1; break;
                case 340: lines += String.Format("\nz++;"); z++; UpdateStrings(); linesLeft -= 1; break;
                case 341: lines += String.Format("\nz--;"); z--; UpdateStrings(); linesLeft -= 1; break;
                default: lines += String.Format("\n//send log!"); UpdateStrings(); linesLeft -= 1; break;
            }
        }
    }

    void GenerateGoal()
    {
        while (goalFound == false)
        {
            Posse = UnityEngine.Random.Range(0, 768);

            switch (Posse)
            {
                case 0: lines += "\n\nif (n == ((x + y) + z)) {"; goal = ((x + y) + z); goalFound = true; break;
                case 1: lines += "\n\nif (n == ((x + y) - z)) {"; goal = ((x + y) - z); goalFound = true; break;
                case 2: lines += "\n\nif (n == ((x + y) * z)) {"; goal = ((x + y) * z); goalFound = true; break;
                case 3: if (z == 0) { break; } lines += "\n\nif (n == ((x + y) / z)) {"; goal = ((x + y) / z); goalFound = true; break;
                case 4: if (z == 0) { break; } lines += "\n\nif (n == ((x + y) % z)) {"; goal = ((x + y) % z); goalFound = true; break;
                case 5: lines += "\n\nif (n == ((x + y) & z)) {"; goal = ((x + y) & z); goalFound = true; break;
                case 6: lines += "\n\nif (n == ((x + y) | z)) {"; goal = ((x + y) | z); goalFound = true; break;
                case 7: lines += "\n\nif (n == ((x + y) ^ z)) {"; goal = ((x + y) ^ z); goalFound = true; break;
                case 8: lines += "\n\nif (n == ((x - y) + z)) {"; goal = ((x - y) + z); goalFound = true; break;
                case 9: lines += "\n\nif (n == ((x - y) - z)) {"; goal = ((x - y) - z); goalFound = true; break;
                case 10: lines += "\n\nif (n == ((x - y) * z)) {"; goal = ((x - y) * z); goalFound = true; break;
                case 11: if (z == 0) { break; } lines += "\n\nif (n == ((x - y) / z)) {"; goal = ((x - y) / z); goalFound = true; break;
                case 12: if (z == 0) { break; } lines += "\n\nif (n == ((x - y) % z)) {"; goal = ((x - y) % z); goalFound = true; break;
                case 13: lines += "\n\nif (n == ((x - y) & z)) {"; goal = ((x - y) & z); goalFound = true; break;
                case 14: lines += "\n\nif (n == ((x - y) | z)) {"; goal = ((x - y) | z); goalFound = true; break;
                case 15: lines += "\n\nif (n == ((x - y) ^ z)) {"; goal = ((x - y) ^ z); goalFound = true; break;
                case 16: lines += "\n\nif (n == ((x * y) + z)) {"; goal = ((x * y) + z); goalFound = true; break;
                case 17: lines += "\n\nif (n == ((x * y) - z)) {"; goal = ((x * y) - z); goalFound = true; break;
                case 18: lines += "\n\nif (n == ((x * y) * z)) {"; goal = ((x * y) * z); goalFound = true; break;
                case 19: if (z == 0) { break; } lines += "\n\nif (n == ((x * y) / z)) {"; goal = ((x * y) / z); goalFound = true; break;
                case 20: if (z == 0) { break; } lines += "\n\nif (n == ((x * y) % z)) {"; goal = ((x * y) % z); goalFound = true; break;
                case 21: lines += "\n\nif (n == ((x * y) & z)) {"; goal = ((x * y) & z); goalFound = true; break;
                case 22: lines += "\n\nif (n == ((x * y) | z)) {"; goal = ((x * y) | z); goalFound = true; break;
                case 23: lines += "\n\nif (n == ((x * y) ^ z)) {"; goal = ((x * y) ^ z); goalFound = true; break;
                case 24: if (y == 0) { break; } lines += "\n\nif (n == ((x / y) + z)) {"; goal = ((x / y) + z); goalFound = true; break;
                case 25: if (y == 0) { break; } lines += "\n\nif (n == ((x / y) - z)) {"; goal = ((x / y) - z); goalFound = true; break;
                case 26: if (y == 0) { break; } lines += "\n\nif (n == ((x / y) * z)) {"; goal = ((x / y) * z); goalFound = true; break;
                case 27: if (y == 0 || z == 0) { break; } lines += "\n\nif (n == ((x / y) / z)) {"; goal = ((x / y) / z); goalFound = true; break;
                case 28: if (y == 0 || z == 0) { break; } lines += "\n\nif (n == ((x / y) % z)) {"; goal = ((x / y) % z); goalFound = true; break;
                case 29: if (y == 0) { break; } lines += "\n\nif (n == ((x / y) & z)) {"; goal = ((x / y) & z); goalFound = true; break;
                case 30: if (y == 0) { break; } lines += "\n\nif (n == ((x / y) | z)) {"; goal = ((x / y) | z); goalFound = true; break;
                case 31: if (y == 0) { break; } lines += "\n\nif (n == ((x / y) ^ z)) {"; goal = ((x / y) ^ z); goalFound = true; break;
                case 32: if (y == 0) { break; } lines += "\n\nif (n == ((x % y) + z)) {"; goal = ((x % y) + z); goalFound = true; break;
                case 33: if (y == 0) { break; } lines += "\n\nif (n == ((x % y) - z)) {"; goal = ((x % y) - z); goalFound = true; break;
                case 34: if (y == 0) { break; } lines += "\n\nif (n == ((x % y) * z)) {"; goal = ((x % y) * z); goalFound = true; break;
                case 35: if (y == 0 || z == 0) { break; } lines += "\n\nif (n == ((x % y) / z)) {"; goal = ((x % y) / z); goalFound = true; break;
                case 36: if (y == 0 || z == 0) { break; } lines += "\n\nif (n == ((x % y) % z)) {"; goal = ((x % y) % z); goalFound = true; break;
                case 37: if (y == 0) { break; } lines += "\n\nif (n == ((x % y) & z)) {"; goal = ((x % y) & z); goalFound = true; break;
                case 38: if (y == 0) { break; } lines += "\n\nif (n == ((x % y) | z)) {"; goal = ((x % y) | z); goalFound = true; break;
                case 39: if (y == 0) { break; } lines += "\n\nif (n == ((x % y) ^ z)) {"; goal = ((x % y) ^ z); goalFound = true; break;
                case 40: lines += "\n\nif (n == ((x & y) + z)) {"; goal = ((x & y) + z); goalFound = true; break;
                case 41: lines += "\n\nif (n == ((x & y) - z)) {"; goal = ((x & y) - z); goalFound = true; break;
                case 42: lines += "\n\nif (n == ((x & y) * z)) {"; goal = ((x & y) * z); goalFound = true; break;
                case 43: if (z == 0) { break; } lines += "\n\nif (n == ((x & y) / z)) {"; goal = ((x & y) / z); goalFound = true; break;
                case 44: if (z == 0) { break; } lines += "\n\nif (n == ((x & y) % z)) {"; goal = ((x & y) % z); goalFound = true; break;
                case 45: lines += "\n\nif (n == ((x & y) & z)) {"; goal = ((x & y) & z); goalFound = true; break;
                case 46: lines += "\n\nif (n == ((x & y) | z)) {"; goal = ((x & y) | z); goalFound = true; break;
                case 47: lines += "\n\nif (n == ((x & y) ^ z)) {"; goal = ((x & y) ^ z); goalFound = true; break;
                case 48: lines += "\n\nif (n == ((x | y) + z)) {"; goal = ((x | y) + z); goalFound = true; break;
                case 49: lines += "\n\nif (n == ((x | y) - z)) {"; goal = ((x | y) - z); goalFound = true; break;
                case 50: lines += "\n\nif (n == ((x | y) * z)) {"; goal = ((x | y) * z); goalFound = true; break;
                case 51: if (z == 0) { break; } lines += "\n\nif (n == ((x | y) / z)) {"; goal = ((x | y) / z); goalFound = true; break;
                case 52: if (z == 0) { break; } lines += "\n\nif (n == ((x | y) % z)) {"; goal = ((x | y) % z); goalFound = true; break;
                case 53: lines += "\n\nif (n == ((x | y) & z)) {"; goal = ((x | y) & z); goalFound = true; break;
                case 54: lines += "\n\nif (n == ((x | y) | z)) {"; goal = ((x | y) | z); goalFound = true; break;
                case 55: lines += "\n\nif (n == ((x | y) ^ z)) {"; goal = ((x | y) ^ z); goalFound = true; break;
                case 56: lines += "\n\nif (n == ((x ^ y) + z)) {"; goal = ((x ^ y) + z); goalFound = true; break;
                case 57: lines += "\n\nif (n == ((x ^ y) - z)) {"; goal = ((x ^ y) - z); goalFound = true; break;
                case 58: lines += "\n\nif (n == ((x ^ y) * z)) {"; goal = ((x ^ y) * z); goalFound = true; break;
                case 59: if (z == 0) { break; } lines += "\n\nif (n == ((x ^ y) / z)) {"; goal = ((x ^ y) / z); goalFound = true; break;
                case 60: if (z == 0) { break; } lines += "\n\nif (n == ((x ^ y) % z)) {"; goal = ((x ^ y) % z); goalFound = true; break;
                case 61: lines += "\n\nif (n == ((x ^ y) & z)) {"; goal = ((x ^ y) & z); goalFound = true; break;
                case 62: lines += "\n\nif (n == ((x ^ y) | z)) {"; goal = ((x ^ y) | z); goalFound = true; break;
                case 63: lines += "\n\nif (n == ((x ^ y) ^ z)) {"; goal = ((x ^ y) ^ z); goalFound = true; break;
                case 64: lines += "\n\nif (n == (x + (y + z))) {"; goal = (x + (y + z)); goalFound = true; break;
                case 65: lines += "\n\nif (n == (x + (y - z))) {"; goal = (x + (y - z)); goalFound = true; break;
                case 66: lines += "\n\nif (n == (x + (y * z))) {"; goal = (x + (y * z)); goalFound = true; break;
                case 67: if (z == 0) { break; } lines += "\n\nif (n == (x + (y / z))) {"; goal = (x + (y / z)); goalFound = true; break;
                case 68: if (z == 0) { break; } lines += "\n\nif (n == (x + (y % z))) {"; goal = (x + (y % z)); goalFound = true; break;
                case 69: lines += "\n\nif (n == (x + (y & z))) {"; goal = (x + (y & z)); goalFound = true; break;
                case 70: lines += "\n\nif (n == (x + (y | z))) {"; goal = (x + (y | z)); goalFound = true; break;
                case 71: lines += "\n\nif (n == (x + (y ^ z))) {"; goal = (x + (y ^ z)); goalFound = true; break;
                case 72: lines += "\n\nif (n == (x - (y + z))) {"; goal = (x - (y + z)); goalFound = true; break;
                case 73: lines += "\n\nif (n == (x - (y - z))) {"; goal = (x - (y - z)); goalFound = true; break;
                case 74: lines += "\n\nif (n == (x - (y * z))) {"; goal = (x - (y * z)); goalFound = true; break;
                case 75: if (z == 0) { break; } lines += "\n\nif (n == (x - (y / z))) {"; goal = (x - (y / z)); goalFound = true; break;
                case 76: if (z == 0) { break; } lines += "\n\nif (n == (x - (y % z))) {"; goal = (x - (y % z)); goalFound = true; break;
                case 77: lines += "\n\nif (n == (x - (y & z))) {"; goal = (x - (y & z)); goalFound = true; break;
                case 78: lines += "\n\nif (n == (x - (y | z))) {"; goal = (x - (y | z)); goalFound = true; break;
                case 79: lines += "\n\nif (n == (x - (y ^ z))) {"; goal = (x - (y ^ z)); goalFound = true; break;
                case 80: lines += "\n\nif (n == (x * (y + z))) {"; goal = (x * (y + z)); goalFound = true; break;
                case 81: lines += "\n\nif (n == (x * (y - z))) {"; goal = (x * (y - z)); goalFound = true; break;
                case 82: lines += "\n\nif (n == (x * (y * z))) {"; goal = (x * (y * z)); goalFound = true; break;
                case 83: if (z == 0) { break; } lines += "\n\nif (n == (x * (y / z))) {"; goal = (x * (y / z)); goalFound = true; break;
                case 84: if (z == 0) { break; } lines += "\n\nif (n == (x * (y % z))) {"; goal = (x * (y % z)); goalFound = true; break;
                case 85: lines += "\n\nif (n == (x * (y & z))) {"; goal = (x * (y & z)); goalFound = true; break;
                case 86: lines += "\n\nif (n == (x * (y | z))) {"; goal = (x * (y | z)); goalFound = true; break;
                case 87: lines += "\n\nif (n == (x * (y ^ z))) {"; goal = (x * (y ^ z)); goalFound = true; break;
                case 88: if ((y + z) == 0) { break; } lines += "\n\nif (n == (x / (y + z))) {"; goal = (x / (y + z)); goalFound = true; break;
                case 89: if ((y - z) == 0) { break; } lines += "\n\nif (n == (x / (y - z))) {"; goal = (x / (y - z)); goalFound = true; break;
                case 90: if ((y * z) == 0) { break; } lines += "\n\nif (n == (x / (y * z))) {"; goal = (x / (y * z)); goalFound = true; break;
                case 91: if (y == 0 || z == 0) { break; } lines += "\n\nif (n == (x / (y / z))) {"; goal = (x / (y / z)); goalFound = true; break;
                case 92: if (y == 0 || z == 0 || (y % z) == 0) { break; } lines += "\n\nif (n == (x / (y % z))) {"; goal = (x / (y % z)); goalFound = true; break;
                case 93: if ((y & z) == 0) { break; } lines += "\n\nif (n == (x / (y & z))) {"; goal = (x / (y & z)); goalFound = true; break;
                case 94: if ((y | z) == 0) { break; } lines += "\n\nif (n == (x / (y | z))) {"; goal = (x / (y | z)); goalFound = true; break;
                case 95: if ((y ^ z) == 0) { break; } lines += "\n\nif (n == (x / (y ^ z))) {"; goal = (x / (y ^ z)); goalFound = true; break;
                case 96: if ((y + z) == 0) { break; } lines += "\n\nif (n == (x % (y + z))) {"; goal = (x % (y + z)); goalFound = true; break;
                case 97: if ((y - z) == 0) { break; } lines += "\n\nif (n == (x % (y - z))) {"; goal = (x % (y - z)); goalFound = true; break;
                case 98: if ((y * z) == 0) { break; } lines += "\n\nif (n == (x % (y * z))) {"; goal = (x % (y * z)); goalFound = true; break;
                case 99: if (y == 0 || z == 0) { break; } if ((y / z) == 0) { break; } lines += "\n\nif (n == (x % (y / z))) {"; goal = (x % (y / z)); goalFound = true; break;
                case 100: if (y == 0 || z == 0) { break; } if ((y % z) == 0) { break; } lines += "\n\nif (n == (x % (y % z))) {"; goal = (x % (y % z)); goalFound = true; break;
                case 101: if ((y & z) == 0) { break; } lines += "\n\nif (n == (x % (y & z))) {"; goal = (x % (y & z)); goalFound = true; break;
                case 102: if ((y | z) == 0) { break; } lines += "\n\nif (n == (x % (y | z))) {"; goal = (x % (y | z)); goalFound = true; break;
                case 103: if ((y ^ z) == 0) { break; } lines += "\n\nif (n == (x % (y ^ z))) {"; goal = (x % (y ^ z)); goalFound = true; break;
                case 104: lines += "\n\nif (n == (x & (y + z))) {"; goal = (x & (y + z)); goalFound = true; break;
                case 105: lines += "\n\nif (n == (x & (y - z))) {"; goal = (x & (y - z)); goalFound = true; break;
                case 106: lines += "\n\nif (n == (x & (y * z))) {"; goal = (x & (y * z)); goalFound = true; break;
                case 107: if (z == 0) { break; } lines += "\n\nif (n == (x & (y / z))) {"; goal = (x & (y / z)); goalFound = true; break;
                case 108: if (z == 0) { break; } lines += "\n\nif (n == (x & (y % z))) {"; goal = (x & (y % z)); goalFound = true; break;
                case 109: lines += "\n\nif (n == (x & (y & z))) {"; goal = (x & (y & z)); goalFound = true; break;
                case 110: lines += "\n\nif (n == (x & (y | z))) {"; goal = (x & (y | z)); goalFound = true; break;
                case 111: lines += "\n\nif (n == (x & (y ^ z))) {"; goal = (x & (y ^ z)); goalFound = true; break;
                case 112: lines += "\n\nif (n == (x | (y + z))) {"; goal = (x | (y + z)); goalFound = true; break;
                case 113: lines += "\n\nif (n == (x | (y - z))) {"; goal = (x | (y - z)); goalFound = true; break;
                case 114: lines += "\n\nif (n == (x | (y * z))) {"; goal = (x | (y * z)); goalFound = true; break;
                case 115: if (z == 0) { break; } lines += "\n\nif (n == (x | (y / z))) {"; goal = (x | (y / z)); goalFound = true; break;
                case 116: if (z == 0) { break; } lines += "\n\nif (n == (x | (y % z))) {"; goal = (x | (y % z)); goalFound = true; break;
                case 117: lines += "\n\nif (n == (x | (y & z))) {"; goal = (x | (y & z)); goalFound = true; break;
                case 118: lines += "\n\nif (n == (x | (y | z))) {"; goal = (x | (y | z)); goalFound = true; break;
                case 119: lines += "\n\nif (n == (x | (y ^ z))) {"; goal = (x | (y ^ z)); goalFound = true; break;
                case 120: lines += "\n\nif (n == (x ^ (y + z))) {"; goal = (x ^ (y + z)); goalFound = true; break;
                case 121: lines += "\n\nif (n == (x ^ (y - z))) {"; goal = (x ^ (y - z)); goalFound = true; break;
                case 122: lines += "\n\nif (n == (x ^ (y * z))) {"; goal = (x ^ (y * z)); goalFound = true; break;
                case 123: if (z == 0) { break; } lines += "\n\nif (n == (x ^ (y / z))) {"; goal = (x ^ (y / z)); goalFound = true; break;
                case 124: if (z == 0) { break; } lines += "\n\nif (n == (x ^ (y % z))) {"; goal = (x ^ (y % z)); goalFound = true; break;
                case 125: lines += "\n\nif (n == (x ^ (y & z))) {"; goal = (x ^ (y & z)); goalFound = true; break;
                case 126: lines += "\n\nif (n == (x ^ (y | z))) {"; goal = (x ^ (y | z)); goalFound = true; break;
                case 127: lines += "\n\nif (n == (x ^ (y ^ z))) {"; goal = (x ^ (y ^ z)); goalFound = true; break;
                case 128: lines += "\n\nif (n == ((x + z) + y)) {"; goal = ((x + z) + y); goalFound = true; break;
                case 129: lines += "\n\nif (n == ((x + z) - y)) {"; goal = ((x + z) - y); goalFound = true; break;
                case 130: lines += "\n\nif (n == ((x + z) * y)) {"; goal = ((x + z) * y); goalFound = true; break;
                case 131: if (y == 0) { break; } lines += "\n\nif (n == ((x + z) / y)) {"; goal = ((x + z) / y); goalFound = true; break;
                case 132: if (y == 0) { break; } lines += "\n\nif (n == ((x + z) % y)) {"; goal = ((x + z) % y); goalFound = true; break;
                case 133: lines += "\n\nif (n == ((x + z) & y)) {"; goal = ((x + z) & y); goalFound = true; break;
                case 134: lines += "\n\nif (n == ((x + z) | y)) {"; goal = ((x + z) | y); goalFound = true; break;
                case 135: lines += "\n\nif (n == ((x + z) ^ y)) {"; goal = ((x + z) ^ y); goalFound = true; break;
                case 136: lines += "\n\nif (n == ((x - z) + y)) {"; goal = ((x - z) + y); goalFound = true; break;
                case 137: lines += "\n\nif (n == ((x - z) - y)) {"; goal = ((x - z) - y); goalFound = true; break;
                case 138: lines += "\n\nif (n == ((x - z) * y)) {"; goal = ((x - z) * y); goalFound = true; break;
                case 139: if (y == 0) { break; } lines += "\n\nif (n == ((x - z) / y)) {"; goal = ((x - z) / y); goalFound = true; break;
                case 140: if (y == 0) { break; } lines += "\n\nif (n == ((x - z) % y)) {"; goal = ((x - z) % y); goalFound = true; break;
                case 141: lines += "\n\nif (n == ((x - z) & y)) {"; goal = ((x - z) & y); goalFound = true; break;
                case 142: lines += "\n\nif (n == ((x - z) | y)) {"; goal = ((x - z) | y); goalFound = true; break;
                case 143: lines += "\n\nif (n == ((x - z) ^ y)) {"; goal = ((x - z) ^ y); goalFound = true; break;
                case 144: lines += "\n\nif (n == ((x * z) + y)) {"; goal = ((x * z) + y); goalFound = true; break;
                case 145: lines += "\n\nif (n == ((x * z) - y)) {"; goal = ((x * z) - y); goalFound = true; break;
                case 146: lines += "\n\nif (n == ((x * z) * y)) {"; goal = ((x * z) * y); goalFound = true; break;
                case 147: if (y == 0) { break; } lines += "\n\nif (n == ((x * z) / y)) {"; goal = ((x * z) / y); goalFound = true; break;
                case 148: if (y == 0) { break; } lines += "\n\nif (n == ((x * z) % y)) {"; goal = ((x * z) % y); goalFound = true; break;
                case 149: lines += "\n\nif (n == ((x * z) & y)) {"; goal = ((x * z) & y); goalFound = true; break;
                case 150: lines += "\n\nif (n == ((x * z) | y)) {"; goal = ((x * z) | y); goalFound = true; break;
                case 151: lines += "\n\nif (n == ((x * z) ^ y)) {"; goal = ((x * z) ^ y); goalFound = true; break;
                case 152: if (z == 0) { break; } lines += "\n\nif (n == ((x / z) + y)) {"; goal = ((x / z) + y); goalFound = true; break;
                case 153: if (z == 0) { break; } lines += "\n\nif (n == ((x / z) - y)) {"; goal = ((x / z) - y); goalFound = true; break;
                case 154: if (z == 0) { break; } lines += "\n\nif (n == ((x / z) * y)) {"; goal = ((x / z) * y); goalFound = true; break;
                case 155: if (z == 0 || y == 0) { break; } lines += "\n\nif (n == ((x / z) / y)) {"; goal = ((x / z) / y); goalFound = true; break;
                case 156: if (z == 0 || y == 0) { break; } lines += "\n\nif (n == ((x / z) % y)) {"; goal = ((x / z) % y); goalFound = true; break;
                case 157: if (z == 0) { break; } lines += "\n\nif (n == ((x / z) & y)) {"; goal = ((x / z) & y); goalFound = true; break;
                case 158: if (z == 0) { break; } lines += "\n\nif (n == ((x / z) | y)) {"; goal = ((x / z) | y); goalFound = true; break;
                case 159: if (z == 0) { break; } lines += "\n\nif (n == ((x / z) ^ y)) {"; goal = ((x / z) ^ y); goalFound = true; break;
                case 160: if (z == 0) { break; } lines += "\n\nif (n == ((x % z) + y)) {"; goal = ((x % z) + y); goalFound = true; break;
                case 161: if (z == 0) { break; } lines += "\n\nif (n == ((x % z) - y)) {"; goal = ((x % z) - y); goalFound = true; break;
                case 162: if (z == 0) { break; } lines += "\n\nif (n == ((x % z) * y)) {"; goal = ((x % z) * y); goalFound = true; break;
                case 163: if (z == 0 || y == 0) { break; } lines += "\n\nif (n == ((x % z) / y)) {"; goal = ((x % z) / y); goalFound = true; break;
                case 164: if (z == 0 || y == 0) { break; } lines += "\n\nif (n == ((x % z) % y)) {"; goal = ((x % z) % y); goalFound = true; break;
                case 165: if (z == 0) { break; } lines += "\n\nif (n == ((x % z) & y)) {"; goal = ((x % z) & y); goalFound = true; break;
                case 166: if (z == 0) { break; } lines += "\n\nif (n == ((x % z) | y)) {"; goal = ((x % z) | y); goalFound = true; break;
                case 167: if (z == 0) { break; } lines += "\n\nif (n == ((x % z) ^ y)) {"; goal = ((x % z) ^ y); goalFound = true; break;
                case 168: lines += "\n\nif (n == ((x & z) + y)) {"; goal = ((x & z) + y); goalFound = true; break;
                case 169: lines += "\n\nif (n == ((x & z) - y)) {"; goal = ((x & z) - y); goalFound = true; break;
                case 170: lines += "\n\nif (n == ((x & z) * y)) {"; goal = ((x & z) * y); goalFound = true; break;
                case 171: if (y == 0) { break; } lines += "\n\nif (n == ((x & z) / y)) {"; goal = ((x & z) / y); goalFound = true; break;
                case 172: if (y == 0) { break; } lines += "\n\nif (n == ((x & z) % y)) {"; goal = ((x & z) % y); goalFound = true; break;
                case 173: lines += "\n\nif (n == ((x & z) & y)) {"; goal = ((x & z) & y); goalFound = true; break;
                case 174: lines += "\n\nif (n == ((x & z) | y)) {"; goal = ((x & z) | y); goalFound = true; break;
                case 175: lines += "\n\nif (n == ((x & z) ^ y)) {"; goal = ((x & z) ^ y); goalFound = true; break;
                case 176: lines += "\n\nif (n == ((x | z) + y)) {"; goal = ((x | z) + y); goalFound = true; break;
                case 177: lines += "\n\nif (n == ((x | z) - y)) {"; goal = ((x | z) - y); goalFound = true; break;
                case 178: lines += "\n\nif (n == ((x | z) * y)) {"; goal = ((x | z) * y); goalFound = true; break;
                case 179: if (y == 0) { break; } lines += "\n\nif (n == ((x | z) / y)) {"; goal = ((x | z) / y); goalFound = true; break;
                case 180: if (y == 0) { break; } lines += "\n\nif (n == ((x | z) % y)) {"; goal = ((x | z) % y); goalFound = true; break;
                case 181: lines += "\n\nif (n == ((x | z) & y)) {"; goal = ((x | z) & y); goalFound = true; break;
                case 182: lines += "\n\nif (n == ((x | z) | y)) {"; goal = ((x | z) | y); goalFound = true; break;
                case 183: lines += "\n\nif (n == ((x | z) ^ y)) {"; goal = ((x | z) ^ y); goalFound = true; break;
                case 184: lines += "\n\nif (n == ((x ^ z) + y)) {"; goal = ((x ^ z) + y); goalFound = true; break;
                case 185: lines += "\n\nif (n == ((x ^ z) - y)) {"; goal = ((x ^ z) - y); goalFound = true; break;
                case 186: lines += "\n\nif (n == ((x ^ z) * y)) {"; goal = ((x ^ z) * y); goalFound = true; break;
                case 187: if (y == 0) { break; } lines += "\n\nif (n == ((x ^ z) / y)) {"; goal = ((x ^ z) / y); goalFound = true; break;
                case 188: if (y == 0) { break; } lines += "\n\nif (n == ((x ^ z) % y)) {"; goal = ((x ^ z) % y); goalFound = true; break;
                case 189: lines += "\n\nif (n == ((x ^ z) & y)) {"; goal = ((x ^ z) & y); goalFound = true; break;
                case 190: lines += "\n\nif (n == ((x ^ z) | y)) {"; goal = ((x ^ z) | y); goalFound = true; break;
                case 191: lines += "\n\nif (n == ((x ^ z) ^ y)) {"; goal = ((x ^ z) ^ y); goalFound = true; break;
                case 192: lines += "\n\nif (n == (x + (z + y))) {"; goal = (x + (z + y)); goalFound = true; break;
                case 193: lines += "\n\nif (n == (x + (z - y))) {"; goal = (x + (z - y)); goalFound = true; break;
                case 194: lines += "\n\nif (n == (x + (z * y))) {"; goal = (x + (z * y)); goalFound = true; break;
                case 195: if (y == 0) { break; } lines += "\n\nif (n == (x + (z / y))) {"; goal = (x + (z / y)); goalFound = true; break;
                case 196: if (y == 0) { break; } lines += "\n\nif (n == (x + (z % y))) {"; goal = (x + (z % y)); goalFound = true; break;
                case 197: lines += "\n\nif (n == (x + (z & y))) {"; goal = (x + (z & y)); goalFound = true; break;
                case 198: lines += "\n\nif (n == (x + (z | y))) {"; goal = (x + (z | y)); goalFound = true; break;
                case 199: lines += "\n\nif (n == (x + (z ^ y))) {"; goal = (x + (z ^ y)); goalFound = true; break;
                case 200: lines += "\n\nif (n == (x - (z + y))) {"; goal = (x - (z + y)); goalFound = true; break;
                case 201: lines += "\n\nif (n == (x - (z - y))) {"; goal = (x - (z - y)); goalFound = true; break;
                case 202: lines += "\n\nif (n == (x - (z * y))) {"; goal = (x - (z * y)); goalFound = true; break;
                case 203: if (y == 0) { break; } lines += "\n\nif (n == (x - (z / y))) {"; goal = (x - (z / y)); goalFound = true; break;
                case 204: if (y == 0) { break; } lines += "\n\nif (n == (x - (z % y))) {"; goal = (x - (z % y)); goalFound = true; break;
                case 205: lines += "\n\nif (n == (x - (z & y))) {"; goal = (x - (z & y)); goalFound = true; break;
                case 206: lines += "\n\nif (n == (x - (z | y))) {"; goal = (x - (z | y)); goalFound = true; break;
                case 207: lines += "\n\nif (n == (x - (z ^ y))) {"; goal = (x - (z ^ y)); goalFound = true; break;
                case 208: lines += "\n\nif (n == (x * (z + y))) {"; goal = (x * (z + y)); goalFound = true; break;
                case 209: lines += "\n\nif (n == (x * (z - y))) {"; goal = (x * (z - y)); goalFound = true; break;
                case 210: lines += "\n\nif (n == (x * (z * y))) {"; goal = (x * (z * y)); goalFound = true; break;
                case 211: if (y == 0) { break; } lines += "\n\nif (n == (x * (z / y))) {"; goal = (x * (z / y)); goalFound = true; break;
                case 212: if (y == 0) { break; } lines += "\n\nif (n == (x * (z % y))) {"; goal = (x * (z % y)); goalFound = true; break;
                case 213: lines += "\n\nif (n == (x * (z & y))) {"; goal = (x * (z & y)); goalFound = true; break;
                case 214: lines += "\n\nif (n == (x * (z | y))) {"; goal = (x * (z | y)); goalFound = true; break;
                case 215: lines += "\n\nif (n == (x * (z ^ y))) {"; goal = (x * (z ^ y)); goalFound = true; break;
                case 216: if ((z + y) == 0) { break; } lines += "\n\nif (n == (x / (z + y))) {"; goal = (x / (z + y)); goalFound = true; break;
                case 217: if ((z - y) == 0) { break; } lines += "\n\nif (n == (x / (z - y))) {"; goal = (x / (z - y)); goalFound = true; break;
                case 218: if ((z * y) == 0) { break; } lines += "\n\nif (n == (x / (z * y))) {"; goal = (x / (z * y)); goalFound = true; break;
                case 219: if (z == 0 || y == 0) { break; } lines += "\n\nif (n == (x / (z / y))) {"; goal = (x / (z / y)); goalFound = true; break;
                case 220: if (z == 0 || y == 0 || (x%y) == 0) { break; } lines += "\n\nif (n == (x / (z % y))) {"; goal = (x / (z % y)); goalFound = true; break;
                case 221: if ((z & y) == 0) { break; } lines += "\n\nif (n == (x / (z & y))) {"; goal = (x / (z & y)); goalFound = true; break;
                case 222: if ((z | y) == 0) { break; } lines += "\n\nif (n == (x / (z | y))) {"; goal = (x / (z | y)); goalFound = true; break;
                case 223: if ((z ^ y) == 0) { break; } lines += "\n\nif (n == (x / (z ^ y))) {"; goal = (x / (z ^ y)); goalFound = true; break;
                case 224: if ((z + y) == 0) { break; } lines += "\n\nif (n == (x % (z + y))) {"; goal = (x % (z + y)); goalFound = true; break;
                case 225: if ((z - y) == 0) { break; } lines += "\n\nif (n == (x % (z - y))) {"; goal = (x % (z - y)); goalFound = true; break;
                case 226: if ((z * y) == 0) { break; } lines += "\n\nif (n == (x % (z * y))) {"; goal = (x % (z * y)); goalFound = true; break;
                case 227: if (z == 0 || y == 0) { break; } if ((z / y) == 0) { break; } lines += "\n\nif (n == (x % (z / y))) {"; goal = (x % (z / y)); goalFound = true; break;
                case 228: if (z == 0 || y == 0) { break; } if ((z % y) == 0) { break; } lines += "\n\nif (n == (x % (z % y))) {"; goal = (x % (z % y)); goalFound = true; break;
                case 229: if ((z & y) == 0) { break; } lines += "\n\nif (n == (x % (z & y))) {"; goal = (x % (z & y)); goalFound = true; break;
                case 230: if ((z | y) == 0) { break; } lines += "\n\nif (n == (x % (z | y))) {"; goal = (x % (z | y)); goalFound = true; break;
                case 231: if ((z ^ y) == 0) { break; } lines += "\n\nif (n == (x % (z ^ y))) {"; goal = (x % (z ^ y)); goalFound = true; break;
                case 232: lines += "\n\nif (n == (x & (z + y))) {"; goal = (x & (z + y)); goalFound = true; break;
                case 233: lines += "\n\nif (n == (x & (z - y))) {"; goal = (x & (z - y)); goalFound = true; break;
                case 234: lines += "\n\nif (n == (x & (z * y))) {"; goal = (x & (z * y)); goalFound = true; break;
                case 235: if (y == 0) { break; } lines += "\n\nif (n == (x & (z / y))) {"; goal = (x & (z / y)); goalFound = true; break;
                case 236: if (y == 0) { break; } lines += "\n\nif (n == (x & (z % y))) {"; goal = (x & (z % y)); goalFound = true; break;
                case 237: lines += "\n\nif (n == (x & (z & y))) {"; goal = (x & (z & y)); goalFound = true; break;
                case 238: lines += "\n\nif (n == (x & (z | y))) {"; goal = (x & (z | y)); goalFound = true; break;
                case 239: lines += "\n\nif (n == (x & (z ^ y))) {"; goal = (x & (z ^ y)); goalFound = true; break;
                case 240: lines += "\n\nif (n == (x | (z + y))) {"; goal = (x | (z + y)); goalFound = true; break;
                case 241: lines += "\n\nif (n == (x | (z - y))) {"; goal = (x | (z - y)); goalFound = true; break;
                case 242: lines += "\n\nif (n == (x | (z * y))) {"; goal = (x | (z * y)); goalFound = true; break;
                case 243: if (y == 0) { break; } lines += "\n\nif (n == (x | (z / y))) {"; goal = (x | (z / y)); goalFound = true; break;
                case 244: if (y == 0) { break; } lines += "\n\nif (n == (x | (z % y))) {"; goal = (x | (z % y)); goalFound = true; break;
                case 245: lines += "\n\nif (n == (x | (z & y))) {"; goal = (x | (z & y)); goalFound = true; break;
                case 246: lines += "\n\nif (n == (x | (z | y))) {"; goal = (x | (z | y)); goalFound = true; break;
                case 247: lines += "\n\nif (n == (x | (z ^ y))) {"; goal = (x | (z ^ y)); goalFound = true; break;
                case 248: lines += "\n\nif (n == (x ^ (z + y))) {"; goal = (x ^ (z + y)); goalFound = true; break;
                case 249: lines += "\n\nif (n == (x ^ (z - y))) {"; goal = (x ^ (z - y)); goalFound = true; break;
                case 250: lines += "\n\nif (n == (x ^ (z * y))) {"; goal = (x ^ (z * y)); goalFound = true; break;
                case 251: if (y == 0) { break; } lines += "\n\nif (n == (x ^ (z / y))) {"; goal = (x ^ (z / y)); goalFound = true; break;
                case 252: if (y == 0) { break; } lines += "\n\nif (n == (x ^ (z % y))) {"; goal = (x ^ (z % y)); goalFound = true; break;
                case 253: lines += "\n\nif (n == (x ^ (z & y))) {"; goal = (x ^ (z & y)); goalFound = true; break;
                case 254: lines += "\n\nif (n == (x ^ (z | y))) {"; goal = (x ^ (z | y)); goalFound = true; break;
                case 255: lines += "\n\nif (n == (x ^ (z ^ y))) {"; goal = (x ^ (z ^ y)); goalFound = true; break;
                case 256: lines += "\n\nif (n == ((y + x) + z)) {"; goal = ((y + x) + z); goalFound = true; break;
                case 257: lines += "\n\nif (n == ((y + x) - z)) {"; goal = ((y + x) - z); goalFound = true; break;
                case 258: lines += "\n\nif (n == ((y + x) * z)) {"; goal = ((y + x) * z); goalFound = true; break;
                case 259: if (z == 0) { break; } lines += "\n\nif (n == ((y + x) / z)) {"; goal = ((y + x) / z); goalFound = true; break;
                case 260: if (z == 0) { break; } lines += "\n\nif (n == ((y + x) % z)) {"; goal = ((y + x) % z); goalFound = true; break;
                case 261: lines += "\n\nif (n == ((y + x) & z)) {"; goal = ((y + x) & z); goalFound = true; break;
                case 262: lines += "\n\nif (n == ((y + x) | z)) {"; goal = ((y + x) | z); goalFound = true; break;
                case 263: lines += "\n\nif (n == ((y + x) ^ z)) {"; goal = ((y + x) ^ z); goalFound = true; break;
                case 264: lines += "\n\nif (n == ((y - x) + z)) {"; goal = ((y - x) + z); goalFound = true; break;
                case 265: lines += "\n\nif (n == ((y - x) - z)) {"; goal = ((y - x) - z); goalFound = true; break;
                case 266: lines += "\n\nif (n == ((y - x) * z)) {"; goal = ((y - x) * z); goalFound = true; break;
                case 267: if (z == 0) { break; } lines += "\n\nif (n == ((y - x) / z)) {"; goal = ((y - x) / z); goalFound = true; break;
                case 268: if (z == 0) { break; } lines += "\n\nif (n == ((y - x) % z)) {"; goal = ((y - x) % z); goalFound = true; break;
                case 269: lines += "\n\nif (n == ((y - x) & z)) {"; goal = ((y - x) & z); goalFound = true; break;
                case 270: lines += "\n\nif (n == ((y - x) | z)) {"; goal = ((y - x) | z); goalFound = true; break;
                case 271: lines += "\n\nif (n == ((y - x) ^ z)) {"; goal = ((y - x) ^ z); goalFound = true; break;
                case 272: lines += "\n\nif (n == ((y * x) + z)) {"; goal = ((y * x) + z); goalFound = true; break;
                case 273: lines += "\n\nif (n == ((y * x) - z)) {"; goal = ((y * x) - z); goalFound = true; break;
                case 274: lines += "\n\nif (n == ((y * x) * z)) {"; goal = ((y * x) * z); goalFound = true; break;
                case 275: if (z == 0) { break; } lines += "\n\nif (n == ((y * x) / z)) {"; goal = ((y * x) / z); goalFound = true; break;
                case 276: if (z == 0) { break; } lines += "\n\nif (n == ((y * x) % z)) {"; goal = ((y * x) % z); goalFound = true; break;
                case 277: lines += "\n\nif (n == ((y * x) & z)) {"; goal = ((y * x) & z); goalFound = true; break;
                case 278: lines += "\n\nif (n == ((y * x) | z)) {"; goal = ((y * x) | z); goalFound = true; break;
                case 279: lines += "\n\nif (n == ((y * x) ^ z)) {"; goal = ((y * x) ^ z); goalFound = true; break;
                case 280: if (x == 0) { break; } lines += "\n\nif (n == ((y / x) + z)) {"; goal = ((y / x) + z); goalFound = true; break;
                case 281: if (x == 0) { break; } lines += "\n\nif (n == ((y / x) - z)) {"; goal = ((y / x) - z); goalFound = true; break;
                case 282: if (x == 0) { break; } lines += "\n\nif (n == ((y / x) * z)) {"; goal = ((y / x) * z); goalFound = true; break;
                case 283: if (x == 0 || z == 0) { break; } lines += "\n\nif (n == ((y / x) / z)) {"; goal = ((y / x) / z); goalFound = true; break;
                case 284: if (x == 0 || z == 0) { break; } lines += "\n\nif (n == ((y / x) % z)) {"; goal = ((y / x) % z); goalFound = true; break;
                case 285: if (x == 0) { break; } lines += "\n\nif (n == ((y / x) & z)) {"; goal = ((y / x) & z); goalFound = true; break;
                case 286: if (x == 0) { break; } lines += "\n\nif (n == ((y / x) | z)) {"; goal = ((y / x) | z); goalFound = true; break;
                case 287: if (x == 0) { break; } lines += "\n\nif (n == ((y / x) ^ z)) {"; goal = ((y / x) ^ z); goalFound = true; break;
                case 288: if (x == 0) { break; } lines += "\n\nif (n == ((y % x) + z)) {"; goal = ((y % x) + z); goalFound = true; break;
                case 289: if (x == 0) { break; } lines += "\n\nif (n == ((y % x) - z)) {"; goal = ((y % x) - z); goalFound = true; break;
                case 290: if (x == 0) { break; } lines += "\n\nif (n == ((y % x) * z)) {"; goal = ((y % x) * z); goalFound = true; break;
                case 291: if (x == 0 || z == 0) { break; } lines += "\n\nif (n == ((y % x) / z)) {"; goal = ((y % x) / z); goalFound = true; break;
                case 292: if (x == 0 || z == 0) { break; } lines += "\n\nif (n == ((y % x) % z)) {"; goal = ((y % x) % z); goalFound = true; break;
                case 293: if (x == 0) { break; } lines += "\n\nif (n == ((y % x) & z)) {"; goal = ((y % x) & z); goalFound = true; break;
                case 294: if (x == 0) { break; } lines += "\n\nif (n == ((y % x) | z)) {"; goal = ((y % x) | z); goalFound = true; break;
                case 295: if (x == 0) { break; } lines += "\n\nif (n == ((y % x) ^ z)) {"; goal = ((y % x) ^ z); goalFound = true; break;
                case 296: lines += "\n\nif (n == ((y & x) + z)) {"; goal = ((y & x) + z); goalFound = true; break;
                case 297: lines += "\n\nif (n == ((y & x) - z)) {"; goal = ((y & x) - z); goalFound = true; break;
                case 298: lines += "\n\nif (n == ((y & x) * z)) {"; goal = ((y & x) * z); goalFound = true; break;
                case 299: if (z == 0) { break; } lines += "\n\nif (n == ((y & x) / z)) {"; goal = ((y & x) / z); goalFound = true; break;
                case 300: if (z == 0) { break; } lines += "\n\nif (n == ((y & x) % z)) {"; goal = ((y & x) % z); goalFound = true; break;
                case 301: lines += "\n\nif (n == ((y & x) & z)) {"; goal = ((y & x) & z); goalFound = true; break;
                case 302: lines += "\n\nif (n == ((y & x) | z)) {"; goal = ((y & x) | z); goalFound = true; break;
                case 303: lines += "\n\nif (n == ((y & x) ^ z)) {"; goal = ((y & x) ^ z); goalFound = true; break;
                case 304: lines += "\n\nif (n == ((y | x) + z)) {"; goal = ((y | x) + z); goalFound = true; break;
                case 305: lines += "\n\nif (n == ((y | x) - z)) {"; goal = ((y | x) - z); goalFound = true; break;
                case 306: lines += "\n\nif (n == ((y | x) * z)) {"; goal = ((y | x) * z); goalFound = true; break;
                case 307: if (z == 0) { break; } lines += "\n\nif (n == ((y | x) / z)) {"; goal = ((y | x) / z); goalFound = true; break;
                case 308: if (z == 0) { break; } lines += "\n\nif (n == ((y | x) % z)) {"; goal = ((y | x) % z); goalFound = true; break;
                case 309: lines += "\n\nif (n == ((y | x) & z)) {"; goal = ((y | x) & z); goalFound = true; break;
                case 310: lines += "\n\nif (n == ((y | x) | z)) {"; goal = ((y | x) | z); goalFound = true; break;
                case 311: lines += "\n\nif (n == ((y | x) ^ z)) {"; goal = ((y | x) ^ z); goalFound = true; break;
                case 312: lines += "\n\nif (n == ((y ^ x) + z)) {"; goal = ((y ^ x) + z); goalFound = true; break;
                case 313: lines += "\n\nif (n == ((y ^ x) - z)) {"; goal = ((y ^ x) - z); goalFound = true; break;
                case 314: lines += "\n\nif (n == ((y ^ x) * z)) {"; goal = ((y ^ x) * z); goalFound = true; break;
                case 315: if (z == 0) { break; } lines += "\n\nif (n == ((y ^ x) / z)) {"; goal = ((y ^ x) / z); goalFound = true; break;
                case 316: if (z == 0) { break; } lines += "\n\nif (n == ((y ^ x) % z)) {"; goal = ((y ^ x) % z); goalFound = true; break;
                case 317: lines += "\n\nif (n == ((y ^ x) & z)) {"; goal = ((y ^ x) & z); goalFound = true; break;
                case 318: lines += "\n\nif (n == ((y ^ x) | z)) {"; goal = ((y ^ x) | z); goalFound = true; break;
                case 319: lines += "\n\nif (n == ((y ^ x) ^ z)) {"; goal = ((y ^ x) ^ z); goalFound = true; break;
                case 320: lines += "\n\nif (n == (y + (x + z))) {"; goal = (y + (x + z)); goalFound = true; break;
                case 321: lines += "\n\nif (n == (y + (x - z))) {"; goal = (y + (x - z)); goalFound = true; break;
                case 322: lines += "\n\nif (n == (y + (x * z))) {"; goal = (y + (x * z)); goalFound = true; break;
                case 323: if (z == 0) { break; } lines += "\n\nif (n == (y + (x / z))) {"; goal = (y + (x / z)); goalFound = true; break;
                case 324: if (z == 0) { break; } lines += "\n\nif (n == (y + (x % z))) {"; goal = (y + (x % z)); goalFound = true; break;
                case 325: lines += "\n\nif (n == (y + (x & z))) {"; goal = (y + (x & z)); goalFound = true; break;
                case 326: lines += "\n\nif (n == (y + (x | z))) {"; goal = (y + (x | z)); goalFound = true; break;
                case 327: lines += "\n\nif (n == (y + (x ^ z))) {"; goal = (y + (x ^ z)); goalFound = true; break;
                case 328: lines += "\n\nif (n == (y - (x + z))) {"; goal = (y - (x + z)); goalFound = true; break;
                case 329: lines += "\n\nif (n == (y - (x - z))) {"; goal = (y - (x - z)); goalFound = true; break;
                case 330: lines += "\n\nif (n == (y - (x * z))) {"; goal = (y - (x * z)); goalFound = true; break;
                case 331: if (z == 0) { break; } lines += "\n\nif (n == (y - (x / z))) {"; goal = (y - (x / z)); goalFound = true; break;
                case 332: if (z == 0) { break; } lines += "\n\nif (n == (y - (x % z))) {"; goal = (y - (x % z)); goalFound = true; break;
                case 333: lines += "\n\nif (n == (y - (x & z))) {"; goal = (y - (x & z)); goalFound = true; break;
                case 334: lines += "\n\nif (n == (y - (x | z))) {"; goal = (y - (x | z)); goalFound = true; break;
                case 335: lines += "\n\nif (n == (y - (x ^ z))) {"; goal = (y - (x ^ z)); goalFound = true; break;
                case 336: lines += "\n\nif (n == (y * (x + z))) {"; goal = (y * (x + z)); goalFound = true; break;
                case 337: lines += "\n\nif (n == (y * (x - z))) {"; goal = (y * (x - z)); goalFound = true; break;
                case 338: lines += "\n\nif (n == (y * (x * z))) {"; goal = (y * (x * z)); goalFound = true; break;
                case 339: if (z == 0) { break; } lines += "\n\nif (n == (y * (x / z))) {"; goal = (y * (x / z)); goalFound = true; break;
                case 340: if (z == 0) { break; } lines += "\n\nif (n == (y * (x % z))) {"; goal = (y * (x % z)); goalFound = true; break;
                case 341: lines += "\n\nif (n == (y * (x & z))) {"; goal = (y * (x & z)); goalFound = true; break;
                case 342: lines += "\n\nif (n == (y * (x | z))) {"; goal = (y * (x | z)); goalFound = true; break;
                case 343: lines += "\n\nif (n == (y * (x ^ z))) {"; goal = (y * (x ^ z)); goalFound = true; break;
                case 344: if ((x + z) == 0) { break; } lines += "\n\nif (n == (y / (x + z))) {"; goal = (y / (x + z)); goalFound = true; break;
                case 345: if ((x - z) == 0) { break; } lines += "\n\nif (n == (y / (x - z))) {"; goal = (y / (x - z)); goalFound = true; break;
                case 346: if ((x * z) == 0) { break; } lines += "\n\nif (n == (y / (x * z))) {"; goal = (y / (x * z)); goalFound = true; break;
                case 347: if (x == 0 || z == 0) { break; } lines += "\n\nif (n == (y / (x / z))) {"; goal = (y / (x / z)); goalFound = true; break;
                case 348: if (x == 0 || z == 0 || (x%z) == 0) { break; } lines += "\n\nif (n == (y / (x % z))) {"; goal = (y / (x % z)); goalFound = true; break;
                case 349: if ((x & z) == 0) { break; } lines += "\n\nif (n == (y / (x & z))) {"; goal = (y / (x & z)); goalFound = true; break;
                case 350: if ((x | z) == 0) { break; } lines += "\n\nif (n == (y / (x | z))) {"; goal = (y / (x | z)); goalFound = true; break;
                case 351: if ((x ^ z) == 0) { break; } lines += "\n\nif (n == (y / (x ^ z))) {"; goal = (y / (x ^ z)); goalFound = true; break;
                case 352: if ((x + z) == 0) { break; } lines += "\n\nif (n == (y % (x + z))) {"; goal = (y % (x + z)); goalFound = true; break;
                case 353: if ((x - z) == 0) { break; } lines += "\n\nif (n == (y % (x - z))) {"; goal = (y % (x - z)); goalFound = true; break;
                case 354: if ((x * z) == 0) { break; } lines += "\n\nif (n == (y % (x * z))) {"; goal = (y % (x * z)); goalFound = true; break;
                case 355: if (x == 0 || z == 0) { break; } if ((x / z) == 0) { break; } lines += "\n\nif (n == (y % (x / z))) {"; goal = (y % (x / z)); goalFound = true; break;
                case 356: if (x == 0 || z == 0) { break; } if ((x % z) == 0) { break; } lines += "\n\nif (n == (y % (x % z))) {"; goal = (y % (x % z)); goalFound = true; break;
                case 357: if ((x & z) == 0) { break; } lines += "\n\nif (n == (y % (x & z))) {"; goal = (y % (x & z)); goalFound = true; break;
                case 358: if ((x | z) == 0) { break; } lines += "\n\nif (n == (y % (x | z))) {"; goal = (y % (x | z)); goalFound = true; break;
                case 359: if ((x ^ z) == 0) { break; } lines += "\n\nif (n == (y % (x ^ z))) {"; goal = (y % (x ^ z)); goalFound = true; break;
                case 360: lines += "\n\nif (n == (y & (x + z))) {"; goal = (y & (x + z)); goalFound = true; break;
                case 361: lines += "\n\nif (n == (y & (x - z))) {"; goal = (y & (x - z)); goalFound = true; break;
                case 362: lines += "\n\nif (n == (y & (x * z))) {"; goal = (y & (x * z)); goalFound = true; break;
                case 363: if (z == 0) { break; } lines += "\n\nif (n == (y & (x / z))) {"; goal = (y & (x / z)); goalFound = true; break;
                case 364: if (z == 0) { break; } lines += "\n\nif (n == (y & (x % z))) {"; goal = (y & (x % z)); goalFound = true; break;
                case 365: lines += "\n\nif (n == (y & (x & z))) {"; goal = (y & (x & z)); goalFound = true; break;
                case 366: lines += "\n\nif (n == (y & (x | z))) {"; goal = (y & (x | z)); goalFound = true; break;
                case 367: lines += "\n\nif (n == (y & (x ^ z))) {"; goal = (y & (x ^ z)); goalFound = true; break;
                case 368: lines += "\n\nif (n == (y | (x + z))) {"; goal = (y | (x + z)); goalFound = true; break;
                case 369: lines += "\n\nif (n == (y | (x - z))) {"; goal = (y | (x - z)); goalFound = true; break;
                case 370: lines += "\n\nif (n == (y | (x * z))) {"; goal = (y | (x * z)); goalFound = true; break;
                case 371: if (z == 0) { break; } lines += "\n\nif (n == (y | (x / z))) {"; goal = (y | (x / z)); goalFound = true; break;
                case 372: if (z == 0) { break; } lines += "\n\nif (n == (y | (x % z))) {"; goal = (y | (x % z)); goalFound = true; break;
                case 373: lines += "\n\nif (n == (y | (x & z))) {"; goal = (y | (x & z)); goalFound = true; break;
                case 374: lines += "\n\nif (n == (y | (x | z))) {"; goal = (y | (x | z)); goalFound = true; break;
                case 375: lines += "\n\nif (n == (y | (x ^ z))) {"; goal = (y | (x ^ z)); goalFound = true; break;
                case 376: lines += "\n\nif (n == (y ^ (x + z))) {"; goal = (y ^ (x + z)); goalFound = true; break;
                case 377: lines += "\n\nif (n == (y ^ (x - z))) {"; goal = (y ^ (x - z)); goalFound = true; break;
                case 378: lines += "\n\nif (n == (y ^ (x * z))) {"; goal = (y ^ (x * z)); goalFound = true; break;
                case 379: if (z == 0) { break; } lines += "\n\nif (n == (y ^ (x / z))) {"; goal = (y ^ (x / z)); goalFound = true; break;
                case 380: if (z == 0) { break; } lines += "\n\nif (n == (y ^ (x % z))) {"; goal = (y ^ (x % z)); goalFound = true; break;
                case 381: lines += "\n\nif (n == (y ^ (x & z))) {"; goal = (y ^ (x & z)); goalFound = true; break;
                case 382: lines += "\n\nif (n == (y ^ (x | z))) {"; goal = (y ^ (x | z)); goalFound = true; break;
                case 383: lines += "\n\nif (n == (y ^ (x ^ z))) {"; goal = (y ^ (x ^ z)); goalFound = true; break;
                case 384: lines += "\n\nif (n == ((y + z) + x)) {"; goal = ((y + z) + x); goalFound = true; break;
                case 385: lines += "\n\nif (n == ((y + z) - x)) {"; goal = ((y + z) - x); goalFound = true; break;
                case 386: lines += "\n\nif (n == ((y + z) * x)) {"; goal = ((y + z) * x); goalFound = true; break;
                case 387: if (x == 0) { break; } lines += "\n\nif (n == ((y + z) / x)) {"; goal = ((y + z) / x); goalFound = true; break;
                case 388: if (x == 0) { break; } lines += "\n\nif (n == ((y + z) % x)) {"; goal = ((y + z) % x); goalFound = true; break;
                case 389: lines += "\n\nif (n == ((y + z) & x)) {"; goal = ((y + z) & x); goalFound = true; break;
                case 390: lines += "\n\nif (n == ((y + z) | x)) {"; goal = ((y + z) | x); goalFound = true; break;
                case 391: lines += "\n\nif (n == ((y + z) ^ x)) {"; goal = ((y + z) ^ x); goalFound = true; break;
                case 392: lines += "\n\nif (n == ((y - z) + x)) {"; goal = ((y - z) + x); goalFound = true; break;
                case 393: lines += "\n\nif (n == ((y - z) - x)) {"; goal = ((y - z) - x); goalFound = true; break;
                case 394: lines += "\n\nif (n == ((y - z) * x)) {"; goal = ((y - z) * x); goalFound = true; break;
                case 395: if (x == 0) { break; } lines += "\n\nif (n == ((y - z) / x)) {"; goal = ((y - z) / x); goalFound = true; break;
                case 396: if (x == 0) { break; } lines += "\n\nif (n == ((y - z) % x)) {"; goal = ((y - z) % x); goalFound = true; break;
                case 397: lines += "\n\nif (n == ((y - z) & x)) {"; goal = ((y - z) & x); goalFound = true; break;
                case 398: lines += "\n\nif (n == ((y - z) | x)) {"; goal = ((y - z) | x); goalFound = true; break;
                case 399: lines += "\n\nif (n == ((y - z) ^ x)) {"; goal = ((y - z) ^ x); goalFound = true; break;
                case 400: lines += "\n\nif (n == ((y * z) + x)) {"; goal = ((y * z) + x); goalFound = true; break;
                case 401: lines += "\n\nif (n == ((y * z) - x)) {"; goal = ((y * z) - x); goalFound = true; break;
                case 402: lines += "\n\nif (n == ((y * z) * x)) {"; goal = ((y * z) * x); goalFound = true; break;
                case 403: if (x == 0) { break; } lines += "\n\nif (n == ((y * z) / x)) {"; goal = ((y * z) / x); goalFound = true; break;
                case 404: if (x == 0) { break; } lines += "\n\nif (n == ((y * z) % x)) {"; goal = ((y * z) % x); goalFound = true; break;
                case 405: lines += "\n\nif (n == ((y * z) & x)) {"; goal = ((y * z) & x); goalFound = true; break;
                case 406: lines += "\n\nif (n == ((y * z) | x)) {"; goal = ((y * z) | x); goalFound = true; break;
                case 407: lines += "\n\nif (n == ((y * z) ^ x)) {"; goal = ((y * z) ^ x); goalFound = true; break;
                case 408: if (z == 0) { break; } lines += "\n\nif (n == ((y / z) + x)) {"; goal = ((y / z) + x); goalFound = true; break;
                case 409: if (z == 0) { break; } lines += "\n\nif (n == ((y / z) - x)) {"; goal = ((y / z) - x); goalFound = true; break;
                case 410: if (z == 0) { break; } lines += "\n\nif (n == ((y / z) * x)) {"; goal = ((y / z) * x); goalFound = true; break;
                case 411: if (z == 0 || x == 0) { break; } lines += "\n\nif (n == ((y / z) / x)) {"; goal = ((y / z) / x); goalFound = true; break;
                case 412: if (z == 0 || x == 0) { break; } lines += "\n\nif (n == ((y / z) % x)) {"; goal = ((y / z) % x); goalFound = true; break;
                case 413: if (z == 0) { break; } lines += "\n\nif (n == ((y / z) & x)) {"; goal = ((y / z) & x); goalFound = true; break;
                case 414: if (z == 0) { break; } lines += "\n\nif (n == ((y / z) | x)) {"; goal = ((y / z) | x); goalFound = true; break;
                case 415: if (z == 0) { break; } lines += "\n\nif (n == ((y / z) ^ x)) {"; goal = ((y / z) ^ x); goalFound = true; break;
                case 416: if (z == 0) { break; } lines += "\n\nif (n == ((y % z) + x)) {"; goal = ((y % z) + x); goalFound = true; break;
                case 417: if (z == 0) { break; } lines += "\n\nif (n == ((y % z) - x)) {"; goal = ((y % z) - x); goalFound = true; break;
                case 418: if (z == 0) { break; } lines += "\n\nif (n == ((y % z) * x)) {"; goal = ((y % z) * x); goalFound = true; break;
                case 419: if (z == 0 || x == 0) { break; } lines += "\n\nif (n == ((y % z) / x)) {"; goal = ((y % z) / x); goalFound = true; break;
                case 420: if (z == 0 || x == 0) { break; } lines += "\n\nif (n == ((y % z) % x)) {"; goal = ((y % z) % x); goalFound = true; break;
                case 421: if (z == 0) { break; } lines += "\n\nif (n == ((y % z) & x)) {"; goal = ((y % z) & x); goalFound = true; break;
                case 422: if (z == 0) { break; } lines += "\n\nif (n == ((y % z) | x)) {"; goal = ((y % z) | x); goalFound = true; break;
                case 423: if (z == 0) { break; } lines += "\n\nif (n == ((y % z) ^ x)) {"; goal = ((y % z) ^ x); goalFound = true; break;
                case 424: lines += "\n\nif (n == ((y & z) + x)) {"; goal = ((y & z) + x); goalFound = true; break;
                case 425: lines += "\n\nif (n == ((y & z) - x)) {"; goal = ((y & z) - x); goalFound = true; break;
                case 426: lines += "\n\nif (n == ((y & z) * x)) {"; goal = ((y & z) * x); goalFound = true; break;
                case 427: if (x == 0) { break; } lines += "\n\nif (n == ((y & z) / x)) {"; goal = ((y & z) / x); goalFound = true; break;
                case 428: if (x == 0) { break; } lines += "\n\nif (n == ((y & z) % x)) {"; goal = ((y & z) % x); goalFound = true; break;
                case 429: lines += "\n\nif (n == ((y & z) & x)) {"; goal = ((y & z) & x); goalFound = true; break;
                case 430: lines += "\n\nif (n == ((y & z) | x)) {"; goal = ((y & z) | x); goalFound = true; break;
                case 431: lines += "\n\nif (n == ((y & z) ^ x)) {"; goal = ((y & z) ^ x); goalFound = true; break;
                case 432: lines += "\n\nif (n == ((y | z) + x)) {"; goal = ((y | z) + x); goalFound = true; break;
                case 433: lines += "\n\nif (n == ((y | z) - x)) {"; goal = ((y | z) - x); goalFound = true; break;
                case 434: lines += "\n\nif (n == ((y | z) * x)) {"; goal = ((y | z) * x); goalFound = true; break;
                case 435: if (x == 0) { break; } lines += "\n\nif (n == ((y | z) / x)) {"; goal = ((y | z) / x); goalFound = true; break;
                case 436: if (x == 0) { break; } lines += "\n\nif (n == ((y | z) % x)) {"; goal = ((y | z) % x); goalFound = true; break;
                case 437: lines += "\n\nif (n == ((y | z) & x)) {"; goal = ((y | z) & x); goalFound = true; break;
                case 438: lines += "\n\nif (n == ((y | z) | x)) {"; goal = ((y | z) | x); goalFound = true; break;
                case 439: lines += "\n\nif (n == ((y | z) ^ x)) {"; goal = ((y | z) ^ x); goalFound = true; break;
                case 440: lines += "\n\nif (n == ((y ^ z) + x)) {"; goal = ((y ^ z) + x); goalFound = true; break;
                case 441: lines += "\n\nif (n == ((y ^ z) - x)) {"; goal = ((y ^ z) - x); goalFound = true; break;
                case 442: lines += "\n\nif (n == ((y ^ z) * x)) {"; goal = ((y ^ z) * x); goalFound = true; break;
                case 443: if (x == 0) { break; } lines += "\n\nif (n == ((y ^ z) / x)) {"; goal = ((y ^ z) / x); goalFound = true; break;
                case 444: if (x == 0) { break; } lines += "\n\nif (n == ((y ^ z) % x)) {"; goal = ((y ^ z) % x); goalFound = true; break;
                case 445: lines += "\n\nif (n == ((y ^ z) & x)) {"; goal = ((y ^ z) & x); goalFound = true; break;
                case 446: lines += "\n\nif (n == ((y ^ z) | x)) {"; goal = ((y ^ z) | x); goalFound = true; break;
                case 447: lines += "\n\nif (n == ((y ^ z) ^ x)) {"; goal = ((y ^ z) ^ x); goalFound = true; break;
                case 448: lines += "\n\nif (n == (y + (z + x))) {"; goal = (y + (z + x)); goalFound = true; break;
                case 449: lines += "\n\nif (n == (y + (z - x))) {"; goal = (y + (z - x)); goalFound = true; break;
                case 450: lines += "\n\nif (n == (y + (z * x))) {"; goal = (y + (z * x)); goalFound = true; break;
                case 451: if (x == 0) { break; } lines += "\n\nif (n == (y + (z / x))) {"; goal = (y + (z / x)); goalFound = true; break;
                case 452: if (x == 0) { break; } lines += "\n\nif (n == (y + (z % x))) {"; goal = (y + (z % x)); goalFound = true; break;
                case 453: lines += "\n\nif (n == (y + (z & x))) {"; goal = (y + (z & x)); goalFound = true; break;
                case 454: lines += "\n\nif (n == (y + (z | x))) {"; goal = (y + (z | x)); goalFound = true; break;
                case 455: lines += "\n\nif (n == (y + (z ^ x))) {"; goal = (y + (z ^ x)); goalFound = true; break;
                case 456: lines += "\n\nif (n == (y - (z + x))) {"; goal = (y - (z + x)); goalFound = true; break;
                case 457: lines += "\n\nif (n == (y - (z - x))) {"; goal = (y - (z - x)); goalFound = true; break;
                case 458: lines += "\n\nif (n == (y - (z * x))) {"; goal = (y - (z * x)); goalFound = true; break;
                case 459: if (x == 0) { break; } lines += "\n\nif (n == (y - (z / x))) {"; goal = (y - (z / x)); goalFound = true; break;
                case 460: if (x == 0) { break; } lines += "\n\nif (n == (y - (z % x))) {"; goal = (y - (z % x)); goalFound = true; break;
                case 461: lines += "\n\nif (n == (y - (z & x))) {"; goal = (y - (z & x)); goalFound = true; break;
                case 462: lines += "\n\nif (n == (y - (z | x))) {"; goal = (y - (z | x)); goalFound = true; break;
                case 463: lines += "\n\nif (n == (y - (z ^ x))) {"; goal = (y - (z ^ x)); goalFound = true; break;
                case 464: lines += "\n\nif (n == (y * (z + x))) {"; goal = (y * (z + x)); goalFound = true; break;
                case 465: lines += "\n\nif (n == (y * (z - x))) {"; goal = (y * (z - x)); goalFound = true; break;
                case 466: lines += "\n\nif (n == (y * (z * x))) {"; goal = (y * (z * x)); goalFound = true; break;
                case 467: if (x == 0) { break; } lines += "\n\nif (n == (y * (z / x))) {"; goal = (y * (z / x)); goalFound = true; break;
                case 468: if (x == 0) { break; } lines += "\n\nif (n == (y * (z % x))) {"; goal = (y * (z % x)); goalFound = true; break;
                case 469: lines += "\n\nif (n == (y * (z & x))) {"; goal = (y * (z & x)); goalFound = true; break;
                case 470: lines += "\n\nif (n == (y * (z | x))) {"; goal = (y * (z | x)); goalFound = true; break;
                case 471: lines += "\n\nif (n == (y * (z ^ x))) {"; goal = (y * (z ^ x)); goalFound = true; break;
                case 472: if ((z + x) == 0) { break; } lines += "\n\nif (n == (y / (z + x))) {"; goal = (y / (z + x)); goalFound = true; break;
                case 473: if ((z - x) == 0) { break; } lines += "\n\nif (n == (y / (z - x))) {"; goal = (y / (z - x)); goalFound = true; break;
                case 474: if ((z * x) == 0) { break; } lines += "\n\nif (n == (y / (z * x))) {"; goal = (y / (z * x)); goalFound = true; break;
                case 475: if (z == 0 || x == 0) { break; } lines += "\n\nif (n == (y / (z / x))) {"; goal = (y / (z / x)); goalFound = true; break;
                case 476: if (z == 0 || x == 0 || (z%x) == 0) { break; } lines += "\n\nif (n == (y / (z % x))) {"; goal = (y / (z % x)); goalFound = true; break;
                case 477: if ((z & x) == 0) { break; } lines += "\n\nif (n == (y / (z & x))) {"; goal = (y / (z & x)); goalFound = true; break;
                case 478: if ((z | x) == 0) { break; } lines += "\n\nif (n == (y / (z | x))) {"; goal = (y / (z | x)); goalFound = true; break;
                case 479: if ((z ^ x) == 0) { break; } lines += "\n\nif (n == (y / (z ^ x))) {"; goal = (y / (z ^ x)); goalFound = true; break;
                case 480: if ((z + x) == 0) { break; } lines += "\n\nif (n == (y % (z + x))) {"; goal = (y % (z + x)); goalFound = true; break;
                case 481: if ((z - x) == 0) { break; } lines += "\n\nif (n == (y % (z - x))) {"; goal = (y % (z - x)); goalFound = true; break;
                case 482: if ((z * x) == 0) { break; } lines += "\n\nif (n == (y % (z * x))) {"; goal = (y % (z * x)); goalFound = true; break;
                case 483: if (z == 0 || x == 0) { break; } if ((z / x) == 0) { break; } lines += "\n\nif (n == (y % (z / x))) {"; goal = (y % (z / x)); goalFound = true; break;
                case 484: if (z == 0 || x == 0) { break; } if ((z % x) == 0) { break; } lines += "\n\nif (n == (y % (z % x))) {"; goal = (y % (z % x)); goalFound = true; break;
                case 485: if ((z & x) == 0) { break; } lines += "\n\nif (n == (y % (z & x))) {"; goal = (y % (z & x)); goalFound = true; break;
                case 486: if ((z | x) == 0) { break; } lines += "\n\nif (n == (y % (z | x))) {"; goal = (y % (z | x)); goalFound = true; break;
                case 487: if ((z ^ x) == 0) { break; } lines += "\n\nif (n == (y % (z ^ x))) {"; goal = (y % (z ^ x)); goalFound = true; break;
                case 488: lines += "\n\nif (n == (y & (z + x))) {"; goal = (y & (z + x)); goalFound = true; break;
                case 489: lines += "\n\nif (n == (y & (z - x))) {"; goal = (y & (z - x)); goalFound = true; break;
                case 490: lines += "\n\nif (n == (y & (z * x))) {"; goal = (y & (z * x)); goalFound = true; break;
                case 491: if (x == 0) { break; } lines += "\n\nif (n == (y & (z / x))) {"; goal = (y & (z / x)); goalFound = true; break;
                case 492: if (x == 0) { break; } lines += "\n\nif (n == (y & (z % x))) {"; goal = (y & (z % x)); goalFound = true; break;
                case 493: lines += "\n\nif (n == (y & (z & x))) {"; goal = (y & (z & x)); goalFound = true; break;
                case 494: lines += "\n\nif (n == (y & (z | x))) {"; goal = (y & (z | x)); goalFound = true; break;
                case 495: lines += "\n\nif (n == (y & (z ^ x))) {"; goal = (y & (z ^ x)); goalFound = true; break;
                case 496: lines += "\n\nif (n == (y | (z + x))) {"; goal = (y | (z + x)); goalFound = true; break;
                case 497: lines += "\n\nif (n == (y | (z - x))) {"; goal = (y | (z - x)); goalFound = true; break;
                case 498: lines += "\n\nif (n == (y | (z * x))) {"; goal = (y | (z * x)); goalFound = true; break;
                case 499: if (x == 0) { break; } lines += "\n\nif (n == (y | (z / x))) {"; goal = (y | (z / x)); goalFound = true; break;
                case 500: if (x == 0) { break; } lines += "\n\nif (n == (y | (z % x))) {"; goal = (y | (z % x)); goalFound = true; break;
                case 501: lines += "\n\nif (n == (y | (z & x))) {"; goal = (y | (z & x)); goalFound = true; break;
                case 502: lines += "\n\nif (n == (y | (z | x))) {"; goal = (y | (z | x)); goalFound = true; break;
                case 503: lines += "\n\nif (n == (y | (z ^ x))) {"; goal = (y | (z ^ x)); goalFound = true; break;
                case 504: lines += "\n\nif (n == (y ^ (z + x))) {"; goal = (y ^ (z + x)); goalFound = true; break;
                case 505: lines += "\n\nif (n == (y ^ (z - x))) {"; goal = (y ^ (z - x)); goalFound = true; break;
                case 506: lines += "\n\nif (n == (y ^ (z * x))) {"; goal = (y ^ (z * x)); goalFound = true; break;
                case 507: if (x == 0) { break; } lines += "\n\nif (n == (y ^ (z / x))) {"; goal = (y ^ (z / x)); goalFound = true; break;
                case 508: if (x == 0) { break; } lines += "\n\nif (n == (y ^ (z % x))) {"; goal = (y ^ (z % x)); goalFound = true; break;
                case 509: lines += "\n\nif (n == (y ^ (z & x))) {"; goal = (y ^ (z & x)); goalFound = true; break;
                case 510: lines += "\n\nif (n == (y ^ (z | x))) {"; goal = (y ^ (z | x)); goalFound = true; break;
                case 511: lines += "\n\nif (n == (y ^ (z ^ x))) {"; goal = (y ^ (z ^ x)); goalFound = true; break;
                case 512: lines += "\n\nif (n == ((z + x) + y)) {"; goal = ((z + x) + y); goalFound = true; break;
                case 513: lines += "\n\nif (n == ((z + x) - y)) {"; goal = ((z + x) - y); goalFound = true; break;
                case 514: lines += "\n\nif (n == ((z + x) * y)) {"; goal = ((z + x) * y); goalFound = true; break;
                case 515: if (y == 0) { break; } lines += "\n\nif (n == ((z + x) / y)) {"; goal = ((z + x) / y); goalFound = true; break;
                case 516: if (y == 0) { break; } lines += "\n\nif (n == ((z + x) % y)) {"; goal = ((z + x) % y); goalFound = true; break;
                case 517: lines += "\n\nif (n == ((z + x) & y)) {"; goal = ((z + x) & y); goalFound = true; break;
                case 518: lines += "\n\nif (n == ((z + x) | y)) {"; goal = ((z + x) | y); goalFound = true; break;
                case 519: lines += "\n\nif (n == ((z + x) ^ y)) {"; goal = ((z + x) ^ y); goalFound = true; break;
                case 520: lines += "\n\nif (n == ((z - x) + y)) {"; goal = ((z - x) + y); goalFound = true; break;
                case 521: lines += "\n\nif (n == ((z - x) - y)) {"; goal = ((z - x) - y); goalFound = true; break;
                case 522: lines += "\n\nif (n == ((z - x) * y)) {"; goal = ((z - x) * y); goalFound = true; break;
                case 523: if (y == 0) { break; } lines += "\n\nif (n == ((z - x) / y)) {"; goal = ((z - x) / y); goalFound = true; break;
                case 524: if (y == 0) { break; } lines += "\n\nif (n == ((z - x) % y)) {"; goal = ((z - x) % y); goalFound = true; break;
                case 525: lines += "\n\nif (n == ((z - x) & y)) {"; goal = ((z - x) & y); goalFound = true; break;
                case 526: lines += "\n\nif (n == ((z - x) | y)) {"; goal = ((z - x) | y); goalFound = true; break;
                case 527: lines += "\n\nif (n == ((z - x) ^ y)) {"; goal = ((z - x) ^ y); goalFound = true; break;
                case 528: lines += "\n\nif (n == ((z * x) + y)) {"; goal = ((z * x) + y); goalFound = true; break;
                case 529: lines += "\n\nif (n == ((z * x) - y)) {"; goal = ((z * x) - y); goalFound = true; break;
                case 530: lines += "\n\nif (n == ((z * x) * y)) {"; goal = ((z * x) * y); goalFound = true; break;
                case 531: if (y == 0) { break; } lines += "\n\nif (n == ((z * x) / y)) {"; goal = ((z * x) / y); goalFound = true; break;
                case 532: if (y == 0) { break; } lines += "\n\nif (n == ((z * x) % y)) {"; goal = ((z * x) % y); goalFound = true; break;
                case 533: lines += "\n\nif (n == ((z * x) & y)) {"; goal = ((z * x) & y); goalFound = true; break;
                case 534: lines += "\n\nif (n == ((z * x) | y)) {"; goal = ((z * x) | y); goalFound = true; break;
                case 535: lines += "\n\nif (n == ((z * x) ^ y)) {"; goal = ((z * x) ^ y); goalFound = true; break;
                case 536: if (x == 0) { break; } lines += "\n\nif (n == ((z / x) + y)) {"; goal = ((z / x) + y); goalFound = true; break;
                case 537: if (x == 0) { break; } lines += "\n\nif (n == ((z / x) - y)) {"; goal = ((z / x) - y); goalFound = true; break;
                case 538: if (x == 0) { break; } lines += "\n\nif (n == ((z / x) * y)) {"; goal = ((z / x) * y); goalFound = true; break;
                case 539: if (x == 0 || y == 0) { break; } lines += "\n\nif (n == ((z / x) / y)) {"; goal = ((z / x) / y); goalFound = true; break;
                case 540: if (x == 0 || y == 0) { break; } lines += "\n\nif (n == ((z / x) % y)) {"; goal = ((z / x) % y); goalFound = true; break;
                case 541: if (x == 0) { break; } lines += "\n\nif (n == ((z / x) & y)) {"; goal = ((z / x) & y); goalFound = true; break;
                case 542: if (x == 0) { break; } lines += "\n\nif (n == ((z / x) | y)) {"; goal = ((z / x) | y); goalFound = true; break;
                case 543: if (x == 0) { break; } lines += "\n\nif (n == ((z / x) ^ y)) {"; goal = ((z / x) ^ y); goalFound = true; break;
                case 544: if (x == 0) { break; } lines += "\n\nif (n == ((z % x) + y)) {"; goal = ((z % x) + y); goalFound = true; break;
                case 545: if (x == 0) { break; } lines += "\n\nif (n == ((z % x) - y)) {"; goal = ((z % x) - y); goalFound = true; break;
                case 546: if (x == 0) { break; } lines += "\n\nif (n == ((z % x) * y)) {"; goal = ((z % x) * y); goalFound = true; break;
                case 547: if (x == 0 || y == 0) { break; } lines += "\n\nif (n == ((z % x) / y)) {"; goal = ((z % x) / y); goalFound = true; break;
                case 548: if (x == 0 || y == 0) { break; } lines += "\n\nif (n == ((z % x) % y)) {"; goal = ((z % x) % y); goalFound = true; break;
                case 549: if (x == 0) { break; } lines += "\n\nif (n == ((z % x) & y)) {"; goal = ((z % x) & y); goalFound = true; break;
                case 550: if (x == 0) { break; } lines += "\n\nif (n == ((z % x) | y)) {"; goal = ((z % x) | y); goalFound = true; break;
                case 551: if (x == 0) { break; } lines += "\n\nif (n == ((z % x) ^ y)) {"; goal = ((z % x) ^ y); goalFound = true; break;
                case 552: lines += "\n\nif (n == ((z & x) + y)) {"; goal = ((z & x) + y); goalFound = true; break;
                case 553: lines += "\n\nif (n == ((z & x) - y)) {"; goal = ((z & x) - y); goalFound = true; break;
                case 554: lines += "\n\nif (n == ((z & x) * y)) {"; goal = ((z & x) * y); goalFound = true; break;
                case 555: if (y == 0) { break; } lines += "\n\nif (n == ((z & x) / y)) {"; goal = ((z & x) / y); goalFound = true; break;
                case 556: if (y == 0) { break; } lines += "\n\nif (n == ((z & x) % y)) {"; goal = ((z & x) % y); goalFound = true; break;
                case 557: lines += "\n\nif (n == ((z & x) & y)) {"; goal = ((z & x) & y); goalFound = true; break;
                case 558: lines += "\n\nif (n == ((z & x) | y)) {"; goal = ((z & x) | y); goalFound = true; break;
                case 559: lines += "\n\nif (n == ((z & x) ^ y)) {"; goal = ((z & x) ^ y); goalFound = true; break;
                case 560: lines += "\n\nif (n == ((z | x) + y)) {"; goal = ((z | x) + y); goalFound = true; break;
                case 561: lines += "\n\nif (n == ((z | x) - y)) {"; goal = ((z | x) - y); goalFound = true; break;
                case 562: lines += "\n\nif (n == ((z | x) * y)) {"; goal = ((z | x) * y); goalFound = true; break;
                case 563: if (y == 0) { break; } lines += "\n\nif (n == ((z | x) / y)) {"; goal = ((z | x) / y); goalFound = true; break;
                case 564: if (y == 0) { break; } lines += "\n\nif (n == ((z | x) % y)) {"; goal = ((z | x) % y); goalFound = true; break;
                case 565: lines += "\n\nif (n == ((z | x) & y)) {"; goal = ((z | x) & y); goalFound = true; break;
                case 566: lines += "\n\nif (n == ((z | x) | y)) {"; goal = ((z | x) | y); goalFound = true; break;
                case 567: lines += "\n\nif (n == ((z | x) ^ y)) {"; goal = ((z | x) ^ y); goalFound = true; break;
                case 568: lines += "\n\nif (n == ((z ^ x) + y)) {"; goal = ((z ^ x) + y); goalFound = true; break;
                case 569: lines += "\n\nif (n == ((z ^ x) - y)) {"; goal = ((z ^ x) - y); goalFound = true; break;
                case 570: lines += "\n\nif (n == ((z ^ x) * y)) {"; goal = ((z ^ x) * y); goalFound = true; break;
                case 571: if (y == 0) { break; } lines += "\n\nif (n == ((z ^ x) / y)) {"; goal = ((z ^ x) / y); goalFound = true; break;
                case 572: if (y == 0) { break; } lines += "\n\nif (n == ((z ^ x) % y)) {"; goal = ((z ^ x) % y); goalFound = true; break;
                case 573: lines += "\n\nif (n == ((z ^ x) & y)) {"; goal = ((z ^ x) & y); goalFound = true; break;
                case 574: lines += "\n\nif (n == ((z ^ x) | y)) {"; goal = ((z ^ x) | y); goalFound = true; break;
                case 575: lines += "\n\nif (n == ((z ^ x) ^ y)) {"; goal = ((z ^ x) ^ y); goalFound = true; break;
                case 576: lines += "\n\nif (n == (z + (x + y))) {"; goal = (z + (x + y)); goalFound = true; break;
                case 577: lines += "\n\nif (n == (z + (x - y))) {"; goal = (z + (x - y)); goalFound = true; break;
                case 578: lines += "\n\nif (n == (z + (x * y))) {"; goal = (z + (x * y)); goalFound = true; break;
                case 579: if (y == 0) { break; } lines += "\n\nif (n == (z + (x / y))) {"; goal = (z + (x / y)); goalFound = true; break;
                case 580: if (y == 0) { break; } lines += "\n\nif (n == (z + (x % y))) {"; goal = (z + (x % y)); goalFound = true; break;
                case 581: lines += "\n\nif (n == (z + (x & y))) {"; goal = (z + (x & y)); goalFound = true; break;
                case 582: lines += "\n\nif (n == (z + (x | y))) {"; goal = (z + (x | y)); goalFound = true; break;
                case 583: lines += "\n\nif (n == (z + (x ^ y))) {"; goal = (z + (x ^ y)); goalFound = true; break;
                case 584: lines += "\n\nif (n == (z - (x + y))) {"; goal = (z - (x + y)); goalFound = true; break;
                case 585: lines += "\n\nif (n == (z - (x - y))) {"; goal = (z - (x - y)); goalFound = true; break;
                case 586: lines += "\n\nif (n == (z - (x * y))) {"; goal = (z - (x * y)); goalFound = true; break;
                case 587: if (y == 0) { break; } lines += "\n\nif (n == (z - (x / y))) {"; goal = (z - (x / y)); goalFound = true; break;
                case 588: if (y == 0) { break; } lines += "\n\nif (n == (z - (x % y))) {"; goal = (z - (x % y)); goalFound = true; break;
                case 589: lines += "\n\nif (n == (z - (x & y))) {"; goal = (z - (x & y)); goalFound = true; break;
                case 590: lines += "\n\nif (n == (z - (x | y))) {"; goal = (z - (x | y)); goalFound = true; break;
                case 591: lines += "\n\nif (n == (z - (x ^ y))) {"; goal = (z - (x ^ y)); goalFound = true; break;
                case 592: lines += "\n\nif (n == (z * (x + y))) {"; goal = (z * (x + y)); goalFound = true; break;
                case 593: lines += "\n\nif (n == (z * (x - y))) {"; goal = (z * (x - y)); goalFound = true; break;
                case 594: lines += "\n\nif (n == (z * (x * y))) {"; goal = (z * (x * y)); goalFound = true; break;
                case 595: if (y == 0) { break; } lines += "\n\nif (n == (z * (x / y))) {"; goal = (z * (x / y)); goalFound = true; break;
                case 596: if (y == 0) { break; } lines += "\n\nif (n == (z * (x % y))) {"; goal = (z * (x % y)); goalFound = true; break;
                case 597: lines += "\n\nif (n == (z * (x & y))) {"; goal = (z * (x & y)); goalFound = true; break;
                case 598: lines += "\n\nif (n == (z * (x | y))) {"; goal = (z * (x | y)); goalFound = true; break;
                case 599: lines += "\n\nif (n == (z * (x ^ y))) {"; goal = (z * (x ^ y)); goalFound = true; break;
                case 600: if ((x + y) == 0) { break; } lines += "\n\nif (n == (z / (x + y))) {"; goal = (z / (x + y)); goalFound = true; break;
                case 601: if ((x - y) == 0) { break; } lines += "\n\nif (n == (z / (x - y))) {"; goal = (z / (x - y)); goalFound = true; break;
                case 602: if ((x * y) == 0) { break; } lines += "\n\nif (n == (z / (x * y))) {"; goal = (z / (x * y)); goalFound = true; break;
                case 603: if (x == 0 || y == 0) { break; } lines += "\n\nif (n == (z / (x / y))) {"; goal = (z / (x / y)); goalFound = true; break;
                case 604: if (x == 0 || y == 0 || (x%y) == 0) { break; } lines += "\n\nif (n == (z / (x % y))) {"; goal = (z / (x % y)); goalFound = true; break;
                case 605: if ((x & y) == 0) { break; } lines += "\n\nif (n == (z / (x & y))) {"; goal = (z / (x & y)); goalFound = true; break;
                case 606: if ((x | y) == 0) { break; } lines += "\n\nif (n == (z / (x | y))) {"; goal = (z / (x | y)); goalFound = true; break;
                case 607: if ((x ^ y) == 0) { break; } lines += "\n\nif (n == (z / (x ^ y))) {"; goal = (z / (x ^ y)); goalFound = true; break;
                case 608: if ((x + y) == 0) { break; } lines += "\n\nif (n == (z % (x + y))) {"; goal = (z % (x + y)); goalFound = true; break;
                case 609: if ((x - y) == 0) { break; } lines += "\n\nif (n == (z % (x - y))) {"; goal = (z % (x - y)); goalFound = true; break;
                case 610: if ((x * y) == 0) { break; } lines += "\n\nif (n == (z % (x * y))) {"; goal = (z % (x * y)); goalFound = true; break;
                case 611: if (x == 0 || y == 0) { break; } if ((x / y) == 0) { break; } lines += "\n\nif (n == (z % (x / y))) {"; goal = (z % (x / y)); goalFound = true; break;
                case 612: if (x == 0 || y == 0) { break; } if ((x % y) == 0) { break; } lines += "\n\nif (n == (z % (x % y))) {"; goal = (z % (x % y)); goalFound = true; break;
                case 613: if ((x & y) == 0) { break; } lines += "\n\nif (n == (z % (x & y))) {"; goal = (z % (x & y)); goalFound = true; break;
                case 614: if ((x | y) == 0) { break; } lines += "\n\nif (n == (z % (x | y))) {"; goal = (z % (x | y)); goalFound = true; break;
                case 615: if ((x ^ y) == 0) { break; } lines += "\n\nif (n == (z % (x ^ y))) {"; goal = (z % (x ^ y)); goalFound = true; break;
                case 616: lines += "\n\nif (n == (z & (x + y))) {"; goal = (z & (x + y)); goalFound = true; break;
                case 617: lines += "\n\nif (n == (z & (x - y))) {"; goal = (z & (x - y)); goalFound = true; break;
                case 618: lines += "\n\nif (n == (z & (x * y))) {"; goal = (z & (x * y)); goalFound = true; break;
                case 619: if (y == 0) { break; } lines += "\n\nif (n == (z & (x / y))) {"; goal = (z & (x / y)); goalFound = true; break;
                case 620: if (y == 0) { break; } lines += "\n\nif (n == (z & (x % y))) {"; goal = (z & (x % y)); goalFound = true; break;
                case 621: lines += "\n\nif (n == (z & (x & y))) {"; goal = (z & (x & y)); goalFound = true; break;
                case 622: lines += "\n\nif (n == (z & (x | y))) {"; goal = (z & (x | y)); goalFound = true; break;
                case 623: lines += "\n\nif (n == (z & (x ^ y))) {"; goal = (z & (x ^ y)); goalFound = true; break;
                case 624: lines += "\n\nif (n == (z | (x + y))) {"; goal = (z | (x + y)); goalFound = true; break;
                case 625: lines += "\n\nif (n == (z | (x - y))) {"; goal = (z | (x - y)); goalFound = true; break;
                case 626: lines += "\n\nif (n == (z | (x * y))) {"; goal = (z | (x * y)); goalFound = true; break;
                case 627: if (y == 0) { break; } lines += "\n\nif (n == (z | (x / y))) {"; goal = (z | (x / y)); goalFound = true; break;
                case 628: if (y == 0) { break; } lines += "\n\nif (n == (z | (x % y))) {"; goal = (z | (x % y)); goalFound = true; break;
                case 629: lines += "\n\nif (n == (z | (x & y))) {"; goal = (z | (x & y)); goalFound = true; break;
                case 630: lines += "\n\nif (n == (z | (x | y))) {"; goal = (z | (x | y)); goalFound = true; break;
                case 631: lines += "\n\nif (n == (z | (x ^ y))) {"; goal = (z | (x ^ y)); goalFound = true; break;
                case 632: lines += "\n\nif (n == (z ^ (x + y))) {"; goal = (z ^ (x + y)); goalFound = true; break;
                case 633: lines += "\n\nif (n == (z ^ (x - y))) {"; goal = (z ^ (x - y)); goalFound = true; break;
                case 634: lines += "\n\nif (n == (z ^ (x * y))) {"; goal = (z ^ (x * y)); goalFound = true; break;
                case 635: if (y == 0) { break; } lines += "\n\nif (n == (z ^ (x / y))) {"; goal = (z ^ (x / y)); goalFound = true; break;
                case 636: if (y == 0) { break; } lines += "\n\nif (n == (z ^ (x % y))) {"; goal = (z ^ (x % y)); goalFound = true; break;
                case 637: lines += "\n\nif (n == (z ^ (x & y))) {"; goal = (z ^ (x & y)); goalFound = true; break;
                case 638: lines += "\n\nif (n == (z ^ (x | y))) {"; goal = (z ^ (x | y)); goalFound = true; break;
                case 639: lines += "\n\nif (n == (z ^ (x ^ y))) {"; goal = (z ^ (x ^ y)); goalFound = true; break;
                case 640: lines += "\n\nif (n == ((z + y) + x)) {"; goal = ((z + y) + x); goalFound = true; break;
                case 641: lines += "\n\nif (n == ((z + y) - x)) {"; goal = ((z + y) - x); goalFound = true; break;
                case 642: lines += "\n\nif (n == ((z + y) * x)) {"; goal = ((z + y) * x); goalFound = true; break;
                case 643: if (x == 0) { break; } lines += "\n\nif (n == ((z + y) / x)) {"; goal = ((z + y) / x); goalFound = true; break;
                case 644: if (x == 0) { break; } lines += "\n\nif (n == ((z + y) % x)) {"; goal = ((z + y) % x); goalFound = true; break;
                case 645: lines += "\n\nif (n == ((z + y) & x)) {"; goal = ((z + y) & x); goalFound = true; break;
                case 646: lines += "\n\nif (n == ((z + y) | x)) {"; goal = ((z + y) | x); goalFound = true; break;
                case 647: lines += "\n\nif (n == ((z + y) ^ x)) {"; goal = ((z + y) ^ x); goalFound = true; break;
                case 648: lines += "\n\nif (n == ((z - y) + x)) {"; goal = ((z - y) + x); goalFound = true; break;
                case 649: lines += "\n\nif (n == ((z - y) - x)) {"; goal = ((z - y) - x); goalFound = true; break;
                case 650: lines += "\n\nif (n == ((z - y) * x)) {"; goal = ((z - y) * x); goalFound = true; break;
                case 651: if (x == 0) { break; } lines += "\n\nif (n == ((z - y) / x)) {"; goal = ((z - y) / x); goalFound = true; break;
                case 652: if (x == 0) { break; } lines += "\n\nif (n == ((z - y) % x)) {"; goal = ((z - y) % x); goalFound = true; break;
                case 653: lines += "\n\nif (n == ((z - y) & x)) {"; goal = ((z - y) & x); goalFound = true; break;
                case 654: lines += "\n\nif (n == ((z - y) | x)) {"; goal = ((z - y) | x); goalFound = true; break;
                case 655: lines += "\n\nif (n == ((z - y) ^ x)) {"; goal = ((z - y) ^ x); goalFound = true; break;
                case 656: lines += "\n\nif (n == ((z * y) + x)) {"; goal = ((z * y) + x); goalFound = true; break;
                case 657: lines += "\n\nif (n == ((z * y) - x)) {"; goal = ((z * y) - x); goalFound = true; break;
                case 658: lines += "\n\nif (n == ((z * y) * x)) {"; goal = ((z * y) * x); goalFound = true; break;
                case 659: if (x == 0) { break; } lines += "\n\nif (n == ((z * y) / x)) {"; goal = ((z * y) / x); goalFound = true; break;
                case 660: if (x == 0) { break; } lines += "\n\nif (n == ((z * y) % x)) {"; goal = ((z * y) % x); goalFound = true; break;
                case 661: lines += "\n\nif (n == ((z * y) & x)) {"; goal = ((z * y) & x); goalFound = true; break;
                case 662: lines += "\n\nif (n == ((z * y) | x)) {"; goal = ((z * y) | x); goalFound = true; break;
                case 663: lines += "\n\nif (n == ((z * y) ^ x)) {"; goal = ((z * y) ^ x); goalFound = true; break;
                case 664: if (y == 0) { break; } lines += "\n\nif (n == ((z / y) + x)) {"; goal = ((z / y) + x); goalFound = true; break;
                case 665: if (y == 0) { break; } lines += "\n\nif (n == ((z / y) - x)) {"; goal = ((z / y) - x); goalFound = true; break;
                case 666: if (y == 0) { break; } lines += "\n\nif (n == ((z / y) * x)) {"; goal = ((z / y) * x); goalFound = true; break;
                case 667: if (y == 0 || x == 0) { break; } lines += "\n\nif (n == ((z / y) / x)) {"; goal = ((z / y) / x); goalFound = true; break;
                case 668: if (y == 0 || x == 0) { break; } lines += "\n\nif (n == ((z / y) % x)) {"; goal = ((z / y) % x); goalFound = true; break;
                case 669: if (y == 0) { break; } lines += "\n\nif (n == ((z / y) & x)) {"; goal = ((z / y) & x); goalFound = true; break;
                case 670: if (y == 0) { break; } lines += "\n\nif (n == ((z / y) | x)) {"; goal = ((z / y) | x); goalFound = true; break;
                case 671: if (y == 0) { break; } lines += "\n\nif (n == ((z / y) ^ x)) {"; goal = ((z / y) ^ x); goalFound = true; break;
                case 672: if (y == 0) { break; } lines += "\n\nif (n == ((z % y) + x)) {"; goal = ((z % y) + x); goalFound = true; break;
                case 673: if (y == 0) { break; } lines += "\n\nif (n == ((z % y) - x)) {"; goal = ((z % y) - x); goalFound = true; break;
                case 674: if (y == 0) { break; } lines += "\n\nif (n == ((z % y) * x)) {"; goal = ((z % y) * x); goalFound = true; break;
                case 675: if (y == 0 || x == 0) { break; } lines += "\n\nif (n == ((z % y) / x)) {"; goal = ((z % y) / x); goalFound = true; break;
                case 676: if (y == 0 || x == 0) { break; } lines += "\n\nif (n == ((z % y) % x)) {"; goal = ((z % y) % x); goalFound = true; break;
                case 677: if (y == 0) { break; } lines += "\n\nif (n == ((z % y) & x)) {"; goal = ((z % y) & x); goalFound = true; break;
                case 678: if (y == 0) { break; } lines += "\n\nif (n == ((z % y) | x)) {"; goal = ((z % y) | x); goalFound = true; break;
                case 679: if (y == 0) { break; } lines += "\n\nif (n == ((z % y) ^ x)) {"; goal = ((z % y) ^ x); goalFound = true; break;
                case 680: lines += "\n\nif (n == ((z & y) + x)) {"; goal = ((z & y) + x); goalFound = true; break;
                case 681: lines += "\n\nif (n == ((z & y) - x)) {"; goal = ((z & y) - x); goalFound = true; break;
                case 682: lines += "\n\nif (n == ((z & y) * x)) {"; goal = ((z & y) * x); goalFound = true; break;
                case 683: if (x == 0) { break; } lines += "\n\nif (n == ((z & y) / x)) {"; goal = ((z & y) / x); goalFound = true; break;
                case 684: if (x == 0) { break; } lines += "\n\nif (n == ((z & y) % x)) {"; goal = ((z & y) % x); goalFound = true; break;
                case 685: lines += "\n\nif (n == ((z & y) & x)) {"; goal = ((z & y) & x); goalFound = true; break;
                case 686: lines += "\n\nif (n == ((z & y) | x)) {"; goal = ((z & y) | x); goalFound = true; break;
                case 687: lines += "\n\nif (n == ((z & y) ^ x)) {"; goal = ((z & y) ^ x); goalFound = true; break;
                case 688: lines += "\n\nif (n == ((z | y) + x)) {"; goal = ((z | y) + x); goalFound = true; break;
                case 689: lines += "\n\nif (n == ((z | y) - x)) {"; goal = ((z | y) - x); goalFound = true; break;
                case 690: lines += "\n\nif (n == ((z | y) * x)) {"; goal = ((z | y) * x); goalFound = true; break;
                case 691: if (x == 0) { break; } lines += "\n\nif (n == ((z | y) / x)) {"; goal = ((z | y) / x); goalFound = true; break;
                case 692: if (x == 0) { break; } lines += "\n\nif (n == ((z | y) % x)) {"; goal = ((z | y) % x); goalFound = true; break;
                case 693: lines += "\n\nif (n == ((z | y) & x)) {"; goal = ((z | y) & x); goalFound = true; break;
                case 694: lines += "\n\nif (n == ((z | y) | x)) {"; goal = ((z | y) | x); goalFound = true; break;
                case 695: lines += "\n\nif (n == ((z | y) ^ x)) {"; goal = ((z | y) ^ x); goalFound = true; break;
                case 696: lines += "\n\nif (n == ((z ^ y) + x)) {"; goal = ((z ^ y) + x); goalFound = true; break;
                case 697: lines += "\n\nif (n == ((z ^ y) - x)) {"; goal = ((z ^ y) - x); goalFound = true; break;
                case 698: lines += "\n\nif (n == ((z ^ y) * x)) {"; goal = ((z ^ y) * x); goalFound = true; break;
                case 699: if (x == 0) { break; } lines += "\n\nif (n == ((z ^ y) / x)) {"; goal = ((z ^ y) / x); goalFound = true; break;
                case 700: if (x == 0) { break; } lines += "\n\nif (n == ((z ^ y) % x)) {"; goal = ((z ^ y) % x); goalFound = true; break;
                case 701: lines += "\n\nif (n == ((z ^ y) & x)) {"; goal = ((z ^ y) & x); goalFound = true; break;
                case 702: lines += "\n\nif (n == ((z ^ y) | x)) {"; goal = ((z ^ y) | x); goalFound = true; break;
                case 703: lines += "\n\nif (n == ((z ^ y) ^ x)) {"; goal = ((z ^ y) ^ x); goalFound = true; break;
                case 704: lines += "\n\nif (n == (z + (y + x))) {"; goal = (z + (y + x)); goalFound = true; break;
                case 705: lines += "\n\nif (n == (z + (y - x))) {"; goal = (z + (y - x)); goalFound = true; break;
                case 706: lines += "\n\nif (n == (z + (y * x))) {"; goal = (z + (y * x)); goalFound = true; break;
                case 707: if (x == 0) { break; } lines += "\n\nif (n == (z + (y / x))) {"; goal = (z + (y / x)); goalFound = true; break;
                case 708: if (x == 0) { break; } lines += "\n\nif (n == (z + (y % x))) {"; goal = (z + (y % x)); goalFound = true; break;
                case 709: lines += "\n\nif (n == (z + (y & x))) {"; goal = (z + (y & x)); goalFound = true; break;
                case 710: lines += "\n\nif (n == (z + (y | x))) {"; goal = (z + (y | x)); goalFound = true; break;
                case 711: lines += "\n\nif (n == (z + (y ^ x))) {"; goal = (z + (y ^ x)); goalFound = true; break;
                case 712: lines += "\n\nif (n == (z - (y + x))) {"; goal = (z - (y + x)); goalFound = true; break;
                case 713: lines += "\n\nif (n == (z - (y - x))) {"; goal = (z - (y - x)); goalFound = true; break;
                case 714: lines += "\n\nif (n == (z - (y * x))) {"; goal = (z - (y * x)); goalFound = true; break;
                case 715: if (x == 0) { break; } lines += "\n\nif (n == (z - (y / x))) {"; goal = (z - (y / x)); goalFound = true; break;
                case 716: if (x == 0) { break; } lines += "\n\nif (n == (z - (y % x))) {"; goal = (z - (y % x)); goalFound = true; break;
                case 717: lines += "\n\nif (n == (z - (y & x))) {"; goal = (z - (y & x)); goalFound = true; break;
                case 718: lines += "\n\nif (n == (z - (y | x))) {"; goal = (z - (y | x)); goalFound = true; break;
                case 719: lines += "\n\nif (n == (z - (y ^ x))) {"; goal = (z - (y ^ x)); goalFound = true; break;
                case 720: lines += "\n\nif (n == (z * (y + x))) {"; goal = (z * (y + x)); goalFound = true; break;
                case 721: lines += "\n\nif (n == (z * (y - x))) {"; goal = (z * (y - x)); goalFound = true; break;
                case 722: lines += "\n\nif (n == (z * (y * x))) {"; goal = (z * (y * x)); goalFound = true; break;
                case 723: if (x == 0) { break; } lines += "\n\nif (n == (z * (y / x))) {"; goal = (z * (y / x)); goalFound = true; break;
                case 724: if (x == 0) { break; } lines += "\n\nif (n == (z * (y % x))) {"; goal = (z * (y % x)); goalFound = true; break;
                case 725: lines += "\n\nif (n == (z * (y & x))) {"; goal = (z * (y & x)); goalFound = true; break;
                case 726: lines += "\n\nif (n == (z * (y | x))) {"; goal = (z * (y | x)); goalFound = true; break;
                case 727: lines += "\n\nif (n == (z * (y ^ x))) {"; goal = (z * (y ^ x)); goalFound = true; break;
                case 728: if ((y + x) == 0) { break; } lines += "\n\nif (n == (z / (y + x))) {"; goal = (z / (y + x)); goalFound = true; break;
                case 729: if ((y - x) == 0) { break; } lines += "\n\nif (n == (z / (y - x))) {"; goal = (z / (y - x)); goalFound = true; break;
                case 730: if ((y * x) == 0) { break; } lines += "\n\nif (n == (z / (y * x))) {"; goal = (z / (y * x)); goalFound = true; break;
                case 731: if (y == 0 || x == 0) { break; } lines += "\n\nif (n == (z / (y / x))) {"; goal = (z / (y / x)); goalFound = true; break;
                case 732: if (y == 0 || x == 0 || (y%x) == 0) { break; } lines += "\n\nif (n == (z / (y % x))) {"; goal = (z / (y % x)); goalFound = true; break;
                case 733: if ((y & x) == 0) { break; } lines += "\n\nif (n == (z / (y & x))) {"; goal = (z / (y & x)); goalFound = true; break;
                case 734: if ((y | x) == 0) { break; } lines += "\n\nif (n == (z / (y | x))) {"; goal = (z / (y | x)); goalFound = true; break;
                case 735: if ((y ^ x) == 0) { break; } lines += "\n\nif (n == (z / (y ^ x))) {"; goal = (z / (y ^ x)); goalFound = true; break;
                case 736: if ((y + x) == 0) { break; } lines += "\n\nif (n == (z % (y + x))) {"; goal = (z % (y + x)); goalFound = true; break;
                case 737: if ((y - x) == 0) { break; } lines += "\n\nif (n == (z % (y - x))) {"; goal = (z % (y - x)); goalFound = true; break;
                case 738: if ((y * x) == 0) { break; } lines += "\n\nif (n == (z % (y * x))) {"; goal = (z % (y * x)); goalFound = true; break;
                case 739: if (y == 0 || x == 0) { break; } if ((y / x) == 0) { break; } lines += "\n\nif (n == (z % (y / x))) {"; goal = (z % (y / x)); goalFound = true; break;
                case 740: if (y == 0 || x == 0) { break; } if ((y % x) == 0) { break; } lines += "\n\nif (n == (z % (y % x))) {"; goal = (z % (y % x)); goalFound = true; break;
                case 741: if ((y & x) == 0) { break; } lines += "\n\nif (n == (z % (y & x))) {"; goal = (z % (y & x)); goalFound = true; break;
                case 742: if ((y | x) == 0) { break; } lines += "\n\nif (n == (z % (y | x))) {"; goal = (z % (y | x)); goalFound = true; break;
                case 743: if ((y ^ x) == 0) { break; } lines += "\n\nif (n == (z % (y ^ x))) {"; goal = (z % (y ^ x)); goalFound = true; break;
                case 744: lines += "\n\nif (n == (z & (y + x))) {"; goal = (z & (y + x)); goalFound = true; break;
                case 745: lines += "\n\nif (n == (z & (y - x))) {"; goal = (z & (y - x)); goalFound = true; break;
                case 746: lines += "\n\nif (n == (z & (y * x))) {"; goal = (z & (y * x)); goalFound = true; break;
                case 747: if (x == 0) { break; } lines += "\n\nif (n == (z & (y / x))) {"; goal = (z & (y / x)); goalFound = true; break;
                case 748: if (x == 0) { break; } lines += "\n\nif (n == (z & (y % x))) {"; goal = (z & (y % x)); goalFound = true; break;
                case 749: lines += "\n\nif (n == (z & (y & x))) {"; goal = (z & (y & x)); goalFound = true; break;
                case 750: lines += "\n\nif (n == (z & (y | x))) {"; goal = (z & (y | x)); goalFound = true; break;
                case 751: lines += "\n\nif (n == (z & (y ^ x))) {"; goal = (z & (y ^ x)); goalFound = true; break;
                case 752: lines += "\n\nif (n == (z | (y + x))) {"; goal = (z | (y + x)); goalFound = true; break;
                case 753: lines += "\n\nif (n == (z | (y - x))) {"; goal = (z | (y - x)); goalFound = true; break;
                case 754: lines += "\n\nif (n == (z | (y * x))) {"; goal = (z | (y * x)); goalFound = true; break;
                case 755: if (x == 0) { break; } lines += "\n\nif (n == (z | (y / x))) {"; goal = (z | (y / x)); goalFound = true; break;
                case 756: if (x == 0) { break; } lines += "\n\nif (n == (z | (y % x))) {"; goal = (z | (y % x)); goalFound = true; break;
                case 757: lines += "\n\nif (n == (z | (y & x))) {"; goal = (z | (y & x)); goalFound = true; break;
                case 758: lines += "\n\nif (n == (z | (y | x))) {"; goal = (z | (y | x)); goalFound = true; break;
                case 759: lines += "\n\nif (n == (z | (y ^ x))) {"; goal = (z | (y ^ x)); goalFound = true; break;
                case 760: lines += "\n\nif (n == (z ^ (y + x))) {"; goal = (z ^ (y + x)); goalFound = true; break;
                case 761: lines += "\n\nif (n == (z ^ (y - x))) {"; goal = (z ^ (y - x)); goalFound = true; break;
                case 762: lines += "\n\nif (n == (z ^ (y * x))) {"; goal = (z ^ (y * x)); goalFound = true; break;
                case 763: if (x == 0) { break; } lines += "\n\nif (n == (z ^ (y / x))) {"; goal = (z ^ (y / x)); goalFound = true; break;
                case 764: if (x == 0) { break; } lines += "\n\nif (n == (z ^ (y % x))) {"; goal = (z ^ (y % x)); goalFound = true; break;
                case 765: lines += "\n\nif (n == (z ^ (y & x))) {"; goal = (z ^ (y & x)); goalFound = true; break;
                case 766: lines += "\n\nif (n == (z ^ (y | x))) {"; goal = (z ^ (y | x)); goalFound = true; break;
                case 767: lines += "\n\nif (n == (z ^ (y ^ x))) {"; goal = (z ^ (y ^ x)); goalFound = true; break;
                default: lines += "//send log!"; goalFound = true; break;
            }
        }
    }
	
    void UpdateStrings ()
    {
        strX += " -> " + x;
        strY += " -> " + y;
        strZ += " -> " + z;
    }

    void numPress (KMSelectable num)
    {
        if(strN.Length != 10)
        {
            num.AddInteractionPunch();
            GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
            for (int i = 0; i <= 9; i++)
            {
                if (num == numButtons[i])
                {
                    if (i != 0 || strN != "0")
                    {
                        if (strN == "0")
                        {
                            strN = "";
                        }
                        strN += digits[i];
                        code.text = lines.Replace("#", strN);
                    }
                }
            }
        }
    }

    void otherPress(KMSelectable other)
    {
        other.AddInteractionPunch();
        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        if (other == otherButtons[0]) //. I REALIZE NOW THAT SINCE INTERGERS ARE USED THIS WILL ALSO NEVER BE USED,
        {                             //HOWEVER IF YOU WANT TO IMPLEMENT SUPPORT FOR FLOATS/DOUBLES/WHATEVER, 
            if (strN == "0")          //FEEL FREE TO DO SO
            {
                strN = "";
            }
            strN += ".";
        } else if (other == otherButtons[1]) //-
        {
            if(strN.Contains("-"))
                strN = strN.Replace("-","");
            else if(!strN.Equals("0"))
                strN = strN.Insert(0, "-");
        } else if (other == otherButtons[2]) //<
        {
            if (strN.Length == 2 && strN.Contains("-"))
            {
                strN = "0";
            }
            else if (strN.Length != 1)
            {
                strN = strN.Substring(0, (strN.Length - 1));
            } else 
            {
                strN = "0";
            }
        } else if (other == otherButtons[3]) //E
        {
            final = int.Parse(strN);
            if (final == goal)
            {
                GetComponent<KMBombModule>().HandlePass();
                Debug.LogFormat("[Lines of Code #{0}] {1} entered, which is correct. Module solved.", moduleId, final);
            } else
            {
                GetComponent<KMBombModule>().HandleStrike();
                Debug.LogFormat("[Lines of Code #{0}] {1} entered, which is incorrect, it should be {2}. Module striked.", moduleId, final, goal);
            }
        }
        code.text = lines.Replace("#", strN);
    }

    //twitch plays
    private bool isValid(string s)
    {
        bool check = false;
        int temp = 0;
        check = int.TryParse(s, out temp);
        if (check)
        {
            //if(temp >= -999999999 && temp <= 999999999)
            //{
                return true;
            //}
        }
        return false;
    }

    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} enter/submit <num> [Enters the specified number]";
    #pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        string[] parameters = command.Split(' ');
        if(Regex.IsMatch(parameters[0], @"^\s*submit\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) || Regex.IsMatch(parameters[0], @"^\s*enter\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            if (parameters.Length == 2)
            {
                if (isValid(parameters[1]))
                {
                    //clear previous input
                    int length = strN.Length;
                    for(int i = 0; i < length; i++)
                    {
                        otherButtons[2].OnInteract();
                        yield return new WaitForSeconds(0.1f);
                    }
                    //enter input
                    for (int i = 0; i < parameters[1].Length; i++)
                    {
                        int temp = 0;
                        int.TryParse(parameters[1].ElementAt(i) + "", out temp);
                        numButtons[temp].OnInteract();
                        yield return new WaitForSeconds(0.1f);
                    }
                    if (parameters[1].ElementAt(0).Equals('-'))
                    {
                        otherButtons[1].OnInteract();
                        yield return new WaitForSeconds(0.1f);
                    }
                    otherButtons[3].OnInteract();
                }
                else
                {
                    yield return "sendtochaterror The specified number '" + parameters[1] + "' is invalid!";
                }
            }
            else
            {
                yield return "sendtochaterror Please specify the number that needs to be entered!";
            }
            yield break;
        }
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        yield return ProcessTwitchCommand("submit "+goal);
    }
}
