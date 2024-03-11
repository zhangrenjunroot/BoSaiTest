using BoSaiTest.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Transactions;

namespace BoSaiTest.Business
{
    public class CustomerHandle : ICustomerHandle
    {

        private readonly IMemoryCache _cache;
        public CustomerHandle(IMemoryCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// 更新客户分数
        /// </summary>
        /// <param name="customerID">客户编号</param>
        /// <param name="changeScore">改变分数</param>
        /// <returns></returns>
        public Result UpdateCustomer(Int64 customerID, decimal changeScore)
        {
            var result = new Result();
            if (_cache.Get(customerID) != null)
            {
                result.IsSuccess = false;
                result.ErrorMsg = "客户正在操作中，请稍后修改！";
                return result;
            }
            _cache.Set(customerID, DateTime.Now, TimeSpan.FromSeconds(10));
            var existCustomer = CustomerDB.Customers.FirstOrDefault(p => p.CustomerID == customerID);
            try
            {
                if (existCustomer != null)
                {
                    var oldScore = existCustomer.Score;
                    existCustomer.UpdateScore(changeScore);
                    ChangeCustomerRank();
                    result.RValue = existCustomer.Score;
                }
                else
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        var customer = new Customer();
                        customer.CustomerID = customerID;
                        customer.UpdateScore(changeScore);
                        CustomerDB.Customers.Add(customer);
                        scope.Complete();
                    }
                    //如果分数大于0，需要插入排行榜
                    if (changeScore > 0)
                    {
                        ChangeCustomerRank();

                    }
                    result.RValue = changeScore;

                }

            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = "操作失败：" + ex.Message;
                _cache.Remove(customerID);
            }
            _cache.Remove(customerID);
            return result;

        }



        /// <summary>
        /// 更改排名
        /// </summary>
        private void ChangeCustomerRank()
        {
            CustomerDB.Customers = CustomerDB.Customers.OrderByDescending(p => p.Score).ThenBy(p => p.CustomerID).ToList();
            for (int i = 0; i < CustomerDB.Customers.Count; i++)
            {
                var customer = CustomerDB.Customers[i];
                if (customer.Score > 0)
                {
                    customer.Rank = i + 1;
                }
            }

        }

        /// <summary>
        /// 获取指定排名范围内的客户
        /// </summary>
        /// <param name="start">开始排名</param>
        /// <param name="end">结束排名</param>
        /// <returns></returns>
        public Result GetCustomerByRank(int start, int end)
        {
            Result r = new Result();
            if (start <= 0 || end <= 0 || start > end)
            {

                r.IsSuccess = false;
                r.ErrorMsg = "参数错误！";
                return r;
            }
            var list = CustomerDB.Customers.Where(p => p.Rank >= start && p.Rank <= end);
            if (list.Count() == 0)
            {
                r.IsSuccess = false;
                r.ErrorMsg = "数据不存在!";
                return r;
            }
            r.RValue = list;
            return r;

        }

        /// <summary>
        /// 获取指定客户前后的客户(包含自己)
        /// </summary>
        /// <param name="customerID">客户id</param>
        /// <param name="low">低于当前客户排名的客户数量</param>
        /// <param name="high">高于当前客户排名的客户数量</param>
        /// <returns></returns>
        public Result GetCustomerByID(Int64 customerID, int low, int high)
        {
            Result r = new Result();
            if (customerID <= 0 || low < 0 || high < 0)
            {

                r.IsSuccess = false;
                r.ErrorMsg = "参数错误！";
                return r;
            }
            var customer = CustomerDB.Customers.FirstOrDefault(p => p.CustomerID == customerID);
            if (customer == null)
            {
                r.IsSuccess = false;
                r.ErrorMsg = $"指定客户ID[{customerID}]的客户不存在！";
                return r;

            }
            if (customer.Rank <= 0)
            {

                r.IsSuccess = false;
                r.ErrorMsg = $"指定客户ID[{customerID}]的客户不在排行榜中！";
                return r;

            }
            var start = customer.Rank - high;
            start = start <= 0 ? 1 : start;
            var end = customer.Rank + low;

            return GetCustomerByRank(start, end);

        }
    }
}
