using System.Windows.Forms;

namespace System
{
    internal class MouseEventArgs
    {
        private Action<object, Windows.Forms.MouseEventArgs> panel1_Click;

        public MouseEventArgs(Action<object, Windows.Forms.MouseEventArgs> panel1_Click)
        {
            this.panel1_Click = panel1_Click;
        }
    }
}