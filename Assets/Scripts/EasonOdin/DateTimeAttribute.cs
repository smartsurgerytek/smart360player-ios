using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eason.Odin
{
    
    public class DateTimeAttribute : Attribute
    {
        public string format;

        public DateTimeAttribute(string format = "yyyy-MM-dd")
        {
            this.format = format;
        }
    }
}
