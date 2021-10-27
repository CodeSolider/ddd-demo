using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EbayPlatform.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "主键Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "账单 ID"),
                    ShopName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "店铺编号，来自ERP"),
                    CurrencyCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, comment: "币种"),
                    AccountState = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, comment: "账单状态"),
                    InvoicePayment = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    InvoicePaymentCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    InvoiceCredit = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    InvoiceCreditCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    InvoiceNewFee = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    InvoiceNewFeeCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Balance = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    BalanceCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    AdditionalAccount_CurrencyCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true, comment: "币种"),
                    AdditionalAccount_AccountCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "账户编号"),
                    SyncDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getDate()", comment: "同步日期")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                },
                comment: "账单");

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "主键Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "eBay下载的订单ID、唯一"),
                    OrderStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, comment: "订单状态"),
                    ShopName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "店铺编号，来自ERP"),
                    AdjustmentAmount = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    AdjustmentAmountCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    AmountPaidCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    AmountSaved = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    AmountSavedCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PaymentMethods = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true, comment: "付款方式"),
                    SellerEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "卖家邮箱地址"),
                    SellerUserID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "订单创建日期"),
                    SyncDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getDate()", comment: "同步日期"),
                    Subtotal = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    SubtotalCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    TotalCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CheckoutStatus_EBayPaymentStatus = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true, comment: "EBayPaymentStatus"),
                    CheckoutStatus_PaymentMethod = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, comment: "结账方式"),
                    CheckoutStatus_Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true, comment: "状态"),
                    CheckoutStatus_IntegratedMerchantCreditCardEnabled = table.Column<bool>(type: "bit", maxLength: 10, nullable: true),
                    CheckoutStatus_PaymentInstrument = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CheckoutStatus_LastModifiedTime = table.Column<DateTime>(type: "datetime2", maxLength: 30, nullable: true, comment: "最后更新时间，非系统时间"),
                    ShippingDetail_SalesTax_SalesTaxPercent = table.Column<float>(type: "real", nullable: true, comment: "营业税率"),
                    ShippingDetail_SalesTax_SalesTaxState = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    SalesTaxAmount = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    SalesTaxAmountCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ShippingDetail_SellingManagerSalesRecordNumber = table.Column<int>(type: "int", nullable: true),
                    ShippingDetail_GetItFast = table.Column<bool>(type: "bit", nullable: true, comment: "是否加急"),
                    ShippingAddress_AddressID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "地址ID，同样具有唯一性，非系统ID"),
                    ShippingAddress_Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true, comment: "地址名称"),
                    ShippingAddress_Street = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true, comment: "街道1"),
                    ShippingAddress_Street1 = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true, comment: "街道1"),
                    ShippingAddress_Street2 = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true, comment: "街道2"),
                    ShippingAddress_CityName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true, comment: "城市名称"),
                    ShippingAddress_StateOrProvince = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true, comment: "地址ID，同样具有唯一性，非系统ID"),
                    ShippingAddress_Country = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true, comment: "地址ID，同样具有唯一性，非系统ID"),
                    ShippingAddress_CountryName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true, comment: "国家名称"),
                    ShippingAddress_Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, comment: "电话号码"),
                    ShippingAddress_PostalCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "邮编"),
                    ShippingAddress_AddressOwner = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true, comment: "地址所属者"),
                    ShippingServiceSelected_ShippingService = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true, comment: "运输服务"),
                    ShippingServiceCost = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: true),
                    ShippingServiceSelected_ShippingServicePriority = table.Column<int>(type: "int", nullable: true),
                    ShippingServiceSelected_ExpeditedService = table.Column<bool>(type: "bit", nullable: true, comment: "是否加急服务"),
                    ShippingServiceSelected_ShippingTimeMin = table.Column<int>(type: "int", nullable: true, comment: "装运时间"),
                    ShippingServiceSelected_ShippingTimeMax = table.Column<int>(type: "int", nullable: true, comment: "装运时间"),
                    ShippingServiceSelected_ShippingPackage_StoreID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ShippingServiceSelected_ShippingPackage_ShippingTrackingEvent = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ShippingServiceSelected_ShippingPackage_ScheduledDeliveryTimeMin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShippingServiceSelected_ShippingPackage_ScheduledDeliveryTimeMax = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShippingServiceSelected_ShippingPackage_ActualDeliveryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShippingServiceSelected_ShippingPackage_EstimatedDeliveryTimeMin = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "预计发货时间 Min"),
                    ShippingServiceSelected_ShippingPackage_EstimatedDeliveryTimeMax = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "预计发货时间 Max"),
                    ShippingServiceSelected_Id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                },
                comment: "订单");

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "主键Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "eBay下载的产品ID、唯一"),
                    MSKU = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "渠道SKU"),
                    ShopName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "店铺编号，来自ERP"),
                    SiteCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, comment: "站点"),
                    StartPrice = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    StartPriceCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    BuyerGuaranteePrice = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    BuyerGuaranteePriceCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    BuyItNowPrice = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    BuyItNowPriceCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ReservePrice = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    ReservePriceCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "数量"),
                    QuantityAvailableHint = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    QuantityThreshold = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Country = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CurrencyCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    HitCount = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    HitCounter = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InventoryTrackingMethod = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ListingType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ListingSubType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentMethods = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ConvertedCurrentPrice = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    ConvertedCurrentPriceCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CurrentPrice = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    CurrentPriceCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SellingStatus_ListingStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SellingStatus_QuantitySold = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    SellingStatus_AdminEnded = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    SellingStatus_PromotionalSaleDetail_StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OriginalPrice = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    OriginalPriceCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SellingStatus_PromotionalSaleDetail_EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FreeAddedCategoryID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FreeAddedCategoryName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PrimaryCategoryID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PrimaryCategoryName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    SecondaryCategoryID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SecondaryCategoryName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PrivateListing = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ItemRevised = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SyncDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getDate()", comment: "同步日期")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                },
                comment: "产品");

            migrationBuilder.CreateTable(
                name: "SyncTaskJobConfig",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "主键Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Job名称"),
                    JobDesc = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "Job描述"),
                    JobAssemblyName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true, comment: "程序集名称"),
                    Cron = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "Cron"),
                    CronDesc = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true, comment: "Cron描述"),
                    JobStatus = table.Column<int>(type: "int", nullable: false, comment: "运行状态"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getDate()", comment: "创建日期"),
                    ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "更新日期")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyncTaskJobConfig", x => x.Id);
                },
                comment: "同步任务作业配置");

            migrationBuilder.CreateTable(
                name: "SystemLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "主键Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObjectId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "外键Id"),
                    LogType = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "日志类型"),
                    Content = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true, comment: "内容"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getDate()", comment: "创建日期")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemLog", x => x.Id);
                },
                comment: "系统日志表");

            migrationBuilder.CreateTable(
                name: "AccountDetail",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "主键Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ItemID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Date = table.Column<DateTime>(type: "Date", nullable: false),
                    AccountType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Balance = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    BalanceCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    GrossDetailAmount = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    GrossDetailAmountCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ConversionRate = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    ConversionRateCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    NetDetailAmount = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    NetDetailAmountCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    VATPercent = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    OrderLineItemID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TransactionID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReceivedTopRatedDiscount = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountDetail_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order_ShippingServiceOptions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "主键Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShippingService = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true, comment: "运输服务"),
                    ShippingServiceCost = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false, defaultValue: 0m),
                    ShippingServiceCostCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ShippingServicePriority = table.Column<int>(type: "int", nullable: true),
                    ExpeditedService = table.Column<bool>(type: "bit", nullable: true, comment: "是否加急服务"),
                    ShippingTimeMin = table.Column<int>(type: "int", nullable: true, comment: "装运时间"),
                    ShippingTimeMax = table.Column<int>(type: "int", nullable: true, comment: "装运时间"),
                    ShippingPackage_StoreID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ShippingPackage_ShippingTrackingEvent = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ShippingPackage_ScheduledDeliveryTimeMin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShippingPackage_ScheduledDeliveryTimeMax = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShippingPackage_ActualDeliveryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShippingPackage_EstimatedDeliveryTimeMin = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "预计发货时间"),
                    ShippingPackage_EstimatedDeliveryTimeMax = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "预计发货时间"),
                    ShippingDetailOrderId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_ShippingServiceOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_ShippingServiceOptions_Order_ShippingDetailOrderId",
                        column: x => x.ShippingDetailOrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderTransaction",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "主键Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrderLineItemID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SiteCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, comment: "站点"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "标题"),
                    ConditionID = table.Column<int>(type: "int", nullable: true, comment: "条件性ID"),
                    ConditionDisplayName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "条件性ID"),
                    QuantityPurchased = table.Column<int>(type: "int", nullable: false, comment: "购买数"),
                    TransactionPrice = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    TransactionPriceCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Status_PaymentHoldStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, comment: "待付款状态"),
                    Status_InquiryStatus = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true, comment: "查询状态"),
                    Status_ReturnStatus = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true, comment: "退回状态"),
                    ShippingServiceSelected_ShippingService = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true, comment: "运输服务"),
                    ShippingServiceCost = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    ShippingServiceCostCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ShippingServiceSelected_ShippingServicePriority = table.Column<int>(type: "int", nullable: true),
                    ShippingServiceSelected_ExpeditedService = table.Column<bool>(type: "bit", nullable: true, comment: "是否加急服务"),
                    ShippingServiceSelected_ShippingTimeMin = table.Column<int>(type: "int", nullable: true, comment: "装运时间"),
                    ShippingServiceSelected_ShippingTimeMax = table.Column<int>(type: "int", nullable: true, comment: "装运时间"),
                    ShippingServiceSelected_ShippingPackage_StoreID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ShippingServiceSelected_ShippingPackage_ShippingTrackingEvent = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ShippingServiceSelected_ShippingPackage_ScheduledDeliveryTimeMin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShippingServiceSelected_ShippingPackage_ScheduledDeliveryTimeMax = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShippingServiceSelected_ShippingPackage_ActualDeliveryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShippingServiceSelected_ShippingPackage_EstimatedDeliveryTimeMin = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "预计发货时间"),
                    ShippingServiceSelected_ShippingPackage_EstimatedDeliveryTimeMax = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "预计发货时间"),
                    ShippingServiceSelected_Id = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "交易创建日期，非系统日期"),
                    OrderId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderTransaction_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShopTask",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShopName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "店铺名称"),
                    ParamValue = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true, comment: "参数值"),
                    ShopTaskStatus = table.Column<int>(type: "int", nullable: false),
                    SyncTaskJobConfigId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopTask_SyncTaskJobConfig_SyncTaskJobConfigId",
                        column: x => x.SyncTaskJobConfigId,
                        principalTable: "SyncTaskJobConfig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountDetail_AccountId",
                table: "AccountDetail",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountDetail_RefNumber_ItemID_Date_AccountType",
                table: "AccountDetail",
                columns: new[] { "RefNumber", "ItemID", "Date", "AccountType" });

            migrationBuilder.CreateIndex(
                name: "IX_Order_ShippingServiceOptions_ShippingDetailOrderId",
                table: "Order_ShippingServiceOptions",
                column: "ShippingDetailOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTransaction_OrderId",
                table: "OrderTransaction",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopTask_SyncTaskJobConfigId",
                table: "ShopTask",
                column: "SyncTaskJobConfigId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountDetail");

            migrationBuilder.DropTable(
                name: "Order_ShippingServiceOptions");

            migrationBuilder.DropTable(
                name: "OrderTransaction");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "ShopTask");

            migrationBuilder.DropTable(
                name: "SystemLog");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "SyncTaskJobConfig");
        }
    }
}
