using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using System.Diagnostics;
using System.Globalization;
using System.Runtime;

namespace tri
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
          
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

        byte[] plaintext; //textbox1 baitai
        byte[] encryptedtext; //encryptinto teksto baitai
       
        
  
        static public byte[] Encryption(byte[] Data, RSAParameters RSAKey, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKey);
                    encryptedData = RSA.Encrypt(Data, DoOAEPPadding);
                }
                return encryptedData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        static public byte[] Decryption(byte[] Data, RSAParameters RSAKey, bool DoOAEPPadding)
        {
            try
            {
                byte[] decryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKey);
                    decryptedData = RSA.Decrypt(Data, DoOAEPPadding);
                }
                return decryptedData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        private void button3_Click(object sender, EventArgs e) //teksto paemimas is failo ir perdavimas i textboxa ir iskart desifravimas
        {
            var nuskaitymas = string.Empty; 
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                string path;
                ofd.Filter = "txt files (*.txt)|*.txt";
                ofd.RestoreDirectory = true;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    path = ofd.FileName;
                    var data = ofd.OpenFile();
                    using (StreamReader sr = new StreamReader(data))
                    {
                        nuskaitymas = sr.ReadToEnd();
                    }
                }
            }
            try
            {
                Encoding encoding = Encoding.GetEncoding("437");
                byte[] decryptedtex = Decryption(encoding.GetBytes(nuskaitymas),
                RSA.ExportParameters(true), false);
                textBox3.Text = encoding.GetString(decryptedtex);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Console.WriteLine("Encryption failed.");
            }
        }

        private void button1_Click(object sender, EventArgs e) //encrypt mygtukas
        {
            string Path = null;
            button4.Visible = true;
            try
            {
                Encoding encoding = Encoding.GetEncoding("437");
                plaintext = encoding.GetBytes(textBox1.Text);
                encryptedtext = Encryption(plaintext, RSA.ExportParameters(false), false);
                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "txt files (*.txt)|*.txt";
                    sfd.RestoreDirectory = true;
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        Path = sfd.FileName;
                    }
                }
                using (StreamWriter writer = new StreamWriter(Path))
                {
                    writer.Write(encoding.GetString(encryptedtext));
                }
                textBox3.Text = encoding.GetString(encryptedtext);

            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Encryption failed.");
            }
        }

        private void button2_Click(object sender, EventArgs e) //decrypt mygtukas
        {

            try
            {
                Encoding encoding = Encoding.GetEncoding("437");
                byte[] decryptedtex = Decryption(encryptedtext, RSA.ExportParameters(true), false);
                textBox3.Text = encoding.GetString(decryptedtex);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Console.WriteLine("Encryption failed.");
            }
        }

        private void button4_Click(object sender, EventArgs e) //sulyginimas su decrypto tekstu nes copy pastina neteisingai rezultata jeigu ranka bandyt
        {
            textBox2.Text = textBox3.Text;
        }
    }
}
