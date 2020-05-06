using CardReaderService;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CardsRegistration
{
    public partial class frmCheck : Form
    {
        List<ParkingCard> m_lstCards;
        int m_count = 0;
        public frmCheck()
        {
            InitializeComponent();

            m_lstCards = new List<ParkingCard>();
        }

        private void FrmCheck_Load(object sender, EventArgs e)
        {
            lblUser.Text = "User: " + Program.user;
            lblBuilding.Text = "Bãi xe: " + Program.building_id;

            LoadCard();
            SetCardReader();
        }

        private async void LoadCard()
        {
            var values = new Dictionary<string, string>
            {
                {"token", Program.token }
            };

            var content = new FormUrlEncodedContent(values);
            HttpClient client = new HttpClient();
            var result = await client.PostAsync(Program.host + "api/parkingCard/getList", content);

            string responseString = await result.Content.ReadAsStringAsync();
            dynamic parkingCards = JsonConvert.DeserializeObject(responseString);
            
            foreach (var parkingCard in parkingCards)
            {
                m_lstCards.Add(new ParkingCard()
                {
                    cardID = parkingCard.cardID,
                    cardNumber = parkingCard.cardNumber,
                    scanned = false
                });
            }

            this.Text = "Kiểm kê thẻ " + m_count + " / " + m_lstCards.Count;

            grdData.DataSource = m_lstCards;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////

        private void SetCardReader()
        {
            StopCard();
            var lst = ModWinsCardReader.ListModWinsCards();
            if (lst != null)
            {
                foreach (var item in lst)
                {
                    CurrentListCardReader.AddCardInfo(new GreenCardReaderInfo() { Type = "ModWinsCard", SerialNumber = item, CallName = "ModWins" });
                }
                var res = CurrentListCardReader.RefreshListCard();
                CurrentListCardReader.StartGreenCardReader(CurrentListCardReader.ListCardInfo, onReadCard, null);


                lblMessage.Text = "Connected to " + res;

            }
            else
            {
                PrintError("Cannot connect to card reader");
            }
        }

        private void StopCard()
        {
            CurrentListCardReader.StartGreenCardReader(CurrentListCardReader.ListCardInfo, onReadCard, null);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////

        private void onReadCard(object sender, GreenCardReaderEventArgs e)
        {
            Invoke(new Action<string>(SetTextAsync), e.CardID);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////

        public async void SetTextAsync(string cardID)
        {
            bool found = false;
            cardID = cardID.ToLower();
            foreach (ParkingCard p in m_lstCards)
            {
                if (p.cardID == cardID)
                {
                    found = true;
                    if (p.scanned == false)
                    {
                        m_count++;
                        p.scanned = true;
                        PrintSuccess("Scan thành công " + p.cardNumber);
                    }
                    else
                    {
                        PrintError("Đã scan thẻ này");
                    }
                    
                    break;
                }
            }

            if(found == false)
            {
                PrintError("Không tìm thấy thẻ");
            }

            this.Text = "Kiểm kê thẻ " + m_count + " / " + m_lstCards.Count;
            grdData.DataSource = null;
            grdData.DataSource = m_lstCards;
            
        }

        ///////////////////////////////////////////////////////////////////////////////////////////

        void PrintError(string msg)
        {
            lblMessage.Text = msg;
            lblMessage.ForeColor = Color.Red;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////

        void PrintSuccess(string msg)
        {
            lblMessage.Text = msg;
            lblMessage.ForeColor = Color.Green;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        private async void BtnFinish_Click(object sender, EventArgs e)
        {
            string scannedList = "";
            string missingList = "";
            foreach (ParkingCard p in m_lstCards)
            {
                if(p.scanned)
                {
                    scannedList += p.cardNumber + "|";
                }
                else
                {
                    missingList += p.cardNumber + "|";
                }                
            }

            var values = new Dictionary<string, string>
            {
                {"scannedList", scannedList },
                {"missingList", missingList },
                { "token", Program.token }
            };

            var content = new FormUrlEncodedContent(values);
            HttpClient client = new HttpClient();
            var result = await client.PostAsync(Program.host + "api/parkingCard/check", content);

            string responseString = await result.Content.ReadAsStringAsync();
        }
    }
}
