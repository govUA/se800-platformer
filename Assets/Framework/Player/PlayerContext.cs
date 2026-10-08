using Framework.Core.Input;
using Framework.Core.Motor;
using Framework.Core.Sensors;
using Framework.Data;

namespace Framework.Player
{
    public class PlayerContext
    {
        public ActorMotor2D Motor { get; }
        public EnvironmentDetector2D Sensors { get; }
        public MovementStatsSO Stats { get; }
        public IInputProvider Input { get; }
        public PlayerController Controller { get; }

        public PlayerContext(ActorMotor2D motor, EnvironmentDetector2D sensors, MovementStatsSO stats,
            IInputProvider input, PlayerController controller)
        {
            Motor = motor;
            Sensors = sensors;
            Stats = stats;
            Input = input;
            Controller = controller;
        }
    }
}