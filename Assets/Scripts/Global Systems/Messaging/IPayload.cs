using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RML.Messaging
{
    public interface IPayload
    {
        string EventName { get; }
    }
}
