using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.Modelos;
using SistemaInventario.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Inicializador
{
    public class DbInicializador : IDbInicializador
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly UserManager<IdentityRole> _rolManager;

       public DbInicializador(ApplicationDbContext db, UserManager<IdentityRole> rolManager, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
            _rolManager = rolManager;
        }

        public void Inicializador()
        {
            try
            {

                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                } 

            }catch (Exception ex)
            {
                throw;
            }

            if (_db.Roles.Any(r => r.Name == DS.Role_Admin)) return;
            
            _rolManager.CreateAsync(new IdentityRole (DS.Role_Admin)).GetAwaiter().GetResult();
            _rolManager.CreateAsync(new IdentityRole(DS.Role_Cliente)).GetAwaiter().GetResult();
            _rolManager.CreateAsync(new IdentityRole(DS.Role_Inventario)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new UsuarioAplicacion
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                Nombres = "Jason",
                Apellidos = "Contreras",


            }, "admintienda23").GetAwaiter().GetResult();

            UsuarioAplicacion usuario = _db.UsuarioAplicacion.Where(u => u.UserName == "admin@gmail.com").FirstOrDefault();
            _userManager.AddToRoleAsync(usuario, DS.Role_Admin).GetAwaiter().GetResult();
        }
    }
}
