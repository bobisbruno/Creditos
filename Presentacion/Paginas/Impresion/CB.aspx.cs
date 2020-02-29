using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using GenCode128;
using System.IO;
using log4net;


public partial class CB : System.Web.UI.Page
{
    ILog log = LogManager.GetLogger(typeof(CB).Name);
    
    protected void Page_Load(object sender, EventArgs e)
    {
          if(Request["a"]!=null)
           {
            string texto = Request["a"];

            Bitmap objBMP = new System.Drawing.Bitmap(250, 50);
            Graphics objGraphics = Graphics.FromImage(objBMP);
            objGraphics.Clear(Color.White);

            objGraphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                            
            //'Configure font to use for text
            Font objFont = new Font("BARCODE 128", 26);

            string randomStr = Barcode128A(texto);
            int[] myIntArray = new int[randomStr.Length];
            int x;

            ////That is to create the random # and add it to our string
            //Random autoRand = new Random();

            //for (x=0;x<5;x++)
            //{
            //    myIntArray[x] =  System.Convert.ToInt32 (autoRand.Next(0,9));
            //    randomStr+= (myIntArray[x].ToString ());
            //}

            ////This is to add the string to session cookie, to be compared later
            //Session.Add("randomStr",randomStr);

            //' Write out the text
            objGraphics.DrawString(randomStr, objFont, Brushes.Black, 1, 1);

            //' Set the content type and return the image
            Response.ContentType = "image/gif";
            objBMP.Save(Response.OutputStream, ImageFormat.Gif);

            objFont.Dispose();
            objGraphics.Dispose();
            objBMP.Dispose();
        }else if((Request["NroExp"] != null))
             {
               string texto = Request["NroExp"];
               try
                 {
                     System.Drawing.Image myimg = Code128Rendering.MakeBarcodeImage(texto, 1, true);
                     //convert Image to Memory Stream
                     MemoryStream stream = new MemoryStream();

                     myimg.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);

                     //convert it to response context for display.
                     Context.Response.BinaryWrite(stream.GetBuffer());
                 }
                 catch(Exception ex)
                      {
                        String myLogs = " Error al generar el codigo de Barra de, Nro de Expediente :  " +texto;  
                        log.Error(string.Format("{0} - Error:{1}->{2}->{3}", System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message,myLogs));
                      }
           }           
   }


    #region Barcode128C
    /*solo carateres numéricos*/
    public string Barcode128C(string cadena)
    {
        char lcStart = Caracter(137);//char lcStart='‰';
        char lcStop = Caracter(138);//char lcStop='Š';

        int lnCheckSum = 105;
        string lcRet = cadena.Trim();
        int lnLong = lcRet.Length;
        //La longitud debe ses par
        if (Math.IEEERemainder(lnLong, 2) != 0.0)
        {
            lcRet = "0" + lcRet;
            lnLong = lcRet.Length;
        }
        //Convierto los caracteres pares
        string lcCar = "";
        for (int ini = 0; ini < lnLong; ini += 2)
        {
            string aux = lcRet.Substring(ini, 2);
            int numaux = int.Parse(lcRet.Substring(ini, 2)) + 32;
            char charaux = Caracter(numaux);
            lcCar = lcCar + charaux.ToString();
        }
        lcRet = lcCar;
        lnLong = lcRet.Length;

        for (int lnI = 0; lnI < lnLong; lnI++)
        {
            string aux = lcRet.Substring(lnI, 1);
            int lnAsc = NumAcs(aux) - 32;
            lnCheckSum = lnCheckSum + (lnAsc * (lnI + 1));
        }
        int result = Modulo(lnCheckSum, 103) + 32;
        char lcCheck = Caracter(result);

        lcRet = lcStart + lcRet + lcCheck + lcStop;

        //Cambio los espacios y caracteres invalidos
        lcRet = lcRet.Replace(Caracter(32), Caracter(232));
        lcRet = lcRet.Replace(Caracter(127), Caracter(192));
        lcRet = lcRet.Replace(Caracter(128), Caracter(193));
        return lcRet;
    }
    #endregion

    #region Barcode128A
    /*caracteres numéricos y alfabéticos (mayusculas)*/
    public string Barcode128A(string cadena)
    {
        char lcStart = Caracter(135);//char lcStart='#';
        char lcStop = Caracter(138);//char lcStop='Š';

        int lnCheckSum = 103;
        string lcRet = cadena.Trim();
        int lnLong = lcRet.Length;

        for (int lnI = 0; lnI < lnLong; lnI++)
        {
            string aux = lcRet.Substring(lnI, 1);
            int lnAsc = NumAcs(aux) - 32;
            if (lnAsc < 0 || lnAsc > 64)
            {
                char tmp = char.Parse(lcRet.Substring(lnI, 1));
                lcRet.Replace(tmp, Caracter(32));

                string aux2 = lcRet.Substring(lnI, 1);
                lnAsc = NumAcs(aux2) - 32;

            }
            lnCheckSum = lnCheckSum + (lnAsc * (lnI + 1));
        }
        int result = Modulo(lnCheckSum, 103) + 32;
        char lcCheck = Caracter(result);

        lcRet = lcStart + lcRet + lcCheck + lcStop;

        //Cambio los espacios y caracteres invalidos
        lcRet = lcRet.Replace(Caracter(32), Caracter(232));
        lcRet = lcRet.Replace(Caracter(127), Caracter(192));
        lcRet = lcRet.Replace(Caracter(128), Caracter(193));

        return lcRet;
    }
    #endregion

    #region Barcode128B

    /*caracteres numéricos y alfabéticos (mayusculas)*/
    public string Barcode128B(string cadena)
    {
        char lcStart = Caracter(136);
        char lcStop = Caracter(138);

        int lnCheckSum = 104;
        string lcRet = cadena.Trim();
        int lnLong = lcRet.Length;

        for (int lnI = 0; lnI < lnLong; lnI++)
        {
            string aux = lcRet.Substring(lnI, 1);
            int lnAsc = NumAcs(aux) - 32;
           
            if (lnAsc < 0 || lnAsc > 99)
            {
                char tmp = char.Parse(lcRet.Substring(lnI, 1));
                lcRet.Replace(tmp, Caracter(32));

                string aux2 = lcRet.Substring(lnI, 1);
                lnAsc = NumAcs(aux2) - 32;

            }
            lnCheckSum = lnCheckSum + (lnAsc * (lnI + 1));
        }
        int result = Modulo(lnCheckSum, 103) + 32;
        char lcCheck = Caracter(result);

        lcRet = lcStart + lcRet + lcCheck + lcStop;

        //Cambio los espacios y caracteres invalidos
        lcRet = lcRet.Replace(Caracter(32), Caracter(232));
        lcRet = lcRet.Replace(Caracter(127), Caracter(192));
        lcRet = lcRet.Replace(Caracter(128), Caracter(193));

        return lcRet;
    }

    #endregion

    #region  Operaciones 

    public char Caracter(int numero)
    {
        Encoding targetEncoding;

        targetEncoding = Encoding.GetEncoding(1252); //windows

        byte[] numeritos = new byte[1];
        numeritos[0] = byte.Parse(numero.ToString());
        string caracter = ((char)targetEncoding.GetChars(numeritos)[0]).ToString();
        return char.Parse(caracter.ToString());
    }

    public int NumAcs(string Caracter)
    {
        Encoding targetEncoding;
        targetEncoding = Encoding.GetEncoding(1252); //windows
        byte[] array = targetEncoding.GetBytes(Caracter);
        return int.Parse(array[0].ToString());

    }
    private int Modulo(int num, int mod)
    {
        int aux;
        if (mod != 0)
        {
            aux = num / mod;
            aux = aux * mod;
            aux = num - aux;
        }
        else
        {
            aux = 0;
        }
        return aux;
    }
    #endregion

}