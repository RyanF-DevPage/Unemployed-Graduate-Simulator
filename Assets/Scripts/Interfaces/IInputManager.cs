using System;
using UnityEngine;

namespace Simulator_Game
{
    public interface IInputManager
    {
        event Action<Vector2> OnWorldClick;
    }
}
