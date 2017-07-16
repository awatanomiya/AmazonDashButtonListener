/* 
 * DashButton.cs: 
 * Copyright 2017 Kazuki Awaki
 */
using System;
using System.Collections.Generic;
using SharpPcap;

namespace AmazonDashButton
{
    public class DashButtonListener
    {
        ICaptureDevice device;
        List<DashButton> dashButtons　= new List<DashButton>();
        
        public DashButtonListener(string InterfaceName)
        {
            foreach (var dev in CaptureDeviceList.Instance)
            {
                if(((dynamic)dev).Interface.FriendlyName == InterfaceName)
                {
                    device = dev;
                    break;
                }
            }

            if (device == null)
                throw new Exception("No device found");

            device.OnPacketArrival += DeviceOnPacketArrival;
        }

        public void StartCapture()
        {
            device.Open();
            device.StartCapture();
        }
        public void StopCapture()
        {
            device.StopCapture();
            device.Close();
        }

        public DashButton AddDashButton(byte[] MacAdress)
        {
            return AddDashButton(new DashButton(MacAdress));
        }
        public DashButton AddDashButton(DashButton dashButton)
        {
            dashButtons.Add(dashButton);
            return dashButton;
        }

        private bool CheckDestMacAdressIsBroadcastAdress(byte[] payload)
        {
            for(int i = 0; i < 6; i++)
            {
                if (payload[i] != 0xff)
                    return false;
            }

            return true;
        }
        private bool CheckSrcMacAdressIsSame(byte[] payload, byte[] targetAdress)
        {
            const int offsetOfSrcMacAdress = 6;
            for (int i = 0; i < 6; i++)
            {
                if (payload[i + offsetOfSrcMacAdress] != targetAdress[i])
                    return false;
            }

            return true;
        }
        private void DeviceOnPacketArrival(object sender, CaptureEventArgs e)
        {
            var payload = e.Packet.Data;
            var packetProtocol = payload[12] * 256 + payload[13];
            
            // Get rid of packets other than ARP
            if (packetProtocol != 0x0806) return;

            // Get rid of not broadcasting packet
            if (!CheckDestMacAdressIsBroadcastAdress(payload)) return;

            foreach (var dashbutton in dashButtons)
            {
                if(CheckSrcMacAdressIsSame(payload, dashbutton.MacAdress))
                {
                    dashbutton.Pressed();
                }
            }
        }
    }
    public class DashButton {
        public byte[] MacAdress { private set; get; }
        public DashButton(byte[] macAdress)
        {
            MacAdress = macAdress;
        }
        public void Pressed()
        {
            OnPressed(this, null);
        }
        public event EventHandler OnPressed;
    }
}
