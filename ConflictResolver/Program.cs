// MIT License
// 
// Copyright (c) 2024 John Russell C. Camo (Russkyc)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using ConflictResolver.Services.Repositories;
using Majorsoft.Blazor.Components.Common.JsInterop;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

namespace ConflictResolver;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        if (!builder.RootComponents.Any())
        {
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
        }

        ConfigureServices(builder.Services,builder.HostEnvironment.BaseAddress);

        await builder.Build().RunAsync();
    }
    
    private static void ConfigureServices(IServiceCollection services, string baseAddress)
    {
#if RELEASE
        services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://russkyc.github.io/resolv/") });
#else
        services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress) });
#endif
        
        services.AddMudServices();
        services.AddJsInteropExtensions();
        
        services.AddScoped<ScheduleRepository>();
    }
}