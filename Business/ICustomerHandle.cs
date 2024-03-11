using BoSaiTest.Models;

namespace BoSaiTest.Business
{
    public interface ICustomerHandle
    {
        /// <summary>
        /// 更新客户信息
        /// </summary>
        /// <param name="customerID">客户编号</param>
        /// <param name="changeScore">改变分数</param>
        /// <returns></returns>
        Result UpdateCustomer(Int64 customerID, decimal changeScore);


        /// <summary>
        /// 获取指定排名范围内的客户
        /// </summary>
        /// <param name="start">开始排名</param>
        /// <param name="end">结束排名</param>
        /// <returns></returns>
        Result GetCustomerByRank(int start, int end);

        /// <summary>
        /// 获取指定客户前后的客户(包含自己)
        /// </summary>
        /// <param name="customerID">客户id</param>
        /// <param name="low">低于当前客户排名的客户数量</param>
        /// <param name="high">高于当前客户排名的客户数量</param>
        /// <returns></returns>
        Result GetCustomerByID(Int64 customerID, int low, int high);
    }
}
