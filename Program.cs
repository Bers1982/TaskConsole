using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ThreadConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var frmDialog = new Form
            {
                Cursor = Cursors.Hand,
                StartPosition = FormStartPosition.CenterScreen
            };
            var lblDownload = new Label
            {
                ForeColor = Color.Black,
                BackColor = Color.Red,
                Font = new Font("Arial", 12),
                AutoSize = true,
                Name = "lblDownload",
                Text = "DOWNLOADING"
            };

            lblDownload.Location = new Point((frmDialog.ClientSize.Width - lblDownload.Width) / 2,
            (frmDialog.ClientSize.Height - lblDownload.Height) / 2);

            frmDialog.Controls.Add(lblDownload);
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Thread Started");

            var task = Task.Run(() => WriteConsole(frmDialog))
                .ContinueWith(t =>frmDialog.Close(),TaskScheduler.FromCurrentSynchronizationContext());
            frmDialog.ShowDialog();
          
            task.Wait();
            
            Console.ReadLine();
        }

        private static void WriteConsole(object obj)
        {
            var frmDialog = obj as Form;
            var lblDownload = frmDialog.Controls["lblDownload"] as Label;
            var text = lblDownload.Text;
            int k = 0;
            Action action = () => {
                k++;
                lblDownload.Text += new string('.', k);
                if (k == 8)
                {
                    k = 0;
                    lblDownload.Text = text;
                }
            };
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            for (int i = 0; i < 100; i++)
            {
                Console.Write($"{i}");                
                frmDialog.Invoke(action);
                Thread.Sleep(100);
            }
        }
    }
}
