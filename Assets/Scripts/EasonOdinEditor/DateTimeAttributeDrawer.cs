using Sirenix.OdinInspector.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
namespace Eason.Odin.Editor
{


     public class DateTimeAttributeDrawer : OdinAttributeDrawer<DateTimeAttribute>
    {
        private string _tempString;
        private string _id;
        private bool _focused;
        private bool _justFocused;
        private bool _justLostFocus;
        protected override void Initialize()
        {
            base.Initialize();
            _tempString = new DateTime((long)Property.ValueEntry.WeakSmartValue).ToString(Attribute.format);
            _id = Guid.NewGuid().ToString();
            Debug.Log(_id);
        }
        protected override void DrawPropertyLayout(GUIContent label)
        {
            if(Property.ValueEntry.TypeOfValue != typeof(Int64))
            {
                base.DrawPropertyLayout(label);
                return;
            }
            GUI.SetNextControlName(_id);
            if(label == null)
            {
                _tempString = GUILayout.TextField(_tempString);
            }
            else
            {
                _tempString = EditorGUILayout.TextField(label, _tempString);
            }

            if (GUI.GetNameOfFocusedControl() == _id)
            {
                if (!_focused) _justFocused = true;
                _focused = true;
            }
            else
            {
                if (_focused) _justLostFocus = true;
                _focused = false;
            }
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return ) { _justLostFocus = true; }

            if (_justLostFocus)
            {
                try
                {
                    Property.ValueEntry.WeakSmartValue = DateTime.Parse(_tempString).Ticks; 
                }
                catch (Exception e){
                    Debug.LogException(e);
                }

                _tempString = new DateTime((long)Property.ValueEntry.WeakSmartValue).ToString(Attribute.format);
            }
            _justFocused = _justLostFocus = false;
        }
    }
}