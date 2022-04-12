namespace Trade
{
    public static class MiddlewareConfiguration
    {
        public static void ConfigureMiddlewares(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            if (app.Environment.IsDevelopment())
                app.UseCors(ServicesConfiguration.CorsAllowAllInDevelopment);

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();
        }
    }
}
