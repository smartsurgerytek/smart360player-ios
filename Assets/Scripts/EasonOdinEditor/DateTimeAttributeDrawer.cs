using Sirenix.OdinInspector.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Eason.Odin.Editor
{


     public class DateTimeAttributeDrawer : OdinAttributeDrawer<DateTimeAttribute>
    {
        private string tempString;
        protected override void DrawPropertyLayout(GUIContent label)
        {
            if(Property.ParentType != typeof(long))
            {
                
                base.DrawPropertyLayout(label);
                var  input = GUI.TextField(Property.LastDrawnValueRect, new DateTime((long)Property.ValueEntry.WeakSmartValue).ToString(Attribute.format));
                try
                {
                    var dateTime = DateTime.Parse(input);
                    Property.ValueEntry.WeakSmartValue = dateTime.Ticks;
                }
                catch { }

                return;
            }

        }
    }
}