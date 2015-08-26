using OpenPop;
using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using OpenPop.Mime;
using OpenPop.Pop3.Exceptions;

namespace emailTest
{
    class connectGmail
    {
        Pop3Client popClient;
        Form c;
        public connectGmail(Form c) {

            popClient = new Pop3Client();
            this.c = c;
        }

        public void LoadEmail(String uName, String uPwd, DataGridView dataGridViewMenu, ProgressBar pb)
        {
            

            // gmail
            if (popClient.Connected)
                popClient.Disconnect();

            popClient.Connect("pop.gmail.com", 995, true);
            try
            {
                popClient.Authenticate(uName, uPwd);

                // 信件數量
                int Count = popClient.GetMessageCount();

                pb.Maximum = Count;
                pb.Minimum = 0;
                pb.Step = 1;
                pb.Value = 0;
                pb.Visible = true;

                int success = 0;
                int fail = 0;
                for (int i = Count; i >= 1; i -= 1)
                {

                    // 取得信件
                    OpenPop.Mime.Message m = popClient.GetMessage(i);


                    DataGridViewRowCollection rows = dataGridViewMenu.Rows;
                    if (m != null)
                    {
                        success++;
                        

                        System.Console.WriteLine("[" + i + "] " + m.Headers.Subject);

                        String t = "";
                        if (m.FindFirstPlainTextVersion() != null)
                            t = m.FindFirstPlainTextVersion().GetBodyAsText();

                        rows.Add(new Object[] { i, m.Headers.From, m.Headers.Subject, t });
                        pb.Value += pb.Step;//讓進度條增加一次
                        pb.Text = (i + 1) + " / " + Count;

             
                        
                    }
                    else
                    {
                        fail++;
                    }
                }
                pb.Visible = false;
                System.Console.WriteLine("Mail received!\nSuccess: " + success + "\nFailed: " + fail);
            }
            catch (InvalidLoginException)
            {
                MessageBox.Show(c, "The server did not accept the user credentials!", "POP3 Server Authentication");
            }
            catch (PopServerNotFoundException)
            {
                MessageBox.Show(c, "The server could not be found", "POP3 Retrieval");
            }
            catch (PopServerLockedException)
            {
                MessageBox.Show(c, "The mailbox is locked. It might be in use or under maintenance. Are you connected elsewhere?", "POP3 Account Locked");
            }
            catch (LoginDelayException)
            {
                MessageBox.Show(c, "Login not allowed. Server enforces delay between logins. Have you connected recently?", "POP3 Account Login Delay");
            }
            catch (Exception e)
            {
                MessageBox.Show(c, "Error occurred retrieving mail. " + e.Message, "POP3 Retrieval");
            }
        }
    }
}
