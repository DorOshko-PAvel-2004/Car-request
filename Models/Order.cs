using Microsoft.Data.SqlClient;
using MVCE.Data;
using MVCE.Server.Tables;

namespace MVCE.Models
{
    public class Order
    {
        internal int OrderID { get; set; }
        internal int Position { get; set; }
        internal int FinishTime { get; set; }
        internal string UserLogin { get; set; }
        dbOrders sql = new dbOrders();

        public Order()
        {
            Position = 0;
            FinishTime = 0;
            UserLogin = "";
        }
        public Order(string userLogin)
        {
            UserLogin = userLogin;
            SearchPosition();
            SearchTime();
        }
        public Order(int OrderID)
        {
            SqlDataReader reader = sql.Select(OrderID);
            if (reader.HasRows)
            {
                reader.Read();
                this.Position = int.Parse(reader["Position"].ToString());
                this.FinishTime = reader["FinishTime"].ToString() == string.Empty ? 0 : int.Parse(reader["FinishTime"].ToString());
                this.UserLogin = reader["UserLogin"].ToString();
                this.OrderID = OrderID;
            }
            reader.Close();
        }
        public int Insert()
        {
            int reader = sql.Create(this);
            try
            {   if (reader == 0) { throw new ArgumentNullException(); }
                return reader;
            }
            catch (ArgumentNullException ex)
            {
                return -1;
            }
        }
        public void Delete()
        {
            sql.Delete(this.OrderID);
        }
        private void SearchPosition()
        {
            this.Position = sql.SearchLastPosition() + 1;
        }
        private void SearchTime()
        {
            this.FinishTime = sql.SearchLastTime();
        }
        public void Update()
        {
            sql.Update(this);
        }
    }
}
