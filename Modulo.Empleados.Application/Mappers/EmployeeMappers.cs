using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Modulo.Empleados.Application.Contracts;
using Modulo.Empleados.Domain.Entities;

namespace Modulo.Empleados.Application.Mappers;

public static class EmployeeMappers
{
    public static EmployeeListItemDto ToListItemDto(this Empleado e) => new()
    {
        Id = e.Id,
        Nombre = e.Nombre,
        Correo = e.Correo,
        Telefono = e.Telefono,
        FechaIngreso = e.FechaIngreso,
        Area = e.Area,
        Puesto = e.Puesto,
        Turno = e.Turno,
        Estatus = e.Estatus
    };

    public static EmployeeDetailDto ToDetailDto(this Empleado e) => new()
    {
        Id = e.Id,
        Nombre = e.Nombre,
        Correo = e.Correo,
        Telefono = e.Telefono,
        FechaIngreso = e.FechaIngreso,
        Area = e.Area,
        Puesto = e.Puesto,
        Turno = e.Turno,
        Estatus = e.Estatus,
        Domicilio = e.Domicilio is null ? null : new MeProfileDto.DomicilioDto
        {
            Calle = e.Domicilio.Calle,
            Numero = e.Domicilio.Numero,
            Colonia = e.Domicilio.Colonia,
            Ciudad = e.Domicilio.Ciudad,
            Estado = e.Domicilio.Estado,
            CodigoPostal = e.Domicilio.CodigoPostal
        },
        ContactoEmergencia = e.Contacto is null ? null : new MeProfileDto.ContactoDto
        {
            Nombre = e.Contacto.Nombre,
            Parentesco = e.Contacto.Parentesco,
            Telefono = e.Contacto.Telefono
        }
    };

    public static MeProfileDto ToMeDto(this (Domicilio? domicilio, ContactoEmergencia? contacto) me) => new()
    {
        Domicilio = me.domicilio is null ? null : new MeProfileDto.DomicilioDto
        {
            Calle = me.domicilio.Calle,
            Numero = me.domicilio.Numero,
            Colonia = me.domicilio.Colonia,
            Ciudad = me.domicilio.Ciudad,
            Estado = me.domicilio.Estado,
            CodigoPostal = me.domicilio.CodigoPostal
        },
        ContactoEmergencia = me.contacto is null ? null : new MeProfileDto.ContactoDto
        {
            Nombre = me.contacto.Nombre,
            Parentesco = me.contacto.Parentesco,
            Telefono = me.contacto.Telefono
        }
    };
}
