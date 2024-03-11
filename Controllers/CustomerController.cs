using BoSaiTest.Business;
using BoSaiTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace BoSaiTest.Controllers
{

    [ApiController]
    [Route("[controller]/[action]")]
    public class CustomerController : Controller
    {
        ICustomerHandle _customerHandle;

        public CustomerController(ICustomerHandle customerHandle)
        {
            _customerHandle = customerHandle;
        }

        /// <summary>
        /// 更新客户
        /// </summary>
        /// <param name="customerID">客户编号</param>
        /// <param name="score">分数</param>
        /// <returns></returns>
        [HttpPost(Name = "UpdateCustomer")]
        public JsonResult UpdateCustomer(Int64 customerID, decimal score)
        {
            var r = _customerHandle.UpdateCustomer(customerID, score);
            return Json(r);
        }

        /// <summary>
        /// 根据排名范围获取客户信息
        /// </summary>
        /// <param name="start">开始排名</param>
        /// <param name="end">结束排名</param>
        /// <returns></returns>
        [HttpGet(Name = "GetCustomerByRank")]
        public JsonResult GetCustomerByRank(int start,int end)
        {
            var r = _customerHandle.GetCustomerByRank(start, end);
            return Json(r);
        }

        /// <summary>
        /// 获取指定客户的邻居
        /// </summary>
        /// <param name="customerID">客户编号</param>
        /// <param name="low">低于当前客户排名的客户个数</param>
        /// <param name="high">高于当前客户排名的客户个数</param>
        /// <returns></returns>
        [HttpGet(Name = "GetCustomerByID")]
        public JsonResult GetCustomerByID(Int64 customerID, int low,int high)
        {
            var r = _customerHandle.GetCustomerByID(customerID, low,high);
            return Json(r);
        }


    }
}
