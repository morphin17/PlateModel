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


        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();

            timer.Tick += new EventHandler(timer_Tick);

            timer.Interval = new TimeSpan(0, 0, 0, 0, 40);

            timer.Start();


            init_PlateOptions1();
            init_Expr1();
            init_Plate1();
            init_HeatMap1();

            init_PlateOptions2();
            init_Expr2();
            init_Plate2();
            init_HeatMap2();
        }

        private void MenuItem_Test1(object sender, RoutedEventArgs e)
        {
            init_PlateOptions1();
            init_PlateOptions2();

            PlateFunctionF1.Text = "0";
            PlateFunctionPhi1.Text = "Sin(pi*x/5)*Sin(pi*y/5)";
            PlateFunctionMu1.Text = "0";
            PlateWidth1.Value = 5;
            PlateWidthPointsCount1.Value = 30;
            PlateHeight1.Value = 5;
            PlateHeightPointsCount1.Value = 30;
            PlateTimeStep1.Value = 60;
            PlateFunctionU1.Text = "Sin(pi*x/5)*Sin(pi*y/5)*Pow(e, -0.000127*(pi*pi/(5*5) + pi*pi/(5*5))*t)";
            PlateSubstance1.SelectedIndex = 0;
            BottomBorderTemp1.Value = 0;
            TopBorderTemp1.Value = 1;

            PlateFunctionF2.Text = "0";
            PlateFunctionPhi2.Text = "Sin(pi*x/5)*Sin(pi*y/5)";
            PlateFunctionMu2.Text = "0";
            PlateWidth2.Value = 5;
            PlateWidthPointsCount2.Value = 30;
            PlateHeight2.Value = 5;
            PlateHeightPointsCount2.Value = 30;
            PlateTimeStep2.Value = 60;
            PlateSubstance2.SelectedIndex = 0;
            BottomBorderTemp2.Value = 0;
            TopBorderTemp2.Value = 1;

            init_Expr1();
            init_Expr2();

            init_Plate1();
            init_HeatMap1();
            init_Plate2();
            init_HeatMap2();
        }

        private void init_PlateOptions1()
        {
            PlateWidth1.Value = 10;
            PlateWidthPointsCount1.Value = 20;
            PlateHeight1.Value = 10;
            PlateHeightPointsCount1.Value = 20;
            PlateTimeStep1.Value = 10;
            PlateFunctionF1.Text = "0";
            PlateFunctionPhi1.Text = "0";
            PlateFunctionMu1.Text = "0";
            PlateFunctionU1.Text = "";
            PlateSubstance1.SelectedIndex = 0;
            BottomBorderTemp1.Value = 0;
            TopBorderTemp1.Value = 10;
        }
        private void init_PlateOptions2()
        {
            PlateWidth2.Value = 10;
            PlateWidthPointsCount2.Value = 20;
            PlateHeight2.Value = 10;
            PlateHeightPointsCount2.Value = 20;
            PlateTimeStep2.Value = 10;
            PlateFunctionF2.Text = "0";
            PlateFunctionPhi2.Text = "0";
            PlateFunctionMu2.Text = "0";
            PlateFunctionU2.Text = "";
            PlateSubstance2.SelectedIndex = 0;
            BottomBorderTemp2.Value = 0;
            TopBorderTemp2.Value = 10;
        }


        private void init_Expr1()
        {
            exprF1 = new NCalc.Expression(PlateFunctionF1.Text);
            exprPhi1 = new NCalc.Expression(PlateFunctionPhi1.Text);
            exprMu1 = new NCalc.Expression(PlateFunctionMu1.Text);
            if (PlateFunctionU1.Text != "")
            {
                exprU1 = new NCalc.Expression(PlateFunctionU1.Text);
            }
        }
        private void init_Expr2()
        {
            exprF2 = new NCalc.Expression(PlateFunctionF2.Text);
            exprPhi2 = new NCalc.Expression(PlateFunctionPhi2.Text);
            exprMu2 = new NCalc.Expression(PlateFunctionMu2.Text);
            if (PlateFunctionU2.Text != "")
            {
                exprU2 = new NCalc.Expression(PlateFunctionU2.Text);
            }
        }


        private void init_Plate1()
        {
            try
            {
                p1 = new Plate(
                    substance[PlateSubstance1.SelectedIndex],
                    Convert.ToDouble(PlateWidth1.Value),
                    Convert.ToDouble(PlateHeight1.Value),
                    Convert.ToDouble(PlateWidth1.Value) / Convert.ToDouble(PlateWidthPointsCount1.Value),
                    Convert.ToDouble(PlateHeight1.Value) / Convert.ToDouble(PlateHeightPointsCount1.Value),
                    Convert.ToDouble(PlateTimeStep1.Value),
                    phi1,
                    f1,
                    mu1,
                    u1
                );
            }
            catch
            {
                MessageBox.Show("Ошибка. Проверьте корректно ли вы ввели начальные условия для левой пластины.");
                return;
            }
        }
        private void init_Plate2()
        {
            try
            {
                p2 = new Plate(
                    substance[PlateSubstance2.SelectedIndex],
                    Convert.ToDouble(PlateWidth2.Value),
                    Convert.ToDouble(PlateHeight2.Value),
                    Convert.ToDouble(PlateWidth2.Value) / Convert.ToDouble(PlateWidthPointsCount2.Value),
                    Convert.ToDouble(PlateHeight2.Value) / Convert.ToDouble(PlateHeightPointsCount2.Value),
                    Convert.ToDouble(PlateTimeStep2.Value),
                    phi2,
                    f2,
                    mu2,
                    u2
                );
            }
            catch
            {
                MessageBox.Show("Ошибка. Проверьте корректно ли вы ввели начальные условия для левой пластины.");
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

            var hm = plt.AddHeatmap(data2D, lockScales: true);
            hm.Update(data2D, min: BottomBorderTemp1.Value, max: TopBorderTemp1.Value);

            hm.YAxisIndex = 1;
            hm.OffsetX = 0;
            hm.OffsetY = 0;
            hm.CellWidth = Convert.ToDouble(PlateWidth1.Value) / Convert.ToDouble(PlateWidthPointsCount1.Value);
            hm.CellHeight = Convert.ToDouble(PlateHeight1.Value) / Convert.ToDouble(PlateHeightPointsCount1.Value);
            plt.Margins(0, 0);

            WpfPlotPlate1.Refresh();
        }
        private void init_HeatMap2()
        {

            var plt = WpfPlotPlate2.Plot;


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

            hm.OffsetX = 0;
            hm.OffsetY = 0;
            hm.CellWidth = Convert.ToDouble(PlateWidth2.Value) / Convert.ToDouble(PlateWidthPointsCount2.Value);
            hm.CellHeight = Convert.ToDouble(PlateHeight2.Value) / Convert.ToDouble(PlateHeightPointsCount2.Value);
            plt.Margins(0, 0);

            WpfPlotPlate2.Refresh();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
        }

        private void next_Click(object sender, RoutedEventArgs e)
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
                MessageBox.Show("Ошибка. Проверьте начальные условия");
                return;
            }

            init_HeatMap1();
            init_HeatMap2();


            CurrentModelTime1.Text = "Текущее время:\n" + Convert.ToString(p1.curT);
            CurrentModelTime2.Text = "Текущее время:\n" + Convert.ToString(p2.curT);
        }


        private double f1(double x, double y, double t)
        {
            exprF1.Parameters["pi"] = Math.PI;
            exprF1.Parameters["e"] = Math.E;
            exprF1.Parameters["x"] = x;
            exprF1.Parameters["y"] = y;
            exprF1.Parameters["t"] = t;

            double r = Convert.ToDouble(exprF1.Evaluate());
            return r;
        }
        private double f2(double x, double y, double t)
        {
            exprF2.Parameters["pi"] = Math.PI;
            exprF2.Parameters["e"] = Math.E;
            exprF2.Parameters["x"] = x;
            exprF2.Parameters["y"] = y;
            exprF2.Parameters["t"] = t;

            double r = Convert.ToDouble(exprF2.Evaluate());
            return r;
        }

        private double phi1(double x, double y)
        {
            exprPhi1.Parameters["pi"] = Math.PI;
            exprPhi1.Parameters["e"] = Math.E;
            exprPhi1.Parameters["x"] = x;
            exprPhi1.Parameters["y"] = y;

            double r = Convert.ToDouble(exprPhi1.Evaluate());
            return r;
        }
        private double phi2(double x, double y)
        {
            exprPhi2.Parameters["pi"] = Math.PI;
            exprPhi2.Parameters["e"] = Math.E;
            exprPhi2.Parameters["x"] = x;
            exprPhi2.Parameters["y"] = y;

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

    }
}
