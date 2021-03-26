﻿﻿using System;

namespace webcalc_multiple_csharp
{
    public class MyHttpServer : HttpServer {
        
        public MyHttpServer(int port) : base(port) { }
        
        public override void HandleGetRequest(HttpProcessor p) {
            var i1 = p.HttpUrl;
            if (i1.IndexOf("?x=", StringComparison.Ordinal) == -1) return;
            var startIndex = i1.IndexOf("x=", StringComparison.Ordinal) + 2;
            var endIndex = i1.IndexOf("&", StringComparison.Ordinal);
            var x = int.Parse(i1.Substring(startIndex, endIndex - startIndex));
            var y = int.Parse(i1.Substring(i1.IndexOf("y=", StringComparison.Ordinal) + 2 ));
            var answer = x * y;
            var result = "{\"error\": false, \"string\":\"" + x + "*" + y + "=" + answer + "\",\"answer\":" + answer + "}";
            p.OutputStream.WriteLine(result);
        }
    }
}