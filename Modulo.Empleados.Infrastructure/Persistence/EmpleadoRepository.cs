using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using Microsoft.Data.SqlClient;
using Modulo.Empleados.Domain.Entities;
using Modulo.Empleados.Domain.Interfaces;

namespace Modulo.Empleados.Infrastructure.Persistence;

public sealed class EmpleadoRepository : IEmpleadoRepository
{
    private readonly SqlOptions _options;
    public EmpleadoRepository(SqlOptions options) => _options = options;

    private SqlConnection NewConn() => new(_options.ConnectionString);

    public async Task<IReadOnlyList<Empleado>> GetAllAsync(CancellationToken ct)
    {
        using var conn = NewConn();
        using var cmd = new SqlCommand("empleados.sp_empleados_listar", conn)
        { CommandType = CommandType.StoredProcedure };

        await conn.OpenAsync(ct);
        using var rd = await cmd.ExecuteReaderAsync(ct);

        var list = new List<Empleado>();
        while (await rd.ReadAsync(ct))
        {
            list.Add(new Empleado
            {
                Id = rd.GetInt32(rd.GetOrdinal("id")),
                Nombre = rd.GetString(rd.GetOrdinal("nombre")),
                Correo = rd.GetString(rd.GetOrdinal("correo")),
                Telefono = rd.GetString(rd.GetOrdinal("telefono")),
                FechaIngreso = rd.GetString(rd.GetOrdinal("fechaIngreso")),
                Area = rd.GetString(rd.GetOrdinal("area")),
                Puesto = rd.GetString(rd.GetOrdinal("puesto")),
                Turno = rd.GetString(rd.GetOrdinal("turno")),
                Estatus = rd.GetString(rd.GetOrdinal("estatus"))
            });
        }
        return list;
    }

