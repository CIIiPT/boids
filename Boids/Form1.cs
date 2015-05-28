using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


namespace Boids
{
    public partial class Form1 : Form
    {
        private Timer timer;
        private Swarm swarm;
        private Swarm swarm1;
        private Image iconRegular;
        private Image iconZombie;
        public Form1(int count, int count1)
        {
            Form2 form = new Form2();
            
           
            int boundary = 1024;//width
            int boundary1 = 768;//height
            
            int countboids = count;
            int countboids1 = count1;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(boundary, boundary1);
            iconRegular = CreateIcon(Brushes.Blue);
            iconZombie = CreateIcon(Brushes.Red);
            swarm = new Swarm(boundary, boundary1, false,countboids);
            swarm1 = new Swarm(boundary, boundary1, true, countboids1);
            timer = new Timer();
            timer.Tick += new EventHandler(this.timer_Tick);
            timer.Interval = 75;
            timer.Start();
            
            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            DrawString(Convert.ToString(swarm.Boids.Count), Convert.ToString(swarm1.Boids.Count), 20, 20);
            foreach (Boid boid in swarm.Boids)
            {
                float angle;
                if (boid.dX == 0) angle = 90f;
                else angle = (float)(Math.Atan(boid.dY / boid.dX) * 57.3);
                if (boid.dX < 0f) angle += 180f;
                Matrix matrix = new Matrix();
                matrix.RotateAt(angle, boid.Position);
                e.Graphics.Transform = matrix;
                if (boid.Zombie) e.Graphics.DrawImage(iconZombie, boid.Position);
                else e.Graphics.DrawImage(iconRegular, boid.Position);
                DrawString(Convert.ToString(swarm.Boids.Count), Convert.ToString(swarm1.Boids.Count), 20, 20);
            }
            DrawString(Convert.ToString(swarm.Boids.Count), Convert.ToString(swarm1.Boids.Count), 20, 20);
            foreach (Boid boid in swarm1.Boids)
            {

                float angle;
                if (boid.dX == 0) angle = 90f;
                else angle = (float)(Math.Atan(boid.dY / boid.dX) * 57.3);
                if (boid.dX < 0f) angle += 180f;
                Matrix matrix = new Matrix();
                matrix.RotateAt(angle, boid.Position);
                e.Graphics.Transform = matrix;
                if (boid.Zombie) e.Graphics.DrawImage(iconZombie, boid.Position);
                else e.Graphics.DrawImage(iconRegular, boid.Position);
                DrawString(Convert.ToString(swarm.Boids.Count), Convert.ToString(swarm1.Boids.Count), 20, 20);
            }
            DrawString(Convert.ToString(swarm.Boids.Count), Convert.ToString(swarm1.Boids.Count), 20, 20);
        }
        private static Image CreateIcon(Brush brush)
        {
            Bitmap icon = new Bitmap(16, 16);
            Graphics g = Graphics.FromImage(icon);
            Point p1 = new Point(0, 16);
            Point p2 = new Point(16, 8);
            Point p3 = new Point(0, 0);
            Point p4 = new Point(5, 8);
            Point[] points = { p1, p2, p3, p4 };
            g.FillPolygon(brush, points);
            return icon;
        }

        private void timer_Tick(object sender, EventArgs e)
        {          
                     
            swarm.MoveBoids();
            swarm1.MoveBoids();
            
            Hunt();
            Invalidate();
            
            
        }
        private void DrawString(string drawString,string drawString1, float x, float y)
        {

            System.Drawing.Graphics formGraphics = this.CreateGraphics();
            System.Drawing.Font drawFont = new System.Drawing.Font(
                "Arial", 14);
            System.Drawing.SolidBrush drawBrush = new
            System.Drawing.SolidBrush(Color.Blue);
            System.Drawing.SolidBrush drawBrush2 = new
            System.Drawing.SolidBrush(Color.Black);
            System.Drawing.SolidBrush drawBrush3 = new
            System.Drawing.SolidBrush(Color.Red);
            formGraphics.DrawString(drawString, drawFont, drawBrush, x, y);
            formGraphics.DrawString(" : ", drawFont, drawBrush2, x+30, y);
            formGraphics.DrawString(drawString1, drawFont, drawBrush3, x+60, y);
            drawFont.Dispose();
            drawBrush.Dispose();
            formGraphics.Dispose();
        }


