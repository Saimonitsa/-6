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
        Storage<Shape> MyStorage = new Storage<Shape>();
        Tree tree;
        public Form1()
        {
            InitializeComponent();
            (pictureBox1 as Control).KeyPress += new KeyPressEventHandler(PressEventHandler);
            tree = new Tree(MyStorage, treeView1);
            MyStorage.AddObserver(tree);
            treeView1.CheckBoxes = true;

        }

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
                if (e.KeyChar == 122) MyStorage.prevCur();
                if (e.KeyChar == 120) MyStorage.nextCur();
                if (e.KeyChar == 8) MyStorage.del();
                if (e.KeyChar == 32)
                {
                    while (MyStorage.size() != 0) MyStorage.del();
                    pictureBox1.Invalidate();
                }



                if (e.KeyChar == 110)
                {
                    while (MyStorage.size() != 0) MyStorage.del();
                    pictureBox1.Invalidate();
                }
                if (e.KeyChar == 99)
                {

                    if (colorDialog1.ShowDialog() == DialogResult.OK) MyStorage.get().SetColor(colorDialog1.Color);
                }

                Random rnd = new Random();
                int obj = rnd.Next(1, 3);
                if (e.KeyChar == 49)
                {
                    int rad = rnd.Next(10, 100);
                    MyStorage.add(new CCircle(rnd.Next(4, pictureBox1.Width - 50), rnd.Next(4, pictureBox1.Height - 50), rad, Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)), pictureBox1.Width - 50, pictureBox1.Height - 50));
                }
                if (e.KeyChar == 51)
                {
                    int rad = rnd.Next(10, 100);
                    MyStorage.add(new Rhombus(rnd.Next(4, pictureBox1.Width - 50), rnd.Next(4, pictureBox1.Height - 50), rad, Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)), pictureBox1.Width - 50, pictureBox1.Height - 50));
                }
                if (e.KeyChar == 50)
                {
                    int rad = rnd.Next(10, 100);
                    MyStorage.add(new Triangle(rnd.Next(4, pictureBox1.Width - 50), rnd.Next(4, pictureBox1.Height - 50), rad, Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)), pictureBox1.Width - 50, pictureBox1.Height - 50));
                }

            }
            tree.Print();
            pictureBox1.Invalidate();
        }



        private void pictureBox1_Paint_1(object sender, PaintEventArgs e)
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

        private void pictureBox1_MouseDown_1(object sender, MouseEventArgs e)
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
                if (radioButton1.Checked == true)
                {
                    Random rnd = new Random();
                    int rad = rnd.Next(10, 100);
                    MyStorage.add(new CCircle(e.X, e.Y, rad, Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)), pictureBox1.Width, pictureBox1.Height));
                }
                else
                if (radioButton2.Checked == true)
                {
                    Random rnd = new Random();
                    int rad = rnd.Next(10, 100);
                    MyStorage.add(new Rhombus(e.X, e.Y, rad, Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)), pictureBox1.Width, pictureBox1.Height));
                }
                else
                if (radioButton3.Checked == true)
                {
                    Random rnd = new Random();
                    int rad = rnd.Next(10, 100);
                    MyStorage.add(new Triangle(e.X, e.Y, rad, Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)), pictureBox1.Width, pictureBox1.Height));
                }

            }
            pictureBox1.Invalidate();

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox1.Focus();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream f = new FileStream(saveFileDialog1.FileName, FileMode.Create);
                StreamWriter stream = new StreamWriter(f);
                stream.WriteLine(MyStorage.size());
                if (MyStorage.size() != 0)
                {
                    MyStorage.toFirst();
                    for (int i = 0; i < MyStorage.size(); i++, MyStorage.next()) MyStorage.getIterator().Save(stream);
                }
                stream.Close();
                f.Close();
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (MyStorage.size() != 0)
            {
                SGroup group = new SGroup(pictureBox1.Width, pictureBox1.Height);
                MyStorage.toFirst();
                int cnt = 0;
                for (int i = 0; i < MyStorage.size(); i++, MyStorage.next()) //считаем количество отмеченных элементов
                    if (MyStorage.isChecked() == true) cnt++;
                while (cnt != 0)
                {
                    if (MyStorage.isChecked() == true)
                    {
                        group.Add(MyStorage.getIterator());
                        MyStorage.delIterator();
                        cnt--;
                    }
                    if (MyStorage.size() != 0) MyStorage.next();
                }
                MyStorage.add(group);
            }
            pictureBox1.Invalidate();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            while (MyStorage.size() != 0) MyStorage.del();
            pictureBox1.Invalidate();
        }

        private void button5_Click(object sender, EventArgs e)
        {

                if (MyStorage.size() != 0 && MyStorage.get() is SGroup)
                {
                    while (((SGroup)MyStorage.get()).size() != 0)
                    {
                    MyStorage.addAfter(((SGroup)MyStorage.get()).Out());
                    MyStorage.prevCur();
                    }
                MyStorage.del();
                }
                pictureBox1.Invalidate();
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream f = new FileStream(openFileDialog1.FileName, FileMode.Open);
                StreamReader stream = new StreamReader(f);
                int i = Convert.ToInt32(stream.ReadLine());
                Factory shapeFactory = new ShapeFactory();  //фабрика КОНКРЕТНЫХ объектов
                for (; i > 0; i--)
                {
                    string tmp = stream.ReadLine();
                    MyStorage.add(shapeFactory.createShape(tmp));
                    MyStorage.get().Load(stream);
                }
                stream.Close();
                f.Close();
            }
            pictureBox1.Invalidate();
            tree.Print();

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if ((e.Action == TreeViewAction.ByKeyboard || e.Action == TreeViewAction.ByMouse) && e.Node.Text != "Фигуры")
            {


                TreeNode tmp = e.Node;

                while (tmp.Parent.Text != "Фигуры") tmp = tmp.Parent;
                treeView1.SelectedNode = tmp;
                MyStorage.toFirst();
                MyStorage.setCurPTR();
                for (int i = 0; i < tmp.Index; i++)
                {

                    MyStorage.nextCur();
                }
                MyStorage.setCurPTR();
            }


        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (MyStorage.size() != 0)
                MyStorage.del();
            pictureBox1.Invalidate();

        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode tmp = e.Node;

            if (e.Button == MouseButtons.Right)
            {
                MyStorage.toFirst();
                MyStorage.setCurPTR();

                for (int i = 0; i < tmp.Index; i++)
                {
                    MyStorage.nextCur();
                }
                if (MyStorage.size() != 0 && MyStorage.get() is SGroup)
                {
                    while (((SGroup)MyStorage.get()).size() != 0)
                    {
                        MyStorage.addAfter(((SGroup)MyStorage.get()).Out());
                        MyStorage.prevCur();
                    }
                    MyStorage.del();
                }
            }
            if (e.Button == MouseButtons.Left)
            {

                treeView1.SelectedNode = tmp;

                MyStorage.toFirst();
                for (int i = 0; i <= treeView1.SelectedNode.Index; i++)
                {
                    MyStorage.next();
                }
                MyStorage.check();

            }
            pictureBox1.Invalidate();

        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            TreeNode tmp = e.Node;
            treeView1.SelectedNode = tmp;

            MyStorage.toFirst();

            for (int i = 0; i < treeView1.SelectedNode.Index; i++)
            {
                MyStorage.next();
            }
            MyStorage.setCurPTR();
            MyStorage.del();


            pictureBox1.Invalidate();


        }
    }

};
public abstract class Shape : ObjObserved
{
    protected Rectangle rect; //область объекта для его отрисовки и выделения
    protected int X, Y, width, height; //x, y для позиции объектов и групп, w и h для учитывания границ отрисовки
    public bool sticky = false;
    abstract public void Resize();
    abstract public void SetXY(int _x, int _y);
    abstract public void OffsetXY(int _x, int _y);
    abstract public void SetColor(Color c);
    abstract public void Grow(int gr);
    abstract public void DrawObj(System.Drawing.Graphics e);
    abstract public void DrawRectangle(System.Drawing.Graphics e, Pen pen);
    abstract public bool Find(int _x, int _y);
    abstract public void Clone();
    abstract public Rectangle GetRectangle();  //получить границы фигуры для контроля выхода за пределы
    abstract public void Save(StreamWriter stream);
    abstract public void Load(StreamReader stream);
    abstract public string GetInfo();

}

