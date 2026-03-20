namespace Livora_Lite_API.Middleware
{
    /// <summary>
    /// Middleware para injetar o script JWT Helper no Swagger UI HTML
    /// </summary>
    public class SwaggerJwtHelperScriptMiddleware
    {
        private readonly RequestDelegate _next;

        public SwaggerJwtHelperScriptMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Apenas interceptar requisições GET para a página raiz do Swagger
            if (context.Request.Path.Equals("/swagger/index.html", StringComparison.OrdinalIgnoreCase) ||
                context.Request.Path.Equals("/swagger", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("[SwaggerJwtHelperScriptMiddleware] ✓ Interceptando página Swagger");
                
                try
                {
                    var originalBodyStream = context.Response.Body;
                    using (var memoryStream = new MemoryStream())
                    {
                        context.Response.Body = memoryStream;

                        // Chamar o próximo middleware
                        await _next(context);

                        // Ler a resposta HTML
                        memoryStream.Position = 0;
                        var html = await new StreamReader(memoryStream).ReadToEndAsync();
                        
                        // Injetar script antes de </body>
                        var scriptTag = "<script src=\"/swagger-jwt-helper.js\"></script>";
                        var modifiedHtml = html.Replace("</body>", $"{scriptTag}\\n</body>");
                        
                        // Escrever a resposta modificada
                        context.Response.Body = originalBodyStream;
                        var modifiedBytes = System.Text.Encoding.UTF8.GetBytes(modifiedHtml);
                        context.Response.ContentLength = modifiedBytes.Length;
                        await originalBodyStream.WriteAsync(modifiedBytes, 0, modifiedBytes.Length);
                        
                        Console.WriteLine("[SwaggerJwtHelperScriptMiddleware] ✓ Script JWT Helper injetado no Swagger UI");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[SwaggerJwtHelperScriptMiddleware] ✗ Erro: {ex.Message}");
                    await _next(context);
                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}
