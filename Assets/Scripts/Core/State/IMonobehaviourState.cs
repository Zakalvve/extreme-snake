using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace ExtremeSnake.Core
{
    public interface IMonobehaviourState : IState
    {
        public void Update();
        public void FixedUpdate();
        public void LateUpdate();
    }
}
