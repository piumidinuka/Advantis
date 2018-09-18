using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Advantech.Adam;
using System.Threading;
using System.Windows;

namespace Advantis
{
    class AdamControl
    {
        int IO;
        int port = 502;
        string IP = "192.168.1.22";
        AdamSocket AdamTCP;

        public AdamControl(int io, string ip)
        {
            this.IO = io;
            this.IP = ip;
            AdamTCP = new AdamSocket();
        }

        public AdamControl(int io)
        {
            this.IO = io;
            AdamTCP = new AdamSocket();
        }

            public void BulbOn()
            {
                if (AdamTCP.Connect(IP, System.Net.Sockets.ProtocolType.Tcp, port))
                {
                    AdamTCP.Modbus().ForceSingleCoil(IO, true);
                }
                else
                {
                    Console.WriteLine("Error! cant connect to the adam");
                MessageBox.Show("Error! cant connect to the adam");
                }
            }

            public void BulbOff()
            {
                if (AdamTCP.Connect(IP, System.Net.Sockets.ProtocolType.Tcp, port))
                {
                    AdamTCP.Modbus().ForceSingleCoil(IO, false);
                }
                else
                {
                    Console.WriteLine("Error! cant connect to the adam");
                MessageBox.Show("Error! cant connect to the adam");
            }
            }
        public static int b = 0;
        public void BulbOnOff(int a)
        {
            

            if (AdamTCP.Connect(IP, System.Net.Sockets.ProtocolType.Tcp, port))
            {
                if (a == 1)
                {
                    AdamTCP.Modbus().ForceSingleCoil(IO, true);
                    ++b;
                    if (b == 1) {
                        Thread t1 = new Thread(new ThreadStart(A));
                        t1.Start();
                        BulbOnOff(0);
                      
                        
                    }
                 
                }
                if (a == 0)
                {
                    AdamTCP.Modbus().ForceSingleCoil(IO, false);
                    b = 0;
                }
                
            }
            else
            {
                Console.WriteLine("Error! cant connect to the adam");
                MessageBox.Show("Error! cant connect to the adam");
            }
        }
        static void A() {
            Thread.Sleep(3500);
        }
    }
}

