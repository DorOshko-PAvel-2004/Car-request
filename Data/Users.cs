using Microsoft.AspNetCore.Mvc;
using MVCE.Models;
using MVCE.Server.Tables;
using System.Xml;

namespace MVCE.Data
{
    public static class Users
    {
        internal static dbUser db = new dbUser();
        public static List<User> Select_All()
        {
            var reader = db.Select_All();
            List<User> users = new List<User>();
            if (reader.HasRows)
            {
                User user;
                while(reader.Read())
                {
                    if (reader["FullAccess"].ToString() == ((int)Type_Access.FullAccess).ToString())continue;
                    user = new User();
                    user.Name = reader["Name"].ToString();
                    user.Password = reader["Password"].ToString();
                    users.Add(user);
                }
            }
            reader.Close();
            return users;
        }
        public static XmlDocument UsersToXml()
        {
            List<User> users = new List<User>();
            users = Select_All();

            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);
            //(2) string.Empty makes cleaner code
            XmlElement body = doc.CreateElement(string.Empty, "body", string.Empty);
            doc.AppendChild(body);
            foreach (var user in users)
            {
                XmlElement xuser = doc.CreateElement(string.Empty, "User", string.Empty);
                body.AppendChild(xuser);

                XmlElement Name = doc.CreateElement(string.Empty, "Name", string.Empty);
                XmlText _Name = doc.CreateTextNode(user.Name);
                Name.AppendChild(_Name);
                xuser.AppendChild(Name);

                XmlElement Password = doc.CreateElement(string.Empty, "Password", string.Empty);
                XmlText _Password = doc.CreateTextNode(user.Password);
                Password.AppendChild(_Password);
                xuser.AppendChild(Password);

                XmlElement Access = doc.CreateElement(string.Empty, "FullAccess", string.Empty);
                XmlText _Access = doc.CreateTextNode(((int)user.FullAccess).ToString());
                Access.AppendChild(_Access);
                xuser.AppendChild(Access);
            }
            //doc.Save("D:\\users.xml");
            return doc;
        }
    }
}
