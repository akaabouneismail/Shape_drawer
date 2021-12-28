// Made by: SID 1738483
// last update: 29/04/2021



using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Cg
{
    public partial class GrafPack : Form
    {
        private MainMenu mainMenu;
        private MenuItem selectItem;
        private MenuItem deleteItem;
        private MenuItem transformItem;

        private bool selectSquareStatus = false;
        private bool selectTriangleStatus = false;
        private bool selectCircleStatus = false;

        private List<Shape> shapes;
        private int positionlistShape = 0;
        private bool isSelected = false;
        private bool IsMove = false;

        private int clicknumber = 0;
        private Point one;
        private Point two;
        private Point three;

        public GrafPack()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.White;

            shapes = new List<Shape>(); // creates list to store created shapes

            // The following approach uses menu items coupled with mouse clicks
            MainMenu mainMenu = new MainMenu();

            /** Create **/
            MenuItem createItem = new MenuItem();
            MenuItem squareItem = new MenuItem();
            MenuItem triangleItem = new MenuItem();
            MenuItem circleItem = new MenuItem();
            

            /** Transforme **/
            transformItem = new MenuItem();
            MenuItem moveItem = new MenuItem();
            MenuItem rotateItem = new MenuItem();
            

            selectItem = new MenuItem();

            deleteItem = new MenuItem();

            MenuItem exitItem = new MenuItem();

            /** Labels Menue**/
            createItem.Text = "&Create";
            squareItem.Text = "&Square";
            triangleItem.Text = "&Triangle";
            selectItem.Text = "&Select";
            circleItem.Text = "&Circle";
            deleteItem.Text = "&Delete";
            exitItem.Text = "&Exit";
            moveItem.Text = "&Move";
            rotateItem.Text = "&Rotate";
            transformItem.Text = "&Transform";
            

            /** Menue items creation **/
            mainMenu.MenuItems.Add(createItem);
            mainMenu.MenuItems.Add(deleteItem);
            mainMenu.MenuItems.Add(selectItem);
            mainMenu.MenuItems.Add(exitItem);
            mainMenu.MenuItems.Add(transformItem);
            transformItem.MenuItems.Add(moveItem);
            transformItem.MenuItems.Add(rotateItem);
            createItem.MenuItems.Add(squareItem);
            createItem.MenuItems.Add(triangleItem);
            createItem.MenuItems.Add(circleItem);
            /***/

            /*This handles mouse click events */

            selectItem.Click += new System.EventHandler(this.selectShape);

            squareItem.Click += new System.EventHandler(this.selectSquare);
            triangleItem.Click += new System.EventHandler(this.selectTriangle);
            circleItem.Click += new System.EventHandler(this.selectCircle);

            deleteItem.Click += new System.EventHandler(this.deleteShape);

            exitItem.Click += new System.EventHandler(this.exitProgram);

            moveItem.Click += new System.EventHandler(this.transformeShapeMove);
            rotateItem.Click += new System.EventHandler(this.transformeShapeRotate);

            


            this.Menu = mainMenu;

            /** handle mouse clics in oder to get the position from the forme (x,y) **/
            this.MouseClick += mouseClick;

            /** handle keyboard keys (right and left) to select a shape **/
            this.KeyDown += mouveSelectionShape;

        }

        // Generally, all methods of the form are usually private
        private void selectSquare(object sender, EventArgs e)
        {
            selectSquareStatus = true;

            MessageBox.Show("Click OK and then click once each at two locations to create a square"); // prompt the user to click on ok then choose the location
        }

        private void transformeShapeMove(object sender, EventArgs e)
        {
            /** check if an item has already been selected **/

            if (shapes.Count <= 0 || isSelected == false)
            {
                MessageBox.Show("Please first select a shape !");

            }

            else if (shapes.Count > 0 && isSelected == true)
            {

                IsMove = true;

            }

        }

        private void transformeShapeRotate(object sender, EventArgs e)
        {
            /** check if an item is already selected **/

            if (shapes.Count <= 0 || isSelected == false)
            {
                MessageBox.Show("Please first select a shape !");

            }

        }

        private void selectTriangle(object sender, EventArgs e)
        {
            selectTriangleStatus = true;

            MessageBox.Show("Click OK and then click once each at three locations to create a triangle"); //prompt the user to select triangle points location
        }

        private void selectCircle(object sender, EventArgs e)
        {
            selectCircleStatus = true;

            MessageBox.Show("Click OK and then click once each at two locations (Diameter) to create a circle"); // prompt the user to choose circle location
        }

        private void selectShapeRed(int position, Color color) // selects shapes and colors them in red
        {

            if (shapes.Count > 0) // if there are shapes
            {
                isSelected = true;

                Graphics g = this.CreateGraphics();

                Pen pen = new Pen(color);

                if (shapes.ElementAt(position).GetType() == typeof(Triangle))
                {

                    Triangle triangle = (Triangle)shapes.ElementAt(position);

                    triangle.draw(g, pen);

                }

                if (shapes.ElementAt(position).GetType() == typeof(Circle))
                {

                    Circle circle = (Circle)shapes.ElementAt(position);

                    circle.draw(g, pen);

                }

                if (shapes.ElementAt(position).GetType() == typeof(Square))
                {

                    Square square = (Square)shapes.ElementAt(position);

                    square.draw(g, pen);

                }

            }

            else // if there are no shapes
            {
                MessageBox.Show("Please create first a shape !");

                isSelected = false;
            }
        }

        private void selectShape(object sender, EventArgs e)
        {

            selectShapeRed(positionlistShape, Color.Red);

            IsMove = false;

        }

        private void deleteShape(object sender, EventArgs e)
        {
            

            if (shapes.Count > 0 && isSelected == true) // if shapes exist and the user has selected one
            {

                shapes.RemoveAt(positionlistShape);

                Graphics graph = this.CreateGraphics();

                /** delete all shapes **/

                graph.Clear(Color.White);

                positionlistShape = 0;

                /** recreate all shapes  **/

                recreateAllShapes();

               
            }

            else if (isSelected == false || shapes.Count == 0) // if no shapes are selected or no shapes exist
            {
                MessageBox.Show("Please create first a shape !");
            }

        }

        private void exitProgram(object sender, EventArgs e)  // exit the program
        {
            Application.Exit();
        }


        // This method is quite important and detects all mouse clicks - other methods may need
        // to be implemented to detect other kinds of event handling eg keyboard presses.
        private void mouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // 'if' statements can distinguish different selected menu operations to implement.
                // There may be other (better, more efficient) approaches to event handling,
                // but this approach works.
                if (selectSquareStatus == true)
                {
                    if (clicknumber == 0) // if no click has been done take first click
                    {
                        one = new Point(e.X, e.Y);
                        clicknumber = 1;
                    }
                    else // if one click has already been done take second click
                    {
                        two = new Point(e.X, e.Y);
                        clicknumber = 0;
                        selectSquareStatus = false;

                        Graphics g = this.CreateGraphics();

                        Pen blackpen = new Pen(Color.Black);

                        Square aShape = new Square(one, two);

                        aShape.draw(g, blackpen); // draw the shape

                        shapes.Add(aShape); // add shape to list
                    }
                }

                else if (selectTriangleStatus == true)
                {
                    if (clicknumber == 0) // if no click has been done take first click
                    {
                        one = new Point(e.X, e.Y);
                        clicknumber = 1;

                    }
                    else if (clicknumber == 1)// if one click has already been done take second click
                    {

                        two = new Point(e.X, e.Y);
                        clicknumber = 2;


                    }

                    else if (clicknumber == 2)// if 2 clicks have already been done take third click
                    {

                        three = new Point(e.X, e.Y);

                        selectTriangleStatus = false;
                        clicknumber = 0;

                        Graphics g = this.CreateGraphics();
                        Pen blackpen = new Pen(Color.Black);

                        Triangle aShape = new Triangle(one, two, three);

                        aShape.draw(g, blackpen); //draw the triangle

                        shapes.Add(aShape); //add the triangle to the list

                    }
                }

                else if (selectCircleStatus == true)
                {
                    if (clicknumber == 0) //if no click has been taken
                    {
                        one = new Point(e.X, e.Y);
                        clicknumber = 1;
                    }
                    else if (clicknumber == 1) //if one click has already been taken then take the second one
                    {
                        two = new Point(e.X, e.Y);
                        clicknumber = 0;
                        selectCircleStatus = false;

                        Graphics g = this.CreateGraphics();
                        Pen blackpen = new Pen(Color.Black);

                        Circle aShape = new Circle(one, two);

                        aShape.draw(g, blackpen); //draw the circle

                        shapes.Add(aShape); //add the circle to the list


                    }
                }
            }
        }

        public void recreateAllShapes() //method to recreate all the shapes 
        {
            foreach (var item in shapes)
            {

                if (item.GetType() == typeof(Triangle))
                {
                    var triangle = (Triangle)item;

                    Graphics graphics = this.CreateGraphics();

                    Pen pen = new Pen(Color.Black);

                    triangle.draw(graphics, pen);
                }

                else if (item.GetType() == typeof(Square))
                {
                    var square = (Square)item;

                    Graphics graphics = this.CreateGraphics();

                    Pen pen = new Pen(Color.Black);

                    square.draw(graphics, pen);
                }

                else if (item.GetType() == typeof(Circle))
                {
                    var circle = (Circle)item;

                    Graphics graphics = this.CreateGraphics();

                    Pen pen = new Pen(Color.Black);

                    circle.draw(graphics, pen);

                }

                /** Reselect the first shape **/

                /** combination of count list and isSelected unless it will be delete without even select a shape **/

                selectItem.PerformClick();


            }
        }

        /** This methode calls selectShapeRed in oder to highlight the shape in red  **/

        private void mouveSelectionShape(object sender, KeyEventArgs e)
        {
            Graphics graph = this.CreateGraphics();
            Pen pen = new Pen(Color.Black);
            if (isSelected)
            {

                if (e.KeyData == Keys.Right) // if the user pushes the right key on keyboard
                {
                    if (positionlistShape < shapes.Count)
                    {
                        selectShapeRed(positionlistShape, Color.Black);

                        int pos = positionlistShape;

                        if (pos + 1 < shapes.Count)

                            selectShapeRed(++positionlistShape, Color.Red);

                        else
                        {
                            positionlistShape = 0;

                            selectShapeRed(positionlistShape, Color.Red); //highlight first shape in red
                        }

                    }


                }

                if (e.KeyData == Keys.Left) // if the user pushes the left key on keyboard
                {
                    if (positionlistShape != 0)
                    {
                        selectShapeRed(positionlistShape, Color.Black);

                        selectShapeRed(--positionlistShape, Color.Red); //highlight shape in red


                    }

                    else if (positionlistShape == 0)
                    {
                        selectShapeRed(0, Color.Black);

                        positionlistShape = shapes.Count - 1;

                        selectShapeRed(positionlistShape, Color.Red);
                    }

                }

                if (e.KeyData == Keys.R)
                {
                    if (shapes.ElementAt(positionlistShape).GetType() == typeof(Triangle))
                    {

                        Triangle triangle = (Triangle)shapes.ElementAt(positionlistShape);

                        /* Move triangle to the right using its coordinates */

                        triangle.FirstPt.X += 5;

                        triangle.SecondPt.X += 5;

                        triangle.ThirdPt.X += 5;



                        /** delete all shapes **/

                        graph.Clear(Color.White);

                        /** recreate all shapes  **/

                        recreateAllShapes();


                    }

                    if (shapes.ElementAt(positionlistShape).GetType() == typeof(Circle))
                    {

                        Circle circle = (Circle)shapes.ElementAt(positionlistShape);

                        /* Move cirlle to the right using the diameter coordinates*/

                        circle.FirstPt.X += 5;

                        circle.SecondPt.X += 5;



                        /** delete all shapes **/

                        graph.Clear(Color.White);

                        /** recreate all shapes  **/

                        recreateAllShapes();

                        

                    }

                    if (shapes.ElementAt(positionlistShape).GetType() == typeof(Square))
                    {

                        Square square = (Square)shapes.ElementAt(positionlistShape);

                        /* Moving the square to the right using its 4 points coordinates*/
                        square.keyPt.X += 5;

                        square.oppPt.X += 5;

                        square.first += 5;

                        square.second += 5;




                        /** delete all shapes **/

                        graph.Clear(Color.White);

                        /** recreate all shapes  **/

                        recreateAllShapes();

                       

                    }
                }

                if (e.KeyData == Keys.D) // if the user presses the D key on the keyboard
                {
                    if (shapes.ElementAt(positionlistShape).GetType() == typeof(Triangle))
                    {

                        Triangle triangle = (Triangle)shapes.ElementAt(positionlistShape);

                        triangle.FirstPt.Y += 5;

                        triangle.SecondPt.Y += 5;

                        triangle.ThirdPt.Y += 5;



                        /** delete all shapes **/

                        graph.Clear(Color.White);

                        /** recreate all shapes  **/

                        recreateAllShapes();

                        


                    }

                    if (shapes.ElementAt(positionlistShape).GetType() == typeof(Circle))
                    {

                        Circle circle = (Circle)shapes.ElementAt(positionlistShape);

                        circle.FirstPt.Y += 5;

                        circle.SecondPt.Y += 5;



                        /** delete all shapes **/

                        graph.Clear(Color.White);

                        /** recreate all shapes  **/

                        recreateAllShapes();

                        

                    }

                    if (shapes.ElementAt(positionlistShape).GetType() == typeof(Square))
                    {

                        Square square = (Square)shapes.ElementAt(positionlistShape);

                        square.keyPt.Y += 5;

                        square.oppPt.Y += 5;

                        square.firstY += 5;

                        square.secondY += 5;



                        /** delete all shapes **/

                        graph.Clear(Color.White);

                        /** recreate all shapes  **/

                        recreateAllShapes();

                        

                    }
                }
                if (e.KeyData == Keys.U) //if the user presses the U key on the keyboard
                {
                    if (shapes.ElementAt(positionlistShape).GetType() == typeof(Triangle))
                    {
                        Triangle triangle = (Triangle)shapes.ElementAt(positionlistShape);

                        /* Moving the triangle using its Y coordinates */

                        triangle.FirstPt.Y -= 5;
                        triangle.SecondPt.Y -= 5;
                        triangle.ThirdPt.Y -= 5;

                        graph.Clear(Color.White);

                        recreateAllShapes();
                    }

                    if (shapes.ElementAt(positionlistShape).GetType() == typeof(Square))
                    {
                        Square square = (Square)shapes.ElementAt(positionlistShape);

                        /* Moving the square using its Y coordinates*/

                        square.keyPt.Y -= 5;
                        square.oppPt.Y -= 5;
                        square.firstY -= 5;
                        square.secondY -= 5;

                        graph.Clear(Color.White);

                        recreateAllShapes();
                    }

                    else if (shapes.ElementAt(positionlistShape).GetType() == typeof(Circle))
                    {
                        Circle circle = (Circle)shapes.ElementAt(positionlistShape);

                        /* Moving the circle using its Y coordinates*/
                        circle.FirstPt.Y -= 5;
                        circle.SecondPt.Y -= 5;

                        graph.Clear(Color.White);

                        recreateAllShapes();
                    }


                }
                if (e.KeyData == Keys.L) //if the user presses the L key on the keyboard
                {
                    if (shapes.ElementAt(positionlistShape).GetType() == typeof(Triangle))
                    {
                        Triangle triangle = (Triangle)shapes.ElementAt(positionlistShape);

                        /* Move the triangle using its X coordinates*/

                        triangle.FirstPt.X -= 5;
                        triangle.SecondPt.X -= 5;
                        triangle.ThirdPt.X -= 5;

                        graph.Clear(Color.White);

                        recreateAllShapes();
                    }

                    else if (shapes.ElementAt(positionlistShape).GetType() == typeof(Square))
                    {
                        Square square = (Square)shapes.ElementAt(positionlistShape);

                        /* Moving the square using its X coordinates */

                        square.keyPt.X -= 5;
                        square.oppPt.X -= 5;
                        square.first -= 5;
                        square.second -= 5;

                        graph.Clear(Color.White);

                        recreateAllShapes();
                    }

                    else if (shapes.ElementAt(positionlistShape).GetType() == typeof(Circle))
                    {
                        Circle circle = (Circle)shapes.ElementAt(positionlistShape);

                        /* Moving the circle using its X coordinates*/

                        circle.FirstPt.X -= 5;
                        circle.SecondPt.X -= 5;

                        graph.Clear(Color.White);

                        recreateAllShapes();
                    }
                }



            }
        }

        private void InitializeComponent() // code generated by visual studio to solve a problem with InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // GrafPack
            // 
            this.ClientSize = new System.Drawing.Size(274, 229);
            this.Name = "GrafPack";
            this.Load += new System.EventHandler(this.GrafPack_Load);
            this.ResumeLayout(false);

        }

        private void GrafPack_Load(object sender, EventArgs e)
        {

        }
    }






    abstract class Shape
    {
        // This is the base class for Shapes in the application. It should allow an array or LL
        // to be created containing different kinds of shapes.

        public Shape()   // constructor
        {

        }


    }

    class Square : Shape // creating square shape
    {
        //This class contains the specific details for a square defined in terms of opposite corners
        public Point keyPt, oppPt;

        public int first, second, firstY, secondY;      // these points identify opposite corners of the square

        public Square(Point keyPt, Point oppPt)   // constructor
        {
            this.keyPt = keyPt;
            this.oppPt = oppPt;
        }

        
        public void draw(Graphics g, Pen blackPen)
        {
            // This method draws the square by calculating the positions of the other 2 corners
            double xDiff, yDiff, xMid, yMid;   // range and mid points of x & y  


            // calculate ranges and mid points
            xDiff = oppPt.X - keyPt.X;
            yDiff = oppPt.Y - keyPt.Y;
            xMid = (oppPt.X + keyPt.X) / 2;
            yMid = (oppPt.Y + keyPt.Y) / 2;

            /** for transformation **/

            first = (int)(xMid + yDiff / 2);
            second = (int)(xMid - yDiff / 2);

            firstY = (int)(yMid - xDiff / 2);
            secondY = (int)(yMid + xDiff / 2);


            // draw square
            g.DrawLine(blackPen, (int)keyPt.X, (int)keyPt.Y, (int)(xMid + yDiff / 2), (int)(yMid - xDiff / 2));
            g.DrawLine(blackPen, first, firstY, (int)oppPt.X, (int)oppPt.Y);
            g.DrawLine(blackPen, (int)oppPt.X, (int)oppPt.Y, (int)(xMid - yDiff / 2), (int)(yMid + xDiff / 2));
            g.DrawLine(blackPen, second, secondY, (int)keyPt.X, (int)keyPt.Y);
        }


    }

    class Triangle : Shape // creating Triangle shape
    {

        public Point FirstPt, SecondPt, ThirdPt;



        public Triangle(Point FirstPt, Point SecondPt, Point ThirdPt)
        {
            this.FirstPt = FirstPt;
            this.SecondPt = SecondPt;
            this.ThirdPt = ThirdPt;


        }

        public void draw(Graphics g, Pen blackPen) //drawing the triangle
        {

            g.DrawLine(blackPen, (int)FirstPt.X, (int)FirstPt.Y, (int)SecondPt.X, (int)SecondPt.Y);
            g.DrawLine(blackPen, (int)FirstPt.X, (int)FirstPt.Y, (int)ThirdPt.X, (int)ThirdPt.Y);
            g.DrawLine(blackPen, (int)SecondPt.X, (int)SecondPt.Y, (int)ThirdPt.X, (int)ThirdPt.Y);

        }


    }

    class Circle : Shape // creating circle shape
    {
        public Point FirstPt, SecondPt;

        public Circle(Point FirstPt, Point SecondPt)
        {
            this.FirstPt = FirstPt;
            this.SecondPt = SecondPt;

        }


        public void draw(Graphics g, Pen blackPen)
        {
            /* Calculate distance between 2 points*/
            double Distance = Math.Sqrt(Math.Pow((SecondPt.X - FirstPt.X), 2) + Math.Pow(SecondPt.Y - FirstPt.Y, 2));

            Rectangle rt = new Rectangle(FirstPt.X, FirstPt.Y, (int)Distance, (int)Distance);

            g.DrawEllipse(blackPen, rt);

        }


    }
}

