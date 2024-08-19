using Microsoft.Data.SqlClient;
using MVCE.Server.Tables;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace MVCE.Models
{
    public record class User
    {
        internal string Name { get; set; }
        protected internal string Password { get; set; }
        internal Type_Access FullAccess { get; private set; }
        private dbUser db = new dbUser();
        public User(string name, string password)
        {
            Name = name;
            Password = password;
            FullAccess = Type_Access.PartialAccess;
        }
        public User(string name)
        {
            Name = name;
            Password = "";
            FullAccess = Type_Access.PartialAccess;
        }
        public User()
        {
            Name = "";
            Password = "";
            FullAccess = Type_Access.PartialAccess;
        }
        public void Insert()
        {
            db.Registration(this);
        }
        public bool Select()
        {
            SqlDataReader reader = db.Authorization(this.Name, this.Password);
            if(reader.HasRows)
            {
                reader.Read();
                string Access = reader["FullAccess"].ToString();
                FullAccess = (Type_Access)int.Parse(Access);
                reader.Close();
                return true;
            }
            reader.Close();
            return false;
        }
        public bool Search_Name()
        {
            bool is_existed = db.Search_User(this.Name) != null ? true : false;
            return is_existed;
        }
        public void SearchByName()
        {
            SqlDataReader reader = db.SearchByName(this.Name);
            if (reader.HasRows)
            {
                reader.Read();
                this.Password = reader["Password"].ToString();
                string Access = reader["FullAccess"].ToString();
                FullAccess = (Type_Access)int.Parse(Access);
            }
            reader.Close();
        }
        public void Update(string LastName)
        {
            db.Update_User(LastName, this);
        }
        public void Delete()
        {
            db.Delete_User(this.Name);
        }
    }
}
