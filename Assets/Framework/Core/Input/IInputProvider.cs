namespace Framework.Core.Input
{
    public interface IInputProvider
    {
        float HorizontalInput { get; }
        bool JumpPressed { get; }
        bool JumpHeld { get; }
        bool JumpReleased { get; }
    }
}