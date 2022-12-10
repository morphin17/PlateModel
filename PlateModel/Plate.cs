using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateModel
{
    public class Plate
    {

        public double a;
        public double b;

        public double h1;
        public double h2;

        public double tau;
        public double curT;

        public double sigma1;
        public double sigma2;
        private double gamma1;
        private double gamma2;
        private double ro;
        public double maxR = 0;


        private Func<double, double> mu;
        private Func<double, double, double> phi;
        private Func<double, double, double, double> f;
        private Func<double, double, double, double> u;
        /// <summary>
        /// Пластина в текущий момент времени
        /// </summary>
        public double[,] U;
        public double[,] solutionU;
        public double[,] FF;

        /// <summary>
        /// Инициализация входных параметров
        /// </summary>
        /// <param name="ro">Коэффициент температуропроводности</param>
        /// <param name="a">Длина пластины по x</param>
        /// <param name="b">Длина пластины по y</param>
        /// <param name="h1">Шаг по x</param>
        /// <param name="h2">Шаг по y</param>
        /// <param name="tau">Шаг по времени</param>
        /// <param name="phi">Функция при краевом условии t=0</param>
        /// <param name="f">Функция нагрева</param>
        /// <param name="mu">Функция при кравеаом условии на границах пластины</param>
        /// <param name="u">Аналитическое решение</param>
        public Plate(double ro, double a, double b, double h1, double h2, double tau,
            Func<double, double, double> phi, Func<double, double, double, double> f, Func<double, double> mu, 
            Func<double, double, double, double> u
            )
        {
            this.a = a;
            this.b = b;
            this.h1 = h1;
            this.h2 = h2;
            this.tau = tau;
            this.ro = ro;

            this.sigma1 = 0.5 * (ro - h1 * h1 / (6 * tau));
            this.sigma2 = 0.5 * (ro - h2 * h2 / (6 * tau));
            this.gamma1 = ro*tau / (h1 * h1);
            this.gamma2 = ro*tau / ( h2 * h2);

            this.mu = mu;
            this.phi = phi;
            this.f = f;
            this.u = u;

            this.curT = 0;
            this.U = new double[Convert.ToInt64(a / h1) + 1, Convert.ToInt64(b / h2) + 1];
            this.solutionU = new double[Convert.ToInt64(a / h1) + 1, Convert.ToInt64(b / h2) + 1];
            this.FF = new double[Convert.ToInt64(a / h1) + 1, Convert.ToInt64(b / h2) + 1];

            //Пластина в нулевой момент времени
            for (int i = 0; i < this.U.GetLength(0); i++)
            {
                for (int j = 0; j < this.U.GetLength(1); j++)
                {
                    this.U[i, j] = phi(h1 * i, h2 * j);
                }
            }

            //Краевые условия на краях пластины
            this.conditionT();

            this.calcSolution(0);
        }

        private void calcFF()
        {
            for (int i = 0; i < this.U.GetLength(0); i++)
            {
                for (int j = 0; j < this.U.GetLength(1); j++)
                {
                    this.FF[i, j] = f(h1 * i, h2 * j, curT + tau / 2);
                }
            }
        }


        public void nextSolution()
        {
            curT += tau;
            calcSolution(curT);
        }

        /// <summary>
        /// Посчитать сетку для аналитического решения
        /// </summary>
        private void calcSolution(double curT)
        {
            for (int i = 0; i < this.U.GetLength(0); i++)
            {
                for (int j = 0; j < this.U.GetLength(1); j++)
                {
                    this.solutionU[i, j] = u(h1 * i, h2 * j, curT);
                }
            }
        }

        /// <summary>
        /// Краевые условия на краях пластины
        /// </summary>
        private void conditionT()
        {
            for (int i = 0; i < U.GetLength(0); i++)
            {
                U[i, 0] = mu(curT);
                U[i, U.GetLength(1) - 1] = mu(curT);
            }

            for (int j = 0; j < U.GetLength(1); j++)
            {
                U[0, j] = mu(curT);
                U[U.GetLength(0) - 1, j] = mu(curT);
            }
        }

        /// <summary>
        /// Вспомогательная функция для подсчета схемы
        /// </summary>
        /// <param name="U">Пластина</param>
        /// <param name="i">Координаты</param>
        /// <param name="j">Координаты</param>
        /// <returns></returns>
        private double F(double[,] U, int i, int j)
        {

            return (ro - sigma1) * (U[i - 1, j] - 2 * U[i, j] + U[i + 1, j]) / (h1 * h1) +
                (ro - sigma2) * (U[i, j - 1] - 2 * U[i, j] + U[i, j + 1]) / (h2 * h2);

                //(ro - sigma1) * (ro - sigma2) * tau *
                //(U[i - 1, j - 1] - 2 * U[i - 1, j] + U[i - 1, j + 1] - 2 * U[i, j - 1] + 4 * U[i, j] - 2 * U[i, j + 1] + U[i + 1, j - 1] - 2 * U[i + 1, j] + U[i + 1, j + 1]) /
                //((h1 * h1) * (h2 * h2)) +
                //((double)1 / 12) * (FF[i - 1, j] - 4 * FF[i, j] + FF[i + 1, j] + FF[i, j - 1] + FF[i, j + 1]) + FF[i, j];
        }

        /// <summary>
        /// Переход на следующий временной слой
        /// </summary>
        public void nextU()
        {
            calcFF();
            Double[,] N;
            double[] alpha;
            double[] beta;

            N = Copy(U);
            alpha = new double[U.GetLength(0) + 1];
            beta = new double[U.GetLength(0) + 1];

            //Прогонка по x
            for (int j = 1; j < U.GetLength(1) - 1; j++)
            {
                alpha[1] = 0;
                beta[1] = N[0, j];

                for (int i = 1; i < U.GetLength(0) - 1; i++)
                {
                    alpha[i + 1] = gamma1 / (1 + 2 * gamma1 - alpha[i] * gamma1);
                    beta[i + 1] = (N[i, j] + tau / 2 * FF[i,j] + gamma1 * beta[i]) / (1 + 2 * gamma1 - alpha[i] * gamma1);
                }

                for (int i = U.GetLength(0) - 2; i > 0; i--)
                {
                    U[i, j] = alpha[i + 1] * U[i + 1, j] + beta[i + 1];
                }
            }

            N = Copy(U);
            alpha = new double[U.GetLength(1) + 1];
            beta = new double[U.GetLength(1) + 1];

            //Прогонка по y
            for (int i = 1; i < U.GetLength(0) - 1; i++)
            {
                alpha[1] = 0;
                beta[1] = N[i, 0];

                for (int j = 1; j < U.GetLength(1) - 1; j++)
                {
                    alpha[j + 1] = gamma2 / (1 + 2 * gamma2 - alpha[j] * gamma2);
                    beta[j + 1] = (N[i, j] + tau / 2 * FF[i, j] + gamma2 * beta[j]) / (1 + 2 * gamma2 - alpha[j] * gamma2);
                }

                for (int j = U.GetLength(1) - 2; j > 0; j--)
                {
                    U[i, j] = alpha[j + 1] * U[i, j + 1] + beta[j + 1];
                }
            }

            curT += tau;
            conditionT();
        }

        static T[,] Copy<T>(T[,] array)
        {
            T[,] newArray = new T[array.GetLength(0), array.GetLength(1)];
            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array.GetLength(1); j++)
                    newArray[i, j] = array[i, j];
            return newArray;
        }

    }

}
