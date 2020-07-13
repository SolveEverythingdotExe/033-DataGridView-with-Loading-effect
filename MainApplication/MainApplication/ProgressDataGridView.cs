using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace MainApplication
{
    /*
    NOTE: This is an improved version compared to what is shown in the video it has an additional feature of showing the
    DataGrid's cells as disabled when the loading cursor is showing.
    */
    public class ProgressDataGridView: DataGridView
    {
        private BackgroundWorker AnimationThread = new BackgroundWorker();
        private BackgroundWorker DataThread = new BackgroundWorker();

        private int CurrentAngle = 0;
        private bool ShowLoadingCursor = false;

        private Bitmap GridCellsImageCopy;
        private ControlsRectangle GridRectangle = new ControlsRectangle();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out ControlsRectangle lpRect);
        [StructLayout(LayoutKind.Sequential)]
        private struct ControlsRectangle
        {
            public int Left;
            public int Top;
            public int Right; 
            public int Bottom;
            public int Width { get { return Right - Left; } }
            public int Height { get { return Bottom - Top; } }
        }

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
            GridCellsImageCopy = null;
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
                Thread.Sleep(75);
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

                //BUGFIX/WORKAROUND ======> pixelation
                //I think it is because the loading cursor that we are painting has no reference color to blend with
                //since it is like a floating image on top of the DataGrid, thats why there is still a pixelation.
                //So the workaround will be to paint an image which will serve as the base/background image so that
                //the loading cursor will have a reference color to blend and remove the pixelation.
                //It also serves as a way to show that the grids cells are disabled, because the data is loading.
                int x = RowHeadersVisible ? RowHeadersWidth : 0;
                int y = ColumnHeadersVisible ? ColumnHeadersHeight : 0;
                graphics.DrawImage(GridCellsImageCopy, x, y);

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

        private void GetGridBodyAndSaveToImage()
        {
            //get the rectangle of the DataGridView in the Screen (not in the form) or the actual position of the
            //DataGridView in the Screen monitor
            GetWindowRect(Handle, out GridRectangle);

            //calculate the dimension of the bitmap and create it
            int rowHeadsWidth = RowHeadersVisible ? RowHeadersWidth : 0;
            int columnHeadsHeight = ColumnHeadersVisible ? ColumnHeadersHeight : 0;
            int width = Width - rowHeadsWidth - 1;
            int height = Height - columnHeadsHeight - 1;
            GridCellsImageCopy = new Bitmap(width, height);

            //copy the DataGrids content to the bitmap but don't include the rowheader and columnheader
            using (Graphics bitmapGraphics = Graphics.FromImage(GridCellsImageCopy))
            {
                bitmapGraphics.CopyFromScreen(GridRectangle.Left + rowHeadsWidth, GridRectangle.Top + columnHeadsHeight,
                    0, 0, new Size(GridRectangle.Width, GridRectangle.Height), CopyPixelOperation.SourceCopy);
            }

            //make the image grayscale only if the grid cells are not empty
            if (Rows.Count > 0)
                GridCellsImageCopy = (Bitmap)ToolStripRenderer.CreateDisabledImage(GridCellsImageCopy);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //disable the painting of the DataGridView when the loading cursor is showing
            if (ShowLoadingCursor) return;

            base.OnPaint(e);
        }

        //public method to execute everything
        public void LoadData(string connectionString, string commandText, bool autoGenerateColumns)
        {
            //BUGFIX ======> if one of the threads is running stop the execution
            if (AnimationThread.IsBusy || DataThread.IsBusy) return;

            //set the flagging that the loading cursor will show
            ShowLoadingCursor = true;

            //get the grid's content and save it to a Bitmap
            GetGridBodyAndSaveToImage();

            //start the animation of cursor
            AnimationThread.RunWorkerAsync();

            //load the data
            object[] arguments = new object[] { connectionString, commandText, autoGenerateColumns };
            DataThread.RunWorkerAsync(arguments);
        }
        //lets build and apply on the form
    }
}
