using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GGCustomToolbar
{
    [InitializeOnLoad]
    public static class EditorToolbar
    {
        private static readonly Type ToolbarType = typeof(Editor).Assembly.GetType("UnityEditor.Toolbar");
        private static ScriptableObject _currentToolbar;
        private static bool _isInitialized;

        private static VisualElement LeftParent { get; }
        private static VisualElement LeftLeftParent { get; }
        private static VisualElement LeftCenterParent { get; }
        private static VisualElement LeftRightParent { get; }
        private static VisualElement RightParent { get; }
        private static VisualElement RightLeftParent { get; }
        private static VisualElement RightCenterParent { get; }
        private static VisualElement RightRightParent { get; }

        static EditorToolbar()
        {
            EditorApplication.update -= OnUpdate;
            EditorApplication.update += OnUpdate;

            _currentToolbar = ScriptableObject.CreateInstance<ScriptableObject>();

            LeftParent = CreateParentElement();
            RightParent = CreateParentElement();

            LeftParent.Add(LeftLeftParent = CreateSectionElement());
            LeftParent.Add(LeftCenterParent = CreateSectionElement());
            LeftParent.Add(LeftRightParent = CreateSectionElement());

            RightParent.Add(RightLeftParent = CreateSectionElement());
            RightParent.Add(RightCenterParent = CreateSectionElement());
            RightParent.Add(RightRightParent = CreateSectionElement());

            LeftCenterParent.style.justifyContent = Justify.Center;
            RightCenterParent.style.justifyContent = Justify.Center;

            LeftRightParent.style.justifyContent = Justify.FlexEnd;
            RightLeftParent.style.justifyContent = Justify.FlexStart;

            AttachMyButton();
        }

        private static void OnUpdate()
        {
            if (_currentToolbar == null || !_isInitialized)
            {
                CreateToolbar();
            }
        }

        private static void CreateToolbar()
        {
            var toolbars = Resources.FindObjectsOfTypeAll(ToolbarType);
            if (toolbars.Length == 0) return;

            _currentToolbar = (ScriptableObject)toolbars[0];
            var root = _currentToolbar.GetType().GetField("m_Root", BindingFlags.NonPublic | BindingFlags.Instance);
            var mRoot = (VisualElement)root!.GetValue(_currentToolbar);

            mRoot.Q("ToolbarZoneLeftAlign").Add(LeftParent);
            mRoot.Q("ToolbarZoneRightAlign").Add(RightParent);
            _isInitialized = true;
        }

        private static VisualElement CreateParentElement()
        {
            return new VisualElement
            {
                style =
                {
                    flexGrow = 1,
                    flexDirection = FlexDirection.Row
                }
            };
        }

        private static VisualElement CreateSectionElement()
        {
            var ve = new VisualElement
            {
                style =
                {
                    flexGrow = 1,
                    flexDirection = FlexDirection.Row,
                    paddingLeft = 1,
                    paddingRight = 1,
                },
            };

            ve.AddToClassList("unity-editor-main-toolbar");
            return ve;
        }

        private static void AttachMyButton()
        {
            var methods = TypeCache.GetMethodsWithAttribute<EditorToolbarButtonAttribute>();
            var allAttributes = new Dictionary<MethodInfo, EditorToolbarButtonAttribute>();

            foreach (var m in methods)
            {
                var attribute = (EditorToolbarButtonAttribute)m.GetCustomAttribute(typeof(EditorToolbarButtonAttribute), false);
                if (attribute != null)
                    allAttributes.Add(m, attribute);
            }

            foreach (var item in allAttributes.OrderByDescending(x => x.Value.Priority))
            {
                CreateToolbarButton(() => item.Key.Invoke(null, null), item.Value);
            }
        }

        private static VisualElement CreateToolbarButton(Action onClick, EditorToolbarButtonAttribute attribute)
        {
            Button buttonVE = new(onClick)
            {
                tooltip = attribute.Tooltip,
                style =
                {
                    paddingRight = 8,
                    paddingLeft = 8,
                    justifyContent = Justify.Center,
                    display = DisplayStyle.Flex,
                    borderTopLeftRadius = 2,
                    borderTopRightRadius = 2,
                    borderBottomLeftRadius = 2,
                    borderBottomRightRadius = 2,
                    marginRight = 1,
                    marginLeft = 1,
                }
            };

            EditorApplication.playModeStateChanged += state =>
            {
                if (state == PlayModeStateChange.EnteredEditMode && attribute.DisableOnPlayMode)
                {
                    buttonVE.SetEnabled(true);
                }
                else if (state == PlayModeStateChange.EnteredPlayMode && attribute.DisableOnPlayMode)
                {
                    buttonVE.SetEnabled(false);
                }
            };

            buttonVE.AddToClassList("unity-toolbar-button");
            buttonVE.AddToClassList("unity-editor-toolbar-element");
            buttonVE.RemoveFromClassList("unity-button");

            VisualElement iconVE = new()
            {
                style =
                {
                    backgroundImage = Background.FromTexture2D((Texture2D)EditorGUIUtility.IconContent(attribute.IconName).image),
                    width = 16,
                    height = 16,
                    alignSelf = Align.Center,
                }
            };
            iconVE.AddToClassList("unity-editor-toolbar-element__icon");

            buttonVE.Add(iconVE);

            VisualElement parentVE = attribute.Position switch
            {
                EditorToolbarPosition.LeftLeft => LeftLeftParent,
                EditorToolbarPosition.LeftCenter => LeftCenterParent,
                EditorToolbarPosition.LeftRight => LeftRightParent,
                EditorToolbarPosition.RightLeft => RightLeftParent,
                EditorToolbarPosition.RightCenter => RightCenterParent,
                EditorToolbarPosition.RightRight => RightRightParent,
                _ => LeftLeftParent
            };

            parentVE.Add(buttonVE);
            return buttonVE;
        }
    }
}
