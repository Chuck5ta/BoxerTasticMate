using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Runtime.InteropServices;
using System.Threading; // required in order to implement a pause (Sleep)
using System.Windows.Forms;

namespace BoxerTasticMate
{
    class MouseHandling
    {
        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        private const uint LEFTDOWN = 0x00000002;
        private const uint LEFTUP = 0x00000004;
        private const uint MIDDLEDOWN = 0x00000020;
        private const uint MIDDLEUP = 0x00000040;
        private const uint MOVE = 0x00000001;
        private const uint ABSOLUTE = 0x00008000;
        private const uint RIGHTDOWN = 0x00000008;
        private const uint RIGHTUP = 0x00000010;



        public static void mouseMove(int x, int y)
        {
            Cursor.Position = new System.Drawing.Point(x, y);
            mouse_event((int)(MOVE), 0, 0, 0, 0);
        }

        public static void leftClick(int x, int y)
        {
            Cursor.Position = new System.Drawing.Point(x, y);
            Thread.Sleep(50);
            mouse_event((int)(LEFTDOWN), 0, 0, 0, 0);
            Thread.Sleep(50);
            mouse_event((int)(LEFTUP), 0, 0, 0, 0);
            Thread.Sleep(50);
        }

        public static void leftDoubleClick(int x, int y)
        {
            Cursor.Position = new System.Drawing.Point(x, y);
            Thread.Sleep(50);
            mouse_event((int)(LEFTDOWN), 0, 0, 0, 0);
            Thread.Sleep(50);
            mouse_event((int)(LEFTUP), 0, 0, 0, 0);
            Thread.Sleep(50);
            mouse_event((int)(LEFTDOWN), 0, 0, 0, 0);
            Thread.Sleep(50);
            mouse_event((int)(LEFTUP), 0, 0, 0, 0);
        }

        public static void rightClick(int x, int y)
        {
            Cursor.Position = new System.Drawing.Point(x, y);
            Thread.Sleep(50);
            mouse_event((int)(RIGHTDOWN), 0, 0, 0, 0);
            Thread.Sleep(50);
            mouse_event((int)(RIGHTUP), 0, 0, 0, 0);
            Thread.Sleep(50);
        }

        public static void leftMouseDown(int x, int y)
        {
            Cursor.Position = new System.Drawing.Point(x, y);
            Thread.Sleep(50);
            mouse_event((int)(LEFTDOWN), 0, 0, 0, 0);
        }

        public static void leftMouseUp(int x, int y)
        {
            Cursor.Position = new System.Drawing.Point(x, y);
            Thread.Sleep(50);
            mouse_event((int)(LEFTUP), 0, 0, 0, 0);
            Thread.Sleep(50);
        }

        public static void rightMouseDown(int x, int y)
        {
            Cursor.Position = new System.Drawing.Point(x, y);
            Thread.Sleep(50);
            mouse_event((int)(RIGHTDOWN), 0, 0, 0, 0);
        }

        public static void rightMouseUp(int x, int y)
        {
            Cursor.Position = new System.Drawing.Point(x, y);
            Thread.Sleep(50);
            mouse_event((int)(RIGHTUP), 0, 0, 0, 0);
            Thread.Sleep(150);
        }

        public static void middleMouseDown(int x, int y)
        {
            Cursor.Position = new System.Drawing.Point(x, y);
            Thread.Sleep(50);
            mouse_event((int)(MIDDLEDOWN), 0, 0, 0, 0);
        }

        public static void middleMouseUp(int x, int y)
        {
            Cursor.Position = new System.Drawing.Point(x, y);
            Thread.Sleep(50);
            mouse_event((int)(MIDDLEUP), 0, 0, 0, 0);
            Thread.Sleep(50);
        }


    }
}
