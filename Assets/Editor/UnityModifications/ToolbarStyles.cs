using DG.DemiEditor;
using UnityEngine;

namespace Sandlib.Editor
{
    public static class ToolbarStyles
    {
        public static readonly GUIStyle NormalButtonStyle;
        public static readonly GUIStyle ActivatedButtonStyle;

        static ToolbarStyles()
        {
            NormalButtonStyle = GUI.skin.button;
            ActivatedButtonStyle = GUI.skin.button.Clone();
            ActivatedButtonStyle.onNormal = ActivatedButtonStyle.active;
        }
    }
}