using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices; // for Win32 API fucntions

namespace BoxerTasticMate
{
    class KeyboardHandling
    {
        [DllImport("user32.dll")]
        public static extern uint SendInput(int numberOfInputs, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] INPUT[] input, int structSize);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr GetMessageExtraInfo();


        //API Constants
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        public const int VK_CONTROL = 0x11;
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_CHAR = 0x102;
        public const int MK_LBUTTON = 0x01;
        public const int VK_RETURN = 0x0d;
        public const int VK_LEFT = 0x25;
        public const int VK_UP = 0x26;
        public const int VK_RIGHT = 0x27;
        public const int VK_DOWN = 0x28;
        public const int VK_F5 = 0x74;
        public const int VK_F6 = 0x75;
        public const int VK_F7 = 0x76;

        public const int KEYEVENTF_EXTENDEDKEY = 0x0001; //Key down flag
        public const int KEYEVENTF_KEYUP = 0x0002; //Key up flag
        public const int VK_LCONTROL = 0xA2; //Left Control key code
        public const int VK_TAB = 0x09; //TAB key
        public const int VK_ESCAPE = 0x1B; //ESC key
        public const int VK_SHIFT = 0x10; //Shift key
        public const int VK_SPACE = 0x20; //Spacebar

        // Keys 0 to 9
        public const int VK_0 = 0x30;
        public const int VK_1 = 0x31;
        public const int VK_2 = 0x32;
        public const int VK_3 = 0x33;
        public const int VK_4 = 0x34;
        public const int VK_5 = 0x35;
        public const int VK_6 = 0x36;
        public const int VK_7 = 0x37;
        public const int VK_8 = 0x38;
        public const int VK_9 = 0x39;


        // Keys A to Z
        public const int VK_A = 0x41; //A Control key code
        public const int VK_B = 0x42;
        public const int VK_C = 0x43;
        public const int VK_D = 0x44;
        public const int VK_E = 0x45;
        public const int VK_F = 0x46;
        public const int VK_G = 0x47;
        public const int VK_H = 0x48;
        public const int VK_I = 0x49;
        public const int VK_J = 0x4A;
        public const int VK_K = 0x4B;
        public const int VK_L = 0x4C;
        public const int VK_M = 0x4D;
        public const int VK_N = 0x4E;
        public const int VK_O = 0x4F;
        public const int VK_P = 0x50;
        public const int VK_Q = 0x51;
        public const int VK_R = 0x52;
        public const int VK_S = 0x53;
        public const int VK_T = 0x54;
        public const int VK_U = 0x55;
        public const int VK_V = 0x56;
        public const int VK_W = 0x57;
        public const int VK_X = 0x58;
        public const int VK_Y = 0x59;
        public const int VK_Z = 0x5A;



        public static void keyPress(ushort virtualKeyCode)
        {
            INPUT Input1 = new INPUT();

            Input1.type = 1; // input is via the keyboard
            Input1.ki.wVk = virtualKeyCode;
            Input1.ki.dwFlags = (int)KEYEVENTF_EXTENDEDKEY; // key down
            Input1.ki.dwExtraInfo = GetMessageExtraInfo();

            INPUT Input2 = new INPUT();

            Input2.type = 1; // input is via the keyboard
            Input2.ki.wVk = virtualKeyCode;
            Input2.ki.dwFlags = (int)KEYEVENTF_KEYUP; // key up
            Input2.ki.dwExtraInfo = GetMessageExtraInfo();

            INPUT[] pInputs = new INPUT[] { Input1, Input2 };

            SendInput(2, pInputs, Marshal.SizeOf(Input1));

        }

        public static void keyDown(ushort virtualKeyCode)
        {
            INPUT Input1 = new INPUT();

            Input1.type = 1; // input is via the keyboard
            Input1.ki.wVk = virtualKeyCode;
            Input1.ki.dwFlags = (int)KEYEVENTF_EXTENDEDKEY; // key down
            Input1.ki.dwExtraInfo = GetMessageExtraInfo();

            INPUT[] pInputs = new INPUT[] { Input1 };

            SendInput(1, pInputs, Marshal.SizeOf(Input1));
        }

        public static void keyUp(ushort virtualKeyCode)
        {
            INPUT Input1 = new INPUT();

            Input1.type = 1; // input is via the keyboard
            Input1.ki.wVk = virtualKeyCode;
            Input1.ki.dwFlags = (int)KEYEVENTF_KEYUP; // key up
            Input1.ki.dwExtraInfo = GetMessageExtraInfo();

            INPUT[] pInputs = new INPUT[] { Input1 };

            SendInput(1, pInputs, Marshal.SizeOf(Input1));
        }


    }
}



[StructLayout(LayoutKind.Sequential)]
public struct MOUSEINPUT
{
    int dx;
    int dy;
    uint mouseData;
    uint dwFlags;
    uint time;
    IntPtr dwExtraInfo;
}

[StructLayout(LayoutKind.Sequential)]
public struct KEYBDINPUT
{
    public ushort wVk;
    public ushort wScan;
    public uint dwFlags;
    public uint time;
    public IntPtr dwExtraInfo;
}

[StructLayout(LayoutKind.Sequential)]
public struct HARDWAREINPUT
{
    uint uMsg;
    ushort wParamL;
    ushort wParamH;
}

[StructLayout(LayoutKind.Explicit)]
public struct INPUT
{
    [FieldOffset(0)]
    public int type;
    [FieldOffset(4)] //*
    public MOUSEINPUT mi;
    [FieldOffset(4)] //*
    public KEYBDINPUT ki;
    [FieldOffset(4)] //*
    public HARDWAREINPUT hi;
}