public abstract class Factory
{
    public abstract Shape createShape(string name);
}

public class ShapeFactory : Factory
{
    public override Shape createShape(string name)
    {
        Shape shape;
        switch (name)
        {
            case "Circle":
                shape = new CCircle();
                break;
            case "Group":
                shape = new SGroup();
                break;
            case "Rhombus":
                shape = new Rhombus();
                break;
            case "Triangle":
                shape = new Triangle();
                break;
            default:
                shape = null;
                break;
        }
        return shape;
    }
}

public class SGroup : Shape
{
    public Storage<Shape> storage;

    public SGroup()
    {
        storage = new Storage<Shape>();
        name = "Group";
    }

    public SGroup(int Width, int Height)
    {
        width = Width;
        height = Height;
        storage = new Storage<Shape>();
        name = "Group";
    }

    public void Add(Shape s)
    {
        storage.add(s);
        storage.get().sticky = false;
        if (storage.size() == 1) rect = new Rectangle(s.GetRectangle().X, s.GetRectangle().Y, s.GetRectangle().Width, s.GetRectangle().Height);
        else
        {
            if (s.GetRectangle().Left < rect.Left)
            {
                int tmp = rect.Right;
                rect.X = s.GetRectangle().Left;
                rect.Width = tmp - rect.X;
            }
            if (s.GetRectangle().Right > rect.Right) rect.Width = s.GetRectangle().Right - rect.X;
            if (s.GetRectangle().Top < rect.Top)
            {
                int tmp = rect.Bottom;
                rect.Y = s.GetRectangle().Top;
                rect.Height = tmp - rect.Y;
            }
            if (s.GetRectangle().Bottom > rect.Bottom) rect.Height = s.GetRectangle().Bottom - rect.Y;
        }
    }

