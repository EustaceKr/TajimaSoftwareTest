using Application.DTOs;
using Data.Context.Entities;

namespace Application.Mapping;

public static class DataMapping
{
    public static Design MapDesign(DesignDTO dto)
    {
        return new Design
        {
            Id = dto.Id,
            DecorationMethod = dto.DecorationMethod,
            Name = dto.Name,
            Width = dto.Width,
            Height = dto.Height,
            CreatedDate = null,
            UpdatedDate = null
        };
    }
    public static Template MapTemplate(TemplateDTO dto)
    {
        return new Template { 
            Id = dto.Id,
            DecorationMethod = dto.DecorationMethod,
            Name = dto.Name,
            TemplateDesigns = dto.TemplateDesigns,
            CreatedDate = null,
            UpdatedDate = null
        };
    }
    public static DesignDTO MapDesignDTO(Design design)
    {
        return new DesignDTO
        {
            Id = design.Id,
            DecorationMethod = design.DecorationMethod,
            Name = design.Name,
            Width = design.Width,
            Height = design.Height
        };
    }
    public static TemplateDTO MapTemplateDTO(Template template)
    {
        return new TemplateDTO
        {
            Id = template.Id,
            DecorationMethod = template.DecorationMethod,
            Name = template.Name,
            TemplateDesigns = template.TemplateDesigns
        };
    }
}
