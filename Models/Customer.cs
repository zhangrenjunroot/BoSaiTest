namespace BoSaiTest.Models
{
    /// <summary>
    /// 客户信息
    /// </summary>
    public class Customer
    {

        public Customer()
        {
            //默认分数为0
            this._score = 0;
            //默认排名为-1.表示不在排行榜中
            this.Rank = -1;

        }

        /// <summary>
        /// 客户ID
        /// </summary>
        public Int64 CustomerID { get; set; }



        private decimal _score;
        /// <summary>
        /// 客户积分
        /// </summary>
        public decimal Score
        {
            get { return _score; }
        }

        /// <summary>
        /// 排名
        /// </summary>
        public int Rank { get; set; }


        public void UpdateScore(decimal changeSocre)
        {
            if (changeSocre < -1000 || changeSocre > 1000)
            {
                throw new ArgumentException("改变分数的范围必须是-1000到1000之间！");
            }
            this._score += changeSocre;
            //如果分数小于等于0，排名置为-1，标识剔除排行榜
            if (this.Score <= 0)
            {
                this.Rank = -1;
            }
        }

    }
}
