namespace T_T_PandorasBox.States.MainWindowViews;

public interface IMainWindowView
{
    public string Name { get; }
    public void Render(float deltaTime);
}