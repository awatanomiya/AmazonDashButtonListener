using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonDashButton
{
    class Program
    {
        static DashButtonListener dashButtonListener;
        static void Main(string[] args)
        {
            dashButtonListener = new DashButtonListener("ワイヤレス ネットワーク接続 4");
            dashButtonListener.AddDashButton(new byte[] { 252, 166, 103, 164, 220, 166 }).OnPressed += (sender, e) => {
                Console.WriteLine("ボタンが押されました");
            };
            dashButtonListener.StartCapture();
            Console.WriteLine("Press Enter To Stop.");
            Console.ReadLine();
            dashButtonListener.StopCapture();
        }
    }
}