using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainApplication
{
    public class ProgressDataGridView: DataGridView
    {
        private BackgroundWorker AnimationThread = new BackgroundWorker();
        private BackgroundWorker DataThread = new BackgroundWorker();

        private int CurrentAngle = 0;
        private bool ShowLoadingCursor = false;

        //constructor
        public ProgressDataGridView()
        {
            //set the event of the worker threads
            AnimationThread.DoWork += AnimationThread_DoWork;

            DataThread.DoWork += DataThread_DoWork;
            DataThread.RunWorkerCompleted += DataThread_RunWorkerCompleted;
        }

        private void DataThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //once the data is loaded, stop the loading cursor and repaint the datagrid
            ShowLoadingCursor = false;
            Invalidate();
        }

        private void DataThread_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] arguments = (object[])e.Argument;
            string connectionString = (string)arguments[0];
            string commandText = (string)arguments[1];
            bool autoGenerateColumns = (bool)arguments[2];

            using (SqlConnection Connection = new SqlConnection(connectionString))
            {
                using (SqlDataAdapter DataAdapter = new SqlDataAdapter(commandText, Connection))
                {

                    DataSet Dataset = new DataSet();
                    DataTable Datatable = new DataTable();

                    //load to dataset
                    DataAdapter.Fill(Dataset);
                    Datatable = Dataset.Tables[0];

                    //loading of data to datagrid should execute on the UI/main thread
                    Invoke(new Action(() =>
                    {
                        DataSource = Datatable;
                        AutoGenerateColumns = autoGenerateColumns;
                    }));
                }
            }
        }

        private void AnimationThread_DoWork(object sender, DoWorkEventArgs e)
        {
            while (ShowLoadingCursor)
            {
                //calculate the angle of the animation
                CurrentAngle += 45;

                if (CurrentAngle > 360)
                    CurrentAngle = 0;

                //paint the animation
                PaintLoadingCursor();

                //animation effect/delay
                Thread.Sleep(100);
            }
        }

        private void PaintLoadingCursor()
        {
            //get the graphics object of the datagridview
            using (Graphics graphics = CreateGraphics())
            {
                //remove pixelation
                graphics.SmoothingMode = SmoothingMode.AntiAlias;

                //calculate the size and position of the cursor
                int cursorSize = 30;
                int cursorX = (Width / 2) - (cursorSize / 2);
                int cursorY = (Height / 2) - (cursorSize / 2);
                int brushWidth = 6;

                //BUGFIX/WORKAROUND ======> pixelation when the grid has no rows
                if (Rows.Count == 0)
                {
                    int backgroundX = cursorX - (brushWidth / 2);
                    int backgroundY = cursorY - (brushWidth / 2);
                    int backroundSize = cursorSize + brushWidth + 1;

                    //create a base which color is the same with the datagrid
                    using (SolidBrush brush = new SolidBrush(BackgroundColor))
                    {
                        graphics.FillRectangle(brush, backgroundX, backgroundY, backroundSize, backroundSize);
                    }
                }

                //draw base image
                using (LinearGradientBrush brush = new LinearGradientBrush(ClientRectangle, Color.FromArgb(93, 93, 93), Color.FromArgb(0, 0, 255), LinearGradientMode.Vertical))
                {
                    using (Pen pen = new Pen(brush, brushWidth))
                    {
                        pen.DashStyle = DashStyle.Dot;
                        graphics.DrawArc(pen, cursorX, cursorY, cursorSize, cursorSize, 0, 360);
                    }
                }

                //draw the animation effect
                using (SolidBrush brush = new SolidBrush(Color.White))
                {
                    using (Pen pen = new Pen(brush, brushWidth))
                    {
                        pen.DashStyle = DashStyle.Dot;
                        graphics.DrawArc(pen, cursorX, cursorY, cursorSize, cursorSize, CurrentAngle, 90);
                    }
                }
            }
        }

        //public method to execute everything
        public void LoadData(string connectionString, string commandText, bool autoGenerateColumns)
        {
            //BUGFIX ======> if one of the threads is running stop the execution
            if (AnimationThread.IsBusy || DataThread.IsBusy) return;

            //show the loading cursor
            ShowLoadingCursor = true;
            AnimationThread.RunWorkerAsync();

            //load the data
            object[] arguments = new object[] { connectionString, commandText, autoGenerateColumns };
            DataThread.RunWorkerAsync(arguments);
        }
        //lets build and apply on the form
    }
}
