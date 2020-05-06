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
using TGMTcs;

namespace CardsRegistration
{
    public partial class frmSelect : Form
    {
        public frmSelect()
        {
            InitializeComponent();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmAdd frm = new frmAdd();
            frm.Closed += (s, args) => this.Close();
            frm.Show();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        private void BtnCheck_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmCheck frm = new frmCheck();
            frm.Closed += (s, args) => this.Close();
            frm.Show();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////

        async void CheckToken()
        {
            if (Program.token == "")
            {
                frmLogin frm = new frmLogin();
                frm.FormClosed += OnFrmLoginClosed; 
                frm.ShowDialog();
            }
            else
            {
                var values = new Dictionary<string, string>
                {
                    {"token", Program.token}
                };

                try
                {
                    var content = new FormUrlEncodedContent(values);
                    HttpClient client = new HttpClient();
                    var result = await client.PostAsync(Program.host + "api/user/verifyToken", content);

                    var responseString = await result.Content.ReadAsStringAsync();

                    LoginHistory loginHistory = JsonConvert.DeserializeObject<LoginHistory>(responseString);

                    if (loginHistory.token == Program.token)
                    {
                        Program.validToken = true;
                        Program.user = loginHistory.user_id;
                        Program.building_id = loginHistory.building_id;
                        Program.level_id = loginHistory.level_id;

                        if(Program.level_id == "Admin" ||
                            Program.level_id == "Ace" ||
                            Program.level_id == "ShiftAce")
                        {
                            btnAdd.Enabled = true;
                        }
                        
                        btnCheck.Enabled = true;

                        lblUser.Text = "User: " + Program.user;
                        lblBuilding.Text = "Bãi xe: " + Program.building_id;
                    }
                    else
                    {
                        if (!Program.validToken)
                        {
                            frmLogin frm = new frmLogin();
                            frm.FormClosed += OnFrmLoginClosed;
                            frm.ShowDialog();
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }                
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        private void FrmSelect_Load(object sender, EventArgs e)
        {
            lblUser.Text = "User: " + Program.user;
            lblBuilding.Text = "Bãi xe: " + Program.building_id;

            TGMTregistry.GetInstance().Init("CardRegister");
            Program.token = TGMTregistry.GetInstance().ReadRegValueString("token");
            CheckToken();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        private void BtnLogout_Click(object sender, EventArgs e)
        {
            TGMTregistry.GetInstance().SaveRegValue("token", "");

            Program.token = "";
            Program.user = "";
            Program.building_id = "";
            Program.validToken = false;

            frmLogin frm = new frmLogin();
            frm.FormClosed += OnFrmLoginClosed;
            frm.ShowDialog();

            lblUser.Text = Program.user;
            lblBuilding.Text = Program.building_id;
        }

        private void OnFrmLoginClosed(object sender, FormClosedEventArgs e)
        {
            if(Program.validToken)
            {
                lblUser.Text = "User: " + Program.user;
                lblBuilding.Text = "Bãi xe: " + Program.building_id;

                if (Program.level_id == "Admin" ||
                            Program.level_id == "Ace" ||
                            Program.level_id == "ShiftAce")
                {
                    btnAdd.Enabled = true;
                }

                btnCheck.Enabled = true;
            }
            else
            {
                btnAdd.Enabled = false;
                btnCheck.Enabled = false;
            }
        }
    }
}
