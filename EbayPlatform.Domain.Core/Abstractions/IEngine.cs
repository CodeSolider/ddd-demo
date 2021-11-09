namespace EbayPlatform.Domain.Core.Abstractions
{
    /// <summary>
    /// 引擎接口
    /// </summary>
    public interface IEngine
    {
        /// <summary>
        /// 创建服务，泛型类型转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Resolve<T>() where T : class; 
    }
}