    public async Task<Empleado?> GetByIdAsync(int id, CancellationToken ct)
    {
        using var conn = NewConn();
        using var cmd = new SqlCommand("empleados.sp_empleados_detalle", conn)
        { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.AddWithValue("@id", id);

        await conn.OpenAsync(ct);
        using var rd = await cmd.ExecuteReaderAsync(ct);

        if (!await rd.ReadAsync(ct)) return null;

        var emp = new Empleado
        {
            Id = rd.GetInt32(rd.GetOrdinal("id")),
            Nombre = rd.GetString(rd.GetOrdinal("nombre")),
            Correo = rd.GetString(rd.GetOrdinal("correo")),
            Telefono = rd.GetString(rd.GetOrdinal("telefono")),
            FechaIngreso = rd.GetString(rd.GetOrdinal("fechaIngreso")),
            Area = rd.GetString(rd.GetOrdinal("area")),
            Puesto = rd.GetString(rd.GetOrdinal("puesto")),
            Turno = rd.GetString(rd.GetOrdinal("turno")),
            Estatus = rd.GetString(rd.GetOrdinal("estatus")),
            Domicilio = rd.IsDBNull(rd.GetOrdinal("calle")) ? null : new Domicilio
            {
                Calle = rd.GetString(rd.GetOrdinal("calle")),
                Numero = rd.GetString(rd.GetOrdinal("numero")),
                Colonia = rd.GetString(rd.GetOrdinal("colonia")),
                Ciudad = rd.GetString(rd.GetOrdinal("ciudad")),
                Estado = rd.GetString(rd.GetOrdinal("estado")),
                CodigoPostal = rd.GetString(rd.GetOrdinal("codigoPostal"))
            },
            Contacto = rd.IsDBNull(rd.GetOrdinal("contactoNombre")) ? null : new ContactoEmergencia
            {
                Nombre = rd.GetString(rd.GetOrdinal("contactoNombre")),
                Parentesco = rd.GetString(rd.GetOrdinal("parentesco")),
                Telefono = rd.GetString(rd.GetOrdinal("contactoTelefono"))
            }
        };
        return emp;
    }

    public async Task<(Domicilio? domicilio, ContactoEmergencia? contacto)?> GetMeAsync(long usuarioId, CancellationToken ct)
    {
        using var conn = NewConn();
        using var cmd = new SqlCommand("empleados.sp_empleado_me", conn)
        { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.AddWithValue("@usuario_id", usuarioId);

        await conn.OpenAsync(ct);
        using var rd = await cmd.ExecuteReaderAsync(ct);
        if (!await rd.ReadAsync(ct)) return null;

        var domicilio = rd.IsDBNull(rd.GetOrdinal("calle")) ? null : new Domicilio
        {
            Calle = rd.GetString(rd.GetOrdinal("calle")),
            Numero = rd.GetString(rd.GetOrdinal("numero")),
            Colonia = rd.GetString(rd.GetOrdinal("colonia")),
            Ciudad = rd.GetString(rd.GetOrdinal("ciudad")),
            Estado = rd.GetString(rd.GetOrdinal("estado")),
            CodigoPostal = rd.GetString(rd.GetOrdinal("codigoPostal"))
        };

        var contacto = rd.IsDBNull(rd.GetOrdinal("contactoNombre")) ? null : new ContactoEmergencia
        {
            Nombre = rd.GetString(rd.GetOrdinal("contactoNombre")),
            Parentesco = rd.GetString(rd.GetOrdinal("parentesco")),
            Telefono = rd.GetString(rd.GetOrdinal("contactoTelefono"))
        };

        return (domicilio, contacto);
    }

    public async Task<(Domicilio? domicilio, ContactoEmergencia? contacto)?> UpdateMeAsync(
        long usuarioId,
        Domicilio domicilio,
        ContactoEmergencia contacto,
        CancellationToken ct)
    {
        using var conn = NewConn();
        using var cmd = new SqlCommand("empleados.sp_empleado_me_update", conn)
        { CommandType = CommandType.StoredProcedure };

        cmd.Parameters.AddWithValue("@usuario_id", usuarioId);
        cmd.Parameters.AddWithValue("@calle", domicilio.Calle);
        cmd.Parameters.AddWithValue("@numero", domicilio.Numero);
        cmd.Parameters.AddWithValue("@colonia", domicilio.Colonia);
        cmd.Parameters.AddWithValue("@ciudad", domicilio.Ciudad);
        cmd.Parameters.AddWithValue("@estado", domicilio.Estado);
        cmd.Parameters.AddWithValue("@codigoPostal", domicilio.CodigoPostal);
        cmd.Parameters.AddWithValue("@contactoNombre", contacto.Nombre);
        cmd.Parameters.AddWithValue("@parentesco", contacto.Parentesco);
        cmd.Parameters.AddWithValue("@contactoTel", contacto.Telefono);

        await conn.OpenAsync(ct);
        using var rd = await cmd.ExecuteReaderAsync(ct);
        if (!await rd.ReadAsync(ct)) return null;

        var dom = rd.IsDBNull(rd.GetOrdinal("calle")) ? null : new Domicilio
        {
            Calle = rd.GetString(rd.GetOrdinal("calle")),
            Numero = rd.GetString(rd.GetOrdinal("numero")),
            Colonia = rd.GetString(rd.GetOrdinal("colonia")),
            Ciudad = rd.GetString(rd.GetOrdinal("ciudad")),
            Estado = rd.GetString(rd.GetOrdinal("estado")),
            CodigoPostal = rd.GetString(rd.GetOrdinal("codigoPostal"))
        };

        var con = rd.IsDBNull(rd.GetOrdinal("contactoNombre")) ? null : new ContactoEmergencia
        {
            Nombre = rd.GetString(rd.GetOrdinal("contactoNombre")),
            Parentesco = rd.GetString(rd.GetOrdinal("parentesco")),
            Telefono = rd.GetString(rd.GetOrdinal("contactoTelefono"))
        };

        return (dom, con);
    }
}
