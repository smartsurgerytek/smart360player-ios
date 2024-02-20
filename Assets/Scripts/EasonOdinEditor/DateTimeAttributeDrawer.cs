using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System;
using UnityEditor;
using UnityEngine;
namespace Eason.Odin.Editor
{
     public class DateTimeAttributeDrawer : OdinAttributeDrawer<DateTimeAttribute>
    {
        private string _tempString;
        private string _id;
        private bool _focused;
        private bool _justFocused;
        private bool _justLostFocus;
        private bool _initialized;
        protected override void Initialize()
        {
            base.Initialize();
            if (_initialized) return;
            _id = Guid.NewGuid().ToString();
            _tempString = new DateTime((long)Property.ValueEntry.WeakSmartValue).ToString(Attribute.format);
            _initialized = true;
        }
        protected override void DrawPropertyLayout(GUIContent label)
        {
            if(Property.ValueEntry.TypeOfValue != typeof(Int64))
            {
                CallNextDrawer(label);
                return;
            }
            Property.ValueEntry.Update();


            var color = GUI.color;
            var backgroundColor = GUI.backgroundColor;
            var valueChanged = false;
            var valueInvalid = false;
            try
            {
                var tempDateTime = DateTime.Parse(_tempString).Ticks;
                if (tempDateTime != (long)Property.ValueEntry.WeakSmartValue) valueChanged = true;
            } 
            catch(Exception ex) 
            {
                valueInvalid = true;
            }
            if (valueInvalid) { 
                GUI.contentColor = new Color(1, 0, 0);
                GUI.backgroundColor = new Color(0, 0, 0);
            } else if (valueChanged)
            {
                var tempDateTime = DateTime.Parse(_tempString).Ticks;

                GUI.contentColor = new Color(1, 1, 0);
                GUI.backgroundColor = new Color(0, 0, 0);
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

            GUI.contentColor = color;
            GUI.backgroundColor = backgroundColor;


            if (!_focused && GUI.GetNameOfFocusedControl() == _id)
            {
                _justFocused = true;
                _focused = true;
            }
            else if(_focused && GUI.GetNameOfFocusedControl() != _id )
            {
                _justLostFocus = true;
                _focused = false;
            }
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return ) 
            { 
                _justLostFocus = true;
            }
            if (!_focused && !_justLostFocus)
            {
                Property.ValueEntry.Update();
                _tempString = new DateTime((long)Property.ValueEntry.WeakSmartValue).ToString(Attribute.format);
            }
            if (_justLostFocus)
            {
                try
                {
                    Property.ValueEntry.Update();
                    Property.ValueEntry.WeakSmartValue = DateTime.Parse(_tempString).Ticks;
                    Property.ValueEntry.ApplyChanges();
                    GUIHelper.RequestRepaint();
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