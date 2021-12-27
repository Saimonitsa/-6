using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace лаба_ооп6
{
    public partial class Form1 : Form
    {
        Storage<Shape> myStorage = new Storage<Shape>();
        public Form1()
        {
            InitializeComponent();
            (pictureBox1 as Control).KeyPress += new KeyPressEventHandler(PressEventHandler);
        }

        public abstract class Shape : ObjObserved
        {
            protected Rectangle rect; //область объекта для его отрисовки и выделения
            protected int x, y, width, height; //x, y для позиции объектов и групп, w и h для учитывания границ отрисовки
            public bool sticky = false;
            abstract public void Resize();
            abstract public void SetXY(int _x, int _y);
            abstract public void OffsetXY(int _x, int _y);
            abstract public void SetColor(Color c);
            abstract public void Grow(int gr);
            abstract public void Rotate(int gr);
            abstract public void DrawObj(System.Drawing.Graphics e);
            abstract public void DrawRectangle(System.Drawing.Graphics e, Pen pen);
            abstract public bool Find(int _x, int _y);
            abstract public Rectangle GetRectangle();  //получить границы фигуры для контроля выхода за пределы

            public void PressEventHandler(object sender, KeyPressEventArgs e)
            {
                if (MyStorage.size() != 0)
                {
                    if (e.KeyChar == 119) MyStorage.get().OffsetXY(0, -1);
                    if (e.KeyChar == 115) MyStorage.get().OffsetXY(0, 1);
                    if (e.KeyChar == 97) MyStorage.get().OffsetXY(-1, 0);
                    if (e.KeyChar == 100) MyStorage.get().OffsetXY(1, 0);
                    if (e.KeyChar == 98) MyStorage.get().Grow(1);
                    if (e.KeyChar == 118) MyStorage.get().Grow(-1);
                    if (e.KeyChar == 110)
                    {
                        while (MyStorage.size() != 0) MyStorage.del();
                        pictureBox1.Invalidate();
                    }
                    if (e.KeyChar == 112)
                    {

                        if (colorDialog1.ShowDialog() == DialogResult.OK) myStorage.get().SetColor(colorDialog1.Color);
                    }

                    Random rnd = new Random();
                    int obj = rnd.Next(1, 3);
                    if (e.KeyChar == 49)
                    {
                        int rad = rnd.Next(10, 100);
                        MyStorage.add(new Circle(rnd.Next(4, pictureBox1.Width - 50), rnd.Next(4, pictureBox1.Height - 50), rad, Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)), pictureBox1.Width - 50, pictureBox1.Height - 50));
                    }
                    if (e.KeyChar == 51)
                    {
                        int rad = rnd.Next(10, 100);
                        MyStorage.add(new Polygon(rnd.Next(4, pictureBox1.Width - 50), rnd.Next(4, pictureBox1.Height - 50), rad, rnd.Next(3, 9), Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)), pictureBox1.Width - 50, pictureBox1.Height - 50));
                    }
                    if (e.KeyChar == 50)
                    {
                        int rad = rnd.Next(10, 100);
                        MyStorage.add(new Star(rnd.Next(4, pictureBox1.Width - 50), rnd.Next(4, pictureBox1.Height - 50), rad, rnd.Next(5, 9), Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)), pictureBox1.Width - 50, pictureBox1.Height - 50));
                    }

                }
                pictureBox1.Invalidate();
            }


            private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
            {
                bool isFinded = false;
                if (MyStorage.size() != 0)
                {
                    MyStorage.toFirst();
                    for (int i = 0; i < MyStorage.size(); i++, MyStorage.next())
                    {
                        if (MyStorage.getIterator().Find(e.X, e.Y) == true && e.Button == MouseButtons.Left)
                        {
                            isFinded = true;
                            MyStorage.setCurPTR();
                            break;
                        }
                        if (MyStorage.getIterator().Find(e.X, e.Y) == true && e.Button == MouseButtons.Right)
                        {
                            isFinded = true;
                            MyStorage.check();
                            break;
                        }
                    }
                }
                if (isFinded == false)
                {
                    if (radioButton3.Checked == true)
                    {
                        Random rnd = new Random();
                        int rad = rnd.Next(10, 100);
                        MyStorage.add(new Circle(e.X, e.Y, rad, Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)), pictureBox1.Width, pictureBox1.Height));
                    }
                    else
                    if (radioButton2.Checked == true)
                    {
                        Random rnd = new Random();
                        int rad = rnd.Next(10, 100);
                        MyStorage.add(new Polygon(e.X, e.Y, rad, rnd.Next(3, 9), Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)), pictureBox1.Width, pictureBox1.Height));
                        ((Polygon)MyStorage.get()).Rotate(rnd.Next(0, 180));
                    }
                }
                pictureBox1.Invalidate();
            }

            private void pictureBox1_Paint(object sender, PaintEventArgs e)
            {
                if (MyStorage.size() != 0)
                {
                    MyStorage.toFirst();
                    for (int i = 0; i < MyStorage.size(); i++, MyStorage.next())
                    {
                        MyStorage.getIterator().DrawObj(e.Graphics);
                        if (MyStorage.isChecked() == true) MyStorage.getIterator().DrawRectangle(e.Graphics, new Pen(Color.Gray, 2));
                    }
                    MyStorage.get().DrawRectangle(e.Graphics, new Pen(Color.Red, 1));
                }
            }


            private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
            {
                pictureBox1.Focus();
            }



            public class CCircle : Shape
            {
                protected Color color;
                protected int R;
                public CCircle()
                {
                    X = 0;
                    Y = 0;
                    R = 0;
                }
                public CCircle(int x, int y, int r, Color c, int width, int height)
                {

                    this.X = x;
                    this.Y = y;
                    width = Width;
                    height = Height;
                    color = c;
                    if (r > x) r = x;
                    if (x + r > width) r = width - x;
                    if (r > y) r = y;
                    if (y + r > height) r = height - y;
                    R = r;
                    rect = new Rectangle(x - R, y - R, 2 * R, 2 * R);

                }
                public override void SetXY(int x, int y)
                {
                    throw new NotImplementedException();
                }
                public void SimpleColor(Graphics g)
                {
                    Pen pen = new Pen(Color.PaleVioletRed, 5);
                    Rectangle rec = new Rectangle(X - 20, Y - 20, R, R);
                    g.DrawEllipse(pen, rec);
                }
                public void SelectedColor(Graphics g)
                {
                    Pen pen = new Pen(Color.Red, 5);
                    Rectangle rec = new Rectangle(X - 20, Y - 20, R, R);
                    g.DrawEllipse(pen, rec);
                }
                public void NewColor(Graphics g)
                {
                    Pen pen = new Pen(Color.Orange, 5);
                    Rectangle rec = new Rectangle(X - 20, Y - 20, R, R);
                    g.DrawEllipse(pen, rec);
                }

                public bool check(int x, int y)
                {
                    if (x < this.X + 30 && x > this.X - 20 && y < this.Y + 20 && y > this.Y - 20)
                        return true;
                    else
                        return false;

                }

            };


            public class MyStorage
            {
                CCircle[] circle;
                int a;
                int max_a;
                public int l;
                int[] sel;
                int selsize;

                void checkSize()
                {
                    if (a >= max_a)
                        increase();
                }
                void increase()
                {
                    CCircle[] temp = new CCircle[max_a + 10];
                    int[] tmp = new int[max_a + 10];
                    for (int i = 0; i < max_a; i++)
                    {
                        tmp[i] = sel[i];
                        temp[i] = this.circle[i];
                    }
                    max_a += 10;
                    sel = new int[max_a];
                    this.circle = new CCircle[max_a];
                    for (int i = 0; i < max_a; i++)
                    {
                        sel[i] = tmp[i];
                        this.circle[i] = temp[i];
                    }
                }
                public int getSelsize()
                {
                    return selsize;
                }
                public void decSel(int index)
                {
                    sel[index]--;
                }
                public int getSel(int index)
                {
                    return sel[index];
                }
                public void clickSel(int x, int y, Graphics g)
                {
                    bool flag = true;
                    for (int i = 0; i < a; i++)
                    {
                        if (GetObject(i).check(x, y) == true)
                        {
                            for (int j = 0; j < selsize; j++)
                            {
                                if (i == sel[j])
                                    flag = false;
                            }
                            if (flag == true)
                            {
                                sel[selsize] = i;
                                selsize++;
                                GetObject(i).SelectedColor(g);
                            }
                        }
                    }
                }
                public void iSel(int i, Graphics g)
                {

                    sel[selsize] = i;
                    selsize++;
                    GetObject(i).NewColor(g);
                }
                public void removeSel()
                {
                    for (int i = 0; i < a; i++)
                    {
                        sel[i] = -1;
                    }
                    selsize = 0;
                }

                public MyStorage()
                {
                    max_a = 0;
                    a = 0;
                    selsize = 0;
                    l = -1;
                    sel = new int[max_a];
                    circle = new CCircle[max_a];

                }
                public MyStorage(int max_a)
                {
                    a = 0;
                    selsize = 0;
                    l = -1;
                    this.max_a = max_a;
                    sel = new int[max_a];
                    circle = new CCircle[max_a];
                }
                void SetObject(int index, CCircle objects)
                {
                    if (index < max_a)
                        circle[index] = objects;
                }
                public CCircle GetObject(int index)
                {
                    return circle[index];
                }

                public int getCount()
                {
                    return a;
                }
                void move(int l)
                {
                    a++;
                    checkSize();
                    for (int i = a - 1; i > l; i--)
                    {
                        circle[i] = circle[i - 1];
                    }
                }
                public void add(int i, CCircle objects)
                {
                    move(i);
                    SetObject(i, objects);
                }

                public void remove(int index)
                {
                    if (a > 0)
                    {
                        l++;
                        for (int i = index; i < a; i++)
                        {
                            circle[i] = circle[i + 1];
                            circle[i + 1] = null;
                        }
                        a++;
                    }
                }
                public void paint(MyStorage storage, Graphics g)
                {
                    storage.removeSel();
                    for (int i = 0; i < storage.getCount() - 1; i++)
                    {
                        storage.GetObject(i).SimpleColor(g);
                    }
                    storage.GetObject(storage.getCount() - 1).SelectedColor(g);
                }

            };
            MyStorage storage = new MyStorage(0);
            private void Form1_MouseClick(object sender, MouseEventArgs e)
            {
                Graphics g = CreateGraphics();
                if (e.Button == MouseButtons.Right)
                {
                    storage.removeSel();
                    Refresh();
                    CCircle c = new CCircle(e.X, e.Y);
                    storage.l++;
                    storage.add(storage.l, c);
                    storage.paint(storage, g);
                    storage.iSel(storage.l, g);
                }
                if (e.Button == MouseButtons.Left)
                {
                    if (Control.ModifierKeys == Keys.Control)
                    {
                        storage.clickSel(e.X, e.Y, g);
                    }
                    else
                    {
                        storage.removeSel();
                        for (int i = 0; i < storage.getCount(); i++)
                        {
                            storage.GetObject(i).SimpleColor(g);
                        }
                        storage.clickSel(e.X, e.Y, g);
                    }
                }


            }

            private void Form1_KeyDown(object sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Delete)
                {
                    for (int i = 0; i < storage.getSelsize(); i++)
                    {
                        storage.remove(storage.getSel(i));
                        for (int j = i + 1; j < storage.getSelsize(); j++)
                        {
                            if (storage.getSel(j) > storage.getSel(i))
                            {
                                storage.decSel(j);
                            }
                        }
                    }
                    storage.removeSel();
                    this.Refresh();
                    Graphics g = CreateGraphics();
                    for (int i = 0; i < storage.getCount(); i++)
                    {
                        storage.GetObject(i).SimpleColor(g);
                    }
                }
            }
        }
    }
}
    
        public class ObjObserved
        {
            public Storage<Shape> storage;
            public void AddStorage(Storage<Shape> sto)
            {
                storage = sto;
            }
        }
        public class Observed
        {
            private List<Observer> observers;
            public Observed()
            {
                observers = new List<Observer>();
            }
            public void AddObserver(Observer o)
            {
                observers.Add(o);
            }
            public void Notify()
            {
                foreach (Observer observer in observers) observer.SubjectChanged();
            }
        }
