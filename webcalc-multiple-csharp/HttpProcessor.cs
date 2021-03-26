﻿﻿using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace webcalc_multiple_csharp
{
    public class HttpProcessor
    {
        private readonly TcpClient _socket;
        private readonly HttpServer _server;

        private Stream _inputStream;
        public StreamWriter OutputStream;

        public string HttpUrl;



        public HttpProcessor(TcpClient s, HttpServer server) {
            _socket = s;
            _server = server;                   
        }
        
        public void Process() {                        
            _inputStream = new BufferedStream(_socket.GetStream());
            OutputStream = new StreamWriter(new BufferedStream(_socket.GetStream()));
            try {
                ParseRequest();
                
                WriteSuccess();
                _server.HandleGetRequest(this);
            } catch (Exception e) {
                Console.WriteLine("Exception: " + e);
                WriteFailure();
            }
            OutputStream.Flush();
            _inputStream = null;
            OutputStream = null;         
            _socket.Close();             
        }

        private void ParseRequest() {
            string data = "";
            while (true) {
                var nextChar = _inputStream.ReadByte();
                if (nextChar == '\n') { break; }
                if (nextChar == '\r') { continue; }
                if (nextChar == -1) { Thread.Sleep(1); continue; };
                data += Convert.ToChar(nextChar);
            }
            string[] tokens = data.Split(' ');
            if (tokens.Length != 3)
                throw new Exception("invalid http request line");
            HttpUrl = tokens[1];
        }

        private void WriteSuccess() {
            OutputStream.WriteLine("HTTP/1.1 200 OK");    
            OutputStream.WriteLine("Access-Control-Allow-Origin: *");   
            OutputStream.WriteLine("Content-Type: application/json;charset=utf-8");
            OutputStream.WriteLine("");
        }

        private void WriteFailure() {
            OutputStream.WriteLine("HTTP/1.0 404 File not found");
            OutputStream.WriteLine("Connection: close");
            OutputStream.WriteLine("");
        }
    }
}