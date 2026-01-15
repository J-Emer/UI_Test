namespace UI.Util
{
    public enum MouseCursorState{Hover, Enter, Exit, None};
    public enum MouseButtonState{Down, Up, None};
    public enum DockStyle{Left, Right, Top, Bottom, Fill, None};
    internal enum WindowDragMode{None, Move, Resize}
    public enum ResizeDirection
    {
        None   = 0,
        Left   = 1 << 0,
        Right  = 1 << 1,
        Top    = 1 << 2,
        Bottom = 1 << 3
    }

}