    public Shape Out()
    {
        if (storage.size() != 0)
        {
            Shape tmp = storage.get();
            storage.del();
            Resize();
            return tmp;
        }
        return null;
    }

    public int size()
    {
        return storage.size();
    }

    public override void Resize()
    {
        if (storage.size() != 0)
        {
            storage.toFirst();
            rect = storage.getIterator().GetRectangle();
            for (int i = 0; i < storage.size(); i++, storage.next())
            {
                if (storage.getIterator().GetRectangle().Left < rect.Left)
                {
                    int tmp = rect.Right;
                    rect.X = storage.getIterator().GetRectangle().Left;
                    rect.Width = tmp - rect.X;
                }
                if (storage.getIterator().GetRectangle().Right > rect.Right) rect.Width = sto.getIterator().GetRectangle().Right - rect.X;
                if (storage.getIterator().GetRectangle().Top < rect.Top)
                {
                    int tmp = rect.Bottom;
                    rect.Y = storage.getIterator().GetRectangle().Top;
                    rect.Height = tmp - rect.Y;
                }
                if (storage.getIterator().GetRectangle().Bottom > rect.Bottom) rect.Height = sto.getIterator().GetRectangle().Bottom - rect.Y;
            }
        }
    }

    public override void Clone()
    {
        throw new NotImplementedException();
    }

    public override void DrawObj(Graphics e)
    {
        if (storage.size() != 0)
        {
            storage.toFirst();
            for (int i = 0; i < storage.size(); i++, storage.next()) storage.getIterator().DrawObj(e);
        }
    }

    public override void Grow(int gr)
    {
        if (storage.size() != 0)
        {
            if (gr > 0 && rect.X + gr > 1 && gr + rect.Right < width - 1 && rect.Y + gr > 1 && gr + rect.Bottom < height - 1)
            {
                rect.X -= gr;
                rect.Y -= gr;
                rect.Width += 2 * gr;
                rect.Height += 2 * gr;
                storage.toFirst();
                for (int i = 0; i < storage.size(); i++, storage.next()) storage.getIterator().Grow(gr);
            }
            if (gr < 0)
            {
                storage.toFirst();
                for (int i = 0; i < storage.size(); i++, storage.next()) storage.getIterator().Grow(gr);
                if (gr < 0) Resize();
            }
        }
    }

