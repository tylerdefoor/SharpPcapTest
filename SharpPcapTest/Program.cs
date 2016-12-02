using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpPcap;
using System.Net;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Collections.ObjectModel;
using PacketDotNet;

namespace SharpPcapTest
{


    class Program
    {
        /*
        public string Traceroute(string ipAddressOrHostName)
        {
            IPAddress ipAddress = Dns.GetHostEntry(ipAddressOrHostName).AddressList[0];
            StringBuilder traceResults = new StringBuilder();
            using (Ping pingSender = new Ping())
            {
                PingOptions pingOptions = new PingOptions();
                Stopwatch stopWatch = new Stopwatch();

                byte[] bytes = new byte[32];
                pingOptions.DontFragment = true;
                pingOptions.Ttl = 1;
                int maxHops = 30;

                traceResults.AppendLine(
                    string.Format(
                        "Tracing route to {0} over a maximum of {1} hops:",
                        ipAddress,
                        maxHops));

                traceResults.AppendLine();

                for (int i = 1; i < maxHops + 1; i++)
                {
                    stopWatch.Reset();
                    stopWatch.Start();
                    PingReply pingReply = pingSender.Send(
                        ipAddress,
                        5000,
                        new byte[32], pingOptions);

                    stopWatch.Stop();

                    traceResults.AppendLine(
                        string.Format("{0}\t{1} ms\t{2}",
                        i,
                        stopWatch.ElapsedMilliseconds,
                        pingReply.Address));
                    
                    if (pingReply.Status == IPStatus.Success)
                    {
                        traceResults.AppendLine();
                        traceResults.AppendLine("Trace complete."); break;
                    }

                    pingOptions.Ttl++;
                }
            }
            return traceResults.ToString();
        }
        */


        static void Main(string[] args)
        {
            // Retrieve the device list
            CaptureDeviceList devices = CaptureDeviceList.Instance;

            // If no devices were found print an error
            if (devices.Count < 1)
            {
                Console.WriteLine("No devices were found on this machine");
                return;
            }

            // Extract a device from the list
            ICaptureDevice device = devices[0];

            // Open the device for capturing
            int readTimeoutMilliseconds = 1000;
            device.Open(DeviceMode.Promiscuous, readTimeoutMilliseconds);

            Console.WriteLine();
            Console.WriteLine("-- Listening on {0}...",
                device.Description);

            RawCapture packet = null;

            // Keep capture packets using GetNextPacket()
            while ((packet = device.GetNextPacket()) != null)
            {
                // Prints the time and length of each received packet
                DateTime time = packet.PcapHeader.Date;
                int len = packet.PcapHeader.PacketLength;
                Console.WriteLine("{0}:{1}:{2},{3} Len={4}",
                    time.Hour, time.Minute, time.Second,
                    time.Millisecond, len);
            }

            // Close the pcap device
            device.Close();
            Console.WriteLine(" -- Capture stopped, device closed.");
        }
    }

}
