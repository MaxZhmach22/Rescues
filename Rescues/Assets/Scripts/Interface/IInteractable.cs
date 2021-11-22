namespace Rescues
{
    public interface IInteractable
    {
        string Id { get; set; }
        bool IsInteractable { get; set; }
        string Description { get; set; }
    }
}
