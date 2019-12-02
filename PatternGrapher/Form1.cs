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


        //occurs whenever a key press occurs, lets us switch from the various modes.
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab) //tab switches screen view (options to display)
            {
                if (WindowState == FormWindowState.Maximized) //Fullscreen to Normal
                {
                    FormBorderStyle = FormBorderStyle.Sizable;
                    WindowState = FormWindowState.Normal;
                    TopMost = false; //topmost makes this window the priority.
                }
                else //Normal to Fullscreen
                {
                    //switch the window back to window mode
                    FormBorderStyle = FormBorderStyle.None;
                    WindowState = FormWindowState.Maximized;
                    TopMost = true;
                }
            }
            if ((e.KeyCode == Keys.R) && (picGraph.Visible))//r begins RunMode Random, mode 1
            {
                drawMode = 1;
            }
            if ((e.KeyCode == Keys.I) && (picGraph.Visible))//i begins RunMode Iter, mode 2 | short timer
            {
                drawMode = 2;
                timer.Interval = 10; //100 fps, 10ms equals 100 pictures drawn per sec.
            }
            if (e.KeyCode == Keys.Space) //space turns off drawing, effectively a pause.
            {
                drawMode = 0;
                try { Clipboard.SetImage(picGraph.Image); } catch { } //copy the image to the clipboard when we pause, but catch crashes if the image is null.
            }
            if (e.KeyCode == Keys.C) //RunMode Climb 3 | long timer
            {
                drawMode = 3;
                timer.Interval = 1000; //one image per second.
            }
        }

        //depending on the draw mode render the image.
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
                }
                else //Climb 3
                {
                    picGraph.Image = handler.renderClimbFrame();
                }
                setVarText(); //update what a,b,c,d are equal to, helpful incase the user wants to know what the equation is.
            }
        }

        //updates what a, b, c, d are equal to, helpful incase the user wants to know what the equation of the picture is
        private void setVarText()
        {
            varLabel0.Text = "a = " + handler.getVar(0);
            varLabel1.Text = "b = " + handler.getVar(1);
            varLabel2.Text = "c = " + handler.getVar(2);
            varLabel3.Text = "d = " + handler.getVar(3);
        }

    }

    //this class handles the image processing / form app inbetween work.
    public class Handler
    {
        Picture picture = null;
        Calculations calc = null;
        int maxCount = 8000; //the resolution of the image, the higher the resolution, the slower the image will be made, rec. between 5000 and 15000

        public Handler()
        {
            picture = new Picture(1200, 1200);
            calc = new Calculations(0, 16, 1200, 1200);
        }

        //create the random frame (happens when the user hits r).
        public Bitmap renderRandomIntFrame()
        {
            calc.iterSome(.02);
            calc.randIntVars();
            picture.newImage();
            for (int runCount = 0; runCount < maxCount; runCount++)
            {
                picture.setPixelRGB(calc.getX(calc.getT(runCount, maxCount)), calc.getY(calc.getT(runCount, maxCount)), 255, 255, 255);
            }
            return picture.requestImage();
        }
        //move some/all of the variables a bit, and render a new image. (happens when the user hits i, and then will continually happen).
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

        //count the variables up 1 (treating a,b,c,d as a single number), and render a new image (happnens when the user hits c, and then continually happens)
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

        //get either a,b,c or d.
        public String getVar(int request) //request a b c or d
        {
            return ((Math.Round(calc.getVar(request)*10000))/10000).ToString(); //return the variable rounding to the 4th decimal place
        }
    }

    //handles the image creation.
    public class Picture
    {
        Bitmap pictureInfo = null;
        int height, width;
        public Picture(int Height, int Width)
        {
            height = Height; width = Width;
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

        //get a color type from 3 seperate variables (r,g,b)
        public Color getRGB(int r, int g, int b)
        {
            return Color.FromArgb(255, r, g, b);
        }

        //set pixel based on coordinate and rgb value
        public void setPixelRGB(int x, int y, int r, int g, int b)
        {
            pictureInfo.SetPixel(x, y, getRGB(r, g, b));
        }
    }

    //handles the math of the pattern generator
    public class Calculations
    {
        //t across equations: (0 to pi)
        //x = 500cos(at)*cos(bt)+500 // when width = 1000.
        //y = 500sin(ct)*sin(dt)+500 // when height = 1000.

        double a, b, c, d   = 0; //variables for pattern gen
        double[] varChange  = { 0, 0, 0, 0 }; //changes for iter mode, think of this like the rate of change of a,b,c,d
        int min = 0; // min value of variables, for a,b,c,d
        int max = 15; // max value of variables, for a,b,c,d
        int width, height; //for math half image size, - 1
        int aMult = 1; int bMult = 1; int cMult = 1; int dMult = 1; //used for climbing variables, allows the counter to count down as well as up

        Random random = null;
        
        public Calculations(int Min, int Max, int Width, int Height)
        {
            random = new Random();
            min = Min;
            max = Max;
            width = Width-1; //-1 because image goes from 0 to Width - 1.
            height = Height-1; //-1 because image goes from 0 to Height - 1.
        }

        //get the x coordinate based on t
        public int getX(double t)
        {
            return (int)( (width*(Math.Cos(a*t)*Math.Cos(b*t) + 1.0))/2 );
        }
        //get the y coordinate based on t
        public int getY(double t)
        {
            return (int)( (height * (Math.Sin(c * t) * Math.Sin(d * t) + 1.0)) / 2 );
        }

        //get t based on which number point we are placing out of how many total points we need to place
        public double getT(int current, int max)
        {
            return (Math.PI*current) / (max);
        }

        //randomize a,b,c,d (by int), +1 because random.Next(min, max) will only make numbers 1 less than max.
        public void randIntVars()
        {
            a = random.Next(min, max+1);
            b = random.Next(min, max+1);
            c = random.Next(min, max+1);
            d = random.Next(min, max+1);
        }

        //randomizes the rate of change of each variable, change is the speed the vars can move at (like volatility)
        public void resetIter(double change)
        {
            for (int i = 0; i < 4; i++)
            {
                varChange[i] = change * 2 * (random.NextDouble() - .5);
            }
        }
        public void varIter() // changes random number of a b c d and moves them at different speeds
        {
            //iterate each variable according to its rate of change variable
            a = a + varChange[0];
            b = b + varChange[1];
            c = c + varChange[2];
            d = d + varChange[3];

            //if the rate of change has pushed a,b,c,d over/under its bounds, then invert the rate of change var s it reverses direction
            if (a + varChange[0] >= max || a + varChange[0] <= min) { varChange[0] = -1 * varChange[0]; }
            if (b + varChange[1] >= max || b + varChange[1] <= min) { varChange[1] = -1 * varChange[1]; }
            if (c + varChange[2] >= max || c + varChange[2] <= min) { varChange[2] = -1 * varChange[2]; }
            if (d + varChange[3] >= max || d + varChange[3] <= min) { varChange[3] = -1 * varChange[3]; }
        }

        // changes random number of a b c d and moves them at different speeds, change is te speed te vars can move at (like volatility)
        public void iterSome(double change)
        {
            resetIter(change); //randomize the rate of change list
            for (int i = 0; i < random.Next(0,4); i++) //keeps either 0,1,2,3 vars the same, sets the rest to 0
            {
                varChange[i] = 0;
            }
            varChange = shuffleList(varChange); //to avoid always having the zeroed variables athe beginning of the list, we shuffle it.
        }

        //simply shuffles a list
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

        //returns either a,b,c or d.
        public double getVar(int request) //request a b c or d
        {
            double[] list = {a,b,c,d};
            return list[request];
        }
        //increase the vars by the change (normally 1) starts with a, if a goes over/under its bounds, move on to b, and so on.
        public void climbVars(double change)
        {
            a = a + change*(aMult);
            if(a > max || a<min )
            {
                a = a - change * (aMult);
                aMult = aMult * -1;
                b = b + change*(bMult);
                if(b > max || b < min)
                {
                    b = b - change * (bMult);
                    bMult = bMult * -1;
                    c = c + change*(cMult);
                    if(c > max || c < min)
                    {
                        c = c - change * (cMult);
                        cMult = cMult * -1;
                        d = d + change*(dMult);
                        if(d > max || d < min)
                        {
                            d = d - change * (dMult);
                            dMult = dMult * -1;
                        }
                    }
                }
            }
        }

        //set a,b,c,d by custom value.
        public void setVars(int A, int B, int C, int D)
        {
            a = A; b = B; c = C; d = D;
        }

        //set a,b,c,d rate of change by custom value.
        public void setVars(double dA, double dB, double dC, double dD)
        {
            varChange[0] = dA;
            varChange[1] = dB;
            varChange[2] = dC;
            varChange[3] = dD;
        }
    }
}
