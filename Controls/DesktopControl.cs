using System;
using UI.Util;

namespace UI.Controls
{
    public class DesktopControl : Control
    {
        public int ZOrder{get;set;} = 10;
        public Desktop ParentDesktop{get;set;}
        public DockStyle Dock{get;set;} = DockStyle.None;

        //todo: other dock related stuff here
    }
}