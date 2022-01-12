namespace JustConveyors.Source.Scripting;

internal interface IScript
{
    void Start();
    void Update();
    void LateUpdate();
    void Close();
}
