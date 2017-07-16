/* 
 * Program.cs: 
 * Copyright 2017 Kazuki Awaki
 */
using System;

namespace AmazonDashButton
{
    class Program
    {
        /// <summary>
        /// DashButtonが押されたかを監視するためのリスナ
        /// </summary>
        static DashButtonListener dashButtonListener;

        static void Main(string[] args)
        {
            /* リスナの初期化
             * 引数はネットワークの名称(ネットワークと共有センタのアクティブなネットワーク接続 > 接続)
             */
            dashButtonListener = new DashButtonListener("ワイヤレス ネットワーク接続 4");

            // DashButtonのMacAdressを設定
            var dashButton = dashButtonListener.AddDashButton(new byte[] { 252, 166, 103, 164, 220, 166 });
            
            // DashButtonが押されたときの動作を定義
            dashButton.OnPressed += (sender, e) => {
                Console.WriteLine("ボタンが押されました");
            };
            
            dashButtonListener.StartCapture();

            Console.WriteLine("Press Enter To Stop.");
            Console.ReadLine();

            dashButtonListener.StopCapture();
        }
    }
}