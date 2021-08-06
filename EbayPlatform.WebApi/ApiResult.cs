namespace EbayPlatform.WebApi
{
    /// <summary>
    /// 统一返回格式
    /// </summary>
    public class ApiResult
    {
        /// <summary>
        /// 返回代码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <returns></returns>
        public static ApiResult OK()
        {
            return new ApiResult
            {
                Code = 200,
                Message = "success"
            };
        }

        /// <summary>
        /// 返回失败
        /// </summary>
        /// <returns></returns>
        public static ApiResult Fail()
        {
            return new ApiResult
            {
                Code = -1,
                Message = "fail"
            };
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiResult OK(string message)
        {
            return new ApiResult
            {
                Code = 200,
                Message = message
            };
        }

        /// <summary>
        /// 返回失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiResult Fail(string message)
        {
            return new ApiResult
            {
                Code = -1,
                Message = message
            };
        }

        /// <summary>
        /// 返回成功的消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult OK<T>(string message, T data)
        {
            return new ApiResult<T>
            {
                Code = 200,
                Message = message,
                Data = data,
            };
        }

        /// <summary>
        /// 返回失败的消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult Fail<T>(string message, T data)
        {
            return new ApiResult<T>
            {
                Code = -1,
                Message = message,
                Data = data,
            };
        }
    }

    public class ApiResult<T> : ApiResult
    {
        public T Data { get; set; }
    }
}
