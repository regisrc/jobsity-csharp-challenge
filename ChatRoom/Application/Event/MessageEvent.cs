﻿namespace Application.Event
{
    public class MessageEvent
    {
        public Guid Guid { get; set; }

        public string Message { get; set; }

        public Guid ChatRoomId { get; set; }
    }
}
