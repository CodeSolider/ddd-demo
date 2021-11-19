namespace EbayPlatform.Domain.Core.Abstractions
{
    /// <summary>
    /// 生命周期为每次创建一个新实例
    /// </summary>
    public interface ITransientDependency : IDependency
    {
    }
}
