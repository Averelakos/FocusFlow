public static class WebApplicationBuilderExtension
{
    public static void ConfigureApplication(this IApplicationBuilder app, IWebHostEnvironment hostEnvironment)
    {
        if (hostEnvironment.IsDevelopment())
        {
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/openapi/v1.json", "FocusFlow API v1");
                options.RoutePrefix = string.Empty; // Serve at root
            });
            // app.UseSwagger();
            // app.UseSwaggerUI();
            // app.MapOpenApi();
        }
    }
}