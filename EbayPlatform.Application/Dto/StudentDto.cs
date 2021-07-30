
namespace EbayPlatform.Application.Dto
{
    public class StudentDto
    {
        /// <summary>
        /// 学生
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public short Gender { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 身高
        /// </summary>
        public decimal Height { get; set; }
    }
}