    public override void OffsetXY(int _x, int _y)
    {
        if (storage.size() != 0)
        {
            if (rect.X + _x > 0 && _x + rect.Right < width)
            {
                rect.X += _x;
                storage.toFirst();
                for (int i = 0; i < storage.size(); i++, storage.next()) storage.getIterator().OffsetXY(_x, 0);
            }
            if (rect.Y + _y > 0 && _y + rect.Bottom < height)
            {
                rect.Y += _y;
                storage.toFirst();
                for (int i = 0; i < storage.size(); i++, storage.next()) storage.getIterator().OffsetXY(0, _y);
            }
        }
    }

    public override void SetColor(Color c)
    {
        if (storage.size() != 0)
        {
            storage.toFirst();
            for (int i = 0; i < storage.size(); i++, storage.next()) storage.getIterator().SetColor(c);
        }
    }

    public override void SetXY(int _x, int _y)
    {
        throw new NotImplementedException();
    }

    public override Rectangle GetRectangle()
    {
        return rect;
    }

    public override bool Find(int _x, int _y)
    {
        if (rect.X < _x && _x < rect.Right && rect.Y < _y && _y < rect.Bottom) return true; else return false;
    }

    public override void DrawRectangle(Graphics e, Pen pen)
    {
        e.DrawRectangle(pen, rect);
    }

    public override void Save(StreamWriter stream)
    {
        stream.WriteLine("Group");
        stream.WriteLine(storage.size() + " " + width + " " + height);
        if (storage.size() != 0)
        {
            storage.toFirst();
            for (int i = 0; i < storage.size(); i++, storage.next()) storage.getIterator().Save(stream);
        }
    }

    public override void Load(StreamReader stream)
    {
        ShapeFactory factory = new ShapeFactory();
        string[] data = stream.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        int i = Convert.ToInt32(data[0]);
        width = Convert.ToInt32(data[1]);
        height = Convert.ToInt32(data[2]);
        for (; i > 0; i--)
        {
            Shape tmp = factory.createShape(stream.ReadLine());
            tmp.Load(stream);
            Add(tmp);
        }
    }

    public override string GetInfo()
    {
        return name + "    Size : " + storage.size().ToString();
    }

    public override void Rotate(int gr)
    {
        if (storage.size() != 0)
        {
            storage.toFirst();
            for (int i = 0; i < storage.size(); i++, storage.next()) storage.getIterator().Rotate(gr);
        }
    }

    public override bool Find(Shape obj)
    {
        if (storage.size() != 0)
        {
            storage.toFirst();
            for (int i = 0; i < storage.size(); i++, storage.next()) if (storage.getIterator().Find(obj) == true) return true;
        }
        return false;
    }
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
        name = "Circle";
    }
    public CCircle(int x, int y, int r, Color c, int Width, int Height)
    {

        this.X = x;
        this.Y = y;
        width = Width;
        height = Height;
        name = "Circle";
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
    public override void Resize()
    {
        rect = new Rectangle(X - R, Y - R, 2 * R, 2 * R);
    }

    public override void Grow(int inc)
    {
        if (R + inc < X && X + R + inc < width && R + inc < Y && Y + R + inc < height && R + inc > 0) R += inc;
        Resize();
    }

    public override void SetColor(Color c)
    {
        color = c;
    }

    public override void DrawObj(Graphics e)
    {
        e.DrawEllipse(new Pen(Color.Black, 2), rect);
        e.FillEllipse(new SolidBrush(color), rect);
    }

    public override void DrawRectangle(Graphics e, Pen pen)
    {
        e.DrawRectangle(pen, rect);
    }

    public override void OffsetXY(int _x, int _y)
    {
        if (storage != null && storage.size() != 0 && sticky == true)
        {
            storage.toFirst();
            for (int i = 0; i < storage.size(); i++, storage.next())
            {
                if (Find(storage.getIterator()) == true && storage.getIterator() != this)
                {
                    if (storage.getIterator().sticky == false)
                        storage.getIterator().OffsetXY(_x, _y);
                }
            }
        }

        if (X + _x > R && X + _x + R < width) X += _x;
        if (Y + _y > R && Y + _y + R < height) Y += _y;
        Resize();
    }
    public override Rectangle GetRectangle()
    {
        return rect;
    }

    public override bool Find(int _x, int _y)
    {
        if (Math.Pow(X - _x, 2) + Math.Pow(Y - _y, 2) <= R * R) return true; else return false;
    }
}
public class Rhombus : CCircle
{
    private int n = 4;
    List<PointF> first;

