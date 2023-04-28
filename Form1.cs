using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp7
{

    public partial class Form1 : Form
    {
        Dictionary<int, Bitmap> pict = new Dictionary<int, Bitmap>();
        Dictionary<int, string> text = new Dictionary<int, string>();
        Dictionary<int, string> check = new Dictionary<int, string>();
        int i = 0;
        public Form1()
        {
            InitializeComponent();
            this.MouseWheel += new MouseEventHandler(this_MouseWheel);
        }
        void this_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                pictureBox1.Size = new Size(pictureBox1.Size.Width + 10, pictureBox1.Size.Height + 10);
            else
                pictureBox1.Size = new Size(pictureBox1.Size.Width - 10, pictureBox1.Size.Height - 10);
        }
        Image Zoom(Image image, int k)
        {
            if (k <= 1) return image;
            Bitmap img = new Bitmap(image);
            int width = img.Width;
            int height = img.Height;
            Image zoomImg = new Bitmap(width * k, height * k);
            Graphics g = Graphics.FromImage(zoomImg);

            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    Color color = img.GetPixel(i, j);
                    g.FillRectangle(new SolidBrush(color), i * k, j * k, k, k);
                }

            return zoomImg;
        }
        private Boolean Сравнить(Bitmap ImageA, Bitmap ImageB)
            {
            if (ImageA.Width == ImageB.Width && ImageA.Height == ImageB.Height)
            {
                for (int x = 0; x < ImageA.Width; x++)
                {
                    for (int y = 0; y < ImageA.Height; y++)
                    {
                                if (ImageA.GetPixel(x, y) != ImageB.GetPixel(x, y))
                                {
                                    return false;
                                }
                    }
                }
                return true;
            }
            return false;
        }
        string знач = string.Empty;
        Bitmap картинка = new Bitmap(10,10);
        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = string.Empty;
            pictureBox1.Image = null;
            label1.Visible = false;
            pictureBox1.Visible = false;
            this.Text = "Буфер";
            timer1.Interval = 200; 
            // Старт отчета времени
        }
        private void timer1_Tick(object sender, EventArgs e)
        {

            var Получатель = Clipboard.GetDataObject();
            

            if (Получатель.GetDataPresent(DataFormats.Bitmap) == true/*&& !Сравнить(картинка, (Bitmap)Получатель.GetData(DataFormats.Bitmap))*/)
            {
                картинка = (Bitmap)Получатель.GetData(DataFormats.Bitmap);
                listBox1.Items.Add(Convert.ToString(i)+".[PICTURE]");
                pict.Add(i, картинка);
                check.Add(i,"[PICTURE]");
                Clipboard.SetDataObject(0);
                i++;
            }
            
            // Функция отключена так как я сам пользуюсь этой программой для создания отчетов
            // Функция копирования текста из БО оказывается не просто не нужна, но наоборот доставляет неудобства 
            // Совсем удалять ее я пока-что не готов 
            
            // Если данные в БО представлены в текстовом формате...
            /*if (Получатель.GetDataPresent(DataFormats.Text) == true&& Получатель.GetData(DataFormats.Text).ToString()!= знач)
            {
                listBox1.Items.Add(Convert.ToString(i) + ".[TEXT]");
                знач = Получатель.GetData(DataFormats.Text).ToString();
                text.Add(i, знач);
                check.Add(i, "[TEXT]");
                Clipboard.SetDataObject(0);
                i++;
            }*/

        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (check[listBox1.SelectedIndex] == "[PICTURE]")
            {
                label1.Visible = false;
                pictureBox1.Visible = true;
                pictureBox1.Image = pict[listBox1.SelectedIndex];
            }
            if (check[listBox1.SelectedIndex] == "[TEXT]")
            {
                label1.Visible = true;
                pictureBox1.Visible = false;
                label1.Text = text[listBox1.SelectedIndex];
            }

        }
        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null && check[listBox1.SelectedIndex] == "[PICTURE]") //если в pictureBox есть изображение
            {
                SaveFileDialog savedialog = new SaveFileDialog();
                savedialog.Filter = "Image Files(*.BMP)|*.BMP|Image Files(*.JPG)|*.JPG|Image Files(*.GIF)|*.GIF|Image Files(*.PNG)|*.PNG|All files (*.*)|*.*";
                if (savedialog.ShowDialog() == DialogResult.OK) //если в диалоговом окне нажата кнопка "ОК"
                {
                    try
                    {
                        pictureBox1.Image.Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    }
                    catch
                    {
                        MessageBox.Show("Невозможно сохранить изображение", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            if (label1.Text != string.Empty && check[listBox1.SelectedIndex] == "[TEXT]")
            {
                SaveFileDialog savedialog = new SaveFileDialog();
                savedialog.Filter = "Текстовые файлы (*.txt)|*.txt|All files (*.*)|*.*";
                if (savedialog.ShowDialog() == DialogResult.OK) //если в диалоговом окне нажата кнопка "ОК"
                {
                    try
                    {
                        // Создание экземпляра StreamWriter для записи в файл:
                        var Писатель = new System.IO.StreamWriter(
                                          savedialog.FileName, false,
                                          System.Text.Encoding.GetEncoding(1251));
                        // - здесь заказ кодовой страницы Win1251 для русских букв
                        Писатель.Write(label1.Text);
                        Писатель.Close();
                    }
                    catch (Exception Ситуация)
                    {
                        // Отчет обо всех возможных ошибках
                        MessageBox.Show(Ситуация.Message, "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }

            }
        }
        private void очититьToolStripMenuItem_Click(object sender, EventArgs e)
        {
           pict = new Dictionary<int, Bitmap>();
           text = new Dictionary<int, string>();
           check = new Dictionary<int, string>();
           i = 0;
            label1.Text = string.Empty;
            pictureBox1.Image = null;
            label1.Visible = false;
            pictureBox1.Visible = false;
            listBox1.Items.Clear();
            знач = string.Empty;
            картинка = new Bitmap(10, 10);
        }
        private void завершитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;// - время пошло
            button3.Visible = false;
            button1.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;// - время остановлено
            button3.Visible = true;
            button1.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (check[listBox1.SelectedIndex] == "[PICTURE]")
            {
                Clipboard.SetDataObject(pict[listBox1.SelectedIndex]);
            }
            if (check[listBox1.SelectedIndex] == "[TEXT]")
            {
                Clipboard.SetDataObject(text[listBox1.SelectedIndex]);
            }
        }

        private void сохранитьВсеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog savedialog = new SaveFileDialog();
            savedialog.Filter = "All files (*.*)|*.*";
            if (savedialog.ShowDialog() == DialogResult.OK) //если в диалоговом окне нажата кнопка "ОК"
            {
                for (int j =0;j< listBox1.Items.Count;j++)
                {
                    listBox1.SelectedIndex = j;
                    if (check[j] == "[PICTURE]") //если в pictureBox есть изображение
                    {

                            try
                            {
                              pictureBox1.Image.Save(savedialog.FileName+j.ToString()+".png", System.Drawing.Imaging.ImageFormat.Png);

                             }
                            catch
                            {
                                MessageBox.Show("Невозможно сохранить изображение", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                    }
                }
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            pictureBox1.Size = new Size(pictureBox1.Size.Width + 10, pictureBox1.Size.Height+10);

        }

        private void button5_Click(object sender, EventArgs e)
        {
            pictureBox1.Size = new Size(pictureBox1.Size.Width - 10, pictureBox1.Size.Height - 10);
        }


    }
}
