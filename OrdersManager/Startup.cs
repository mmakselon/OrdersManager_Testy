﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(OrdersManager.Startup))]

namespace OrdersManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
