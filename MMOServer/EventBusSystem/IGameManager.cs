using System;
using System.Collections.Generic;

namespace MMOServer.EventBusSystem
{
    interface IGameManager
    {
        void HandleGameEvent(GameEvent gameEvent);
        List<Type> HandledGameEvents { get; }
    }
}
