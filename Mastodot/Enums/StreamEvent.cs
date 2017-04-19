using System;
namespace Mastodot.Enums
{
    public enum StreamEvent
    {
        Update,
        Notification,
        Delete
    }

    internal static class StreamEventExtentions
    {
        public static StreamEvent FromString(string ev)
        {
            switch (ev.ToLower().Trim())
            {
                case "update":
                    return StreamEvent.Update;
                case "notification":
                    return StreamEvent.Notification;
                case "delete":
                    return StreamEvent.Delete;
                default:
                    throw new NotSupportedException($"Unknown StreamEvent type: {ev}");
            }
        }
    }
}
