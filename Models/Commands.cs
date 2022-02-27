using System.IO.Ports;

namespace gRPC_uArm_API.Models
{
    public class Commands
    {
        private static bool debug;
        private static string current_response;
        private static string last_response;
        private static SerialPort serialPort;
        private static readonly List<Error> errors = new List<Error>
            {
                new Error { Name = 20, Description = "command not exist" },
                new Error { Name = 21, Description = "parameter error" },
                new Error { Name = 22, Description = "address out of range" },
                new Error { Name = 23, Description = "command buffer full" },
                new Error { Name = 24, Description = "power unconnected" },
                new Error { Name = 25, Description = "operation failure" }
            };
        public Commands(Setting setting)
        {
            serialPort = new SerialPort(setting.Port, 115200);
            serialPort.Open();
            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceived);
            debug = setting.Debug;
        }
        public static void Send(string command)
        {
            serialPort.WriteLine((debug ? "#n " : "") + command);
        }
        public static async Task<string> Recive()
        {
            while (current_response == last_response)
            {
                await Task.Delay(100);
            }
            last_response = current_response;
            return current_response.Split("|")[0];
        }
        private static void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            current_response = serialPort.ReadExisting();
            if (debug)
            {
                current_response.Remove(0, 3);
            }

            if (current_response.Contains("ok"))
            {
                current_response = "ok";
            }
            else if (current_response.Contains("E"))
            {
                try
                {
                    int ex = Convert.ToInt32(current_response.Substring(current_response.IndexOf("E") + 1, 2));
                    current_response = errors.Find(e => e.Name == ex).Description;
                }
                catch (Exception ex)
                {
                    current_response = ex.Message;
                }
            }
            current_response += "|" + new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
        }
    }
}
