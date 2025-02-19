﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KasaProgramı
{
    public partial class Login : Form
    {
        private readonly UserService _userService;
        private readonly MyDbContext _myDbContext;
        public Login()
        {
            InitializeComponent();
            _userService = new UserService(new MyDbContext());
            _myDbContext = new MyDbContext();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                // TextBox'un şifre karakterini kapat
                txt_sifre.PasswordChar = '\0';
                txt_sifre.UseSystemPasswordChar = false;
            }
            else
            {
                // CheckBox işaretli değilse, TextBox'u şifre karakteri ile gizle
                txt_sifre.PasswordChar = '*';
                txt_sifre.UseSystemPasswordChar = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_giris_Click(object sender, EventArgs e)
        {


            string username = txt_kullaniciadi.Text;
            string password = txt_sifre.Text;



            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("TÜM ALANLARI DOLDURUNUZ !", "HATA");
            }
            else
            {
                var result = _userService.AuthenticateUser(username, password);
                if (result)
                {
                    var user = _myDbContext.Users.FirstOrDefault(x => x.UserName == username);

                    if (user.Authority == "admin")
                    {
                        anasayfa anasayfa = new anasayfa();
                        anasayfa.Show();
                        this.Hide();
                    }
                    else
                    {
                        anasayfa anasayfa = new anasayfa();
                        anasayfa.btn_paragirisisayfa.Enabled = false;
                        anasayfa.btn_paracikisisayfa.Enabled = false;
                        anasayfa.btn_borcGir.Enabled = false;
                        anasayfa.btn_borcDus.Enabled = false;
                        anasayfa.btn_islemDuzenle.Enabled = false;
                        anasayfa.ayarlarMenuItem.Visible = false;

                        anasayfa.Show();
                        this.Hide();
                    }


                }
                else
                {
                    MessageBox.Show("Kullanıcı adı veya şifre hatalı!", "HATA");
                }

            }
        }

        private void lbl_sifreunuttum_Click(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {
            bool veriVarMi = _myDbContext.Users.Any();
            if (veriVarMi)
            {
                lbl_kayit.Visible = false;
            }
        }

        private void txt_sifre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && txt_sifre.Focused)
            {
                // Giriş butonunun tıklama işlemi burada gerçekleşiyor.
                btn_giris.PerformClick();
            }
        }

        private void lbl_kayit_Click(object sender, EventArgs e)
        {
            Kayit kayit = new Kayit(_myDbContext);
            kayit.Show();
            this.Hide();
        }
    }
}
