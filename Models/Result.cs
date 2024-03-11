namespace BoSaiTest.Models
{
    /// <summary>
    /// 返回结果
    /// </summary>
    public class Result
    {
        public bool IsSuccess { get; set; } = true;
        public string ErrorMsg { get; set; }

        public object RValue { get; set; }
    }
}
