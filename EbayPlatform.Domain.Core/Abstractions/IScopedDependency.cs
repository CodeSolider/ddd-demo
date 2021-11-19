namespace EbayPlatform.Domain.Core.Abstractions
{
    /// <summary>
    /// 生命周期为每次请求创建一个实例
    /// </summary>
    public interface IScopedDependency : IDependency
    {
    }
}
