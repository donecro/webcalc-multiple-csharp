﻿using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace webcalc_multiple_csharp
{
    public abstract class HttpServer {
        private readonly int _port;

        private TcpListener _listener;
        
        bool is_active = true;

        protected HttpServer(int port) {
            _port = port;
        }

        public void Listen() {
            IPAddress addr = new IPAddress(new byte[] {127,0,0,1});
            _listener = new TcpListener(addr, _port);
            _listener.Start();
            while (is_active) {                
                HttpProcessor processor = new HttpProcessor(_listener.AcceptTcpClient(), this);
                Thread thread = new Thread(processor.Process);
                thread.Start();
                Thread.Sleep(1);
            }
        }

        public abstract void HandleGetRequest(HttpProcessor p);
    }
}