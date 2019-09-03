using System.Diagnostics;
using System.Runtime.InteropServices;

namespace my_class
{

    public class MousePosition
    {
        [DllImport("User32")]
        public static extern bool GetCursorPos(out POINT pt);

        public struct POINT
        {
            public int X;
            public int Y;
            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }
        //鼠标移动并计算坐标
        public POINT _GetCursorPos()
        {
            POINT tmp = new POINT();
            GetCursorPos(out tmp);
            return tmp;
        }
    }
}