    public Rhombus(int x, int y, int r, Color c, int Width, int Height) : base(x, y, r, c, Width, Height)
    {

        if (r > x) r = x;
        if (x + r > width) r = width - x;
        if (r > y) r = y;
        if (y + r > height) r = height - y;
        Resize();
    }

    public override void DrawObj(Graphics e)
    {
        e.FillPolygon(new SolidBrush(color), first.ToArray());
    }

    public override void DrawRectangle(Graphics e, Pen pen)
    {
        e.DrawRectangle(pen, rect);
    }

    public override Rectangle GetRectangle()
    {
        return rect;
    }

    public override void Grow(int inc)
    {
        if (R + inc < X && X + R + inc < width && R + inc < Y && Y + R + inc < height && R + inc > 0) R += inc;
        Resize();
    }

    public override void OffsetXY(int _x, int _y)
    {
        if (X + _x > R && X + _x + R < width) X += _x;
        if (Y + _y > R && Y + _y + R < height) Y += _y;
        Resize();
    }

    public override void Resize()
    {
        first = null;
        first = new List<PointF>();
        for (int i = 0; i < 360; i += 360 / n)
        {
            double radiani = (double)(i * 3.14) / 180;
            float xx = X + (int)(R * Math.Cos(radiani));
            float yy = Y + (int)(R * Math.Sin(radiani));
            first.Add(new PointF(xx, yy));
        }
        rect = new Rectangle(X - R, Y - R, 2 * R, 2 * R);
    }
    public override void Clone()
    {
        throw new NotImplementedException();
    }

    public override void SetColor(Color c)
    {
        color = c;
    }

    public override void SetXY(int _x, int _y)
    {

    }

    public override bool Find(int _x, int _y)
    {
        if (Math.Pow(X - _x, 2) + Math.Pow(Y - _y, 2) <= R * R) return true; else return false;
    }
    public override void Save(StreamWriter stream)
    {
        stream.WriteLine("Circle");
        stream.WriteLine((rect.X + R) + " " + (rect.Y + R) + " " + R + " " + color.R + " " + color.G + " " + color.B + " " + width + " " + height);
    }

    public override void Load(StreamReader stream)
    {
        string[] data = stream.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        X = Convert.ToInt32(data[0]);
        Y = Convert.ToInt32(data[1]);
        R = Convert.ToInt32(data[2]);
        color = Color.FromArgb(Convert.ToInt32(data[3]), Convert.ToInt32(data[4]), Convert.ToInt32(data[5]));
        width = Convert.ToInt32(data[6]);
        height = Convert.ToInt32(data[7]);
        Resize();
    }

    public override string GetInfo()
    {
        return name + "  X: " + X + " Y: " + Y + " Rad: " + R + " " + color.ToString();
    }

    public override bool Find(Shape obj)
    {
        string[] data = obj.GetInfo().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (data[0] != "Group")
        {
            int _x = Convert.ToInt32(data[2]);
            int _y = Convert.ToInt32(data[4]);
            int _rad = Convert.ToInt32(data[6]);
            if (Math.Pow(X - _x, 2) + Math.Pow(Y - _y, 2) <= Math.Pow(R + _rad, 2)) return true;
        }
        else return obj.Find(this); //просим группу поискать нас
        return false;
    }

}

public class Triangle : CCircle
{
    private int n = 3;
    private int rotate = 0;
    List<PointF> first;
    public Triangle() : base()
    {
        name = "Triangle";
    }

    public Triangle(int x, int y, int r, int n, Color c, int Width, int Height) : base(x, y, r, c, Width, Height)
    {
        this.n = n;
        if (r > x) r = x;
        if (x + r > width) r = width - x;
        if (r > y) r = y;
        if (y + r > height) r = height - y;
        Resize();
        name = "Triangle";
    }
    public override void Clone()
    {
        throw new NotImplementedException();
    }

    public override void DrawObj(Graphics e)
    {
        e.DrawPolygon(new Pen(Color.Black, 2), first.ToArray());
        e.FillPolygon(new SolidBrush(color), first.ToArray());
    }

