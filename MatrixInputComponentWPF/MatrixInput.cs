using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonRessources;
using System.Threading;
using System.Text.RegularExpressions;

namespace MatrixInputComponentWPF
{
    class MatrixInput : IComponent
    {
        private Guid componentGuid;

        private string friendlyName;

        private IEnumerable<string> inputHints;

        private IEnumerable<string> outputHints;

        private IEnumerable<string> inputDescriptions;

        private IEnumerable<string> outputDescriptions;

        private List<int[,]> matrix;

        private MainWindow inputBox;

        public MatrixInput()
        {
            this.componentGuid = new Guid("DE8FFB30-360D-4666-B89A-80DEE47474B6");

            this.friendlyName = "Matrix-Input";

            this.inputHints = new List<String>() {typeof(string).ToString()};

            this.outputHints = new List<string>() { typeof(int[,]).ToString() };

            this.inputDescriptions = new List<string>() {"Parameter: Job Identifier"};

            this.outputDescriptions = new List<string>() { "Two dimensional integer array [,] representing a Matrix." };
        }
        public Guid ComponentGuid
        {
            get { return this.componentGuid; }
        }

        public string FriendlyName
        {
            get { return this.friendlyName; }
        }

        public IEnumerable<string> InputHints
        {
            get { return this.inputHints; }
        }

        public IEnumerable<string> OutputHints
        {
            get { return this.OutputHints; }
        }

        public IEnumerable<string> InputDescriptions
        {
            get
            {
                return this.inputDescriptions;
            }
            set
            {
               this.inputDescriptions = value;
            }
        }

        public IEnumerable<string> OutputDescriptions
        {
            get
            {
                return this.outputDescriptions;
            }
            set
            {
                this.outputDescriptions = value;
            }
        }

        public IEnumerable<object> Evaluate(IEnumerable<object> values)
        {
            var x = values.ToArray();

            if (x.Length != 1)
            {
                throw new ArgumentException("There must be only one string in the value list: Job desciption");
            }
            else
            {
                var t = new Thread(new ParameterizedThreadStart(Instantiate));
                t.SetApartmentState(ApartmentState.STA);
                var y = values.Any() ? values.First() : "no parameter";
                t.Start(y);
                t.IsBackground = true;
                t.Join();
                return this.matrix;
            }
        }

        private void Instantiate(object obj)
        {
            string info = (string)obj;
            inputBox = new MainWindow(info);
            inputBox.MouseLeftButtonDown += inputBox_MouseLeftButtonDown;
            System.Windows.Application application = new System.Windows.Application();
            application.Run(inputBox);
        }

        void inputBox_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string toConvert = (string)sender; 

            if (testRegEx(toConvert))
            {
                toConvert = toConvert.Substring(1, toConvert.Length - 2);
                string[] splitted = toConvert.Split(';');

                List<int[]> rows = new List<int[]>();

                for (int i = 0; i < splitted.Length; i++)
                {
                    string[] temp = splitted[i].Split(',');
                    int[] arr = new int[temp.Length];

                    for (int j = 0; j < temp.Length; j++)
                    {
                        arr[j] = int.Parse(temp[j]);
                    }

                    rows.Add(arr);
                }

                int[,] matrix = new int[rows.Count, rows[0].Length];

                for (int i = 0; i < rows.Count; i++)
                {
                    for (int j = 0; j < rows[i].Length; j++)
                    {
                        matrix[i, j] = rows[i][j];
                    }
                }

                this.matrix.Add(matrix);
            }
            else
            {
                throw new ArgumentException("Error: Wrong format! Couldn't convert string to int[,]!");
            }
        }
        private bool testRegEx(string eval)
        {
            string query = "(\\[([0-9]+(,[0-9]+)*;)*([0-9]+(,[0-9]+)*\\]))";
            string query1 = "([[]])";
            if (Regex.IsMatch(eval.Trim(), query1))
                return true;

            return Regex.IsMatch(eval.Trim(), query);
        }

    }
}
