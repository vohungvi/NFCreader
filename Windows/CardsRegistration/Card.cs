using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardsRegistration
{
    class ParkingCard
    {
        string m_cardID;
        string m_cardNumber;
        string m_status;

        //public string _id { get; set; }
        public string cardID { get; set; }
        public string cardNumber { get; set; }
        public bool scanned { get; set; }
        //public string building_id { get; set; }
        //public string customer { get; set; }
        //public string note { get; set; }
        //public string userAdd { get; set; }
        //public string status { get; set; }
        //public string isDeleted { get; set; }
        //public string dateAdd { get; set; }
        //public string cardType_id { get; set; }
        //public string plate { get; set; }
        //public string userDelete { get; set; }
        //public string userUpdate { get; set; }


        public ParkingCard()
        {

        }
        public ParkingCard(string cardID, string cardNumber)
        {
            m_cardID = cardID;
            m_cardNumber = cardNumber;
        }

        public ParkingCard(string cardID, string cardNumber, string status)
        {
            m_cardID = cardID;
            m_cardNumber = cardNumber;
            m_status = status;
        }

        public Dictionary<string, string> ToDictionary()
        {
            var values = new Dictionary<string, string>
            {
                {"cardID", m_cardID},
                {"cardNumber", m_cardNumber},
                {"cardType_id", "VL"},
                {"status", m_status},
                {"building_id", Program.building_id},
                {"userAdd", Program.user},
                {"token", Program.token }
            };

            return values;
        }
    }
}
