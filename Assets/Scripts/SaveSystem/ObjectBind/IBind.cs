public interface IBind<in T> where T: ISaveable
{
    public string Id { get; }
    public void Bind(T data);
}