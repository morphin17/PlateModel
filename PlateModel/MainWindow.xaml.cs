using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Forms;

namespace PlateModel
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        /** Анимация **/
        private bool animateFlag = false;       //Воспроизвести
        DispatcherTimer timer;                  //Таймер для анимации

        double[] substance = { 0.000127, 0.00016563, 0.000111, 0.000088 };

        public Plate p1;                         //Пластина, числнное решение
        public Plate p2;                        //Пластина, для шага в два раза больше

        NCalc.Expression exprF1;
        NCalc.Expression exprPhi1;
        NCalc.Expression exprMu1;
        NCalc.Expression exprU1;

        NCalc.Expression exprF2;
        NCalc.Expression exprPhi2;
        NCalc.Expression exprMu2;
        NCalc.Expression exprU2;

        bool blockPlateWidth1 = true;
        bool blockPlateFunctionF1 = true;
        bool blockPlateFunctionPhi1 = true;
        bool blockPlateFunctionMu1 = true;
        bool blockPlateWidthPointsCount1 = true;
        bool blockPlateHeight1 = true;
        bool blockPlateHeightPointsCount1 = true;
        bool blockPlateTimeStep1 = true;
        bool blockPlateFunctionU1 = true;
        bool blockPlateSubstance1 = true;
        bool blockBottomBorderTemp1 = true;
        bool blockTopBorderTemp1 = true;

        bool blockPlateWidth2 = true;
        bool blockPlateFunctionF2 = true;
        bool blockPlateFunctionPhi2 = true;
        bool blockPlateFunctionMu2 = true;
        bool blockPlateWidthPointsCount2 = true;
        bool blockPlateHeight2 = true;
        bool blockPlateHeightPointsCount2 = true;
        bool blockPlateTimeStep2 = true;
        bool blockPlateFunctionU2 = true;
        bool blockPlateSubstance2 = true;
        bool blockBottomBorderTemp2 = true;
        bool blockTopBorderTemp2 = true;

        Dictionary<string, string> functionsByName = new Dictionary<string, string>()
        {
            { "Без нагрева", "0" },
            { "По центру", "Sin(pi * x / a) * Sin(pi * y / b)" },
            {"Левый верхний угол", "Sin((x+a)*pi/(2*a))*Sin((y+b)*pi/(2*b))"},
            {"Правый верхний угол", "Sin((x+a)*pi/(2*a))*Sin((y)*pi/(2*b))"},
            {"Левый нижний угол", "Sin(x*pi/(2*a))*Sin((y+b)*pi/(2*b))"},
            {"Правый нижний угол", "Sin(x*pi/(2*a))*Sin(y*pi/(2*b))"},
        };

        public MainWindow()
        {
            InitializeComponent();


            timer = new DispatcherTimer();

            timer.Tick += new EventHandler(timer_Tick);

            timer.Interval = new TimeSpan(0, 0, 0, 0, 40);

            timer.Start();

            BlockPlateOptiopns1();

            init_PlateOptions1();
            init_Expr1();
            init_Plate1();
            init_HeatMap1();

            UnblockPlateOptiopns1();

            BlockPlateOptiopns2();

            init_PlateOptions2();
            init_Expr2();
            init_Plate2();
            init_HeatMap2();

            UnblockPlateOptiopns2();

        }

        private void BlockPlateOptiopns1()
        {
            blockPlateWidth1 = true;
            blockPlateFunctionF1 = true;
            blockPlateFunctionPhi1 = true;
            blockPlateFunctionMu1 = true;
            blockPlateWidthPointsCount1 = true;
            blockPlateHeight1 = true;
            blockPlateHeightPointsCount1 = true;
            blockPlateTimeStep1 = true;
            blockPlateFunctionU1 = true;
            blockPlateSubstance1 = true;
            blockBottomBorderTemp1 = true;
            blockTopBorderTemp1 = true;
        }

        private void UnblockPlateOptiopns1()
        {
            blockPlateWidth1 = false;
            blockPlateFunctionF1 = false;
            blockPlateFunctionPhi1 = false;
            blockPlateFunctionMu1 = false;
            blockPlateWidthPointsCount1 = false;
            blockPlateHeight1 = false;
            blockPlateHeightPointsCount1 = false;
            blockPlateTimeStep1 = false;
            blockPlateFunctionU1 = false;
            blockPlateSubstance1 = false;
            blockBottomBorderTemp1 = false;
            blockTopBorderTemp1 = false;
        }
        private void BlockPlateOptiopns2()
        {
            blockPlateWidth2 = true;
            blockPlateFunctionF2 = true;
            blockPlateFunctionPhi2 = true;
            blockPlateFunctionMu2 = true;
            blockPlateWidthPointsCount2 = true;
            blockPlateHeight2 = true;
            blockPlateHeightPointsCount2 = true;
            blockPlateTimeStep2 = true;
            blockPlateFunctionU2 = true;
            blockPlateSubstance2 = true;
            blockBottomBorderTemp2 = true;
            blockTopBorderTemp2 = true;
        }

        private void UnblockPlateOptiopns2()
        {
            blockPlateWidth2 = false;
            blockPlateWidth2 = false;
            blockPlateFunctionF2 = false;
            blockPlateFunctionPhi2 = false;
            blockPlateFunctionMu2 = false;
            blockPlateWidthPointsCount2 = false;
            blockPlateHeight2 = false;
            blockPlateHeightPointsCount2 = false;
            blockPlateTimeStep2 = false;
            blockPlateFunctionU2 = false;
            blockPlateSubstance2 = false;
            blockBottomBorderTemp2 = false;
            blockTopBorderTemp2 = false;
        }

        private void MenuItem_Test1(object sender, RoutedEventArgs e)
        {
            BlockPlateOptiopns1();
            BlockPlateOptiopns2();

            init_PlateOptions1();
            init_PlateOptions2();

            PlateFunctionF1.Text = "Без нагрева";
            PlateFunctionPhi1.Text = "Без нагрева";
            PlateFunctionMu1.Text = "0";
            PlateWidth1.Value = 0.05;
            PlateWidthPointsCount1.Value = 100;
            PlateHeight1.Value = 0.05;
            PlateHeightPointsCount1.Value = 100;
            PlateTimeStep1.Value = 0.1;
            PlateFunctionU1.Text = "Sin(pi*x/0.05)*Sin(pi*y/0.05)*Pow(e, -0.000127*(pi*pi/(0.05*0.05) + pi*pi/(0.05*0.05))*t)";
            PlateSubstance1.SelectedIndex = 0;
            BottomBorderTemp1.Value = 0;
            TopBorderTemp1.Value = 1;

            PlateFunctionF2.Text = "Без нагрева";
            PlateFunctionPhi2.Text = "По центру";
            PlateFunctionMu2.Text = "0";
            PlateWidth2.Value = 0.05;
            PlateWidthPointsCount2.Value = 100;
            PlateHeight2.Value = 0.05;
            PlateHeightPointsCount2.Value = 100;
            PlateTimeStep2.Value = 0.1;
            PlateSubstance2.SelectedIndex = 0;
            BottomBorderTemp2.Value = 0;
            TopBorderTemp2.Value = 1;

            init_Expr1();
            init_Expr2();

            init_Plate1();
            init_HeatMap1();
            init_Plate2();
            init_HeatMap2();

            UnblockPlateOptiopns1();
            UnblockPlateOptiopns2();
        }
        private void MenuItem_Test2(object sender, RoutedEventArgs e)
        {
            BlockPlateOptiopns1();
            BlockPlateOptiopns2();

            init_PlateOptions1();
            init_PlateOptions2();

            PlateFunctionF1.Text = "Без нагрева";
            PlateFunctionPhi1.Text = "По центру";
            PlateFunctionMu1.Text = "0";
            PlateWidth1.Value = 0.5;
            PlateWidthPointsCount1.Value = 100;
            PlateHeight1.Value = 0.5;
            PlateHeightPointsCount1.Value = 100;
            PlateTimeStep1.Value = 10;
            PlateFunctionU1.Text = "";
            PlateSubstance1.SelectedIndex = 0;
            BottomBorderTemp1.Value = 0;
            TopBorderTemp1.Value = 1;

            PlateFunctionF2.Text = "Без нагрева";
            PlateFunctionPhi2.Text = "По центру";
            PlateFunctionMu2.Text = "0";
            PlateWidth2.Value = 0.5;
            PlateWidthPointsCount2.Value = 100;
            PlateHeight2.Value = 0.5;
            PlateHeightPointsCount2.Value = 100;
            PlateTimeStep2.Value = 10;
            PlateSubstance2.SelectedIndex = 3;
            BottomBorderTemp2.Value = 0;
            TopBorderTemp2.Value = 1;

            init_Expr1();
            init_Expr2();

            init_Plate1();
            init_HeatMap1();
            init_Plate2();
            init_HeatMap2();

            UnblockPlateOptiopns1();
            UnblockPlateOptiopns2();
        }
        private void MenuItem_Test3(object sender, RoutedEventArgs e)
        {
            BlockPlateOptiopns1();
            BlockPlateOptiopns2();

            init_PlateOptions1();
            init_PlateOptions2();

            PlateFunctionF1.Text = "По центру";
            PlateFunctionPhi1.Text = "Без нагрева";
            PlateFunctionMu1.Text = "0";
            PlateWidth1.Value = 0.05;
            PlateWidthPointsCount1.Value = 100;
            PlateHeight1.Value = 0.05;
            PlateHeightPointsCount1.Value = 100;
            PlateTimeStep1.Value = 0.1;
            PlateFunctionU1.Text = "";
            PlateSubstance1.SelectedIndex = 0;
            BottomBorderTemp1.Value = 0;
            TopBorderTemp1.Value = 1;

            PlateFunctionF2.Text = "По центру";
            PlateFunctionPhi2.Text = "Без нагрева";
            PlateFunctionMu2.Text = "0";
            PlateWidth2.Value = 0.05;
            PlateWidthPointsCount2.Value = 100;
            PlateHeight2.Value = 0.05;
            PlateHeightPointsCount2.Value = 100;
            PlateTimeStep2.Value = 0.1;
            PlateSubstance2.SelectedIndex = 3;
            BottomBorderTemp2.Value = 0;
            TopBorderTemp2.Value = 1;

            init_Expr1();
            init_Expr2();

            init_Plate1();
            init_HeatMap1();
            init_Plate2();
            init_HeatMap2();

            UnblockPlateOptiopns1();
            UnblockPlateOptiopns2();
        }
        private void MenuItem_Test4(object sender, RoutedEventArgs e)
        {
            BlockPlateOptiopns1();
            BlockPlateOptiopns2();

            init_PlateOptions1();
            init_PlateOptions2();

            PlateFunctionF1.Text = "Левый нижний угол";
            PlateFunctionPhi1.Text = "Правый верхний угол";
            PlateFunctionMu1.Text = "0";
            PlateWidth1.Value = 0.5;
            PlateWidthPointsCount1.Value = 100;
            PlateHeight1.Value = 0.5;
            PlateHeightPointsCount1.Value = 100;
            PlateTimeStep1.Value = 0.1;
            PlateFunctionU1.Text = "";
            PlateSubstance1.SelectedIndex = 0;
            BottomBorderTemp1.Value = 0;
            TopBorderTemp1.Value = 1;

            PlateFunctionF2.Text = "Левый нижний угол";
            PlateFunctionPhi2.Text = "Правый верхний угол";
            PlateFunctionMu2.Text = "0";
            PlateWidth2.Value = 0.5;
            PlateWidthPointsCount2.Value = 100;
            PlateHeight2.Value = 0.5;
            PlateHeightPointsCount2.Value = 100;
            PlateTimeStep2.Value = 0.1;
            PlateSubstance2.SelectedIndex = 3;
            BottomBorderTemp2.Value = 0;
            TopBorderTemp2.Value = 1;

            init_Expr1();
            init_Expr2();

            init_Plate1();
            init_HeatMap1();
            init_Plate2();
            init_HeatMap2();

            UnblockPlateOptiopns1();
            UnblockPlateOptiopns2();
        }


        private void init_PlateOptions1()
        {
            PlateWidth1.Value = 10;
            PlateWidthPointsCount1.Value = 100;
            PlateHeight1.Value = 10;
            PlateHeightPointsCount1.Value = 100;
            PlateTimeStep1.Value = 1;
            PlateFunctionF1.Text = "Без нагрева";
            PlateFunctionPhi1.Text = "Без нагрева";
            PlateFunctionMu1.Text = "0";
            PlateFunctionU1.Text = "";
            PlateSubstance1.SelectedIndex = 0;
            BottomBorderTemp1.Value = 0;
            TopBorderTemp1.Value = 1;
        }
        private void init_PlateOptions2()
        {
            PlateWidth2.Value = 10;
            PlateWidthPointsCount2.Value = 100;
            PlateHeight2.Value = 10;
            PlateHeightPointsCount2.Value = 100;
            PlateTimeStep2.Value = 1;
            PlateFunctionF2.Text = "Без нагрева";
            PlateFunctionPhi2.Text = "Без нагрева";
            PlateFunctionMu2.Text = "0";
            PlateFunctionU2.Text = "";
            PlateSubstance2.SelectedIndex = 0;
            BottomBorderTemp2.Value = 0;
            TopBorderTemp2.Value = 1;
        }


        private void init_Expr1()
        {
            var phitext = PlateFunctionPhi1.Text;
            if (functionsByName.ContainsKey(phitext))
            {
                phitext = functionsByName[phitext];
            }

            var ftext = PlateFunctionF1.Text;
            if (functionsByName.ContainsKey(ftext))
            {
                ftext = functionsByName[ftext];
            }

            exprF1 = new NCalc.Expression(ftext);
            exprPhi1 = new NCalc.Expression(phitext);
            exprMu1 = new NCalc.Expression(PlateFunctionMu1.Text);
            if (PlateFunctionU1.Text != "")
            {
                exprU1 = new NCalc.Expression(PlateFunctionU1.Text);
            }
        }
        private void init_Expr2()
        {
            var phitext = PlateFunctionPhi2.Text;
            if (functionsByName.ContainsKey(phitext))
            {
                phitext = functionsByName[phitext];
            }

            var ftext = PlateFunctionF2.Text;
            if (functionsByName.ContainsKey(ftext))
            {
                ftext = functionsByName[ftext];
            }

            exprF2 = new NCalc.Expression(ftext);
            exprPhi2 = new NCalc.Expression(phitext);
            exprMu2 = new NCalc.Expression(PlateFunctionMu2.Text);
            if (PlateFunctionU2.Text != "")
            {
                exprU2 = new NCalc.Expression(PlateFunctionU2.Text);
            }
        }


        private void init_Plate1()
        {

            CurrentModelTime1.Text = "Текущее время: 0 с.";
            try
            {
                p1 = new Plate(
                    substance[PlateSubstance1.SelectedIndex],
                    Convert.ToDouble(PlateHeight1.Value),
                    Convert.ToDouble(PlateWidth1.Value),
                    Convert.ToDouble(PlateHeight1.Value) / Convert.ToDouble(PlateHeightPointsCount1.Value),
                    Convert.ToDouble(PlateWidth1.Value) / Convert.ToDouble(PlateWidthPointsCount1.Value),
                    Convert.ToDouble(PlateTimeStep1.Value),
                    phi1,
                    f1,
                    mu1,
                    u1
                );
            }
            catch
            {
                //MessageBox.Show("Ошибка. Проверьте корректно ли вы ввели начальные условия для левой пластины.");
                return;
            }
        }
        private void init_Plate2()
        {
            CurrentModelTime2.Text = "Текущее время: 0 с.";
            try
            {
                p2 = new Plate(
                    substance[PlateSubstance2.SelectedIndex],
                    Convert.ToDouble(PlateHeight2.Value),
                    Convert.ToDouble(PlateWidth2.Value),
                    Convert.ToDouble(PlateHeight2.Value) / Convert.ToDouble(PlateHeightPointsCount2.Value),
                    Convert.ToDouble(PlateWidth2.Value) / Convert.ToDouble(PlateWidthPointsCount2.Value),
                    Convert.ToDouble(PlateTimeStep2.Value),
                    phi2,
                    f2,
                    mu2,
                    u2
                );
            }
            catch
            {
                //MessageBox.Show("Ошибка. Проверьте корректно ли вы ввели начальные условия для левой пластины.");
                return;
            }
        }

        private void init_HeatMap1()
        {

            var plt = WpfPlotPlate1.Plot;

            plt.YAxis.Ticks(false);
            plt.YAxis.Grid(false);
            plt.YAxis2.Ticks(true);
            plt.YAxis2.Grid(true);


            double[,] data2D;

            if (PlateFunctionU1.Text == "")
            {
                data2D = p1.U;
            }
            else
            {
                data2D = p1.solutionU;
            }


            plt.Clear();

            var hm = plt.AddHeatmap(data2D, lockScales: false);
            hm.Update(data2D, min: BottomBorderTemp1.Value, max: TopBorderTemp1.Value);

            hm.YAxisIndex = 1;
            hm.OffsetX = 0;
            hm.OffsetY = 0;
            hm.CellWidth = Convert.ToDouble(PlateWidth1.Value) / Convert.ToDouble(PlateWidthPointsCount1.Value);
            hm.CellHeight = Convert.ToDouble(PlateHeight1.Value) / Convert.ToDouble(PlateHeightPointsCount1.Value);

            var xOffset = Convert.ToDouble(PlateWidth1.Value) / Convert.ToDouble(PlateHeight1.Value);
            if (xOffset <= 1) {
                xOffset = 1 - xOffset;
            }
            else
            {
                xOffset = 0;
            }

            var yOffset = Convert.ToDouble(PlateHeight1.Value) / Convert.ToDouble(PlateWidth1.Value);
            if (yOffset <= 1) {
                yOffset = 1 - yOffset;
            }
            else
            {
                yOffset = 0;
            }

            plt.Margins(xOffset, yOffset);

            WpfPlotPlate1.Refresh();
        }

        private void init_HeatMap2()
        {

            var plt = WpfPlotPlate2.Plot;

            plt.YAxis.Ticks(false);
            plt.YAxis.Grid(false);
            plt.YAxis2.Ticks(true);
            plt.YAxis2.Grid(true);


            double[,] data2D;

            if (PlateFunctionU2.Text == "")
            {
                data2D = p2.U;
            }
            else
            {
                data2D = p2.solutionU;
            }


            plt.Clear();

            var hm = plt.AddHeatmap(data2D, lockScales: true);
            hm.Update(data2D, min: BottomBorderTemp2.Value, max: TopBorderTemp2.Value);

            hm.YAxisIndex = 1;
            hm.OffsetX = 0;
            hm.OffsetY = 0;
            hm.CellWidth = Convert.ToDouble(PlateWidth2.Value) / Convert.ToDouble(PlateWidthPointsCount2.Value);
            hm.CellHeight = Convert.ToDouble(PlateHeight2.Value) / Convert.ToDouble(PlateHeightPointsCount2.Value);

            var xOffset = Convert.ToDouble(PlateWidth2.Value) / Convert.ToDouble(PlateHeight2.Value);
            if (xOffset <= 1) {
                xOffset = 1 - xOffset;
            }
            else
            {
                xOffset = 0;
            }

            var yOffset = Convert.ToDouble(PlateHeight2.Value) / Convert.ToDouble(PlateWidth2.Value);
            if (yOffset <= 1) {
                yOffset = 1 - yOffset;
            }
            else
            {
                yOffset = 0;
            }

            plt.Margins(xOffset, yOffset);

            WpfPlotPlate2.Refresh();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (animateFlag)
            {

                try
                {
                    next();
                }
                catch
                {
                    System.Windows.MessageBox.Show("Ошибка. Проверьте корректны ли введены начальные условия.");
                    animateFlag = false;
                    if (animateFlag)
                    {
                        Play.Content = "⏸";
                    }
                    else
                    {
                        Play.Content = "▶";
                    }
                    return;
                }
            }
        }

        private void play_Click(object sender, RoutedEventArgs e)
        {
            animateFlag = !animateFlag;
            if (animateFlag)
            {
                Play.Content = "⏸";
            }
            else
            {
                Play.Content = "▶";
            }
        }

        private void next ()
        {

            try
            {
                //for (int i = 0; i < (int) (PlateTimeStep1.Value / 0.01); i++)
                    if (PlateFunctionU1.Text == "") { 
                            p1.nextU();
                    }
                    else
                    {
                        p1.nextSolution();
                    }

                //for (int i = 0; i < (int) (PlateTimeStep2.Value / 0.01); i++)
                    if (PlateFunctionU2.Text == "") { 
                            p2.nextU();
                    }
                    else
                    {
                        p2.nextSolution();
                    }
            }
            catch
            {
                System.Windows.MessageBox.Show("Ошибка. Проверьте начальные условия на корректный ввод формул.");
                animateFlag = false;
                if (animateFlag)
                {
                    Play.Content = "⏸";
                }
                else
                {
                    Play.Content = "▶";
                }
                return;
            }

            init_HeatMap1();
            init_HeatMap2();


            CurrentModelTime1.Text = "Текущее время:\n" + Convert.ToString(Math.Round(p1.curT, 2)) + " с.";
            CurrentModelTime2.Text = "Текущее время:\n" + Convert.ToString(Math.Round(p2.curT, 2)) + " с.";
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            next();
        }


        private double f1(double a, double b, double x, double y, double t)
        {
            exprF1.Parameters["pi"] = Math.PI;
            exprF1.Parameters["e"] = Math.E;
            exprF1.Parameters["x"] = x;
            exprF1.Parameters["y"] = y;
            exprF1.Parameters["t"] = t;
            exprF1.Parameters["a"] = a;
            exprF1.Parameters["b"] = b;

            double r = Convert.ToDouble(exprF1.Evaluate());
            return r;
        }
        private double f2(double a, double b, double x, double y, double t)
        {
            exprF2.Parameters["pi"] = Math.PI;
            exprF2.Parameters["e"] = Math.E;
            exprF2.Parameters["x"] = x;
            exprF2.Parameters["y"] = y;
            exprF2.Parameters["t"] = t;
            exprF2.Parameters["a"] = a;
            exprF2.Parameters["b"] = b;

            double r = Convert.ToDouble(exprF2.Evaluate());
            return r;
        }

        private double phi1(double a, double b, double x, double y)
        {
            exprPhi1.Parameters["pi"] = Math.PI;
            exprPhi1.Parameters["e"] = Math.E;
            exprPhi1.Parameters["x"] = x;
            exprPhi1.Parameters["y"] = y;
            exprPhi1.Parameters["a"] = a;
            exprPhi1.Parameters["b"] = b;

            double r = Convert.ToDouble(exprPhi1.Evaluate());
            return r;
        }
        private double phi2(double a, double b, double x, double y)
        {
            exprPhi2.Parameters["pi"] = Math.PI;
            exprPhi2.Parameters["e"] = Math.E;
            exprPhi2.Parameters["x"] = x;
            exprPhi2.Parameters["y"] = y;
            exprPhi2.Parameters["a"] = a;
            exprPhi2.Parameters["b"] = b;

            double r = Convert.ToDouble(exprPhi2.Evaluate());
            return r;
        }

        private double mu1(double t)
        {
            exprMu1.Parameters["pi"] = Math.PI;
            exprMu1.Parameters["e"] = Math.E;
            exprMu1.Parameters["t"] = t;

            double r = Convert.ToDouble(exprMu1.Evaluate());
            return r;
        }

        private double mu2(double t)
        {
            exprMu2.Parameters["pi"] = Math.PI;
            exprMu2.Parameters["e"] = Math.E;
            exprMu2.Parameters["t"] = t;

            double r = Convert.ToDouble(exprMu2.Evaluate());
            return r;
        }

        private double u1(double x, double y, double t)
        {
            if (PlateFunctionU1.Text == "")
            {
                return 0;
            }
            exprU1.Parameters["pi"] = Math.PI;
            exprU1.Parameters["e"] = Math.E;
            exprU1.Parameters["t"] = t;
            exprU1.Parameters["x"] = x;
            exprU1.Parameters["y"] = y;

            double r = Convert.ToDouble(exprU1.Evaluate());
            return r;
        }
        private double u2(double x, double y, double t)
        {
            if (PlateFunctionU2.Text == "")
            {
                return 0;
            }

            exprU2.Parameters["pi"] = Math.PI;
            exprU2.Parameters["e"] = Math.E;
            exprU2.Parameters["t"] = t;
            exprU2.Parameters["x"] = x;
            exprU2.Parameters["y"] = y;

            double r = Convert.ToDouble(exprU2.Evaluate());
            return r;
        }

        private void PlateWidth1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (blockPlateWidth1)
            {
                return;
            }

            init_Plate1();
            init_HeatMap1();
        }

        private void PlateWidthPointsCount1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (blockPlateWidthPointsCount1)
            {
                return;
            }

            init_Plate1();
            init_HeatMap1();
        }

        private void PlateHeight1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (blockPlateHeight1)
            {
                return;
            }

            init_Plate1();
            init_HeatMap1();
        }

        private void PlateHeightPointsCount1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (blockPlateHeightPointsCount1)
            {
                return;
            }

            init_Plate1();
            init_HeatMap1();

        }

        private void PlateTimeStep1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (blockPlateTimeStep1)
            {
                return;
            }

            init_Plate1();
            init_HeatMap1();

        }

        private void BottomBorderTemp1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (blockBottomBorderTemp1)
            {
                return;
            }

            init_HeatMap1();

        }

        private void TopBorderTemp1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (blockTopBorderTemp1)
            {
                return;
            }

            init_HeatMap1();
        }

        private void PlateWidth2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (blockPlateWidth2)
            {
                return;
            }

            init_Plate2();
            init_HeatMap2();
        }

        private void PlateWidthPointsCount2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (blockPlateWidthPointsCount2)
            {
                return;
            }

            init_Plate2();
            init_HeatMap2();

        }

        private void PlateHeight2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (blockPlateHeight2)
            {
                return;
            }

            init_Plate2();
            init_HeatMap2();
        }

        private void PlateHeightPointsCount2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (blockPlateHeight2)
            {
                return;
            }

            init_Plate2();
            init_HeatMap2();
        }

        private void PlateTimeStep2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (blockPlateTimeStep2)
            {
                return;
            }

            init_Plate2();
            init_HeatMap2();
        }

        private void BottomBorderTemp2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (blockBottomBorderTemp2)
            {
                return;
            }

            init_HeatMap2();

        }

        private void TopBorderTemp2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (blockTopBorderTemp2)
            {
                return;
            }

            init_HeatMap2();
        }

        private void PlateSubstance1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (blockPlateSubstance1)
            {
                return;
            }

            init_Plate1();
            init_HeatMap1();
        }

        private void PlateSubstance2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (blockPlateSubstance2)
            {
                return;
            }

            init_Plate2();
            init_HeatMap2();
        }

        private void PlateFunctionF1_TextChanged(object sender, TextChangedEventArgs e)
        {
            var ftext = PlateFunctionF1.Text;
            if (functionsByName.ContainsKey(ftext))
            {
                ftext = functionsByName[ftext];
            }

            if (blockPlateFunctionF1)
            {
                return;
            }
            
            try
            {
                exprF1 = new NCalc.Expression(ftext);
                init_Plate1();
                init_HeatMap1();
            }catch
            {
                Console.WriteLine(PlateFunctionF1.Text);
                return;
            }
        }

        private void PlateFunctionF2_TextChanged(object sender, TextChangedEventArgs e)
        {
            var ftext = PlateFunctionF2.Text;
            if (functionsByName.ContainsKey(ftext))
            {
                ftext = functionsByName[ftext];
            }

            if (blockPlateFunctionF2)
            {
                return;
            }
            
            try
            {
                exprF2 = new NCalc.Expression(ftext);
                init_Plate2();
                init_HeatMap2();
            }catch
            {
                return;
            }
        }

        private void PlateFunctionPhi1_TextChanged(object sender, TextChangedEventArgs e)
        {
            var phitext = PlateFunctionPhi1.Text;
            if (functionsByName.ContainsKey(phitext))
            {
                phitext = functionsByName[phitext];
            }

            if (blockPlateFunctionPhi1)
            {
                return;
            }
            
            try
            {
                exprPhi1 = new NCalc.Expression(phitext);
                init_Plate1();
                init_HeatMap1();
            }catch
            {
                Console.WriteLine("Ошибка");
                Console.WriteLine(PlateFunctionPhi1.Text);
                return;
            }

        }

        private void PlateFunctionMu1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (blockPlateFunctionMu1)
            {
                return;
            }

            try
            {
                exprMu1 = new NCalc.Expression(PlateFunctionMu1.Text);
                init_Plate1();
                init_HeatMap1();
            }
            catch
            {
                return;
            }


        }

        private void PlateFunctionU1_TextChanged(object sender, TextChangedEventArgs e)
        {
            var u1text = PlateFunctionU1.Text;
            if (u1text.Length > 0)
            {
                PlateFunctionF1.IsEnabled = false;
                PlateFunctionPhi1.IsEnabled = false;
                PlateFunctionMu1.IsEnabled = false;
                PlateFunctionMuSlider1.IsEnabled = false;
                PlateSubstance1.IsEnabled = false;
            }
            else
            {
                PlateFunctionF1.IsEnabled = true;
                PlateFunctionPhi1.IsEnabled = true;
                PlateFunctionMu1.IsEnabled = true;
                PlateFunctionMuSlider1.IsEnabled = true;
                PlateSubstance1.IsEnabled = true;

                init_Plate1();
                init_HeatMap1();
            }

            if (blockPlateFunctionU1)
            {
                return;
            }

            try
            {
                exprU1 = new NCalc.Expression(u1text);
                init_Plate1();
                init_HeatMap1();
            }
            catch
            {
            }


        }

        private void PlateFunctionPhi2_TextChanged(object sender, TextChangedEventArgs e)
        {
            var phitext = PlateFunctionPhi2.Text;
            if (functionsByName.ContainsKey(phitext))
            {
                phitext = functionsByName[phitext];
            }

            if (blockPlateFunctionPhi2)
            {
                return;
            }

            try
            {
                exprPhi2 = new NCalc.Expression(phitext);
                init_Plate2();
                init_HeatMap2();
            }
            catch
            {
                return;
            }


        }

        private void PlateFunctionMu2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (blockPlateFunctionMu2)
            {
                return;
            }

            try
            {
                exprMu2 = new NCalc.Expression(PlateFunctionMu2.Text);
                init_Plate2();
                init_HeatMap2();
            }
            catch
            {
                return;
            }

        }

        private void PlateFunctionU2_TextChanged(object sender, TextChangedEventArgs e)
        { 
            var utext = PlateFunctionU2.Text;
            if (utext.Length > 0)
            {
                PlateFunctionF2.IsEnabled = false;
                PlateFunctionPhi2.IsEnabled = false;
                PlateFunctionMu2.IsEnabled = false;
                PlateFunctionMuSlider2.IsEnabled = false;
                PlateSubstance2.IsEnabled = false;
            }
            else
            {
                PlateFunctionF2.IsEnabled = true;
                PlateFunctionPhi2.IsEnabled = true;
                PlateFunctionMu2.IsEnabled = true;
                PlateFunctionMuSlider2.IsEnabled = true;
                PlateSubstance2.IsEnabled = false;
                init_Plate2();
                init_HeatMap2();
            }

            if (blockPlateFunctionU2)
            {
                return;
            }

            try
            {
                exprU2 = new NCalc.Expression(utext);
                init_Plate2();
                init_HeatMap2();
            }
            catch
            {
            }
        }

        private void PlateReset1_Click(object sender, RoutedEventArgs e)
        {
            BlockPlateOptiopns1();

            init_PlateOptions1();
            init_Expr1();
            init_Plate1();
            init_HeatMap1();

            UnblockPlateOptiopns1();
        }

        private void PlateReset2_Click(object sender, RoutedEventArgs e)
        {
            BlockPlateOptiopns2();

            init_PlateOptions2();
            init_Expr2();
            init_Plate2();
            init_HeatMap2();

            UnblockPlateOptiopns2();
        }

        private void PlateResetTime1_Click(object sender, RoutedEventArgs e)
        {

            init_Plate1();
            init_HeatMap1();
        }
        private void PlateResetTime2_Click(object sender, RoutedEventArgs e)
        {

            init_Plate2();
            init_HeatMap2();
        }

        private void HelpMenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Help.ShowHelp(null, @".\PlateModel.chm");
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown(); 
        }
    }
}
