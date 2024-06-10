namespace Services
{
    public interface IMonoFactory<out T> 
    {
        public T CreateObject();
    }
}