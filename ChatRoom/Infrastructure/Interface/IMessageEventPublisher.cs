﻿namespace Infrastructure.Interface
{
    public interface IMessageEventPublisher
    {
        void Publish(object message);

        void PublishBotMessage(object message);
    }
}
