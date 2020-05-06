using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardsRegistration
{
    class User
    {
        public string userID { get; set; }
        public string token { get; set; }
        public string building_id { get; set; }
        public string level_id { get; set; }

        public string Error { get; set; }
    }
}
