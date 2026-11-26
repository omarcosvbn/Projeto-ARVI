using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace YawVR {
    public enum ControllerState { Disconnected, Connecting, Connected, Started }

    public class YawController : MonoBehaviour {
        public static YawController Instance { get; private set; }

        const int DevicePort = 50010;
        const byte CheckIn = 0x30, CheckInAnswer = 0x31, StartCmd = 0xA1, StopCmd = 0xA2, ExitCmd = 0xA3, GetState = 0xE5;

        [SerializeField] private string gameName;
        [SerializeField] private int udpClientPort;
        [SerializeField] private MotionCompensation cancellation;

        public ControllerState State { get; private set; }
        public float DeviceYaw { get; private set; }
        public YawTracker Tracker { get; private set; }

        private Orientation orientation;
        private UdpClient udp;
        private TcpClient tcp = new TcpClient();
        private IPEndPoint deviceEndPoint;
        private float nextHeartbeat;

        void Awake() {
            Instance = this;
            orientation = GetComponentInChildren<Orientation>();
            Tracker = GetComponentInChildren<YawTracker>();
            udp = new UdpClient(udpClientPort);
            ReceiveUdp();
        }

        IEnumerator Start() {
            var call = Encoding.ASCII.GetBytes("YAW_CALLING");
            var broadcast = new IPEndPoint(IPAddress.Broadcast, DevicePort);
            while (true) {
                if (State == ControllerState.Disconnected) {
                    udp.Send(call, call.Length, broadcast);
                }
                yield return new WaitForSeconds(1);
            }
        }

        void Update() {
            if (State >= ControllerState.Connected && Time.time >= nextHeartbeat) {
                nextHeartbeat = Time.time + 1;
                SendTcp(GetState);
            }
        }

        void FixedUpdate() {
            if (State < ControllerState.Connected) {
                return;
            }
            var message = $"Y[{Format(orientation.yaw)}]P[{Format(orientation.pitch)}]R[{Format(orientation.roll)}]V[0,0,0,0]F[0,0]";
            var data = Encoding.ASCII.GetBytes(message);
            udp.Send(data, data.Length, deviceEndPoint);
        }

        void OnDestroy() {
            if (State >= ControllerState.Connected) {
                SendTcp(ExitCmd);
            }
            tcp.Close();
            udp.Close();
        }

        static string Format(float angle) {
            return angle.ToString("000.000", CultureInfo.InvariantCulture);
        }

        async void ReceiveUdp() {
            try {
                while (true) {
                    var result = await udp.ReceiveAsync();
                    var message = Encoding.ASCII.GetString(result.Buffer);
                    if (message.StartsWith("YAWDEVICE")) {
                        var parts = message.Split(';');
                        if (State == ControllerState.Disconnected && parts[4] == "AVAILABLE") {
                            Connect(result.RemoteEndPoint.Address, int.Parse(parts[3]));
                        }
                    } else {
                        var yaw = Regex.Match(message, @"Y\[(-?[0-9.]+)\]");
                        if (yaw.Success) {
                            DeviceYaw = float.Parse(yaw.Groups[1].Value, CultureInfo.InvariantCulture);
                        }
                    }
                }
            } catch (Exception e) {
                Debug.Log(e.Message);
            }
        }

        async void Connect(IPAddress ip, int port) {
            State = ControllerState.Connecting;
            tcp = new TcpClient();
            try {
                await tcp.ConnectAsync(ip, port);
                deviceEndPoint = new IPEndPoint(ip, DevicePort);
                var udpPort = ((IPEndPoint)udp.Client.LocalEndPoint).Port;
                var portBytes = new[] { (byte)(udpPort >> 24), (byte)(udpPort >> 16), (byte)(udpPort >> 8), (byte)udpPort };
                SendTcp(new[] { CheckIn }.Concat(portBytes).Concat(Encoding.ASCII.GetBytes(gameName)).ToArray());

                var buffer = new byte[4096];
                int count;
                while ((count = await tcp.GetStream().ReadAsync(buffer, 0, buffer.Length)) > 0) {
                    HandleTcp(buffer, count);
                }
            } catch (Exception e) {
                Debug.Log(e.Message);
            }
            State = ControllerState.Disconnected;
            tcp.Close();
        }

        void HandleTcp(byte[] data, int count) {
            switch (data[0]) {
                case CheckInAnswer:
                    if (Encoding.ASCII.GetString(data, 1, count - 1).Contains("AVAILABLE")) {
                        State = ControllerState.Connected;
                        cancellation.Invoke(nameof(MotionCompensation.UpdateOffset), 0.1f);
                        SendTcp(StartCmd);
                    } else {
                        tcp.Close();
                    }
                    break;
                case StartCmd:
                    State = ControllerState.Started;
                    break;
                case StopCmd:
                    State = ControllerState.Connected;
                    break;
                case ExitCmd:
                    tcp.Close();
                    break;
            }
        }

        void SendTcp(params byte[] data) {
            tcp.GetStream().Write(data, 0, data.Length);
        }
    }
}