        private void Hunt()
        {
            List<Boid> boids = new List<Boid>();
            int dist = 0;
            foreach (Boid boid in swarm1.Boids)
            {
                foreach (Boid boid1 in swarm.Boids)
                {
                    dist = (int)Math.Abs(Boid.Distance(boid.Position, boid1.Position));
                    if (dist < 4)
                    {
                        boids.Add(boid1);
                    }
                }
            }
            foreach (Boid boid in boids)
            {
                swarm.Boids.Remove(boid);
            }
        } 
            
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        
        
    }

    public class Swarm
    {
        public List<Boid> Boids = new List<Boid>();
        
        public Swarm(int boundary,int boundary1,bool ind,int kol)
        {
            for (int i = 0; i < kol; i++)
            {
                Boids.Add(new Boid(ind, boundary, boundary1));
            }
        }
        
        public void MoveBoids()
        {
            
            foreach (Boid boid in Boids)
            {
                               
                    boid.Move(Boids);                 
            }
        }
        
    }

    public class Boid
    {
        
        private static Random rnd = new Random();
        private static float border = 100f;
        private static float sight = 75f;
        private static float space = 30f;
        private static float speed = 12f;
        private float boundary;
        private float boundary1;
        public float dX = 8f;
        public float dY = 8f;
        public bool Zombie;
        public PointF Position;

        public Boid(bool zombie, int boundary, int boundary1)
        {
            Position = new PointF(rnd.Next(boundary), rnd.Next(boundary1));
            this.boundary = boundary;
            this.boundary1 = boundary1;
            Zombie = zombie;
        }
        public Boid(bool zombie, int boundary,int boundary1, Point p)
        {
            Position = new PointF(p.X, p.Y);
            this.boundary = boundary;
            this.boundary1 = boundary1;
            Zombie = zombie;
        }

        public void Move(List<Boid> boids)
        {
            if (!Zombie)
            {

                Flock(boids);
            }
            else
            {
                Flock(boids);
            }              
            
            CheckBounds();
            CheckSpeed();
            Position.X += dX;
            Position.Y += dY;
            
        }

        private void Flock(List<Boid> boids)
        {
            foreach (Boid boid in boids)
            {
                float distance = Distance(Position, boid.Position);
                if (boid != this )
                {
                    if (distance < space)
                    {
                        // Create space.
                        dX += Position.X - boid.Position.X;
                        dY += Position.Y - boid.Position.Y;
                    }
                    else if (distance < sight)
                    {
                        // Flock together.
                        dX += (boid.Position.X - Position.X) * 0.001f;
                        dY += (boid.Position.Y - Position.Y) * 0.001f;

                         // Align movement.
                        dX += boid.dX * 0.5f;
                        dY += boid.dY * 0.5f;
                    }
                    
                }
                
               
            }
        }
        
        

        public static float Distance(PointF p1, PointF p2)
        {
            double val = Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2);
            return (float)Math.Sqrt(val);
        }

        private void CheckBounds()
        {
            float val = boundary - border;
            float val1 = boundary1 - border;
            if (Position.X < border) dX += border - Position.X;
            if (Position.Y < border) dY += border - Position.Y;
            if (Position.X > val) dX += val - Position.X;
            if (Position.Y > val1) dY += val1 - Position.Y;
        }

        private void CheckSpeed()
        {
            float s;
            s = speed;
            float val = Distance(new PointF(0f, 0f), new PointF(dX, dY));
            if (val > s)
            {
                dX = dX * s / val;
                dY = dY * s / val;
            }
        }
    }
}
