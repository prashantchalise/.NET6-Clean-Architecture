
namespace PROJBP.Model
{
    public interface IEntity<T>
    {
        // T Id { get; set; }  
    }

    public abstract class BaseEntity
    {

    }

    public abstract class Entity<T> : BaseEntity, IEntity<T>
    {
        //public virtual T Id { get; set; }
    }

}