    public override void DrawRectangle(Graphics e, Pen pen)
    {
        e.DrawRectangle(pen, rect);
    }

    public override Rectangle GetRectangle()
    {
        return rect;
    }
    public override bool Find(int _x, int _y)
    {
        if (rect.X < _x && _x < rect.Right && rect.Y < _y && _y < rect.Bottom) return true; else return false;
    }
    public override void Load(StreamReader stream)
    {
        string[] data = stream.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        X = Convert.ToInt32(data[0]);
        Y = Convert.ToInt32(data[1]);
        R = Convert.ToInt32(data[2]);
        n = Convert.ToInt32(data[3]);
        rotate = Convert.ToInt32(data[4]);
        color = Color.FromArgb(Convert.ToInt32(data[5]), Convert.ToInt32(data[6]), Convert.ToInt32(data[7]));
        width = Convert.ToInt32(data[8]);
        height = Convert.ToInt32(data[9]);
        Resize();
    }

    public override void Grow(int inc)
    {
        if (R + inc < X && X + R + inc < width && R + inc < Y && Y + R + inc < height && R + inc > 0) R += inc;
        Resize();
    }

    public override void OffsetXY(int _x, int _y)
    {
        if (storage != null && storage.size() != 0 && sticky == true)
        {
            storage.toFirst();
            for (int i = 0; i < storage.size(); i++, storage.next())
            {
                if (Find(storage.getIterator()) == true && storage.getIterator() != this)
                {
                    if (storage.getIterator().sticky == false)
                        storage.getIterator().OffsetXY(_x, _y);
                }
            }
        }

        if (X + _x > R && X + _x + R < width) X += _x;
        if (Y + _y > R && Y + _y + R < height) Y += _y;
        Resize();
    }

    public override void Save(StreamWriter stream)
    {
        stream.WriteLine("Triangle");
        stream.WriteLine(X + " " + Y + " " + R + " " + n + " " + rotate + " " + color.R + " " + color.G + " " + color.B + " " + width + " " + height);
    }
    public override string GetInfo()
    {
        return name + "  X: " + X + " Y: " + Y + " Rad: " + R + " N: " + n + " " + color.ToString();
    }

    public override void Rotate(int gr)
    {
        rotate += gr;
        Resize();
    }
    public void growN(int gr)
    {
        if (n + gr > 2) n += gr;
        Resize();
    }


    public override void Resize()
    {
        first = null;
        first = new List<PointF>();
        for (int i = rotate; i < rotate + 360; i += 360 / n)
        {
            double radiani = (double)(i * 3.14) / 180;
            float xx = X + (int)(R * Math.Cos(radiani));
            float yy = Y + (int)(R * Math.Sin(radiani));
            first.Add(new PointF(xx, yy));
        }
        rect = new Rectangle(X - R, Y - R, 2 * R, 2 * R);
    }

    public override void SetColor(Color c)
    {
        color = c;
    }

    public override void SetXY(int _x, int _y)
    {

    }

    public override bool Find(int _x, int _y)
    {
        if (rect.X < _x && _x < rect.Right && rect.Y < _y && _y < rect.Bottom) return true; else return false;
    }
}



public class ObjObserved
{
    public Storage<Shape> storage;
    public void AddStorage(Storage<Shape> MyStorage)
    {
        storage = MyStorage;
    }
}


public class Storage<MStorage>
{
    public class list
    {
        public MStorage data { get; set; }
        public list right { get; set; }
        public list left { get; set; }
        public bool isChecked = false;
    };
    private list first;
    private list last;
    private list current;
    private list iterator;

