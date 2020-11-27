using Unity.Entities;
using UnityEngine;

namespace Shooter.ECS
{
    [GenerateAuthoringComponent]
    struct InputData : IComponentData
    {
        public KeyCode upKey;
        public KeyCode downKey;
        public KeyCode leftKey;
        public KeyCode rightKey;    
        public KeyCode fireKey;
    }

    public class InputComponent { }
}
