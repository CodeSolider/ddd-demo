namespace EbayPlatform.Domain.Core.Abstractions
{
    /// <summary>
    /// 实体接口
    /// </summary>
    public interface IEntity
    {
        object[] GetKeys();
    }

    /// <summary>
    /// 实体接口
    /// </summary>
    public interface IEntity<TKey>: IEntity
    {
        TKey Id { get; }
    }
}