    private int rate;
    public Storage()
    {
        first = null;
        rate = 0;
    }
    public void add(MStorage figure)
    {
        list tmp = new list();
        tmp.data = figure;
        if (first != null)
        {
            tmp.left = last;
            last.right = tmp;
            last = tmp;
        }
        else
        {
            first = tmp;
            last = first;
            current = first;
        }
        last.right = first;
        current = tmp;
        first.left = last;
        rate++;
    }
    public void addBefore(MStorage figure)
    {
        list tmp = new list();
        tmp.data = figure;
        if (first != null)
        {
            tmp.left = (current.left);
            (current.left).right = tmp;
            current.left = tmp;
            tmp.right = current;
            if (current == first) first = current.left;
        }
        else
        {
            first = tmp;
            last = first;
            current = first;
            first.right = first;
            first.left = first;
        }
        current = tmp;
        rate++;
    }
    public void addAfter(MStorage figure)
    {
        list tmp = new list();
        tmp.data = figure;
        if (first != null)
        {
            tmp.left = current;
            tmp.right = current.right;
            (current.right).left = tmp;
            current.right = tmp;
            if (current == last) last = current.right;
        }
        else
        {
            first = tmp;
            last = first;
            current = first;
            first.right = first;
            first.left = first;
        }
        current = tmp;
        rate++;
    }
    public void toFirst()
    {
        iterator = first;
    }
    public void toLast()
    {
        iterator = last;
    }
    public void next()
    {
        iterator = iterator.right;
    }
    public void prev()
    {
        iterator = iterator.left;
    }
    public void nextCur()
    {
        current = current.right;
    }
    public void prevCur()
    {
        current = current.left;
    }
    public void del()
    {
        if (rate == 1)
        {
            first = null;
            last = null;
            current = null;
        }
        else
        {
            (current.left).right = current.right;
            (current.right).left = current.left;
            list tmp = current;
            if (current == last)
            {
                current = current.left;
                last = current;
            }
            else
            {
                if (current == first) first = current.right;
                current = current.right;
            }
        }
        rate--;
    }
    public void delIterator()
    {
        if (rate == 1)
        {
            first = null;
            last = null;
            iterator = null;
        }
        else
        {
            (iterator.left).right = iterator.right;
            (iterator.right).left = iterator.left;
            if (iterator == last)
            {
                iterator = iterator.left;
                last = iterator;
            }
            else
            {
                if (iterator == first) first = iterator.right;
                iterator = iterator.left;
            }
        }
        rate--;
    }
    public int size()
    {
        return rate;
    }
    public list getIteratorPTR()
    {
        return iterator;
    }
    public list getCurPTR()
    {
        return current;
    }
    public void setCurPTR()
    {
        current = iterator;
    }
    public bool isChecked()
    {
        if (iterator.isChecked == true) return true; else return false;
    }
    public void check()
    {
        iterator.isChecked = !iterator.isChecked;
    }
    public MStorage getIterator()
    {
        return (iterator.data);
    }
    public MStorage get()
    {
        return (current.data);
    }
}

// для лаб.работы 7 и 8 


public class Watch
{
    public virtual void SubjectChanged() { return; }
}

class Tree : Watch
{
    private Storage<Shape> sto;
    private TreeView tree;
    public Tree(Storage<Shape> sto, TreeView tree)
    {
        this.sto = sto;
        this.tree = tree;
    }

    public void Print()
    {
        tree.Nodes.Clear();
        if (sto.size() != 0)
        {
            int SelectedIndex = 0;
            TreeNode start = new TreeNode("Фигуры");
            sto.toFirst();
            for (int i = 0; i < sto.size(); i++, sto.next())
            {
                if (sto.getCurPTR() == sto.getIteratorPTR()) SelectedIndex = i;
                PrintNode(start, sto.getIterator());
            }
            tree.Nodes.Add(start);

            for (int i = 0; i < sto.size(); i++)
            {
                sto.next();
                tree.SelectedNode = tree.Nodes[0].Nodes[i];
                if (sto.isChecked() == true)
                    tree.SelectedNode.ForeColor = Color.Red;
                else tree.SelectedNode.ForeColor = Color.Black;
            }
        }
        tree.ExpandAll();

    }
    private void PrintNode(TreeNode node, Shape shape)
    {
        if (shape is SGroup)
        {
            TreeNode tn = new TreeNode(shape.GetInfo());
            if (((SGroup)shape).sto.size() != 0)
            {
                ((SGroup)shape).sto.toFirst();
                for (int i = 0; i < ((SGroup)shape).sto.size(); i++, ((SGroup)shape).sto.next())
                    PrintNode(tn, ((SGroup)shape).sto.getIterator());
            }
            node.Nodes.Add(tn);
        }
        else
        {

            node.Nodes.Add(shape.GetInfo());
        }
    }


    public override void SubjectChanged()
    {
        Print();
    }
}
