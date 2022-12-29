using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordsManagement_Client.Model
{
    public class Admin
    {
        public Admin(string adminName, string adminPass)
        {
            AdminName = adminName;
            AdminPass = adminPass;
        }

        public int Id { get; set; }
        public string AdminName { get; set; } = null!;
        public string AdminPass { get; set; } = null!;
    }
}
