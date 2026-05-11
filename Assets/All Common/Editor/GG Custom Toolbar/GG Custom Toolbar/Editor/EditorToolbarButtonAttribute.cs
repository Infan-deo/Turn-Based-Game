using System;

namespace GGCustomToolbar
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EditorToolbarButtonAttribute : Attribute
    {
        public string IconName { get; }
        public string Tooltip { get; }
        public int Priority { get; }
        public EditorToolbarPosition Position { get; }
        public bool DisableOnPlayMode { get; }

        public EditorToolbarButtonAttribute(string iconName, string tooltip = "", int priority = 0, EditorToolbarPosition position = EditorToolbarPosition.LeftLeft, bool disableOnPlayMode = false)
        {
            IconName = iconName;
            Tooltip = tooltip;
            Priority = priority;
            Position = position;
            DisableOnPlayMode = disableOnPlayMode;
        }
    }
}
