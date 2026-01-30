using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ABCsystem.Window
{
    public class WindowConstraintBehavior : NativeWindow
    {
        private readonly Form _form;
        private readonly Func<Rectangle> _getBoundsScreen;

        private const int WM_MOVING = 0x0216;
        private const int WM_SIZING = 0x0214;

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left, Top, Right, Bottom;

            public Rectangle ToRectangle() => Rectangle.FromLTRB(Left, Top, Right, Bottom);
            public static RECT FromRectangle(Rectangle r) => new RECT { Left = r.Left, Top = r.Top, Right = r.Right, Bottom = r.Bottom };
        }

        public WindowConstraintBehavior(Form form, Func<Rectangle> getBoundsScreen)
        {
            _form = form;
            _getBoundsScreen = getBoundsScreen;

            // 핸들이 생성되면 붙이고, 파괴되면 떼기
            _form.HandleCreated += (s, e) => AssignHandle(_form.Handle);
            _form.HandleDestroyed += (s, e) => ReleaseHandle();

            if (_form.IsHandleCreated)
                AssignHandle(_form.Handle);
        }

        protected override void WndProc(ref Message m)
        {
            if ((m.Msg == WM_MOVING || m.Msg == WM_SIZING) && _getBoundsScreen != null)
            {
                RECT rr = (RECT)Marshal.PtrToStructure(m.LParam, typeof(RECT));
                Rectangle clamped = WindowClampHelper.Clamp(rr.ToRectangle(), _getBoundsScreen());
                RECT outRect = RECT.FromRectangle(clamped);
                Marshal.StructureToPtr(outRect, m.LParam, true);
            }

            base.WndProc(ref m);
        }
    }
}