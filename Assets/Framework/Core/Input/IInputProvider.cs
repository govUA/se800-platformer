namespace Framework.Core.Input
{
    public interface IInputProvider
    {
        float HorizontalInput { get; }
        bool JumpPressed { get; } 
        //тут було щось непотрібне
        bool JumpReleased { get; }
    }
}