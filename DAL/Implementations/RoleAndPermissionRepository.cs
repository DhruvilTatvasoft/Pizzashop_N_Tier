using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace DAL.Implementations
{
    public class RoleAndPermissionRepository : IRoleAndPermissionRepository
    {
        private readonly PizzashopCContext _context;

        

        public RoleAndPermissionRepository(PizzashopCContext context)
        {
            _context = context;
        }

        public void AddPermission(Rolesandpermission permission)
{
    try
    {
        var exists = _context.Rolesandpermissions
            .AsNoTracking()
            .Any(p => p.Permissionid == permission.Permissionid && p.Roleid == permission.Roleid);

        if (!exists)
        {
            _context.Rolesandpermissions.Add(permission);
            _context.SaveChanges();
        }
        else
        {
            Console.WriteLine($"Permission already exists for RoleId {permission.Roleid} and PermissionId {permission.Permissionid}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error adding permission: {ex.Message}");
        throw;
    }
}

        public void UpdatePermission(Rolesandpermission permission)
        {
            _context.Rolesandpermissions.Update(permission);
            _context.SaveChanges();
        }

        public void RemovePermission(int permissionId, int roleId)
        {
            var permission = _context.Rolesandpermissions.FirstOrDefault(p => p.Permissionid == permissionId && p.Roleid == roleId);
            if (permission != null)
            {
                _context.Rolesandpermissions.Remove(permission);
                _context.SaveChanges();
            }
        }

        public Rolesandpermission GetPermission(int permissionId, int roleId)
        {
            return _context.Rolesandpermissions.FirstOrDefault(p => p.Permissionid == permissionId && p.Roleid == roleId);
        }

        public List<Rolesandpermission> GetUserPermissions(string role)
        {
            var permissions = _context.Rolesandpermissions
                .Include(rp => rp.Permission)
                .Include(rp => rp.Role)
                .Where(rp => rp.Role.Rolename == role)
                .ToList();
                return permissions;
        }

        public string getPermissionName(int permissionid)
        {
            return _context.Permissions.FirstOrDefault(p=>p.Permissionid == permissionid)?.Permissionname ?? string.Empty;
        }

        public List<Permission> getAllPermissions()
        {
            return _context.Permissions.ToList();
        }

        public Role GetRoleByRoleName(string roleName)
        {
            return _context.Roles.FirstOrDefault(role=>role.Rolename == roleName)!;
        }

        public List<Rolesandpermission> GetRolePermissions(int roleid)
        {
            return _context.Rolesandpermissions.Where(RoleAndPermission=>RoleAndPermission.Roleid == roleid).Include(role=>role.Permission).ToList();
        }
    }
}