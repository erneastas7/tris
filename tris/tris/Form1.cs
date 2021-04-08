using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
namespace tris
{
    public partial class Form1 : Form
    {
        //4.Now make some variables into the class that are:
        UnicodeEncoding ByteConverter = new UnicodeEncoding();
        RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
        byte[] plaintext;
        byte[] encryptedtext;
        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }


    //2.Now make a function for Encryption.
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
    //3.Now make a function for Decryption
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
   
    //5.Now handle the Click Event for the Encrypt Button with the following code:
    private void button1_Click(object sender, EventArgs e)
    {
        plaintext = ByteConverter.GetBytes(textBox1.Text);
        encryptedtext = Encryption(plaintext, RSA.ExportParameters(false), false);
        txtencrypt.Text = ByteConverter.GetString(encryptedtext);
    }
    //6.Now handle the Click Event for the Decrypt Button with the following code:
    private void button2_Click(object sender, EventArgs e)
    {
        byte[] decryptedtex = Decryption(encryptedtext,
        RSA.ExportParameters(true), false);
        txtdecrypt.Text = ByteConverter.GetString(decryptedtex);
    }
}