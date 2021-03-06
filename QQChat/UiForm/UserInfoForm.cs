﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Util;
using Bll;
using Model;
using SqlDal;
using System.IO;


namespace QQChat.UiForm
{
    public partial class UserInfoForm : Form
    {
        private SessionBll session;
        private User user;
        UserDal userdal = new UserDal();
        public UserInfoForm()
        {
            InitializeComponent();
            session = SessionBll.GetInstance();
            user = session.User;
            UserName_label.Text = Convert.ToString(user.Username);
            
            if (user.Sex == -1)
               label_Sex.Text="男";
            else
               label_Sex.Text = "女";
          
            if (user.Age == -1)
            {
                label_Age.Text = "0";
            }
            else
                label_Age.Text = Convert.ToString(user.Age);

            label_Uid.Text = Convert.ToString(user.Email);
            label_NickName.Text = Convert.ToString(user.Username) ;
            label_Email.Text = Convert.ToString(user.Email);
            Sign_textBox.Text = Convert.ToString(user.Sign);
            Sign_textBox.ReadOnly = true;
            
        }

        private void EditInfo_Click(object sender, EventArgs e)
        {
            Sign_textBox.Enabled = true;
            Sign_textBox.Focus();//获取焦点
            UserInfopanel.Hide();
            Sign_textBox.Enabled = true;
            Sign_textBox.ReadOnly =false;
            if (user.Sex == -1)
               SexChoice_comboBox.Text = "男";
            else
               SexChoice_comboBox.Text = "女";

            if (user.Age == -1)
            {
                Age_textBox.Text = "0";
            }
            else
                Age_textBox.Text = Convert.ToString(user.Age);
            label2_Uid.Text = user.Email;
            NickName_textBox.Text = user.Username;
            Email_textBox.Text = user.Email;
            Sign_textBox.Text = user.Sign;    
        }

        private void Save_button_Click(object sender, EventArgs e)
        {

            
            string signUpdate = Sign_textBox.Text;
            user.Sign = signUpdate;

            string nicknameUpdate = NickName_textBox.Text;
            user.Username = nicknameUpdate;

            string sexUpdate = SexChoice_comboBox.Text;
            if (sexUpdate == "男")
                user.Sex = -1;
            else
                user.Sex = 0;
            try
            {
                int ageUpdate = Int32.Parse(Age_textBox.Text);
                user.Age = ageUpdate;
            }
            catch
            {
                MessageBox.Show("输入有误!");
                Age_textBox.Focus();
                Age_textBox.ForeColor = System.Drawing.Color.Red;
                return;
            }

            
            string emailUpdate = Email_textBox.Text;
            user.Email = emailUpdate;
            userdal.update(user,user.UId);
            label_Uid.Text = Convert.ToString(user.Email);
            label_NickName.Text = Convert.ToString(user.Username);
            label_Email.Text = Convert.ToString(user.Email);
            Sign_textBox.Text = Convert.ToString(user.Sign);
            UserInfopanel.Show();
            Sign_textBox.ReadOnly = true;
        }

        private void backUpdate_Click(object sender, EventArgs e)
        { 
            Panel EditInfo_Panel = new Panel();
            EditInfo_Panel.Hide();
            UserInfopanel.Show();
            
        }

        private void UserInfoForm_Load(object sender, EventArgs e)
        {
            //加载页面时候添加头像
            if (user.Photo == null)
            {
                Image errorIm = Image.FromFile("Head/error.jpg");
                Size size = new Size(92, 108);
                pictureBox1.Image = new Bitmap(errorIm, size);

            }
            else
            {
                MemoryStream stream = new MemoryStream(user.Photo);
                Size size = new Size(92, 108);
                Bitmap im=new Bitmap(stream);
                pictureBox1.Image = new Bitmap(im, size);
            }
            //加载页面时候签名不可编辑
            Sign_textBox.Enabled = false;
        }

        private void Sign_textBox_TextChanged(object sender, EventArgs e)
        {
            //Sign_textBox.Enabled = true;
            Sign_textBox.Focus();//获取焦点
        }

        //上传头像
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            UpdatePhoto();           
        }
        #region 更新头像
        private void UpdatePhoto()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "jpg图像(*.jpg)|*.jpg|png图像(*.png)|*.png|gif(*.gif)|*.gif";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                 string file = fileDialog.FileName;
                 try
                 {
                     //用上面这个方法设置textfield
                     FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                     int length = (int)fs.Length;
                     Byte[] photos = new Byte[length];
                     fs.Read(photos, 0, photos.Length);
                     user.Photo = photos;
                     fs.Close();
                     //上传到数据库
                     if (userdal.update(user, user.UId))
                     {
                         MessageBox.Show("更改成功！");
                         UserInfopanel.Show();
                     }
                     else
                     {
                         MessageBox.Show("更改失败1111！");
                     }
                 }
                 catch {
                     MessageBox.Show("更改失败222！");
                 }
               

            }
            else {
                return;
            } 
          
        }
        #endregion 
    }
}
