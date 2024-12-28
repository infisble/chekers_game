using System;
using System.Numerics;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WinFormsApp11
{
    public partial class Form1 : Form
    {
        bool try_to_beat = false;
        int p = 0;
        int on_clicked;
        bool try_to_qwn_beat = false;
        int player;
        Image image_b;
        Image image_w;
        int[,] map = new int[8, 8] {
                { 0,1,0,1,0,1,0,1 },
                { 1,0,1,0,1,0,1,0 },
                { 0,1,0,1,0,1,0,1 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 2,0,2,0,2,0,2,0 },
                { 0,2,0,2,0,2,0,2 },
                { 2,0,2,0,2,0,2,0 }
            };
        public Form1()
        {
            InitializeComponent();

            player = 1;
            of_all();
            on_all();
            int a = 0;
            bool ch = false;

            for (int i = 0; i <= 63; i = i + 1)
            {
                var button = Controls.Find("button" + i, true).FirstOrDefault() as Button;
                button.Click += new EventHandler(my_hid);
            }
            for (int i = 0; i <= 63; i = i + 2)
            {
                a++;
                var button = Controls.Find("button" + i, true).FirstOrDefault() as Button;

                button.BackColor = Color.Gray;
                if (a == 4)
                {
                    if (!ch)
                    {
                        i++;
                        ch = true;
                    }
                    else
                    {
                        i--;
                        ch = false;
                    }
                    a = 0;

                }


            }

        }

        private void clear_table()
        {
            for (int i = 0; i <= 63; i = i + 1)
            {
                var button = Controls.Find("button" + i, true).FirstOrDefault() as Button;

                button.BackColor = Color.White;
            }
            int a = 0;
            bool ch = false;
            for (int i = 0; i <= 63; i = i + 2)
            {
                a++;
                var button = Controls.Find("button" + i, true).FirstOrDefault() as Button;

                button.BackColor = Color.Gray;
                if (a == 4)
                {
                    if (!ch)
                    {
                        i++;
                        ch = true;
                    }
                    else
                    {
                        i--;
                        ch = false;
                    }
                    a = 0;

                }
            }
        }
        private void is_white()
        {


            int index = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (map[i, j] != 1 || map[i, j] != 3)
                    {
                        index = i * 8 + j;
                        var button = Controls.Find("button" + index, true).FirstOrDefault() as Button;
                        button.Enabled = false;
                    }
                }
            }

        }
        private void is_black()
        {
            int index = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (map[i, j] != 2 || map[i, j] != 4)
                    {
                        index = i * 8 + j;
                        var button = Controls.Find("button" + index, true).FirstOrDefault() as Button;
                        button.Enabled = false;
                    }
                }
            }
        }
        private void on_all()
        {
            int ii, jj;
            for (int i = 0; i <= 63; i = i + 1)
            {
                ii = i / 8;
                jj = i % 8;
                var button = Controls.Find("button" + i, true).FirstOrDefault() as Button;
                if (map[ii, jj] == 3 && player == 1)
                {
                    button.Enabled = true;
                }
                if (map[ii, jj] == 4 && player == 2)
                {
                    button.Enabled = true;
                }
                if (map[ii, jj] == player)
                {
                    button.Enabled = true;
                }

            }
        }
        private void of_all()
        {
            for (int i = 0; i <= 63; i = i + 1)
            {

                var button = Controls.Find("button" + i, true).FirstOrDefault() as Button;

                button.Enabled = false;

            }
        }
        private void hide_all()
        {
            for (int i = 0; i <= 63; i++)
            {
                var button = Controls.Find("button" + i, true).FirstOrDefault() as Button;
                if (button != null)
                {
                    button.BackColor = SystemColors.Control;
                }
            }
        }
        private void set_hid_white(int ii, int jj, int num)
        {
            if (jj - 1 != -1 && num <= 63 && num >= 0)
            {

                var button = Controls.Find("button" + num, true).FirstOrDefault() as Button;

                button.BackColor = Color.White;
                button.Enabled = false;
                num = (ii + 1) * 8 + (jj + 1);


            }
            if (jj + 1 != 8 && num <= 63 && num >= 0)
            {
                var button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                button.BackColor = Color.White;
                button.Enabled = false;
            }
            on_all();
        }
        private void set_hid_blask(int ii, int jj, int num)
        {
            if (jj - 1 != -1 && num <= 63 && num >= 0)
            {
                var button = Controls.Find("button" + num, true).FirstOrDefault() as Button;

                button.BackColor = Color.White;
                button.Enabled = false;
                num = (ii - 1) * 8 + (jj + 1);

            }
            if (jj + 1 != 8 && num <= 63 && num >= 0)
            {
                var button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                button.BackColor = Color.White;
                button.Enabled = false;
            }
            on_all();
        }

        private void my_hid(object sender, EventArgs e)
        {


            on_clicked = p;
            for (int i = 0; i <= 63; i = i + 1)
            {
                var button = Controls.Find("button" + i, true).FirstOrDefault() as Button;
                if (button == sender)
                {
                    p = i;
                    break;
                }
            }

            int ii = p / 8, jj = p % 8;
           // label1.Text = map[ii, jj].ToString();
            if (map[ii, jj] == 3 || map[ii, jj] == 4)
            {

                int iii = ii, jjj = jj;
                can_quween_eat(ii, jj);
                if (!try_to_qwn_beat)
                {
                    while (iii >0 && jjj < 7)
                    {
                        iii--;
                        jjj++;
                        if (map[iii, jjj] != 0)
                        {
                            break;
                        }

                        int num = (iii) * 8 + jjj;
                        if (num <= 63 && num >= 0 && map[iii, jjj] != 1 && map[iii, jjj] != 2 && map[iii, jjj] != 3 && map[iii, jjj] != 4)
                        {
                            var button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                            button.Enabled = true;
                            button.BackColor = Color.Yellow;
                        }
                        
                    }
                    iii = ii;
                    jjj = jj;
                    while (iii >0 && jjj > 0)
                    {
                        iii--;
                        jjj--;
                        if (map[iii, jjj] != 0)
                        {
                            break;
                        }

                        int num = (iii) * 8 + jjj;
                        if (num <= 63 && num >= 0 && map[iii, jjj] != 1 && map[iii, jjj] != 2 && map[iii, jjj] != 3 && map[iii, jjj] != 4)
                        {
                            var button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                            button.Enabled = true;
                            button.BackColor = Color.Yellow;
                        }
                       
                    }
                    iii = ii;
                    jjj = jj;
                    while (iii <7 && jjj > 0)
                    {
                        iii++;
                        jjj--;
                        if (map[iii, jjj] != 0)
                        {
                            break;
                        }

                        int num = (iii) * 8 + jjj;
                        if (num <= 63 && num >= 0 && map[iii, jjj] != 1 && map[iii, jjj] != 2 && map[iii, jjj] != 3 && map[iii, jjj] != 4)
                        {
                            var button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                            button.Enabled = true;
                            button.BackColor = Color.Yellow;
                        }
                       
                    }
                    iii = ii;
                    jjj = jj;
                    while (iii <7 && jjj < 7)
                    {
                        iii++;
                        jjj++;
                        if (map[iii, jjj] != 0)
                        {
                            break;
                        }

                        int num = (iii) * 8 + jjj;
                        if (num <= 63 && num >= 0 && map[iii, jjj] != 1 && map[iii, jjj] != 2 && map[iii, jjj] != 3 && map[iii, jjj] != 4)
                        {
                            var button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                            button.Enabled = true;
                            button.BackColor = Color.Yellow;
                        }
                       
                    }
                }
             
            }
            else
            if (map[ii, jj] == 0)
            {
                int hi = on_clicked / 8, hj = on_clicked % 8;
                if (map[hi, hj] == 3)
                {

                }
                is_queen(p);
                clear_table();

                if (try_to_beat)
                {
                    var button3 = Controls.Find("button" + p, true).FirstOrDefault() as Button;
                    button3.Enabled = false;
                    MyChange(on_clicked, p, player);

                    int iii = (ii + on_clicked / 8) / 2;
                    int jjj = (jj + on_clicked % 8) / 2;
                    map[iii, jjj] = 0;
                    int numbers = (iii) * 8 + jjj;
                    var buttons = Controls.Find("button" + numbers, true).FirstOrDefault() as Button;
                    buttons.Image = null;



                    if (first_eat(p, ii, jj, player) == true)
                    {
                        var button2 = Controls.Find("button" + on_clicked, true).FirstOrDefault() as Button;
                        button2.Image = null;
                      button2 = Controls.Find("button" + p, true).FirstOrDefault() as Button;
                        if (player == 1)
                            button2.Image = Properties.Resources.white;
                        else
                            button2.Image = Properties.Resources.black;
                        return;


                    }
                }
                var button = Controls.Find("button" + on_clicked, true).FirstOrDefault() as Button;
                button.Image = null;
                MyChange(on_clicked, p, player);


                button = Controls.Find("button" + p, true).FirstOrDefault() as Button;
                if (player == 1)
                    button.Image = Properties.Resources.white;
                else
                    button.Image = Properties.Resources.black;
                if (player == 1) { player = 2; }
                else
                {
                    player = 1;
                }

                of_all();
                on_all();
                int num = (ii) * 8 + (jj);
                button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                button.BackColor = Color.White;
                button.Enabled = false;
                if (jj + 2 <= 7)
                {
                    num = (ii) * 8 + (jj + 2);
                    button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                    button.BackColor = Color.White;
                    button.Enabled = false;

                }
                if (jj - 2 >= 0)
                {
                    num = (ii) * 8 + (jj - 2);
                    button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                    button.BackColor = Color.White;
                    button.Enabled = false;

                }
                on_all();
            }
            else
            {
                if (p == on_clicked)
                {
                    on_all();

                    if (player == 1)
                    {
                        int num = (ii + 1) * 8 + (jj - 1);
                        set_hid_white(ii, jj, num);
                        p = 1;
                    }
                  else
                    {
                        int num = (ii - 1) * 8 + (jj - 1);
                        set_hid_blask(ii, jj, num);
                        p = 1;
                    }

                }
                else
                {
                    of_all();

                    var button1 = Controls.Find("button" + p, true).FirstOrDefault() as Button;
                    button1.Enabled = true;
                    //label1.Text = ii.ToString() + jj.ToString();



                    if (map[ii, jj] == 1)
                    {

                        if (jj - 1 != -1 && ii != 7)
                        {

                            if (map[ii + 1, jj - 1] != 1 && map[ii + 1, jj - 1] != 2)
                            {
                                int num = (ii + 1) * 8 + (jj - 1);

                                var button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                                button.BackColor = Color.Yellow;
                                button.Enabled = true;

                            }
                        }
                        if (jj + 1 != 8 && ii != 7)
                        {
                            if (map[ii + 1, jj + 1] != 1 && map[ii + 1, jj + 1] != 2)
                            {
                                int num = (ii + 1) * 8 + (jj + 1);

                                var button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                                button.BackColor = Color.Yellow;
                                button.Enabled = true;
                            }
                        }
                    }


                    if (map[ii, jj] == 2)
                    {
                        // label1.Text = ii.ToString() + jj.ToString();


                        if (jj != 0 && ii != 0)
                        {

                            if (map[ii - 1, jj - 1] != 1 && map[ii - 1, jj - 1] != 2)
                            {
                                int num = (ii - 1) * 8 + (jj - 1);

                                var button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                                button.BackColor = Color.Yellow;
                                button.Enabled = true;
                            }
                        }
                        if (jj != 7 && ii != 0)
                        {
                            if (map[ii - 1, jj + 1] != 1 && map[ii - 1, jj + 1] != 2)
                            {
                                int num = (ii - 1) * 8 + (jj + 1);

                                var button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                                button.BackColor = Color.Yellow;
                                button.Enabled = true;
                            }
                        }
                    }









                }



                /// 


            }
            can_eat();
            first_eat(p, ii, jj, player);
        }





        private void MyChange(int a, int chislo, int playerred)
        {
            int ii = a / 8, jj = a % 8;
            if (playerred == 1 && map[ii, jj] == 3)
            {
                int iii = chislo / 8;
                int jjj = chislo % 8;
                map[iii, jjj] = 3;
                int num1 = (iii) * 8 + (jjj);
                var button = Controls.Find("button" + num1, true).FirstOrDefault() as Button;
                button.Text = "Q";
            }
            if (playerred == 2 && map[ii, jj] == 4)
            {
                int iii = chislo / 8;
                int jjj = chislo % 8;
                map[iii, jjj] = 4;
                int num1 = (iii) * 8 + (jjj);
                var button = Controls.Find("button" + num1, true).FirstOrDefault() as Button;
                button.Text = "Q";
            }
            map[ii, jj] = 0;
            int num = (ii) * 8 + (jj);
            var buttons = Controls.Find("button" + num, true).FirstOrDefault() as Button;
            buttons.Text = "";
            ii = chislo / 8;
            jj = chislo % 8;
            if (playerred == 1 && map[ii, jj] != 3 && map[ii, jj] != 4)
            {
                map[ii, jj] = 1;


            }

            if (playerred == 2 && map[ii, jj] != 3 && map[ii, jj] != 4)
            {
                map[ii, jj] = 2;


            }





        }



        private void can_eat()
        {
            bool evl = false;
            for (int ii = 0; ii < 8; ii++)
            {
                for (int jj = 0; jj < 8; jj++)
                {
                    if (player == 2 && map[ii, jj] == 2)
                    {

                        if (jj - 1 != -1 && ii != 7 && jj != 1 & ii != 6)
                        {
                            if (map[ii + 1, jj - 1] == 1)
                            {
                                if (map[ii + 2, jj - 2] == 0)
                                {
                                    if (evl == false)
                                    {
                                        of_all();
                                        evl = true;

                                    }
                                    int num = (ii) * 8 + (jj);

                                    var button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                                    button.Enabled = true;
                                }
                            }
                        }
                        if (jj + 1 != 8 && ii != 7 && jj != 6 && ii != 6)
                        {
                            if (map[ii + 1, jj + 1] == 1)
                            {
                                if (map[ii + 2, jj + 2] == 0)
                                {

                                    if (evl == false)
                                    {
                                        of_all();
                                        evl = true;

                                    }
                                    int num = (ii) * 8 + (jj);

                                    var button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                                    button.Enabled = true;
                                }
                            }
                        }

                        if (jj != 6 && ii != 1 && jj != 7 && ii != 0)
                        {
                            if (map[ii - 1, jj + 1] == 1)
                            {
                                if (map[ii - 2, jj + 2] == 0)
                                {
                                    if (evl == false)
                                    {
                                        of_all();
                                        evl = true;

                                    }
                                    int num = (ii) * 8 + (jj);

                                    var button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                                    button.Enabled = true;
                                }
                            }

                        }


                        if (jj != 1 && ii != 1 && jj != 0 && ii != 0)
                        {
                            if (map[ii - 1, jj - 1] == 1)
                            {

                                if (map[ii - 2, jj - 2] == 0)
                                {
                                    if (evl == false)
                                    {
                                        of_all();
                                        evl = true;

                                    }
                                    int num = (ii) * 8 + (jj);

                                    var button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                                    button.Enabled = true;

                                }
                            }
                        }


                    }
                    else if (player == 1 && map[ii, jj] == 1)
                    {

                        if (jj != 6 && ii != 1 && jj != 7 && ii != 0)
                        {
                            if (map[ii - 1, jj + 1] == 2)
                            {
                                if (map[ii - 2, jj + 2] == 0)
                                {
                                    if (evl == false)
                                    {
                                        of_all();
                                        evl = true;

                                    }
                                    int num = (ii) * 8 + (jj);

                                    var button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                                    button.Enabled = true;
                                }
                            }

                        }


                        if (jj != 1 && ii != 1 && jj != 0 && ii != 0)
                        {
                            if (map[ii - 1, jj - 1] == 2)
                            {

                                if (map[ii - 2, jj - 2] == 0)
                                {
                                    if (evl == false)
                                    {
                                        of_all();
                                        evl = true;

                                    }
                                    int num = (ii) * 8 + (jj);

                                    var button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                                    button.Enabled = true;

                                }
                            }
                        }

                        if (jj - 1 != -1 && ii != 7 && jj != 1 & ii != 6)
                        {
                            if (map[ii + 1, jj - 1] == 2)
                            {
                                if (map[ii + 2, jj - 2] == 0)
                                {
                                    if (evl == false)
                                    {
                                        of_all();
                                        evl = true;

                                    }
                                    int num = (ii) * 8 + (jj);

                                    var button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                                    button.Enabled = true;
                                }
                            }
                        }
                        if (jj + 1 != 8 && ii != 7 && jj != 6 && ii != 6)
                        {
                            if (map[ii + 1, jj + 1] == 2)
                            {
                                if (map[ii + 2, jj + 2] == 0)
                                {

                                    if (evl == false)
                                    {
                                        of_all();
                                        evl = true;

                                    }
                                    int num = (ii) * 8 + (jj);

                                    var button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                                    button.Enabled = true;
                                }
                            }
                        }
                    }

                }
            }
        }
        private bool first_eat(int a, int ii, int jj, int playerrd)
        {



            try_to_beat = false;
            if (playerrd == 2 && map[ii, jj] == 2)
            {
                if (jj != 6 && ii != 1 && jj != 7 && ii != 0)
                {
                    if (map[ii - 1, jj + 1] == 1)
                    {
                        if (map[ii - 2, jj + 2] == 0)
                        {
                            int num = (ii - 2) * 8 + (jj + 2);

                            var button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                            button.BackColor = Color.Yellow;
                            button.Enabled = true;
                            try_to_beat = true;





                            num = (ii - 1) * 8 + (jj - 1);

                            button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                            button.BackColor = Color.White;
                        }
                    }

                }


                if (jj != 1 && ii != 1 && jj != 0 && ii != 0)
                {
                    if (map[ii - 1, jj - 1] == 1)
                    {

                        if (map[ii - 2, jj - 2] == 0)
                        {
                            int num = (ii - 2) * 8 + (jj - 2);

                            var button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                            button.BackColor = Color.Yellow;
                            button.Enabled = true;
                            try_to_beat = true;





                            num = (ii - 1) * 8 + (jj + 1);

                            button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                            button.BackColor = Color.White;


                        }
                    }
                }
                if (jj != 0 && ii != 7 && jj != 1 && ii != 6)
                {
                    if (map[ii + 1, jj - 1] == 1)
                    {
                        if (map[ii + 2, jj - 2] == 0)
                        {
                            int num = (ii + 2) * 8 + (jj - 2);

                            var button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                            button.BackColor = Color.Yellow;
                            button.Enabled = true;
                            try_to_beat = true;



                            num = (ii + 1) * 8 + (jj + 1);

                            button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                            button.BackColor = Color.White;


                        }
                    }
                }
                if (jj + 1 != 8 && ii != 7 && jj != 6 && ii != 6)
                {
                    if (map[ii + 1, jj + 1] == 1)
                    {
                        if (map[ii + 2, jj + 2] == 0)
                        {
                            int num = (ii + 2) * 8 + (jj + 2);

                            var button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                            button.BackColor = Color.Yellow;
                            button.Enabled = true;
                            try_to_beat = true;




                            num = (ii + 1) * 8 + (jj - 1);

                            button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                            button.BackColor = Color.White;
                        }
                    }
                }

            }
            else if (playerrd == 1 && map[ii, jj] == 1)
            {

                if (jj != 6 && ii != 1 && jj != 7 && ii != 0)
                {
                    if (map[ii - 1, jj + 1] == 2)
                    {
                        if (map[ii - 2, jj + 2] == 0)
                        {
                            int num = (ii - 2) * 8 + (jj + 2);

                            var button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                            button.BackColor = Color.Yellow;
                            button.Enabled = true;
                            try_to_beat = true;





                            num = (ii - 1) * 8 + (jj - 1);

                            button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                            button.BackColor = Color.White;
                        }
                    }

                }


                if (jj != 1 && ii != 1 && jj != 0 && ii != 0)
                {
                    if (map[ii - 1, jj - 1] == 2)
                    {

                        if (map[ii - 2, jj - 2] == 0)
                        {
                            int num = (ii - 2) * 8 + (jj - 2);

                            var button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                            button.BackColor = Color.Yellow;
                            button.Enabled = true;
                            try_to_beat = true;





                            num = (ii - 1) * 8 + (jj + 1);

                            button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                            button.BackColor = Color.White;


                        }
                    }
                }
                if (jj != 0 && ii != 7 && jj != 1 && ii != 6)
                {
                    if (map[ii + 1, jj - 1] == 2)
                    {
                        if (map[ii + 2, jj - 2] == 0)
                        {
                            int num = (ii + 2) * 8 + (jj - 2);

                            var button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                            button.BackColor = Color.Yellow;
                            button.Enabled = true;
                            try_to_beat = true;
                            num = (ii + 1) * 8 + (jj + 1);

                            button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                            button.BackColor = Color.White;


                        }
                    }
                }
                if (jj + 1 != 8 && ii != 7 && jj != 6 && ii != 6)
                {
                    if (map[ii + 1, jj + 1] == 2)
                    {
                        if (map[ii + 2, jj + 2] == 0)
                        {
                            int num = (ii + 2) * 8 + (jj + 2);

                            var button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                            button.BackColor = Color.Yellow;
                            button.Enabled = true;
                            try_to_beat = true;




                            num = (ii + 1) * 8 + (jj - 1);

                            button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                            button.BackColor = Color.White;
                        }
                    }
                }
            }

  return try_to_beat;
        }

        private bool is_queen(int num)
        {
            if ((num >= 0 && num <= 7) || (num >= 56 && num <= 63)) {
                var button = Controls.Find("button" + num, true).FirstOrDefault() as Button;
                button.Text = "D";
                int ii = num / 8, jj = num % 8;
       
                if (player == 1)
                {
                    map[ii, jj] = 3;
                } else if (player == 2)
                {
                    map[ii, jj] = 4;
                }
                return true;
            }
            return false;
        }


        private void can_quween_eat(int ii, int jj)
        {

            try_to_qwn_beat = false;



            int iii = ii, jjj = jj;
            while (iii >0 && jjj < 7)
            {

                int num = (iii) * 8 + jjj;
                if (num <= 63 && num >= 0)
                {
                    if ((player == 2 && (map[iii, jjj] == 1 || map[iii, jjj] == 3)) || ((player == 1 && (map[iii, jjj] == 2 || map[iii, jjj] == 4))))
                    {
                        label1.Text = iii.ToString() + " " + jjj.ToString();
                        if (map[iii - 1, jjj + 1] == 0 && (map[iii + 1, jjj - 1] == 0 || (iii + 1 == ii && jjj - 1 == jj)))
                        {
                            // label1.Text = iii.ToString() + " " + jjj.ToString();
                            while (iii >= 1 && jjj <= 6)
                            {

                                iii--;
                                jjj++;
                                if (map[iii, jjj] != 0) { break; }
                                int num1 = (iii) * 8 + jjj;
                                var button = Controls.Find("button" + num1, true).FirstOrDefault() as Button;
                                button.Enabled = true;
                                button.BackColor = Color.Yellow;

                            }
                            try_to_qwn_beat = true;
                                break;
                        }
                    }
                }
                iii--;
                jjj++;

            }
        







            iii = ii;
            jjj = jj;
            while (iii > 0 && jjj > 0)
            {


                int num = (iii) * 8 + jjj;
                if (num <= 63 && num >= 0)
                {
                    if ((player == 2 && (map[iii, jjj] == 1 || map[iii, jjj] == 3)) || ((player == 1 && (map[iii, jjj] == 2 || map[iii, jjj] == 4))))
                    {
                        label1.Text = iii.ToString() + " " + jjj.ToString();
                        if (map[iii - 1, jjj - 1] == 0 && (map[iii + 1, jjj + 1] == 0 || (iii + 1 == ii && jjj + 1 == jj)))
                        {
                            // label1.Text = iii.ToString() + " " + jjj.ToString();
                            while (iii >= 1 && jjj >= 1)
                            {

                                iii--;
                                jjj--;
                                if (map[iii, jjj] != 0) { break; }
                                int num1 = (iii) * 8 + jjj;
                                var button = Controls.Find("button" + num1, true).FirstOrDefault() as Button;
                                button.Enabled = true;
                                button.BackColor = Color.Yellow;

                            }
                            try_to_qwn_beat = true;
                            break;
                        }
                    }
                }
                iii--;
                jjj--;

            
        }
            iii = ii;
            jjj = jj;
            while (iii < 7 && jjj > 0)
            {
                int num = (iii) * 8 + jjj;
                if (num <= 63 && num >= 0)
                {
                    if ((player == 2 && (map[iii, jjj] == 1 || map[iii, jjj] == 3)) || ((player == 1 && (map[iii, jjj] == 2 || map[iii, jjj] == 4))))
                    {
                        label1.Text = iii.ToString() + " " + jjj.ToString();
                        if (map[iii + 1, jjj - 1] == 0 && (map[iii - 1, jjj + 1] == 0 || (iii - 1 == ii && jjj + 1 == jj)))
                        {
                            // label1.Text = iii.ToString() + " " + jjj.ToString();
                            while (iii <= 6 && jjj >= 1)
                            {

                                iii++;
                                jjj--;
                                if (map[iii, jjj] != 0) { break; }
                                int num1 = (iii) * 8 + jjj;
                                var button = Controls.Find("button" + num1, true).FirstOrDefault() as Button;
                                button.Enabled = true;
                                button.BackColor = Color.Yellow;

                            }
                            try_to_qwn_beat = true;
                            break;
                        }
                    }
                }
                iii++;
                jjj--;

            }
        
            iii = ii;
            jjj = jj;
           while (iii < 7 && jjj < 7)
            {

                int num = (iii) * 8 + jjj;
                if (num <= 63 && num >= 0 )
                {
                    if ((player == 2 && (map[iii, jjj] == 1 || map[iii, jjj] == 3)) || ((player == 1 && (map[iii, jjj] == 2 || map[iii, jjj] == 4)) ))
                    {
                        label1.Text = iii.ToString() + " " + jjj.ToString();
                        if (map[iii + 1, jjj + 1] == 0 && (map[iii - 1, jjj - 1] == 0 || (iii - 1 == ii && jjj - 1 == jj)))
                        {
                           // label1.Text = iii.ToString() + " " + jjj.ToString();
                            while (iii <= 6 && jjj <= 6)
                            {
                               
                                iii++;
                                jjj++;
                                if (map[iii, jjj] != 0) { break; }
                                int num1 = (iii) * 8 + jjj;
                                var button = Controls.Find("button" + num1, true).FirstOrDefault() as Button;
                                button.Enabled = true;
                                button.BackColor = Color.Yellow;

                            }
                            try_to_qwn_beat = true;
                            break;
                        }
                    }
                }
                    iii++;
                    jjj++;
                
            }
        } 


    
      /*  private bool second_eat(int ii, int jj)
        {

            return true;


        }*/
        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button48_Click(object sender, EventArgs e)
        {

        }

        private void button49_Click(object sender, EventArgs e)
        {

        }

        private void button40_Click(object sender, EventArgs e)
        {

            Image image = Properties.Resources.black;

            button40.Image = image;
            //button40.Image = null;
        }

        private void button15_Click(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {

        }
    }
}
