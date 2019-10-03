using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JogoDama
{
    public partial class Form1 : Form
    {
        int turn = 0;
        bool movExtra = false;
        PictureBox select = null;

        List<PictureBox> red = new List<PictureBox>();
        List<PictureBox> blue = new List<PictureBox>();

        public Form1()
        {
            InitializeComponent();
            LoadList(); 
        }

        private void LoadList()
        {
                blue.Add(blue1);
                blue.Add(blue2);
                blue.Add(blue3);
                blue.Add(blue4);
                blue.Add(blue5);
                blue.Add(blue6);
                blue.Add(blue7);
                blue.Add(blue8);
                blue.Add(blue9);
                blue.Add(blue10);
                blue.Add(blue11);
                blue.Add(blue12);

                red.Add(red1);
                red.Add(red2);
                red.Add(red3);
                red.Add(red4);
                red.Add(red5);
                red.Add(red6);
                red.Add(red7);
                red.Add(red8);
                red.Add(red9);
                red.Add(red10);
                red.Add(red11);
                red.Add(red12);
        }

        public void Selection(object objeto)
        {
            if (!movExtra)
            {
                try { select.BackColor = Color.Black; }
                catch { }
                PictureBox piece = (PictureBox)objeto;
                select = piece;
                select.BackColor = Color.Lime;
            }
        }

        private void SelectionRed(object sender, MouseEventArgs e)
        {
            if(turn % 2 == 0)
            {
                Selection(sender);
            }
            else
            {
                MessageBox.Show("Turno da equipe Azul!");
            }
        }

        private void SelectionBlue(object sender, MouseEventArgs e)
        {
            if (turn % 2 == 1)
            {
                Selection(sender);
            }
            else
            {
                MessageBox.Show("Turno da equipe Vermelha!");
            }
        }

        private void Movement(PictureBox picture)
        {
            if(select != null)
            {                   
               string color = select.Name.ToString().Substring(0, 3);

                if (validate(select, picture, color))
                    {
                    Point back = select.Location;
                    select.Location = picture.Location;
                    int foward = back.Y - picture.Location.Y;


                    if (!MovimentosExtras(color) || Math.Abs(foward) == 50)//verificar moviemntos extras
                    {
                        IfQueen(color);
                        turn++;
                        select.BackColor = Color.Black;
                        movExtra = false;
                    }
                    else
                    {
                        movExtra = true;
                    }
                }
            }
        }

        private bool MovimentosExtras(string color)
        {
            List<PictureBox> oposto = color == "red" ? blue : red;
            List<Point> position = new List<Point>();
            int nextPosition = color == "red" ? -100 : 100;

            position.Add(new Point(select.Location.X + 100, select.Location.Y + nextPosition));
            position.Add(new Point(select.Location.X - 100, select.Location.Y + nextPosition));

            if(select.Tag == "queen")
            {
                position.Add(new Point(select.Location.X + 100, select.Location.Y - nextPosition));
                position.Add(new Point(select.Location.X - 100, select.Location.Y - nextPosition));
            }
            bool result = false;
            for (int i=0; i<position.Count; i++)
            {
                if(position[i].X >=50 && position[i].X <= 400 && position[i].Y >= 50 && position[i].Y <= 400)
                {
                    if(!ocupado(position[i], red)&& !ocupado(position[i], blue))
                    {
                        Point midPoint = new Point(promedio(position[i].X, select.Location.X), promedio(position[i].Y, select.Location.Y));
                        if(ocupado(midPoint, oposto))
                        {
                            result = true;
                        }
                    }
                }
            }
            return result;
        }

        private bool ocupado(Point point, List<PictureBox> lado)
        {
            for (int i = 0; i < lado.Count; i++)
            {
                if(point == lado[i].Location)
                {
                    return true;
                }
            }
            return false;
        }
        private int promedio(int n1, int n2)
        {
            int resultado = n1 + n2;
            resultado /= 2;
            return Math.Abs(resultado);
        }
        private bool validate(PictureBox origem, PictureBox destino, string color)
        {
            Point pointOrigem = origem.Location;
            Point pointDestino = destino.Location;
            int next = pointOrigem.Y - pointDestino.Y;
            next = color == "red" ? next : (next * -1);
            next = select.Tag == "queen" ? Math.Abs(next) : next;

            if(next == 50)
            {
                return true;
            }
            else if(next == 100)
            {
                Point midpoint = new Point(promedio(pointDestino.X, pointOrigem.X), promedio(pointDestino.Y, pointOrigem.Y));
                List<PictureBox> oposto = color == "red" ? blue : red;

                for(int i = 0; i<oposto.Count; i++)
                {
                    if (oposto[i].Location == midpoint)
                    {
                        oposto[i].Location = new Point(0, 0);
                        oposto[i].Visible = false;
                        return true;
                    }
                }
            }
            return false;
        }
        private void IfQueen(string color)
        {
            if (color == "blu" && select.Location.Y == 400)
            {
                select.BackgroundImage = Properties.Resources.PecaAzulDama2;
                select.Tag = "queen";
            }
            if (color == "red" && select.Location.Y == 50)
            {
                select.BackgroundImage = Properties.Resources.PecaVermDama2;
                select.Tag = "queen";
            }
        }

        private void SquareClick(object sender, MouseEventArgs e)
        {
            Movement((PictureBox)sender);
        }
    }
}
