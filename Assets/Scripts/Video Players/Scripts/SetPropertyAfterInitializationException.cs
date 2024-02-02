using System;

namespace SmartSurgery.VideoControllers
{
    public class SetPropertyAfterInitializationException : Exception
    {
        public SetPropertyAfterInitializationException() : base("You can't set the property after initialization.")
        {
        }
    }
}