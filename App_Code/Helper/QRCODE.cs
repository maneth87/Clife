using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;

public class QRCODE
{
    private string[] _data = new string[] {};
    private string _log_path = "";
    private byte[] _qr_image;


    public string[] DATA
    {
        get
        {
            return _data;
        }
        set
        {
            _data = value;
        }
    }
    public string LogoImagePath
    {
        get
        {
            return _log_path;
        }
        set
        {
            _log_path = value;
        }
    }
    public byte[] QRImage
    {
        get
        {
            return generateQRCode();
        }
    }

    public byte[] generateQRCode()
    {
        byte[] myQRimage;
        string qr_data = "";

        QRCodeEncoder qr = new QRCodeEncoder();

        qr.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
        qr.QRCodeScale = 10;
        System.Drawing.Bitmap img;

        foreach (string str in DATA)
        {
            qr_data += str + System.Environment.NewLine;
        }
        img = qr.Encode(qr_data);

        // Add logo on qr code
        if (LogoImagePath.Trim() != "")
        {
            System.Drawing.Image logo = System.Drawing.Image.FromFile(LogoImagePath);
            int left = 0;
            int top = 0;
            left = (img.Width / 2) - (logo.Width / 2);
            top = (img.Height / 2) - (logo.Width / 2);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(img);
            g.DrawImage(logo, new System.Drawing.Point(left, top));
        }
        var stream = new MemoryStream();
        img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        myQRimage = stream.ToArray();
        //img.Save(AppDomain.CurrentDomain.BaseDirectory + "/App_Themes/images/qr_logo1.png");
        

        return myQRimage;
    }
}
