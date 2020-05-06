using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CardReaderService;
using System.Threading;
using System.IO;
using Microsoft.VisualBasic;
using TGMTcs;
using Newtonsoft.Json;
using System.Net.Http;

namespace CardsRegistration
{

    public partial class frmAdd : Form
    {

        private List<DataCards> m_lstCards;

        
        ///////////////////////////////////////////////////////////////////////////////////////////
        
        public frmAdd()
        {
            InitializeComponent();          
            
            m_lstCards = new List<DataCards>();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////

        private void Form1_Load(object sender, EventArgs e)
        {            
            lblUser.Text = "User: " + Program.user;
            lblBuilding.Text = "Bãi xe: " + Program.building_id;

            SetCardReader();
            LoadCard();
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

        ///////////////////////////////////////////////////////////////////////////////////////////

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
            lblMessage.Text = "";
            if (!Program.validToken)
            {
                MessageBox.Show("Chưa đăng nhập");
                return;
            }
            cardID = cardID.ToLower();
            var c = m_lstCards.FirstOrDefault(cc => cc.CardID == cardID);

            string cardNumber = Interaction.InputBox("Nhập số in trên thẻ", "Nhập số thẻ");
            ParkingCard card = new ParkingCard(cardID, cardNumber);          



            FormUrlEncodedContent content = new FormUrlEncodedContent(card.ToDictionary());            

            HttpClient client = new HttpClient();
            var result = await client.PostAsync(Program.host + "api/parkingCard/add", content);
            var responseString = await result.Content.ReadAsStringAsync();
            LoginHistory response = JsonConvert.DeserializeObject<LoginHistory>(responseString);

            if(response.Error != null && response.Error != "")
            {
                PrintError(response.Error + ": " + cardNumber);
                return;
            }
            else if(response.Success != null && response.Success != "")
            {
                PrintSuccess(response.Success + ": " + cardNumber);
            }


            if (c==null)
            {
                if (m_lstCards.Count == 0)
                {
                    m_lstCards.Add(new DataCards()
                    {
                        CardID = cardID,
                        CardNumber = cardNumber,
                        CratedDate = DateTime.Now,
                        VehicleType = ""
                    });
                }                    
                else
                {
                    m_lstCards.Insert(0, new DataCards()
                    {
                        CardID = cardID,
                        CardNumber = cardNumber,
                        CratedDate = DateTime.Now,
                        VehicleType = ""
                    });
                }
                    
                grdData.DataSource = null;
                grdData.DataSource = m_lstCards;
            }
            else
            {
                grdData.DataSource = null;
                grdData.DataSource = m_lstCards;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            SetCardReader();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////

        private void btnReset_Click(object sender, EventArgs e)
        {
            m_lstCards.Clear();
            grdData.DataSource = null;
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
                m_lstCards.Add(new DataCards()
                {
                    CardID = parkingCard.cardID,
                    CardNumber = parkingCard.cardNumber,
                    CratedDate = DateTime.Now,
                    VehicleType = ""
                });
            }
            grdData.DataSource = null;
            grdData.DataSource = m_lstCards;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////

        private async void btnUploadCard_ClickAsync(object sender, EventArgs e)
        {
            string[] lines = File.ReadAllLines(@"D:\SGCP1.csv");

            for(int i=0; i<lines.Length; i++)
            {
                string[] splitted = lines[i].Split(',');
                string cardNumber = splitted[1];
                string cardID = splitted[2];
                string status = splitted[3];

                ParkingCard card = new ParkingCard(cardID, cardNumber, status);


                FormUrlEncodedContent content = new FormUrlEncodedContent(card.ToDictionary());
                HttpClient client = new HttpClient();
                var result = await client.PostAsync(Program.host + "api/parkingCard/add", content);
                var responseString = await result.Content.ReadAsStringAsync();
                LoginHistory response = JsonConvert.DeserializeObject<LoginHistory>(responseString);

                if (response.Error != null && response.Error != "")
                {
                    PrintError(response.Error + ": " + cardNumber);
                    return;
                }
                else if (response.Success != null && response.Success != "")
                {
                    PrintSuccess(response.Success + ": " + cardNumber);
                }
            }
        }
    }

}
