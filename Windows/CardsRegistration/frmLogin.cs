using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using System.Xml.Linq;

namespace CardsRegistration
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            var param = new Dictionary<string, string>
            {
                {"user_id", txtUsername.Text},
                {"password", txtPassword.Text} 
            };

            var content = new FormUrlEncodedContent(param);
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(Program.host + "api/user/login", content);

            var responseString = await response.Content.ReadAsStringAsync();

            User user = JsonConvert.DeserializeObject<User>(responseString);
            if(user.token == null)
            {
                Program.validToken = false;
                MessageBox.Show("Login failed" + user.Error);
            }
            else
            {
                Program.user = user.userID;
                Program.token = user.token;
                Program.level_id = user.level_id;
                Program.building_id = user.building_id;
                Program.validToken = true;

                TGMTcs.TGMTregistry.GetInstance().SaveRegValue("token", Program.token);

                this.Close();
            }
        }
    }
}
