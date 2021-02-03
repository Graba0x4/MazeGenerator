using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MazeGenerator
{
    public struct Ceill
    {
        public bool IsVisited { get; set; }
        public Point p { get; set; }
    }

    public partial class Form1 : Form
    {


        public Form1()
        {

            InitializeComponent();

            if (PanelH % MazeH != 0)
            {
                PointOffsetH = (int)(Math.Ceiling(PanelH / (double)(MazeH - 1)));
            }
            if (PanelW % MazeW != 0)
            {
                PointOffsetW = (int)(Math.Ceiling(PanelH / (double)(MazeW - 1)));
            }

            Size = new Size((PointOffsetW * MazeW) + 17, (PointOffsetH * MazeH) + 40);
            
            Update();

            grid = new Ceill[MazeH, MazeW];
            Point temp;
            for (int j = 0; j < MazeW; j++)
            {
                for (int i = 0; i < MazeH; i++)
                {
                    temp = new Point(j * PointOffsetW,
                                     i * PointOffsetH);

                    //temp = new Point(j * 40, i * 40);
                    grid[i, j] = new Ceill() { IsVisited = false, p = temp };
                }
            }

            g = panel1.CreateGraphics();
            p.SetLineCap(System.Drawing.Drawing2D.LineCap.Square, System.Drawing.Drawing2D.LineCap.Square, System.Drawing.Drawing2D.DashCap.Flat);
            yellow.SetLineCap(System.Drawing.Drawing2D.LineCap.Square, System.Drawing.Drawing2D.LineCap.Square, System.Drawing.Drawing2D.DashCap.Flat);
            magenta.SetLineCap(System.Drawing.Drawing2D.LineCap.Square, System.Drawing.Drawing2D.LineCap.Square, System.Drawing.Drawing2D.DashCap.Flat);
            stack = new Stack<Ceill>();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {


            for (int a = 0; a < MazeH; a++)
            {
                for (int b = 0; b < MazeW; b++)
                {
                    e.Graphics.DrawRectangle(p, new Rectangle(grid[a, b].p, new Size(1, 1)));

                }
            }
            //start point;
            stack.Push(grid[0, 0]);
            grid[0, 0].IsVisited = true;
            Console.WriteLine(grid[9, 9].p.ToString());

            string str = "";
            string actions = "";
            int count = 0;
            char r = char.MinValue;
            Random random = new Random();
            int i = 0, j = 0;

            while (stack.Count >= 1)
            {

                //find neighbor
                if (i - 1 >= 0)
                {
                    // is possible to go up
                    if (grid[i - 1, j].IsVisited == false)
                    {
                        str += "U";
                        count++;
                    }
                }
                if (j + 1 <= MazeW - 1)
                {
                    //is possible to go right
                    if (grid[i, j + 1].IsVisited == false)
                    {
                        str += "R";
                        count++;
                    }
                }
                if (i + 1 <= MazeH - 1)
                {
                    // is possible to go bottom
                    if (grid[i + 1, j].IsVisited == false)
                    {
                        str += "B";
                        count++;
                    }
                }
                if (j - 1 >= 0)
                {
                    // is possible to go left
                    if (grid[i, j - 1].IsVisited == false)
                    {
                        str += "L";
                        count++;
                    }
                }




                if (count == 0)
                {
                    Debug.WriteLine("Stack Pop \n " + actions);
                    if (stack.Count > 1)
                    {
                        e.Graphics.DrawLine(yellow, stack.Peek().p, stack.ElementAt(1).p);
                    }

                    stack.Pop();
                    if (stack.Count == 0) break;
                    switch (actions[actions.Length - 1])
                    {
                        case 'U':
                            i++;
                            break;
                        case 'R':
                            j--;
                            break;
                        case 'B':
                            i--;
                            break;
                        case 'L':
                            j++;
                            break;
                    }
                    actions = actions.Remove(actions.Length - 1);
                    Debug.WriteLine("Remove : " + actions);
                    continue;
                }
                else
                {
                    if (str.Length == 1)
                    {
                        r = str[0];
                    }
                    else
                    {
                        int b1 = 0;
                        int b2 = str.Length - 1;
                        r = str[random.Next(b1, b2 + 1)];
                    }
                    //next ceill
                    switch (r)
                    {
                        case 'U': //up

                            Debug.WriteLine("go up");
                            stack.Push(grid[--i, j]);


                            goto case 'A';
                        case 'R': // right
                            Debug.WriteLine("go r");

                            stack.Push(grid[i, ++j]);


                            goto case 'A';
                        case 'B': //bottom
                            Debug.WriteLine("go b");

                            stack.Push(grid[++i, j]);

                            goto case 'A';
                        case 'L': //left

                            Debug.WriteLine("go l");

                            stack.Push(grid[i, --j]);
                            goto case 'A';

                        case 'A':
                            grid[i, j].IsVisited = true;
                            Debug.WriteLine($"grid[{i},{j}].IsVisited = {grid[i, j].IsVisited}");
                            break;

                        default:
                            throw new ApplicationException();

                    }

                    //debug
                    if (stack.Count > 1)
                        e.Graphics.DrawLine(magenta, stack.Peek().p, stack.ElementAt(1).p);



                    Console.Clear();
                    string betterdisp = $"i = {i} \n" +
                                      $"j = {j} \n" +
                                      $"str = {str} \n" +
                                      $"count = {count} \n" +
                                      $"r = {r} \n" +
                                      $"actions = {actions}";
                    Console.WriteLine(betterdisp);

                    actions += r;
                    str = "";
                    r = char.MinValue;
                    count = 0;
                }
                Thread.Sleep(50);
                //Console.ReadKey();

            }
            //Console.Clear();
            Console.WriteLine("Maze generated!!!");
        }

        private void Panel1_Resize(object sender, EventArgs e)
        {
            panel1.Update();
        }
    }
}
