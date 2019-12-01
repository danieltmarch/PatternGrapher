using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PatternGrapher
{
    public partial class Form1 : Form
    {
        Handler handler = null;
        int drawMode = 0; //0 is off, 1 is random new, 2 is iter, 9 to end
        public Form1()
        {
            handler = new Handler();
            InitializeComponent();
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab) //switches screen view (options to display)
            {
                if (picGraph.Visible) //Fullscreen to Normal
                {
                    FormBorderStyle = FormBorderStyle.Sizable;
                    WindowState = FormWindowState.Normal;
                    TopMost = false;

                    picGraph.Visible = false;

                    drawMode = 0;
                    timer.Enabled = false;
                }
                else //Normal to Fullscreen
                {
                    FormBorderStyle = FormBorderStyle.None;
                    WindowState = FormWindowState.Maximized;
                    TopMost = true;

                    picGraph.Left = (picGraph.Parent.Width / 2) - (picGraph.Width / 2);
                    picGraph.Top = (picGraph.Parent.Height / 2) - (picGraph.Height / 2);
                    picGraph.Visible = true;
                    timer.Enabled = true;

                }
            }
            if ((e.KeyCode == Keys.R) && (picGraph.Visible))//RunMode Random 1
            {
                drawMode = 1;
            }
            if ((e.KeyCode == Keys.I) && (picGraph.Visible))//RunMode Iter 2 | short timer
            {
                drawMode = 2;
                timer.Interval = 1;
            }
            if (e.KeyCode == Keys.Space) //turns off drawing
            {
                drawMode = 0;
                try { Clipboard.SetImage(picGraph.Image); } catch { }
            }
            if (e.KeyCode == Keys.C) //RunMode Climb 3 | long timer
            {
                drawMode = 3;
                timer.Interval = 1000;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (drawMode != 0) //Not on no draw mode
            {
                if (drawMode == 1) //Random 1
                {
                    picGraph.Image = handler.renderRandomIntFrame();
                    drawMode = 0;
                } else if(drawMode == 2)//Iter 2
                {
                    picGraph.Image = handler.renderIterFrame();
                } else
                {
                    picGraph.Image = handler.renderClimbFrame();
                }
                setVarText();
            }
        }

        private void setVarText()
        {
            varLabel0.Text = "a = " + handler.getVar(0);
            varLabel1.Text = "b = " + handler.getVar(1);
            varLabel2.Text = "c = " + handler.getVar(2);
            varLabel3.Text = "d = " + handler.getVar(3);
        }

        
    }

    public class Handler
    {
        Picture picture = null;
        Calculations calc = null;
        int maxCount = 10000;

        public Handler()
        {
            picture = new Picture();
            calc = new Calculations();
        }

        public Bitmap renderRandomFrame()
        {
            calc.resetIter(.02);
            calc.randomizeVars();
            calc.resetMults();
            picture.newImage();
            for (int runCount = 0; runCount <maxCount; runCount++)
            {
                picture.setPixelRGB( calc.getX(calc.getT(runCount, maxCount)) , calc.getY(calc.getT(runCount,maxCount)) , 255, 255, 255);
            }
            return picture.requestImage();
        }

        public Bitmap renderRandomIntFrame()
        {
            calc.iterSome(calc.getRandomN(), .02);
            calc.randIntVars();
            picture.newImage();
            for (int runCount = 0; runCount < maxCount; runCount++)
            {
                picture.setPixelRGB(calc.getX(calc.getT(runCount, maxCount)), calc.getY(calc.getT(runCount, maxCount)), 255, 255, 255);
            }
            return picture.requestImage();
        }

        public Bitmap renderIterFrame()
        {
            calc.varIter();
            picture.newImage();
            for (int runCount = 0; runCount < maxCount; runCount++)
            {
                picture.setPixelRGB(calc.getX(calc.getT(runCount, maxCount)), calc.getY(calc.getT(runCount, maxCount)), 255, 255, 255);
            }
            return picture.requestImage();
        }

        public Bitmap renderClimbFrame()
        {
            calc.climbVars(1);
            picture.newImage();
            for (int runCount = 0; runCount < maxCount; runCount++)
            {
                picture.setPixelRGB(calc.getX(calc.getT(runCount, maxCount)), calc.getY(calc.getT(runCount, maxCount)), 255, 255, 255);
            }
            return picture.requestImage();
        }

        public String getVar(int request) //request a b c or d
        {
            return ((Math.Round(calc.getVar(request)*10000))/10000).ToString();
        }
    }





    public class Picture
    {
        Bitmap pictureInfo = null;
        int height, width = 0;
        public Picture()
        {
            height = 1500; width = height;
            pictureInfo = new Bitmap(width, height);
        }
        public Bitmap requestImage()
        {
            return pictureInfo;
        }
        public void newImage()
        {
            pictureInfo = new Bitmap(width, height);
        }

        public Color getRGB(int r, int g, int b)
        {
            return Color.FromArgb(255, r, g, b);
        }
        public Color getHue(double degree)
        {
            int r, g, b = 0;
            if (degree <= 1 / 6.0) // R|b
            {
                r = 255; b = 0; degree = (degree * 6) - 0;
                g = (int)(255 * degree);
            }
            else if (degree <= 2 / 6.0)// |Gb
            {
                g = 255; b = 0; degree = (degree * 6) - 1;
                r = (int)(255 * degree);
            }
            else if (degree <= 3 / 6.0) // rG|
            {
                g = 255; r = 0; degree = (degree * 6) - 2;
                b = (int)(255 * degree);
            }
            else if (degree <= 4 / 6.0) // r|B
            {
                b = 255; r = 0; degree = (degree * 6) - 3;
                g = (int)(255 * degree);
            }
            else if (degree <= 5 / 6.0) // |gB
            {
                b = 255; g = 0; degree = (degree * 6) - 4;
                r = (int)(255 * degree);
            }
            else // Rg|
            {
                r = 255; g = 0; degree = (degree * 6) - 5;
                b = (int)(255 * degree);
            }
            return getRGB(r, g, b); //not a perfect hue, but it should work good enough
        }
        public void setPixelRGB(int x, int y, int r, int g, int b)
        {
            pictureInfo.SetPixel(x, y, getRGB(r, g, b));
        }
        public void setPixel(int x, int y, double degree)
        {
            pictureInfo.SetPixel(x, y, getHue(degree));
        }
        public void setPixelNxN(int x, int y, int size, double degree)
        {
            size = (size - 1) / 2;
            for (int i = -size; i <= size; i++)
            {
                for (int a = -size; a <= size; a++)
                {
                    pictureInfo.SetPixel(x + a, y + i, getHue(degree));
                }
            }
        }
        public void setPixelNxNrgb(int x, int y, int size, int r, int g, int b)
        {
            size = (size - 1) / 2;
            for (int i = -size; i <= size; i++)
            {
                for (int a = -size; a <= size; a++)
                {
                    pictureInfo.SetPixel(x + a, y + i, getRGB(r, g, b));
                }
            }
        }
        public void setPixelGray(int x, int y, int color)
        {
            pictureInfo.SetPixel(x, y, getRGB(color, color, color));
        }
    }


    public class Calculations
    {
        //t across equations: (0 to pi)
        //x = 500cos(at)*cos(bt)+500
        //y = 500sin(ct)*sin(dt)+500
        double a, b, c, d = 0; //variables for pattern gen
        double[] varChange = { 0, 0, 0, 0 }; //changes for iter mode
        int min = 0; // min value of variables
        int max = 15; // max value of variables
        int sclAdj = (1500/2)-1; //for math half image size
        int aMult = 1; int bMult = 1; int cMult = 1; int dMult = 1;

        Random random = null;
        
        public Calculations()
        {
            random = new Random();
        }

        public int getX(double t)
        {
            return (int)Math.Round((sclAdj * Math.Cos(a * t) * Math.Cos(b * t)) + sclAdj);
        }
        public int getY(double t)
        {
            return (int)Math.Round((sclAdj * Math.Sin(c * t) * Math.Sin(d * t)) + sclAdj);
        }

        public double getT(int current, int max)
        {
            return Math.PI*(current + .01) / max;
        }

        public void randomizeVars()
        {
            a = min + (max - min) * (random.NextDouble());
            b = min + (max - min) * (random.NextDouble());
            c = min + (max - min) * (random.NextDouble());
            d = min + (max - min) * (random.NextDouble());
        }
        public void randIntVars()
        {
            randomizeVars();
            a = Math.Round(a);
            b = Math.Round(b);
            c = Math.Round(c);
            d = Math.Round(d);
        }

        public void resetIter(double change) //copies vars and sets new var changes for each variable
        {
            for (int i = 0; i < 4; i++)
            {
                varChange[i] = change * 2 * (random.NextDouble() - .5);
            }
        }
        public void varIter() // changes random number of a b c d and moves them at different speeds
        {
            a = a + varChange[0];
            b = b + varChange[1];
            c = c + varChange[2];
            d = d + varChange[3];
            if (a + varChange[0] >= max || a + varChange[0] <= min) { varChange[0] = -1 * varChange[0]; }
            if (b + varChange[1] >= max || b + varChange[1] <= min) { varChange[1] = -1 * varChange[1]; }
            if (c + varChange[2] >= max || c + varChange[2] <= min) { varChange[2] = -1 * varChange[2]; }
            if (d + varChange[3] >= max || d + varChange[3] <= min) { varChange[3] = -1 * varChange[3]; }
        }

        public void iterSome(int n, double change) // changes random number of a b c d and moves them at different speeds
        {
            resetIter(change);
            for (int i = 0; i < 4-n; i++) //keeps n of the 4 vars the same, sets the rest to 0
            {
                varChange[i] = 0;
            }
            varChange = shuffleList(varChange);
        }

        public double[] shuffleList(double[] list)
        {
            var count = list.Count();
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = random.Next(i , count);
                var tmp = list[i];
                list[i] = list[r];
                list[r] = tmp;
            }
            return list;
        }

        public double getVar(int request) //request a b c or d
        {
            double[] list = {a,b,c,d};
            return list[request];
        }

        public int getRandomN() //Random number between 1 and 4
        {
            return random.Next(1,5);
        }

        public void setVars(double A, double B, double C, double D)
        {
            a = A;
            b = B;
            c = C;
            d = D;
        }

        public void climbVars(double change)
        {
            a = a + change*(aMult);
            if(a >= max || a<=min )
            {
                aMult = aMult * -1;
                b = b + change*(bMult);
                if(b >= max || b <= min)
                {
                    bMult = bMult * -1;
                    c = c + change*(cMult);
                    if(c >= max || c <= min)
                    {
                        cMult = cMult * -1;
                        d = d + change*(dMult);
                        if(d >= max || d <= min)
                        {
                            dMult = dMult * -1;
                        }
                    }
                }
            }
        }
        public void resetMults()
        {
            aMult = 1; bMult = 1; cMult = 1; dMult = 1;
        }

        public void modifyVars(int aC, int bC, int cC, int dC)
        {
            a = a + aC;
            b = b + bC;
            c = c + cC;
            d = d + dC;
        }
    }
}
