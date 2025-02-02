using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NetTopologySuite.Geometries;
using ToDoAPI.DTOs;
using ToDoAPI.Entidades;

namespace ToDoAPI.Utilidades
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory)
        {
            ConfigurarMapeoTareas();
            ConfigurarMapeoUsuarios();
        }

        private void ConfigurarMapeoTareas()
        {
            CreateMap<TareaCreacionDTO, Tarea>();
            CreateMap<Tarea, TareaDTO>();
        }

        private void ConfigurarMapeoUsuarios()
        {
            CreateMap<IdentityUser, UsuarioDTO>();
        }

    }
}
