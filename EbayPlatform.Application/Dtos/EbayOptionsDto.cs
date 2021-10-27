using EbayPlatform.Domain.Models.Enums;

namespace EbayPlatform.Application.Dtos
{
    public class EbayOptionsDto
    {
        #region Const String
        private const string Sandbox_EPSServerUrl = "https://api.sandbox.ebay.com/ws/api.dll";
        private const string Production_EPSServerUrl = "https://api.ebay.com/ws/api.dll";

        private const string Sandbox_SoapApiServerUrl = "https://api.sandbox.ebay.com/wsapi";
        private const string Production_SoapApiServerUrl = "https://api.ebay.com/wsapi";
        #endregion

        /// <summary>
        /// EPSServerUrl
        /// </summary>
        public string EPSServerUrl
        {
            get
            {
                return Environment == EnvironmentsType.Sandbox ? Sandbox_EPSServerUrl : Production_EPSServerUrl;
            }
        }

        /// <summary>
        ///  SoapApiServerUrl
        /// </summary>
        public string SoapApiServerUrl
        {
            get
            {
                return Environment == EnvironmentsType.Sandbox ? Sandbox_SoapApiServerUrl : Production_SoapApiServerUrl;
            }
        }

        /// <summary>
        /// 环境
        /// </summary>
        public EnvironmentsType Environment { get; set; }
    }

}
