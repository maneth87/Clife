using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



public partial class barcode : System.Web.UI.Page
{
    private static readonly Random getrandom = new Random();
    private static readonly object syncLock = new object();
    
    //Function to get random number
    public static int GetRandomNumber(int min, int max)
    {
        lock(syncLock) { // synchronize
            return getrandom .Next(min, max);
        }
    }

    private void GenerateBarCode(string codeInfo, string card_number) //codeInfo is the info which you want to barcoded.
    {
        Bitmap barCode = new Bitmap(1, 1);
        Font code128 = new Font("IDAutomationHC39M", 900, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
       // Font code128 = new Font("Khmer OS Battambang", 900, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        Graphics graphics = Graphics.FromImage(barCode);
        SizeF dataSize = graphics.MeasureString("*" + codeInfo + "*", code128);

        Bitmap barCode2 = new Bitmap(barCode, dataSize.ToSize());
        graphics = Graphics.FromImage(barCode2);
        graphics.Clear(Color.White);
        graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;

        graphics.DrawString("*" + codeInfo + "*", code128, new SolidBrush(Color.Black), 0, 0);
        graphics.Flush();
        code128.Dispose();
        string path = "";

        switch (ddlProduct.SelectedValue)
        {
            case "T1011":
                path = Server.MapPath("~/Upload/Barcode/TermOne/" + card_number + ".jpeg");
                break;
            case "FT013":
                path = Server.MapPath("~/Upload/Barcode/FlexiTerm/" + card_number + ".jpeg");
                break;
        }
       
        barCode2.Save(path, ImageFormat.Jpeg);
        barCode2.Dispose();
        barCode.Dispose();
        graphics.Dispose();
        code128.Dispose();
                            
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        int quantity = Convert.ToInt32(txtQuantity.Text);
        int completed = 0;
        for (int i = 0; i <= quantity - 1; i++)
        {
            //Initial barcode number
            int barcode = GetRandomNumber(1, 99999999);
            string strBarcode = barcode.ToString();
            while (strBarcode.Length < 8)
            {
                strBarcode = "0" + strBarcode;
            }

            //Loop to find available barcode
            while(da_banc_card.CheckExistingBarcode(strBarcode)){
                barcode = GetRandomNumber(1, 99999999);   
                strBarcode = barcode.ToString();
                 while (strBarcode.Length < 8)
                {
                    strBarcode = "0" + strBarcode;
                }
            }


           // strBarcode = "ព្រះរាជាណាចក្រកម្ពុជា";

            //Save Card and barcode
            bl_banc_card term_one_card = new bl_banc_card();
            term_one_card.Status = 1;
            term_one_card.Card_ID = strBarcode;
            term_one_card.Product_ID = ddlProduct.SelectedValue;
            term_one_card.Sum_Insured = Convert.ToDouble(txtSumInsured.Text);
            term_one_card.Premium = Convert.ToDouble(txtPremium.Text);
            term_one_card.Created_By = "admin";
            term_one_card.Created_On = System.DateTime.Today;

            //Get last card number
            string last_card = da_banc_card.GetLastCardNumber(term_one_card.Product_ID);

            //Add + 1 to card number
            term_one_card.Card_Number = Convert.ToString(Convert.ToInt32(last_card) + 1);                      
          
            //Add 0 to the front
            string str_card_number = term_one_card.Card_Number.ToString();

            while (str_card_number.Length < 4)
            {
                str_card_number = "0" + str_card_number;
            }

            term_one_card.Card_Number = str_card_number;

            //Barcode Image Server Path
            switch (ddlProduct.SelectedValue)
           {
            case "T1011":
                   term_one_card.Url = "~/Upload/Barcode/TermOne/" + term_one_card.Card_Number + ".jpeg";
                   break;
            case "FT013":
                   term_one_card.Url = "~/Upload/Barcode/FlexiTerm/" + term_one_card.Card_Number + ".jpeg";
                   break;
           }
            
            //If failed to insert stop creating 
            if (!da_banc_card.InsertBancCard(term_one_card))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Saving Barcode to card Failed. Barcode: '" + term_one_card.Card_ID + ", Card Number: " + term_one_card.Card_Number + ")", true);
            }
            else
            {                

                GenerateBarCode(strBarcode, str_card_number);
                completed += 1;
            }
            
          
        }

        if (completed == quantity)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Generate Barcode Completed.')", true);
        }
    }
}