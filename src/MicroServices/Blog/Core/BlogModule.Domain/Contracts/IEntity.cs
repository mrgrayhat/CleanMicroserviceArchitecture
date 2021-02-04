
namespace BlogModule.Domain.Contracts
{
    /// <summary>
    /// generic entity identifier
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class IEntity<T>
    {
        public virtual T Id { get; set; }
    }
}
